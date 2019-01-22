//=============================================================================
// File    : DateTextBox.js
// Author  : Eric Woodruff  (Eric@EWoodruff.us)
// Updated : 01/04/2008
//
// This contains code for the DateTextBox control that formats a date
// value with the correct separators and number of digits.  It also
// contains the code to invoke a popup calendar dialog box and special
// key handling.

// TODO: Take into account date format, separator, etc?
// Format a date value for the date text box
function DTB_FormatDate(ctl)
{
    var strDate, strParts, nIdx, nYear, dtToday = new Date();

    // Strip out invalid characters
    strDate = ctl.value.toLowerCase().replace(/[^0-9/]/g, "");

    strParts = strDate.split("/");

    if(strParts.length == 1)
    {
        // Try to figure out where to insert the separator
        switch(strDate.length)
        {
            case 1:     // Month only (one digit)
                strDate = "0" + strDate + "/01/" + dtToday.getFullYear();
                break;

            case 2:     // Month only (2 digits)
                strDate += "/01/" + dtToday.getFullYear();
                break;

            case 3:     // MDD
                strDate = "0" + strDate.substr(0,1) + "/" +
                    strDate.substr(1, 2) + "/" + dtToday.getFullYear();
                break;

            case 4:     // MMDD
                strDate = strDate.substr(0, 2) + "/" + strDate.substr(2) +
                    "/" + dtToday.getFullYear();
                break;

            case 6:     // MMDDYY so add century
                nYear = parseInt(strDate.substr(4), 10);
                if(nYear < 1000)
                    if(nYear < 30 || nYear > 99)
                        nYear += 2000;
                    else
                        nYear += 1900;

                strDate = strDate.substr(0, 2) + "/" + strDate.substr(2, 2) +
                    "/" + nYear;
                break;

            case 8:     // MMDDYYYY
                strDate = strDate.substr(0, 2) + "/" + strDate.substr(2, 2) +
                    "/" + strDate.substr(4);
                break;

            default:
                // Give up and leave it as-is, the validator will catch it
                strDate = ctl.value;
                break;
        }
    }
    else
    {
        // Format the parts with leading zeros.  Any parts after
        // the third are discarded.  We make no assumption about the
        // format other than the first two parts are month and day.
        if(strParts[0].length < 2)
            strParts[0] = "0" + strParts[0];

        // If there are only two parts, assume it's a month unless it
        // is more than two digits long
        if(strParts.length == 2)
        {
            if(strParts[1].length > 2)
            {
                nYear = parseInt(strParts[1], 10);
                strParts[1] = "01";
            }
            else
            {
                nYear = dtToday.getFullYear();

                if(strParts[1].length < 2)
                    strParts[1] = "0" + strParts[1];
            }
        }
        else
        {
            if(strParts[1].length < 2)
                strParts[1] = "0" + strParts[1];

            nYear = parseInt(strParts[2], 10);
        }

        if(nYear < 1000)
            if(nYear < 30 || nYear > 99)
                nYear += 2000;
            else
                nYear += 1900;

        strDate = strParts[0] + "/" + strParts[1] + "/" + nYear.toString();
    }

    if(strDate != ctl.value)
    {
        ctl.value = strDate;

        // If there is an on OnChange event, call it.  The ASP.NET
        // validators hook themselves into this and the unformatted
        // value may have displayed an error.  This will hide it.
        if(ctl.onchange != null)
            ctl.onchange();
    }
}

// Check for special keys on date text box fields
function DTB_DateKeys(ctl, nKeyCode)
{
    var strDate, dtDate, nDay, nMonth, nYear;

	// Enter key?  If so, format and exit.  This is done because Enter
    // usually submits the page and that fires before the OnChange event
    // and we get a bogus validation error.
	if(nKeyCode == 13)
	{
        DTB_FormatDate(ctl);
		return true;
	}

    // Popup calendar?
    if(nKeyCode == 80)
    {
        DTB_PopupCal(ctl.id);
        return false;
    }

    // Only used for PgUp/Down, Arrow Up/Down, and T
    if(nKeyCode != 33 && nKeyCode != 34 && nKeyCode != 38 &&
      nKeyCode != 40 && nKeyCode != 84)
        return true;

    if(nKeyCode != 84 && ctl.value != "")
    {
        DTB_FormatDate(ctl);
        dtDate = new Date(ctl.value);

        if(isNaN(dtDate))
            return;
    }
    else
        dtDate = new Date();

    switch(nKeyCode)
    {
        case 33:    // Page up - Add a month
            dtDate.setMonth(dtDate.getMonth() + 1);
            break;

        case 34:    // Page down - Subtract a month
            dtDate.setMonth(dtDate.getMonth() - 1);
            break;

        case 38:    // Up arrow - Add a day
            dtDate.setDate(dtDate.getDate() + 1);
            break;

        case 40:    // Down arrow - Subtract a day
            dtDate.setDate(dtDate.getDate() - 1);
            break;

        default:    // Must have been "T" for Today
            break;
    }

    nDay = dtDate.getDate();
    nMonth = dtDate.getMonth() + 1;
    nYear = dtDate.getFullYear();

    if(nMonth < 10)
        strDate = "0" + nMonth.toString() + "/";
    else
        strDate = nMonth.toString() + "/";

    if(nDay < 10)
        strDate = strDate + "0" + nDay.toString() + "/" + nYear.toString();
    else
        strDate = strDate + nDay.toString() + "/" + nYear.toString();

    ctl.value = strDate;
    ctl.select();

    if(ctl.onchange != null)
        ctl.onchange();

    return false;
}

// Show a popup calendar for the specified control.  The text in the
// control is passed as the starting date.
function DTB_PopupCal(strDateCtlID)
{
    var ctl, dtDate, strRetVal;

    ctl = document.getElementById(strDateCtlID);

    // Ignore it if the control is disabled
    if(ctl.disabled)
        return;

    strRetVal = window.showModalDialog(
        '<%=WebResource("EWSoftware.Web.Controls.HTML.Calendar.html")%>',
        ctl.value, "dialogHeight: 200px; dialogWidth: 260px; " +
        "center: yes; help: no; resizable: yes; status: no;");

    // If a date was selected, assign it to the control
    if(strRetVal != null)
        ctl.value = strRetVal;

    ctl.focus();
    ctl.select();
}
