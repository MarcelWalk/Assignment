namespace FileParser
{
    public interface IProgressReporter
    {
        void ReportProgress(long currentLine, long numOfLines);

        void ReportProgress(int num);

        int CalculateProgress(long currentLine, long numOfLines);

        void ResetProgress();
    }
}