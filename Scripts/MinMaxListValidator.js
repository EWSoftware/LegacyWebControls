//=============================================================================
// File    : MinMaxListValidator.js
// Author  : Eric Woodruff  (Eric@EWoodruff.us)
// Updated : 03/23/2003
function MMLB_MinMaxEvaluateIsValid(a){var b,c=0;var d=document.getElementById(a.controltovalidate);for(b=0;b<d.options.length;b++)if(d.options[b].selected)c++;if(c<a.minsel||(a.maxsel>0&&c>a.maxsel))return false;return true;}function MMCKRB_MinMaxEvaluateIsValid(a){var e,f,g,h,b,c=0;g=a.controltovalidate+"_";h=g.length;e=document.getElementsByTagName('*');for(b=0;b<e.length;b++){f=e[b];if(f.id.substr(0,h)==g)if(f.checked)c++;}if(c<a.minsel||(a.maxsel>0&&c>a.maxsel))return false;return true;}