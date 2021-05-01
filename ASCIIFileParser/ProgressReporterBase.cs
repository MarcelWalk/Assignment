using System;

namespace FileParser
{
    public abstract class ProgressReporterBase
    {
        public abstract void ReportProgress(long currentLine, long numOfLines);

        protected virtual void OnProgressMade(ProgressEventArgs e)
        {
            var handler = ProgressMade;
            handler?.Invoke(this, e);
        }

        protected static long CalculateProgress(long currentLine, long numOfLines)
        {
            var onePercent = 100.0f / numOfLines;
            return (int) (onePercent * currentLine);
        }

        public event EventHandler<ProgressEventArgs> ProgressMade;

        public abstract void ResetProgress();
    }
}