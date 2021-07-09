using Serilog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
//using WebTest.Collections;

namespace Extensions
{
    public static class StringExtensions
    {
        static int MAX_LEN = 10485759;  // 10 Mb - 1 byte. If length is 10MB or larger, then string was truncated.

        #region -- FindSubString -----
        public static string FindSubString(this string source, string startsAfter, string endsWith)
        {
            int x = 0;
            return FindSubString(source, startsAfter, endsWith, 1, StringComparison.InvariantCulture, ref x, false);
        }

        public static string FindSubString(this string source, string startsAfter, string endsWith, bool doNotFailOnMissingEndsWith)
        {
            int x = 0;
            return FindSubString(source, startsAfter, endsWith, 1, StringComparison.InvariantCulture, ref x, doNotFailOnMissingEndsWith);
        }

        public static string FindSubString(this string source, string startsAfter, string endsWith, int iIndex)
        {
            int x = 0;
            return FindSubString(source, startsAfter, endsWith, iIndex, StringComparison.InvariantCulture, ref x, false);
        }

        public static string FindSubString(this string source, string startsAfter, string endsWith, int iIndex, StringComparison comp)
        {
            int x = 0;
            return FindSubString(source, startsAfter, endsWith, iIndex, StringComparison.InvariantCulture, ref x, false);
        }

        public static string FindSubString(this string source, string startsAfter, string endsWith, int iIndex, ref int subStringFoundAtLocation)
        {
            return FindSubString(source, startsAfter, endsWith, iIndex, StringComparison.InvariantCulture, ref subStringFoundAtLocation, false);
        }

        public static string FindSubString(this string source, string startsAfter, string endsWith, int iIndex, StringComparison comp, ref int subStringFoundAtLocation, bool doNotFailOnMissingEndsWith)
        {
            subStringFoundAtLocation = -1;
            if (string.IsNullOrEmpty(source))
                return string.Empty;

            int iStart = -1;
            int iCurrent = 0;
            int iLoopCounter = 0;

            for (iLoopCounter = 0; iLoopCounter < iIndex; iLoopCounter++)
            {
                iStart = source.IndexOf(startsAfter, iCurrent, comp);
                if (iStart == -1)
                    return String.Empty;
                iCurrent = iStart + startsAfter.Length;
            }

            int iEnd = source.IndexOf(endsWith, iCurrent, comp);
            if (iEnd == -1 && doNotFailOnMissingEndsWith == false)
                return String.Empty;

            subStringFoundAtLocation = iCurrent;
            if (iEnd == -1)
            {
                return source.Substring(iCurrent);
            }
            return source.Substring(iCurrent, iEnd - iCurrent);
        }
        #endregion

        #region -- Misc -----
        public static bool Contains(this String str, String substring, StringComparison comp)
        {
            if (substring == null)
                throw new ArgumentNullException("substring",
                                                "substring cannot be null.");
            else if (!Enum.IsDefined(typeof(StringComparison), comp))
                throw new ArgumentException("comp is not a member of StringComparison",
                                            "comp");

            int x = str.IndexOf(substring, comp);
            if (x > -1)
            {
                return true;
            }
            return false;
        }

        public static bool IsBoolean(this string source)
        {
            if (source.Length > 5)
                return false;
            if (source.ToUpper() == "TRUE" || source.ToUpper() == "FALSE")
                return true;
            return false;
        }

        public static Type IsNumeric(this string source)
        {
            // https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/integral-numeric-types
            if (Int32.TryParse(source, System.Globalization.NumberStyles.Number, CultureInfo.InvariantCulture, out _) == true)
                return typeof(System.Int32);
            else if (double.TryParse(source, out _) == true)
                return typeof(System.Double);
            else
                return typeof(System.String);
        }

        public static string GetUrlWithoutQuery(this String str)
        {
            string decoded = str.UrlDecode();
            int x = decoded.IndexOf("?");
            if (x == -1)
                return decoded;
            else
                return decoded.Substring(0, x);
        }

        public static string GetLeftPart(this string source, string delimiter, bool returnEmptyIfDelimiterNotFound = true)
        {
            int iEnd = source.IndexOf(delimiter);

            if (iEnd == -1)
            {
                if (returnEmptyIfDelimiterNotFound == true)
                {
                    return string.Empty;
                }
                else
                {
                    return source;
                }
            }

            return source.Substring(0, iEnd);
        }

        public static string GetRightPart(this string source, string delimiter)
        {
            int iStart = source.IndexOf(delimiter);

            if (iStart == -1)
            {
                return string.Empty;
            }

            return source.Substring(iStart + delimiter.Length);
        }

        public static string Shortened(this String str, int maxLen)
        {
            if (str.Length >= maxLen && maxLen > 13)
                return str.Substring(0, maxLen - 13) + "**TRUNCATED**";
            else if (maxLen <= 13)
                return str.Substring(0, maxLen);
            else
                return str;
        }

        public static string Flattened(this String str, int maxLen)
        {
            string newStr = str.Replace("\r\n", "").Replace("\r", "").Replace("\n", "");
            return newStr.Shortened(maxLen);
        }
        #endregion

        #region -- Returns lists etc -----
        public static List<string> CsvStrToList(this string source)
        {
            List<string> list = new List<string>();
            try
            {
                foreach (string subStr in source.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries))
                {
                    list.Add(subStr);
                }
            }
            catch (Exception ex)
            {
                Log.ForContext("SourceContext", "StringExtensions").Error(ex, "{method} failed.", "CsvStrToList");
                list.Clear();
            }
            return list;
        }

        public static int[] DelimitedStringToIntArray(this string source, string stringSplitText)
        {
            try
            {
                string cleanedSource = source.Replace("[", "").Replace("]", "");
                List<string> list = new List<string>();
                foreach (string subStr in cleanedSource.Split(new string[] { stringSplitText }, StringSplitOptions.RemoveEmptyEntries))
                {
                    list.Add(subStr);
                }

                int[] intArray = new int[list.Count];

                for (int x = 0; x < list.Count; x++)
                {
                    intArray[x] = Int32.Parse(list[x]);
                }
                return intArray;
            }

            catch
            {
            }

            return new int[0];
        }

        public static List<string> FindSubStrings(this string source, string startsAfter, string endsWith)
        {
            int x = 0;
            int loopCounter = 1;
            string item = "";
            List<string> items = new List<string>();

            item = FindSubString(source, startsAfter, endsWith, loopCounter, StringComparison.InvariantCulture, ref x, false);

            while (item != string.Empty)
            {
                items.Add(item);
                loopCounter++;
                item = FindSubString(source, startsAfter, endsWith, loopCounter, StringComparison.InvariantCulture, ref x, false);
            }
            return items;
        }

        public static MemoryStream ToStream(this string str)
        {
            return ToStream(str, false);
        }

        public static MemoryStream ToStream(this string str, bool leaveStreamOpen)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream, System.Text.Encoding.ASCII, str.Length, leaveStreamOpen);
            writer.Write(str);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
        #endregion

        #region -- modifiers -----
        public static string RemoveQuoteEscapes(this String str)
        {
            return str.Replace("\\\"", "\"");
        }

        public static string RemoveEncapsulatingQuotes(this String str)
        {
            if (str.StartsWith("\"") && str.EndsWith("\""))
                return str.Substring(1, str.Length - 2);
            return str.Replace("\\\"", "\"");
        }

        public static string Replace(this string source, string oldString, string newString, StringComparison comp)
        {
            int index = source.IndexOf(oldString, comp);

            // Determine if we found a match
            bool MatchFound = index >= 0;

            if (MatchFound)
            {
                // Remove the old text
                source = source.Remove(index, oldString.Length);

                // Add the replacemenet text
                source = source.Insert(index, newString);
            }
            return source;
        }

        public static string Remove(this string source, string strToRemove)
        {
            return Remove(source, strToRemove, StringComparison.CurrentCulture);
        }

        public static string Remove(this string source, string strToRemove, StringComparison comp)
        {
            int index = source.IndexOf(strToRemove, comp);

            // Determine if we found a match
            bool MatchFound = index >= 0;

            if (MatchFound)
            {
                // Remove the old text
                source = source.Remove(index, strToRemove.Length);
            }
            return source;
        }

        public static string AddBraces(this string source)
        {
            return "{{" + source + "}}";
        }

        public static string ToCamelCase(this string value)
        {
            if (string.IsNullOrEmpty(value)) return value;

            var cameCasedParts = value.Split('.').Select(part => char.ToLowerInvariant(part[0]) + part.Substring(1));

            return string.Join(".", cameCasedParts);
        }
        #endregion

        #region -- Encoders -----
        public static string UrlDecode(this String str)
        {
            return HttpUtility.UrlDecode(str);
        }

        public static string Base64Encode(this string sIn)
        {
            if (sIn.Length < MAX_LEN)
            {
                int i = 0;
                byte[] _byte = new byte[(sIn.Length)];
                foreach (char c in sIn.ToCharArray())
                {
                    _byte[i++] = (byte)c;
                }
                return Convert.ToBase64String(_byte);
            }
            else
                return sIn;
        }

        public static string Base64Decode(this string sIn)
        {
            if (sIn.Length < MAX_LEN)
            {
                byte[] _byte = new byte[(sIn.Length)];
                _byte = Convert.FromBase64String(sIn);

                StringBuilder s = new StringBuilder();
                foreach (char c in _byte)
                {
                    // Quick hack to deal with unicode. Needs to be fixed
                    if (c != 0)
                        s.Append(c);
                }
                return s.ToString();
            }
            else
                return sIn;
        }
        #endregion

    }
}
