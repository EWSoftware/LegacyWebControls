<%@ Register TagPrefix="EWSC" Namespace="EWSoftware.Web.Controls" Assembly="EWSoftware.Web.Controls" %>
<%@ Page Language="vb" AutoEventWireup="false" Inherits="EWSControlsDemo.Utilities" CodeFile="Utilities.aspx.vb" %>

<form id="frmUtilities" method="post" runat="server">
<h2>AddToFavorites</h2>
The <b>AddToFavorites</b> control can be used to add a link or label to a
page to allow the user to add the page to their favorites/bookmarks list.  The
control renders as a link on Internet Explorer 4+ and as a label on Netscape
and all other browsers.<br>
<br>
<div align="center">
Default link to add the current page:
<EWSC:AddToFavorites id="atfFav" runat="Server" OtherText="Not supported on this browser" /><br><br>
Custom link to add a different page (IE only):
<EWSC:AddToFavorites id="atfFavText" runat="Server" IEText="Add a hard coded link to your favorites"
	FavoritesURL="http://www.microsoft.com" FavoritesText="Custom Link Text"
	IEToolTip="Add to favorites list" NSText="Not supported for Netscape"
	OtherText="Not supported for this browser" /><br><br>
</div>

<h2>FileLink</h2>
The <b>FileLink</b> control can be used to render a link for a file along with
an image that indicates its type and the size of the file.  The control renders
the URL as a normal hyperlink and, if defined, the specified image following it.
If the file can be found and it is wanted, the file size can also be displayed
in parentheses following the link.  The size is expressed in bytes, kilobytes,
or megabytes depending on the size of the file.<br>
<br>
<div align="center">
<EWSC:FileLink id="flLink1" runat="server" Text="An image" URL="Images/EWSCtls.bmp"
	FileImage="Images/PDF.gif" ToolTip="It's not really a PDF file" ShowFileSize="True" />
</div>

<h2>WindowOpener</h2>
The <b>WindowOpener</b> control can be used to render a link or a button to a
page that, when clicked, will open another browser window.  It supports all of
the options to enable or disable various aspects of the opened window such as
toolbars, status bars, scrollbars, etc. and can also set the windows initial
position and height.<br>
<br>
<div align="center">
Rendered as a button, opened window uses default options:&nbsp;&nbsp;&nbsp;
<EWSC:WindowOpener id="woButton" runat="server" ControlStyle-CssClass="FormBtn"
    URL="http://www.microsoft.com" ToolTip="Open window" Name="Popup1" />
<br><br>
Rendered as a link, opened window is resizeable with scrollbars only:&nbsp;&nbsp;&nbsp;
<EWSC:WindowOpener id="woLink" runat="server" Button="False" Text="Click Here"
    URL="http://www.microsoft.com" ToolTip="Open window" Left="100" Top="75"
    EnableOptionsString="Resizable,Scrollbars" Height="200" Width="200"
	Name="Popup2" />
</div>

<h2>CtrlUtils</h2>
The <b>CtrlUtils</b> class contains some static utility methods that are
used by the control classes and can be used by you as well.  The following
sections describe the available methods.

<h3>OpenWindowJS</h3>
This method takes a URL as a parameter and returns a string containing a
JavaScript call to the <b>window.open</b> method that uses the specified
options to open a browser window and give it the focus.  This method is
used by the <b>WindowOpener</b> class above but can be used by your own
code as well.

<h3>ReplaceParameters</h3>
This method takes a variable argument list and performs replacements on
the specified message string.  The passed list must have a length that is
a multiple of two.  The first element is the match string, the second element
is the replacement string, repeat for as many replacement pairs as needed.
The control classes use this method to substitute the replaceable parameters
in message strings such as <b>{MinVal}</b> and <b>{MaxVal}</b>.

<h3>ToProperCase</h3>
This method converts a string to proper case (i.e. a name).  The <b>EWSTextBox</b>
class uses this method when its <b>Casing</b> property is set to <b>Proper</b>.
</form>
