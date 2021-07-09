using System;
using Xunit;
using Extensions;
using System.Threading;
using System.Collections.Generic;
using Serilog;
using Xunit.Abstractions;
using System.Linq;
using System.Text;
using System.IO;

namespace Extensions.Test
{
    // https://andrewlock.net/creating-strongly-typed-xunit-theory-test-data-with-theorydata/

    [Collection("xUnitHelper Collection")]
    public class StringExtensions_Tests : IClassFixture<xUnitClassFixture>
    {
        xUnitCollectionFixture _collectionFixture;
        xUnitClassFixture _classFixture;
        string testStr = "The quick Brown fox jumped over the lazy brown dog.";

        public StringExtensions_Tests(xUnitClassFixture classFixture, xUnitCollectionFixture collectionFixture, ITestOutputHelper output)
        {
            _collectionFixture = collectionFixture;
            _classFixture = classFixture;
            collectionFixture.ConfigureLogging(output, @"c:\temp\ListExtensions_Testing");
        }

        #region -- FindSubString -----
        [Theory]
        [InlineData("qu", " ", 1, StringComparison.InvariantCulture, -1, false, "ick")]
        [InlineData(" ", " ", 3, StringComparison.InvariantCulture, -1, false, "fox")]
        [InlineData("brown ", ".", 1, StringComparison.InvariantCulture, -1, false, "dog")]
        [InlineData("brown ", " ", 1, StringComparison.CurrentCultureIgnoreCase, -1, false, "fox")]
        [InlineData("brown ", ".", 1, StringComparison.InvariantCulture, 47, false, "dog")]
        [InlineData("brown ", " ", 1, StringComparison.CurrentCultureIgnoreCase, 16, false, "fox")]
        [InlineData("lazy", "cat", 1, StringComparison.InvariantCulture, -1, false, "")]
        [InlineData("lazy", "cat", 1, StringComparison.InvariantCulture, -1, true, " brown dog.")]
        public void StringExtensions_FindSubString_Test(string startsAfter, string endsWith, int iIndex, StringComparison comp, int subStringFoundAtLocation, bool doNotFailOnMissingEndsWith, string expectedResult)
        {
            int x = 0;
            string actualResult = testStr.FindSubString(startsAfter, endsWith, iIndex, comp, ref x, doNotFailOnMissingEndsWith);
            Log.Information("{actualResult} was found.", actualResult);
            Assert.Equal(expectedResult, actualResult);
            if(subStringFoundAtLocation != -1)
            {
                Log.Information("FindSubString_Test included {foundAtLocation} validation.", subStringFoundAtLocation);
                Assert.Equal(subStringFoundAtLocation, x);
            }
        }
        #endregion

        #region -- Misc -----
        [Theory]
        [InlineData("fox", StringComparison.InvariantCulture, true)]
        [InlineData("Fox", StringComparison.InvariantCulture, false)]
        [InlineData("fox", StringComparison.InvariantCultureIgnoreCase, true)]
        [InlineData("Fox", StringComparison.InvariantCultureIgnoreCase, true)]
        [InlineData("cat", StringComparison.InvariantCulture, false)]
        public void StringExtensions_Contains_Test(string subString, StringComparison comp, bool expectedResult)
        {
            bool actualResult = testStr.Contains(subString, comp);
            Assert.Equal(expectedResult, actualResult);
        }

        [Theory]
        [InlineData("true", true)]
        [InlineData("True", true)]
        [InlineData("FALSE", true)]
        [InlineData("falSE", true)]
        [InlineData("False ", false)]
        [InlineData("0", false)]
        [InlineData("Really? Not this one", false)]
        public void StringExtensions_IsBoolean_Test(string inputStr, bool expectedResult)
        {
            bool actualResult = inputStr.IsBoolean();
            Assert.Equal(expectedResult, actualResult);
        }

        [Theory]
        [InlineData("one", typeof(System.String))]
        [InlineData("1", typeof(System.Int32))]
        [InlineData("2147483647", typeof(System.Int32))]
        [InlineData("2147483648", typeof(System.Double))]
        [InlineData("2,147,483,646", typeof(System.Int32))]
        [InlineData("-2,147,483,647", typeof(System.Int32))]
        [InlineData("2,147,483,648", typeof(System.Double))]
        [InlineData("1.0", typeof(System.Int32))]
        [InlineData("1.1", typeof(System.Double))]
        [InlineData("0.1", typeof(System.Double))]
        [InlineData("111111111111111111111111111111.111111111111111111111111111111", typeof(System.Double))]
        [InlineData("", typeof(System.String))]
        public void StringExtensions_IsNumeric_Test(string inputStr, Type expectedResult)
        {
            // https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/integral-numeric-types
            Type actualResult = inputStr.IsNumeric();
            Assert.Equal(expectedResult, actualResult);
        }

        [Theory]
        [InlineData("https://www.mysite.com/myfolder/mypage?item1=value1&item2={value2}", "https://www.mysite.com/myfolder/mypage")]
        [InlineData("https%3a%2f%2fwww.mysite.com%2fmyfolder%2fmypage%3fitem1%3dvalue1%26item2%3d%7bvalue2%7d", "https://www.mysite.com/myfolder/mypage")]
        [InlineData("http://bing.com", "http://bing.com")]
        [InlineData("http%3a%2f%2fbing.com", "http://bing.com")]
        public void StringExtensions_GetUrlWithoutQuery_Test(string inputStr, string expectedResult)
        {
            string actualResult = inputStr.GetUrlWithoutQuery();
            Assert.Equal(expectedResult, actualResult);
        }

        [Theory]
        [InlineData("fox", false, "The quick Brown ")]
        [InlineData("cat", true, "")]
        [InlineData("cat", false, "The quick Brown fox jumped over the lazy brown dog.")]
        public void StringExtensions_GetLeftPart_Test(string delimiter, bool returnEmptyIfDelimiterNotFound, string expectedResult)
        {
            string actualResult = testStr.GetLeftPart(delimiter, returnEmptyIfDelimiterNotFound);
            Assert.Equal(expectedResult, actualResult);
        }

        [Theory]
        [InlineData("fox", " jumped over the lazy brown dog.")]
        [InlineData("cat", "")]
        public void StringExtensions_GetRightPart_Test(string delimiter, string expectedResult)
        {
            string actualResult = testStr.GetRightPart(delimiter);
            Assert.Equal(expectedResult, actualResult);
        }

        [Theory]
        [InlineData(12, "The quick Br")]
        [InlineData(15, "Th**TRUNCATED**")]
        [InlineData(23, "The quick **TRUNCATED**")]
        [InlineData(45, "The quick Brown fox\r\njumped over**TRUNCATED**")]
        [InlineData(75, "The quick Brown fox\r\njumped over the\nlazy brown dog; several t**TRUNCATED**")]
        [InlineData(76, "The quick Brown fox\r\njumped over the\nlazy brown dog; several times in a row")]
        public void StringExtensions_Shortened_Test(int maxLen, string expectedResult)
        {
            string longTestStr = "The quick Brown fox\r\njumped over the\nlazy brown dog; several times in a row";
            string actualResult = longTestStr.Shortened(maxLen);
            Assert.Equal(expectedResult, actualResult);
        }

        [Theory]
        [InlineData(12, "The quick Br")]
        [InlineData(15, "Th**TRUNCATED**")]
        [InlineData(23, "The quick **TRUNCATED**")]
        [InlineData(42, "The quick Brown foxjumped ove**TRUNCATED**")]
        [InlineData(72, "The quick Brown foxjumped over thelazy brown dog; several t**TRUNCATED**")]
        [InlineData(73, "The quick Brown foxjumped over thelazy brown dog; several times in a row")]
        public void StringExtensions_Flattened_Test(int maxLen, string expectedResult)
        {
            string longTestStr = "The quick Brown fox\r\njumped over the\nlazy brown dog; several times in a row";
            string actualResult = longTestStr.Flattened(maxLen);
            Assert.Equal(expectedResult, actualResult);
        }
        #endregion

        #region -- Returns lists etc -----
        [Fact]
        public void StringExtensions_CsvStrToList_Test()
        {
            string inputStr = "Value1,Value2,Value3";
            List<string> expectedResult = new List<string>() { "Value1", "Value2", "Value3" };
            
            List<string> actualResult = inputStr.CsvStrToList();
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void StringExtensions_DelimitedStringToIntArray_Test()
        {
            string inputStr = "1,2,3";
            int[] expectedResult = new int[] { 1, 2, 3 };

            int[] actualResult = inputStr.DelimitedStringToIntArray(",");
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void StringExtensions_FindSubStrings_Test()
        {
            string inputStr = "Value1,Value2,Value3";
            List<string> expectedResult = new List<string>() { "1", "2" };

            List<string> actualResult = inputStr.FindSubStrings("Value", ",");
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void StringExtensions_ToStream_Test()
        {
            MemoryStream newStream = testStr.ToStream(true);
            try
            {
                Assert.True(newStream.CanRead);
            }
            catch
            {
                Assert.True(false);
            }
            finally
            {
                if(newStream != null)
                {
                    newStream.Dispose();
                }
            }
        }
        #endregion

        #region -- modifiers -----
        [Theory]
        [InlineData("This string has \\\" escaped \\\" quotes", "This string has \" escaped \" quotes")]
        public void StringExtensions_RemoveQuoteEscapes_Test(string inputStr, string expectedResult)
        {
            string actualResult = inputStr.RemoveQuoteEscapes();
            Assert.Equal(expectedResult, actualResult);
        }
        
        [Theory]
        [InlineData("\"This string has \\\" escaped \\\" quotes and encapsulating quotes\"", "This string has \\\" escaped \\\" quotes and encapsulating quotes")]
        [InlineData("This string has \\\" escaped \\\" quotes and encapsulating quotes\"", "This string has \" escaped \" quotes and encapsulating quotes\"")]
        public void StringExtensions_RemoveEncapsulatingQuotes_Test(string inputStr, string expectedResult)
        {
            string actualResult = inputStr.RemoveEncapsulatingQuotes();
            Assert.Equal(expectedResult, actualResult);
        }
        
        [Theory]
        [InlineData("Brown", "green", StringComparison.InvariantCultureIgnoreCase, "The quick green fox jumped over the lazy green dog.")]
        [InlineData("Brown", "green", StringComparison.InvariantCulture, "The quick green fox jumped over the lazy brown dog.")]
        [InlineData("Gray", "green", StringComparison.InvariantCulture, "The quick Brown fox jumped over the lazy brown dog.")]
        public void StringExtensions_Replace_Test(string oldStr, string newStr, StringComparison comp, string expectedResult)
        {
            string actualResult = testStr.Replace(oldStr, newStr, comp);
            Assert.Equal(expectedResult, actualResult);
        }
        
        [Theory]
        [InlineData("Brown", StringComparison.CurrentCulture, "The quick  fox jumped over the lazy brown dog.")]
        [InlineData("brown", StringComparison.CurrentCultureIgnoreCase, "The quick  fox jumped over the lazy brown dog.")]
        [InlineData("green", StringComparison.InvariantCulture, "The quick Brown fox jumped over the lazy brown dog.")]
        public void StringExtensions_Remove_Test(string strToRemove, StringComparison comp, string expectedResult)
        {
            string actualResult = testStr.Remove(strToRemove, comp);
            Assert.Equal(expectedResult, actualResult);
        }
        
        [Theory]
        [InlineData("TestString", "{{TestString}}")]
        public void StringExtensions_AddBraces_Test(string inputStr, string expectedResult)
        {
            string actualResult = inputStr.AddBraces();
            Assert.Equal(expectedResult, actualResult);
        }
        #endregion

        #region -- Encoders -----
        [Theory]
        [InlineData("https%3a%2f%2fwww.mysite.com%2fmyfolder%2fmypage%3fitem1%3dvalue1%26item2%3d%7bvalue2%7d", "https://www.mysite.com/myfolder/mypage?item1=value1&item2={value2}")]
        [InlineData("http%3a%2f%2fbing.com", "http://bing.com")]
        public void StringExtensions_UrlDecode_Test(string inputStr, string expectedResult)
        {
            string actualResult = inputStr.UrlDecode();
            Assert.Equal(expectedResult, actualResult);
        }
        
        [Theory]
        [InlineData("This is a test of the plain old Base64Encoder", "VGhpcyBpcyBhIHRlc3Qgb2YgdGhlIHBsYWluIG9sZCBCYXNlNjRFbmNvZGVy")]
        public void StringExtensions_Base64Encode_Test(string inputStr, string expectedResult)
        {
            string actualResult = inputStr.Base64Encode();
            Assert.Equal(expectedResult, actualResult);
        }
        
        [Theory]
        [InlineData("VGhpcyBpcyBhIHRlc3Qgb2YgdGhlIHBsYWluIG9sZCBCYXNlNjRFbmNvZGVy", "This is a test of the plain old Base64Encoder")]
        public void StringExtensions_Base64Decode_Test(string inputStr, string expectedResult)
        {
            string actualResult = inputStr.Base64Decode();
            Assert.Equal(expectedResult, actualResult);
        }
        #endregion
    }

}
