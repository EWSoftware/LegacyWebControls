//=============================================================================
// System  : ASP.NET Web Control Library
// File    : CtrlUtils.cs
// Author  : Eric Woodruff  (Eric@EWoodruff.us)
// Updated : Fri 02/20/04
// Note    : Copyright 2002-2004, Eric Woodruff, All rights reserved
// Compiler: Microsoft Visual C#
//
// This file contains a sealed class containing some utility functions used
// by the other classes in the EWSoftware.Web.Controls namespaces.  All members
// are static (shared) so just call them directly.
//
// Version     Date     Who  Comments
// ============================================================================
// 1.0.0    09/19/2002  EFW  Created the code
//=============================================================================

using System;
using System.Text;
using System.Text.RegularExpressions;

namespace EWSoftware.Web.Controls
{
    /// <summary>
    /// This enumerated type defines the locations in which a browser window
    /// can be opened with the <see cref="WindowOpener"/> control or the
    /// <see cref="CtrlUtils">CtrlUtils.OpenWindowJS</see> method.
    /// </summary>
    [Serializable]
    public enum WindowTarget
    {
        /// <summary>The URL is loaded into the same window if already open
        /// or a new, unnamed window if it is not already open.</summary>
        Same = 0,
        /// <summary>The URL is loaded into a new, unnamed window.</summary>
        Blank = 1,
        /// <summary>The URL is loaded into the HTML content area of the
        /// Media Bar. Available in Internet Explorer 6 or later.</summary>
        Media = 2,
        /// <summary>The URL is loaded into the current frame's parent. If
        /// the frame has no parent, this value acts like the value
        /// <b>Self</b>.</summary>
        Parent = 3,
        /// <summary>Available in Internet Explorer 5 and later. The URL is
        /// opened in the browser's search pane.</summary>
        Search = 4,
        /// <summary>The current document is replaced with the specified
        /// URL. </summary>
        Self = 5,
        /// <summary>URL replaces any framesets that may be loaded. If there
        /// are no framesets defined, this value acts like the value
        /// <b>Self</b>.</summary>
        Top = 6
    }

    /// <summary>
    /// This enumerated type defines the options that can be enabled for
    /// browser windows opened with the <see cref="WindowOpener"/> control
    /// or the <see cref="CtrlUtils">CtrlUtils.OpenWindowJS</see> method.
    /// </summary>
    [Flags, Serializable]
    public enum WindowOptions
    {
        /// <summary>Specifies whether to display the window in theater mode
        /// and show the channel band.  The default is disabled.</summary>
        ChannelMode = 0x00000001,
        /// <summary>Specifies whether to add directory buttons.  The
        /// default is enabled.</summary>
        Directories = 0x00000002,
        /// <summary>Specifies whether to display the browser in full-screen
        /// mode.  The default is disabled.</summary>
        FullScreen  = 0x00000004,
        /// <summary>Specifies whether to display the input field for entering URLs
        /// directly into the browser.  The default is enabled.</summary>
        Location    = 0x00000008,
        /// <summary>Specifies whether to display the menu bar. The default
        /// is enabled.</summary>
        MenuBar     = 0x00000010,
        /// <summary>Specifies whether to display resize handles at the
        /// corners of the window. The default is enabled.</summary>
        Resizable   = 0x00000020,
        /// <summary>Specifies whether to display horizontal and vertical
        /// scroll bars. The default is enabled.</summary>
        Scrollbars  = 0x00000040,
        /// <summary>Specifies whether to add a status bar at the bottom of
        /// the window. The default is enabled.</summary>
        Status      = 0x00000080,
        /// <summary>Specifies whether to display a title bar for the window.
        /// This parameter is ignored unless the calling application is an
        /// HTML Application or a trusted dialog box. The default is
        /// enabled.</summary>
        TitleBar    = 0x00000100,
        /// <summary>Specifies whether to display the browser toolbar, making
        /// buttons such as Back, Forward, and Stop available. The default
        /// is enabled.</summary>
        Toolbar     = 0x00000200,
        /// <summary>Use all options that have a default state of enabled.</summary>
        Defaults    = Directories | Location | MenuBar | Resizable |
                      Scrollbars | Status | TitleBar | Toolbar
    }

	/// <summary>
	/// This class contains a set of common utility methods used by the
    /// classes in this namespace.
	/// </summary>
    /// <remarks>The methods in this class are more generic in nature and may
    /// be of use to code outside of the specific control classes.</remarks>
	public sealed class CtrlUtils
	{
        //=====================================================================
        // Constants

        /// <summary>
        /// These are the project namespace prefixes for embedded resources
        /// </summary>
        internal const string ScriptsPath = "EWSoftware.Web.Controls.Scripts.";
        internal const string HtmlPath = "EWSoftware.Web.Controls.HTML.";

        //=====================================================================
        // Methods, etc

        /// Private constructor.  This class cannot be instantiated.
		private CtrlUtils()
		{
		}

        /// <summary>
        /// This method returns a string containing a JavaScript call to the
        /// <b>window.open</b> method that uses the specified options to open
        /// a browser window and give it the focus.
        /// </summary>
        /// <param name="strURL">The URL to open in the window</param>
        /// <param name="wt">The target in which to open the window</param>
        /// <param name="nLeft">The position of the left side of the window</param>
        /// <param name="nTop">The position of the top of the window</param>
        /// <param name="nHeight">The height of the window</param>
        /// <param name="nWidth">The width of the window</param>
        /// <param name="wo">The window options to enable</param>
        /// <param name="bReplaceHistory">If set to open in the current window,
        /// this determines whether the new URL replaces the current one in
        /// the browser's history list.</param>
        /// <param name="strName">The name of the window if the target is set
        /// to <b>Same</b></param>
        /// <returns>The JavaScript <b>window.open</b> call based on the
        /// specified parameters.  The string is terminated with a semi-colon.
        /// </returns>
        /// <example>
        /// C#:
        /// <code>
        /// // Create the window.open call
        /// string strScript = "javascript:";
        ///
        /// strScript += CtrlUtils.OpenWindowJS("http://www.test.com",
        ///     WindowTarget.Blank, 50, 50, 600, 700, WindowOptions.Defaults,
        ///     false, null);
        ///
        /// strScript += " return false;";
        ///
        /// // Assign it to the OnClick event of a button control
        /// btnLink.Attributes.Add("onclick", strScript);
        /// </code>
        /// VB.NET:
        /// <code lang="vbnet">
        /// ' Create the window.open call
        /// Dim strScript As String = "javascript:"
        ///
        /// strScript += CtrlUtils.OpenWindowJS("http://www.test.com",
        ///     WindowTarget.Blank, 50, 50, 600, 700, WindowOptions.Defaults,
        ///     False, Nothing)
        ///
        /// strScript += " return false;"
        ///
        /// ' Assign it to the OnClick event of a button control
        /// btnLink.Attributes.Add("onclick", strScript)
        /// </code>
        /// </example>
        public static string OpenWindowJS(string strURL, WindowTarget wt,
          int nLeft, int nTop, int nHeight, int nWidth, WindowOptions wo,
          bool bReplaceHistory, string strName)
        {
            // Build the window opener JavaScript
            StringBuilder strScript = new StringBuilder("window.open('", 256);

            strScript.Append(strURL);
            strScript.Append("','");

            // If using same window, set the name.  If no name is specified,
            // use a default name.
            if(wt == WindowTarget.Same)
            {
                if(strName == null)
                    strScript.Append("SameOne");
                else
                    strScript.Append(strName);
            }
            else
            {
                strScript.Append('_');
                strScript.Append(wt.ToString().ToLower());
            }

            strScript.Append("','");

            // Add the size and position options
            strScript.Append("left=");
            strScript.Append(nLeft);
            strScript.Append(",top=");
            strScript.Append(nTop);
            strScript.Append(",height=");
            strScript.Append(nHeight);
            strScript.Append(",width=");
            strScript.Append(nWidth);

            // Add the window options
            for(int nBit = 1; nBit <= (int)WindowOptions.Toolbar; nBit <<= 1)
            {
                strScript.Append(',');
                strScript.Append(((WindowOptions)nBit).ToString());
                strScript.Append('=');

                if(((int)wo & nBit) != 0)
                    strScript.Append("yes");
                else
                    strScript.Append("no");
            }

            strScript.Append("',");
            strScript.Append(bReplaceHistory.ToString().ToLower());
            strScript.Append(").focus();");

            return strScript.ToString();
        }

        /// <summary>
        /// This takes a variable argument list and performs replacements on
        /// the specified message string.
        /// </summary>
        /// <remarks>The passed list must have a length that is a multiple of
        /// two.  The first element is the match string, the second element is
        /// the replacement string, repeat for as many replacement pairs as
        /// needed.</remarks>
        /// <param name="strMsg">The message on which to do the replacements</param>
        /// <param name="strReplace">The set of replacements to perform</param>
        /// <returns>The specified message with replacements</returns>
        /// <example>
        /// C#:
        /// <code>
        /// strFormatted = CtrlUtils.ReplaceParameters(strTemplate,
        ///     "{Param1}", strValue1, "{Param2}", strValue2);
        /// </code>
        /// VB.NET:
        /// <code lang="vbnet">
        /// strFormatted = CtrlUtils.ReplaceParameters(strTemplate, _
        ///     "{Param1}", strValue1, "{Param2}", strValue2)
        /// </code>
        /// </example>
        /// <seealso cref="DateTextBox.DateErrorMessage"/>
        /// <seealso cref="EWSTextBox.MaxLenErrorMessage"/>
        /// <seealso cref="MinMaxListBox.MinMaxErrorMessage">MinMaxListBox.MinMaxErrorMessage</seealso>
        /// <seealso cref="MinMaxCheckBoxList.MinMaxErrorMessage">MinMaxCheckBoxList.MinMaxErrorMessage</seealso>
        /// <seealso cref="NumericTextBox.DecimalErrorMessage"/>
        /// <seealso cref="NumericTextBox.NumberErrorMessage"/>
        public static string ReplaceParameters(string strMsg,
          params string[] strReplace)
        {
            // Don't bother if it's null or empty
            if(strMsg == null || strMsg.Length == 0 || strReplace == null)
                return strMsg;

            StringBuilder strRepMsg = new StringBuilder(strMsg, strMsg.Length + 50);

            for(int nIdx = 0; nIdx < strReplace.Length; nIdx += 2)
                strRepMsg.Replace(strReplace[nIdx], strReplace[nIdx + 1]);

            return strRepMsg.ToString();
        }

        /// <summary>
        /// Convert a string to proper case (i.e. a name).
        /// </summary>
        /// <param name="strText">The text to convert to proper case</param>
        /// <param name="strUCWords">Additional words to convert to
        /// uppercase. Pass null if there aren't any. If there are
        /// additional words such as suffixes and abbreviations that should
        /// be converted to uppercase besides the standard set, pass them
        /// in this parameter separated by the pipe (|) character.  The
        /// standard set includes DBA, LLP, LLC, PS, II, III, IV, VI,
        /// VII, and VIII.</param>
        /// <returns>The specified text converted to proper case</returns>
        /// <example>
        /// C#:
        /// <code>
        /// strProper1 = CtrlUtils.ToProperCase(strName1, null);
        /// strProper2 = CtrlUtils.ToProperCase(strName2, "abc");
        /// strProper3 = CtrlUtils.ToProperCase(strName3, "def|xyz");
        /// </code>
        /// VB.NET:
        /// <code lang="vbnet">
        /// strProper1 = CtrlUtils.ToProperCase(strName1, Nothing);
        /// strProper2 = CtrlUtils.ToProperCase(strName2, "abc");
        /// strProper3 = CtrlUtils.ToProperCase(strName3, "def|xyz");
        /// </code>
        /// </example>
        /// <seealso cref="EWSTextBox.Text"/>
        public static string ToProperCase(string strText, string strUCWords)
        {
            string strWords = @"\b(dba|llp|llc|ps|iii|ii|iv|viii|vii|vi";

            if(strText == null)
                return null;

            // Are there any additional words to add?
            if(strUCWords != null)
                strWords += "|" + strUCWords;

            strWords += @")\b";

            // Set up the match evaluators
            MatchEvaluator WordMatchEval = new MatchEvaluator(OnWordMatch),
                McMatchEval = new MatchEvaluator(OnMcMatch),
                AllUCMatchEval = new MatchEvaluator(OnAllUCMatch),
                ApostMatchEval = new MatchEvaluator(OnApostMatch);

            // Create the regular expressions
            Regex reWords = new Regex(@"(\w+)");
            Regex reMcWords = new Regex(@"(\bmc\w+\b)", RegexOptions.IgnoreCase);
            Regex reAllUC = new Regex(strWords, RegexOptions.IgnoreCase);
            Regex reApost = new Regex(@"'\w\b");

            // Convert the whole string to lowercase and then change
            // the first letter of each word, the letter after all
            // occurrences of "Mc", and the specified "words" to uppercase.
            // Single letters after an apostrophe on word boundaries are
            // convert to lowercase.
            strText = strText.ToLower();
            strText = reWords.Replace(strText, WordMatchEval);
            strText = reMcWords.Replace(strText, McMatchEval);
            strText = reAllUC.Replace(strText, AllUCMatchEval);
            strText = reApost.Replace(strText, ApostMatchEval);

            return strText;
        }

        // Convert the first letter of each word found to uppercase
        private static string OnWordMatch(Match match)
        {
            string strMatch = match.Value;
            return strMatch.Substring(0, 1).ToUpper() + strMatch.Substring(1);
        }

        // Convert the letter after each occurrence of "Mc" to uppercase.
        // Note that the word evaluator above will handle the "M" in "Mc".
        private static string OnMcMatch(Match match)
        {
            string strMatch = match.Value;
            return strMatch.Substring(0, 2) +
                strMatch.Substring(2, 1).ToUpper() + strMatch.Substring(3);
        }

        // Convert selected "words" such as name titles to all uppercase
        // (i.e. II, III, etc).
        private static string OnAllUCMatch(Match match)
        {
            return match.Value.ToUpper();
        }

        // Convert single letters after an apostrophe to lowercase if
        // they are on a word boundary.
        private static string OnApostMatch(Match match)
        {
            return match.Value.ToLower();
        }
	}
}
