using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FileParser
{
    public class AsciiFileParser
    {
        private CancellationTokenSource _cts;

        private ConcurrentDictionary<string, int> _wordDictionary;

        public bool IsParsing { get; set; }

        public ConcurrentDictionary<string, int> ParseFile(string filePath, IProgressReporter reporter)
        {
            _cts = new CancellationTokenSource();
            _wordDictionary = new ConcurrentDictionary<string, int>();

            IsParsing = true;

            reporter.ReportProgress(-1);

            var fileLines = SplitFileIntoLines(filePath);
            var numOfLines = fileLines.Count();

            reporter.ResetProgress();

            var po = new ParallelOptions
            {
                CancellationToken = _cts.Token,
                MaxDegreeOfParallelism = Environment.ProcessorCount
            };

            var lineNumber = 0;

            try
            {
                Parallel.ForEach(fileLines, po, i =>
                {
                    if (po.CancellationToken.IsCancellationRequested)
                        return;

                    GetLineWordCount(i);

                    Interlocked.Increment(ref lineNumber);

                    reporter.ReportProgress(lineNumber, numOfLines);
                });
            }
            catch (OperationCanceledException e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                _cts.Dispose();
            }

            IsParsing = false;

            return _wordDictionary;
        }

        private IEnumerable<string> SplitFileIntoLines(string filePath)
        {
            return File.ReadLines(filePath, Encoding.ASCII);
        }

        private void GetLineWordCount(string line)
        {
            var words = line.Split();

            foreach (var word in words)
                if (!string.IsNullOrEmpty(word.Trim()))
                    _wordDictionary.AddOrUpdate(word, 1, (key, value) => value + 1);
        }

        public void AbortProcessing()
        {
            _cts.Cancel();
        }
    }
}