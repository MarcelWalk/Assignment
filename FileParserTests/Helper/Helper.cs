using System;
using System.Collections.Generic;
using System.Text;

namespace FileParserTests
{
    public class Helper
    {
        public static KeyValuePair<bool, string> CompareDictionaries(Dictionary<string, int> referenceDictioanry,
            Dictionary<string, int> sourceDictionary)
        {
            if (sourceDictionary.Count != referenceDictioanry.Count)
                return new KeyValuePair<bool, string>(false, 
                    $"Dictionaries have a different size.\nExpected: {referenceDictioanry.Count}, Is: {sourceDictionary.Count}");

            foreach (var kvpRef in referenceDictioanry)
            {
                try
                {
                    if (kvpRef.Value != sourceDictionary[kvpRef.Key])
                        return new KeyValuePair<bool, string>(false,
                            $"Dictionaries differ at {kvpRef.Key}.\nExpected: {kvpRef.Value}, Is: {sourceDictionary[kvpRef.Key]}");
                }
                catch (KeyNotFoundException e)
                {
                    return new KeyValuePair<bool, string>(false,
                        $"Missing key in source: {kvpRef.Key}");
                }
                catch (Exception e)
                {
                    return new KeyValuePair<bool, string>(false,
                        $"Unexpected error:\n{e.Message}");
                }
            }
            
            return new KeyValuePair<bool, string>(true, "Success");
        }
    }
}
