<%@ Page Language="vb" AutoEventWireup="false" Inherits="EWSControlsDemo.ListControls" CodeFile="ListControls.aspx.vb" %>
<%@ Register TagPrefix="EWSC" Namespace="EWSoftware.Web.Controls" Assembly="EWSoftware.Web.Controls" %>

<form id="frmDropDown" method="post" runat="server">
<asp:ValidationSummary id="vsSummary" DisplayMode="BulletList" ForeColor="" CssClass="ErrorMsg"
	HeaderText="<br>Please correct the following problems:" runat="server" /><br>

<table class="Shaded" cellSpacing="0" cellPadding="4" width="100%" border="0">
  <tr>
    <td colspan="3"><b>EWSDropDownList</b> is the base class for the
    other DropDown controls on this page.  It adds some extra properties
    that allow the specification of a default selection to use when no
    other item is selected when the page is rendered, the ability to set
    the selection based on the text description rather than the value,
    and some other helper properties and methods.</td>
  </tr>
  <tr>
    <td width="15%">&nbsp;</td>
    <td><EWSC:EWSDropDownList id="cboEWSDropDownList" runat="Server" /></td>
    <td>Demonstrates the use of the extra properties and methods.</td>
  </tr>
  <tr>
	<td class="FieldLabel" valign="top">Status Info</td>
	<td colspan="2"><asp:Label id="lblEWSDDMsg" runat="Server" /></td>
  </tr>
  <tr>
	<td colspan="3">
	<asp:Button cssClass="FormBtn" id="btnPost" Text="Post" ToolTip="Just post back"
		runat="Server" />
	<asp:Button cssClass="FormBtn" id="btnDefault" Text="Default"
		ToolTip="Reset to default item" runat="Server" />
	<asp:Button cssClass="FormBtn" id="btnSetByValue" Text="By Value"
		ToolTip="Set by value" runat="Server" />
	<asp:Button cssClass="FormBtn" id="btnSetByText" Text="By Text"
		ToolTip="Set by text" runat="Server" />
	</td>
  </tr>
</table>
<br>
<table class="Shaded" cellSpacing="0" cellPadding="4" width="100%" border="0">
  <tr>
    <td colspan="3"><b>EWSDropDownList</b> contains a Required property that
    allows you to specify whether or not a selection is required in the
    dropdown list.  There should be an item in the list of options with a
    blank value so that a "no selection" condition can be detected.</td>
  </tr>
  <tr>
    <td width="15%">&nbsp;</td>
    <td width="20%"><EWSC:EWSDropDownList id="cboRequiredDropDownList"
		runat="Server" Required="False"
		RequiredMessage="Select a value from the dropdown list" /></td>
    <td><asp:CheckBox id="chkRequireSel" runat="Server" AutoPostBack="True"
		Text="Check here to require selection" Checked="False"/></td>
  </tr>
  <tr>
	<td colspan="3">
	<asp:Button cssClass="FormBtn" id="btnValidate" Text="Validate"
		ToolTip="Test required dropdown list" runat="Server" />
	</td>
  </tr>
</table>
<br>
<table class="Shaded" cellSpacing="0" cellPadding="4" width="100%" border="0">
  <tr>
    <td width="50%" valign="top"><b>EWSDropDownList</b> and
<b>EWSListBox</b> (see below) also enable incremental searching in browsers
that support client-side script.  Start typing and the list will jump to the
matching selection.  Pressing Backspace will remove the last typed character
and find the previous match.  Pressing Escape will revert to the default
selected item.</td>
    <td width="25%" valign="top">
    <EWSC:EWSDropDownList id="cboState" runat="server">
        <asp:ListItem Value="AL">Alabama</asp:ListItem>
        <asp:ListItem Value="AK">Alaska</asp:ListItem>
        <asp:ListItem Value="AZ">Arizona</asp:ListItem>
        <asp:ListItem Value="AR">Arkansas</asp:ListItem>
        <asp:ListItem Value="CA">California</asp:ListItem>
        <asp:ListItem Value="CO">Colorado</asp:ListItem>
        <asp:ListItem Value="CT" Selected="true">Connecticut</asp:ListItem>
        <asp:ListItem Value="DE">Delaware</asp:ListItem>
        <asp:ListItem Value="DC">District of Columbia</asp:ListItem>
        <asp:ListItem Value="FL">Florida</asp:ListItem>
        <asp:ListItem Value="GA">Georgia</asp:ListItem>
        <asp:ListItem Value="HI">Hawaii</asp:ListItem>
        <asp:ListItem Value="ID">Idaho</asp:ListItem>
        <asp:ListItem Value="IL">Illinois</asp:ListItem>
        <asp:ListItem Value="IN">Indiana</asp:ListItem>
        <asp:ListItem Value="IA">Iowa</asp:ListItem>
        <asp:ListItem Value="KS">Kansas</asp:ListItem>
        <asp:ListItem Value="KY">Kentucky</asp:ListItem>
        <asp:ListItem Value="LA">Louisiana</asp:ListItem>
        <asp:ListItem Value="ME">Maine</asp:ListItem>
        <asp:ListItem Value="MD">Maryland</asp:ListItem>
        <asp:ListItem Value="MA">Massachusetts</asp:ListItem>
        <asp:ListItem Value="MI">Michigan</asp:ListItem>
        <asp:ListItem Value="MN">Minnesota</asp:ListItem>
        <asp:ListItem Value="MS">Mississippi</asp:ListItem>
        <asp:ListItem Value="MO">Missouri</asp:ListItem>
        <asp:ListItem Value="MT">Montana</asp:ListItem>
        <asp:ListItem Value="NE">Nebraska</asp:ListItem>
        <asp:ListItem Value="NV">Nevada</asp:ListItem>
        <asp:ListItem Value="NH">New Hampshire</asp:ListItem>
        <asp:ListItem Value="NJ">New Jersey</asp:ListItem>
        <asp:ListItem Value="NM">New Mexico</asp:ListItem>
        <asp:ListItem Value="NY">New York</asp:ListItem>
        <asp:ListItem Value="NC">North Carolina</asp:ListItem>
        <asp:ListItem Value="ND">North Dakota</asp:ListItem>
        <asp:ListItem Value="OH">Ohio</asp:ListItem>
        <asp:ListItem Value="OK">Oklahoma</asp:ListItem>
        <asp:ListItem Value="OR">Oregon</asp:ListItem>
        <asp:ListItem Value="PA">Pennsylvania</asp:ListItem>
        <asp:ListItem Value="RI">Rhode Island</asp:ListItem>
        <asp:ListItem Value="SC">South Carolina</asp:ListItem>
        <asp:ListItem Value="SD">South Dakota</asp:ListItem>
        <asp:ListItem Value="TN">Tennessee</asp:ListItem>
        <asp:ListItem Value="TX">Texas</asp:ListItem>
        <asp:ListItem Value="UT">Utah</asp:ListItem>
        <asp:ListItem Value="VT">Vermont</asp:ListItem>
        <asp:ListItem Value="VA">Virginia</asp:ListItem>
        <asp:ListItem Value="WA">Washington</asp:ListItem>
        <asp:ListItem Value="WV">West Virginia</asp:ListItem>
        <asp:ListItem Value="WI">Wisconsin</asp:ListItem>
        <asp:ListItem Value="WY">Wyoming</asp:ListItem>
    </EWSC:EWSDropDownList></td>
    <td width="25%"><EWSC:EWSListBox id="lbState" runat="server" Size="10">
        <asp:ListItem Value="AL">Alabama</asp:ListItem>
        <asp:ListItem Value="AK">Alaska</asp:ListItem>
        <asp:ListItem Value="AZ">Arizona</asp:ListItem>
        <asp:ListItem Value="AR">Arkansas</asp:ListItem>
        <asp:ListItem Value="CA">California</asp:ListItem>
        <asp:ListItem Value="CO">Colorado</asp:ListItem>
        <asp:ListItem Value="CT">Connecticut</asp:ListItem>
        <asp:ListItem Value="DE">Delaware</asp:ListItem>
        <asp:ListItem Value="DC">District of Columbia</asp:ListItem>
        <asp:ListItem Value="FL" Selected="true">Florida</asp:ListItem>
        <asp:ListItem Value="GA">Georgia</asp:ListItem>
        <asp:ListItem Value="HI">Hawaii</asp:ListItem>
        <asp:ListItem Value="ID">Idaho</asp:ListItem>
        <asp:ListItem Value="IL">Illinois</asp:ListItem>
        <asp:ListItem Value="IN">Indiana</asp:ListItem>
        <asp:ListItem Value="IA">Iowa</asp:ListItem>
        <asp:ListItem Value="KS">Kansas</asp:ListItem>
        <asp:ListItem Value="KY">Kentucky</asp:ListItem>
        <asp:ListItem Value="LA">Louisiana</asp:ListItem>
        <asp:ListItem Value="ME">Maine</asp:ListItem>
        <asp:ListItem Value="MD">Maryland</asp:ListItem>
        <asp:ListItem Value="MA">Massachusetts</asp:ListItem>
        <asp:ListItem Value="MI">Michigan</asp:ListItem>
        <asp:ListItem Value="MN">Minnesota</asp:ListItem>
        <asp:ListItem Value="MS">Mississippi</asp:ListItem>
        <asp:ListItem Value="MO">Missouri</asp:ListItem>
        <asp:ListItem Value="MT">Montana</asp:ListItem>
        <asp:ListItem Value="NE">Nebraska</asp:ListItem>
        <asp:ListItem Value="NV">Nevada</asp:ListItem>
        <asp:ListItem Value="NH">New Hampshire</asp:ListItem>
        <asp:ListItem Value="NJ">New Jersey</asp:ListItem>
        <asp:ListItem Value="NM">New Mexico</asp:ListItem>
        <asp:ListItem Value="NY">New York</asp:ListItem>
        <asp:ListItem Value="NC">North Carolina</asp:ListItem>
        <asp:ListItem Value="ND">North Dakota</asp:ListItem>
        <asp:ListItem Value="OH">Ohio</asp:ListItem>
        <asp:ListItem Value="OK">Oklahoma</asp:ListItem>
        <asp:ListItem Value="OR">Oregon</asp:ListItem>
        <asp:ListItem Value="PA">Pennsylvania</asp:ListItem>
        <asp:ListItem Value="RI">Rhode Island</asp:ListItem>
        <asp:ListItem Value="SC">South Carolina</asp:ListItem>
        <asp:ListItem Value="SD">South Dakota</asp:ListItem>
        <asp:ListItem Value="TN">Tennessee</asp:ListItem>
        <asp:ListItem Value="TX">Texas</asp:ListItem>
        <asp:ListItem Value="UT">Utah</asp:ListItem>
        <asp:ListItem Value="VT">Vermont</asp:ListItem>
        <asp:ListItem Value="VA">Virginia</asp:ListItem>
        <asp:ListItem Value="WA">Washington</asp:ListItem>
        <asp:ListItem Value="WV">West Virginia</asp:ListItem>
        <asp:ListItem Value="WI">Wisconsin</asp:ListItem>
        <asp:ListItem Value="WY">Wyoming</asp:ListItem>
    </EWSC:EWSListBox></td>
  </tr>
</table>
<br>
<table class="Shaded" cellSpacing="0" cellPadding="4" width="100%" border="0">
  <tr>
	<td>The DataGrid below demonstrates the <b>GridDropDownList</b>.  The
	normal <b>DropDownList</b> isn't able to bubble events up to the parent.
	<b>GridDropDownList</b> allows the <b>DefaultSelected</b> and
    <b>SelectedIndexChanged</b> events to bubble up and get handled by you in
    the parent grid control.
	<p>This example also demonstrates the <b>ConfirmButton</b> and <b>ConfirmLinkButton</b>
	controls.  These pop up a message box asking to confirm the operation performed
	by the button or link.  Cancelling the message prevents the postback and thus
	the button or link's action.
	<p>The <b>ActionRequiredValidator</b> is also demonstrated.  This validator
	provides a way to display a validation message that is tied to a condition
	requiring some action from the user rather than a control.  The server-side
	code uses it in the example to tell the user to finish changes to the current
	grid item being edited before editing another item or deleting items.
  </tr>
</table>
<br>
<EWSC:ActionRequiredValidator id="arvSave" runat="server"
	ErrorMessage="Please save changes to the contact info before continuing" />

<asp:DataGrid id="dgPhones" runat="server" CellPadding="2" Width="100%"
	GridLines="None" AutoGenerateColumns="False" ShowFooter="True">
	<HeaderStyle cssclass="ColHeader" />
	<FooterStyle cssClass="ColFooter" />
	<Columns>
		<asp:BoundColumn DataField="PhoneKey" ReadOnly="True" Visible="False" />
		<asp:TemplateColumn HeaderText="&nbsp;Contact Type" ItemStyle-CssClass="CellField">
			<ItemTemplate>
				<%# DataBinder.Eval(Container.DataItem, "PhTypeDesc") %>
			</ItemTemplate>
			<FooterTemplate>
				<br>&nbsp;<asp:LinkButton CommandName="Add" Text="Add New Contact Info" runat="server"
				ToolTip="Add contact info" /><br><br>
			</FooterTemplate>
			<EditItemTemplate>
				<EWSC:GridDropDownList id="cboPhoneType" runat="server"
					DataValueField="Key" DataTextField="Value"
					DataSource="<%# funGetPhoneTypes() %>"
					DefaultSelection='<%# DataBinder.Eval(Container.DataItem, "PhoneType") %>' />
			</EditItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="&nbsp;Number / E-Mail Address" ItemStyle-CssClass="CellField">
			<ItemTemplate>
				<%# DataBinder.Eval(Container.DataItem, "PhoneNumb") %>
			</ItemTemplate>
			<EditItemTemplate>
				<EWSC:PatternTextBox Pattern="Phone" id="txtPhoneNumb" runat="server"
					PatternErrorMessage="Enter a valid phone number ((999) 999-9999)" Required="true"
					RequiredMessage="A phone number is required" MaxLength="50" Columns="30"
					Text='<%# DataBinder.Eval(Container.DataItem, "PhoneNumb") %>' />
			</EditItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn ItemStyle-CssClass="Cell">
			<ItemTemplate>
				<asp:LinkButton CommandName="Edit" Text="Edit" runat="server"
					ToolTip="Edit this entry" />
            </ItemTemplate>
			<EditItemTemplate>
				<asp:LinkButton CommandName="Update" Text="Update" runat="server"
					ToolTip="Save changes to this entry" ID="Linkbutton3"/>&nbsp;&nbsp;
				<asp:LinkButton CommandName="Cancel" Text="Cancel" runat="server"
					CausesValidation="False" ToolTip="Cancel changes" />
			</EditItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn ItemStyle-CssClass="Cell">
			<ItemTemplate>
				<EWSC:ConfirmLinkButton CommandName="Delete" Text="Delete" runat="server"
					CausesValidation="false" ToolTip="Delete this entry"
					ConfirmPrompt="Are you sure you want to delete this phone number?" />
            </ItemTemplate>
		</asp:TemplateColumn>
	</Columns>
</asp:DataGrid>
<br>
<EWSC:ConfirmButton cssClass="FormBtn" id="btnRemoveAll" runat="Server"
	Text="Remove All" ToolTip="Remove all entries" runat="Server" CausesValidation="False"
	ConfirmPrompt="Are you sure you want to remove all contact info?" />
<br><br>
<table class="Shaded" cellSpacing="0" cellPadding="4" width="100%" border="0">
  <tr>
	<td>The following sections are disabled so as not to interfere with the
	demos above.  Check the checkbox to enable the demos below.
	</td>
  </tr>
  <tr>
	<td align="center"><asp:CheckBox id="chkEnableDemos" Text="&nbsp;Enable the demos below"
		AutoPostBack="True" runat="server" /></td>
  </tr>
</table>
<br>
<table class="Shaded" cellSpacing="0" cellPadding="4" width="100%" border="0">
  <tr>
    <td colspan="2" valign="top"><b>EWSListBox</b> is the base class for
    <b>MinMaxListBox</b> (see below).  It adds some extra properties
that allow the specification of a default selection or selections to use
when no other item is selected when the page is rendered, the ability to
set the selection or selections based on the text description rather
than the value, and some other helper properties and methods.  As demonstrated
above, it also has incremental search abilities.
    </td>
    <td rowspan="3" align="center"><EWSC:EWSListBox id="lbEWSListBox" runat="server" 
			Rows="10" SelectionMode="Multiple" /><br>
		Demonstrates the use of the extra properties and methods.</td>
  </tr>
  <tr>
	<td class="FieldLabel" valign="top" width="15%">Status Info</td>
	<td valign="top"><asp:Label id="lblEWSLBMsg" runat="Server" />&nbsp;</td>
  </tr>
  <tr>
	<td colspan="2">
	<asp:Button cssClass="FormBtn" id="btnLBPost" Text="Post" ToolTip="Just post back"
		runat="Server" />
	<asp:Button cssClass="FormBtn" id="btnLBDefault" Text="Default"
		ToolTip="Reset to default item" runat="Server" />
	<asp:Button cssClass="FormBtn" id="btnLBSetByValue" Text="By Value"
		ToolTip="Set by value" runat="Server" />
	<asp:Button cssClass="FormBtn" id="btnLBSetByText" Text="By Text"
		ToolTip="Set by text" runat="Server" />
	</td>
  </tr>
</table>
<br>
<table class="Shaded" cellSpacing="0" cellPadding="4" width="100%" border="0">
  <tr>
    <td width="50%" valign="top"><b>MinMaxListBox</b> is an
    <b>EWSListBox</b>-derived class that automatically generates a
    <b>MinMaxListValidator</b> for itself. The validator will ensure that the
    number of selections in the listbox control does not exceed a specified
    minimum and/or maximum number of entries.  Client-side script validation
    support is available.
	</td>
	<td width="25%" align="center" rowspan="2">
	<EWSC:MinMaxListBox id="lbMinMaxLB" runat="server" Rows="10" MinSel="1"
	  MinMaxErrorMessage="Select an item from the list" >
		<asp:ListItem>Item 1</asp:ListItem>
		<asp:ListItem>Item 2</asp:ListItem>
		<asp:ListItem>Item 3</asp:ListItem>
		<asp:ListItem>Item 4</asp:ListItem>
	</EWSC:MinMaxListBox>
	</td>
	<td width="25%" align="center" rowspan="2">
	<EWSC:MinMaxListBox id="lbMinMaxListBox" runat="server" Rows="10"
	  SelectionMode="Multiple" MinSel="2" MaxSel="5"
	  MinMaxErrorMessage="Select a minimum of {MinSel} and a maximum of {MaxSel} entries">
		<asp:ListItem>Item 1</asp:ListItem>
		<asp:ListItem>Item 2</asp:ListItem>
		<asp:ListItem>Item 3</asp:ListItem>
		<asp:ListItem>Item 4</asp:ListItem>
		<asp:ListItem>Item 5</asp:ListItem>
		<asp:ListItem>Item 6</asp:ListItem>
		<asp:ListItem>Item 7</asp:ListItem>
		<asp:ListItem>Item 8</asp:ListItem>
		<asp:ListItem>Item 9</asp:ListItem>
		<asp:ListItem>Item 10</asp:ListItem>
	</EWSC:MinMaxListBox>
	</td>
  </tr>
  <tr>
	<td><asp:Label id="lblLBSelections" runat="server" />&nbsp;</td>
  </tr>
  <tr>
	<td with="50%"><asp:Button cssClass="FormBtn" id="btnLBValidate" Text="Validate"
		ToolTip="Test min/max selection list boxes" runat="Server" />
	</td>
	<td valign="top" align="center">Select at least one item</td>
	<td valign="top" align="center">Select a minimum of two and a maximum of five items</td>
  </tr>
</table>
<br>
<table class="Shaded" cellSpacing="0" cellPadding="4" width="100%" border="0">
  <tr>
    <td width="50%" valign="top"><b>MinMaxCheckBoxList</b> is an
    <b>EWSCheckBoxList</b>-derived class that automatically generates a
    <b>MinMaxListValidator</b> for itself. The validator will ensure that the
    number of selections in the checkbox list control does not exceed a
    specified minimum and/or maximum number of entries.  Client-side script
    validation support is available.  <b>EWSCheckBoxList</b> and
    <b>MinMaxCheckBoxList</b> contain a similar set of properties and methods
	as their list box derived counterparts.  Although unusual, you can
    apply the <b>MinMaxListValidator</b> to a <b>RadioButtonList</b> control
    to enforce the selection of one item.  This would only be useful if the
    <b>RadioButtonList</b> has no selections when first rendered on the page
    as in this example.  The library also contains <b>EWSRadioButtonList</b>
    (not demonstrated) which has additional properties like
    <b>EWSCheckBoxList</b> such as <b>DefaultSelection</b> and
    <b>CurrentSelection</b>.
	</td>
	<td width="25%" align="center" rowspan="2">
	<EWSC:MinMaxCheckBoxList id="chkList" runat="server" MinSel="2" MaxSel="3"
	  MinMaxErrorMessage="Select a minimum of {MinSel} and a maximum of {MaxSel} checkbox entries">
		<asp:ListItem>Selection 1</asp:ListItem>
		<asp:ListItem>Selection 2</asp:ListItem>
		<asp:ListItem>Selection 3</asp:ListItem>
		<asp:ListItem>Selection 4</asp:ListItem>
	</EWSC:MinMaxCheckBoxList>
	</td>
	<td width="25%" align="center" rowspan="2">
	<asp:RadioButtonList id="rbList" runat="server">
		<asp:ListItem>Selection 1</asp:ListItem>
		<asp:ListItem>Selection 2</asp:ListItem>
		<asp:ListItem>Selection 3</asp:ListItem>
		<asp:ListItem>Selection 4</asp:ListItem>
	</asp:RadioButtonList>
	<EWSC:MinMaxListValidator id="mmValRB" runat="server" ControlToValidate="rbList"
		Display="None" ErrorMessage="Select an entry in the radiobutton list" />
	</td>
  </tr>
  <tr>
	<td><asp:Label id="lbCKBSelections" runat="server" />&nbsp;</td>
  </tr>
  <tr>
	<td with="50%"><asp:Button cssClass="FormBtn" id="btnCKRBValidate" Text="Validate"
		ToolTip="Test min/max selection checkbox list and radio button list" runat="Server" />
	</td>
	<td valign="top" align="center">Select a minimum of two and a maximum of three items</td>
	<td valign="top" align="center">Select a radio button item</td>
  </tr>
</table>

</form>
