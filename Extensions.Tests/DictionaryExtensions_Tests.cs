using System;
using Xunit;
using Extensions;
using System.Threading;
using System.Collections.Generic;
using Serilog;
using Xunit.Abstractions;

namespace Extensions.Test
{
    // https://andrewlock.net/creating-strongly-typed-xunit-theory-test-data-with-theorydata/

    [Collection("xUnitHelper Collection")]
    public class DictionaryExtensions_Tests : IClassFixture<xUnitClassFixture>
    {
        xUnitCollectionFixture _collectionFixture;
        xUnitClassFixture _classFixture;

        public DictionaryExtensions_Tests(xUnitClassFixture classFixture, xUnitCollectionFixture collectionFixture, ITestOutputHelper output)
        {
            _collectionFixture = collectionFixture;
            _classFixture = classFixture;
            collectionFixture.ConfigureLogging(output, @"c:\temp\DictionaryExtensions_Testing");
        }

        [Theory]
        [ClassData(typeof(TestDataForAddSafely))]
        public void DictionaryExtensions_AddSafely_Test(Dictionary<string, string> sampleDictionary, string sKey, string sValue)
        {
            // xUnit does not have a check for "call does not throw an exception,"
            // (https://peterdaugaardrasmussen.com/2019/10/27/xunit-how-to-check-if-a-call-does-not-throw-an-exception/)
            // so I made my own check.
            try
            {
                sampleDictionary.AddSafely(sKey, sValue);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("AddSafely did NOT add safely!", ex.ToString());
            }
        }

        [Theory]
        [ClassData(typeof(TestDataForAddOrUpdateCount))]
        public void DictionaryExtensions_AddOrUpdateCount_Test(Dictionary<string, int> sampleDictionary, string sKey, int numEntriesInDictionary, int expectedValueForGivenKey)
        {
            _classFixture.incrementIteration();
            Log.ForContext<DictionaryExtensions_Tests>().Information("Executing iteration {iteration} @{sampleDictionary}", _classFixture.iterationInstance, sampleDictionary);
            
            sampleDictionary.AddOrUpdateCount(sKey);
            Assert.Equal(numEntriesInDictionary, sampleDictionary.Count);
            Assert.Equal(sampleDictionary[sKey], expectedValueForGivenKey);
        }
    }

    #region -- TheoryData -----
    public class TestDataForAddSafely : TheoryData<Dictionary<string, string>, string, string>
    {
        public TestDataForAddSafely()
        {
            Dictionary<string, string> dictionaryOfValues = new Dictionary<string, string>();
            dictionaryOfValues.Add("Key1", "Value1");
            dictionaryOfValues.Add("Key2", "Value2");
            dictionaryOfValues.Add("Key3", "Value3");

            // Dictionary doesn't contain values
            Add(dictionaryOfValues, "Key4", "Value4");

            // Dictionary does contain values
            Add(dictionaryOfValues, "Key2", "Value4");
        }
    }

    /// <summary>
    /// TheoryData Contents
    /// Dictionary<string, int> : The dictionary that will get modified
    /// string : The value to add to (or update) the dictionary.
    /// int : The nuber of items expected in the dictionary after the call
    /// int : The quantity (value part of the KeyValuePair) of the Key after the call.
    /// </summary>
    public class TestDataForAddOrUpdateCount : TheoryData<Dictionary<string, int>, string, int, int>
    {
        public TestDataForAddOrUpdateCount()
        {
            Dictionary<string, int> dictionaryOfValues = new Dictionary<string, int>();
            dictionaryOfValues.Add("Key1", 2);
            dictionaryOfValues.Add("Key2", 2);
            dictionaryOfValues.Add("Key3", 2);

            // Dictionary does contain values
            Add(dictionaryOfValues, "Key2", 3, 3);

            // Dictionary doesn't contain value
            Add(dictionaryOfValues, "Key4", 4, 1);

            // Dictionary does contain values
            Add(dictionaryOfValues, "Key2", 4, 4);
        }
    }

    public class TestDataForAddOrUpdateItems : TheoryData<Dictionary<string, List<string>>, string, string>
    {
        public TestDataForAddOrUpdateItems()
        {
            Dictionary<string, List<string>> dictionaryOfValues = new Dictionary<string, List<string>>();
            dictionaryOfValues.Add("Key1", new List<string>());
            dictionaryOfValues["Key1"].Add("Item1");
            dictionaryOfValues["Key1"].Add("Item2");


            // Dictionary doesn't contain value
            Add(dictionaryOfValues, "Key3", "Item1");

            // Dictionary does contain value
            Add(dictionaryOfValues, "Key1", "Item1");
        }
    }
    #endregion
}
