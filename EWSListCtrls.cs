//=============================================================================
// System  : ASP.NET Web Control Library
// File    : EWSListCtrls.cs
// Author  : Eric Woodruff  (Eric@EWoodruff.us)
// Updated : Fri 02/20/04
// Note    : Copyright 2002-2004, Eric Woodruff, All rights reserved
// Compiler: Microsoft Visual C#
//
// This file contains derived ListBox and CheckBoxList classes that contain
// some extra properties to specify a default selection and to set the current
// selection based on a value or text rather than an index.  The list box
// control can also enable incremental search when typing characters in it.
//
// Version     Date     Who  Comments
// ============================================================================
// 1.0.0    12/02/2002  EFW  Created the code
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
	/// This is a derived ListBox class that contains some extra properties
    /// to specify default selection(s) and to set current selection(s) based
    /// on a value or text rather than an index.  It can also enable
    /// incremental search when typing characters in the list box.
	/// </summary>
	[ToolboxData("<{0}:EWSListBox runat=\"server\" />"),
     AspNetHostingPermission(SecurityAction.LinkDemand,
        Level=AspNetHostingPermissionLevel.Minimal),
     AspNetHostingPermission(SecurityAction.InheritanceDemand,
        Level=AspNetHostingPermissionLevel.Minimal)]
	public class EWSListBox : System.Web.UI.WebControls.ListBox
	{
        //=====================================================================
        // Properties

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
                Object oDefSel = ViewState["DefaultSelections"];
                if(oDefSel != null)
                    return ((string [])oDefSel)[0];

                return null;
            }

			set
            {
                string [] strDefSel = new String[1];
                strDefSel[0] = value;

                // Pass it through to DefaultSelections as an array
                DefaultSelections = strDefSel;
            }
		}

	    /// <summary>
        /// This is used to get/set multiple default selections to use if no
        /// other items have been selected first.  Use this when you can't
        /// guarantee that the items collection has been loaded (i.e. in
        /// design-time HTML code or prior to data binding).  This is only
        /// valid for multi-select list boxes.
	    /// </summary>
		[Category("Data"), Browsable(false), Bindable(true),
         Description("Set one or more default selected item values")]
		public string [] DefaultSelections
		{
            get
            {
                Object oDefSel = ViewState["DefaultSelections"];
                if(oDefSel != null)
                    return (string [])oDefSel;

                return null;
            }

			set { ViewState["DefaultSelections"] = value; }
		}

        /// <summary>
        /// This is like DefaultSelection except that it sets the
        /// SelectedIndex to the item matching the specified string
        /// immediately.  Use this when you are sure that the items
        /// collection has been loaded (i.e. in code-behind after
        /// binding a data source to the list box).
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
                                value, "Specified item not found in list box.  " +
                                "Do you need to use the 'DefaultSelection' " +
                                "property instead?");
                }
                else
                    SelectedIndex = -1;
            }
        }

        /// <summary>
        /// This is like CurrentSelection but it accepts a string array
        /// to set multiple selections at the same time.  Use this when
        /// you are sure that the items collection has been loaded (i.e.
        /// in code-behind after binding a data source to the list box).
        /// </summary>
        [Category("Data"), Browsable(false), Bindable(true),
         Description("Set one or more current selections")]
        public string [] CurrentSelections
        {
            set
            {
                int nSelCount = 0;

                // Clear current selection(s)
                ClearSelection();

                if(value != null)
                {
                    foreach(string strValue in value)
                    {
                        ListItem liItem = Items.FindByValue(strValue);
                        if(liItem != null)
                        {
                            liItem.Selected = true;
                            nSelCount++;
                        }
                        else
                            if(strValue.Length > 0)
                                throw new ArgumentOutOfRangeException("CurrentSelections",
                                    strValue, "Specified item not found in list box.  " +
                                    "Do you need to use the 'DefaultSelection' " +
                                    "property instead?");
                    }

                    // If multiple selections were made in a single selection
                    // list box, throw an exception.
                    if(SelectionMode == ListSelectionMode.Single && nSelCount > 1)
                        throw new ArgumentOutOfRangeException("CurrentSelections",
                            nSelCount.ToString(), "Multiple items selected in single-selection list box");
                }
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

	    /// <summary>
        /// This property will return a collection containing the currently
        /// selected items in the list box.  The collection is read-only but
        /// the items in it are not.
	    /// </summary>
		[Category("Data"), Browsable(false),
            Description("The currently selected items in the listbox")]
		public SelectedItemsCollection SelectedItems
		{
			get { return new SelectedItemsCollection(Items); }
		}

	    /// <summary>
        /// This property will return a collection containing the currently
        /// selected indices in the list box.  The collection is read-only but
        /// the items in it are not.
	    /// </summary>
		[Category("Data"), Browsable(false),
            Description("The currently selected indices in the listbox")]
		public SelectedIndicesCollection SelectedIndices
		{
			get { return new SelectedIndicesCollection(Items); }
		}

	    /// <summary>
        /// The IncrementalSearch property determines whether or not the
        /// control allows incrementally searching for entries by typing
	    /// characters in it.  It is enabled by default but only works in
        /// Internet Explorer.
	    /// </summary>
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
        /// This returns true if the control is being rendered in an
        /// up-level browser (currently IE 4+ only)
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

                return (Context.Request.Browser.MSDomVersion.Major >= 4);
            }
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
        /// no default item specified, nothing will be selected.  If multiple
        /// selections have been set, they are all selected.
        /// </summary>
        /// <returns>The selected item's index</returns>
        protected int GetIndexOrSetDefault()
        {
            int nIdx = -1;      // Return -1 if there are no items
            int nSelIdx, nSelCount = 0;

            if(Items.Count > 0)
            {
                // Find and return any existing selection first
    	        for(nIdx = 0; nIdx < Items.Count; nIdx++)
    		        if(Items[nIdx].Selected == true)
    			        return nIdx;

                // Nothing selected, use the default if possible.  Find
                // the item and then determine the index.  If not found
                // and it isn't blank, throw an exception.
                nIdx = -1;

                string [] strDefSels = DefaultSelections;

                if(strDefSels != null)
                {
                    foreach(string strDef in strDefSels)
                    {
                        ListItem liItem = Items.FindByValue(strDef);
                        if(liItem != null)
                        {
                            nSelIdx = Items.IndexOf(liItem);

                            // Return only the first index found but select
                            // all of them if there are multiple entries.
                            if(nIdx == -1)
                            {
                                nIdx = nSelIdx;
                                SelectedIndex = nIdx;
                            }
                            else
                                Items[nSelIdx].Selected = true;

                            nSelCount++;
                        }
                        else
                            if(strDef.Length > 0)
                                throw new ArgumentOutOfRangeException("DefaultSelections",
                                    strDef, "Specified item not found in list box");
                    }

                    // If multiple selections were made in a single selection
                    // list box, throw an exception.
                    if(SelectionMode == ListSelectionMode.Single && nSelCount > 1)
                        throw new ArgumentOutOfRangeException("DefaultSelections",
                            nSelCount.ToString(), "Multiple items selected in single-selection list box");

                    // Fire the Default Selected event if necessary
                    if(nIdx != -1)
                        OnDefaultSelected(EventArgs.Empty);
                }
            }

            return nIdx;
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
        /// OnPreRender is overridden to ensure that the default value gets
        /// selected if it hasn't been done already.  It also renders the
        /// incremental search script if needed.
        /// </summary>
        /// <param name="e">The event arguments</param>
        protected override void OnPreRender(System.EventArgs e)
        {
            base.OnPreRender(e);
            GetIndexOrSetDefault();

            // Register the script resource.  It uses the same script as
            // the dropdown list control.
            if(this.Visible && this.IncrementalSearch &&
              this.CanRenderUpLevel &&
              !this.Page.ClientScript.IsClientScriptBlockRegistered("EWS_SelIncrSrch"))
                this.Page.ClientScript.RegisterClientScriptInclude(
                    typeof(EWSDropDownList), "EWS_SelIncrSrch",
                    this.Page.ClientScript.GetWebResourceUrl(
                    typeof(EWSDropDownList),
                    CtrlUtils.ScriptsPath + "SelectIncrSearch.js"));
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

	/// <summary>
	/// This is a derived CheckBoxList class that contains some extra
    /// properties to specify default selection(s) and to set current
    /// selection(s).
	/// </summary>
	[ToolboxData("<{0}:EWSCheckBoxList runat=\"server\" />"),
     AspNetHostingPermission(SecurityAction.LinkDemand,
        Level=AspNetHostingPermissionLevel.Minimal),
     AspNetHostingPermission(SecurityAction.InheritanceDemand,
        Level=AspNetHostingPermissionLevel.Minimal)]
	public class EWSCheckBoxList : System.Web.UI.WebControls.CheckBoxList
	{
        //=====================================================================
        // Properties

	    /// <summary>
        /// This is used to get/set multiple default selections to use if no
        /// other items have been selected first.  Use this when you can't
        /// guarantee that the items collection has been loaded (i.e. in
        /// design-time HTML code or prior to data binding).
	    /// </summary>
		[Category("Data"), Browsable(false), Bindable(true),
         Description("Set one or more default selected item values")]
		public string [] DefaultSelections
		{
            get
            {
                Object oDefSel = ViewState["DefaultSelections"];
                if(oDefSel != null)
                    return (string [])oDefSel;

                return null;
            }

			set { ViewState["DefaultSelections"] = value; }
		}

        /// <summary>
        /// This is like DefaultSelections but it accepts a string array
        /// to set multiple selections at the same time.  Use this when
        /// you are sure that the items collection has been loaded (i.e.
        /// in code-behind after binding a data source to the checkbox list).
        /// </summary>
        [Category("Data"), Browsable(false), Bindable(true),
         Description("Set one or more current selections")]
        public string [] CurrentSelections
        {
            set
            {
                int nSelCount = 0;

                // Clear current selection(s)
                ClearSelection();

                if(value != null)
                {
                    foreach(string strValue in value)
                    {
                        ListItem liItem = Items.FindByValue(strValue);
                        if(liItem != null)
                        {
                            liItem.Selected = true;
                            nSelCount++;
                        }
                        else
                            if(strValue.Length > 0)
                                throw new ArgumentOutOfRangeException("CurrentSelections",
                                    strValue, "Specified item not found in checkbox list.  " +
                                    "Do you need to use the 'DefaultSelections' " +
                                    "property instead?");
                    }
                }
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

	    /// <summary>
        /// This property will return a collection containing the currently
        /// selected items in the checkbox list.  The collection is read-only
        /// but the items in it are not.
	    /// </summary>
		[Category("Data"), Browsable(false),
            Description("The currently selected items in the checkbox list")]
		public SelectedItemsCollection SelectedItems
		{
			get { return new SelectedItemsCollection(Items); }
		}

	    /// <summary>
        /// This property will return a collection containing the currently
        /// selected indices in the checkbox list .  The collection is
        /// read-only but the items in it are not.
	    /// </summary>
		[Category("Data"), Browsable(false),
            Description("The currently selected indices in the checkbox list")]
		public SelectedIndicesCollection SelectedIndices
		{
			get { return new SelectedIndicesCollection(Items); }
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
        /// no default item specified, nothing will be selected.  If multiple
        /// selections have been set, they are all selected.
        /// </summary>
        /// <returns>The selected item's index</returns>
        protected int GetIndexOrSetDefault()
        {
            int nIdx = -1;      // Return -1 if there are no items
            int nSelIdx, nSelCount = 0;

            if(Items.Count > 0)
            {
                // Find and return any existing selection first
    	        for(nIdx = 0; nIdx < Items.Count; nIdx++)
    		        if(Items[nIdx].Selected == true)
    			        return nIdx;

                // Nothing selected, use the default if possible.  Find
                // the item and then determine the index.  If not found
                // and it isn't blank, throw an exception.
                nIdx = -1;

                string [] strDefSels = DefaultSelections;

                if(strDefSels != null)
                {
                    foreach(string strDef in strDefSels)
                    {
                        ListItem liItem = Items.FindByValue(strDef);
                        if(liItem != null)
                        {
                            nSelIdx = Items.IndexOf(liItem);

                            // Return only the first index found but select
                            // all of them if there are multiple entries.
                            if(nIdx == -1)
                            {
                                nIdx = nSelIdx;
                                SelectedIndex = nIdx;
                            }
                            else
                                Items[nSelIdx].Selected = true;

                            nSelCount++;
                        }
                        else
                            if(strDef.Length > 0)
                                throw new ArgumentOutOfRangeException("DefaultSelections",
                                    strDef, "Specified item not found in checkbox list");
                    }

                    // Fire the Default Selected event if necessary
                    if(nIdx != -1)
                        OnDefaultSelected(EventArgs.Empty);
                }
            }

            return nIdx;
        }

        /// <summary>
        /// OnPreRender is overridden to ensure that the default value gets
        /// selected if it hasn't been done already.
        /// </summary>
        /// <param name="e">The event arguments</param>
        protected override void OnPreRender(System.EventArgs e)
        {
            base.OnPreRender(e);
            GetIndexOrSetDefault();
        }
    }

	/// <summary>
	/// This is a derived RadioButtonList class that contains some extra
    /// properties to specify the default selection and to set the current
    /// selection.
	/// </summary>
	[ToolboxData("<{0}:EWSRadioButtonList runat=\"server\" />"),
     AspNetHostingPermission(SecurityAction.LinkDemand,
        Level=AspNetHostingPermissionLevel.Minimal),
     AspNetHostingPermission(SecurityAction.InheritanceDemand,
        Level=AspNetHostingPermissionLevel.Minimal)]
	public class EWSRadioButtonList : System.Web.UI.WebControls.RadioButtonList
	{
        //=====================================================================
        // Properties

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
        /// binding a data source to the radio button list).
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
                                value, "Specified item not found in radio button " +
                                "list.  Do you need to use the 'DefaultSelection' " +
                                "property instead?");
                }
                else
                    SelectedIndex = -1;
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
        /// no default item specified, nothing will be selected.
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
                nIdx = -1;

                string strDefSel = DefaultSelection;

                if(strDefSel != null)
                {
                    ListItem liItem = Items.FindByValue(strDefSel);
                    if(liItem != null)
                        nIdx = Items.IndexOf(liItem);
                    else
                        if(strDefSel.Length > 0)
                            throw new ArgumentOutOfRangeException("DefaultSelection",
                                strDefSel, "Specified item not found in radio button list");

                    if(nIdx != -1)
                    {
                        SelectedIndex = nIdx;

                        // Fire the Default Selected event
                        OnDefaultSelected(EventArgs.Empty);
                    }
                }
            }

            return nIdx;
        }

        /// <summary>
        /// OnPreRender is overridden to ensure that the default value gets
        /// selected if it hasn't been done already.
        /// </summary>
        /// <param name="e">The event arguments</param>
        protected override void OnPreRender(System.EventArgs e)
        {
            base.OnPreRender(e);
            GetIndexOrSetDefault();
        }
    }
}
