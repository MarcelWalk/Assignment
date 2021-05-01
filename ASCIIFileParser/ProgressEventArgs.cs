using System;

namespace FileParser
{
    public class ProgressEventArgs : EventArgs
    {
        /// <summary>
        ///     Contains the current progress in percent
        /// </summary>
        public int Progress { get; set; }
    }
}