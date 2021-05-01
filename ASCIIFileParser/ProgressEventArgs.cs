using System;

namespace FileParser
{
    public class ProgressEventArgs : EventArgs
    {
        public long Progress { get; set; }
    }
}
