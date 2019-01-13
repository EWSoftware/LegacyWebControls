'==============================================================================
' File    : MenuCtrl.ascx.vb
' Author  : Eric Woodruff  (Eric@EWoodruff.us)
' Updated : 11/21/2002
' Compiler: VB.NET
'
' This is a user control that renders a menu.  An image appears at the top
' followed by the menu items.  The image name, alternate text, and the menu
' XSD/XML file names are loaded from Web.Config application settings.
'
' NOTE: Due to the help event handler, this control cannot be cached!
'
'    Date     Who  Comments
' =============================================================================
' 07/18/2002  EFW  Created the code
'==============================================================================

Option Strict

Imports System.Text

Imports EWSoftware.Web.Controls


Namespace EWSControlsDemo


Partial  Class MenuCtrl
    Inherits System.Web.UI.UserControl


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

    ' This function is called by the data list control to add an entry
    ' to the list.
    Protected Function funAddMenuLink(diItem As DataListItem) As String
        Dim strLink As New StringBuilder(1024), strMenuText As String

        strMenuText = DataBinder.Eval(diItem.DataItem, "MenuText").ToString

        strLink.Append("<a class='MenuLink' title='")
        strLink.Append(DataBinder.Eval(diItem.DataItem, "MenuTip").ToString)

        ' If this isn't the help link, add a normal "A HREF" link.  If it is
        ' the help link, change it to show help for the parent page.
        If strMenuText <> "Help" Then
            strLink.Append("' href='")
            strLink.Append(DataBinder.Eval(diItem.DataItem, "MenuLink").ToString)
            strLink.Append("'>")
            strLink.Append(strMenuText)
            strLink.Append("</a>")
        Else
            strLink.Append("' href='javascript: return false;' onclick=")
            strLink.Append(Chr(34))

            Dim strURL As String = "Help.aspx?Key="
            strURL += TypeName(Parent.Parent)

            strLink.Append(CtrlUtils.OpenWindowJS(strURL, WindowTarget.Same, _
                50, 50, 500, 700, _
                WindowOptions.Resizable Or WindowOptions.Scrollbars, _
                False, Nothing))

            strLink.Append(" return false;")
            strLink.Append(Chr(34))
            strLink.Append(">")
            strLink.Append(strMenuText)
            strLink.Append("</a>")
        End If

        Return strLink.ToString
    End Function

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not IsPostBack Then
            Dim dsMenu As New DataSet()

            ' Get image and title from Web.Config
            imgLogo.ImageUrl = ConfigurationManager.AppSettings("LogoImage")
            imgLogo.AlternateText= ConfigurationManager.AppSettings("LogoText")
            lblMenuAppName.Text = ConfigurationManager.AppSettings("MenuAppName")

            dsMenu.ReadXmlSchema(Server.MapPath( _
                ConfigurationManager.AppSettings("AppMenuXSD")))
            dsMenu.ReadXml(Server.MapPath( _
                ConfigurationManager.AppSettings("AppMenuXML")))

            dlMenu.DataSource = dsMenu.Tables(0).DefaultView
            dlMenu.DataBind()
        End If
    End Sub

End Class

End Namespace
