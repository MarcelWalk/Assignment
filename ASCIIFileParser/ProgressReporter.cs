namespace FileParser
{
    public class ProgressReporter : ProgressReporterBase
    {
        private long _lastProgress;

        public override void ReportProgress(long currentLine, long numOfLines)
        {
            var progress = CalculateProgress(currentLine, numOfLines);

            if (progress == _lastProgress) return;

            var args = BuildProgressEventArgs(progress);
            OnProgressMade(args);
            _lastProgress = progress;
        }

        public override void ResetProgress()
        {
            var args = BuildProgressEventArgs(0);
            OnProgressMade(args);
        }

        private static ProgressEventArgs BuildProgressEventArgs(long progress)
        {
            return new ProgressEventArgs
            {
                Progress = progress
            };
        }
    }
}