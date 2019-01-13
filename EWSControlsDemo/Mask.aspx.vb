'==============================================================================
' System  : ASP.NET Web Control Library Demo
' File    : Mask.aspx.vb
' Author  : Eric Woodruff  (Eric@EWoodruff.us)
' Updated : 12/14/2003
' Note    : Copyright 2002-2003, Eric Woodruff, All rights reserved
' Compiler: Microsoft VB.NET
'
' This demonstrates the MaskTextBox control
'
' Version     Date     Who  Comments
' =============================================================================
' 1.0.0    12/14/2003  EFW  Created the code
'==============================================================================

Option Strict

Imports EWSoftware.Web
Imports EWSoftware.Web.Controls


Namespace EWSControlsDemo


Partial Class Mask
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

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not Page.IsPostBack Then
            PageTitle = "MaskTextBox Examples"
        End If

        SetFocus(txtMaskTextBox)
    End Sub

End Class

End Namespace
