//=============================================================================
// System  : ASP.NET Web Control Library
// File    : EWSTextBox.cs
// Author  : Eric Woodruff  (Eric@EWoodruff.us)
// Updated : Fri 02/20/04
// Note    : Copyright 2002-2004, Eric Woodruff, All rights reserved
// Compiler: Microsoft Visual C#
//
// This file contains a derived TextBox class that can optionally convert its
// text to uppercase, lowercase, or proper case and trim off leading and
// trailing blanks.  It also contains a length validator for multi-line
// textareas and a required field validator for use if the Required property
// is set to True.  Default behavior is no case conversion but it does trim
// blanks.  It will only generate the textarea length validator if the
// MaxLength property is set and TextMode is set to MultiLine.  The Required
// property is set to False by default.
//
// Version     Date     Who  Comments
// ============================================================================
// 1.0.0    09/12/2002  EFW  Created the code
//=============================================================================

using System;
using System.ComponentModel;
using System.Security.Permissions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EWSoftware.Web.Controls
{
    /// <summary>
    /// This enumerated type defines the different casing modes supported by
    /// the <see cref="EWSTextBox"/> control.
    /// </summary>
    /// <seealso cref="EWSTextBox.Casing"/>
    public enum CasingMode
    {
        /// <summary>No conversion</summary>
        None = 0,
        /// <summary>Converts text to upper case</summary>
        Upper,
        /// <summary>Converts text to lower case</summary>
        Lower,
        /// <summary>Converts text to proper (name) case</summary>
        Proper
    }

	/// <summary>
	/// This derived TextBox class can optionally convert its text to
    /// uppercase, lowercase, or proper case and trim off leading and
    /// trailing blanks.  It also contains a length validator for multi-line
    /// textareas and a required field validator for use if the Required
    /// property is set to True.  Default behavior is no case conversion but
    /// it does trim blanks.  It will only generate the textarea length
    /// validator if the MaxLength property is set and TextMode is set to
    /// MultiLine.  The Required property is set to False by default.
	/// </summary>
	[DefaultProperty("Text"),
     ToolboxData("<{0}:EWSTextBox runat=\"server\" />"),
     AspNetHostingPermission(SecurityAction.LinkDemand,
        Level=AspNetHostingPermissionLevel.Minimal),
     AspNetHostingPermission(SecurityAction.InheritanceDemand,
        Level=AspNetHostingPermissionLevel.Minimal)]
	public class EWSTextBox : System.Web.UI.WebControls.TextBox
	{
        //=====================================================================
        // Private class members
        private RequiredFieldValidator     rfVal;   // The validators
        private RegularExpressionValidator reLen;

        //=====================================================================
        // Properties

        /// <summary>
        /// This is overridden to ensure that the child controls are always
        /// created when needed.
        /// </summary>
        [Browsable(false)]
        public override ControlCollection Controls
        {
            get
            {
                EnsureChildControls();
                return base.Controls;
            }
        }

	    /// <summary>
        /// The Casing property determines whether or not the text is converted
        /// to upper, lower, or proper case when changed (default = None).
	    /// </summary>
		[Category("Behavior"), DefaultValue(CasingMode.None), Bindable(true),
         Description("Convert text to upper, lower, or proper case if set")]
		public CasingMode Casing
		{
			get
            {
	            Object oCase = ViewState["Casing"];
	            return (oCase == null) ? CasingMode.None : (CasingMode)oCase;
            }
			set { ViewState["Casing"] = value; }
		}

	    /// <summary>
        /// The Trim property determines whether or not the text is trimmed
        /// of leading and trailing whitespace when changed (default = true).
	    /// </summary>
		[Category("Behavior"), DefaultValue(true), Bindable(true),
         Description("Trim leading and trailing whitespace")]
		public bool Trim
		{
			get
            {
	            Object oTrim = ViewState["Trim"];
	            return (oTrim == null) ? true : (bool)oTrim;
            }
			set { ViewState["Trim"] = value; }
		}

	    /// <summary>
        /// The Required property determines whether or not the
        /// RequiredFieldValidator is generated (default = false).
	    /// </summary>
		[Category("Behavior"), DefaultValue(false), Bindable(true),
         Description("Determines whether or not the control requires data to be entered")]
		public bool Required
		{
			get
            {
	            Object oRequired = ViewState["Required"];
	            return (oRequired == null) ? false : (bool)oRequired;
            }
			set { ViewState["Required"] = value; }
		}

	    /// <summary>
        /// This is the error message to display if a textarea exceeds
        /// its maximum length.  The string can contain the place holder
        /// {MaxLen} so that the current maximum length is displayed.
	    /// </summary>
		[Category("Appearance"), Bindable(true),
         DefaultValue("Too many characters"),
         Description("The error message to display if a textarea exceeds its maximum length")]
		public string MaxLenErrorMessage
		{
			get
            {
	            Object oMsg = ViewState["MaxLenErrMsg"];
	            return (oMsg == null) ? "Too many characters" : (string)oMsg;
            }
			set
            {
                EnsureChildControls();
                ViewState["MaxLenErrMsg"] = value;

                // Clear the message in the validator.  It'll get formatted
                // when rendering the control.
                reLen.ErrorMessage = String.Empty;
            }
		}

	    /// <summary>
        /// This is the error message to display if required and left blank
	    /// </summary>
		[Category("Appearance"), DefaultValue("Enter a value"), Bindable(true),
         Description("The error message to display if required and left blank")]
		public string RequiredMessage
		{
			get
            {
                EnsureChildControls();
                return rfVal.ErrorMessage;
            }
			set
            {
                EnsureChildControls();
                rfVal.ErrorMessage = value;
            }
		}

	    /// <summary>
        /// This property can be used to set additional words that should be
        /// converted to upper case when the Casing mode is set to Proper.
        /// Specify a list of words separaterd by the pipe (|) character.
	    /// </summary>
		[Category("Behavior"), Bindable(true),
         Description("Additional words to convert to upper case when using Casing.Proper")]
		public string UCWords
		{
			get
            {
	            Object oUC = ViewState["UCWords"];
	            return (oUC == null) ? null : (string)oUC;
            }
			set { ViewState["UCWords"] = value; }
		}

	    /// <summary>
        /// The Display property determines whether or not the validators
        /// are displayed by themselves or if they will make use of a summary
        /// control.
	    /// </summary>
		[Category("Appearance"), DefaultValue(ValidatorDisplay.None), Bindable(true),
         Description("The validator display mode")]
		public ValidatorDisplay Display
		{
			get
            {
                // The display setting is the same for all validators
                EnsureChildControls();
                return rfVal.Display;
            }
			set
            {
                BaseValidator bv;

                EnsureChildControls();

                foreach(WebControl ctl in Controls)
                {
                    bv = ctl as BaseValidator;

                    if(bv != null)
                        bv.Display = value;
                }
            }
		}

	    /// <summary>
        /// This can be used to specify whether or not client-side script is enabled
	    /// </summary>
		[Category("Behavior"), DefaultValue(true), Bindable(true),
         Description("Determines whether or not client-side script is enabled")]
		public bool EnableClientScript
		{
			get
            {
                // The script setting is the same for all validators
                EnsureChildControls();
                return rfVal.EnableClientScript;
            }
			set
            {
                BaseValidator bv;

                EnsureChildControls();

                foreach(WebControl ctl in Controls)
                {
                    bv = ctl as BaseValidator;

                    if(bv != null)
                        bv.EnableClientScript = value;
                }
            }
		}

	    /// <summary>
        /// This returns true if the control is being rendered in an
        /// up-level browser (IE 4+, NS 6+, or ECMAScript 1.2 supported)
	    /// </summary>
		[Category("Behavior"), Browsable(false),
         Description("Indicates whether or not the control is being rendered in an up-level browser")]
        public virtual bool CanRenderUpLevel
        {
            get
            {
                // This prevents an exception being reported in design view
                if(this.Context == null)
                    return true;

                HttpBrowserCapabilities bc = this.Context.Request.Browser;

                if(bc.MSDomVersion.Major >= 4 ||
                  (bc.Browser == "Netscape" && bc.MajorVersion > 5) ||
		          (bc.EcmaScriptVersion.CompareTo(new Version(1, 2)) < 0) == false)
                    return true;

                return false;
            }
        }

	    /// <summary>
        /// Enabled is overridden to ensure that the validators are also
        /// enabled or disabled along with the control so that they only
        /// fire when the control is enabled.
	    /// </summary>
		[Category("Behavior"), DefaultValue(true), Bindable(true)]
        public override bool Enabled
        {
            get { return base.Enabled; }
            set
            {
                base.Enabled = value;

                EnsureChildControls();
                foreach(WebControl ctl in Controls)
                    if(ctl is System.Web.UI.WebControls.BaseValidator)
                        ctl.Enabled = value;
            }
        }

	    /// <summary>
        /// Visible is overridden to ensure that the validators are also
        /// made visible or invisible based on the state of the associated
        /// control so that they only fire when the control is visible.
	    /// </summary>
		[Category("Behavior"), DefaultValue(true), Bindable(true)]
        public override bool Visible
        {
            get { return base.Visible; }
            set
            {
                base.Visible = value;

                EnsureChildControls();
                foreach(WebControl ctl in Controls)
                    if(ctl is System.Web.UI.WebControls.BaseValidator)
                        ctl.Visible = value;
            }
        }

	    /// <summary>
        /// MaxLength is overridden to ensure that the length validator is
        /// formatted with the appropriate regular expression.
	    /// </summary>
		[Category("Behavior"), Bindable(true)]
        public override int MaxLength
        {
            get { return base.MaxLength; }
            set
            {
                base.MaxLength = value;

                // Format the length validation expression.  Must match any
                // character, CR, or LF to make it work correctly.  The error
                // message is cleared to force reformatting when rendered.
                EnsureChildControls();
                reLen.ErrorMessage = String.Empty;
                reLen.ValidationExpression =
                    String.Format(@"(.|\r|\n){{0,{0}}}", value);
            }
        }

	    /// <summary>
        /// Text is overridden to ensure that the Casing and Trim properties
        /// are applied when a value is assigned to the control.
	    /// </summary>
		[Category("Appearance"), Bindable(true)]
        public override string Text
        {
            get { return base.Text; }
            set
            {
                string strVal = value;

                if(Trim == true)
                    strVal = strVal.Trim();

                switch(Casing)
                {
                    case CasingMode.Upper:
                        strVal = strVal.ToUpper();
                        break;

                    case CasingMode.Lower:
                        strVal = strVal.ToLower();
                        break;

                    case CasingMode.Proper:
                        strVal = CtrlUtils.ToProperCase(strVal, UCWords);
                        break;

                    default:
                        break;
                }

                base.Text = strVal;
            }
        }

        //=====================================================================
        // Methods, etc

        /// <summary>
        /// CreateChildControls() is overridden to create the validators
        /// </summary>
        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            rfVal = new RequiredFieldValidator();
            reLen = new RegularExpressionValidator();
            rfVal.Enabled = reLen.Enabled = this.Enabled;

            // Use validation summary by default
            rfVal.Display = reLen.Display = ValidatorDisplay.None;
            rfVal.ErrorMessage = "Enter a value";

            this.Controls.Add(rfVal);
            this.Controls.Add(reLen);
        }

        /// <summary>
        /// OnPreRender is overridden to set the control to validate and the
        /// validation control IDs.  These can't be set earlier as we can't
        /// guarantee the order in which the properties are set.  The names of
        /// the validator controls are based on the control to validate.
        /// It also sets the validators to invisible if they aren't being used.
        /// </summary>
        /// <param name="e">The event arguments</param>
        protected override void OnPreRender(System.EventArgs e)
        {
            if(this.Visible)
            {
                if(this.Required == true)
                {
                    rfVal.Visible = true;
                    rfVal.ControlToValidate = this.ID;
                    rfVal.ID = this.ID + "_RFV";
                }
                else
                    rfVal.Visible = false;

                if(this.TextMode == TextBoxMode.MultiLine && this.MaxLength > 0)
                {
                    reLen.Visible = true;
                    reLen.ControlToValidate = this.ID;
                    reLen.ID = this.ID + "_RELV";

                    // Format the maximum length error message if necessary
                    if(reLen.ErrorMessage.Length == 0)
                        reLen.ErrorMessage = CtrlUtils.ReplaceParameters(
                            MaxLenErrorMessage, "{MaxLen}", MaxLength.ToString());
                }
                else
                    reLen.Visible = false;
            }

            base.OnPreRender(e);
        }

		/// <summary>
		/// Render this control to the output parameter specified.
		/// </summary>
		/// <param name="writer">The HTML writer to which the output is written</param>
		protected override void Render(HtmlTextWriter writer)
		{
			base.Render(writer);

            // Render the validators if needed (may not exist at design time)
            if(rfVal != null)
            {
                if(rfVal.Visible == true)
                    rfVal.RenderControl(writer);

                if(reLen.Visible == true)
                    reLen.RenderControl(writer);
            }
		}
    }
}
