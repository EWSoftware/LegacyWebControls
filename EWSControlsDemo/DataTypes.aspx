<%@ Register TagPrefix="EWSC" Namespace="EWSoftware.Web.Controls" Assembly="EWSoftware.Web.Controls" %>
<%@ Page Language="vb" AutoEventWireup="false" Inherits="EWSControlsDemo.DataTypes" CodeFile="DataTypes.aspx.vb" %>

<form id="frmDataTypes" method="post" runat="server">

<asp:ValidationSummary id="vsSummary" DisplayMode="BulletList" ForeColor="" CssClass="ErrorMsg"
	HeaderText="<br>Please correct the following problems:" runat="server" /><br>

<table class="Shaded" cellSpacing="0" cellPadding="4" width="100%" border="0">
  <tr>
    <td colspan="3"><b>DateTextBox</b> only allows valid dates to be entered.
    A range validator is used to enforce the type as well as an optional date
    range. For browsers that support client-side script, formatting code is
    used to format the date with separators and a century if not entered.  A
    popup calendar can also be invoked by clicking the icon to the right of
    the date text box.  The date text box also supports several special keys
    used to set or alter the date (i.e. 'T' for today's date).  See the
    documentation for details.</td>
  </tr>
  <tr>
    <td width="17%"><EWSC:DateTextBox id="txtDateTextBoxFrom" runat="Server"
        Required="True" RequiredMessage="A From Date is required"
        DateErrorMessage="Please enter a valid date between {MinDate} and {MaxDate}" />
    </td>
    <td width="17%"><EWSC:DateTextBox id="txtDateTextBoxTo" runat="Server"
        Required="True" RequiredMessage="A To Date is required"
        DateErrorMessage="Please enter a valid date between {MinDate} and {MaxDate}"
        ControlToCompare="txtDateTextBoxFrom" Operator="GreaterThanEqual"
        CompareErrorMessage="To Date must be greater than or equal to From Date" />
    </td>
    <td>Enter any valid dates +/- 3 months from today.  Both are required and
    the To Date must be greater than or equal to the From Date.</td>
  </tr>
</table>
<br>
<table class="Shaded" cellSpacing="0" cellPadding="4" width="100%" border="0">
  <tr>
    <td colspan="3"><b>NumericTextBox</b> can be used to limit input to an
    integer, double, or currency value.  A range validator is used to enforce
    the type as well as an optional value range. For double values, the maximum
    number of decimal places can be specified.  For browsers that support
    client-side script, formatting code is used to format the number with the
    proper number of decimal places.</td>
  </tr>
  <tr>
    <td width="15%">&nbsp;</td>
    <td width="17%"><EWSC:NumericTextBox id="txtIntegerFrom" runat="Server" Type="Integer"
        MaxLength="2" Columns="2" MinimumValue="5" MaximumValue="10"
        NumberErrorMessage="Starting integer must be between {MinVal} and {MaxVal}" Required="True"
        RequiredMessage="Enter a starting integer value" /> to
        <EWSC:NumericTextBox id="txtIntegerTo" runat="Server" Type="Integer"
        MaxLength="2" Columns="2" MinimumValue="5" MaximumValue="10"
        NumberErrorMessage="Ending integer must be between {MinVal} and {MaxVal}" Required="True"
        RequiredMessage="Enter an ending integer value"
        ControlToCompare="txtIntegerFrom" Operator="LessThanEqual"
        CompareErrorMessage="Ending integer must be less than or equal to starting integer" />
    </td>
    <td>Enter integer values between 5 and 10.  Ending value must be less
    than or equal to the starting value.</td>
  </tr>
  <tr>
    <td>&nbsp;</td>
    <td><EWSC:NumericTextBox id="txtDouble" runat="Server" Type="Double"
    MaxLength="7" Columns="7" MinimumValue="-50.123" MaximumValue="1000.543"
    DecimalPlaces="3" DecimalErrorMessage="There can be a maximum of {Decimals} decimal places"
	NumberErrorMessage="Double must be between {MinVal} and {MaxVal} with up to {Decimals} decimal places" /></td>
    <td>Enter a double value between -50.123 and 1000.543.  There can be a
    maximum of 3 decimal place.</td>
  </tr>
  <tr>
    <td>&nbsp;</td>
    <td><EWSC:NumericTextBox id="txtCurrency" runat="Server" Type="Currency"
	MaxLength="7" Columns="7" MinimumValue="100.00" MaximumValue="5000.00"
	NumberErrorMessage="Currency must be between {MinVal} and {MaxVal} with only two decimal places" /></td>
    <td>Enter a currency value between 100.00 and 5000.00.  This is similar to
    a double but there can only ever be a maximum of two decimal places.
    </td>
  </tr>
  <tr>
    <td>&nbsp;</td>
    <td><EWSC:NumericTextBox id="txtInsertDec" runat="Server" Type="Double"
	MaxLength="12" Columns="12" InsertDecimal="True" DecimalPlaces="2"
    NumberErrorMessage="Enter a valid double with up to {Decimals} decimal places" /></td>
    <td>Enter a double.  This one uses the <b>InsertDecimal</b> property to
    insert a decimal point if one is not entered rather than appending missing
    decimal places.
    </td>
  </tr>
  <tr>
    <td colspan="3">
    <b>NOTE:</b> Due to a bug in the .NET validation code, currency symbols
    cannot be entered despite what the documentation says.  The client-side
    formatting code will remove all non-numeric characters including currency
    symbols and grouping characters as well as leading zeros.
    </td>
  </tr>
</table>
<br>
<table class="Shaded" cellSpacing="0" cellPadding="4" width="100%" border="0">
  <tr>
    <td colspan="3"><b>CreditCardTextBox</b> can be used to validate a credit
    card number.  It can be set to limit its input to a set of accepted credit
    card types.  It will also checksum the number to ensure it is valid.
    Client-side script is supplied to perform the same validation on the
    client to limit roundtrips to the server.
    </td>
  </tr>
  <tr>
    <td colspan="3" style="color: red"><b>WARNING: The demo application has
    no security so do not enter real credit card numbers.  Use the following
    test numbers instead:</b>
    </td>
  </tr>
  <tr>
	<td width="15%">&nbsp;</td>
	<td width="20%">VISA</td><td>4111111111111111</td>
  </tr>
  <tr>
	<td width="15%">&nbsp;</td><td>MasterCard</td><td>5500000000000004</td>
  </tr>
  <tr>
	<td width="15%">&nbsp;</td><td>American Express</td><td>340000000000009</td>
  </tr>
  <tr>
	<td width="15%">&nbsp;</td><td>Discover</td><td>6011000000000004</td>
  </tr>
  <tr>
	<td width="15%">&nbsp;</td><td>Diner's Club / Carte Blanche</td><td>30000000000004</td>
  </tr>
  <tr>
	<td width="15%">&nbsp;</td><td>enRoute</td><td>201400000000009</td>
  </tr>
  <tr>
	<td width="15%">&nbsp;</td><td>JCB</td><td>3088000000000009</td>
  </tr>
  <tr>
    <td>&nbsp;</td>
    <td><EWSC:CreditCardTextBox id="txtCreditCard" runat="Server"
	CardErrorMessage="Please enter a valid credit card number" /></td>
    <td>Enter a valid credit card number</td>
  </tr>
</table>
<br>
<table class="Shaded" cellSpacing="0" cellPadding="4" width="100%" border="0">
  <tr>
    <td colspan="3"><b>CCTypeDropDownList</b> can be used to provide an
    additional edit for the credit card number.  When associated with a
    <b>CreditCardTextBox</b> or a <b>CreditCardValidator</b>, the entered
    card number's type must match the card type selected in the dropdown
    list.  The dropdown list will only present card types that the card
    number control or validator will accept.
    </td>
  </tr>
  <tr>
    <td width="20%"><EWSC:CCTypeDropDownList id="cboCCTypes" runat="Server"
		CreditCardCtlID="txtLimitedCard" /></td>
    <td width="20%"><EWSC:CreditCardTextBox id="txtLimitedCard" runat="Server"
    AcceptedCardTypesString="VISA, MasterCard, Discover" CardTypeCtlID="cboCCTypes"
	CardErrorMessage="Please enter a valid credit card number that matches the type selected" /></td>
    <td>Select VISA, MasterCard, or Discover and enter a card number</td>
  </tr>
</table>
<br>
<asp:button cssClass="FormBtn" id="btnValidate" runat="server" Text="Validate" />
</form>
