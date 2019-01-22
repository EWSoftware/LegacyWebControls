//=============================================================================
// File    : CCValidator.js
// Author  : Eric Woodruff  (Eric@EWoodruff.us)
// Updated : 04/09/2003
//
// This contains the client-side credit card validator code.

var CCV_lCurrType;

// Validate the credit card number
function CCV_ValidatorEvaluateIsValid(val)
{
	var lCardTypes = parseInt(val.cardtypes, 10);
    var strCardNbr = ValidatorGetValue(val.controltovalidate);
    var bRetVal, strTypeCtl, lType;

    strCardNbr = ValidatorTrim(strCardNbr);

    // If the control is empty, it will return true
    if(strCardNbr.length == 0)
		return true;

	// Check the number based on the card type.  If that's okay,
    // validate the card number.
    CCV_lCurrType = 0;
    bRetVal = CCV_ValidateCardNumber(strCardNbr, lCardTypes);

	// Is there a card type control to check?  If so, do it.
    if(bRetVal && typeof(val.cardtypectl) == "string")
    {
		strTypeCtl = val.cardtypectl;

		if(bRetVal && strTypeCtl != "")
		{
			lType = ValidatorGetValue(strTypeCtl);
			if(parseInt(lType, 10) != CCV_lCurrType)
				bRetVal = false;
		}
	}

	return bRetVal;
}

// Validate the card number by type and checksum.  Also sets the current type.
// NOTE: If the regular expressions change, update the server-side code too!
function CCV_ValidateCardNumber(strCardNbr, lCardTypes)
{
    // VISA starts with 4 and is 13 or 16 digits
    if((lCardTypes & 0x00000001) != 0 &&
      /^(4\d{12})|(4\d{15})$/.test(strCardNbr))
    {
		CCV_lCurrType = 0x00000001;
        return CCV_TestChecksum(strCardNbr);
    }

    // MasterCard starts with 51 through 55 and is 16 digits
    if((lCardTypes & 0x00000002) != 0 && /^5[1-5]\d{14}$/.test(strCardNbr))
    {
		CCV_lCurrType = 0x00000002;
        return CCV_TestChecksum(strCardNbr);
    }

    // AMEX starts with 34 or 37 and is 15 digits
    if((lCardTypes & 0x00000004) != 0 && /^3[47]\d{13}$/.test(strCardNbr))
    {
		CCV_lCurrType = 0x00000004;
        return CCV_TestChecksum(strCardNbr);
    }

    // Discover starts with 6011 and is 16 digits
    if((lCardTypes & 0x00000008) != 0 && /^6011\d{12}$/.test(strCardNbr))
    {
		CCV_lCurrType = 0x00000008;
        return CCV_TestChecksum(strCardNbr);
    }

    // Diners Club starts with 300-305, 36, or 38, and is 14 digits
    if((lCardTypes & 0x00000010) != 0 &&
      /^(30[0-5]\d{11})|(3[68]\d{12})$/.test(strCardNbr))
    {
		CCV_lCurrType = 0x00000010;
        return CCV_TestChecksum(strCardNbr);
    }

    // enRoute starts with 2014 or 2149 and is 15 digits
    if((lCardTypes & 0x00000020) != 0 && /^(2014|2149)\d{11}/.test(strCardNbr))
    {
		CCV_lCurrType = 0x00000020;
        return CCV_TestChecksum(strCardNbr);
    }

    // JCB starts with 3 and is 16 digits or
    // starts with 2131 or 1800 and is 15 digits.
    if((lCardTypes & 0x00000040) != 0 &&
      /^(3\d{15})|((2131|1800)\d{11})$/.test(strCardNbr))
    {
		CCV_lCurrType = 0x00000040;
        return CCV_TestChecksum(strCardNbr);
    }

    // Couldn't recognize anything to this point.  Are we accepting
    // Other as a card type?  If so, return true and rely on the
    // number passing the checksum validation.
    if((lCardTypes & 0x01000000) != 0)
    {
		CCV_lCurrType = 0x01000000;
        return CCV_TestChecksum(strCardNbr);
    }

    // Not valid
    return false;
}

// Validate the card number checksum using Luhn's formula
function CCV_TestChecksum(strCardNbr)
{
    var nDigit, nProdDigit, strProduct, nChecksum = 0;

    // Work from right to left
    for(nDigit = strCardNbr.length - 1; nDigit >= 0; nDigit--)
    {
        // Add the value of the first digit
        nChecksum += parseInt(strCardNbr.charAt(nDigit), 10);
        nDigit--;

        if(nDigit >= 0)
        {
            // For each alternating digit, add the sum of the digits
            // for the digit value times two.
            strProduct = String(parseInt(strCardNbr.charAt(nDigit), 10) * 2);

            for(nProdDigit = 0; nProdDigit < strProduct.length; nProdDigit++)
                nChecksum += parseInt(strProduct.charAt(nProdDigit), 10);
        }
    }

    // It's valid if the mod 10 value is zero
    return (nChecksum % 10 == 0);
}
