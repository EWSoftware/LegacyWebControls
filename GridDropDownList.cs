//=============================================================================
// System  : ASP.NET Web Control Library
// File    : GridDropDownList.cs
// Author  : Eric Woodruff  (Eric@EWoodruff.us)
// Updated : Fri 02/20/04
// Note    : Copyright 2002-2004, Eric Woodruff, All rights reserved
// Compiler: Microsoft Visual C#
//
// This file contains a derived EWSDropDownList class that bubbles the
// OnSelectedIndexChanged and OnDefaultSelected events up the control chain.
// This is useful when the control is embedded in a DataGrid where the events
// normally wouldn't be seen.  Override the DataGrid's ItemCommand event and
// look for an e.CommandName of "DropDownChanged" or "DefaultSelected".  You
// can then use the e.Item object to find and work with controls in the row
// including the dropdown list that raised the event.  For simple use,
// e.CommandArguments will contain the newly selected value.  AutoPostBack is
// set to True by default.
//
// Version     Date     Who  Comments
// ============================================================================
// 1.0.0    09/12/2002  EFW  Created the code
//=============================================================================

using System;
using System.ComponentModel;
using System.Security.Permissions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EWSoftware.Web.Controls
{
	/// <summary>
	/// This is a derived EWSDropDownList class that bubbles the
    /// OnSelectedIndexChanged and OnDefaultSelected event up the control
    /// chain.  Use it in DataGrid controls if you need to handle the events
    /// from the dropdown list.  AutoPostBack is set to True by default.
	/// </summary>
	[ToolboxData("<{0}:GridDropDownList runat=\"server\" />"),
     AspNetHostingPermission(SecurityAction.LinkDemand,
        Level=AspNetHostingPermissionLevel.Minimal),
     AspNetHostingPermission(SecurityAction.InheritanceDemand,
        Level=AspNetHostingPermissionLevel.Minimal)]
	public class GridDropDownList : EWSoftware.Web.Controls.EWSDropDownList
	{
        //=====================================================================
        // Events

        /// <summary>
        /// A custom command event.  This is used to bubble the
        /// SelectedIndexChanged and DefaultSelected events.
        /// </summary>
		[Category("Action"),
         Description("Fires when the control needs to bubble the SelectedIndexChanged and DefaultSelected events")]
        public event System.Web.UI.WebControls.CommandEventHandler Command;

        //=====================================================================
        // Methods, etc

        /// <summary>
        /// Constructor.  AutoPostBack is set to true by default.
        /// </summary>
        public GridDropDownList()
        {
            AutoPostBack = true;
        }

        /// <summary>
        /// This bubbles the event up
        /// </summary>
        /// <param name="e">The command event arguments</param>
        protected virtual void OnCommand(System.Web.UI.WebControls.CommandEventArgs e)
        {
            if(Command != null)
                Command(this, e);

            // The whole purpose of this Command event is for bubbling
            RaiseBubbleEvent(this, e);
        }

        /// <summary>
        /// Handle the event here and bubble it up.  The new value is
        /// passed as the command event argument.
        /// </summary>
        /// <param name="e">The event arguments</param>
        protected override void OnSelectedIndexChanged(System.EventArgs e)
        {
            base.OnSelectedIndexChanged(e);

            OnCommand(new
                System.Web.UI.WebControls.CommandEventArgs("DropDownChanged",
                    SelectedItem.Value));
        }

        /// <summary>
        /// Handle the event here and bubble it up.  The new value is
        /// passed as the command event argument.
        /// </summary>
        /// <param name="e">The event arguments</param>
        protected override void OnDefaultSelected(System.EventArgs e)
        {
            base.OnDefaultSelected(e);

            OnCommand(new
                System.Web.UI.WebControls.CommandEventArgs("DefaultSelected",
                    SelectedItem.Value));
        }
    }
}
