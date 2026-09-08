using System;
using System.Runtime.CompilerServices;

namespace NReco.PdfGenerator
{
    public class WkHtmlInput
    {
        public string CustomWkHtmlPageArgs
        {
            get;
            set;
        }

        internal string FooterFilePath
        {
            get;
            set;
        }

        internal string HeaderFilePath
        {
            get;
            set;
        }

        public string Input
        {
            get;
            set;
        }

        public string PageFooterHtml
        {
            get;
            set;
        }

        public string PageHeaderHtml
        {
            get;
            set;
        }

        public WkHtmlInput(string inputFileOrUrl)
        {
            this.Input = inputFileOrUrl;
        }
    }
}