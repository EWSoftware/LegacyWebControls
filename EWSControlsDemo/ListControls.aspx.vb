'==============================================================================
' System  : ASP.NET Web Control Library Demo
' File    : ListControls.aspx.vb
' Author  : Eric Woodruff  (Eric@EWoodruff.us)
' Updated : Sun 12/01/02 13:28:01
' Note    : Copyright 2002, Eric Woodruff, All rights reserved
' Compiler: Microsoft Visual VB.NET
'
' This demonstrates the list controls
'
' Version     Date     Who  Comments
' =============================================================================
' 1.0.0    11/26/2002  EFW  Created the code
'==============================================================================

Option Strict On

Imports EWSoftware.Web
Imports EWSoftware.Web.Controls


Namespace EWSControlsDemo


Partial Class ListControls
    Inherits EWSoftware.Web.MenuPage


#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    ' The DataView for the example grid
    Dim dvPhones As DataView

    ' This is called by the DataGrid to bind the data source to the
    ' Phone Type dropdown list.
    Protected Function funGetPhoneTypes() As SortedList
        Dim slPhoneTypes As New SortedList()

        slPhoneTypes("HP") = "Home Phone"
        slPhoneTypes("WP") = "Work Phone"
        slPhoneTypes("CP") = "Cell Phone"
        slPhoneTypes("EM") = "E-Mail Address"

        Return slPhoneTypes
    End Function

    ' Create a test data source for the example grid control
    Private Sub CreateDataSource()
        Dim dt As New DataTable()
        Dim dr As DataRow

        dt.Columns.Add(New DataColumn("PhoneKey", GetType(Int32)))
        dt.Columns.Add(New DataColumn("PhoneType", GetType(String)))
        dt.Columns.Add(New DataColumn("PhTypeDesc", GetType(String)))
        dt.Columns.Add(New DataColumn("PhoneNumb", GetType(String)))

        dr = dt.NewRow()
        dr(0) = 1
        dr(1) = "HP"
        dr(2) = "Home Phone"
        dr(3) = "(509) 555-1234"
        dt.Rows.Add(dr)

        dr = dt.NewRow()
        dr(0) = 2
        dr(1) = "WP"
        dr(2) = "Work Phone"
        dr(3) = "(509) 555-9876x1234"
        dt.Rows.Add(dr)

        dr = dt.NewRow()
        dr(0) = 3
        dr(1) = "EM"
        dr(2) = "E-Mail Address"
        dr(3) = "JoeUser@Test.com"
        dt.Rows.Add(dr)

        dvPhones = New DataView(dt)
    End Sub

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not Page.IsPostBack Then
            Me.PageTitle = "List Control Examples"

            ' Add some items to the drop down lists and list boxes
            Dim slTest As New SortedList()
            slTest("1") = "Item 1"
            slTest("2") = "Item 2"
            slTest("3") = "Item 3"
            slTest("4") = "Item 4"
            slTest("5") = "Item 5"

            cboEWSDropDownList.DataSource = slTest
            cboEWSDropDownList.DataValueField = "Key"
            cboEWSDropDownList.DataTextField = "Value"

            lbEWSListBox.DataSource = slTest
            lbEWSListBox.DataValueField = "Key"
            lbEWSListBox.DataTextField = "Value"

            ' The DefaultSelection property can be used before data binding
            ' has occurred.  It won't get used unless there is no current
            ' selection when the dropdown list or list box is finally rendered.
            cboEWSDropDownList.DefaultSelection = "3"
            lbEWSListBox.DefaultSelection = "3"

            Dim slTest2 As New SortedList()
            slTest2("") = "<Select a Value>"
            slTest2("A") = "Item A"
            slTest2("B") = "Item B"
            slTest2("C") = "Item C"
            slTest2("D") = "Item D"

            cboRequiredDropDownList.DataSource = slTest2
            cboRequiredDropDownList.DataValueField = "Key"
            cboRequiredDropDownList.DataTextField = "Value"

            ' Add some test records to the grid.  Save it to the
            ' session for the demo.
            Call CreateDataSource()
            dgPhones.DataSource = dvPhones
            Session("dvPhones") = dvPhones

            Page.DataBind()

            ' Disable the listbox, checkbox list, and radio button list
            ' controls until they are enabled.
            SetEnabledState(False, lbEWSListBox, btnLBPost, _
                btnLBDefault, btnLBSetByValue, btnLBSetByText, _
                lbMinMaxLB, lbMinMaxListBox, btnLBValidate, _
                chkList, rbList, mmValRB, lbCKBSelections, btnCKRBValidate)

            ' Set focus to the first dropdown list on the initial load
            SetFocus(cboEWSDropDownList)
        Else
            ' The CurrentSelection property is a shorthand way of
            ' getting the current value (SelectedItem.Value).  It can also
            ' be used to set the current item (see btnSetByValue_Click and
            ' btnLBSetByValue_Click).
            lblEWSDDMsg.Text = "Selection on post back: " & _
                cboEWSDropDownList.CurrentSelection & "<br>"
            lblEWSLBMsg.Text = "1st selected item on post back: " & _
                lbEWSListBox.CurrentSelection & "<br>"

            ' Restore the DataView for the demo
            dvPhones = DirectCast(Session("dvPhones"), DataView)
        End If
    End Sub

    ' This event is fired when the EWSDropDownList sets the current selection
    ' to the default selection because nothing else was selected when the
    ' control is rendered.  You can catch this event to handle other
    ' processing if necessary.  See the GridDropDownList for an example.
    Private Sub cboEWSDropDownList_DefaultSelected(ByVal sender As Object, _
      ByVal args As System.EventArgs) Handles cboEWSDropDownList.DefaultSelected
        lblEWSDDMsg.Text = lblEWSDDMsg.Text & "DefaultSelected event was fired<br>"
    End Sub

    ' Clear the selection.  This will force the control to make the
    ' default selection the current item.
    Private Sub btnDefault_Click(ByVal sender As Object, _
      ByVal e As System.EventArgs) Handles btnDefault.Click
        cboEWSDropDownList.SelectedIndex = -1
        lblEWSDDMsg.Text = lblEWSDDMsg.Text & "Selection cleared, default will take effect<br>"
        SetFocus(cboEWSDropDownList)
    End Sub

    ' The CurrentSelection property can also be used as a convenient way
    ' to set the current selection without having to manually find the item
    ' index that matches the value.
    Private Sub btnSetByValue_Click(ByVal sender As Object, _
      ByVal e As System.EventArgs) Handles btnSetByValue.Click
        cboEWSDropDownList.CurrentSelection = "5"
        lblEWSDDMsg.Text = lblEWSDDMsg.Text & "Item set by value (5)<br>"
        SetFocus(cboEWSDropDownList)
    End Sub

    ' You can also set the item by the display text instead of the item value.
    ' This can be helpful if you obtain a description and need to translate it
    ' to a value in the dropdown list.  You can specify a default if needed and
    ' whether or not comparisons are case-sensitive.
    Private Sub btnSetByText_Click(ByVal sender As Object, _
      ByVal e As System.EventArgs) Handles btnSetByText.Click
        cboEWSDropDownList.SetSelectionByText("item 2", Nothing, True)
        lblEWSDDMsg.Text = lblEWSDDMsg.Text & "Item set by text (item 2)<br>"
        SetFocus(cboEWSDropDownList)
    End Sub

    ' When checked, require a selection in cboRequiredDropDownList
    Private Sub chkRequireSel_CheckedChanged(ByVal sender As Object, _
      ByVal e As System.EventArgs) Handles chkRequireSel.CheckedChanged
        cboRequiredDropDownList.Required = chkRequireSel.Checked
        cboRequiredDropDownList.CurrentSelection = String.Empty
        SetFocus(cboRequiredDropDownList)
    End Sub

    ' This handles modification of the validation info for the phone number
    ' field when the phone type changes or when a row is being created
    ' for editing.  The GridDropDownList fires DropDownChanged when the
    ' selected index changes.  It fires DefaultSelected when the row is being
    ' created because the default is set to the current value for the row.
    Private Sub dgPhones_ItemCommand(ByVal source As Object, _
      ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgPhones.ItemCommand
        If e.CommandName = "DropDownChanged" Or e.CommandName = "DefaultSelected" Then
            Dim cboPhoneType As GridDropDownList = _
                DirectCast(e.Item.FindControl("cboPhoneType"), GridDropDownList)
            Dim txtPhoneNumb As PatternTextBox = _
                DirectCast(e.Item.FindControl("txtPhoneNumb"), PatternTextBox)

            ' Change the edit mask on the phone number control based on
            ' the type selected.  Clear the number when changed.
            If e.CommandName = "DropDownChanged" Then
                txtPhoneNumb.Text = String.Empty
            End If

            If cboPhoneType.CurrentSelection <> "EM" Then
                txtPhoneNumb.Pattern = PatternType.Phone
                txtPhoneNumb.PatternErrorMessage = "Enter a valid phone number ((999) 999-9999)"
                txtPhoneNumb.RequiredMessage = "A phone number is required"
            Else
                txtPhoneNumb.Pattern = PatternType.EMail
                txtPhoneNumb.PatternErrorMessage = "Enter a valid e-mail address (abc@xyz.com)"
                txtPhoneNumb.RequiredMessage = "An e-mail address is required"
            End If

            ' Set focus to the phone/e-mail field
            SetFocus("txtPhoneNumb")
        Else
            If e.CommandName = "Add" Then
                Dim tblPhones As DataTable, rowNew As DataRow

                ' Ignore the request if validation failed or we are
                ' already editing something else.
                If Page.IsValid <> True OrElse arvSave.ActionRequired = True Then
                    Exit Sub
                End If

                tblPhones = dvPhones.Table
                rowNew = tblPhones.NewRow()
                rowNew.Item("PhoneKey") = -1    ' New row
                rowNew.Item("PhoneType") = "WP"
                tblPhones.Rows.Add(rowNew)

                dgPhones.EditItemIndex = tblPhones.Rows.Count - 1
                dgPhones.DataSource = dvPhones
                dgPhones.DataBind()

                Session("dvPhones") = dvPhones

                ' Set the ActionRequiredValidator flag
                arvSave.ActionRequired = True
                SetFocus("cboPhoneType")
            End If
        End If
    End Sub

    Private Sub dgPhones_EditCommand(ByVal source As Object, _
      ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) _
      Handles dgPhones.EditCommand
        If Page.IsValid <> True OrElse arvSave.ActionRequired = True Then
            Exit Sub
        End If

        ' Set the item to edit and bind the phone info grid to its data source
        dgPhones.EditItemIndex = e.Item.ItemIndex
        dgPhones.DataSource = dvPhones
        dgPhones.DataBind()

        ' Set the ActionRequiredValidator flag
        arvSave.ActionRequired = True
        SetFocus("cboPhoneType")
    End Sub

    Private Sub dgPhones_UpdateCommand(ByVal source As Object, _
      ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) _
      Handles dgPhones.UpdateCommand
        Dim nPhoneKey As Integer = Int32.Parse(e.Item.Cells(0).Text)

        If Page.IsValid <> True Then
            Exit Sub
        End If

        ' Add or update the phone entry
        Dim dr As DataRow = dvPhones.Table.Rows(e.Item.ItemIndex)

        dr(0) = e.Item.ItemIndex
        dr(1) = DirectCast(e.Item.Cells(1).FindControl("cboPhoneType"), _
            GridDropDownList).CurrentSelection
        dr(2) = DirectCast(e.Item.Cells(1).FindControl("cboPhoneType"), _
            GridDropDownList).SelectedItem.Text
        dr(3) = DirectCast(e.Item.Cells(2).FindControl("txtPhoneNumb"), _
            PatternTextBox).Text

        ' Bind the phone info grid to its data source
        dgPhones.EditItemIndex = -1
        dgPhones.DataSource = dvPhones
        dgPhones.DataBind()

        Session("dvPhones") = dvPhones

        ' Turn off the ActionRequiredValidator flag
        arvSave.ActionRequired = False
    End Sub

    Private Sub dgPhones_CancelCommand(ByVal source As Object, _
      ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) _
      Handles dgPhones.CancelCommand
        Dim nPhoneKey As Integer = Int32.Parse(e.Item.Cells(0).Text)

        ' Delete the row if it was a newly added row
        If nPhoneKey = -1 Then
            dvPhones.Table.Rows(dvPhones.Table.Rows.Count - 1).Delete()
        End If

        ' Cancel update and bind the phone info grid to its data source
        dgPhones.EditItemIndex = -1
        dgPhones.DataSource = dvPhones
        dgPhones.DataBind()

        ' Turn off the ActionRequiredValidator flag
        arvSave.ActionRequired = False
    End Sub

    ' Handle deleting a grid entry.  This gets called by the ConfirmLinkButton
    Private Sub dgPhones_DeleteCommand(ByVal source As Object, _
      ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) _
      Handles dgPhones.DeleteCommand
        ' If an add or edit is in progress on another row, ignore the request
        If dgPhones.EditItemIndex <> -1 And _
          dgPhones.EditItemIndex <> e.Item.ItemIndex Then
            ' Set the IsValid flag to false to show the message
            arvSave.IsValid = False
            Exit Sub
        End If

        ' Delete the entry
        dvPhones.Table.Rows(e.Item.ItemIndex).Delete()

        ' Cancel any update in progress and bind the phone info grid to its data source
        dgPhones.EditItemIndex = -1
        dgPhones.DataSource = dvPhones
        dgPhones.DataBind()

        Session("dvPhones") = dvPhones

        ' Turn off the ActionRequiredValidator flag
        arvSave.ActionRequired = False
    End Sub

    ' Delete all grid entries.  This gets called by the ConfirmButton
    Private Sub btnRemoveAll_Click(ByVal sender As Object, _
      ByVal e As System.EventArgs) Handles btnRemoveAll.Click
        ' If an add or edit is in progress, ignore the request
        If dgPhones.EditItemIndex <> -1 Then
            ' Set the IsValid flag to false to show the message
            arvSave.IsValid = False
            Exit Sub
        End If

        dvPhones.Table.Rows.Clear()
        dgPhones.DataSource = dvPhones
        dgPhones.DataBind()

        Session("dvPhones") = dvPhones
    End Sub

    ' This event is fired when the EWSListBox sets the current selection
    ' to the default selection because nothing else was selected when the
    ' control is rendered.  You can catch this event to handle other
    ' processing if necessary.
    Private Sub lbEWSListBox_DefaultSelected(ByVal sender As Object, _
      ByVal args As System.EventArgs) Handles lbEWSListBox.DefaultSelected
        lblEWSLBMsg.Text = lblEWSLBMsg.Text & "DefaultSelected event was fired<br>"
    End Sub

    ' Clear the selection in the list box.  This will force the control to
    ' make the default selection the current item.
    Private Sub btnLBDefault_Click(ByVal sender As Object, _
      ByVal e As System.EventArgs) Handles btnLBDefault.Click
        Dim strDefaults() As String = { "1", "3", "5" }

        ' For the list box, DefaultSelections is like DefaultSelection
        ' but it accepts a string array so that multiple selections can
        ' be selected in a list box with multiple selection enabled.
        lbEWSListBox.DefaultSelections = strDefaults

        lbEWSListBox.SelectedIndex = -1
        lblEWSLBMsg.Text = lblEWSLBMsg.Text & "Selection cleared, default(s) will take effect<br>"
        SetFocus(lbEWSListBox)
    End Sub

    ' The CurrentSelection and CurrentSelections property can also be used
    ' as a convenient way to set the current selection(s) without having to
    ' manually find the item index that matches the value.
    Private Sub btnLBSetByValue_Click(ByVal sender As Object, _
      ByVal e As System.EventArgs) Handles btnLBSetByValue.Click
        Dim strItems() As String = { "2", "4" }

        If lbEWSListBox.SelectedIndices.Count = 1 Then
            lbEWSListBox.CurrentSelection = "5"
            lblEWSLBMsg.Text = lblEWSLBMsg.Text & "Item set by value (5)<br>"
        Else
            lbEWSListBox.CurrentSelections = strItems
            lblEWSLBMsg.Text = lblEWSLBMsg.Text & "Items set by value (2, 4)<br>"
        End If

        SetFocus(lbEWSListBox)
    End Sub

    ' You can also set the item by the display text instead of the item value.
    ' This can be helpful if you obtain a description and need to translate it
    ' to a value in the list box.  You can specify a default if needed and
    ' whether or not comparisons are case-sensitive.
    Private Sub btnLBSetByText_Click(ByVal sender As Object, _
      ByVal e As System.EventArgs) Handles btnLBSetByText.Click
        lbEWSListBox.SetSelectionByText("item 2", Nothing, True)
        lblEWSLBMsg.Text = lblEWSLBMsg.Text & "Item set by text (item 2)<br>"
        SetFocus(lbEWSListBox)
    End Sub

    ' This demonstrates the use of the SelectedItems and SelectedIndices properties
    ' of EWSListBox and the derived MinMaxListBox.
    Private Sub btnLBValidate_Click(ByVal sender As Object, _
      ByVal e As System.EventArgs) Handles btnLBValidate.Click
        Dim siColl As SelectedItemsCollection = lbMinMaxListBox.SelectedItems
        Dim siInd As SelectedIndicesCollection = lbMinMaxListBox.SelectedIndices
        Dim liItem As ListItem

        lblLBSelections.Text = "<b>Selected Items When Validated</b><br>"

        ' Iterate over the collection...
        For Each liItem In siColl
            lblLBSelections.Text = lblLBSelections.Text & liItem.Value & "&nbsp;&nbsp;"
        Next

        lblLBSelections.Text = lblLBSelections.Text & "<br><br><b>Selected Indices When Validated</b><br>"

        ' ...or use ToString(), both collections override it to return their
        ' lists in a single string of comma-separated values.
        lblLBSelections.Text = lblLBSelections.Text & siInd.ToString & "&nbsp;&nbsp;"

        SetFocus(lbMinMaxListBox)
    End Sub

    ' This demonstrates the use of the SelectedItems and SelectedIndices properties
    ' of EWSCheckBoxList and the derived MinMaxCheckBoxList.
    Private Sub btnCKRBValidate_Click(ByVal sender As Object, _
      ByVal e As System.EventArgs) Handles btnCKRBValidate.Click
        Dim siColl As SelectedItemsCollection = chkList.SelectedItems
        Dim siInd As SelectedIndicesCollection = chkList.SelectedIndices
        Dim liItem As ListItem

        lbCKBSelections.Text = "<b>Selected CheckBox List Items When Validated</b><br>"

        ' Iterate over the collection...
        For Each liItem In siColl
            lbCKBSelections.Text = lbCKBSelections.Text & liItem.Value & "&nbsp;&nbsp;"
        Next

        lbCKBSelections.Text = lbCKBSelections.Text & "<br><br><b>Selected Indices When Validated</b><br>"

        ' ...or use ToString(), both collections override it to return their
        ' lists in a single string of comma-separated values.
        lbCKBSelections.Text = lbCKBSelections.Text & siInd.ToString & "&nbsp;&nbsp;"

        SetFocus(chkList)
    End Sub

    Private Sub chkEnableDemos_CheckedChanged(ByVal sender As Object, _
      ByVal e As System.EventArgs) Handles chkEnableDemos.CheckedChanged
        ' Enable/disable the listbox, checkbox list, and radio button list controls
        SetEnabledState(chkEnableDemos.Checked, lbEWSListBox, _
            btnLBPost, btnLBDefault, btnLBSetByValue, btnLBSetByText, _
            lbMinMaxLB, lbMinMaxListBox, btnLBValidate, _
            chkList, rbList, mmValRB, lbCKBSelections, btnCKRBValidate)

        SetFocus(chkEnableDemos)
    End Sub
End Class

End Namespace
