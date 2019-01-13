// #pragma NoCompStart
//=============================================================================
// File    : SelectIncrSearch.js
// Author  : Eric Woodruff  (Eric@EWoodruff.us)
// Updated : 10/27/2003
// #pragma NoCompEnd
//
// This contains code to add incremental search capabilities to the SELECT
// controls (dropdowns and listboxes).  It only works in Internet Explorer.

// #pragma NoCompStart
var SIS_strSearchText = "", SIS_bChanged = false;
// #pragma NoCompEnd

// Take action based on the key pressed
function SIS_OnKeyDown(ctlList, nKeyCode)
{
    var nIdx, nSelIdx, bRetVal = true;

    switch(nKeyCode)
    {
        case 8:     // Remove last character and search for match
            SIS_strSearchText =
                SIS_strSearchText.substr(0, SIS_strSearchText.length - 1);
            SIS_OnKeyPress(ctlList, 0);

            // Return false to prevent browser from navigating back one page
            bRetVal = false;
            break;

        case 33:    // Go backwards or forwards by a page.
        case 34:
        case 35:    // Go to start or end of list.
        case 36:
        case 38:    // Go backwards or forwards one entry.
        case 40:
            SIS_strSearchText = "";  // Clear text for new search
            break;

        case 27:    // Cancel search and reset to default entry
        case 46:
	    	SIS_strSearchText = "";

    		for(nIdx = nSelIdx = 0; nIdx < ctlList.options.length; nIdx++)
				if(ctlList.options[nIdx].defaultSelected)
                {
				    nSelIdx = nIdx;
                    break;
    			}

            ctlList.selectedIndex = nSelIdx;
            break;

        default:
            break;
    }

    return bRetVal;
}

// Search for an entry based on prior text plus the new character
function SIS_OnKeyPress(ctlList, nKeyCode)
{
    var chChar, re;

    // Ignore character if called from SIS_OnKeyDown()
    if(nKeyCode == 0)
        chChar = "";
    else
	    chChar = String.fromCharCode(nKeyCode);

	re = new RegExp("^" + SIS_strSearchText + chChar, "i");

	for(var nIdx = 0; nIdx < ctlList.options.length; nIdx++)
		if(re.test(ctlList.options[nIdx].text))
		{
            SIS_bChanged = true;
			ctlList.selectedIndex = nIdx;
			SIS_strSearchText += chChar;
			break;
		}

    return false;
}

// This is called on loss of focus to see if an auto-postback should
// occur.  When incremental search is enabled, the normal auto-postback
// method is not useable as it fires for every change in selected item.
// When incremental search is enabled, auto-postback only occurs when the
// control loses focus or the user presses ENTER.
function SIS_OnBlur(ctlList)
{
    if(SIS_bChanged == true)
        window.setTimeout(ctlList.AutoPostBack, 0, 'JavaScript');
}

// This sets the changed flag so that the OnBlur event knows when
// it should do an auto-postback.
function SIS_OnChange()
{
    SIS_bChanged = true;
}
