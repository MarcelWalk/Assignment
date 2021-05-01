using System.Collections.Generic;
using System.IO;
using System.Linq;
using FileParserTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FileParser.Tests
{
    [TestClass]
    public class AsciiFileParserTests
    {
        private readonly Dictionary<string, int> ShortSampleResultDictionary = new Dictionary<string, int>
        {
            {"1:1", 1},
            {"Adam", 2},
            {"Seth", 2},
            {"Enos", 1},
            {"1:2", 1},
            {"Cainan", 1},
            {"Iared", 1}
        };

        [TestMethod]
        public void ParseFileTest()
        {
            var parser = new AsciiFileParser();
            var resultDictionary = parser.ParseFile(
                "./TestFiles/ShortSampleFile.txt",
                new FileProgressReporter()
            );

            var testResult = Helper.CompareDictionaries(ShortSampleResultDictionary,
                resultDictionary.ToDictionary(entry => entry.Key,
                    entry => entry.Value));

            Assert.IsTrue(testResult.Key, testResult.Value);
        }

        [TestMethod]
        public void ReportProgressTest()
        {
            var fileContent = "";
            var fileName = "./TestFiles/GeneratedSampleLong.txt";
            var amount = 100000;

            for (long i = 0; i < amount; i++) fileContent += i + "\n";

            File.WriteAllText(fileName, fileContent);

            var reporter = new FileProgressReporter();

            var parser = new AsciiFileParser();
            var resFile = parser.ParseFile(
                fileName,
                reporter
            );

            Assert.AreEqual(amount, reporter.ReadLines);
        }
    }
}