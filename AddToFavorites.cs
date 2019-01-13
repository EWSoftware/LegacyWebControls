//=============================================================================
// System  : ASP.NET Web Control Library
// File    : AddToFavorites.cs
// Author  : Eric Woodruff  (Eric@EWoodruff.us)
// Updated : Fri 02/20/04
// Note    : Copyright 2002-2004, Eric Woodruff, All rights reserved
// Compiler: Microsoft Visual C#
//
// This file contains a derived Control class that can be used to render an
// "Add to Favorites" hyperlink or label to a web form.
//
// Version     Date     Who  Comments
// ============================================================================
// 1.0.0    02/11/2003  EFW  Created the code
//=============================================================================

using System;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using EWSoftware.Web.Design;

namespace EWSoftware.Web.Controls
{
    /// <summary>
    /// This control will render as a hyperlink if rendering on Internet
    /// Explorer 4+ allowing the user to add the current page or a different
    /// specified page to their Favorites list.  On Netscape and all other
    /// browsers, it renders as a label telling the user how to bookmark the
    /// current page.
    /// </summary>
	[DefaultProperty("IEText"),
		ToolboxData("<{0}:AddToFavorites runat=server />"),
        Designer(typeof(AddToFavoritesDesigner))]
	public class AddToFavorites : System.Web.UI.Control, INamingContainer
	{
        //=====================================================================
        // Private class members
        private Style sLink, sText;     // Styles

        //=====================================================================
        // Properties

        /// <summary>
        /// This is overridden to ensure that the child controls are always
        /// created when needed.
        /// </summary>
        [Browsable(false)]
        public override ControlCollection Controls
        {
            get
            {
                EnsureChildControls();
                return base.Controls;
            }
        }

	    /// <summary>
        /// This property is used to set the text displayed for the hyperlink
        /// that can be clicked to add the page to the Favorites list when
        /// rendered in Internet Explorer 4+.
	    /// </summary>
        /// <value>If not set, it defaults to "Add to Favorites".</value>
		[Category("Appearance"), DefaultValue("Add to Favorites"), Bindable(true),
         Description("The text to display in the Internet Explorer hyperlink")]
		public string IEText
		{
			get
			{
	            Object oText = ViewState["IEText"];
                return (oText == null) ? "Add to Favorites" : (string)oText;
            }
			set { ViewState["IEText"] = value; }
		}

	    /// <summary>
        /// This property is used to set the text displayed for Netscape
        /// browsers to tell the user how to bookmark the page.
	    /// </summary>
        /// <value>If not set, it defaults to the text "To bookmark this
        /// site, click <b>Bookmarks | Bookmark This Page</b> or press
        /// <b>Ctrl+D</b>."</value>
        [Category("Appearance"), Bindable(true),
            DefaultValue("To bookmark this site, click <b>Bookmarks | " +
                "Bookmark This Page</b> or press <b>Ctrl+D</b>."),
            Description("The text to display for Netscape browsers")]
		public string NSText
		{
			get
			{
	            Object oText = ViewState["NSText"];
                return (oText == null) ?
                    "To bookmark this site, click <b>Bookmarks | " +
                    "Bookmark This Page</b> or press <b>Ctrl+D</b>." :
                    (string)oText;
            }
			set { ViewState["NSText"] = value; }
		}

	    /// <summary>
        /// This property is used to set the text displayed to tell the user
        /// how to bookmark the page manually for browsers other than IE or
        /// Netscape or if the <see cref="ForceLabel"/> property is set to true.
	    /// </summary>
        /// <value>If not set, it defaults to a non-breaking space (no
        /// message displayed).</value>
		[Category("Appearance"), DefaultValue("&nbsp;"), Bindable(true),
         Description("The text to display for unknown browsers")]
		public string OtherText
		{
			get
			{
	            Object oText = ViewState["OtherText"];
                return (oText == null) ? "&nbsp;" : (string)oText;
            }
			set { ViewState["OtherText"] = value; }
		}

	    /// <summary>
        /// This property is used to set the tooltip displayed for the
        /// Internet Explorer hyperlink.
	    /// </summary>
        /// <value>If not set, it defaults to "Add this page to your
        /// favorites list".</value>
		[Category("Appearance"), Bindable(true),
            DefaultValue("Add this page to your favorites list"),
            Description("The tooltip to display on the hyperlink")]
		public string IEToolTip
		{
			get
			{
	            Object oTip = ViewState["IETip"];
                return (oTip == null) ?
                    "Add this page to your favorites list" : (string)oTip;
            }
			set { ViewState["IETip"] = value; }
		}

	    /// <summary>
        /// This property is used to set the URL for the Favorites entry
        /// when adding the page to the Favorites list in Internet Explorer.
	    /// </summary>
        /// <value>If not set, the current page's URL will be used.</value>
		[Category("Appearance"), Bindable(true),
         Description("The URL to use for the page in the Favorites list")]
		public string FavoritesURL
		{
			get { return (string)ViewState["FavURL"]; }
			set { ViewState["FavURL"] = value; }
		}

	    /// <summary>
        /// This property is used to set the text for the Favorites title
        /// when adding the page to the Favorites list in Internet Explorer.
	    /// </summary>
        /// <value>If not set, the actual page title will be used as returned
        /// by the JavaScript code <b>document.title</b>.</value>
		[Category("Appearance"), Bindable(true),
         Description("The title to use for the page in the Favorites list")]
		public string FavoritesText
		{
			get { return (string)ViewState["FavText"]; }
			set { ViewState["FavText"] = value; }
		}

	    /// <summary>
        /// This property is used to set the hyperlink style when rendered in
        /// Internet Explorer 4+.
	    /// </summary>
        /// <value>If not set, the hyperlink style for the current page
        /// is used.</value>
		[Category("Appearance"), Bindable(true),
         Description("The style to use for the hyperlink in Internet Explorer")]
		public Style IEStyle
		{
			get
            {
                if(sLink == null)
                    sLink = new Style();

                return sLink;
			}
            set { sLink = value; }
		}

	    /// <summary>
        /// This property is used to set the text style when rendered in
        /// any other browser including Netscape.
	    /// </summary>
        /// <value>If not set, the body text style for the current page
        /// is used.</value>
		[Category("Appearance"), Bindable(true),
         Description("The text style to use in other browsers")]
		public Style OtherStyle
		{
			get
            {
                if(sText == null)
                    sText = new Style();

                return sText;
			}
            set { sText = value; }
		}

        /// <summary>
        /// This property is used to force use of the label control.  It will
        /// display the message stored in the <see cref="OtherText"/> property.
        /// </summary>
        /// <value>If not set to true, the control will determine how to
        /// render itself based on the client's browser.</value>
		[Category("Behavior"), DefaultValue(false), Bindable(true),
         Description("Force the use of a label to display OtherText")]
        public bool ForceLabel
		{
			get
			{
	            Object oForce = ViewState["ForceLbl"];
                return (oForce == null) ? false : (bool)oForce;
            }
			set { ViewState["ForceLbl"] = value; }
		}

        //=====================================================================
        // Methods, etc

	    /// <summary>
        /// This method is overridden to generate the hyperlink or label
        /// control that will be rendered on the client.
	    /// </summary>
        protected override void CreateChildControls()
        {
            HttpBrowserCapabilities bc = null;
            Style sStyle = new Style();

            // Not available at design time
            if(this.Context != null)
                bc = Context.Request.Browser;

            if(!this.ForceLabel && (bc == null ||
              (bc.Browser == "IE" && bc.MajorVersion > 3)))
            {
                // For Internet Explorer 4+, use a hyperlink
    			sStyle.MergeWith(this.IEStyle);

                StringBuilder strLink = new StringBuilder(1024);

                strLink.Append("javascript:window.external.AddFavorite(");
                strLink.Append('\"');

                // Use the current page's URL if nothing was specified
                if(bc != null && this.FavoritesURL == null)
                    strLink.Append(this.Context.Request.Url.ToString());
                else
                    strLink.Append(this.FavoritesURL);

                strLink.Append("\", ");

                // Use the current page's title if nothing was specified
                if(FavoritesText == null)
                    strLink.Append("document.title");
                else
                {
                    strLink.Append('\"');
                    strLink.Append(this.FavoritesText);
                    strLink.Append('\"');
                }

                strLink.Append(')');

                HyperLink hlLink = new HyperLink();
                hlLink.ApplyStyle(sStyle);
                hlLink.NavigateUrl = strLink.ToString();
                hlLink.Text = this.IEText;
                hlLink.ToolTip = this.IEToolTip;
                hlLink.ID = "hlAddFav";
                this.Controls.Add(hlLink);
            }
            else
            {
                // For Netscape and other browsers, just show a label
	    		sStyle.MergeWith(this.OtherStyle);

                Label lblText = new Label();
                lblText.ApplyStyle(sStyle);

                // Show the Netscape message unless it isn't that one
                // or ForceLabel is set to true.
                if(!this.ForceLabel && (bc == null || bc.Browser == "Netscape"))
                    lblText.Text = this.NSText;
                else
                    lblText.Text = this.OtherText;

                lblText.ID = "lblAddFav";
                this.Controls.Add(lblText);
            }
        }

        /// <summary>
        /// This is called by the <see cref="EWSoftware.Web.Design.AddToFavoritesDesigner"/>
        /// control designer class when rendering at design time.
        /// </summary>
        internal void RenderAtDesignTime()
        {
            this.Controls.Clear();
            CreateChildControls();
        }
    }
}
