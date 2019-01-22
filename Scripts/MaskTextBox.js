//=============================================================================
// File    : MaskTextBox.js
// Author  : Eric Woodruff  (Eric@EWoodruff.us)
// Updated : Sun 12/14/03 15:55:39
//
// Client-side formatting code for the MaskTextBox control

// See if entered character matches mask and append any literals that
// occur after it.
function MTB_ApplyMask(ctl, evt)
{
	var strVal, strMask, strOrigVal = ctl.value, keyCode = evt.keyCode;

    // Ignore certain keys so as not to format the text unnecessarily
    switch(keyCode)
    {
        case 0:
        case 13:
        case 27:
        case 44:
        case 45:
        case 46:
        case 91:
        case 93:
        case 144:
        case 145:
            return;

        default:
            // Ignore these ranges too
            if((keyCode > 15 && keyCode < 21) ||
               (keyCode > 32 && keyCode < 41) ||
               (keyCode > 111 && keyCode < 124))
                return;

            break;
    }

    strMask = ctl.mask;

    // Insert literals at the start of the text if necessary
	if(keyCode != 8 && keyCode != 9)
        if(strOrigVal.length == 0 && evt.type != "blur")
		    MTB_AppendMask(ctl);
        else
            if(strOrigVal.length == 1 && MTB_IsLiteral(strMask.charAt(0)))
            {
                // This catches cases where the first character typed
                // needs to come after a literal that starts the text.
                ctl.value = "";
                MTB_AppendMask(ctl);
                ctl.value += strOrigVal;
            }

    // Make the entered text conform to the mask
	strVal = MTB_MatchMask(ctl.value, strMask);

	if(ctl.value != strVal)
		ctl.value = strVal;

    // Append literal mask characters if necessary
	if(keyCode != 8 && ctl.value.length != 0)
		MTB_AppendMask(ctl);
}

// Find all text that matches the mask
function MTB_MatchMask(strValue, strMask)
{
	var nMaskIdx, nIdx, nSize = strValue.length;

	if(nSize == 0)
		return "";

    // Get the mask portion to use for the entered text.
    // The actual mask length needs to be adjusted for
    // escaped literals.
    for(nIdx = nMaskIdx = 0; nIdx < nSize; nIdx++, nMaskIdx++)
        if(strMask.charAt(nMaskIdx) == "\\")
            nMaskIdx++;

	var re = new RegExp("^" +
        MTB_ToRegExp(strMask.substr(0, nMaskIdx)) + "$");

    // If it doesn't match, strip off characters until it does
	if(!re.test(strValue))
		return MTB_MatchMask(strValue.substr(0, nSize - 1), strMask);

	return strValue;
}

// Append literal characters from the mask occurring after the
// last character.
function MTB_AppendMask(ctl)
{
	var chChar, nIdx, nMaskIdx;
    var strMask = ctl.mask;
    var nMaskSize = strMask.length;
	var strValue = ctl.value;
    var nSize = strValue.length;

    // Find the position in the mask that matches the
    // text entered so far.
    for(nIdx = nMaskIdx = 0; nIdx < nSize; nIdx++, nMaskIdx++)
        if(strMask.charAt(nMaskIdx) == "\\")
            nMaskIdx++;

    // Now append literal characters up to the next mask character
    while(nMaskIdx < nMaskSize)
    {
        chChar = strMask.charAt(nMaskIdx);

		switch(chChar)
        {
            case "0":       // Mask characters
            case "9":
            case "#":
            case "L":
            case "?":
            case "A":
            case "a":
            case "&":
            case "C":
				nMaskIdx = nMaskSize;
				break;

            case "\\":      // Escaped literal
                nMaskIdx++;
                strValue += strMask.charAt(nMaskIdx);
                break;

			default:        // Literal
				strValue += chChar;
                break;
		}

        nMaskIdx++;
	}

	if(ctl.value != strValue)
		ctl.value = strValue;
}

// See if the specified mask character is a literal
function MTB_IsLiteral(chMaskChar)
{
    return /[^09#L\?Aa&C]/.test(chMaskChar);
}

// Build a regular expression based on the passed mask
function MTB_ToRegExp(strMask)
{
    var chChar, nIdx, nLen = strMask.length, strRegExp = "";

    for(nIdx = 0; nIdx < nLen; nIdx++)
    {
        chChar = strMask.charAt(nIdx);

        switch(chChar)
        {
            case "\\":      // Escaped literal
                nIdx++;
                chChar = strMask.charAt(nIdx);
                strRegExp += "\\";      // Escape it here too for
                strRegExp += chChar;    // good measure.
                break;

            case "0":       // Digit (0 to 9, entry required, +/- not allowed)
                strRegExp += "\\d";
                break;

            case "9":       // Digit or space (entry optional, +/- not allowed)
                strRegExp += "[0-9 ]";
                break;

            case "#":       // Digit or space (entry optional; +/- signs allowed)
                strRegExp += "[0-9 +\-]";
                break;

            case "L":       // Letter (A to Z, entry required)
            case "?":       // Letter (A to Z, entry optional)
                strRegExp += "[A-Za-z]";
                break;

            case "A":       // Letter or digit (entry required)
            case "a":       // Letter or digit (entry optional)
                strRegExp += "[A-Za-z0-9]";
                break;

            case "&":       // Any character (entry required)
            case "C":       // Any character (entry optional)
                strRegExp += ".";
                break;

            default:        // Literal
                strRegExp += "\\";      // Escape it here too for
                strRegExp += chChar;    // good measure.
                break;
        }
    }

	return strRegExp;
}
