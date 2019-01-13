//=============================================================================
// System  : ASP.NET Web Control Library
// File    : MinMaxListCtrls.cs
// Author  : Eric Woodruff  (Eric@EWoodruff.us)
// Updated : 04/05/2007
// Note    : Copyright 2002-2007, Eric Woodruff, All rights reserved
// Compiler: Microsoft Visual C#
//
// This file contains a derived BaseValidator class that can be used to ensure
// that the number of selected items in a list control does not exceed a
// specified minimum and/or maximum number of selections.  Client-side script
// validation support is available.  This validator will work with a ListBox,
// CheckBoxList, or RadioButtonList control.
//
// Also included in here is the MinMaxListBox, an EWSListBox-derived class,
// and the MinMaxCheckBoxList, an EWSCheckBoxList-derived class, each of which
// automatically generate a MinMaxListValidator for themselves.
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
using System.Web.UI.Design.WebControls;
using System.Web.UI.WebControls;

namespace EWSoftware.Web.Controls
{
    /// <summary>
    /// This is a derived BaseValidator class that ensures that the list
    /// control with which it is associated does not exceed a minimum and/or
    /// maximum number of selections.
    /// </summary>
    [DefaultProperty("MinSel"),
     ToolboxData("<{0}:MinMaxListValidator runat=\"server\" MinSel=\"1\" " +
        "ErrorMessage=\"Selections exceed minimum/maximum\" />"),
     Designer(typeof(BaseValidatorDesigner)),
     AspNetHostingPermission(SecurityAction.LinkDemand,
        Level=AspNetHostingPermissionLevel.Minimal),
     AspNetHostingPermission(SecurityAction.InheritanceDemand,
        Level=AspNetHostingPermissionLevel.Minimal)]
    public class MinMaxListValidator : System.Web.UI.WebControls.BaseValidator
	{
        //=====================================================================
        // Properties

        /// <summary>
        /// This is used to set the minimum number of selections.  If not
        /// specified, a minimum of one selection is assumed.
        /// </summary>
        [Category("Behavior"), DefaultValue(1), Bindable(true),
          Description("The minimum number of selections to make (0 = no minimum)")]
        public int MinSel
        {
            get
            {
                Object oSel = ViewState["MinSel"];
                return (oSel == null) ? 1 : (int)oSel;
            }
            set
            {
                if(value < 0)
                    throw new ArgumentOutOfRangeException("MinSel",
                        value, "The minimum selection count cannot be less than zero");

                ViewState["MinSel"] = value;
            }
        }

        /// <summary>
        /// This is used to set the maximum number of selections.  If not
        /// specified, no maximum is used.
        /// </summary>
        [Category("Behavior"), DefaultValue(0), Bindable(true),
        Description("The maximum number of selections to make (0 = no maximum)")]
        public int MaxSel
        {
            get
            {
                Object oSel = ViewState["MaxSel"];
                return (oSel == null) ? 0 : (int)oSel;
            }
            set
            {
                if(value < 0)
                    throw new ArgumentOutOfRangeException("MaxSel",
                        value, "The maximum selection count cannot be " +
                            "set to less than zero (no maximum)");

                ViewState["MaxSel"] = value;
            }
        }

        //=====================================================================
        // Methods, etc

        /// <summary>
        /// ControlPropertiesValid() is overridden because ListControl objects
        /// can't be validated by default.
        /// </summary>
        /// <returns>Always returns true if no exceptions are thrown</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">This is thrown
        /// if the control specified via the <b>ControlToValidate</b>
        /// property cannot be found.  It is also thrown if the control to
        /// validate is a <b>RadioButtonList</b> and the <see cref="MinSel"/>
        /// and/or <see cref="MaxSel"/> properties are set to a value
        /// greater than one.</exception>
        /// <exception cref="System.ArgumentException">This is thrown if the
        /// control specified by <b>ControlToValidate</b> is not a control
        /// derived from the <b>ListControl</b> class.</exception>
        protected override bool ControlPropertiesValid()
        {
            // Throw an exception if the associated control is not found
            Control ctl = this.Parent.FindControl(this.ControlToValidate);
            if(ctl == null)
                throw new ArgumentOutOfRangeException("ControlToValidate",
                    ControlToValidate, "The specified control was not found");

            // Throw an exception if it isn't of the expected type
            if(!(ctl is ListControl))
                throw new ArgumentException("ControlToValidate should be " +
                    "a ListControl-derived control");

            // For a radio button list, it's pointless to have minumum
            // or maximum selection value greater than one.
            if(ctl is RadioButtonList)
            {
                if(MinSel > 1)
                    throw new ArgumentOutOfRangeException("MinSel", MinSel,
                        "Minimum selection should be 0 or 1 for a RadioButtonList");

                if(MaxSel > 1)
                    throw new ArgumentOutOfRangeException("MaxSel", MaxSel,
                        "Maximum selection should be 0 or 1 for a RadioButtonList");
            }

            return true;
        }

        /// <summary>
        /// EvaluateIsValid() is overridden to handle validating the selection
        /// count.
        /// </summary>
        /// <returns>Returns true if the associated control is valid, false if
        /// it is not.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">This is thrown
        /// if the control specified via the <b>ControlToValidate</b>
        /// property cannot be found.</exception>
        /// <exception cref="System.ArgumentException">This is thrown if the
        /// control specified by <b>ControlToValidate</b> is not a control
        /// derived from the <b>ListControl</b> class.</exception>
        protected override bool EvaluateIsValid()
        {
            int nMaxSel = MaxSel, nCount = 0;

            // Throw an exception if the associated control is not found
            Control ctl = this.Parent.FindControl(ControlToValidate);
            if(ctl == null)
                throw new ArgumentOutOfRangeException("ControlToValidate",
                    ControlToValidate, "The specified control was not found");

            ListControl lc = ctl as ListControl;

            // Throw an exception if it isn't of the expected type
            if(lc == null)
                throw new ArgumentException("ControlToValidate should be " +
                    "a ListControl-derived control");

            foreach(ListItem li in lc.Items)
                if(li.Selected == true)
                    nCount++;

            // Maximum is optional.  If set to zero, it won't be used.
            if(nCount < MinSel || (nMaxSel > 0 && nCount > nMaxSel))
                return false;

            return true;
        }

        /// <summary>
        /// OnPreRender() is overridden to add the client-side script
        /// if needed.
        /// </summary>
        /// <param name="e">The event arguments</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if(this.Visible && this.RenderUplevel &&
              !this.Page.ClientScript.IsClientScriptBlockRegistered("EWS_MMListVal"))
            {
                // Register the script resource
                this.Page.ClientScript.RegisterClientScriptInclude(
                    typeof(MinMaxListValidator), "EWS_MMListVal",
                    this.Page.ClientScript.GetWebResourceUrl(
                    typeof(MinMaxListValidator),
                    CtrlUtils.ScriptsPath + "MinMaxListValidator.js"));
            }
        }

        /// <summary>
        /// AddAttributesToRender() is overridden to add the evaluation
        /// function and min/max attributes to the rendered HTML.
        /// </summary>
		/// <param name="writer">The HTML writer to which the output is written</param>
        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            base.AddAttributesToRender(writer);
            if(RenderUplevel)
            {
                // Throw an exception if the associated control is not found
                Control ctl = this.Parent.FindControl(ControlToValidate);
                if(ctl == null)
                    throw new ArgumentOutOfRangeException("ControlToValidate",
                        ControlToValidate, "The specified control was not found");

                // If it isn't a list box, assume it is a checkbox list or
                // a radio button list.
                if(ctl is ListBox)
                    writer.AddAttribute("evaluationfunction",
                        "MMLB_MinMaxEvaluateIsValid");
                else
                    writer.AddAttribute("evaluationfunction",
                        "MMCKRB_MinMaxEvaluateIsValid");

                writer.AddAttribute("minsel", MinSel.ToString());
                writer.AddAttribute("maxsel", MaxSel.ToString());
            }
        }
	}

    /// <summary>
	/// This derived EWSListBox class can generate a MinMaxListValidator for
    /// itself to insure that the listbox has a minimum and/or maximum number
	/// of selected items.
	/// </summary>
	[DefaultProperty("MinMaxErrorMessage"),
     ToolboxData("<{0}:MinMaxListBox runat=\"server\" MinSel=\"1\" " +
        "MinMaxErrorMessage=\"Selections exceed minimum/maximum\" />"),
     AspNetHostingPermission(SecurityAction.LinkDemand,
        Level=AspNetHostingPermissionLevel.Minimal),
     AspNetHostingPermission(SecurityAction.InheritanceDemand,
        Level=AspNetHostingPermissionLevel.Minimal)]
	public class MinMaxListBox : EWSoftware.Web.Controls.EWSListBox
	{
        //=====================================================================
        // Private class members
        private MinMaxListValidator mmVal;      // The validator

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
        /// The Display property determines whether or not the validators
        /// are displayed by themselves or if they will make use of a summary
        /// control.
	    /// </summary>
		[Category("Appearance"), DefaultValue(ValidatorDisplay.None),
         Bindable(true), Description("The validator display mode")]
		public ValidatorDisplay Display
		{
			get
            {
                // The display setting is the same for all validators
                EnsureChildControls();
                return mmVal.Display;
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
                return mmVal.EnableClientScript;
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
        /// This is the error message to display if the listbox exceeds the
        /// selected item limits.  The string can contain the place holders
        /// {MinSel} and {MaxSel} so that the current minimum and maximum
        /// values are displayed.
	    /// </summary>
		[Category("Appearance"), Bindable(true),
            DefaultValue("Selections exceed minimum/maximum"),
            Description("The error message to display if the listbox exceeds the selected item limits")]
		public string MinMaxErrorMessage
		{
			get
            {
	            Object oMsg = ViewState["MinMaxErrMsg"];
	            return (oMsg == null) ?
                    "Selections exceed minimum/maximum" : (string)oMsg;
            }
			set
            {
                EnsureChildControls();
                ViewState["MinMaxErrMsg"] = value;

                // Clear the message in the validator.  It'll get formatted
                // when rendering the control.
                mmVal.ErrorMessage = String.Empty;
            }
		}

        /// <summary>
        /// This is used to set the minimum number of selections.  If not
        /// specified, a minimum of one selection is assumed.
        /// </summary>
        [Category("Behavior"), DefaultValue(1), Bindable(true),
          Description("The minimum number of selections to make (0 = no minimum)")]
        public int MinSel
        {
            get
            {
                EnsureChildControls();
                return mmVal.MinSel;
            }
            set
            {
                EnsureChildControls();
                mmVal.MinSel = value;

                // The error message is cleared to force reformatting when
                // rendered.
                mmVal.ErrorMessage = String.Empty;
            }
        }

        /// <summary>
        /// This is used to set the maximum number of selections.  If not
        /// specified, no maximum is used.
        /// </summary>
        [Category("Behavior"), DefaultValue(0), Bindable(true),
        Description("The maximum number of selections to make (0 = no maximum)")]
        public int MaxSel
        {
            get
            {
                EnsureChildControls();
                return mmVal.MaxSel;
            }
            set
            {
                EnsureChildControls();
                mmVal.MaxSel = value;

                // The error message is cleared to force reformatting when
                // rendered.
                mmVal.ErrorMessage = String.Empty;
            }
        }

        //=====================================================================
        // Methods, etc

        /// <summary>
        /// CreateChildControls() is overridden to create the validator
        /// </summary>
        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            mmVal = new MinMaxListValidator();
            mmVal.Enabled = this.Enabled;

            // Use validation summary by default
            mmVal.Display = ValidatorDisplay.None;

            this.Controls.Add(mmVal);
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
            mmVal.ControlToValidate = this.ID;
            mmVal.ID = this.ID + "_LBMMV";

            // Format the error message if necessary
            if(mmVal.ErrorMessage.Length == 0)
                mmVal.ErrorMessage = CtrlUtils.ReplaceParameters(
                    MinMaxErrorMessage, "{MinSel}", MinSel.ToString(),
                    "{MaxSel}", MaxSel.ToString());
        }

		/// <summary>
		/// Render this control to the output parameter specified.
		/// </summary>
		/// <param name="writer">The HTML writer to which the output is written</param>
		protected override void Render(HtmlTextWriter writer)
		{
			base.Render(writer);

            if(mmVal != null)
                mmVal.RenderControl(writer);
		}
	}

    /// <summary>
	/// This derived EWSCheckBoxList class can generate a MinMaxListValidator
    /// for itself to insure that the checkbox list has a minimum and/or
	/// maximum number of selected items.
	/// </summary>
	[DefaultProperty("MinMaxErrorMessage"),
     ToolboxData("<{0}:MinMaxCheckBoxList runat=\"server\" MinSel=\"1\" " +
        "MinMaxErrorMessage=\"Selections exceed minimum/maximum\" />"),
     AspNetHostingPermission(SecurityAction.LinkDemand,
        Level=AspNetHostingPermissionLevel.Minimal),
     AspNetHostingPermission(SecurityAction.InheritanceDemand,
        Level=AspNetHostingPermissionLevel.Minimal)]
	public class MinMaxCheckBoxList : EWSoftware.Web.Controls.EWSCheckBoxList
	{
        //=====================================================================
        // Private class members
        private MinMaxListValidator mmVal;      // The validator

        //=====================================================================
        // Properties

	    /// <summary>
        /// The Display property determines whether or not the validators
        /// are displayed by themselves or if they will make use of a summary
        /// control.
	    /// </summary>
		[Category("Appearance"), DefaultValue(ValidatorDisplay.None),
          Bindable(true), Description("The validator display mode")]
		public ValidatorDisplay Display
		{
			get
            {
                // The display setting is the same for all validators
                EnsureChildControls();
                return mmVal.Display;
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
                return mmVal.EnableClientScript;
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
        /// This is the error message to display if the checkbox list exceeds
        /// the selected item limits.  The string can contain the place holders
        /// {MinSel} and {MaxSel} so that the current minimum and maximum
        /// values are displayed.
	    /// </summary>
		[Category("Appearance"), Bindable(true),
            DefaultValue("Selections exceed minimum/maximum"),
            Description("The error message to display if the checkbox list exceeds the selected item limits")]
		public string MinMaxErrorMessage
		{
			get
            {
	            Object oMsg = ViewState["MinMaxErrMsg"];
	            return (oMsg == null) ?
                    "Selections exceed minimum/maximum" : (string)oMsg;
            }
			set
            {
                EnsureChildControls();
                ViewState["MinMaxErrMsg"] = value;

                // Clear the message in the validator.  It'll get formatted
                // when rendering the control.
                mmVal.ErrorMessage = String.Empty;
            }
		}

        /// <summary>
        /// This is used to set the minimum number of selections.  If not
        /// specified, a minimum of one selection is assumed.
        /// </summary>
        [Category("Behavior"), DefaultValue(1), Bindable(true),
          Description("The minimum number of selections to make (0 = no minimum)")]
        public int MinSel
        {
            get
            {
                EnsureChildControls();
                return mmVal.MinSel;
            }
            set
            {
                EnsureChildControls();
                mmVal.MinSel = value;

                // The error message is cleared to force reformatting when
                // rendered.
                mmVal.ErrorMessage = String.Empty;
            }
        }

        /// <summary>
        /// This is used to set the maximum number of selections.  If not
        /// specified, no maximum is used.
        /// </summary>
        [Category("Behavior"), DefaultValue(0), Bindable(true),
        Description("The maximum number of selections to make (0 = no maximum)")]
        public int MaxSel
        {
            get
            {
                EnsureChildControls();
                return mmVal.MaxSel;
            }
            set
            {
                EnsureChildControls();
                mmVal.MaxSel = value;

                // The error message is cleared to force reformatting when
                // rendered.
                mmVal.ErrorMessage = String.Empty;
            }
        }

        //=====================================================================
        // Methods, etc

        /// <summary>
        /// CreateChildControls() is overridden to create the validator
        /// </summary>
        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            mmVal = new MinMaxListValidator();
            mmVal.Enabled = this.Enabled;

            // Use validation summary by default
            mmVal.Display = ValidatorDisplay.None;

            this.Controls.Add(mmVal);
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
            mmVal.ControlToValidate = this.ID;
            mmVal.ID = this.ID + "_CKLMMV";

            // Format the error message if necessary
            if(mmVal.ErrorMessage.Length == 0)
                mmVal.ErrorMessage = CtrlUtils.ReplaceParameters(
                    MinMaxErrorMessage, "{MinSel}", MinSel.ToString(),
                    "{MaxSel}", MaxSel.ToString());
        }

		/// <summary>
		/// Render the controls to the output parameter specified.
		/// </summary>
		/// <param name="writer"> The HTML writer to write out to </param>
		protected override void Render(HtmlTextWriter writer)
        {
			base.Render(writer);

            if(mmVal != null)
                mmVal.RenderControl(writer);
		}
	}
}
