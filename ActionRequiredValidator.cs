//=============================================================================
// System  : ASP.NET Web Control Library
// File    : ActionRequiredValidator.cs
// Author  : Eric Woodruff  (Eric@EWoodruff.us)
// Updated : Fri 02/20/04
// Note    : Copyright 2002-2004, Eric Woodruff, All rights reserved
// Compiler: Microsoft Visual C#
//
// This file contains a derived BaseValidator class that can be used to
// display an action required message (i.e. "Save your changes").
//
// Version     Date     Who  Comments
// ============================================================================
// 1.0.0    10/03/2002  EFW  Created the code
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
	/// This derived <see cref="System.Web.UI.WebControls.BaseValidator"/>
    /// class can be used to display an action required message (i.e.
    /// "Save your changes").
	/// </summary>
    /// <remarks>This validator differs from others in that you must
    /// explicitly set its <see cref="ActionRequired"/> property and later on
    /// explicitly check it in order for it to display its message.  This
    /// control's validation is always performed server-side.  There is no
    /// client-side implementation and shouldn't be as you could end up
    /// preventing the action you are requiring (i.e. clicking a Save button).
    /// Note that the <see cref="System.Web.UI.WebControls.BaseValidator.IsValid"/>
    /// property can be explicitly set to false to force it to show the
    /// message too.</remarks>
	[DefaultProperty("ActionRequired"),
	 ToolboxData("<{0}:ActionRequiredValidator runat=\"server\" " +
        "ErrorMessage=\"Action Required\" />"),
     Designer(typeof(BaseValidatorDesigner)),
     AspNetHostingPermission(SecurityAction.LinkDemand,
        Level=AspNetHostingPermissionLevel.Minimal),
     AspNetHostingPermission(SecurityAction.InheritanceDemand,
        Level=AspNetHostingPermissionLevel.Minimal)]
	public class ActionRequiredValidator : System.Web.UI.WebControls.BaseValidator
	{
        //=====================================================================
        // Properties

	    /// <summary>
        /// This property is used to specify whether or not action is
        /// required on the part of the user.
	    /// </summary>
        /// <value>Getting the current value sets the
        /// <see cref="System.Web.UI.WebControls.BaseValidator.IsValid"/>
        /// property based on whether or not the <b>ActionRequired</b>
        /// property is true or false.  If invalid (this property is still
        /// true and thus the action is still required), the associated
        /// error message is displayed.</value>
		[Category("Behavior"), DefaultValue(false), Bindable(true),
         Description("Specify whether or not action is required")]
		public bool ActionRequired
		{
			get
            {
	            Object oActionReq = ViewState["ActionRequired"];
                bool bActionReq = (oActionReq == null) ? false : (bool)oActionReq;

                this.IsValid = !bActionReq;
                return bActionReq;
            }
			set { ViewState["ActionRequired"] = value; }
		}

        //=====================================================================
        // Methods, etc

        /// <summary>
        /// Default constructor.  By default, the
        /// <see cref="System.Web.UI.WebControls.BaseValidator.Display"/>
        /// property is set to
        /// <see cref="System.Web.UI.WebControls.ValidatorDisplay">None</see>
        /// to display the error message in a
        /// <see cref="System.Web.UI.WebControls.ValidationSummary"/> control.
        /// </summary>
        public ActionRequiredValidator()
        {
            this.Display = ValidatorDisplay.None;
        }

        /// <summary>
        /// <b>EvaluateIsValid</b> always returns true.  Check the
        /// <see cref="ActionRequired"/> property in your server-side code
        /// to see if it's valid.
        /// </summary>
        /// <remarks>It is done this way to prevent the validator from
        /// blocking the action you are trying to have done (i.e. clicking
        /// a Save button).</remarks>
        /// <returns>Always true</returns>
        protected override bool EvaluateIsValid()
        {
            return true;
        }

        /// <summary>
        /// <b>ControlPropertiesValid</b> always returns true.  There's
        /// nothing in this control that requires any special handling.
        /// </summary>
        /// <returns>Always true</returns>
        protected override bool ControlPropertiesValid()
        {
            return true;
        }
    }
}
