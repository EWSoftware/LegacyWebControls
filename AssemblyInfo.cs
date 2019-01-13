//=============================================================================
// System  : ASP.NET Web Control Library
// File    : AssemblyInfo.cs
// Author  : Eric Woodruff  (Eric@EWoodruff.us)
// Updated : 09/30/2010
// Note    : Copyright 2002-2010, Eric Woodruff, All rights reserved
// Compiler: Microsoft Visual C#
//
// ASP.NET Web Control Library developed by Eric Woodruff
//
// Version     Date     Who  Comments
// ============================================================================
// 1.0.0.0  10/03/2002  EFW  Created the code
// 2.0.0.0  02/16/2006  EFW  Updated for use with .NET 2.0
// 3.0.0.0  09/30/2010  EFW  Updated for use with .NET 4.0
//=============================================================================

using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Web.UI;

//
// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
//
[assembly: AssemblyProduct("EWSoftware ASP.NET Web Control Library")]
[assembly: AssemblyTitle("ASP.NET Web Control Library")]
[assembly: AssemblyDescription("A collection of useful web control classes")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Eric Woodruff (Eric@EWoodruff.us)")]
[assembly: AssemblyCopyright("Copyright \xA9 2002-2010, Eric Woodruff, All Rights Reserved")]
[assembly: AssemblyTrademark("Eric Woodruff, All Rights Reserved")]
[assembly: AssemblyCulture("")]
[assembly: CLSCompliant(true)]
[assembly: ComVisible(false)]

//
// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version
//      Build Number
//      Revision
//
// Always specify the version value
[assembly: AssemblyVersion("3.0.0.0")]

//
// Tag prefix for the custom controls
//
[assembly: TagPrefix("EWSoftware.Web.Controls", "EWSC") ]

// Define the embedded resources
[assembly: WebResource(EWSoftware.Web.Controls.CtrlUtils.ScriptsPath +
    "CCValidator.js", "text/javascript")]

[assembly: WebResource(EWSoftware.Web.Controls.CtrlUtils.ScriptsPath +
    "DateTextBox.js", "text/javascript", PerformSubstitution=true)]

[assembly: WebResource(EWSoftware.Web.Controls.CtrlUtils.ScriptsPath +
    "MaskTextBox.js", "text/javascript")]

[assembly: WebResource(EWSoftware.Web.Controls.CtrlUtils.ScriptsPath +
    "MinMaxListValidator.js", "text/javascript")]

[assembly: WebResource(EWSoftware.Web.Controls.CtrlUtils.ScriptsPath +
    "NumericTextBox.js", "text/javascript")]

[assembly: WebResource(EWSoftware.Web.Controls.CtrlUtils.ScriptsPath +
    "PatternTextBox.js", "text/javascript")]

[assembly: WebResource(EWSoftware.Web.Controls.CtrlUtils.ScriptsPath +
    "SelectIncrSearch.js", "text/javascript")]

[assembly: WebResource(EWSoftware.Web.Controls.CtrlUtils.HtmlPath +
    "Calendar.html", "text/html")]

[assembly: WebResource(EWSoftware.Web.Controls.CtrlUtils.HtmlPath +
    "Calendar.bmp", "image/bmp")]
