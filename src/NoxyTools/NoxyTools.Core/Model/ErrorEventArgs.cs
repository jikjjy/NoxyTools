using System;

namespace NoxyTools.Core.Model
{
    public class ErrorEventArgs : EventArgs
    {
        public string Message { get; set; } = string.Empty;
        public Exception Exception { get; set; } = null;
    }
}
