<%@ Register TagPrefix="EWSC" Namespace="EWSoftware.Web.Controls" Assembly="EWSoftware.Web.Controls" %>
<%@ Page Language="vb" AutoEventWireup="false" Inherits="EWSControlsDemo.Mask" CodeFile="Mask.aspx.vb" %>

<form id="frmMask" method="post" runat="server">
<asp:ValidationSummary id="vsSummary" DisplayMode="BulletList" ForeColor="" CssClass="ErrorMsg"
	HeaderText="<br>Please correct the following problems:" runat="server" /><br>

<table class="Shaded" cellSpacing="0" cellPadding="4" width="100%" border="0">
  <tr>
    <td colspan="3">The <b>MaskTextBox</b> is similar to the <b>PatternTextBox</b>
    when used with its <b>Custom</b> pattern type.  The difference is that the
	<b>MaskTextBox</b> allows you to specify a mask rather than a regular
	expression to define the validation pattern.  This makes it easier to
	use than the <b>PatternTextBox</b> with the <b>Custom</b> pattern type.
	In addition, mask characters are automatically inserted as the user
	types text into the control when rendered on Internet Explorer.
    TODO: Add additional text on the valid mask characters and implement a demo that
    allows the user to specify a mask, set it, and test it in a MaskTextBox control.
    </td>
  </tr>
  <tr>
    <td width="20%" align="right">Mask Text Box</td>
    <td width="45%"><EWSC:MaskTextBox id="txtMaskTextBox" runat="Server" Columns="50"
    Mask="00000-9999" MaskErrorMessage="Enter text that matches the specified pattern" /></td>
    <td>Enter text that matches the specified pattern</td>
  </tr>
</table>
<br>
<asp:button cssClass="FormBtn" id="btnValidate" runat="server" Text="Validate" />

</form>
