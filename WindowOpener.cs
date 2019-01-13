//=============================================================================
// System  : ASP.NET Web Control Library
// File    : WindowOpener.cs
// Author  : Eric Woodruff  (Eric@EWoodruff.us)
// Updated : Fri 02/20/04
// Note    : Copyright 2002-2004, Eric Woodruff, All rights reserved
// Compiler: Microsoft Visual C#
//
// This file contains a derived Control class that can be used to render a
// button or hyperlink to a web form that opens another browser window.
//
// Version     Date     Who  Comments
// ============================================================================
// 1.0.0    09/03/2003  EFW  Created the code
//=============================================================================

using System;
using System.Collections;
using System.ComponentModel;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

using EWSoftware.Web.Design;

namespace EWSoftware.Web.Controls
{
    /// <summary>
    /// This control can be rendered as a button or a hyperlink that opens
    /// another browser window when clicked.
    /// </summary>
	[DefaultProperty("Text"),
		ToolboxData("<{0}:WindowOpener runat=server />"),
        Designer(typeof(WindowOpenerDesigner))]
	public class WindowOpener : System.Web.UI.Control, INamingContainer
	{
        //=====================================================================
        // Private class members
        private Style sCtlStyle;

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
        /// Set or get whether control is rendered as a button.
	    /// </summary>
        /// <value>Renders as a button if set to true (the default) or a
        /// hyperlink if set to false.</value>
		[Category("Behavior"), DefaultValue(true), Bindable(true),
         Description("Render as a button if true, a hyperlink if false")]
		public bool Button
		{
			get
			{
	            Object oButton = ViewState["Button"];
                return (oButton == null) ? true : (bool)oButton;
            }
			set { ViewState["Button"] = value; }
		}

	    /// <summary>
        /// This property is used to set the text displayed for the
        /// button or hyperlink.
	    /// </summary>
        /// <value>If not set, it defaults to <b>Open</b>.</value>
		[Category("Appearance"), DefaultValue("Open"), Bindable(true),
         Description("The text to display on the button or hyperlink")]
		public string Text
		{
			get
			{
	            Object oText = ViewState["Text"];
                return (oText == null) ? "Open" : (string)oText;
            }
			set { ViewState["Text"] = value; }
		}

	    /// <summary>
        /// This property is used to set the tool tip displayed for the
        /// button or hyperlink.
	    /// </summary>
        /// <value>If not set, no tooltip is rendered.</value>
		[Category("Appearance"), Bindable(true),
         Description("The tool tip to display on the button or hyperlink")]
		public string ToolTip
		{
			get
			{
	            Object oTip = ViewState["ToolTip"];
                return (oTip == null) ? null : (string)oTip;
            }
			set { ViewState["ToolTip"] = value; }
		}

	    /// <summary>
        /// This property is used to set the style of the button or hyperlink.
	    /// </summary>
        /// <value>If not set, the button or hyperlink style for the current
        /// page is used.</value>
		[Category("Appearance"), Bindable(true),
         Description("The style to use for the button or hyperlink")]
		public Style ControlStyle
		{
			get
            {
                if(sCtlStyle == null)
                    sCtlStyle = new Style();

                return sCtlStyle;
			}

            set { sCtlStyle = value; }
		}

	    /// <summary>
        /// This property is used to set the URL opened in the window
	    /// </summary>
        /// <value>If not set, it defaults to "about:blank".</value>
		[Category("Navigation"), DefaultValue("about:blank"), Bindable(true),
         Description("The URL to open in the window")]
		public string URL
		{
			get
			{
	            Object oURL = ViewState["URL"];
                return (oURL == null) ? "about:blank" : (string)oURL;
            }
			set { ViewState["URL"] = value; }
		}

	    /// <summary>
        /// This property is used to set or get the target location in which
        /// to open the window.
	    /// </summary>
        /// <value>If not set, it defaults to <b>Same</b> and the URL will
        /// be opened in a new window or the existing one if already open.</value>
		[Category("Navigation"), DefaultValue(WindowTarget.Same), Bindable(true),
         Description("The target location in which to open the window")]
		public WindowTarget Target
		{
			get
			{
	            Object oTarget = ViewState["Target"];
                return (oTarget == null) ? WindowTarget.Same :
                    (WindowTarget)oTarget;
            }
			set { ViewState["Target"] = value; }
		}

	    /// <summary>
        /// Set or get the position of the left side of the window.
	    /// </summary>
        /// <value>If not set, it defaults to 50.  The minimum value is 0.</value>
		[Category("Appearance"), DefaultValue(50), Bindable(true),
         Description("Set or get the position of the left side of the window")]
		public int Left
		{
			get
			{
	            Object oLeft = ViewState["Left"];
                return (oLeft == null) ? 50 : (int)oLeft;
            }
			set
            {
                if(value >= 0)
                    ViewState["Left"] = value;
                else
                    ViewState["Left"] = 0;
            }
		}

	    /// <summary>
        /// Set or get the position of the top of the window.
	    /// </summary>
        /// <value>If not set, it defaults to 50.  The minimum value is 0.</value>
		[Category("Appearance"), DefaultValue(50), Bindable(true),
         Description("Set or get the position of the top of the window")]
		public int Top
		{
			get
			{
	            Object oTop = ViewState["Top"];
                return (oTop == null) ? 50 : (int)oTop;
            }
			set
            {
                if(value >= 0)
                    ViewState["Top"] = value;
                else
                    ViewState["Top"] = 0;
            }
		}

	    /// <summary>
        /// Set or get the height of the window
	    /// </summary>
        /// <value>If not set, it defaults to 500.  The minimum value is 100.</value>
		[Category("Appearance"), DefaultValue(500), Bindable(true),
         Description("Set or get the height of the window")]
		public int Height
		{
			get
			{
	            Object oHeight = ViewState["Height"];
                return (oHeight == null) ? 500 : (int)oHeight;
            }
			set
            {
                if(value >= 100)
                    ViewState["Height"] = value;
                else
                    ViewState["Height"] = 100;
            }
		}

	    /// <summary>
        /// Set or get the width of the window.
	    /// </summary>
        /// <value>If not set, it defaults to 700.  The minimum value is 100.</value>
		[Category("Appearance"), DefaultValue(700), Bindable(true),
         Description("Set or get the width of the window")]
		public int Width
		{
			get
			{
	            Object oWidth = ViewState["Width"];
                return (oWidth == null) ? 700 : (int)oWidth;
            }
			set
            {
                if(value >= 100)
                    ViewState["Width"] = value;
                else
                    ViewState["Width"] = 100;
            }
		}

	    /// <summary>
        /// Set or get the window options to enable
	    /// </summary>
        /// <value>List the options to enable.  If not set, system defaults
        /// are used (see <see cref="WindowOptions"/>).  Only the options listed
        /// are enabled if set to something other than <b>Defaults</b>.</value>
		[Category("Appearance"), DefaultValue(WindowOptions.Defaults),
         Bindable(true), Description("Set window options to enable")]
		public WindowOptions EnableOptions
		{
			get
			{
	            Object oOpts = ViewState["EnableOpts"];
                return (oOpts == null) ? WindowOptions.Defaults : (WindowOptions)oOpts;
            }
			set { ViewState["EnableOpts"] = value; }
		}

        /// <summary>
        /// Same as the <see cref="EnableOptions"/> property but it takes
        /// a string that will be parsed into the options to enable.
        /// </summary>
        /// <value>If not set, it returns <see cref="WindowOptions">Defaults</see>
        /// as a string value.  Only the options listed are enabled if set to
        /// something other than <b>Defaults</b>.</value>
        [Category("Appearance"), DefaultValue("Defaults"), Bindable(true),
         Description("Specify comma separated list of window options to enable")]
        public string EnableOptionsString
        {
            get
            {
                Object oOpts = ViewState["EnableOpts"];
                return (oOpts == null) ? WindowOptions.Defaults.ToString() :
                    ((WindowOptions)oOpts).ToString();
            }
            set
            {
                ViewState["EnableOpts"] = (WindowOptions)Enum.Parse(
                        typeof(WindowOptions), value, true);
            }
        }

        /// <summary>
        /// Sets or gets whether or not the window adds to history or
        /// replaces the history entry for the current window when loaded
        /// into the same window (i.e. <see cref="Target"/> is set to
        /// <b>Self</b>).
        /// </summary>
        /// <value>When set to true, the URL replaces the current document in
        /// the history list.  When set to false (the default), the URL
        /// creates a new entry in the history list.</value>
		[Category("Navigation"), DefaultValue(false), Bindable(true),
         Description("Set or get the history list replacement behaviour")]
		public bool ReplaceHistory
		{
			get
			{
	            Object oReplace = ViewState["Replace"];
                return (oReplace == null) ? false : (bool)oReplace;
            }
			set { ViewState["Replace"] = value; }
		}

        /// <summary>
        /// Sets or gets the name of the window used when <see cref="Target"/>
        /// is set to <b>Same</b>.
        /// </summary>
        /// <value>If not set, it defaults to null.  This property can be
        /// used to create uniquely named windows that are opened independently
        /// of any others when you have multiple <b>WindowOpener</b> controls
        /// on a page that make use of the <b>WindowTarget.Same</b> setting or
        /// if you have a common window used on multiple pages such as a
        /// help popup window.</value>
		[Category("Navigation"), Bindable(true),
         Description("Set or get the name of the window")]
		public string Name
		{
			get
			{
	            Object oName = ViewState["Name"];
                return (oName == null) ? null : (string)oName;
            }
			set { ViewState["Name"] = value; }
		}

        //=====================================================================
        // Methods, etc

	    /// <summary>
        /// This method is overridden to generate the button or hyperlink
        /// control that will be rendered on the client.
	    /// </summary>
        protected override void CreateChildControls()
        {
            Style sStyle = new Style();
            sStyle.MergeWith(this.ControlStyle);

            // Create the window.open call
            string strScript = "javascript:";

            strScript += CtrlUtils.OpenWindowJS(this.URL, this.Target,
                this.Left, this.Top, this.Height, this.Width,
                this.EnableOptions, this.ReplaceHistory, this.Name);

            strScript += " return false;";

            // Render as a button or as a hyperlink?
            if(this.Button == true)
            {
                Button btnLink = new Button();
                btnLink.CausesValidation = false;
                btnLink.ApplyStyle(sStyle);
                btnLink.Attributes.Add("onclick", strScript);
                btnLink.Text = this.Text;
                btnLink.ToolTip = this.ToolTip;
                btnLink.ID = "btnOpen";
                this.Controls.Add(btnLink);
            }
            else
            {
                HyperLink hlLink = new HyperLink();
                hlLink.ApplyStyle(sStyle);
                hlLink.NavigateUrl = "javascript: return false;";
                hlLink.Attributes.Add("onclick", strScript);
                hlLink.Text = this.Text;
                hlLink.ToolTip = this.ToolTip;
                hlLink.ID = "hlOpen";
                this.Controls.Add(hlLink);
            }
        }

        /// <summary>
        /// This is called by the <see cref="EWSoftware.Web.Design.WindowOpenerDesigner"/>
        /// control designer class when rendering at design time.
        /// </summary>
        internal void RenderAtDesignTime()
        {
            this.Controls.Clear();
            CreateChildControls();
        }
    }
}
