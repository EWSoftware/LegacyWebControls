//=============================================================================
// System  : ASP.NET Web Control Library
// File    : NumericTextBox.cs
// Author  : Eric Woodruff  (Eric@EWoodruff.us)
// Updated : Fri 02/20/04
// Note    : Copyright 2002-2004, Eric Woodruff, All rights reserved
// Compiler: Microsoft Visual C#
//
// This file contains a derived CompareTextBox class that can automatically
// generate a range validator for itself that checks to see if the data
// entered is a numeric value (currency, integer, or double) and, optionally,
// if it is above, below, or between minimum and maximum values.  A regular
// expression validator can also be emitted to limit the number of decimal
// places in double and decimal type numbers.
//
// Version     Date     Who  Comments
// ============================================================================
// 1.0.0    09/04/2002  EFW  Created the code
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
    /// This enumerated type is a subset of
    /// <see cref="System.Web.UI.WebControls.ValidationDataType"/>
    /// (numeric only) and is used by the <see cref="NumericTextBox"/> control.
    /// </summary>
    [Serializable]
    public enum NumericType
    {
        /// <summary>A 32-bit signed integer value</summary>
        Integer = ValidationDataType.Integer,
        /// <summary>A double precision floating point value</summary>
        Double = ValidationDataType.Double,
        /// <summary>A monetary data type treated like a <b>System.Decimal</b>
        /// value</summary>
        Currency = ValidationDataType.Currency
    }

	/// <summary>
	/// This derived CompareTextBox class can generate a RangeValidator
	/// for itself to insure a date value is entered and is optionally
	/// within a specified range.  It also has special formatting abilities
    /// when rendered in Internet Explorer.
	/// </summary>
    /// <include file='Doc/Controls.xml'
    /// path='Controls/NumericTextBox/Member[@name="Class"]/*' />
	[DefaultProperty("NumberErrorMessage"),
     ToolboxData("<{0}:NumericTextBox runat=\"server\" " +
        "Type=\"Integer\" MinimumValue=\"0\" MaximumValue=\"999\" " +
        "NumberErrorMessage=\"Not a valid numeric value\" />"),
     AspNetHostingPermission(SecurityAction.LinkDemand,
        Level=AspNetHostingPermissionLevel.Minimal),
     AspNetHostingPermission(SecurityAction.InheritanceDemand,
        Level=AspNetHostingPermissionLevel.Minimal)]
	public class NumericTextBox : EWSoftware.Web.Controls.CompareTextBox
	{
        //=====================================================================
        // Private class members

        private RangeValidator rvRange;     // The validators
        private RegularExpressionValidator reDecimals;

        //=====================================================================
        // Properties

	    /// <summary>
        /// This is the error message to display if the number is not valid or
        /// it is not in the expected range.  The string can contain the place
        /// holders {MinVal} and {MaxVal} so that the current minimum and
        /// maximum values are displayed.
	    /// </summary>
		[Category("Appearance"), Bindable(true),
            DefaultValue("Not a valid numeric value"),
            Description("The error message to display for invalid numbers")]
		public string NumberErrorMessage
		{
			get
            {
	            Object oMsg = ViewState["NumErrMsg"];
	            return (oMsg == null) ? "Not a valid numeric value" :
                    (string)oMsg;
            }
			set
            {
                EnsureChildControls();
                ViewState["NumErrMsg"] = value;

                // Clear the message in the validator.  It'll get formatted
                // when rendering the control.
                rvRange.ErrorMessage = String.Empty;
            }
		}

        /// <summary>
        /// The numeric type.  Note that use of any of the Min/Max
        /// properties will automatically set the appropriate data type.
        /// </summary>
		[Category("Behavior"), DefaultValue(NumericType.Integer), Bindable(true),
         Description("The numeric type")]
        public NumericType Type
        {
            get
            {
                EnsureChildControls();
                return (NumericType)rvRange.Type;
            }
            set
            {
                EnsureChildControls();
                rvRange.Type = (ValidationDataType)value;

                // Set the data type on the base class's compare
                // validator too.
                CompareType = (ValidationDataType)value;
            }
        }

        /// <summary>
        /// This allows the specification of a minimum value.  The Type
        /// property must be set accordingly or the range validator type
        /// will default to integer.
        /// </summary>
		[Category("Behavior"), DefaultValue("-2147483648"), Bindable(true),
            Description("The minimum value")]
        public string MinimumValue
        {
            get { return rvRange.MinimumValue; }
            set
            {
                EnsureChildControls();

                // Parse it as a decimal to ensure that it is at least
                // a number.  It'll throw an exception if it isn't.
                decimal dTemp = Decimal.Parse(value);
                rvRange.MinimumValue = value;

                // The error message is cleared to force reformatting when
                // rendered.
                rvRange.ErrorMessage = String.Empty;
            }
        }

        /// <summary>
        /// This allows the specification of a maximum value.  The Type
        /// property must be set accordingly or the range validator type
        /// will default to integer.
        /// </summary>
		[Category("Behavior"), DefaultValue("2147483647"), Bindable(true),
            Description("The maximum value")]
        public string MaximumValue
        {
            get { return rvRange.MaximumValue; }
            set
            {
                EnsureChildControls();

                // Parse it as a decimal to ensure that it is at least
                // a number.  It'll throw an exception if it isn't.
                decimal dTemp = Decimal.Parse(value);
                rvRange.MaximumValue = value;

                // The error message is cleared to force reformatting when
                // rendered.
                rvRange.ErrorMessage = String.Empty;
            }
        }

        /// <summary>
        /// This sets the maximum number of decimal places allowed on the
        /// number.  Set it to -1 to allow any number of decimal places.
        /// </summary>
		[Category("Behavior"), DefaultValue(-1), Bindable(true),
         Description("The number of decimal places")]
        public int DecimalPlaces
        {
			get
            {
	            Object oNumDecs = ViewState["DecimalPlaces"];
	            return (oNumDecs == null) ? -1 : (int)oNumDecs;
            }
            set
            {
                EnsureChildControls();
                ViewState["DecimalPlaces"] = value;

                if(value > -1)
                {
                    // Set the validation expression with the right number of
                    // decimal places.  There can be a leading + or -.
                    reDecimals.ValidationExpression = String.Format(
                        @"^\s*([\+-])?(\d+)?(\.(\d{{0,{0}}}))?\s*$", value);

                    // The error message is cleared to force reformatting when
                    // rendered.
                    reDecimals.ErrorMessage = String.Empty;
                }
            }
        }

	    /// <summary>
        /// This is the error message to display if the number of decimal
        /// places is not valid.  The message can contain the place holder
        /// {Decimals} to display the current maximum decimal places value.
	    /// </summary>
		[Category("Appearance"), Bindable(true),
            DefaultValue("There can be a maximum of {Decimals} decimal place(s)"),
            Description("The error to display if an invalid number of decimal places is entered")]
		public string DecimalErrorMessage
		{
			get
            {
	            Object oMsg = ViewState["DecErrMsg"];
	            return (oMsg == null) ?
                    "There can be a maximum of {Decimals} decimal place(s)" :
                    (string)oMsg;
            }
			set
            {
                EnsureChildControls();
                ViewState["DecErrMsg"] = value;

                // Clear the message in the validator.  It'll get formatted
                // when rendering the control.
                reDecimals.ErrorMessage = String.Empty;
            }
		}

        /// <summary>
        /// This sets the client-side formatting behaviour for numbers with
        /// decimal places.  If set to true, the decimal point is inserted
        /// into the number.  If false, decimal places are appended to the
        /// number.
        /// </summary>
		[Category("Behavior"), DefaultValue(false), Bindable(true),
         Description("Insert or append decimal point for client-side formatting")]
        public bool InsertDecimal
        {
			get
            {
	            Object oInsDec = ViewState["InsertDecimal"];
	            return (oInsDec == null) ? false : (bool)oInsDec;
            }
            set { ViewState["InsertDecimal"] = value; }
        }

        //=====================================================================
        // Methods, etc

        /// <summary>
        /// Constructor.  Default state: Any number of decimals allowed and,
        /// unless overridden, the field will be right-aligned.
        /// </summary>
        public NumericTextBox()
        {
            this.Style["text-align"] = "right";
        }

        /// <summary>
        /// CreateChildControls() is overridden to create the validator
        /// </summary>
        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            rvRange = new RangeValidator();
            reDecimals = new RegularExpressionValidator();

            // Assume an integer type by default
            MinimumValue = Int32.MinValue.ToString();
            MaximumValue = Int32.MaxValue.ToString();
            rvRange.Type = CompareType = ValidationDataType.Integer;
            rvRange.Enabled = reDecimals.Enabled = this.Enabled;

            // Use validation summary by default
            rvRange.Display = reDecimals.Display = ValidatorDisplay.None;

            this.Controls.Add(rvRange);
            this.Controls.Add(reDecimals);
        }

        /// <summary>
        /// OnPreRender is overridden to set the control to validate and the
        /// validation control IDs.  These can't be set earlier as we can't
        /// guarantee the order in which the properties are set.  The names of
        /// the validator controls are based on the control to validate.
        /// It also sets the decimal places validator to invisible if it isn't
        /// being used.
        /// </summary>
        /// <param name="e">The event arguments</param>
        protected override void OnPreRender(System.EventArgs e)
        {
            if(Visible)
            {
                if(DecimalPlaces > -1 && Type != NumericType.Integer)
                {
                    reDecimals.Visible = true;
                    reDecimals.ControlToValidate = this.ID;
                    reDecimals.ID = this.ID + "_NMREV";
                    reDecimals.Attributes["insdec"] = InsertDecimal.ToString();
                    reDecimals.Attributes["decplaces"] = DecimalPlaces.ToString();

                    // Format the error message if necessary
                    if(reDecimals.ErrorMessage.Length == 0)
                        reDecimals.ErrorMessage = CtrlUtils.ReplaceParameters(
                            DecimalErrorMessage, "{Decimals}",
                            DecimalPlaces.ToString());
                }
                else
                    reDecimals.Visible = false;

                // Register formatting script if needed
                if(!this.AutoPostBack && this.EnableClientScript &&
                  this.CanRenderUpLevel)
                {
                    this.Attributes["onblur"] = "javascript:NTB_FormatNumeric(this);";

                    if(!Page.ClientScript.IsClientScriptBlockRegistered("EWS_NTBFormat"))
                        this.Page.ClientScript.RegisterClientScriptInclude(
                            typeof(NumericTextBox), "EWS_NTBFormat",
                            this.Page.ClientScript.GetWebResourceUrl(
                            typeof(NumericTextBox),
                            CtrlUtils.ScriptsPath + "NumericTextBox.js"));
                }
            }

            base.OnPreRender(e);

            rvRange.ControlToValidate = this.ID;
            rvRange.ID = this.ID + "_NMRV";

            // Format the error message if necessary
            if(rvRange.ErrorMessage.Length == 0)
                rvRange.ErrorMessage = CtrlUtils.ReplaceParameters(
                    NumberErrorMessage, "{MinVal}", rvRange.MinimumValue,
                    "{MaxVal}", rvRange.MaximumValue, "{Decimals}",
                    DecimalPlaces.ToString());
        }

		/// <summary>
		/// Render this control to the output parameter specified.
		/// </summary>
		/// <param name="writer">The HTML writer to which the output is written</param>
		protected override void Render(HtmlTextWriter writer)
		{
			base.Render(writer);

            if(rvRange != null)
            {
                rvRange.RenderControl(writer);

                if(reDecimals.Visible == true)
                    reDecimals.RenderControl(writer);
            }
		}

	    /// <summary>
        /// This returns the current value of the textbox as an integer.
	    /// </summary>
        /// <returns>If empty, it returns 0.</returns>
        public int ToInteger()
        {
            if(this.Text.Length == 0)
                return 0;

            return Int32.Parse(this.Text);
        }

	    /// <summary>
        /// This returns the current value of the textbox as a double.
	    /// </summary>
        /// <returns>If empty, it returns 0.0.</returns>
        public double ToDouble()
        {
            if(this.Text.Length == 0)
                return 0.0;

            return Double.Parse(this.Text);
        }

	    /// <summary>
        /// This returns the current value of the textbox as a
        /// <see cref="System.Decimal"/>.
	    /// </summary>
        /// <returns>If empty, it returns 0.0.</returns>
        public Decimal ToDecimal()
        {
            if(this.Text.Length == 0)
                return 0.0M;

            return Decimal.Parse(this.Text);
        }
	}
}
