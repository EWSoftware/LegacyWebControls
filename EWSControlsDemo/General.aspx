<%@ Register TagPrefix="EWSC" Namespace="EWSoftware.Web.Controls" Assembly="EWSoftware.Web.Controls" %>
<%@ Page Language="vb" AutoEventWireup="false" Inherits="EWSControlsDemo.General" CodeFile="General.aspx.vb" %>

<form id="frmGeneral" method="post" runat="server">

<asp:ValidationSummary id="vsSummary" DisplayMode="BulletList" ForeColor="" CssClass="ErrorMsg"
	HeaderText="<br>Please correct the following problems:" runat="server" /><br>

<table class="Shaded" cellSpacing="0" cellPadding="4" width="100%" border="0">
  <tr>
    <td colspan="3"><b>EWSTextBox</b> trims leading and trailing whitespace
    and offers three styles of case converison.  Case conversion and trimming
    can be turned off if not wanted.  All textbox classes ultimately derive
    from this one.</td>
  </tr>
  <tr>
    <td width="15%">&nbsp;</td>
    <td width="22%"><EWSC:EWSTextBox id="txtTrimTextBox" runat="Server" /></td>
    <td>Trims leading and trailing whitespace, no text conversion</td>
  </tr>
  <tr>
    <td>&nbsp;</td>
    <td><EWSC:EWSTextBox id="txtUpperTextBox" runat="Server" Casing="Upper"/></td>
    <td>Trims text and converts it to uppercase</td>
  </tr>
  <tr>
    <td>&nbsp;</td>
    <td><EWSC:EWSTextBox id="txtLowerTextBox" runat="Server" Casing="Lower"/></td>
    <td>Trims text and converts it to lowercase</td>
  </tr>
  <tr>
    <td>&nbsp;</td>
    <td><EWSC:EWSTextBox id="txtProperTextBox" runat="Server" Casing="Proper"/></td>
    <td>Trims text and converts it to proper case (i.e. a name).  It handles
	apostrophes such as "O'Conner", the "Mc" prefix as in "McDonald" and a set
	of suffixes	such as II, III, etc. that can be customized.</td>
  </tr>
</table>
<br>
<table class="Shaded" cellSpacing="0" cellPadding="4" width="100%" border="0">
  <tr>
    <td colspan="3"><b>EWSTextBox</b> includes a <b>Required</b> property that,
    when enabled, requires something to be entered in the field.  It also
    contains a maximum length validator for use with TEXTAREA input boxes
    (<b>TextMode</b> = MultiLine).</td>
  </tr>
  <tr>
    <td width="15%">&nbsp;</td>
    <td width="22%"><EWSC:EWSTextBox id="txtRequiredTextBox" runat="Server"
    	Required="True" RequiredMessage="Enter something in the required text box" /></td>
    <td>Something must be entered in here</td>
  </tr>
  <tr>
    <td width="15%">&nbsp;</td>
    <td width="22%"><EWSC:EWSTextBox id="txtMultiLineTextBox" runat="Server"
        Columns="60" Rows="3" MaxLength="100" TextMode="MultiLine"
    	MaxLenErrorMessage="A maximum of {MaxLen} characters is allowed" /></td>
    <td>Enter a maximum of 100 characters (includes carriage returns and line feeds)</td>
  </tr>
</table>
<br>
<table class="Shaded" cellSpacing="0" cellPadding="4" width="100%" border="0">
  <tr>
    <td colspan="3"><b>CompareTextBox</b> includes a comparison validator
    that allows you to compare one control to another or to a value.  It
    allows for only one comparison so any additional comparisons will need to
    be added manually with additional comparison validators on the page.
    All other textbox controls except <b>EWSTextBox</b> derive from this one.
    This example shows a value comparison.  The <b>DateTextBox</b> and
    <b>NumericTextBox</b> examples show how to use it to compare against
    another control. Note that the type-specific controls automatically set
    the <b>CompareType</b> property to the appropriate data type.  For
    <b>CompareTextBox</b>, you must set it manually.
    </td>
  </tr>
  <tr>
    <td width="15%">&nbsp;</td>
    <td width="22%"><EWSC:CompareTextBox id="txtCompareTextBox" runat="Server"
    ValueToCompare="ABCDEF" Operator="Equal" CompareType="String" Casing="Upper"
    CompareErrorMessage="Enter 'ABCDEF' in uppercase in the CompareTextBox"/></td>
    <td>Enter ABCDEF in uppercase in the CompareTextBox</td>
  </tr>
</table>
<br>
<asp:button cssClass="FormBtn" id="btnValidate" runat="server" Text="Validate" />

</form>
