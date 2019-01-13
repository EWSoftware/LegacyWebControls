// #pragma NoCompStart
//=============================================================================
// File    : MinMaxListValidator.js
// Author  : Eric Woodruff  (Eric@EWoodruff.us)
// Updated : 03/23/2003
// #pragma NoCompEnd
//
// This contains the client-side code for the min/max list validators

// Ensure that a min/max number of items are selected in a list box
function MMLB_MinMaxEvaluateIsValid(val)
{
	var nIdx, nCount = 0;
	var ctl = document.getElementById(val.controltovalidate);

	// Count the selected items
	for(nIdx = 0; nIdx < ctl.options.length; nIdx++)
		if(ctl.options[nIdx].selected)
			nCount++;

	// Maximum is optional.  It won't be used if set to zero.
	if(nCount < val.minsel || (val.maxsel > 0 && nCount > val.maxsel))
		return false;

	return true;
}

// Ensure that a min/max number of items are selected in a checkbox list
// or radio button list.  For a radio button list, it's always going to
// be a minimum and maximum of one but the code is the same.
function MMCKRB_MinMaxEvaluateIsValid(val)
{
    var htmlColl, ctrl, strPrefix, nLen, nIdx, nCount = 0;

    // Get the list control item prefix
    strPrefix = val.controltovalidate + "_";
    nLen = strPrefix.length;

    // Now search the document for items with the prefix that are
    // checked/selected.
    htmlColl = document.getElementsByTagName('*');

    for(nIdx = 0; nIdx < htmlColl.length; nIdx++)
    {
        ctrl = htmlColl[nIdx];

        if(ctrl.id.substr(0, nLen) == strPrefix)
			if(ctrl.checked)
				nCount++;
    }

	// Maximum is optional.  It won't be used if set to zero.
	if(nCount < val.minsel || (val.maxsel > 0 && nCount > val.maxsel))
		return false;

	return true;
}
