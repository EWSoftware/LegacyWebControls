//=============================================================================
// File    : MaskTextBox.js
// Author  : Eric Woodruff  (Eric@EWoodruff.us)
// Updated : Sun 12/14/03 15:55:39
function MTB_ApplyMask(a,b){var f,d,g=a.value,h=b.keyCode;switch(h){case 0:case 13:case 27:case 44:case 45:case 46:case 91:case 93:case 144:case 145:return;default:if((h>15&&h<21)||(h>32&&h<41)||(h>111&&h<124))return;break;}d=a.mask;if(h!=8&&h!=9)if(g.length==0&&b.type!="blur")MTB_AppendMask(a);else if(g.length==1&&MTB_IsLiteral(d.charAt(0))){a.value="";MTB_AppendMask(a);a.value+=g;}f=MTB_MatchMask(a.value,d);if(a.value!=f)a.value=f;if(h!=8&&a.value.length!=0)MTB_AppendMask(a);}function MTB_MatchMask(c,d){var i,j,k=c.length;if(k==0)return"";for(j=i=0;j<k;j++,i++)if(d.charAt(i)=="\\")i++;var l=new RegExp("^"+MTB_ToRegExp(d.substr(0,i))+"$");if(!l.test(c))return MTB_MatchMask(c.substr(0,k-1),d);return c;}function MTB_AppendMask(a){var m,j,i;var d=a.mask;var n=d.length;var c=a.value;var k=c.length;for(j=i=0;j<k;j++,i++)if(d.charAt(i)=="\\")i++;while(i<n){m=d.charAt(i);switch(m){case"0":case"9":case"#":case"L":case"?":case"A":case"a":case"&":case"C":i=n;break;case"\\":i++;c+=d.charAt(i);break;default:c+=m;break;}i++;}if(a.value!=c)a.value=c;}function MTB_IsLiteral(e){return/[^09#L\?Aa&C]/.test(e);}function MTB_ToRegExp(d){var m,j,o=d.length,p="";for(j=0;j<o;j++){m=d.charAt(j);switch(m){case"\\":j++;m=d.charAt(j);p+="\\";p+=m;break;case"0":p+="\\d";break;case"9":p+="[0-9 ]";break;case"#":p+="[0-9 +\-]";break;case"L":case"?":p+="[A-Za-z]";break;case"A":case"a":p+="[A-Za-z0-9]";break;case"&":case"C":p+=".";break;default:p+="\\";p+=m;break;}}return p;}