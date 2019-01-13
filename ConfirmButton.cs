//=============================================================================
// System  : ASP.NET Web Control Library
// File    : ConfirmButton.cs
// Author  : Eric Woodruff  (Eric@EWoodruff.us)
// Updated : Fri 02/20/04
// Note    : Copyright 2002-2004, Eric Woodruff, All rights reserved
// Compiler: Microsoft Visual C#
//
// This file contains derived Button and LinkButton classes that will prompt
// the user for confirmation of the operation.  If they click OK, the button
// or link will submit the request.  Clicking Cancel will cancel the request.
// If the button/link causes validation, it will defer to the client-side
// validation code to determine whether or not the submit should actually
// occur.  Note that there cannot be any server-side prompt so the
// confirmation is strictly client-side.  Down-level clients that don't
// support script will probably just execute the action.
//
// Version     Date     Who  Comments
// ============================================================================
// 1.0.0    09/06/2002  EFW  Created the code
// 1.0.0    09/11/2002  EFW  Added ConfirmLinkButton class
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
	/// This derived <see cref="System.Web.UI.WebControls.Button"/> class will
    /// prompt the user with a confirmation message before allowing the submit
    /// to occur.
	/// </summary>
	[DefaultProperty("ConfirmPrompt"),
     ToolboxData("<{0}:ConfirmButton runat=\"server\" Text=\"Confirm\" " +
        "ConfirmPrompt=\"Are you sure?\" />"),
     AspNetHostingPermission(SecurityAction.LinkDemand,
        Level=AspNetHostingPermissionLevel.Minimal),
     AspNetHostingPermission(SecurityAction.InheritanceDemand,
        Level=AspNetHostingPermissionLevel.Minimal)]
	public class ConfirmButton : System.Web.UI.WebControls.Button
	{
        //=====================================================================
        // Properties

        /// <summary>
        /// This is the confirmation prompt to display.
        /// </summary>
        /// <value>If not set, it defaults to "<b>Are you sure?</b>"</value>
        [Category("Appearance"), DefaultValue("Are you sure?"), Bindable(true),
         Description("The confirmation prompt to display")]
		public string ConfirmPrompt
		{
			get
            {
	            Object oConfirmPrompt = ViewState["ConfirmPrompt"];
	            return (oConfirmPrompt == null) ? "Are you sure?" : (string)oConfirmPrompt;
            }
			set { ViewState["ConfirmPrompt"] = value; }
		}

        //=====================================================================
        // Methods, etc

        /// <summary>
        /// This is overridden to generate a custom JavaScript <b>OnClick</b>
        /// event handler for the button that prompts the user to confirm
        /// the postback.
        /// </summary>
        /// <param name="writer">The HTML text writer to use for output</param>
        /// <remarks>This is a bit of a hack, but there is no other way to
        /// alter the generated <b>OnClick</b> attribute.  This tells the base
        /// class that there is no validation so that it does not add the
        /// <b>OnClick</b> attribute.  Then it restores the actual
        /// <b>CausesValidation</b> property state and adds our own
        /// handler.</remarks>
        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            string strOnClick;
            bool bCausesVal = this.CausesValidation;

            // Fake the base class into not adding it's own OnClick handler
            this.CausesValidation = false;
            base.AddAttributesToRender(writer);
            this.CausesValidation = bCausesVal;

	        if(this.Page != null)
            {
                if(bCausesVal == true && this.Page.Validators.Count > 0)
                {
    		        // It normally calls Util.GetClientValidateEvent() to get
                    // the code that does the validation but it's an internal
                    // member so we don't have access to it.  The code it
                    // returns is added as a literal to work around the
                    // problem.
                    strOnClick = "if(confirm('" + this.ConfirmPrompt + "')) " +
                        "{ if (typeof(Page_ClientValidate) == 'function') " +
                            "Page_ClientValidate(); } else return false;";
	            }
                else
                    strOnClick = "return confirm('" + this.ConfirmPrompt + "');";

                writer.AddAttribute(HtmlTextWriterAttribute.Onclick, strOnClick);
		        writer.AddAttribute("language", "javascript");
            }
        }
	}

	/// <summary>
	/// This derived <see cref="System.Web.UI.WebControls.LinkButton"/> class
    /// will prompt the user with a confirmation message before allowing the
    /// submit to occur.
	/// </summary>
	[DefaultProperty("ConfirmPrompt"),
     ToolboxData("<{0}:ConfirmLinkButton runat=\"server\" CommandName=\"Confirm\" " +
        "Text=\"Confirm\" ConfirmPrompt=\"Are you sure?\"/>"),
     AspNetHostingPermission(SecurityAction.LinkDemand,
        Level=AspNetHostingPermissionLevel.Minimal),
     AspNetHostingPermission(SecurityAction.InheritanceDemand,
        Level=AspNetHostingPermissionLevel.Minimal)]
	public class ConfirmLinkButton : System.Web.UI.WebControls.LinkButton
	{
        //=====================================================================
        // Properties

        /// <summary>
        /// This is the confirmation prompt to display.
        /// </summary>
        /// <value>If not set, it defaults to "<b>Are you sure?</b>"</value>
        [Category("Appearance"), DefaultValue("Are you sure?"), Bindable(true),
         Description("The confirmation prompt to display")]
		public string ConfirmPrompt
		{
			get
            {
	            Object oConfirmPrompt = ViewState["ConfirmPrompt"];
	            return (oConfirmPrompt == null) ? "Are you sure?" : (string)oConfirmPrompt;
            }
			set { ViewState["ConfirmPrompt"] = value; }
		}

        //=====================================================================
        // Methods, etc

        /// <summary>
        /// This is overridden to generate a custom <b>href</b> attribute
        /// for the link.
        /// </summary>
        /// <param name="writer">The HTML text writer to use for output</param>
        /// <remarks>This is a bit of a hack, but there's no other way to alter
        /// the generated <b>href</b> attribute.  This has to add the common
        /// attributes manually rather than letting the base classes do it.
        /// </remarks>
        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            string strOnClick;

            if(this.Page != null)
                this.Page.VerifyRenderingInServerForm(this);

	        if(this.ID != null)
	            writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ClientID);

            if(this.AccessKey.Length > 0)
			    writer.AddAttribute(HtmlTextWriterAttribute.Accesskey,
                    this.AccessKey);

	        if(!this.Enabled)
                writer.AddAttribute(HtmlTextWriterAttribute.Disabled, "disabled");

            if(this.TabIndex != 0)
                writer.AddAttribute(HtmlTextWriterAttribute.Tabindex,
                    this.TabIndex.ToString());

            if(this.ToolTip.Length > 0)
                writer.AddAttribute(HtmlTextWriterAttribute.Title, this.ToolTip);

	        if(this.ControlStyleCreated)
	            this.ControlStyle.AddAttributesToRender(writer, this);

            foreach(string strAttr in Attributes.Keys)
	            writer.AddAttribute(strAttr, Attributes[strAttr]);

            if(this.Enabled == true && this.Page != null)
            {
                if(this.CausesValidation == true && this.Page.Validators.Count > 0)
                {
    		        // It normally calls Util.GetClientValidateEvent() to get the
                    // code that does the validation but it's an internal member
                    // so we don't have access to it.  The code it returns is
                    // added as a literal to work around the problem.
                    strOnClick = "javascript: { if(confirm('" +
                        this.ConfirmPrompt + "')) " +
                        "{ if (typeof(Page_ClientValidate) != 'function' || Page_ClientValidate()) " +
                            this.Page.ClientScript.GetPostBackEventReference(this, String.Empty) +
                        "} }";
	            }
                else
                    strOnClick = "javascript: if(confirm('" +
                        this.ConfirmPrompt + "')) " +
            		    this.Page.ClientScript.GetPostBackEventReference(this, String.Empty);

                writer.AddAttribute(HtmlTextWriterAttribute.Href, strOnClick);
            }
        }
	}
}
