//=============================================================================
// System  : ASP.NET Web Control Library
// File    : PatternTextBox.cs
// Author  : Eric Woodruff  (Eric@EWoodruff.us)
// Updated : Fri 02/20/04
// Note    : Copyright 2002-2004, Eric Woodruff, All rights reserved
// Compiler: Microsoft Visual C#
//
// This file contains a derived CompareTextBox class that can automatically
// generate a regular expression validator for itself that checks to see if
// the data entered matches a common data type pattern or a user-defined
// pattern.  See the PatternType enumeration for the supported types.
//
// Version     Date     Who  Comments
// ============================================================================
// 1.0.0    09/05/2002  EFW  Created the code
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
    /// This enumerated type defines the various pattern types supported
    /// by the <see cref="PatternTextBox"/> control.
    /// </summary>
    /// <remarks>
    /// The <b>StPosProv</b> setting accepts the following values in
    /// addition to US state abbreviations.<br/><br/>
    /// <list type="table">
    ///     <listheader>
    ///         <term>Abbreviation</term>
    ///         <description>Possession</description>
    ///     </listheader>
    ///     <item>
    ///         <term>AS</term>
    ///         <description>American Samoa</description>
    ///     </item>
    ///     <item>
    ///         <term>FM</term>
    ///         <description>Federated States Of Micronesia</description>
    ///     </item>
    ///     <item>
    ///         <term>GU</term>
    ///         <description>Guam</description>
    ///     </item>
    ///     <item>
    ///         <term>MH</term>
    ///         <description>Marshall Islands</description>
    ///     </item>
    ///     <item>
    ///         <term>MP</term>
    ///         <description>Northern Mariana Islands</description>
    ///     </item>
    ///     <item>
    ///         <term>PR</term>
    ///         <description>Puerto Rico</description>
    ///     </item>
    ///     <item>
    ///         <term>PW</term>
    ///         <description>Palau</description>
    ///     </item>
    ///     <item>
    ///         <term>VI</term>
    ///         <description>Virgin Islands</description>
    ///     </item>
    /// </list>
    /// <br/>
    /// <list type="table">
    ///     <listheader>
    ///         <term>Abbreviation</term>
    ///         <description>Military "State"</description>
    ///     </listheader>
    ///     <item>
    ///         <term>AA</term>
    ///         <description>Armed Forces Americas (except Canada)</description>
    ///     </item>
    ///     <item>
    ///         <term>AE</term>
    ///         <description>Armed Forces Africa, Canada, Europe, Middle East</description>
    ///     </item>
    ///     <item>
    ///         <term>AP</term>
    ///         <description>Armed Forces Pacific</description>
    ///     </item>
    /// </list>
    /// <br/>
    /// <list type="table">
    ///     <listheader>
    ///         <term>Abbreviation</term>
    ///         <description>Canadian Province</description>
    ///     </listheader>
    ///     <item>
    ///         <term>AB</term>
    ///         <description>Alberta</description>
    ///     </item>
    ///     <item>
    ///         <term>BC</term>
    ///         <description>British Columbia</description>
    ///     </item>
    ///     <item>
    ///         <term>MB</term>
    ///         <description>Manitoba</description>
    ///     </item>
    ///     <item>
    ///         <term>NB</term>
    ///         <description>New Brunswick</description>
    ///     </item>
    ///     <item>
    ///         <term>NF</term>
    ///         <description>Newfoundland</description>
    ///     </item>
    ///     <item>
    ///         <term>NT</term>
    ///         <description>Northwest Territories</description>
    ///     </item>
    ///     <item>
    ///         <term>NS</term>
    ///         <description>Nova Scotia</description>
    ///     </item>
    ///     <item>
    ///         <term>ON</term>
    ///         <description>Ontario</description>
    ///     </item>
    ///     <item>
    ///         <term>PE</term>
    ///         <description>Prince Edward Island</description>
    ///     </item>
    ///     <item>
    ///         <term>QC</term>
    ///         <description>Quebec</description>
    ///     </item>
    ///     <item>
    ///         <term>SK</term>
    ///         <description>Saskatchewan</description>
    ///     </item>
    ///     <item>
    ///         <term>YT</term>
    ///         <description>Yukon</description>
    ///     </item>
    /// </list>
    /// </remarks>
    public enum PatternType
    {
        /// <summary>Phone Number, area code and extension are optional.
        /// Parentheses around the area code are optional.  A dash or space
        /// can appear between the area code and the phone number.  The
        /// extension can be separated from the phone number by a space or
        /// lowercase 'x'.</summary>
        Phone = 0,
        /// <summary>Phone Number, area code optional, no extension allowed.
        /// Parentheses around the area code are optional.  A dash or space
        /// can appear between the area code and the phone number.
        /// <see cref="PatternTextBox"/> will limit its length to fourteen
        /// characters when using this pattern type.</summary>
        PhoneNoExt,
        /// <summary>Phone Number, area code required, extension optional.
        /// Parentheses around the area code are optional.  A dash or space
        /// can appear between the area code and the phone number.  The
        /// extension can be separated from the phone number by a space or
        /// lowercase 'x'.</summary>
        AreaPhone,
        /// <summary>Phone Number, area code required, no extension allowed.
        /// Parentheses around the area code are optional.  A dash or space
        /// can appear between the area code and the phone number.
        /// <see cref="PatternTextBox"/> will limit its length to fourteen
        /// characters when using this pattern type.</summary>
        AreaPhNoExt,
        /// <summary>E-Mail address. A domain name or IP address can follow
        /// the '@' symbol.</summary>
        EMail,
        /// <summary>A URL with or without a protocol specifier (HTTP,
        /// HTTPS, or FTP)</summary>
        URL,
        /// <summary>A UNC path. The path following the server name can
        /// include spaces.</summary>
        UNC,
        /// <summary>IPv4 address (i.e. 127.0.0.1). <see cref="PatternTextBox"/>
        /// will limit its length to fifteen characters when using this
        /// pattern type.</summary>
        IPv4Address,
        /// <summary>IPv6 address (i.e. FF02:3::5). Supports entry of the
        /// full address or one with zero compression. <see cref="PatternTextBox"/>
        /// will limit its length to 39 characters when using this pattern
        /// type.</summary>
        IPv6Address,
        /// <summary>US state abbreviation only (including Washington DC).
        /// <see cref="PatternTextBox"/> will limit its length to two
        /// characters and set the <see cref="EWSTextBox.Casing"/> property
        /// to <see cref="CasingMode">Upper</see> when using this pattern type.</summary>
        State,
        /// <summary>US state/possession/military state or Canadian province.
        /// <see cref="PatternTextBox"/> will limit its length to two
        /// characters and set the <see cref="EWSTextBox.Casing"/> property to
        /// <see cref="CasingMode">Upper</see> when using this pattern type.
        /// See <b>Remarks</b> for additional values accepted besides US
        /// states.</summary>
        StPosProv,
        /// <summary>US ZIP code (5 digit or ZIP+4 format).  If the +4 digits
        /// are specified, they must be separated from the first five digits
        /// by a dash. <see cref="PatternTextBox"/> will limit its length to
        /// 10 characters when using this pattern type.</summary>
        ZIP,
        /// <summary>US ZIP code (ZIP+4 format only).  The +4 digits must be
        /// separated from the first five digits by a dash.
        /// <see cref="PatternTextBox"/> will limit its length to 10
        /// characters when using this pattern type.</summary>
        ZIP4,
        /// <summary>US ZIP code (5 digit format only). <see cref="PatternTextBox"/>
        /// will limit its length to 5 characters when using this pattern type.</summary>
        ZIP5,
        /// <summary>Social Security Number.  Dashes must separate the
        /// sections (i.e. 000-00-0000). <see cref="PatternTextBox"/> will
        /// limit its length to 11 characters when using this pattern type.</summary>
        SSN,
        /// <summary>Time of day (24 hour or AM/PM format).  For AM/PM format,
        /// the AM or PM can be in upper, lower, or mixed case but must be
        /// separated from the time value by a space.  For hours and minutes
        /// less than ten, leading zeros are optional. <see cref="PatternTextBox"/>
        /// will limit its length to 8 characters when using this pattern
        /// type. The <see cref="PatternTextBox.ToDateTime"/> method can be
        /// used to convert the entered time text to a
        /// <see cref="System.DateTime"/> value.</summary>
        Time,
        /// <summary>Bit field (Y, N, T, F, yes, no, true, false, 0, 1, -1).
        /// <see cref="PatternTextBox"/> will limit its length to 5 characters
        /// when using this pattern type. The <see cref="PatternTextBox.ToBoolean"/>
        /// method can be used to convert the entered text to a
        /// <see cref="System.Boolean"/> value.</summary>
        YesNo,
        /// <summary>A custom format set via the
        /// <see cref="PatternTextBox.PatternRegExp"/> property.
        /// <see cref="PatternTextBox"/> will automatically set
        /// <see cref="PatternTextBox.Pattern"/> to this value if a
        /// string is assigned to the <see cref="PatternTextBox.PatternRegExp"/>
        /// property.</summary>
        Custom
    }

    /// <summary>
	/// This derived CompareTextBox class can generate a
	/// RegularExpressionValidator for itself to insure that the text
	/// entered matches one of a set of common formats or a user-defined
	/// pattern.
	/// </summary>
    /// <include file='Doc/Controls.xml'
    /// path='Controls/PatternTextBox/Member[@name="Class"]/*' />
	[DefaultProperty("Pattern"),
     ToolboxData("<{0}:PatternTextBox runat=\"server\" Pattern=\"Phone\" " +
        "PatternErrorMessage=\"Not valid for selected pattern\" />"),
     AspNetHostingPermission(SecurityAction.LinkDemand,
        Level=AspNetHostingPermissionLevel.Minimal),
     AspNetHostingPermission(SecurityAction.InheritanceDemand,
        Level=AspNetHostingPermissionLevel.Minimal)]
	public class PatternTextBox : EWSoftware.Web.Controls.CompareTextBox
	{
        //=====================================================================
        // Private class members
        private RegularExpressionValidator rePattern;   // The validator

        //=====================================================================
        // Properties

	    /// <summary>
        /// This is the error message to display if the data entered does
        /// not match the specified pattern.
	    /// </summary>
		[Category("Appearance"), Bindable(true),
            DefaultValue("Not valid for selected pattern"),
            Description("The error message to display for invalid data")]
		public string PatternErrorMessage
		{
			get
            {
                EnsureChildControls();
                return rePattern.ErrorMessage;
            }
			set
            {
                EnsureChildControls();
                rePattern.ErrorMessage = value;
            }
		}

	    /// <summary>
        /// This is the pattern to use for input comparison.  Note that some
        /// patterns also set the maximum length and number of columns for
        /// the text box.
	    /// </summary>
		[Category("Behavior"), DefaultValue(PatternType.Custom), Bindable(true),
         Description("The pattern type to use for data entry")]
        public PatternType Pattern
        {
			get
            {
	            Object oPattern = ViewState["PatternType"];
	            return (oPattern == null) ? PatternType.Custom : (PatternType)oPattern;
            }
            set
            {
                EnsureChildControls();

                // Match the value to one of the common types or pass it on
                // as-is.
                switch(value)
                {
                    case PatternType.Phone:
                        rePattern.ValidationExpression =
                            @"^(\(?\d{3}\)?(\s|-))?\d{3}-\d{4}((\s|x).*)?$";
                        break;

                    case PatternType.PhoneNoExt:
                        MaxLength = Columns = 14;
                        rePattern.ValidationExpression =
                            @"^(\(?\d{3}\)?(\s|-))?\d{3}-\d{4}$";
                        break;

                    case PatternType.AreaPhone:
                        rePattern.ValidationExpression =
                            @"^\(?\d{3}\)?(\s|-)\d{3}-\d{4}((\s|x).*)?$";
                        break;

                    case PatternType.AreaPhNoExt:
                        MaxLength = Columns = 14;
                        rePattern.ValidationExpression =
                            @"^\(?\d{3}\)?(\s|-)\d{3}-\d{4}$";
                        break;

                    case PatternType.EMail:
                        rePattern.ValidationExpression = @"^([a-zA-Z0-9_\-])" +
                            @"([a-zA-Z0-9_\-\.]*)@(\[((25[0-5]|2[0-4]" +
                            @"[0-9]|1[0-9][0-9]|[1-9][0-9]|[0-9])\.)" +
                            @"{3}|((([a-zA-Z0-9\-]+)\.)+))([a-zA-Z]" +
                            @"{2,}|(25[0-5]|2[0-4][0-9]|1[0-9][0-9]|"+
                            @"[1-9][0-9]|[0-9])\])$";
                        break;

                    case PatternType.URL:
                        rePattern.ValidationExpression =
                            @"^(((HTTP|http|HTTPS|https|FTP|ftp)://)|" +
                            @"(www\.))+[\w]+(.[\w]+)([\w\-\.,@?^=%&:/~\+#]*" +
                            @"[\w\-\@?^=%&/~\+#])?$";
                        break;

                    case PatternType.UNC:
                        rePattern.ValidationExpression =
                            @"^\\{2}\w+(\\([\w\-\.,@?^=%&:/~\+#\$ ]*)?)*$";
                        break;

                    case PatternType.IPv4Address:
                        MaxLength = Columns = 15;
                        rePattern.ValidationExpression = @"^((25[0-5]|2[0-4]" +
                            @"[0-9]|1[0-9][0-9]|[1-9][0-9]|[0-9])\.){3}" +
                            @"(25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9][0-9]|[0-9])$";
                        break;

                    case PatternType.IPv6Address:
                        MaxLength = Columns = 39;
                        rePattern.ValidationExpression = @"^(([\dA-Fa-f]" +
                            @"{1,4}:){7}[\dA-Fa-f]{1,4})|(([\dA-Fa-f]" +
                            @"{1,4}:){1,5}:(([\dA-Fa-f]{1,4}:){0,4})" +
                            @"[\dA-Fa-f]{1,4})$";
                        break;

                    case PatternType.State:
                        MaxLength = Columns = 2;
                        Casing = CasingMode.Upper;
                        rePattern.ValidationExpression =
                            @"^(([Aa][KLRZklrz])|([Cc][AOTaot])|([Dd][CEce])|" +
                            @"FL|fl|GA|ga|HI|hi|([Ii][ADLNadln])|([Kk][SYsy])|" +
                            @"LA|la|([Mm][ADEINOSTadeinost])|([Nn][CDEHJMVYcdehjmvy])|" +
                            @"([Oo][HKRhkr])|PA|pa|RI|ri|([Ss][CDcd])|([Tt][NXnx])|" +
                            @"UT|ut|([Vv][ATat])|([Ww][AIVYaivy]))$";
                        break;

                    case PatternType.StPosProv:
                        MaxLength = Columns = 2;
                        Casing = CasingMode.Upper;
                        rePattern.ValidationExpression =
                            @"^(([Aa][ABEKLPRSZabeklprsz])|BC|bc|([Cc][AOTaot])|" +
                            @"([Dd][CEce])|([Ff][LMlm])|([Gg][AUau])|HI|hi|" +
                            @"([Ii][ADLNadln])|([Kk][SYsy])|LA|la|ON|on|" +
                            @"([Mm][ABDEHINOPSTabdehinopst])|([Nn][BCDEFHJMSTVYbcdefhjmstvy])|" +
                            @"([Oo][HKRhkr])|([Pp][AERWaerw])|QC|qc|RI|ri|([Ss][CDKcdk])|" +
                            @"([Tt][NXnx])|UT|ut|([Vv][AITait])|([Ww][AIVYaivy])|YT|yt)$";
                        break;

                    case PatternType.ZIP:
                        MaxLength = Columns = 10;
                        rePattern.ValidationExpression = @"^\d{5}(-\d{4})?$";
                        break;

                    case PatternType.ZIP4:
                        MaxLength = Columns = 10;
                        rePattern.ValidationExpression = @"^\d{5}-\d{4}$";
                        break;

                    case PatternType.ZIP5:
                        MaxLength = Columns = 5;
                        rePattern.ValidationExpression = @"^\d{5}$";
                        break;

                    case PatternType.SSN:
                        MaxLength = Columns = 11;
                        rePattern.ValidationExpression = @"^\d{3}-\d{2}-\d{4}$";
                        break;

                    case PatternType.Time:
                        MaxLength = Columns = 8;
                        rePattern.ValidationExpression =
                            @"(^(\d|(0\d)|(1\d)|(2[0123])):(\d|([012345]\d))$)|" +
                            @"(^(\d|(0\d)|(1[012])):(\d|([012345]\d))\s?[aApP][mM]$)";
                        break;

                    case PatternType.YesNo:
                        MaxLength = Columns = 5;
                        rePattern.ValidationExpression =
                            @"^(([YyNnTtFf01])|(yes)|(Yes)|(YES)|(no)|" +
                            @"(No)|(NO)|(true)|(True)|(TRUE)|(false)|" +
                            @"(False)|(FALSE)|(-1))$";
                        break;

                    default:
                        // Custom.  User should set pattern with PatternRegExp.
                        rePattern.ValidationExpression = ".*";
                        break;
                }

                ViewState["PatternType"] = value;
            }
        }

	    /// <summary>
        /// This returns the actual regular expression used by the validator.
        /// If set, the pattern type is set to PatternType.Custom automatically.
	    /// </summary>
		[Category("Behavior"), DefaultValue(".*"), Bindable(true),
            Description("The underlying regular expression")]
        public string PatternRegExp
        {
            get
            {
                EnsureChildControls();
                return (rePattern.ValidationExpression.Length == 0) ?
                    ".*" : rePattern.ValidationExpression;
            }
            set
            {
                EnsureChildControls();
                ViewState["PatternType"] = PatternType.Custom;
                rePattern.ValidationExpression = value;
            }
        }

        //=====================================================================
        // Methods, etc

        /// <summary>
        /// This is overridden to create the
        /// <see cref="System.Web.UI.WebControls.RegularExpressionValidator"/>
        /// used to validate the input.
        /// </summary>
        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            rePattern = new RegularExpressionValidator();
            rePattern.Enabled = this.Enabled;
            rePattern.ValidationExpression = ".*";
            rePattern.ErrorMessage = "Not valid for selected pattern";

            // Use validation summary by default
            rePattern.Display = ValidatorDisplay.None;

            this.Controls.Add(rePattern);
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
            bool bFormatted;

            base.OnPreRender(e);
            rePattern.ControlToValidate = this.ID;
            rePattern.ID = this.ID + "_PREV";

            // Register formatting script?
            if(!this.AutoPostBack && this.EnableClientScript &&
              this.CanRenderUpLevel)
            {
                bFormatted = true;

                switch(this.Pattern)
                {
                    case PatternType.Phone:
                    case PatternType.PhoneNoExt:
                    case PatternType.AreaPhone:
                    case PatternType.AreaPhNoExt:
                        this.Attributes["onblur"] = "javascript:PTB_FormatPhone(this);";
                        this.Attributes["onkeydown"] = "javascript:return PTB_Keys(event.srcElement, event.keyCode, 'Phone');";
                        break;

                    case PatternType.ZIP:
                    case PatternType.ZIP4:
                        this.Attributes["onblur"] = "javascript:PTB_FormatZip(this);";
                        this.Attributes["onkeydown"] = "javascript:return PTB_Keys(event.srcElement, event.keyCode, 'Zip');";
                        break;

                    case PatternType.SSN:
                        this.Attributes["onblur"] = "javascript:PTB_FormatSSN(this);";
                        this.Attributes["onkeydown"] = "javascript:return PTB_Keys(event.srcElement, event.keyCode, 'SSN');";
                        break;

                    case PatternType.Time:
                        this.Attributes["onblur"] = "javascript:PTB_FormatTime(this);";
                        this.Attributes["onkeydown"] = "javascript:return PTB_TimeKeys(event.srcElement, event.keyCode);";
                        break;

                    default:
                        bFormatted = false;
                        break;
                }

                if(bFormatted &&
                  !this.Page.ClientScript.IsClientScriptBlockRegistered("EWS_PTBFormat"))
                    this.Page.ClientScript.RegisterClientScriptInclude(
                        typeof(PatternTextBox), "EWS_PTBFormat",
                        this.Page.ClientScript.GetWebResourceUrl(
                        typeof(PatternTextBox),
                        CtrlUtils.ScriptsPath + "PatternTextBox.js"));
            }
        }

		/// <summary>
		/// Render this control to the output parameter specified.
		/// </summary>
		/// <param name="writer">The HTML writer to which the output is written</param>
		protected override void Render(HtmlTextWriter writer)
		{
			base.Render(writer);

            if(rePattern != null)
                rePattern.RenderControl(writer);
		}

	    /// <summary>This is for the <b>YesNo</b> pattern.</summary>
        /// <returns>Returns true if the current text value matches any of
        /// the accepted values for true.  It returns false if not.</returns>
        public bool ToBoolean()
        {
            string strTemp = this.Text.ToUpper().Trim();

            if(strTemp == "Y" || strTemp == "T" || strTemp == "YES" ||
              strTemp == "TRUE" || strTemp == "1" || strTemp == "-1")
                return true;

            return false;
        }

	    /// <summary>
        /// This is for the <b>Time</b> pattern.  It returns the current value
        /// of the textbox as a <see cref="System.DateTime"/> object.
	    /// </summary>
        /// <returns>The date part defaults to the current date.  If empty or,
        /// the <see cref="Pattern"/> property is not set to <b>Time</b>, it
        /// returns a default <see cref="System.DateTime"/> object.</returns>
        public DateTime ToDateTime()
        {
            if(this.Pattern != PatternType.Time || this.Text.Length == 0)
                return new DateTime();

            return DateTime.Parse(this.Text);
        }
	}
}
