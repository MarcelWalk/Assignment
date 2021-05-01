using System;
using System.Threading;
using FileParser;

namespace FileParserTests
{
    public class FileProgressReporter : IProgressReporter
    {
        private int _lastProgress;
        private long _readLines;

        public long ReadLines
        {
            get => _readLines;
            set => _readLines = value;
        }

        public void ReportProgress(long currentLine, long numOfLines)
        {
            Interlocked.Increment(ref _readLines);

            var progress = CalculateProgress(currentLine, numOfLines);

            if (progress == _lastProgress) return;

            Console.WriteLine($"[{currentLine + 1}/{numOfLines}] Progress: {progress}%");
        }

        public void ReportProgress(int num)
        {
            Console.WriteLine($"Progress was set to {num}");
        }

        public int CalculateProgress(long currentLine, long numOfLines)
        {
            var onePercent = 100.0f / numOfLines;
            return (int) (onePercent * currentLine);
        }

        public void ResetProgress()
        {
            _lastProgress = 0;
            _readLines = 0;
        }
    }
}