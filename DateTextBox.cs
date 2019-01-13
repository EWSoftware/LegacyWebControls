//=============================================================================
// System  : ASP.NET Web Control Library
// File    : DateTextBox.cs
// Author  : Eric Woodruff  (Eric@EWoodruff.us)
// Updated : 10/04/2010
// Note    : Copyright 2002-2010, Eric Woodruff, All rights reserved
// Compiler: Microsoft Visual C#
//
// This file contains a derived CompareTextBox class that can automatically
// generate a range validator for itself that checks to see if the data
// entered is a date and, optionally, if it is above, below, or between
// minimum and maximum date values.
//
// Version     Date     Who  Comments
// ============================================================================
// 1.0.0.0  09/03/2002  EFW  Created the code
// 3.0.0.0  10/04/2010  EFW  Updated for use with .NET 4.0
//=============================================================================

using System;
using System.ComponentModel;
using System.Security.Permissions;
using System.Web.UI;
using System.Web.UI.WebControls;

using EWSoftware.Web.Design;

namespace EWSoftware.Web.Controls
{
	/// <summary>
	/// This derived <see cref="CompareTextBox"/> class can generate a
    /// <see cref="System.Web.UI.WebControls.RangeValidator"/> for itself
	/// to insure a date value is entered and is optionally within a
    /// specified range.  It also has special abilities when rendered in
    /// Internet Explorer.
	/// </summary>
    /// <include file='Doc/Controls.xml'
    /// path='Controls/DateTextBox/Member[@name="Class"]/*' />
	[DefaultProperty("DateErrorMessage"),
     ToolboxData("<{0}:DateTextBox runat=\"server\" " +
        "DateErrorMessage=\"Not a valid date value\" />"),
     Designer(typeof(DateTextBoxDesigner))]
	public class DateTextBox : EWSoftware.Web.Controls.CompareTextBox
	{
        //=====================================================================
        // Private class members

        private const string cMinDate = "01/01/1000";   // Minimum date
        private const string cMaxDate = "12/31/9999";   // Maximum date

        private RangeValidator rvRange;     // The validator

        private string strCalPopup;         // Calendar popup HTML

        //=====================================================================
        // Properties

        /// <summary>
        /// This property is overridden to format the incoming text as a date.
        /// </summary>
        /// <value>If null, empty, or not a valid date value, the text is set
        /// to an empty string.</value>
		[Category("Appearance"), DefaultValue(""), Bindable(true)]
        public override string Text
        {
            get { return base.Text; }
            set
            {
                try
                {
                    base.Text = (value == null || value.Length == 0) ? String.Empty :
                        String.Format("{0:MM/dd/yyyy}", DateTime.Parse(value));
                }
                catch
                {
                    // Ignore exceptions on invalid dates, just clear the text
                    base.Text = String.Empty;
                }
            }
        }

	    /// <summary>
        /// This is the error message to display if the date is not valid or
        /// it is not in the expected range.
	    /// </summary>
        /// <value>If not set, it defaults to "<b>Not a valid date value</b>".
        /// <p/>The message can contain the place holders <b>{MinDate}</b> and
        /// <b>{MaxDate}</b> so that the current minimum and maximum date
        /// values are formatted in the message before being rendered to
        /// the browser.</value>
		[Category("Appearance"), Bindable(true),
         DefaultValue("Not a valid date value"),
         Description("The error message to display for invalid dates")]
		public string DateErrorMessage
		{
			get
            {
	            Object oMsg = ViewState["DateErrMsg"];
	            return (oMsg == null) ? "Not a valid date value" : (string)oMsg;
            }
			set
            {
                EnsureChildControls();
                ViewState["DateErrMsg"] = value;

                // Clear the message in the validator.  It'll get formatted
                // when rendering the control.
                rvRange.ErrorMessage = String.Empty;
            }
		}

        /// <summary>
        /// This allows the specification of a minimum date value
        /// </summary>
        /// <value>If not set, a minimum date value of 01/01/1000 is used.</value>
		[Category("Behavior"),  Bindable(true), Description("The minimum date value")]
        public DateTime MinimumDate
        {
            get
            {
                EnsureChildControls();
                return DateTime.Parse(rvRange.MinimumValue);
            }
            set
            {
                EnsureChildControls();

                // The validator doesn't seem to like minimum dates
                // less than 01/01/1000.
                if(value < DateTime.Parse(cMinDate))
                    rvRange.MinimumValue = cMinDate;
                else
                    rvRange.MinimumValue = value.ToString("MM/dd/yyyy");

                // The error message is cleared to force reformatting when
                // rendered.
                rvRange.ErrorMessage = String.Empty;
            }
        }

        /// <summary>
        /// This allows the specification of a maximum date value
        /// </summary>
        /// <value>If not set, a maximum date value of 12/31/9999 is used.</value>
		[Category("Behavior"),  Bindable(true), Description("The maximum date value")]
        public DateTime MaximumDate
        {
            get
            {
                EnsureChildControls();
                return DateTime.Parse(rvRange.MaximumValue);
            }
            set
            {
                EnsureChildControls();

                // Limit to maximum date (without time)
                if(value > DateTime.Parse(cMaxDate))
                    rvRange.MaximumValue = cMaxDate;
                else
                    rvRange.MaximumValue = value.ToString("MM/dd/yyyy");

                // The error message is cleared to force reformatting when
                // rendered.
                rvRange.ErrorMessage = String.Empty;
            }
        }

        //=====================================================================
        // Methods, etc

        /// <summary>
        /// Constructor.  Default state: Size and length are limited to a
        /// maximum of 10 characters.
        /// </summary>
        public DateTextBox()
        {
            this.MaxLength = this.Columns = 10;
        }

        /// <summary>
        /// This is overridden to create the
        /// <see cref="System.Web.UI.WebControls.RangeValidator"/> used to
        /// validate the input.
        /// </summary>
        /// <remarks>The <see cref="CompareTextBox.CompareType"/> property
        /// is automatically set to
        /// <see cref="System.Web.UI.WebControls.ValidationDataType">Date</see>.
        /// </remarks>
        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            rvRange = new RangeValidator();
            rvRange.Type = ValidationDataType.Date;
            rvRange.Enabled = this.Enabled;

            this.MinimumDate = DateTime.MinValue;
            this.MaximumDate = DateTime.MaxValue;

            // Use validation summary by default
            rvRange.Display = ValidatorDisplay.None;

            // Set the data type on the base class's comparison validator
            // to date so that the user doesn't have to do it.
            this.CompareType = ValidationDataType.Date;

            this.Controls.Add(rvRange);
        }

        /// <summary>
        /// OnPreRender is overridden to set the control to validate and the
        /// validation control IDs.
        /// </summary>
        /// <remarks>The ID of the control to validate and the validator's
        /// ID cannot be set earlier as we cannot guarantee the order in which
        /// the properties are set.  The ID of the validator control is the
        /// same as the control ID with a suffix of "_DTRV".  This also makes
        /// the range validator invisible if it is not being used.
        /// <p/>If the control does not have auto-postback enabled, client-side
        /// script is enabled, and it is being rendered in an
        /// <see cref="EWSTextBox.CanRenderUpLevel">up-level</see> browser,
        /// the control will render client-side script that enables several
        /// special keys and auto-formatting.  It will also render a calendar
        /// image to the right of the control that can be clicked to invoke
        /// a popup calendar that can be used to select a date.</remarks>
        /// <param name="e">The event arguments</param>
        protected override void OnPreRender(System.EventArgs e)
        {
            base.OnPreRender(e);
            rvRange.ControlToValidate = this.ID;
            rvRange.ID = this.ID + "_DTRV";

            // Format the error message if necessary
            if(rvRange.ErrorMessage.Length == 0)
                rvRange.ErrorMessage = CtrlUtils.ReplaceParameters(
                    this.DateErrorMessage, "{MinDate}", rvRange.MinimumValue,
                    "{MaxDate}", rvRange.MaximumValue);

            // Register formatting and popup calendar script?
            if(!this.AutoPostBack && this.EnableClientScript &&
              this.CanRenderUpLevel)
            {
                this.Attributes["onblur"] = "javascript:DTB_FormatDate(this);";
                this.Attributes["onkeydown"] = "javascript:return DTB_DateKeys(event.srcElement, event.keyCode);";
                this.Attributes["ondblclick"] = "javascript:return " +
                    "DTB_PopupCal('" + this.ClientID + "');";

                // Add the popup calendar image control
                strCalPopup = "&nbsp;<img src=\"" +
                    this.Page.ClientScript.GetWebResourceUrl(
                    typeof(DateTextBox), CtrlUtils.HtmlPath + "Calendar.bmp") +
                    "\" alt=\"Popup Calendar\" " +
                    "style=\"cursor: hand; position: relative; top: 3px;\" onclick=\"javascript: " +
                    "DTB_PopupCal('" + this.ClientID + "'); return false;\">";

                if(!this.Page.ClientScript.IsClientScriptBlockRegistered("EWS_DTBFormat"))
                    this.Page.ClientScript.RegisterClientScriptInclude(
                        typeof(DateTextBox), "EWS_DTBFormat",
                        this.Page.ClientScript.GetWebResourceUrl(
                        typeof(DateTextBox),
                        CtrlUtils.ScriptsPath + "DateTextBox.js"));
            }
        }

		/// <summary>
		/// Render this control to the output parameter specified.
		/// </summary>
		/// <param name="writer">The HTML writer to which the ouput is written</param>
		protected override void Render(HtmlTextWriter writer)
        {
			base.Render(writer);

            // Write out the popup calendar HTML if used
            if(strCalPopup != null)
                writer.Write(strCalPopup);

            if(rvRange != null)
                rvRange.RenderControl(writer);
		}

	    /// <summary>
        /// This returns the current value of the textbox as a
        /// <see cref="System.DateTime"/> object.
	    /// </summary>
        /// <returns>The time part defaults to 12:00am.  If empty, it returns
        /// a default <see cref="System.DateTime"/> object.</returns>
        public DateTime ToDateTime()
        {
            if(this.Text.Length == 0)
                return new DateTime();

            return DateTime.Parse(this.Text);
        }

        /// <summary>
        /// This is called by the control designer when rendering at
        /// design time.
        /// </summary>
        /// <remarks>At design-time, the popup calendar image is rendered
        /// as a simple lightgrey placeholder.</remarks>
        internal void RenderAtDesignTime()
        {
            // The actual image isn't available if the project hasn't been
            // compiled so just show an empty place holder image tag.
            strCalPopup = "&nbsp;<img height='16' width='16' " +
                "style='background-color: lightgrey;' />";
        }
	}
}
