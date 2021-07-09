using System;
using Xunit;
using Extensions;

namespace Extensions.Test
{
    public class CharArrayExtensions_Tests
    {
        string testStr = "this is a test";

        [Theory]
        [InlineData('a')]
        [InlineData(' ')]
        public void CharArrayExtensions_Test1(char value)
        {
            char[] testCharArray = testStr.ToCharArray();
            Assert.True(testCharArray.Contains(value));
        }

        [Theory]
        [InlineData('z')]
        public void CharArrayExtensions_Test2(char value)
        {
            char[] testCharArray = testStr.ToCharArray();
            Assert.False(testCharArray.Contains(value));
        }

        [Fact]
        public void CharArrayExtensions_Test3()
        {
            char[] testCharArray = testStr.ToCharArray();
            Assert.Equal(testCharArray.AsString(), testStr);
        }


    }
}
