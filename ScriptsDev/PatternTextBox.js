// #pragma NoCompStart
//=============================================================================
// File    : PatternTextBox.js
// Author  : Eric Woodruff  (Eric@EWoodruff.us)
// Updated : Fri 12/12/03
// #pragma NoCompEnd
//
// Client-side formatting code for the PatternTextBox control

// Check for Enter key on any type
function PTB_Keys(ctl, nKeyCode, strType)
{
	// Enter key?  If so, format and exit.  This is done because Enter
    // usually submits the page and that fires before the OnChange event
    // and we get a bogus validation error.
	if(nKeyCode == 13)
        switch(strType)
        {
            case "Phone":
                PTB_FormatPhone(ctl);
                break;

            case "Zip":
                PTB_FormatZip(ctl);
                break;

            case "SSN":
                PTB_FormatSSN(ctl);
                break;

            default:
                break;
        }

    return true;
}

// Format a phone number for the various phone patterns
function PTB_FormatPhone(ctl)
{
    var nPos, strPhone, strExt = "", bParen = true;

    // Extract and save any extension text.  It should appear
    // after an 'x' or a space after the phone number.
    strPhone = ctl.value;

    for(nPos = 6; nPos < strPhone.length; nPos++)
        if(strPhone.charAt(nPos) == " " || strPhone.charAt(nPos) == "x" ||
          strPhone.charAt(nPos) == "X")
            break;

    if(nPos < strPhone.length)
    {
        strExt = strPhone.substr(nPos);

        // If "x" is used to indicate the extension, make it lower case
        // if necessary to match the pattern.
        if(strExt.substr(0, 1) == "X")
            strExt = "x" + strExt.substr(1);

        strPhone = strPhone.substr(0, nPos);
    }

    // Trim off any leading whitespace
    strPhone = strPhone.replace(/^\s*/g, "");

    // Note whether or not area code is in parentheses.  We'll assume
    // it is by default.
    if(strPhone.substr(0, 1) != "(" && strPhone.substr(3, 1) == "-")
        bParen = false;

    // Strip all non-numeric characters
    strPhone = strPhone.replace(/[^0-9]/g, "");

    // Insert formatting if the length is okay.  Let the validator
    // handle it if not.
    if(strPhone.length == 7 || strPhone.length == 10)
    {
        if(strPhone.length == 7)
            strPhone = strPhone.substr(0, 3) + "-" + strPhone.substr(3);
        else
        {
            // Keep the area code in parentheses?
            if(bParen == true)
                strPhone = "(" + strPhone.substr(0, 3) + ") " +
                    strPhone.substr(3, 3) + "-" + strPhone.substr(6);
            else
                strPhone = strPhone.substr(0, 3) + "-" +
                    strPhone.substr(3, 3) + "-" + strPhone.substr(6);
        }

        strPhone = strPhone + strExt;

        if(strPhone != ctl.value)
        {
            ctl.value = strPhone;

            // If there is an on OnChange event, call it.  The ASP.NET
            // validators hook themselves into this and the unformatted
            // value may have displayed an error.  This will hide it.
            if(ctl.onchange != null)
                ctl.onchange();
        }
    }
}

// Format a zip code for the Zip and Zip4 patterns
function PTB_FormatZip(ctl)
{
    var strZip, strPlus4;

    // Strip out invalid characters
    strZip = ctl.value.replace(/[^0-9\-]/g, "");

    nPos = strZip.indexOf("-");

    // Insert dash?  If it's there already or the length is
    // too short or long, let the validator handle it.
    if(nPos == -1 && strZip.length > 5 && strZip.length < 10)
    {
        // Left pad the +4 part with leading zeros if needed
        strPlus4 = strZip.substr(5);
        while(strPlus4.length < 4)
            strPlus4 = "0" + strPlus4;

        ctl.value = strZip.substr(0, 5) + "-" + strPlus4;

        // If there is an on OnChange event, call it.  The ASP.NET
        // validators hook themselves into this and the unformatted
        // value may have displayed an error.  This will hide it.
        if(ctl.onchange != null)
            ctl.onchange();
    }
}

// Format a social security number for the SSN pattern
function PTB_FormatSSN(ctl)
{
    var strSSN;

    // Strip out invalid characters
    strSSN = ctl.value.replace(/[^0-9]/g, "");

    // Insert dashes if the length is okay
    if(strSSN.length == 9)
        strSSN = strSSN.substr(0, 3) + "-" + strSSN.substr(3, 2) + "-" +
            strSSN.substr(5);

    if(strSSN != ctl.value)
    {
        ctl.value = strSSN;

        // If there is an on OnChange event, call it.  The ASP.NET
        // validators hook themselves into this and the unformatted
        // value may have displayed an error.  This will hide it.
        if(ctl.onchange != null)
            ctl.onchange();
    }
}

// Format a time value for the Time pattern
function PTB_FormatTime(ctl)
{
    var strTime, strAMPM, strParts, nIdx;

    // Strip out invalid characters
    strTime = ctl.value.toLowerCase().replace(/[^0-9apm\\:]/g, "");

    // Save the am/pm designation if it's there and remove it
    // to help with formatting below.
    strAMPM = strTime.substr(strTime.length - 2);

    if(strAMPM == "am" || strAMPM == "pm")
        strTime = strTime.substr(0, strTime.length - 2);
    else
        if(strAMPM.charAt(1) == "a" || strAMPM.charAt(1) == "p")
        {
            strAMPM = strAMPM.charAt(1) + "m";
            strTime = strTime.substr(0, strTime.length - 1);
        }
        else
            strAMPM = "";

    strParts = strTime.split(":");

    if(strParts.length == 1)
    {
        // Try to figure out where to insert the separator
        switch(strTime.length)
        {
            case 1:
                strTime = "0" + strTime;
                // Fall through and append minutes

            case 2:
                strTime += ":00";
                break;

            case 3:
                strTime = "0" + strTime.charAt(0) + ":" + strTime.substr(1);
                break;

            case 4:
                strTime = strTime.substr(0, 2) + ":" + strTime.substr(2);
                break;

            default:
                // Give up and leave it as-is, the validator will catch it
                strTime = ctl.value;
                strAMPM = "";
                break;
        }
    }
    else
    {
        // Format the hours and minutes with leading zeros.
        // Any extra parts like seconds are disgarded.
        for(nIdx = 0; nIdx < 2; nIdx++)
            if(strParts[nIdx].length < 2)
                strParts[nIdx] = "0" + strParts[nIdx];

        strTime = strParts[0] + ":" + strParts[1];
    }

    // Add back am/pm if it was there
    strTime += strAMPM;

    if(strTime != ctl.value)
    {
        ctl.value = strTime;

        // If there is an on OnChange event, call it.  The ASP.NET
        // validators hook themselves into this and the unformatted
        // value may have displayed an error.  This will hide it.
        if(ctl.onchange != null)
            ctl.onchange();
    }
}

// Check for special keys on Time pattern fields
function PTB_TimeKeys(ctl, nKeyCode)
{
    var nHour, nMin, strAMPM, dtTime, strTime;

	// Enter key?  If so, format and exit.  This is done because Enter
    // usually submits the page and that fires before the OnChange event
    // and we get a bogus validation error.
	if(nKeyCode == 13)
	{
        PTB_FormatTime(ctl);
		return true;
	}

    // Only used for PgUp/Down, Arrow Up/Down and N
    if(nKeyCode != 33 && nKeyCode != 34 && nKeyCode != 38 &&
      nKeyCode != 40 && nKeyCode != 78)
        return true;

    PTB_FormatTime(ctl);

    dtTime = new Date();
    strTime = ctl.value;

    if(strTime != "")
    {
        nHour = parseInt(strTime, 10);
        nMin = parseInt(strTime.substr(3), 10);

        if(isNaN(nHour) || isNaN(nMin))
            return;

        strAMPM = strTime.substr(strTime.length - 2);

        if(strAMPM != "am" && strAMPM != "pm")
            if(strAMPM.charAt(1) == "a" || strAMPM.charAt(1) == "p")
                strAMPM = strAMPM.charAt(1) + "m";
            else
                strAMPM = "";

        if(strAMPM == "pm" && nHour < 12)
            nHour += 12;
        else
            if(strAMPM == "am" && nHour == 12)
                nHour -= 12;
    }
    else
    {
        nHour = dtTime.getHours();
        nMin = dtTime.getMinutes();
        if(nHour < 12)
            strAMPM = "am";
        else
            strAMPM = "pm";
    }

    switch(nKeyCode)
    {
        case 33:    // Page up - Add an hour
            dtTime.setMinutes(nMin);
            dtTime.setHours(nHour + 1);
            break;

        case 34:    // Page down - Subtract an hour
            dtTime.setMinutes(nMin);
            dtTime.setHours(nHour - 1);
            break;

        case 38:    // Up arrow - Add a minute
            dtTime.setHours(nHour);
            dtTime.setMinutes(nMin + 1);
            break;

        case 40:    // Down arrow - Subtract a minute
            dtTime.setHours(nHour);
            dtTime.setMinutes(nMin - 1);
            break;

        default:    // Must have been "N" for Now
            break;
    }

    nHour = dtTime.getHours();
    nMin = dtTime.getMinutes();

    // Set the new time value.  Keep the am/pm format
    // if it was used.
    if(strAMPM != "")
        if(nHour < 12)
        {
            if(nHour == 0)
                nHour = 12;

            strAMPM = "am";
        }
        else
        {
            if(nHour > 12)
                nHour -= 12;

            strAMPM = "pm";
        }

    if(nHour < 10)
        strTime = "0" + nHour.toString() + ":";
    else
        strTime = nHour.toString() + ":";

    if(nMin < 10)
        strTime = strTime + "0" + nMin.toString() + strAMPM;
    else
        strTime = strTime + nMin.toString() + strAMPM;

    ctl.value = strTime;
    ctl.select();

    if(ctl.onchange != null)
        ctl.onchange();

    return false;
}
