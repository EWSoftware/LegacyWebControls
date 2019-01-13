//=============================================================================
// System  : ASP.NET Web Control Library
// File    : CreditCard.cs
// Author  : Eric Woodruff  (Eric@EWoodruff.us)
// Updated : 04/05/2007
// Note    : Copyright 2002-2007, Eric Woodruff, All rights reserved
// Compiler: Microsoft Visual C#
//
// This file contains a derived BaseValidator class that can be used to
// validate credit card numbers.  It checks the number based on the type and
// also that the number passes the checksum calculation (done using Luhn's
// Formula).  Client-side script validation support is available.
//
// Also included in here is the CreditCardTextBox, a CompareTextBox-derived
// class used for entering credit card numbers and a CCTypeDropDownList which
// is an EWSDropDownList-derived control that will set its selections based on
// the accepted credit card types of an associated CreditCardTextBox or
// CreditCardValidator control.  When the dropdown list is joined with one of
// the other controls, it offers an additional edit to confirm that the
// entered number is valid.
//
// Version     Date     Who  Comments
// ============================================================================
// 1.0.0    11/30/2002  EFW  Created the code
//=============================================================================

using System;
using System.Collections;
using System.ComponentModel;
using System.Security.Permissions;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.Design.WebControls;
using System.Web.UI.WebControls;

namespace EWSoftware.Web.Controls
{
    /// <summary>
    /// This enumerated type defines the valid card types recognized by
    /// the credit card controls.  It also defines a type for other valid,
    /// but unrecognized cards, another for unknown invalid types, and one
    /// for all valid types combined except <b>Other</b> and <b>Unknown</b>.
    /// </summary>
    /// <remarks>
    /// <b>NOTE:</b> If you change these constants, update the client-side
    /// JavaScript code too!
    /// </remarks>
    /// <seealso cref="CreditCardValidator"/>
    /// <seealso cref="CreditCardTextBox"/>
    /// <seealso cref="CCTypeDropDownList"/>
    [Flags, Serializable]
    public enum CardTypes
    {
        /// <summary>VISA starts with 4 and is 13 or 16 digits</summary>
        VISA        = 0x00000001,
        /// <summary>MasterCard starts with 51 through 55 and is 16 digits</summary>
        MasterCard  = 0x00000002,
        /// <summary>American Express starts with 34 or 37 and is 15 digits</summary>
        Amex        = 0x00000004,
        /// <summary>Discover starts with 6011 and is 16 digits</summary>
        Discover    = 0x00000008,
        /// <summary>Diners Club starts with 300-305, 36, or 38, and is
        /// 14 digits</summary>
        DinersClub  = 0x00000010,
        /// <summary>enRoute starts with 2014 or 2149 and is 15 digits</summary>
        enRoute     = 0x00000020,
        /// <summary>JCB starts with 3 and is 16 digits or starts with 2131
        /// or 1800 and is 15 digits</summary>
        JCB         = 0x00000040,
        /// <summary>Catch-all for valid but unknown types.  It passed
        /// the checksum test but was not recognized as one of the above
        /// types.</summary>
        Other       = 0x01000000,
        /// <summary>Invalid type or number</summary>
        Unknown     = 0x02000000,
        /// <summary>All recognized valid types except <b>Other</b>
        /// and <b>Unknown</b></summary>
        All         = CardTypes.VISA | CardTypes.MasterCard | CardTypes.Amex |
                      CardTypes.Discover | CardTypes.DinersClub |
                      CardTypes.enRoute | CardTypes.JCB
    }

    /// <summary>
    /// This derived <see cref="System.Web.UI.WebControls.BaseValidator"/>
    /// class can be used to validate credit card numbers.  It checks the
    /// number based on the type and also that the number passes the checksum
    /// calculation (done using <b>Luhn's Formula</b>).  Client-side script
    /// validation support is available.
    /// </summary>
    /// <remarks>The <b>ControlToValidate</b> property should be set to the
    /// <see cref="System.Web.UI.WebControls.TextBox"/> control that contains
    /// the card number to validate. </remarks>
    /// <seealso cref="CardTypes"/>
    /// <seealso cref="CreditCardTextBox"/>
    /// <seealso cref="CCTypeDropDownList"/>
	[DefaultProperty("AcceptedCardTypes"),
     ToolboxData("<{0}:CreditCardValidator runat=\"server\" " +
        "ErrorMessage=\"Invalid card number\" />"),
     Designer(typeof(BaseValidatorDesigner)),
     AspNetHostingPermission(SecurityAction.LinkDemand,
        Level=AspNetHostingPermissionLevel.Minimal),
     AspNetHostingPermission(SecurityAction.InheritanceDemand,
        Level=AspNetHostingPermissionLevel.Minimal)]
    public class CreditCardValidator : System.Web.UI.WebControls.BaseValidator
    {
        //=====================================================================
        // Properties

	    /// <summary>
        /// This property determines what card types can be accepted by the
        /// input control.
	    /// </summary>
        /// <value>If not set, it returns <see cref="CardTypes">All</see> and
        /// will accept all valid card types except
        /// <see cref="CardTypes">Other</see>.</value>
		[Category("Behavior"), DefaultValue(CardTypes.All), Bindable(true),
          Description("Specify what credit card types are accepted")]
		public CardTypes AcceptedCardTypes
		{
			get
            {
	            Object oTypes = ViewState["AcceptedCardTypes"];
	            return (oTypes == null) ? CardTypes.All : (CardTypes)oTypes;
            }
			set
            {
                ViewState["AcceptedCardTypes"] = value;
            }
		}

        /// <summary>
        /// Same as the <see cref="AcceptedCardTypes"/> property but it takes
        /// a string that will be parsed into the accepted types.
        /// </summary>
        /// <value>If not set, it returns <see cref="CardTypes">All</see>
        /// as a string value.</value>
        [Category("Behavior"), DefaultValue("All"), Bindable(true),
          Description("Specify comma separated list of accepted card types")]
        public string AcceptedCardTypesString
        {
            get
            {
                Object oTypes = ViewState["AcceptedCardTypes"];
                return (oTypes == null) ? CardTypes.All.ToString() :
                    ((CardTypes)oTypes).ToString();
            }
            set
            {
                ViewState["AcceptedCardTypes"] = (CardTypes)Enum.Parse(
                        typeof(CardTypes), value, true);
            }
        }

        /// <summary>
        /// This returns the card type after validation has occurred.
        /// </summary>
        /// <value>If not validated or it is invalid, this will return
        /// the type <see cref="CardTypes">Unknown</see>.</value>
		[Category("Format"), Browsable(false),
         Description("The current card type detected")]
        public CardTypes CurrentType
        {
			get
            {
	            Object oType = ViewState["CurrentType"];
	            return (oType == null) ? CardTypes.Unknown : (CardTypes)oType;
            }
        }

        /// <summary>
        /// This is used to set the <see cref="CCTypeDropDownList"/> control
        /// with which the validator is associated (if any).
        /// </summary>
        /// <value>If set, the validator will check that the current card
        /// number type matches the selected card type in the dropdown list
        /// when validation occurs.</value>
        [Category("Behavior"), Bindable(true),
         Description("Specify the ID of the credit card type " +
            "dropdown list with which this validator is associated")]
        public string CardTypeCtlID
        {
            get
            {
                Object oID = ViewState["CardTypeCtlID"];
                return (oID == null) ? null : (string)oID;
            }
            set
            {
                ViewState["CardTypeCtlID"] = value;
            }
        }

        //=====================================================================
        // Methods, etc

        /// <summary>
        /// This is overridden to handle validating the credit card number.
        /// </summary>
        /// <returns>If the control to validate is empty, this will always
        /// return true.  If not empty, the value is validated based on the
        /// card type detected.  If a <see cref="CCTypeDropDownList"/> control
        /// has been specified via the <see cref="CardTypeCtlID"/> property,
        /// it also confirms that the card number type matches the one selected
        /// in the dropdown list.  It returns true if the number is valid or
        /// false if not.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">This is thrown
        /// if the <see cref="CCTypeDropDownList"/> control specified via the
        /// <see cref="CardTypeCtlID"/> property cannot be found.</exception>
        /// <exception cref="System.ArgumentException">This is thrown if the
        /// control specified by the <see cref="CardTypeCtlID"/> is not a
        /// <see cref="CCTypeDropDownList"/> control.</exception>
        protected override bool EvaluateIsValid()
        {
            int    ct;
            bool   bRetVal;
            string strCardNbr = this.GetControlValidationValue(ControlToValidate);

            // If the control is empty, it will return true
            if(strCardNbr == null || strCardNbr.Length == 0)
                return true;

            // Check the number based on the card type and see if
            // it passes the checksum test.
            bRetVal = this.ValidateCardNumber(strCardNbr);

            // Is there a card type control to check?  If so, do it.
            string strTypeCtl = this.CardTypeCtlID;
            if(bRetVal && strTypeCtl != null && strTypeCtl.Length != 0)
            {
                // Throw an exception if the associated control is not found
                Control ctl = this.Parent.FindControl(CardTypeCtlID);
                if(ctl == null)
                    throw new ArgumentOutOfRangeException("CardTypeCtlID",
                        strTypeCtl, "The specified control was not found");

                CCTypeDropDownList ccdl = ctl as CCTypeDropDownList;

                // Throw an exception if it isn't of the expected type
                if(ccdl == null)
                    throw new ArgumentException("CardTypeCtlID should be " +
                        "a CCTypeDropDownList control");

                ct = Int32.Parse(ccdl.CurrentSelection);

                bRetVal = ((CardTypes)ct == this.CurrentType);
            }

            return bRetVal;
        }

        /// <summary>
        /// This is overridden to add the client-side script if needed.
        /// </summary>
        /// <remarks>Client-side script is only rendered if the control is
        /// visible and it is being rendered on an
        /// <see cref="System.Web.UI.WebControls.BaseValidator.RenderUplevel">
        /// up-level</see> browser.</remarks>
        /// <param name="e">The event arguments</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if(this.Visible && this.RenderUplevel &&
              !this.Page.ClientScript.IsClientScriptBlockRegistered("EWS_CCVal"))
            {
                // Register the script resource
                this.Page.ClientScript.RegisterClientScriptInclude(
                    typeof(CreditCardValidator), "EWS_CCVal",
                    this.Page.ClientScript.GetWebResourceUrl(
                    typeof(CreditCardValidator),
                    CtrlUtils.ScriptsPath + "CCValidator.js"));
            }
        }

        /// <summary>
        /// This is overridden to add the evaluation function and card type
        /// attributes to the rendered HTML of the control.
        /// </summary>
        /// <remarks>Client-side script is only rendered if the control is
        /// visible and it is being rendered on an
        /// <see cref="System.Web.UI.WebControls.BaseValidator.RenderUplevel">
        /// up-level</see> browser.</remarks>
        /// <param name="writer">The HTML text writer to which output is written</param>
        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            base.AddAttributesToRender(writer);
            if(this.RenderUplevel)
            {
                writer.AddAttribute("evaluationfunction",
                    "CCV_ValidatorEvaluateIsValid");
                writer.AddAttribute("cardtypes",
                    ((Int32)this.AcceptedCardTypes).ToString());

                string strTypeCtl = this.CardTypeCtlID;
                if(strTypeCtl == null)
                    strTypeCtl = String.Empty;

                writer.AddAttribute("cardtypectl", strTypeCtl);
            }
        }

        /// <summary>Determine the card type and test the checksum.</summary>
        /// <remarks>This method will set the <see cref="CurrentType"/>
        /// property to the detected card type.
        /// <p/><b>NOTE:</b> If you change the regular expressions used in
        /// this method to detect the card types, update the client-side
        /// JavaScript validation code too!</remarks>
        /// <param name="strCardNbr">The card number to validate</param>
        /// <returns>Returns true unless the card number fails the checksum
        /// test or is not recognized as one of the acceptable card types.
        /// In that case, it will return false.  However, if accepting
        /// <see cref="CardTypes">Other</see> as a card type, it will rely
        /// solely on the result of the checksum test of the card number.
        /// </returns>
        public bool ValidateCardNumber(string strCardNbr)
        {
            CardTypes ctTypes = this.AcceptedCardTypes;

            // VISA starts with 4 and is 13 or 16 digits
            if((ctTypes & CardTypes.VISA) != 0 &&
              Regex.IsMatch(strCardNbr, @"^(4\d{12})|(4\d{15})$"))
            {
                ViewState["CurrentType"] = CardTypes.VISA;
                return CreditCardValidator.TestChecksum(strCardNbr);
            }

            // MasterCard starts with 51 through 55 and is 16 digits
            if((ctTypes & CardTypes.MasterCard) != 0 &&
              Regex.IsMatch(strCardNbr, @"^5[1-5]\d{14}$"))
            {
                ViewState["CurrentType"] = CardTypes.MasterCard;
                return CreditCardValidator.TestChecksum(strCardNbr);
            }

            // AMEX starts with 34 or 37 and is 15 digits
            if((ctTypes & CardTypes.Amex) != 0 &&
              Regex.IsMatch(strCardNbr, @"^3[47]\d{13}$"))
            {
                ViewState["CurrentType"] = CardTypes.Amex;
                return CreditCardValidator.TestChecksum(strCardNbr);
            }

            // Discover starts with 6011 and is 16 digits
            if((ctTypes & CardTypes.Discover) != 0 &&
              Regex.IsMatch(strCardNbr, @"^6011\d{12}$"))
            {
                ViewState["CurrentType"] = CardTypes.Discover;
                return CreditCardValidator.TestChecksum(strCardNbr);
            }

            // Diners Club starts with 300-305, 36, or 38, and is 14 digits
            if((ctTypes & CardTypes.DinersClub) != 0 &&
              Regex.IsMatch(strCardNbr, @"^(30[0-5]\d{11})|(3[68]\d{12})$"))
            {
                ViewState["CurrentType"] = CardTypes.DinersClub;
                return CreditCardValidator.TestChecksum(strCardNbr);
            }

            // enRoute starts with 2014 or 2149 and is 15 digits
            if((ctTypes & CardTypes.enRoute) != 0 &&
              Regex.IsMatch(strCardNbr, @"^(2014|2149)\d{11}"))
            {
                ViewState["CurrentType"] = CardTypes.enRoute;
                return CreditCardValidator.TestChecksum(strCardNbr);
            }

            // JCB starts with 3 and is 16 digits or
            // starts with 2131 or 1800 and is 15 digits.
            if((ctTypes & CardTypes.JCB) != 0 &&
              Regex.IsMatch(strCardNbr, @"^(3\d{15})|((2131|1800)\d{11})$"))
            {
                ViewState["CurrentType"] = CardTypes.JCB;
                return CreditCardValidator.TestChecksum(strCardNbr);
            }

            // Couldn't recognize anything to this point.  Are we accepting
            // Other as a card type?  If so, rely solely on the number passing
            // the checksum validation.
            if((ctTypes & CardTypes.Other) != 0)
            {
                ViewState["CurrentType"] = CardTypes.Other;
                return CreditCardValidator.TestChecksum(strCardNbr);
            }

            // Not valid
            ViewState["CurrentType"] = CardTypes.Unknown;
            return false;
        }

        /// <summary>
        /// This performs a checksum validation on the card number using
        /// <b>Luhn's Formula</b>.
        /// </summary>
        /// <remarks><b>NOTE:</b> If you change the algorithm used in
        /// this method to calculate the checksum, update the client-side
        /// JavaScript validation code too!</remarks>
        /// <param name="strCardNbr">The card number to be checksummed</param>
        /// <returns>True if it passes the checksum test, false if not</returns>
        /// <exception cref="ArgumentNullException">This is thrown if the
        /// credit card number parameter is null.</exception>
        public static bool TestChecksum(string strCardNbr)
        {
            int nDigit, nProdDigit, nChecksum = 0;
            string strProduct;

            if(strCardNbr == null)
                throw new ArgumentNullException("strCardNbr",
                    "The card number cannot be null");

            // Work from right to left
            for(nDigit = strCardNbr.Length - 1; nDigit >= 0; nDigit--)
            {
                // Add the value of this digit
                nChecksum += (int)strCardNbr[nDigit] - 48;
                nDigit--;

                if(nDigit >= 0)
                {
                    // For each alternating digit, add the sum of the digits
                    // for the alternating digit value times two.
                    strProduct = (((int)strCardNbr[nDigit] - 48) * 2).ToString();

                    for(nProdDigit = 0; nProdDigit < strProduct.Length; nProdDigit++)
                        nChecksum += (int)strProduct[nProdDigit] - 48;
                }
            }

            // It's valid if the mod 10 value is zero
            return (nChecksum % 10 == 0);
        }
    }

    /// <summary>
	/// This derived <see cref="CompareTextBox"/> class can generate a
	/// <see cref="CreditCardValidator"/> for itself to insure that the text
	/// entered matches one of a set of acceptable credit card types.
	/// </summary>
    /// <seealso cref="CardTypes"/>
    /// <seealso cref="CreditCardValidator"/>
    /// <seealso cref="CCTypeDropDownList"/>
	[DefaultProperty("CardErrorMessage"),
     ToolboxData("<{0}:CreditCardTextBox runat=\"server\" " +
        "CardErrorMessage=\"Invalid card number\" />"),
     AspNetHostingPermission(SecurityAction.LinkDemand,
        Level=AspNetHostingPermissionLevel.Minimal),
     AspNetHostingPermission(SecurityAction.InheritanceDemand,
        Level=AspNetHostingPermissionLevel.Minimal)]
	public class CreditCardTextBox : EWSoftware.Web.Controls.CompareTextBox
	{
        //=====================================================================
        // Private class members
        private CreditCardValidator ccVal;  // The validator

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
        /// This is the error message to display if the credit card number
        /// entered is not acceptable.
	    /// </summary>
        /// <value>If not set, it defaults to "<b>Invalid card number</b>"</value>
		[Category("Appearance"), Bindable(true),
         DefaultValue("Invalid card number"),
         Description("The error message to display for invalid card numbers")]
		public string CardErrorMessage
		{
			get
            {
                EnsureChildControls();
                return ccVal.ErrorMessage;
            }
			set
            {
                EnsureChildControls();
                ccVal.ErrorMessage = value;
            }
		}

	    /// <summary>
        /// This property determines what card types can be accepted by the
        /// control.
	    /// </summary>
        /// <value>If not set, it returns <see cref="CardTypes">All</see> and
        /// will accept all valid card types except
        /// <see cref="CardTypes">Other</see>.</value>
		[Category("Behavior"), DefaultValue(CardTypes.All), Bindable(true),
          Description("Determines what credit card types are accepted")]
		public CardTypes AcceptedCardTypes
		{
			get
            {
	            EnsureChildControls();
	            return ccVal.AcceptedCardTypes;
            }
			set
            {
                EnsureChildControls();
                ccVal.AcceptedCardTypes = value;
            }
		}

        /// <summary>
        /// Same as the <see cref="AcceptedCardTypes"/> property but it takes
        /// a string that will be parsed into the accepted types.
        /// </summary>
        /// <value>If not set, it returns <see cref="CardTypes">All</see>
        /// as a string value.</value>
        [Category("Behavior"), DefaultValue("All"), Bindable(true),
          Description("Specify comma separated list of accepted card types")]
        public string AcceptedCardTypesString
        {
            get
            {
                EnsureChildControls();
                return ccVal.AcceptedCardTypesString;
            }
            set
            {
                EnsureChildControls();
                ccVal.AcceptedCardTypesString = value;
            }
        }

        /// <summary>
        /// This returns the card type after validation has occurred.
        /// </summary>
        /// <value>If not validated or it is invalid, this will return
        /// the type <see cref="CardTypes">Unknown</see>.</value>
		[Category("Format"), Browsable(false),
         Description("The current card type detected")]
        public CardTypes CurrentType
        {
			get
            {
	            EnsureChildControls();
	            return ccVal.CurrentType;
            }
        }

        /// <summary>
        /// This is used to set the <see cref="CCTypeDropDownList"/> control
        /// with which the credit card text box is associated (if any).
        /// </summary>
        /// <value>If set, the control will check that the current card
        /// number type matches the selected card type in the dropdown list
        /// when validation occurs.</value>
        [Category("Behavior"), Bindable(true),
        Description("Specify the ID of the credit card type " +
            "dropdown list with which this validator is associated")]
        public string CardTypeCtlID
        {
            get
            {
                EnsureChildControls();
                return ccVal.CardTypeCtlID;
            }
            set
            {
                EnsureChildControls();
                ccVal.CardTypeCtlID = value;
            }
        }

        //=====================================================================
        // Methods, etc

        /// <summary>
        /// Constructor.  Default state: Text is <see cref="EWSTextBox.Trim">
        /// trimmed</see>.  The control size and input length are limited to
        /// a maximum of sixteen characters.
        /// </summary>
        public CreditCardTextBox()
        {
            MaxLength = Columns = 16;
        }

        /// <summary>
        /// This is overridden to create the <see cref="CreditCardValidator"/>
        /// used to validate the input.
        /// </summary>
        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            ccVal = new CreditCardValidator();
            ccVal.Enabled = this.Enabled;
            ccVal.ErrorMessage = "Invalid card number";

            // Use validation summary by default
            ccVal.Display = ValidatorDisplay.None;

            this.Controls.Add(ccVal);
        }

        /// <summary>
        /// OnPreRender is overridden to set the control to validate and the
        /// validation control IDs.
        /// </summary>
        /// <remarks>The ID of the control to validate and the validator's
        /// ID cannot be set earlier as we cannot guarantee the order in which
        /// the properties are set.  The ID of the validator control is the
        /// same as the control ID with a suffix of "_CCVAL".</remarks>
        /// <param name="e">The event arguments</param>
        protected override void OnPreRender(System.EventArgs e)
        {
            base.OnPreRender(e);
            ccVal.ControlToValidate = this.ID;
            ccVal.ID = this.ID + "_CCVAL";
        }

		/// <summary>
		/// Render this control to the output parameter specified.
		/// </summary>
		/// <param name="writer">The HTML writer to which the output is written</param>
		protected override void Render(HtmlTextWriter writer)
		{
			base.Render(writer);

            if(ccVal != null)
                ccVal.RenderControl(writer);
		}
	}

    /// <summary>
    /// This derived <see cref="EWSDropDownList"/> class will load its items
    /// collection with the card types accepted by the associated
    /// <see cref="CreditCardTextBox"/> or <see cref="CreditCardValidator"/>
    /// control.
    /// </summary>
    /// <seealso cref="CardTypes"/>
    /// <seealso cref="CreditCardValidator"/>
    /// <seealso cref="CreditCardTextBox"/>
    [DefaultProperty("CreditCardCtlID"),
     ToolboxData("<{0}:CCTypeDropDownList runat=\"server\" " +
        "CreditCardCtlID=\"txtCardNbr\" />"),
     AspNetHostingPermission(SecurityAction.LinkDemand,
        Level=AspNetHostingPermissionLevel.Minimal),
     AspNetHostingPermission(SecurityAction.InheritanceDemand,
        Level=AspNetHostingPermissionLevel.Minimal)]
    public class CCTypeDropDownList : EWSoftware.Web.Controls.EWSDropDownList
    {
        //=====================================================================
        // Properties

        /// <summary>
        /// This is used to set the <see cref="CreditCardTextBox"/> or the
        /// <see cref="CreditCardValidator"/> control with which the card
        /// type dropdown list is associated.
        /// </summary>
        [Category("Behavior"), Bindable(true),
          Description("Specify the ID of the credit card text " +
              "box with which this list is associated")]
        public string CreditCardCtlID
        {
            get
            {
                Object oID = ViewState["CreditCardCtlID"];
                return (oID == null) ? null : (string)oID;
            }
            set
            {
                ViewState["CreditCardCtlID"] = value;
            }
        }

        //=====================================================================
        // Methods, etc

        /// <summary>
        /// This is overridden to load the items collection with the types
        /// accepted by the associated credit card text box or validator.
        /// </summary>
        /// <param name="e">The event arguments</param>
        /// <exception cref="System.ArgumentOutOfRangeException">This is thrown
        /// if the control specified via the <see cref="CreditCardCtlID"/>
        /// property cannot be found.</exception>
        /// <exception cref="System.ArgumentException">This is thrown if the
        /// control specified by the <see cref="CreditCardCtlID"/> property is
        /// not a <see cref="CreditCardValidator"/> or a
        /// <see cref="CreditCardTextBox"/> control.</exception>
        protected override void OnPreRender(System.EventArgs e)
        {
            int nLastType = 0, ct, nBit;

            base.OnPreRender(e);

            // Throw an exception if the associated control is not found
            Control ctl = this.Parent.FindControl(CreditCardCtlID);
            if(ctl == null)
                throw new ArgumentOutOfRangeException("CreditCardCtlID",
                    CreditCardCtlID, "The specified control was not found");

            CreditCardValidator ccv = ctl as CreditCardValidator;

            // Throw an exception if it isn't of the expected type
            if(ccv != null)
                ct = (int)(ccv.AcceptedCardTypes & (CardTypes.All |
                    CardTypes.Other));
            else
            {
                CreditCardTextBox cctb = ctl as CreditCardTextBox;

                if(cctb != null)
                    ct = (int)(cctb.AcceptedCardTypes & (CardTypes.All |
                        CardTypes.Other));
                else
                    throw new ArgumentException("CreditCardCtlID should be " +
                        "a CreditCardValidator or CreditCardTextBox control");
            }

            Object oTypes = ViewState["LastCCTypes"];
            if(oTypes != null)
                nLastType = (int)oTypes & (int)(CardTypes.All | CardTypes.Other);

            // If the accepted types didn't change, don't reload the collection
            if(nLastType != ct)
            {
                Items.Clear();

                for(nBit = 1; nBit < (int)CardTypes.Other; nBit <<= 1)
                    if((ct & nBit) != 0)
                        Items.Add(new ListItem(((CardTypes)nBit).ToString(),
                            nBit.ToString()));

                // Save the last set of types.  The list won't need changing
                // unless the types change.
                ViewState["LastCCTypes"] = ct;
            }
        }
    }
}
