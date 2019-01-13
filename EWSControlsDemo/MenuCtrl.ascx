<%@ Control Language="vb" AutoEventWireup="false" Inherits="EWSControlsDemo.MenuCtrl" CodeFile="MenuCtrl.ascx.vb" %>

<table class="Menu" cellSpacing="0" cellPadding="4" width="100%" border="0">
  <tr>
	<td width="80px"><asp:Image id="imgLogo" runat="server" Height="45" Width="63" /></td>
	<td width="10px">&nbsp;</td>
	<td class="MenuAppName"><asp:Label id="lblMenuAppName" Runat="server" /></td>
	<td align="right">
<asp:DataList id="dlMenu" runat="server" RepeatDirection="Horizontal">
    <ItemTemplate>
		<%# funAddMenuLink(Container) %>
    </ItemTemplate>
</asp:DataList>
	</td>
  </tr>
</table>
