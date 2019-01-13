//=============================================================================
// System  : ASP.NET Web Control Library
// File    : EWSDropDownList.cs
// Author  : Eric Woodruff  (Eric@EWoodruff.us)
// Updated : Fri 02/20/04
// Note    : Copyright 2002-2004, Eric Woodruff, All rights reserved
// Compiler: Microsoft Visual C#
//
// This file contains a derived DropDownList class that contains some extra
// properties to specify a default selection and to set the current selection
// based on a value or text rather than an index.  It also contains a required
// field validator for use if the Required property is set to True.  It can
// also enable incremental search when typing characters in the dropdown list.
//
// Version     Date     Who  Comments
// ============================================================================
// 1.0.0    09/13/2002  EFW  Created the code
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
	/// This is a derived DropDownList class that contains some extra
    /// properties to specify a default selection and to set the current
    /// selection based on a value or text rather than an index.  It also
    /// contains a required field validator for use if the Required property
    /// is set to True.  It can also enable incremental search when typing
    /// characters in the dropdown list.
	/// </summary>
	[ToolboxData("<{0}:EWSDropDownList runat=\"server\" />"),
     AspNetHostingPermission(SecurityAction.LinkDemand,
        Level=AspNetHostingPermissionLevel.Minimal),
     AspNetHostingPermission(SecurityAction.InheritanceDemand,
        Level=AspNetHostingPermissionLevel.Minimal)]
	public class EWSDropDownList : System.Web.UI.WebControls.DropDownList
	{
        //=====================================================================
        // Private class members

        private RequiredFieldValidator rfVal;   // The validator

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
        /// The Required property determines whether or not the
        /// RequiredFieldValidator is generated (default = false).
	    /// </summary>
		[Category("Behavior"), DefaultValue(false), Bindable(true),
         Description("Determines whether or not the control requires a non-blank value to be selected")]
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
        /// The IncrementalSearch property determines whether or not the
        /// control allows incrementally searching for entries by typing
	    /// characters in it.  It is enabled by default but only works in
        /// Internet Explorer.
	    /// </summary>
        /// <remarks>When enabled with <b>AutoPostBack</b> set to true,
        /// the auto-postback behavior is modified.  Instead of posting back
        /// on each change in the selected item as you type, the control will
        /// not perform the post back until it loses focus.</remarks>
		[Category("Behavior"), DefaultValue(true), Bindable(true),
         Description("Enables or disables the incremental search ability")]
		public bool IncrementalSearch
		{
			get
            {
	            Object oIncSearch = ViewState["IncSearch"];
	            return (oIncSearch == null) ? true : (bool)oIncSearch;
            }
			set { ViewState["IncSearch"] = value; }
		}

	    /// <summary>
        /// The Display property determines whether or not the validator is
        /// displayed by itself or if it will make use of a summary control.
	    /// </summary>
		[Category("Appearance"), Bindable(true),
         DefaultValue(ValidatorDisplay.None),
         Description("The validator display mode")]
		public ValidatorDisplay Display
		{
			get
            {
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

                // Show or hide each attached validator
                EnsureChildControls();
                foreach(WebControl ctl in Controls)
                    if(ctl is System.Web.UI.WebControls.BaseValidator)
                        ctl.Visible = value;
            }
        }

	    /// <summary>
        /// This is the error message to display if nothin is selected
	    /// </summary>
		[Category("Appearance"), DefaultValue("Select an item"), Bindable(true),
         Description("The error message to display if nothing is selected")]
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
        /// This can be used to specify whether or not client-side script
        /// is enabled.
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
        /// This is used to get/set the default selection to use if no other
        /// item has been selected first.  Use this when you can't guarantee
        /// that the items collection has been loaded (i.e. in design-time
        /// HTML code or prior to data binding).
	    /// </summary>
		[Category("Data"), Bindable(true),
         Description("The default selected item value")]
		public string DefaultSelection
		{
            get
            {
                Object oDefSel = ViewState["DefaultSelection"];
                if(oDefSel != null)
                    return (string)oDefSel;

                return null;
            }

			set { ViewState["DefaultSelection"] = value; }
		}

        /// <summary>
        /// This is like DefaultSelection except that it sets the
        /// SelectedIndex to the item matching the specified string
        /// immediately.  Use this when you are sure that the items
        /// collection has been loaded (i.e. in code-behind after
        /// binding a data source to the dropdown list).
        /// </summary>
		[Category("Data"), Bindable(true),
         Description("The currently selected item value")]
        public string CurrentSelection
        {
            get
            {
                ListItem liItem = SelectedItem;
                return (liItem != null) ? liItem.Value : null;
            }
            set
            {
                if(value != null)
                {
                    ListItem liItem = Items.FindByValue(value);
                    if(liItem != null)
                        SelectedIndex = Items.IndexOf(liItem);
                    else
                        if(value.Length > 0)
                            throw new ArgumentOutOfRangeException("CurrentSelection",
                                value, "Specified item not found in dropdown " +
                                "list.  Do you need to use the 'DefaultSelection' " +
                                "property instead?");
                }
                else
                    SelectedIndex = -1;
            }
        }

        /// <summary>
        /// This returns true if the control is being rendered in an
        /// up-level browser (MSDOM >= 4.0, ECMAScript enabled)
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

                return (this.Context.Request.Browser.MSDomVersion.Major >= 4);
            }
        }

        /// <summary>
        /// SelectedIndex is overridden so that we can set the default item
        /// when first accessed if no other item is already selected.
        /// </summary>
		[Category("Data"), Browsable(true), Bindable(true), DefaultValue(-1),
         Description("The currently selected item value")]
        public override int SelectedIndex
        {
            get { return GetIndexOrSetDefault(); }
            set { base.SelectedIndex = value; }
        }

        //=====================================================================
        // Events

        /// <summary>
        /// This event is raised when the control selects the default item as
        /// the current selection.
        /// </summary>
		[Category("Action"),
         Description("Fires when the control selects the default item as the current selection")]
        public event EventHandler DefaultSelected;

        /// <summary>
        /// This raises the DefaultSelected event
        /// </summary>
        /// <param name="e">The event arguments</param>
        protected virtual void OnDefaultSelected(System.EventArgs e)
        {
            if(DefaultSelected != null)
                DefaultSelected(this, e);
        }

        //=====================================================================
        // Methods, etc

        /// <summary>
        /// This is a helper method that either returns the index of the
        /// currently selected item or, if nothing is set, it finds the
        /// default value and makes it the current selection.  If there is
        /// no default item specified, item zero is set as the currently
        /// selected item to match the behavior of the base class.
        /// </summary>
        /// <returns>The selected item's index</returns>
        protected int GetIndexOrSetDefault()
        {
            int nIdx = -1;      // Return -1 if there are no items

            if(Items.Count > 0)
            {
                // Find and return any existing selection first
    	        for(nIdx = 0; nIdx < Items.Count; nIdx++)
    		        if(Items[nIdx].Selected == true)
    			        return nIdx;

                // Nothing selected, use the default if possible.  Find
                // the item and then determine the index.  If not found
                // and it isn't blank, throw an exception.
                nIdx = 0;

                string strDefSel = DefaultSelection;

                if(strDefSel != null)
                {
                    ListItem liItem = Items.FindByValue(strDefSel);
                    if(liItem != null)
                        nIdx = Items.IndexOf(liItem);
                    else
                        if(strDefSel.Length > 0)
                            throw new ArgumentOutOfRangeException("DefaultSelection",
                                strDefSel, "Specified item not found in dropdown list");
                }

                SelectedIndex = nIdx;

                // Fire the Default Selected event
                OnDefaultSelected(EventArgs.Empty);
            }

            return nIdx;
        }

        /// <summary>
        /// The base <see cref="System.Web.UI.WebControls.DropDownList"/> class
        /// doesn't allow a child control collection.  We'll override it so
        /// that we can have one for the validator.
        /// </summary>
        /// <returns>A control collection object</returns>
        protected override System.Web.UI.ControlCollection CreateControlCollection()
        {
            return new ControlCollection(this);
        }

        /// <summary>
        /// This is overridden to create the
        /// <see cref="System.Web.UI.WebControls.RequiredFieldValidator"/>
        /// used to validate the input.
        /// </summary>
        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            rfVal = new RequiredFieldValidator();
            rfVal.Enabled = this.Enabled;

            // Use validation summary by default
            rfVal.Display = ValidatorDisplay.None;
            rfVal.ErrorMessage = "Select an item";

            this.Controls.Add(rfVal);
        }

        /// <summary>
        /// AddAttributesToRender is overridden to add attributes for the
        /// incremental search feature and to provide correct support for
        /// auto-postback when using incremental search.
        /// </summary>
		/// <param name="writer">The HTML writer to which the output is written</param>
        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            if(this.IncrementalSearch && this.CanRenderUpLevel)
            {
                // Hook up the event handlers
                writer.AddAttribute("onkeydown",
                    "javascript:return SIS_OnKeyDown(event.srcElement, event.keyCode)");
                writer.AddAttribute("onkeypress",
                    "javascript:return SIS_OnKeyPress(event.srcElement, event.keyCode)");
                writer.AddAttribute("onfocus",
                    "javascript:SIS_strSearchText = ''");

                if(this.AutoPostBack && this.Page != null)
                {
                    writer.AddAttribute("AutoPostBack",
                        this.Page.ClientScript.GetPostBackEventReference(this, String.Empty));
                    writer.AddAttribute("onblur", "javascript:SIS_OnBlur(event.srcElement)");

                    // Reset the OnChange event to what it was without the
                    // normal auto-postback code.
                    writer.AddAttribute(HtmlTextWriterAttribute.Onchange,
                        "javascript:SIS_OnChange();" + base.Attributes["onchange"]);

                    // Turn off AutoPostBack so that the base class doesn't render the
                    // OnChange attribute again.  This won't affect view state as we
                    // are already in the rendering phase.
                    this.AutoPostBack = false;
                }
            }

            base.AddAttributesToRender(writer);
        }

        /// <summary>
        /// OnPreRender is overridden to set the control to validate and the
        /// validation control IDs and to render the incremental search
        /// script when necessary.
        /// </summary>
        /// <remarks>The ID of the control to validate and the validator's
        /// ID cannot be set earlier as we cannot guarantee the order in which
        /// the properties are set.  The ID of the validator control is the
        /// same as the control ID with a suffix of "_RFV".  This also makes
        /// the required field validator invisible if it is not being used.
        /// <p/>If client-side script is enabled, and it is being rendered in
        /// an <see cref="CanRenderUpLevel">up-level</see> browser,
        /// the control will render client-side script that enables several
        /// special keys and incremental searching.</remarks>
        /// <param name="e">The event arguments</param>
        protected override void OnPreRender(System.EventArgs e)
        {
            if(this.Visible)
            {
                if(this.Required)
                {
                    rfVal.Visible = true;
                    rfVal.ControlToValidate = this.ID;
                    rfVal.ID = this.ID + "_RFV";
                }
                else
                    rfVal.Visible = false;

                // Register the script resource
                if(this.IncrementalSearch && this.CanRenderUpLevel &&
                  !this.Page.ClientScript.IsClientScriptBlockRegistered("EWS_SelIncrSrch"))
                    this.Page.ClientScript.RegisterClientScriptInclude(
                        typeof(EWSDropDownList), "EWS_SelIncrSrch",
                        this.Page.ClientScript.GetWebResourceUrl(
                        typeof(EWSDropDownList),
                        CtrlUtils.ScriptsPath + "SelectIncrSearch.js"));
            }
            else
                rfVal.Visible = false;

            base.OnPreRender(e);
            GetIndexOrSetDefault();
        }

		/// <summary>
		/// Render this control to the output parameter specified.
		/// </summary>
		/// <param name="writer">The HTML writer to which the output is written</param>
		protected override void Render(HtmlTextWriter writer)
		{
			base.Render(writer);

            // Render the required field validator if needed
            if(rfVal != null && rfVal.Visible == true)
                rfVal.RenderControl(writer);
		}

        /// <summary>
        /// Find an item by its text and set it as the currently selected item
        /// or, if not found, default to the specified item (no selection if
        /// it is null).  The search comparison can be case insensitive or
        /// case sensitive.
        /// </summary>
        /// <param name="strValue">The text value to locate</param>
        /// <param name="strDefault">The default text value to use if the
        /// main one is not found</param>
        /// <param name="bCaseInsensitive">Whether or not the search is case insensitive</param>
        public void SetSelectionByText(string strValue, string strDefault,
            bool bCaseInsensitive)
        {
            int nIdx = -1, nCurPos = 0;

            // Try to find the specified value
            if(strValue != null)
                foreach(ListItem li in Items)
                {
                    if(String.Compare(li.Text, strValue, bCaseInsensitive) == 0)
                    {
                        nIdx = nCurPos;
                        break;
                    }

                    nCurPos++;
                }

            // If not found and a default has been specified, try to find
            // and use it instead.
            if(nIdx == -1 && strDefault != null)
                SetSelectionByText(strDefault, null, bCaseInsensitive);
            else
                SelectedIndex = nIdx;
        }
    }
}
