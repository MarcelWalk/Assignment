using System;

namespace FileParser
{
    public class ProgressReporter : IProgressReporter
    {
        private int _lastProgress;

        public void ReportProgress(long currentLine, long numOfLines)
        {
            var progressInPercent = CalculateProgress(currentLine, numOfLines);

            if (progressInPercent == _lastProgress) return;

            ReportProgress(progressInPercent);
        }

        public void ReportProgress(int progress)
        {
            var args = BuildProgressEventArgs(progress);
            OnProgressMade(args);
            _lastProgress = progress;
        }

        public int CalculateProgress(long currentLine, long numOfLines)
        {
            var onePercent = 100.0f / numOfLines;
            return (int) (onePercent * currentLine);
        }

        public void ResetProgress()
        {
            var args = BuildProgressEventArgs(0);
            OnProgressMade(args);
        }

        protected virtual void OnProgressMade(ProgressEventArgs e)
        {
            var handler = ProgressMade;
            handler?.Invoke(this, e);
        }

        public event EventHandler<ProgressEventArgs> ProgressMade;

        private static ProgressEventArgs BuildProgressEventArgs(int progress)
        {
            return new ProgressEventArgs
            {
                Progress = progress
            };
        }
    }
}