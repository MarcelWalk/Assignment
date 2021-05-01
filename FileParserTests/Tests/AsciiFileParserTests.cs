using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Assignment.ViewModel;
using FileParserTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FileParser.Tests
{
    [TestClass]
    public class AsciiFileParserTests
    {
        private readonly Dictionary<string, int> _shortSampleResultDictionary = new Dictionary<string, int>
        {
            {"1:1", 1},
            {"Adam", 2},
            {"Seth", 2},
            {"Enos", 1},
            {"1:2", 1},
            {"Cainan", 1},
            {"Iared", 1}
        };

        private readonly Dictionary<string, int> _testDictionary = new Dictionary<string, int>
        {
            {"1", 1},
            {"2", 2},
            {"3", 2},
            {"4", 1},
            {"5", 1},
            {"6", 1},
            {"7", 1}
        };

        [TestMethod]
        public void ParseFileTest()
        {
            var parser = new AsciiFileParser();
            var resultDictionary = parser.ParseFile(
                "./TestFiles/ShortSampleFile.txt",
                new FileProgressReporter()
            );

            var testResult = Helper.CompareDictionaries(_shortSampleResultDictionary,
                resultDictionary.ToDictionary(entry => entry.Key,
                    entry => entry.Value));

            Assert.IsTrue(testResult.Key, testResult.Value);
        }

        [TestMethod]
        public void ParseMultipleFileTest()
        {
            var parser = new AsciiFileParser();

            parser.ParseFile(
                "./TestFiles/ShortSampleFile.txt",
                new FileProgressReporter()
            );

            var resultDictionary = parser.ParseFile(
                "./TestFiles/ShortSampleFile.txt",
                new FileProgressReporter()
            );

            var testResult = Helper.CompareDictionaries(_shortSampleResultDictionary,
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

        [TestMethod]
        public void CancelProcessingTest()
        {
            var vm = new MainWindowViewModel();

            vm.StartParsing("./TestFiles/SampleLong.txt");
            Thread.Sleep(10);
            var progressSnapshot1 = vm.ParsingProgress;
            Thread.Sleep(10);
            vm.CancelParsing(null);
            var progressSnapshot2 = vm.ParsingProgress;

            Assert.AreEqual(progressSnapshot1, progressSnapshot2);
        }

        [TestMethod]
        public void FullHappyPathTest()
        {
            var vm = new MainWindowViewModel();

            Assert.IsFalse(vm.StartAllowed(null));
            
            vm.StartParsing("./TestFiles/ShortSampleFile.txt");

            while (vm.ParsingProgress < 100)
            {
                Thread.Sleep(10);
            }

            var parsingResult = vm.WordCountCollection.ToDictionary(entry => entry.Word,
                entry => entry.Count);

            var testResult = Helper.CompareDictionaries(_shortSampleResultDictionary,
                parsingResult);

            Assert.IsTrue(testResult.Key, testResult.Value);
        }

        [TestMethod]
        public void CompareDictionariesHappyPathTest()
        {
            var compareDictionary = new Dictionary<string, int>
            {
                {"1", 1},
                {"2", 2},
                {"3", 2},
                {"4", 1},
                {"5", 1},
                {"6", 1},
                {"7", 1}
            };

            var testResult = Helper.CompareDictionaries(_testDictionary,
                compareDictionary);

            Assert.IsTrue(testResult.Key, testResult.Value);
        }

        [TestMethod]
        public void CompareDictionariesDifferentValueTest()
        {
            var compareDictionary = new Dictionary<string, int>
            {
                {"1", 1},
                {"2", 2},
                {"3", 2},
                {"4", 999},
                {"5", 1},
                {"6", 1},
                {"7", 1}
            };

            var testResult = Helper.CompareDictionaries(_testDictionary,
                compareDictionary);

            Assert.IsFalse(testResult.Key);
            Assert.AreEqual("Dictionaries differ at 4.\nExpected: 1, Is: 999", testResult.Value);
        }

        [TestMethod]
        public void CompareDictionariesDifferentKeyTest()
        {
            var compareDictionary = new Dictionary<string, int>
            {
                {"1", 1},
                {"2", 2},
                {"3", 2},
                {"999", 1},
                {"5", 1},
                {"6", 1},
                {"7", 1}
            };

            var testResult = Helper.CompareDictionaries(_testDictionary,
                compareDictionary);

            Assert.IsFalse(testResult.Key);
            Assert.AreEqual("Missing key in source: 4", testResult.Value);
        }

        [TestMethod]
        public void CompareDictionariesDifferentSizeTest()
        {
            var compareDictionary = new Dictionary<string, int>
            {
                {"1", 1},
                {"2", 2},
                {"3", 2},
                {"4", 2},
                {"999", 1},
                {"5", 1},
                {"6", 1},
                {"7", 1}
            };

            var testResult = Helper.CompareDictionaries(_testDictionary,
                compareDictionary);

            Assert.IsFalse(testResult.Key);
            Assert.AreEqual("Dictionaries have a different size.\nExpected: 7, Is: 8", testResult.Value);
        }
    }
}