'==============================================================================
' System  : ASP.NET Web Control Library Demo
' File    : EWSCtlDemo.aspx.vb
' Author  : Eric Woodruff  (Eric@EWoodruff.us)
' Updated : Sun 12/01/02 13:23:37
' Note    : Copyright 2002, Eric Woodruff, All rights reserved
' Compiler: Microsoft Visual VB.NET
'
' This is the About/start-up page for the application
'
' Version     Date     Who  Comments
' =============================================================================
' 1.0.0    11/26/2002  EFW  Created the code
'==============================================================================

Option Strict

Imports EWSoftware.Web
Imports EWSoftware.Web.Controls

Namespace EWSControlsDemo

Partial Class EWSCtlDemo
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
            PageTitle = "Eric Woodruff ASP.NET Web Controls Demo"

            ' Retrieve version from application settings in Web.Config
            lblVersion.Text = ConfigurationManager.AppSettings("Version")
            lblReleaseDate.Text = ConfigurationManager.AppSettings("ReleaseDate")
        End If
    End Sub

End Class

End Namespace
