//=============================================================================
// System  : ASP.NET Web Control Library
// File    : CompareTextBox.cs
// Author  : Eric Woodruff  (Eric@EWoodruff.us)
// Updated : Fri 02/20/04
// Note    : Copyright 2002-2004, Eric Woodruff, All rights reserved
// Compiler: Microsoft Visual C#
//
// This file contains a derived EWSTextBox class that can automatically
// generate a CompareValidator for itself that checks to see if the text
// meets comparison criteria with another control.  This is good for one
// comparison.  Additional comparisons to other fields and/or values will have
// to be added manually as regular CompareValidators on the page.
//
// Version     Date     Who  Comments
// ============================================================================
// 1.0.0    09/20/2002  EFW  Created the code
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
	/// This derived <see cref="EWSTextBox"/> class can automatically generate
    /// a <see cref="CompareValidator"/> for itself that checks to see if the
    /// text meets comparison criteria with another control or a value.
	/// </summary>
	[DefaultProperty("CompareErrorMessage"),
     ToolboxData("<{0}:CompareTextBox runat=\"server\" ControlToCompare=\"txtCtlID\" " +
        "Operator=\"Equal\" CompareErrorMessage=\"A comparison failed\" />"),
     AspNetHostingPermission(SecurityAction.LinkDemand,
        Level=AspNetHostingPermissionLevel.Minimal),
     AspNetHostingPermission(SecurityAction.InheritanceDemand,
        Level=AspNetHostingPermissionLevel.Minimal)]
	public class CompareTextBox : EWSoftware.Web.Controls.EWSTextBox
	{
        //=====================================================================
        // Private class members

        private CompareValidator cvComp;    // The validator

        //=====================================================================
        // Properties

	    /// <summary>
        /// This is the error message to display if the data entered does
        /// not match the specified comparison criteria.
	    /// </summary>
        /// <value>If not set, it defaults to "<b>A comparison failed</b>".</value>
		[Category("Appearance"), DefaultValue("A comparison failed"), Bindable(true),
         Description("The error message to display for invalid comparisons")]
		public string CompareErrorMessage
		{
			get
            {
                EnsureChildControls();
                return cvComp.ErrorMessage;
            }
			set
            {
                EnsureChildControls();
                cvComp.ErrorMessage = value;
            }
		}

	    /// <summary>
        /// Get/set the control to compare with this control.  Do not set both
        /// this and <see cref="ValueToCompare"/>.
	    /// </summary>
		[Category("Behavior"), Bindable(true),
         Description("The control to which this one should be compared")]
        public string ControlToCompare
        {
            get
            {
                EnsureChildControls();
                return cvComp.ControlToCompare;
            }
            set
            {
                EnsureChildControls();
                cvComp.ControlToCompare = value;
            }
        }

	    /// <summary>
        /// Get/set the value to compare with this control.  Do not set both
        /// this and <see cref="ControlToCompare"/>.
	    /// </summary>
		[Category("Behavior"), Bindable(true),
         Description("The value to which this control should be compared")]
        public string ValueToCompare
        {
            get
            {
                EnsureChildControls();
                return cvComp.ValueToCompare;
            }
            set
            {
                EnsureChildControls();
                cvComp.ValueToCompare = value;
            }
        }

	    /// <summary>
        /// Get/set the data type to use for the comparison
	    /// </summary>
		[Category("Behavior"), DefaultValue(ValidationDataType.String), Bindable(true),
         Description("The data type to use for the comparison")]
        public ValidationDataType CompareType
        {
            get
            {
                EnsureChildControls();
                return cvComp.Type;
            }
            set
            {
                EnsureChildControls();
                cvComp.Type = value;
            }
        }

	    /// <summary>
        /// Get/set the operator type to use for the comparison
	    /// </summary>
		[Category("Behavior"), Bindable(true),
         DefaultValue(ValidationCompareOperator.Equal),
         Description("The operator to use for the comparison")]
        public ValidationCompareOperator Operator
        {
            get
            {
                EnsureChildControls();
                return cvComp.Operator;
            }
            set
            {
                EnsureChildControls();
                cvComp.Operator = value;
            }
        }

        //=====================================================================
        // Methods, etc

        /// <summary>
        /// This is overridden to create the
        /// <see cref="System.Web.UI.WebControls.CompareValidator"/> used to
        /// validate the input.
        /// </summary>
        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            cvComp = new CompareValidator();
            cvComp.Enabled = this.Enabled;
            cvComp.ErrorMessage = "A comparison failed";

            // Use validation summary by default
            cvComp.Display = ValidatorDisplay.None;

            this.Controls.Add(cvComp);
        }

        /// <summary>
        /// OnPreRender is overridden to set the control to validate and the
        /// validation control IDs.
        /// </summary>
        /// <remarks>The ID of the control to validate and the validator's
        /// ID cannot be set earlier as we cannot guarantee the order in which
        /// the properties are set.  The ID of the validator control is the
        /// same as the control ID with a suffix of "_CV".  This also makes the
        /// comparison validator invisible if it is not being used.</remarks>
        /// <param name="e">The event arguments</param>
        protected override void OnPreRender(System.EventArgs e)
        {
            if(this.Visible)
                if(cvComp.ControlToCompare.Length == 0 &&
                  cvComp.ValueToCompare.Length == 0)
                    cvComp.Visible = false;
                else
                {
                    cvComp.Visible = true;
                    cvComp.ControlToValidate = this.ID;
                    cvComp.ID = this.ID + "_CV";
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

            // Render the validator if needed (may not exist at design time)
            if(cvComp != null && cvComp.Visible == true)
                cvComp.RenderControl(writer);
		}
	}
}
