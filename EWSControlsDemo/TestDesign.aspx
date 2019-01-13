<%@ Register TagPrefix="ewsc" Namespace="EWSoftware.Web.Controls" Assembly="EWSoftware.Web.Controls" %>
<%@ Page Language="vb" AutoEventWireup="false" Inherits="EWSControlsDemo.TestDesign" CodeFile="TestDesign.aspx.vb" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
  <HEAD>
<title>TestDesign</title>
<meta name="GENERATOR" content="Microsoft Visual Studio.NET 7.0">
<meta name="CODE_LANGUAGE" content="Visual Basic 7.0">
<meta name=vs_defaultClientScript content="JavaScript">
<meta name=vs_targetSchema content="http://schemas.microsoft.com/intellisense/ie5">
<link rel="stylesheet" type="text/css" href="styles.css">
  </HEAD>

<body>

<form id="Form1" method="post" runat="server">
<P>This page is used for testing the design time stuff on all controls:</P>
<P><ewsc:ActionRequiredValidator id=ActionRequiredValidator1 runat="server" ErrorMessage="Action Required"></ewsc:ActionRequiredValidator></P>
<P><ewsc:AddToFavorites id="AddToFavorites1" runat="server" /></P>
<P><ewsc:CCTypeDropDownList id=CCTypeDropDownList1 runat="server" CreditCardCtlID="CreditCardTextBox1"></ewsc:CCTypeDropDownList></P>
<P><ewsc:CompareTextBox id=CompareTextBox1 runat="server" CompareErrorMessage="A comparison failed" Operator="Equal" ControlToCompare="DateTextBox1"></ewsc:CompareTextBox></P>
<P><ewsc:ConfirmButton id=ConfirmButton1 runat="server" Text="Confirm" ConfirmPrompt="Are you sure?"></ewsc:ConfirmButton></P>
<P><ewsc:ConfirmLinkButton id=ConfirmLinkButton1 runat="server" Text="Confirm" ConfirmPrompt="Are you sure?" CommandName="Confirm"></ewsc:ConfirmLinkButton></P>
<P><ewsc:CreditCardTextBox id=CreditCardTextBox1 runat="server" CardErrorMessage="Invalid card number"></ewsc:CreditCardTextBox></P>
<P><ewsc:CreditCardValidator id=CreditCardValidator1 runat="server" ErrorMessage="Invalid card number" ControlToValidate="EWSTextBox1"></ewsc:CreditCardValidator></P>
<P><ewsc:DateTextBox id=DateTextBox1 runat="server" DateErrorMessage="Not a valid date value"></ewsc:DateTextBox></P>
<P><ewsc:EWSCheckBoxList id=EWSCheckBoxList1 runat="server"></ewsc:EWSCheckBoxList></P>
<P><ewsc:EWSDropDownList id=EWSDropDownList1 runat="server"></ewsc:EWSDropDownList></P>
<P><ewsc:EWSListBox id=EWSListBox1 runat="server"></ewsc:EWSListBox></P>
<P><ewsc:EWSRadioButtonList id=EWSRadioButtonList1 runat="server"></ewsc:EWSRadioButtonList></P>
<P><ewsc:EWSTextBox id=EWSTextBox1 runat="server"></ewsc:EWSTextBox></P>
<p><ewsc:FileLink id=FileLink1 runat="server" URL="Test.pdf" FileImage="PDFImage.gif" ShowFileSize="True" />
<P><ewsc:GridDropDownList id=GridDropDownList1 runat="server"></ewsc:GridDropDownList></P>
<P><ewsc:MinMaxCheckBoxList id=MinMaxCheckBoxList1 runat="server" MinSel="1" MinMaxErrorMessage="Selections exceed minimum/maximum"></ewsc:MinMaxCheckBoxList></P>
<P><ewsc:MinMaxListValidator id=MinMaxListValidator1 runat="server" ErrorMessage="Selections exceed minimum/maximum" MinSel="1" ControlToValidate="EWSListBox1"></ewsc:MinMaxListValidator></P>
<P><ewsc:PatternTextBox id=PatternTextBox1 runat="server" PatternErrorMessage="Not valid for selected pattern" Pattern="Phone"></ewsc:PatternTextBox></P>
<P><ewsc:MinMaxListBox id=MinMaxListBox1 runat="server" MinSel="1" MinMaxErrorMessage="Selections exceed minimum/maximum"></ewsc:MinMaxListBox></P>
<P><ewsc:NumericTextBox id=NumericTextBox1 runat="server" NumberErrorMessage="Not a valid numeric value" MaximumValue="999" MinimumValue="0" Type="Integer"></ewsc:NumericTextBox></P>
<P><ewsc:WindowOpener id="woTest" runat="server" /></P>

</form>

</body>
</HTML>
