using System;
using System.Runtime.CompilerServices;

namespace NReco.PdfGenerator
{
    public class WkHtmlToPdfException : Exception
    {
        public int ErrorCode
        {
            get;
            private set;
        }

        public WkHtmlToPdfException(int errCode, string message) : base(String.Format("{0} (exit code: {1})", (object)message, errCode))
        {
            this.ErrorCode = errCode;
        }
    }
}