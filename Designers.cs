//=============================================================================
// System  : ASP.NET Web Control Library
// File    : Designers.cs
// Author  : Eric Woodruff  (Eric@EWoodruff.us)
// Updated : Fri 02/20/04
// Note    : Copyright 2002-2004, Eric Woodruff, All rights reserved
// Compiler: Microsoft Visual C#
//
// This file contains designers for the controls in the EWSoftware.Web.Controls
// namespace that need them.
//
// Version     Date     Who  Comments
// ============================================================================
// 1.0.0    04/27/2003  EFW  Created the code
//=============================================================================

using System;
using System.IO;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.ComponentModel.Design;

using EWSoftware.Web.Controls;

namespace EWSoftware.Web.Design
{
	/// <summary>
	/// This provides basic design time support for some of the controls
    /// in the <see cref="EWSoftware.Web.Controls"/> namespace.
	/// </summary>
    public class EWSControlDesigner : ControlDesigner
	{
		/// <summary>
		/// This returns the design time HTML.  It simply renders the
        /// control and returns the resulting HTML.
		/// </summary>
		/// <returns>The design time HTML</returns>
        public override string GetDesignTimeHtml()
		{
			try
			{
                Control ctl = (Control)this.Component;

    			StringWriter tw = new StringWriter();
	    		HtmlTextWriter writer = new HtmlTextWriter(tw);

		    	ctl.RenderControl(writer);
			    return tw.ToString();
			}
			catch(Exception e)
			{
				return GetErrorDesignTimeHtml(e);
			}
		}

		/// <summary>
		/// Render a place holder describing an error that occurred while
        /// creating the design time HTML.
		/// </summary>
		/// <param name="e">The exception that occurred</param>
		/// <returns>A string describing the error wrapped in
        /// placeholder HTML</returns>
        protected override string GetErrorDesignTimeHtml(Exception e)
		{
			return CreatePlaceHolderDesignTimeHtml(String.Format(
                "There was an error and the control cannot be " +
                "displayed.<br>Exception: {0}<br>{1}", e.Message,
                e.StackTrace));
		}
	}

	/// <summary>
	/// This is a control designer for the <see cref="AddToFavorites"/> control
	/// </summary>
    public class AddToFavoritesDesigner : EWSControlDesigner
	{
		/// <summary>
		/// This renders the design time HTML for the
        /// <see cref="AddToFavorites"/> control.
		/// </summary>
		/// <returns>The design time HTML</returns>
        public override string GetDesignTimeHtml()
		{
            AddToFavorites atf = (AddToFavorites)this.Component;
            bool bVisible = atf.Visible;

			try
			{
    			StringWriter tw = new StringWriter();
	    		HtmlTextWriter writer = new HtmlTextWriter(tw);

                if(!bVisible)
                    atf.Visible = true;

                atf.RenderAtDesignTime();
		    	atf.RenderControl(writer);
			    return tw.ToString();
			}
			catch(Exception e)
			{
				return GetErrorDesignTimeHtml(e);
			}
            finally
            {
                atf.Visible = bVisible;
            }
		}
	}

	/// <summary>
	/// This is a control designer for the <see cref="FileLink"/> control
	/// </summary>
    public class FileLinkDesigner : EWSControlDesigner
	{
		/// <summary>
		/// This renders the design time HTML for the
        /// <see cref="FileLink"/> control.
		/// </summary>
		/// <returns>The design time HTML</returns>
        public override string GetDesignTimeHtml()
		{
            FileLink fl = (FileLink)this.Component;
            bool bVisible = fl.Visible;

			try
			{
    			StringWriter tw = new StringWriter();
	    		HtmlTextWriter writer = new HtmlTextWriter(tw);

                if(!bVisible)
                    fl.Visible = true;

                fl.RenderAtDesignTime();
		    	fl.RenderControl(writer);
			    return tw.ToString();
			}
			catch(Exception e)
			{
				return GetErrorDesignTimeHtml(e);
			}
            finally
            {
                fl.Visible = bVisible;
            }
		}
	}

	/// <summary>
	/// This is a control designer for the <see cref="WindowOpener"/> control
	/// </summary>
    public class WindowOpenerDesigner : EWSControlDesigner
	{
		/// <summary>
		/// This renders the design time HTML for the
        /// <see cref="WindowOpener"/> control.
		/// </summary>
		/// <returns>The design time HTML</returns>
        public override string GetDesignTimeHtml()
		{
            WindowOpener wo = (WindowOpener)this.Component;
            bool bVisible = wo.Visible;

			try
			{
    			StringWriter tw = new StringWriter();
	    		HtmlTextWriter writer = new HtmlTextWriter(tw);

                if(!bVisible)
                    wo.Visible = true;

                wo.RenderAtDesignTime();
		    	wo.RenderControl(writer);
			    return tw.ToString();
			}
			catch(Exception e)
			{
				return GetErrorDesignTimeHtml(e);
			}
            finally
            {
                wo.Visible = bVisible;
            }
		}
	}

	/// <summary>
	/// This is a control designer for the <see cref="DateTextBox"/> control
	/// </summary>
    public class DateTextBoxDesigner : EWSControlDesigner
	{
		/// <summary>
		/// This renders the design time HTML for the
        /// <see cref="DateTextBox"/> control.  An empty placeholder is
        /// displayed in place of the popup calendar image at design time.
		/// </summary>
		/// <returns>The design time HTML</returns>
        public override string GetDesignTimeHtml()
		{
            DateTextBox dtb = (DateTextBox)this.Component;
            bool bVisible = dtb.Visible;

			try
			{
    			StringWriter tw = new StringWriter();
	    		HtmlTextWriter writer = new HtmlTextWriter(tw);

                if(!bVisible)
                    dtb.Visible = true;

                dtb.RenderAtDesignTime();
		    	dtb.RenderControl(writer);
			    return tw.ToString();
			}
			catch(Exception e)
			{
				return GetErrorDesignTimeHtml(e);
			}
            finally
            {
                dtb.Visible = bVisible;
            }
		}
	}
}
