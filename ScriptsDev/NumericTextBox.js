// #pragma NoCompStart
//=============================================================================
// File    : NumericTextBox.js
// Author  : Eric Woodruff  (Eric@EWoodruff.us)
// Updated : 04/09/2003
// #pragma NoCompEnd
//
// This contains code for the NumericTextBox control that formats a numeric
// value with the correct number of decimal places.  All non-numeric
// characters and leading zeros are removed.

function NTB_FormatNumeric(ctl)
{
    var nIdx, nPos, strDecChar = ".", nDecPlaces = 0, bInsDec = false;
    var vals, strNum, strSign, reStrip, strGroupChar = ",";

    // Extract the options from the validators if there are any
    if(typeof(ctl.Validators) != "undefined")
    {
        vals = ctl.Validators;

        for(nIdx = 0; nIdx < vals.length; nIdx++)
        {
            if(typeof(vals[nIdx].decimalchar) != "undefined")
                strDecChar = vals[nIdx].decimalchar;

            if(typeof(vals[nIdx].decplaces) != "undefined")
                nDecPlaces = parseInt(vals[nIdx].decplaces, 10);

			// For currency, "digits" is used instead of "decplaces"
            if(typeof(vals[nIdx].digits) != "undefined")
                nDecPlaces = parseInt(vals[nIdx].digits, 10);

            if(typeof(vals[nIdx].groupchar) != "undefined")
                strGroupChar = vals[nIdx].groupchar;

            if(typeof(vals[nIdx].insdec) != "undefined")
                if(vals[nIdx].insdec.toLowerCase() == "true")
                    bInsDec = true;
                else
                    bInsDec = false;
        }

        // Group character is only specified for currency validators.
        // If not found and it matches the decimal character,
        // set it to something else so that it won't remove the
        // decimal character.
        if(strGroupChar == strDecChar)
            strGroupChar = (strDecChar == ",") ? "." : "X";
    }

    // Remove all non-numeric characters
    reStrip = new RegExp("[^0-9\\-\\+\\" + strGroupChar + "\\" +
        strDecChar + "]", "g");
    strNum = ctl.value.replace(reStrip, "");

    // Don't bother for non-numbers, let the validator catch it.
    if(isNaN(parseFloat(strNum)))
        return;

    // Remove sign to help with formatting below
    strSign = strNum.charAt(0);
    if(strSign == "-" || strSign == "+")
        strNum = strNum.substr(1);
    else
        strSign = "";

    // Strip leading zeros
    while(strNum.charAt(0) == "0" && strNum.charAt(1) != "." &&
      strNum.charAt(1) != "")
        strNum = strNum.substr(1);

    nPos = strNum.indexOf(strDecChar);

    // Strip decimals?
    if(nDecPlaces == 0)
        strNum = Math.round(parseFloat(strNum)).toString();
    else
        if(nPos == -1)
        {
            // No decimals at all, so add them
            if(bInsDec == false)
            {
                strNum += strDecChar;
                while(nDecPlaces > 0)
                {
                    strNum += "0";
                    nDecPlaces--;
                }
            }
            else
                if(strNum.length > nDecPlaces)
                    strNum = strNum.substr(0, strNum.length -
                        nDecPlaces) + strDecChar +
                        strNum.substr(strNum.length - nDecPlaces);
                else
                {
                    nDecPlaces -= strNum.length;
                    while(nDecPlaces > 0)
                    {
                        strNum = "0" + strNum;
                        nDecPlaces--;
                    }

                    strNum = "0." + strNum;
                }
        }
        else
        {
            // Add or remove decimal places
            if(strNum.substr(nPos + 1).length > nDecPlaces)
                strNum = strNum.substr(0, nPos + nDecPlaces + 1);
            else
            {
                nDecPlaces -= strNum.substr(nPos + 1).length;
                while(nDecPlaces > 0)
                {
                    strNum += "0";
                    nDecPlaces--;
                }
            }
        }

    // Add a leading zero if needed
    if(strNum.charAt(0) == strDecChar)
        strNum = "0" + strNum;

    // Add back sign if there was one
    strNum = strSign + strNum;

    if(strNum != ctl.value)
    {
        ctl.value = strNum;

        // If there is an on OnChange event, call it.  The ASP.NET
        // validators hook themselves into this and the unformatted
        // value may have displayed an error.  This will hide it.
        if(ctl.onchange != null)
            ctl.onchange();
    }
}
