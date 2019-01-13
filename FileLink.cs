//=============================================================================
// System  : ASP.NET Web Control Library
// File    : FileLink.cs
// Author  : Eric Woodruff  (Eric@EWoodruff.us)
// Updated : Fri 02/20/04
// Note    : Copyright 2002-2004, Eric Woodruff, All rights reserved
// Compiler: Microsoft Visual C#
//
// This file contains a derived Control class that can be used to render a
// file hyperlink followed by an image to show its type (i.e. PDF) and the size
// of the file pointed to by the link.
//
// Version     Date     Who  Comments
// ============================================================================
// 1.0.0    04/26/2004  EFW  Created the code
//=============================================================================

using System;
using System.IO;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using EWSoftware.Web.Design;

namespace EWSoftware.Web.Controls
{
    /// <summary>
    /// This control can be used to render a file hyperlink followed by an
    /// image to show its type (i.e. PDF) and the size of the file pointed to
    /// by the link.
    /// </summary>
	[DefaultProperty("Text"),
        ToolboxData("<{0}:FileLink runat='server' File='PDFFiles/Test.pdf' " +
        "Image='Images/PDF.gif' ShowFileSize='True' />"),
        Designer(typeof(FileLinkDesigner))]
	public class FileLink : System.Web.UI.Control, INamingContainer
	{
        //=====================================================================
        // Private class members
        private Style sLink;    // Hyperlink style

        //=====================================================================
        // Private designer methods.  These are used because the default value
        // for some of the properties is the URL property.

        // The designer uses this to determine whether or not to serialize
        // changes to the Text property.
        private bool ShouldSerializeText()
        {
            return this.Text != this.URL;
        }

        // The designer uses this to determine whether or not to serialize
        // changes to the Tooltip property.
        private bool ShouldSerializeToolTip()
        {
            return this.ToolTip != this.URL;
        }

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
        /// This property is used to set the text displayed for the hyperlink.
	    /// </summary>
        /// <value>If not set, it defaults to the URL.</value>
		[Category("Appearance"), Bindable(true),
         Description("The text to display in the hyperlink")]
		public string Text
		{
			get
			{
	            Object oText = ViewState["Text"];
                return (oText == null) ? this.URL : (string)oText;
            }
			set { ViewState["Text"] = value; }
		}

	    /// <summary>
        /// This property is used to set the tooltip displayed for the
        /// hyperlink.
	    /// </summary>
        /// <value>If not set, it defaults to the URL.</value>
		[Category("Appearance"), Bindable(true),
            Description("The tooltip to display on the hyperlink")]
		public string ToolTip
		{
			get
			{
	            Object oTip = ViewState["Tip"];
                return (oTip == null) ? this.URL : (string)oTip;
            }
			set { ViewState["Tip"] = value; }
		}

	    /// <summary>
        /// This property is used to set the URL for the file.
	    /// </summary>
		[Category("Appearance"), Bindable(true),
         Description("The URL to use for the hyperlink")]
		public string URL
		{
			get { return (string)ViewState["URL"]; }
			set { ViewState["URL"] = value; }
		}

	    /// <summary>
        /// This property is used to set the image for the file.
	    /// </summary>
		[Category("Appearance"), Bindable(true),
         Description("The image to use for the link")]
		public string FileImage
		{
			get { return (string)ViewState["FileImage"]; }
			set { ViewState["FileImage"] = value; }
		}

	    /// <summary>
        /// This property is used to set the width of the image
        /// following the link.
	    /// </summary>
		[Category("Appearance"), Bindable(true), DefaultValue(16),
         Description("The width of the image following the link")]
		public int ImageWidth
		{
			get
            {
                object oWidth = ViewState["ImageWidth"];
                return (oWidth == null) ? 16 : (int)oWidth;
            }
			set { ViewState["ImageWidth"] = value; }
		}

	    /// <summary>
        /// This property is used to set the height of the image
        /// following the link.
	    /// </summary>
		[Category("Appearance"), Bindable(true), DefaultValue(16),
         Description("The height of the image following the link")]
		public int ImageHeight
		{
			get
            {
                object oHeight = ViewState["ImageHeight"];
                return (oHeight == null) ? 16 : (int)oHeight;
            }
			set { ViewState["ImageHeight"] = value; }
		}

	    /// <summary>
        /// This property is used to set whether or not the file size is
        /// shown after the link.
	    /// </summary>
        /// <value>If not set, it defaults to false (not shown).</value>
		[Category("Appearance"), Bindable(true), DefaultValue(false),
         Description("Show file size after the link")]
		public bool ShowFileSize
		{
			get
            {
                object oShowSize = ViewState["ShowFileSize"];
                return (oShowSize == null) ? false : (bool)oShowSize;
            }
			set { ViewState["ShowFileSize"] = value; }
		}

	    /// <summary>
        /// This property is used to set the hyperlink style.
	    /// </summary>
        /// <value>If not set, the hyperlink style for the current page
        /// is used.</value>
		[Category("Appearance"), Bindable(true),
         Description("The style to use for the hyperlink")]
		public Style LinkStyle
		{
			get
            {
                if(sLink == null)
                    sLink = new Style();

                return sLink;
			}
            set { sLink = value; }
		}

        //=====================================================================
        // Methods, etc

	    /// <summary>
        /// This method is overridden to generate the hyperlink, the image,
        /// and the file size label.
	    /// </summary>
        protected override void CreateChildControls()
        {
            double dSize;
            Style sStyle = new Style();

            // If no URL is specified, render nothing
            if(this.URL == null || this.URL.Length == 0)
                return;

    		sStyle.MergeWith(this.LinkStyle);

            HyperLink hlLink = new HyperLink();
            hlLink.ApplyStyle(sStyle);
            hlLink.NavigateUrl = this.URL;
            hlLink.Text = this.Text;
            hlLink.ToolTip = this.ToolTip;
            hlLink.ID = "hlFileLink";
            this.Controls.Add(hlLink);

            if((this.FileImage == null || this.FileImage.Length == 0) &&
              this.ShowFileSize == false)
                return;

            StringBuilder strLink = new StringBuilder(256);

            // Add file image if specified
            if(this.FileImage != null && this.FileImage.Length > 0)
                strLink.AppendFormat("&nbsp;<img src='{0}' width='{1}' " +
                    "height='{2}' border='0' alt='{3}' align='middle'>",
                    this.FileImage, this.ImageWidth, this.ImageHeight,
                    this.FileImage);

            // Add file size label if specified.  It's only retrieved once
            // on first use and is stored in view state after that.
            if(this.ShowFileSize == true)
            {
                string strSizeInfo;

                strSizeInfo = (string)this.ViewState["FileSize"];

                if(strSizeInfo == null)
                {
                    try
                    {
                        FileInfo fi = new FileInfo(this.Page.Server.MapPath(this.URL));
                        if(fi.Exists == true)
                        {
                            if(fi.Length > 1023)
                            {
                                dSize = (double)fi.Length / 1024.0;

                                if(dSize < 1024.0)
                                    strSizeInfo = String.Format("{0:F1}kb", dSize);
                                else
                                {
                                    dSize /= 1024.0;
                                    strSizeInfo = String.Format("{0:F1}mb", dSize);
                                }
                            }
                            else
                                strSizeInfo = String.Format("{0} bytes", fi.Length);
                        }
                        else
                            strSizeInfo = "not found";
                    }
                    catch
                    {
                        // Ignore errors
                        strSizeInfo = "??kb";
                    }

                    this.ViewState["FileSize"] = strSizeInfo;
                }

                strLink.Append("&nbsp;<small><i>(");
                strLink.Append(strSizeInfo);
                strLink.Append(")</i></small>");
            }

            this.Controls.Add(new LiteralControl(strLink.ToString()));
        }

        /// <summary>
        /// This is called by the <see cref="EWSoftware.Web.Design.FileLinkDesigner"/>
        /// control designer class when rendering at design time.
        /// </summary>
        internal void RenderAtDesignTime()
        {
            this.Controls.Clear();
            CreateChildControls();
        }
    }
}
