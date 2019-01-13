<%@ Register TagPrefix="EWSC" Namespace="EWSoftware.Web.Controls" Assembly="EWSoftware.Web.Controls" %>
<%@ Page Language="vb" AutoEventWireup="false" Inherits="EWSControlsDemo.Pattern" CodeFile="Pattern.aspx.vb" %>

<form id="frmPattern" method="post" runat="server">
<asp:ValidationSummary id="vsSummary" DisplayMode="BulletList" ForeColor="" CssClass="ErrorMsg"
	HeaderText="<br>Please correct the following problems:" runat="server" /><br>

<table class="Shaded" cellSpacing="0" cellPadding="4" width="100%" border="0">
  <tr>
    <td colspan="3"><b>PatternTextBox</b> offers a way to enter various
    data types other than the standard date and numeric types.  A regular
    expression validator is used to enter one of several specific types or
    a custom type that allows you to set the pattern.  The class derives from
    <b>CompareTextBox</b> so it inherits all of the other base properties
    too.  For browsers that support client-side script, the patterns
    <b>Phone</b>, <b>PhoneNoExt</b>, <b>AreaPhone</b>, <b>AreaPhNoExt</b>,
    <b>SSN</b>, <b>Time</b>, <b>Zip</b>, and <b>Zip4</b> support formatting to
    insert missing separators, etc.  The <b>Time</b> pattern also supports
    several special keys used to set or alter the time (i.e. 'N' for the
    current time). See the documentation for pattern-specific notes on what
    is considered acceptable input.</td>
  </tr>
  <tr>
    <td width="15%" align="right">Phone</td>
    <td width="45%"><EWSC:PatternTextBox id="txtPhone" runat="Server" Pattern="Phone" Columns="50"
	PatternErrorMessage="Enter a phone number (###) ###-####, area code and extension optional" /></td>
    <td>Phone Number, area code and extension info are optional</td>
  </tr>
  <tr>
    <td align="right">PhoneNoExt</td>
    <td><EWSC:PatternTextBox id="txtPhoneNoExt" Pattern="PhoneNoExt"
    PatternErrorMessage="Enter a phone number (###) ###-####, area code optional" runat="Server" /></td>
    <td>Phone Number, area code optional, no extension info allowed</td>
  </tr>
  <tr>
    <td align="right">AreaPhone</td>
    <td><EWSC:PatternTextBox id="txtAreaPhone" Pattern="AreaPhone" Columns="50"
	PatternErrorMessage="Enter phone in format (###) ###-####, extension optional" runat="Server" /></td>
    <td>Phone Number, area code required, extension info optional</td>
  </tr>
  <tr>
    <td align="right">AreaPhNoExt</td>
    <td><EWSC:PatternTextBox id="txtAreaPhNoExt" Pattern="AreaPhNoExt"
	PatternErrorMessage="Enter phone in format (###) ###-####" runat="Server" /></td>
    <td>Phone Number, area code required, no extension info allowed</td>
  </tr>
</table>
<br>
<table class="Shaded" cellSpacing="0" cellPadding="4" width="100%" border="0">
  <tr>
    <td width="15%" align="right">EMail</td>
    <td width="45%"><EWSC:PatternTextBox id="txtEMail" Pattern="EMail" Columns="50"
	PatternErrorMessage="Enter a valid e-mail address" runat="Server" /></td>
    <td>Enter a valid e-mail address.  IP addresses are supported in place
    of the domain name</td>
  </tr>
  <tr>
    <td align="right">URL</td>
    <td><EWSC:PatternTextBox id="txtURL" Pattern="URL" Columns="50"
	PatternErrorMessage="Enter a valid URL" runat="Server" /></td>
    <td>Enter a valid URL with or without the protocol (http/https/ftp)</td>
  </tr>
  <tr>
    <td align="right">UNC</td>
    <td><EWSC:PatternTextBox id="txtUNC" Pattern="UNC" Columns="50"
	PatternErrorMessage="Enter a valid UNC path" runat="Server" /></td>
    <td>Enter a valid UNC path (i.e. \\Server\Folder\SubFolder).  The path
    name can contain spaces</td>
  </tr>
  <tr>
    <td align="right">IPv4Address</td>
    <td><EWSC:PatternTextBox id="txtIPv4Addr" Pattern="IPv4Address"
	PatternErrorMessage="Enter a valid IPv4 address" runat="Server" /></td>
    <td>Enter a valid IPv4 address (i.e. 127.0.0.1)</td>
  </tr>
  <tr>
    <td align="right">IPv6Address</td>
    <td><EWSC:PatternTextBox id="txtIPv6Addr" Pattern="IPv6Address"
	PatternErrorMessage="Enter a valid IPv6 address" runat="Server" /></td>
    <td>Enter a valid IPv6 address (i.e. 21DA:D3::2F3B:2AA:FF:FE28:9C5A)</td>
  </tr>
</table>
<br>
<table class="Shaded" cellSpacing="0" cellPadding="4" width="100%" border="0">
  <tr>
    <td width="15%" align="right">State</td>
    <td width="22%"><EWSC:PatternTextBox id="txtState" Pattern="State"
	PatternErrorMessage="Enter a valid state abbreviation" runat="Server" /></td>
    <td>US state abbreviations only</td>
  </tr>
  <tr>
    <td align="right">StPosProv</td>
    <td><EWSC:PatternTextBox id="txtStPosProv" Pattern="StPosProv"
	PatternErrorMessage="Enter a valid state, possession, or province abbreviation" runat="Server" /></td>
    <td>US state/possession/military state or Canadian province abbreviations</td>
  </tr>
  <tr>
    <td align="right">ZIP</td>
    <td><EWSC:PatternTextBox id="txtZip" Pattern="ZIP" runat="server"
	PatternErrorMessage="Enter 5 digit ZIP code or 9 digit ZIP+4 code" /></td>
    <td>Enter a US ZIP code (5 digit or a 9 digit ZIP+4)</td>
  </tr>
  <tr>
    <td align="right">ZIP4</td>
    <td><EWSC:PatternTextBox id="txtZipPlus4" Pattern="ZIP4"
	PatternErrorMessage="Enter a US ZIP+4 ZIP code" runat="Server" /></td>
    <td>Enter a US ZIP+4 ZIP code only</td>
  </tr>
  <tr>
    <td align="right">ZIP5</td>
    <td><EWSC:PatternTextBox id="txtZip5Digit" Pattern="ZIP5"
	PatternErrorMessage="Enter a 5 digit ZIP code" runat="Server" /></td>
    <td>Enter a US ZIP code (5 digit only)</td>
  </tr>
</table>
<br>
<table class="Shaded" cellSpacing="0" cellPadding="4" width="100%" border="0">
  <tr>
    <td width="15%" align="right">SSN</td>
    <td width="22%"><EWSC:PatternTextBox id="txtSSN" Pattern="SSN"
	PatternErrorMessage="Enter a valid Social Security number" runat="Server" /></td>
    <td>Enter a social security number&nbsp;&nbsp;
<b style="color: red">WARNING: No security, do not enter your real one</b></td>
  </tr>
  <tr>
    <td align="right">Time</td>
    <td><EWSC:PatternTextBox id="txtTime" Pattern="Time"
	PatternErrorMessage="Enter a valid time" runat="Server" /></td>
    <td>Enter a time of day (24 hour or AM/PM format)</td>
  </tr>
  <tr>
    <td align="right">YesNo</td>
    <td><EWSC:PatternTextBox id="txtYesNo" Pattern="YesNo"
	PatternErrorMessage="Enter Y, N, T, F, yes, no, true, false, 0, 1, or -1"
    runat="Server" /></td>
    <td>Enter a yes or no value (Y, N, T, F, yes, no, true, false, 0, 1, -1)</td>
  </tr>
  <tr>
    <td align="right">Custom</td>
    <td><EWSC:PatternTextBox id="txtCustom" PatternRegExp="^[AaBb].*[CcDd]$" Columns="20"
	PatternErrorMessage="Enter something in the format ^[AaBb].*[CcDd]$" runat="Server" /></td>
    <td>Custom format expression: ^[AaBb].*[CcDd]$</td>
  </tr>
</table>
<br>
<asp:button cssClass="FormBtn" id="btnValidate" runat="server" Text="Validate" />

</form>
