using System;
using System.Collections.Generic;
using System.Web;
using System.Linq;

namespace Extensions
{
    public static class DictionaryExtensions
    {
        public static void AddSafely(this Dictionary<string, string> source, string sKey, string sValue)
        {
            if (source.ContainsKey(sKey))
                return;
            source.Add(sKey, sValue);
        }

        public static void AddOrUpdateCount(this Dictionary<string, int> source, string item)
        {
            if (source.ContainsKey(item))
            {
                source[item]++;
            }
            else
            {
                source.Add(item, 1);
            }
        }

        public static void AddOrUpdateItems(this Dictionary<string, List<string>> source, string item, string value)
        {
            if (source.ContainsKey(item))
            {
                source[item].Add(value);
            }
            else
            {
                source.Add(item, new List<string>());
                source[item].Add(value);
            }
        }
    }
}
