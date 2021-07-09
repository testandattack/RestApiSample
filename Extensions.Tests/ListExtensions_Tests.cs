using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Extensions.Test
{
    // https://andrewlock.net/creating-strongly-typed-xunit-theory-test-data-with-theorydata/

    [Collection("xUnitHelper Collection")]
    public class ListExtensions_Tests : IClassFixture<xUnitClassFixture>
    {
        List<string> stringOfValues;
        List<int> intOfValues;

        xUnitCollectionFixture _collectionFixture;
        xUnitClassFixture _classFixture;

        public ListExtensions_Tests(xUnitClassFixture classFixture, xUnitCollectionFixture collectionFixture, ITestOutputHelper output)
        {
            _collectionFixture = collectionFixture;
            _classFixture = classFixture;
            collectionFixture.ConfigureLogging(output, @"c:\temp\ListExtensions_Testing");

            stringOfValues = new List<string>() {"Value1","Value2","Value3"};
            intOfValues = new List<int>() { 1, 2, 3 };
        }

        [Theory]
        [ClassData(typeof(TestDataForAddUnique))]
        public void ListExtensions_AddSafely_Test(List<string> sampleList, string sValue, int iCount)
        {
            sampleList.AddUnique(sValue);
            Assert.Equal(sampleList.Count, iCount);
        }

        [Theory]
        [InlineData("Value1;Value2;Value3", ";", false)]
        [InlineData("Value1; Value2; Value3", ";", true)]
        public void ListExtensions_StringToString_Test(string finalString, string separator, bool addSpace)
        {
            if (addSpace == false)
            {
                // Assert the List extension
                Assert.Equal(stringOfValues.ToString(separator), finalString);
                // Assert the IEnumerable extension
                Assert.Equal(stringOfValues.Where(i => i is string).ToString(separator), finalString);
            }
            else
            {
                // Assert the List extension
                Assert.Equal(stringOfValues.ToString(separator, addSpace), finalString);
                // Assert the IEnumerable extension
                Assert.Equal(stringOfValues.Where(i => i is string).ToString(separator, addSpace), finalString);
            }
        }

        [Theory]
        [InlineData("1;2;3", ";", false)]
        [InlineData("1; 2; 3", ";", true)]
        public void ListExtensions_IntToString_Test(string finalString, string separator, bool addSpace)
        {
            if (addSpace == false)
            {
                // Assert the List extension
                Assert.Equal(intOfValues.ToString(separator), finalString);
                // Assert the IEnumerable extension
                Assert.Equal(intOfValues.Select(i => i).ToString(separator), finalString);
            }
            else
            {
                // Assert the List extension
                Assert.Equal(intOfValues.ToString(separator, addSpace), finalString);
                // Assert the IEnumerable extension
                Assert.Equal(intOfValues.Select(i => i).ToString(separator, addSpace), finalString);
            }
        }
    }

    #region -- TheoryData -----
    /// <summary>
    /// Defines a class that can be used to pass data into the 
    /// test method. use the command "Add()" to add a row of test data.
    /// The shape of the data to add is defined by the class' inheritance
    /// declaration
    /// </summary>
    /// List[string] is the list to test against
    /// string is the item to try to add
    /// int is the number of items in the list after the test call.
    public class TestDataForAddUnique : TheoryData<List<string>, string, int>
    {
        public TestDataForAddUnique()
        {
            List<string> stringOfValues = new List<string>();
            stringOfValues.Add("Value1");
            stringOfValues.Add("Value2");
            stringOfValues.Add("Value3");

            // list doesn't contain value
            Add(stringOfValues, "Value4", 4);

            // string does contain value
            Add(stringOfValues, "Value4", 4);
        }
    }
    #endregion
}
