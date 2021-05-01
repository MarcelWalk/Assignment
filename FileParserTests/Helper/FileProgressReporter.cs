using System;
using System.Threading;
using FileParser;

namespace FileParserTests
{
    public class FileProgressReporter : ProgressReporterBase
    {
        private float _lastProgress;
        private long _readLines;

        public long ReadLines
        {
            get => _readLines;
            set => _readLines = value;
        }

        public override void ReportProgress(long currentLine, long numOfLines)
        {
            Interlocked.Increment(ref _readLines);

            var progress = CalculateProgress(currentLine, numOfLines);

            if (progress == _lastProgress) return;

            Console.WriteLine($"[{currentLine + 1}/{numOfLines}] Progress: {progress}%");
        }

        public override void ResetProgress()
        {
            _lastProgress = 0;
            _readLines = 0;
        }
    }
}