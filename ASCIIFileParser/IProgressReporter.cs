using System;

namespace FileParser
{
    public interface IProgressReporter
    {
        abstract void ReportProgress(long currentLine, long numOfLines);

        abstract void ReportProgress(int num);

        abstract int CalculateProgress(long currentLine, long numOfLines);

        abstract void ResetProgress();
    }
}