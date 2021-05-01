using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FileParser
{
    public class AsciiFileParser
    {
        private readonly ConcurrentDictionary<string, int> _wordDictionary = new ConcurrentDictionary<string, int>();
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();

        public ConcurrentDictionary<string, int> ParseFile(string filePath, ProgressReporterBase reporter)
        {
            reporter.ResetProgress();

            var fileLines = SplitFileIntoLines(filePath);
            var numOfLines = fileLines.Length;

            var po = new ParallelOptions
            {
                CancellationToken = _cts.Token,
                MaxDegreeOfParallelism = Environment.ProcessorCount
            };

            var lineNumber = 0;

            try
            {
                Parallel.For(0, numOfLines, po, i =>
                {
                    if (po.CancellationToken.IsCancellationRequested)
                        return;

                    GetLineWordCount(fileLines[i]);

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

            return _wordDictionary;
        }

        private string[] SplitFileIntoLines(string filePath)
        {
            return File.ReadAllLines(filePath);
        }

        private void GetLineWordCount(string line)
        {
            var words = line.Split();

            foreach (var word in words)
            {
                if (!string.IsNullOrEmpty(word.Trim()))
                    _wordDictionary.AddOrUpdate(word, 1, (key, value) => value + 1);
            }
        }

        public void AbortProcessing()
        {
            _cts.Cancel();
        }
    }
}