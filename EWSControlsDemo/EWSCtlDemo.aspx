<%@ Register TagPrefix="EWSC" Namespace="EWSoftware.Web.Controls" Assembly="EWSoftware.Web.Controls" %>
<%@ Page Language="vb" AutoEventWireup="false" Inherits="EWSControlsDemo.EWSCtlDemo" CodeFile="EWSCtlDemo.aspx.vb" %>

<form id="frmEWSCtlDemo" method="post" runat="server">
<table width="100%">
  <tr>
	<td class="AboutHeader">
		<h1>Eric Woodruff ASP.NET Web Controls Demo</h1>
	</td>
  </tr>
  <tr>
	<td>
	<div align="center">
	<b>Version <asp:label id="lblVersion" runat="server" />&nbsp;&nbsp;&nbsp;&nbsp;
Release Date <asp:label id="lblReleaseDate" runat="server" /></b>
	</div>
	</td>
  </tr>
</table>
<br>
<div align="center"><img src="Images/EWSCtls.bmp" alt="EWSoftware.Web.Controls"></div>
<br>
This application is used to demonstrate the various controls and classes in the
<b>EWSoftware.Web.Controls</b> namespace:<br><br>
<div align="center">
<table cellspacing="2" cellpadding="2" border="0" width="90%">
  <tr>
    <td valign="top" width="25%">
        <b>Validators</b><br>
        ActionRequiredValidator<br>
        CreditCardValidator<br>
        MinMaxListValidator<br>
    </td>
    <td valign="top" width="25%">
        <b>Text Box Controls</b><br>
        EWSTextBox<br>
        CompareTextBox<br>
        NumericTextBox<br>
        DateTextBox<br>
        PatternTextBox<br>
        MaskTextBox<br>
        CreditCardTextBox<br>
    </td>
    <td valign="top" width="25%">
        <b>List Controls</b><br>
        EWSDropDownList<br>
        GridDropDownList<br>
        EWSListBox<br>
        EWSCheckBoxList<br>
        EWSRadioButtonList<br>
        MinMaxListBox<br>
        MinMaxCheckBoxList<br>
        CCTypeDropDownList<br>
    </td>
    <td valign="top" width="25%">
        <b>Miscellaneous</b><br>
        AddToFavorites<br>
        ConfirmButton<br>
        ConfirmLinkButton<br>
		FileLink<br>
		WindowOpener<br>
        CtrlUtils<br>
    </td>
  </tr>
</table>
</div>
<p>For assistance, e-mail
<a href="mailto:Eric@EWoodruff.us?Subject=EWSoftware Web Controls">Eric@EWoodruff.us</a>.
</form>
