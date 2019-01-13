//=============================================================================
// System  : ASP.NET Web Control Library
// File    : MaskTextBox.cs
// Author  : Eric Woodruff  (Eric@EWoodruff.us)
// Updated : Fri 02/20/04
// Note    : Copyright 2002-2004, Eric Woodruff, All rights reserved
// Compiler: Microsoft Visual C#
//
// This file contains a derived CompareTextBox class that can automatically
// generate a regular expression validator for itself that checks to see if
// the data entered matches a user-defined mask pattern.
//
// Version     Date     Who  Comments
// ============================================================================
// 1.0.0    12/14/2003  EFW  Created the code
//=============================================================================

using System;
using System.ComponentModel;
using System.Security.Permissions;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EWSoftware.Web.Controls
{
    /// <summary>
	/// This derived CompareTextBox class can automatically generate a
	/// RegularExpressionValidator for itself to insure that the text
	/// entered matches a user-defined mask pattern.  The mask consists of
	/// literal characters and special mask characters that allow or disallow
    /// specific character types at the position at which they occur.
	/// </summary>
    /// <include file='Doc/Controls.xml'
    /// path='Controls/MaskTextBox/Member[@name="Class"]/*' />
	[DefaultProperty("Mask"),
     ToolboxData("<{0}:MaskTextBox runat=\"server\" Mask=\"CCCC\" " +
        "MaskErrorMessage=\"Not valid for specified mask\" />"),
     AspNetHostingPermission(SecurityAction.LinkDemand,
        Level=AspNetHostingPermissionLevel.Minimal),
     AspNetHostingPermission(SecurityAction.InheritanceDemand,
        Level=AspNetHostingPermissionLevel.Minimal)]
	public class MaskTextBox : EWSoftware.Web.Controls.CompareTextBox
	{
        //=====================================================================
        // Private class members
        private RegularExpressionValidator rePattern;   // The validator

        //=====================================================================
        // Properties

	    /// <summary>
        /// This is the error message to display if the data entered does
        /// not match the specified mask.
	    /// </summary>
		[Category("Appearance"), Bindable(true),
            DefaultValue("Not valid for specified mask"),
            Description("The error message to display for invalid data")]
		public string MaskErrorMessage
		{
			get
            {
                EnsureChildControls();
                return rePattern.ErrorMessage;
            }
			set
            {
                EnsureChildControls();
                rePattern.ErrorMessage = value;
            }
		}

	    /// <summary>
        /// This is used to set or get the mask to use for data entry.
	    /// </summary>
        /// <include file='Doc/Controls.xml'
        /// path='Controls/MaskTextBox/Member[@name="Mask"]/*' />
		[Category("Behavior"), DefaultValue("CCCC"), Bindable(true),
         Description("The mask to use for data entry")]
        public string Mask
        {
			get
            {
	            Object oMask = ViewState["Mask"];
	            return (oMask == null) ? "CCCC" : (string)oMask;
            }
            set
            {
                EnsureChildControls();
                rePattern.ValidationExpression =
                    MaskTextBox.BuildRegExpFromMask(value);
                ViewState["Mask"] = value;
            }
        }

	    /// <summary>
        /// This read-only property returns the actual regular expression
        /// used by the validator.
	    /// </summary>
		[Category("Behavior"), Browsable(false),
            Description("The underlying regular expression")]
        public string MaskRegExp
        {
            get
            {
                EnsureChildControls();
                return rePattern.ValidationExpression;
            }
        }

        //=====================================================================
        // Methods, etc

        /// <summary>
        /// This is overridden to create the
        /// <see cref="System.Web.UI.WebControls.RegularExpressionValidator"/>
        /// used to validate the input.
        /// </summary>
        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            rePattern = new RegularExpressionValidator();
            rePattern.Enabled = this.Enabled;
            rePattern.ValidationExpression = "....";
            rePattern.ErrorMessage = "Not valid for specified mask";

            // Use validation summary by default
            rePattern.Display = ValidatorDisplay.None;

            this.Controls.Add(rePattern);
        }

        /// <summary>
        /// OnPreRender is overridden to set the control to validate and the
        /// validation control IDs.  These can't be set earlier as we can't
        /// guarantee the order in which the properties are set.  The names of
        /// the validator controls are based on the control to validate.
        /// </summary>
        /// <param name="e">The event arguments</param>
        protected override void OnPreRender(System.EventArgs e)
        {
            base.OnPreRender(e);
            rePattern.ControlToValidate = this.ID;
            rePattern.ID = this.ID + "_MREV";

            // Register formatting script?
            if(!this.AutoPostBack && this.EnableClientScript &&
              this.CanRenderUpLevel)
            {
                this.Attributes["mask"] = this.Mask;
                this.Attributes["onblur"] = "javascript:MTB_ApplyMask(this, event);";
                this.Attributes["onkeydown"] = "javascript:return MTB_ApplyMask(this, event);";
                this.Attributes["onkeyup"] = "javascript:return MTB_ApplyMask(this, event);";

                if(!this.Page.ClientScript.IsClientScriptBlockRegistered("EWS_MTBFormat"))
                    this.Page.ClientScript.RegisterClientScriptInclude(
                        typeof(MaskTextBox), "EWS_MTBFormat",
                        this.Page.ClientScript.GetWebResourceUrl(
                        typeof(MaskTextBox),
                        CtrlUtils.ScriptsPath + "MaskTextBox.js"));
            }
        }

		/// <summary>
		/// Render this control to the output parameter specified.
		/// </summary>
		/// <param name="writer">The HTML writer to which the output is written</param>
		protected override void Render(HtmlTextWriter writer)
		{
			base.Render(writer);

            if(rePattern != null)
                rePattern.RenderControl(writer);
		}

        /// <summary>
        /// This method is used to build a regular expression based on the
        /// user-defined mask.  It interprets each character of the mask
        /// and appends the appropriate regular expression data for it.
        /// </summary>
        /// <param name="strMask">The mask to use when building the regular
        /// expression.</param>
        private static string BuildRegExpFromMask(string strMask)
        {
            StringBuilder strRegExp = new StringBuilder();

            char[] achMaskChars = strMask.ToCharArray();
            int nIdx, nMinReqIdx = -1, nLen = achMaskChars.Length;

            // The first step is to find the last required character or
            // literal character.  All characters before that are also
            // required regardless of whether or not their behavior
            // allows them to be optional.
            for(nIdx = 9; nIdx < nLen && nMinReqIdx == -1; nIdx++)
                switch(achMaskChars[nIdx])
                {
                    case '0':
                    case 'L':
                    case 'A':
                    case 'a':
                    case '&':
                    case '\\':
                        nMinReqIdx = nLen;
                        break;

                    default:
                        break;
                }

            // Build the regular expression
            for(nIdx = 0; nIdx < nLen; nIdx++)
            {
                switch(achMaskChars[nIdx])
                {
                    case '\\':      // Escaped literal
                        nIdx++;
                        strRegExp.Append('\\'); // Escape it here too for good measure
                        strRegExp.Append(achMaskChars[nIdx]);
                        break;

                    case '0':       // Digit (0 to 9, entry required, +/- not allowed)
                        strRegExp.Append(@"\d");
                        break;

                    case '9':       // Digit or space (entry optional, +/- not allowed)
                        strRegExp.Append("[0-9 ]");

                        if(nIdx > nMinReqIdx)
                            strRegExp.Append('?');  // Can be optional
                        break;

                    case '#':       // Digit or space (entry optional; +/- signs allowed)
                        strRegExp.Append(@"[0-9 +\-]");

                        if(nIdx > nMinReqIdx)
                            strRegExp.Append('?');  // Can be optional
                        break;

                    case 'L':       // Letter (A to Z, entry required)
                        strRegExp.Append("[A-Za-z]");
                        break;

                    case '?':       // Letter (A to Z, entry optional)
                        strRegExp.Append("[A-Za-z]");

                        if(nIdx > nMinReqIdx)
                            strRegExp.Append('?');  // Can be optional
                        break;

                    case 'A':       // Letter or digit (entry required)
                        strRegExp.Append("[A-Za-z0-9]");
                        break;

                    case 'a':       // Letter or digit (entry optional)
                        strRegExp.Append("[A-Za-z0-9]");

                        if(nIdx > nMinReqIdx)
                            strRegExp.Append('?');  // Can be optional
                        break;

                    case '&':       // Any character (entry required)
                        strRegExp.Append('.');
                        break;

                    case 'C':       // Any character (entry optional)
                        strRegExp.Append('.');

                        if(nIdx > nMinReqIdx)
                            strRegExp.Append('?');  // Can be optional
                        break;

                    default:        // Literal
                        strRegExp.Append('\\'); // Escape it here too for good measure
                        strRegExp.Append(achMaskChars[nIdx]);
                        break;
                }
            }

            return strRegExp.ToString();
        }
	}
}
