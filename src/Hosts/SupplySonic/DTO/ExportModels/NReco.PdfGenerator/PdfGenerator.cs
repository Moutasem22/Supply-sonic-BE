using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace NReco.PdfGenerator
{
    public class HtmlToPdfConverter
    {
        private Process WkHtmlToPdfProcess;

        private bool batchMode;

        private const string headerFooterHtmlTpl = "<!DOCTYPE html><html><head>\r\n<meta http-equiv=\"content-type\" content=\"text/html; charset=utf-8\" />\r\n<script>\r\nfunction subst() {{\r\n    var vars={{}};\r\n    var x=document.location.search.substring(1).split('&');\r\n\r\n    for(var i in x) {{var z=x[i].split('=',2);vars[z[0]] = unescape(z[1]);}}\r\n    var x=['frompage','topage','page','webpage','section','subsection','subsubsection'];\r\n    for(var i in x) {{\r\n      var y = document.getElementsByClassName(x[i]);\r\n      for(var j=0; j<y.length; ++j) y[j].textContent = vars[x[i]];\r\n    }}\r\n}}\r\n</script></head><body style=\"border:0; margin: 0;\" onload=\"subst()\">{0}</body></html>\r\n";

        private static object globalObj;

        private static string[] ignoreWkHtmlToPdfErrLines;

        public string CustomWkHtmlArgs
        {
            get;
            set;
        }

        public string CustomWkHtmlCoverArgs
        {
            get;
            set;
        }

        public string CustomWkHtmlPageArgs
        {
            get;
            set;
        }

        public string CustomWkHtmlTocArgs
        {
            get;
            set;
        }

        public TimeSpan? ExecutionTimeout
        {
            get;
            set;
        }

        public bool GenerateToc
        {
            get;
            set;
        }

        public bool Grayscale
        {
            get;
            set;
        }

        public LicenseInfo License
        {
            get;
            private set;
        }

        public bool LowQuality
        {
            get;
            set;
        }

        public PageMargins Margins
        {
            get;
            set;
        }

        public PageOrientation Orientation
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

        public float? PageHeight
        {
            get;
            set;
        }

        public float? PageWidth
        {
            get;
            set;
        }

        public string PdfToolPath
        {
            get;
            set;
        }

        public ProcessPriorityClass ProcessPriority
        {
            get;
            set;
        }

        public IntPtr? ProcessProcessorAffinity
        {
            get;
            set;
        }

        public bool Quiet
        {
            get;
            set;
        }

        public PageSize Size
        {
            get;
            set;
        }

        public string TempFilesPath
        {
            get;
            set;
        }

        public string TocHeaderText
        {
            get;
            set;
        }

        public string WkHtmlToPdfExeName
        {
            get;
            set;
        }

        public float Zoom
        {
            get;
            set;
        }

        static HtmlToPdfConverter()
        {
            HtmlToPdfConverter.globalObj = new Object();
            HtmlToPdfConverter.ignoreWkHtmlToPdfErrLines = new String[] { "Exit with code 1 due to network error: ContentNotFoundError", "QFont::setPixelSize: Pixel size <= 0", "Exit with code 1 due to network error: ProtocolUnknownError", "Exit with code 1 due to network error: HostNotFoundError", "Exit with code 1 due to network error: ContentOperationNotPermittedError", "Exit with code 1 due to network error: UnknownContentError" };
        }

        public HtmlToPdfConverter()
        {
            this.ProcessPriority = ProcessPriorityClass.Normal;
            this.ProcessProcessorAffinity = null;
            this.License = new LicenseInfo();
            string str = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wkhtmltopdf");
            this.PdfToolPath = str;
            this.TempFilesPath = null;
            this.WkHtmlToPdfExeName = "wkhtmltopdf.exe";
            this.Orientation = PageOrientation.Default;
            this.Size = PageSize.Default;
            this.LowQuality = false;
            this.Grayscale = false;
            this.Quiet = true;
            this.Zoom = 1f;
            this.Margins = new PageMargins();
        }

        public void BeginBatch()
        {
            if (this.batchMode)
            {
                throw new InvalidOperationException("HtmlToPdfConverter is already in the batch mode.");
            }
            this.batchMode = true;
            this.EnsureWkHtmlLibs();
        }

        private void CheckExitCode(int exitCode, string lastErrLine, bool outputNotEmpty)
        {
            if (exitCode == 0)
            {
                return;
            }
            if (!((exitCode != 1 ? false : Array.IndexOf<string>(HtmlToPdfConverter.ignoreWkHtmlToPdfErrLines, lastErrLine.Trim()) >= 0) & outputNotEmpty))
            {
                throw new WkHtmlToPdfException(exitCode, lastErrLine);
            }
        }

        private void CheckWkHtmlProcess()
        {
            if (!this.batchMode && this.WkHtmlToPdfProcess != null)
            {
                throw new InvalidOperationException("WkHtmlToPdf process is already started");
            }
        }

        private string ComposeArgs(HtmlToPdfConverter.PdfSettings pdfSettings)
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (this.Quiet)
            {
                stringBuilder.Append(" -q ");
            }
            if (this.Orientation != PageOrientation.Default)
            {
                PageOrientation orientation = this.Orientation;
                stringBuilder.AppendFormat(" -O {0} ", orientation.ToString());
            }
            if (this.Size != PageSize.Default)
            {
                PageSize size = this.Size;
                stringBuilder.AppendFormat(" -s {0} ", size.ToString());
            }
            if (this.LowQuality)
            {
                stringBuilder.Append(" -l ");
            }
            if (this.Grayscale)
            {
                stringBuilder.Append(" -g ");
            }
            if (this.Margins != null)
            {
                if (this.Margins.Top.HasValue)
                {
                    stringBuilder.AppendFormat(CultureInfo.InvariantCulture, " -T {0}", this.Margins.Top);
                }
                if (this.Margins.Bottom.HasValue)
                {
                    stringBuilder.AppendFormat(CultureInfo.InvariantCulture, " -B {0}", this.Margins.Bottom);
                }
                if (this.Margins.Left.HasValue)
                {
                    stringBuilder.AppendFormat(CultureInfo.InvariantCulture, " -L {0}", this.Margins.Left);
                }
                if (this.Margins.Right.HasValue)
                {
                    stringBuilder.AppendFormat(CultureInfo.InvariantCulture, " -R {0}", this.Margins.Right);
                }
            }
            if (this.PageWidth.HasValue)
            {
                stringBuilder.AppendFormat(CultureInfo.InvariantCulture, " --page-width {0}", this.PageWidth);
            }
            if (this.PageHeight.HasValue)
            {
                stringBuilder.AppendFormat(CultureInfo.InvariantCulture, " --page-height {0}", this.PageHeight);
            }
            if (pdfSettings.HeaderFilePath != null)
            {
                stringBuilder.AppendFormat(" --header-html \"{0}\"", pdfSettings.HeaderFilePath);
            }
            if (pdfSettings.FooterFilePath != null)
            {
                stringBuilder.AppendFormat(" --footer-html \"{0}\"", pdfSettings.FooterFilePath);
            }
            if (!String.IsNullOrEmpty(this.CustomWkHtmlArgs))
            {
                stringBuilder.AppendFormat(" {0} ", this.CustomWkHtmlArgs);
            }
            if (pdfSettings.CoverFilePath != null)
            {
                stringBuilder.AppendFormat(" cover \"{0}\" ", pdfSettings.CoverFilePath);
                if (!String.IsNullOrEmpty(this.CustomWkHtmlCoverArgs))
                {
                    stringBuilder.AppendFormat(" {0} ", this.CustomWkHtmlCoverArgs);
                }
            }
            if (this.GenerateToc)
            {
                stringBuilder.Append(" toc ");
                if (!String.IsNullOrEmpty(this.TocHeaderText))
                {
                    stringBuilder.AppendFormat(" --toc-header-text \"{0}\"", this.TocHeaderText.Replace("\"", "\\\""));
                }
                if (!String.IsNullOrEmpty(this.CustomWkHtmlTocArgs))
                {
                    stringBuilder.AppendFormat(" {0} ", this.CustomWkHtmlTocArgs);
                }
            }
            WkHtmlInput[] inputFiles = pdfSettings.InputFiles;
            for (int i = 0; i < (int)inputFiles.Length; i++)
            {
                WkHtmlInput wkHtmlInput = inputFiles[i];
                stringBuilder.AppendFormat(" \"{0}\" ", wkHtmlInput.Input);
                string customWkHtmlPageArgs = wkHtmlInput.CustomWkHtmlPageArgs ?? this.CustomWkHtmlPageArgs;
                if (!String.IsNullOrEmpty(customWkHtmlPageArgs))
                {
                    stringBuilder.AppendFormat(" {0} ", customWkHtmlPageArgs);
                }
                if (wkHtmlInput.HeaderFilePath != null)
                {
                    stringBuilder.AppendFormat(" --header-html \"{0}\"", wkHtmlInput.HeaderFilePath);
                }
                if (wkHtmlInput.FooterFilePath != null)
                {
                    stringBuilder.AppendFormat(" --footer-html \"{0}\"", wkHtmlInput.FooterFilePath);
                }
                if (this.Zoom != 1f)
                {
                    stringBuilder.AppendFormat(CultureInfo.InvariantCulture, " --zoom {0} ", this.Zoom);
                }
            }
            stringBuilder.AppendFormat(" \"{0}\" ", pdfSettings.OutputFile);
            return stringBuilder.ToString();
        }

        private void CopyStream(Stream inputStream, Stream outputStream, int bufSize)
        {
            byte[] numArray = new Byte[bufSize];
            while (true)
            {
                int num = inputStream.Read(numArray, 0, (int)numArray.Length);
                int num1 = num;
                if (num <= 0)
                {
                    break;
                }
                outputStream.Write(numArray, 0, num1);
            }
        }

        private string CreateTempFile(string content, string tempPath, List<string> tempFilesList)
        {
            string str = Path.Combine(tempPath, String.Concat("pdfgen-", Path.GetRandomFileName(), ".html"));
            tempFilesList.Add(str);
            if (content != null)
            {
                File.WriteAllBytes(str, Encoding.UTF8.GetBytes(content));
            }
            return str;
        }

        private void DeleteFileIfExists(string filePath)
        {
            if (filePath != null && File.Exists(filePath))
            {
                try
                {
                    File.Delete(filePath);
                }
                catch
                {
                }
            }
        }

        public void EndBatch()
        {
            if (!this.batchMode)
            {
                throw new InvalidOperationException("HtmlToPdfConverter is not in the batch mode.");
            }
            this.batchMode = false;
            if (this.WkHtmlToPdfProcess != null)
            {
                if (!this.WkHtmlToPdfProcess.HasExited)
                {
                    this.WkHtmlToPdfProcess.StandardInput.Close();
                    this.WkHtmlToPdfProcess.WaitForExit();
                    this.WkHtmlToPdfProcess.Close();
                }
                this.WkHtmlToPdfProcess = null;
            }
        }

        private void EnsureWkHtmlLibs()
        {
            this.License.Check();
        }

        private void EnsureWkHtmlProcessStopped()
        {
            if (this.WkHtmlToPdfProcess == null)
            {
                return;
            }
            if (this.WkHtmlToPdfProcess.HasExited)
            {
                this.WkHtmlToPdfProcess.Close();
                this.WkHtmlToPdfProcess = null;
            }
            else
            {
                try
                {
                    this.WkHtmlToPdfProcess.Kill();
                    this.WkHtmlToPdfProcess.Close();
                    this.WkHtmlToPdfProcess = null;
                }
                catch (Exception exception)
                {
                }
            }
        }

        public byte[] GeneratePdf(string htmlContent)
        {
            return this.GeneratePdf(htmlContent, null);
        }

        public byte[] GeneratePdf(string htmlContent, string coverHtml)
        {
            MemoryStream memoryStream = new MemoryStream();
            this.GeneratePdf(htmlContent, coverHtml, memoryStream);
            return memoryStream.ToArray();
        }

        public void GeneratePdf(string htmlContent, string coverHtml, Stream output)
        {
            if (htmlContent == null)
            {
                throw new ArgumentNullException("htmlContent");
            }
            this.GeneratePdfInternal(null, htmlContent, coverHtml, "-", output);
        }

        public void GeneratePdf(string htmlContent, string coverHtml, string outputPdfFilePath)
        {
            if (htmlContent == null)
            {
                throw new ArgumentNullException("htmlContent");
            }
            this.GeneratePdfInternal(null, htmlContent, coverHtml, outputPdfFilePath, null);
        }

        public byte[] GeneratePdfFromFile(string htmlFilePath, string coverHtml)
        {
            MemoryStream memoryStream = new MemoryStream();
            this.GeneratePdfInternal(new String[] { htmlFilePath }, coverHtml, memoryStream);
            return memoryStream.ToArray();
        }

        public void GeneratePdfFromFile(string htmlFilePath, string coverHtml, Stream output)
        {
            this.GeneratePdfInternal(new String[] { htmlFilePath }, coverHtml, output);
        }

        public void GeneratePdfFromFile(string htmlFilePath, string coverHtml, string outputPdfFilePath)
        {
            if (File.Exists(outputPdfFilePath))
            {
                File.Delete(outputPdfFilePath);
            }
            this.GeneratePdfInternal(new WkHtmlInput[] { new WkHtmlInput(htmlFilePath) }, null, coverHtml, outputPdfFilePath, null);
        }

        public void GeneratePdfFromFiles(string[] htmlFiles, string coverHtml, Stream output)
        {
            this.GeneratePdfInternal(htmlFiles, coverHtml, output);
        }

        public void GeneratePdfFromFiles(string[] htmlFiles, string coverHtml, string outputPdfFilePath)
        {
            this.GeneratePdfFromFiles(this.GetWkHtmlInputFromFiles(htmlFiles), coverHtml, outputPdfFilePath);
        }

        public void GeneratePdfFromFiles(WkHtmlInput[] inputs, string coverHtml, string outputPdfFilePath)
        {
            this.License.Check();
            if (File.Exists(outputPdfFilePath))
            {
                File.Delete(outputPdfFilePath);
            }
            this.GeneratePdfInternal(inputs, null, coverHtml, outputPdfFilePath, null);
        }

        private void GeneratePdfInternal(string[] htmlFiles, string coverHtml, Stream output)
        {
            this.GeneratePdfInternal(this.GetWkHtmlInputFromFiles(htmlFiles), null, coverHtml, "-", output);
        }

        private void GeneratePdfInternal(WkHtmlInput[] htmlFiles, string inputContent, string coverHtml, string outputPdfFilePath, Stream outputStream)
        {
            string str;
            string str1;
            string str2;
            string str3;
            string str4;
            if (!this.batchMode)
            {
                this.EnsureWkHtmlLibs();
            }
            this.License.Check();
            this.CheckWkHtmlProcess();
            string tempPath = this.GetTempPath();
            HtmlToPdfConverter.PdfSettings pdfSetting = new HtmlToPdfConverter.PdfSettings()
            {
                InputFiles = htmlFiles,
                OutputFile = outputPdfFilePath
            };
            List<string> strs = new List<string>();
            HtmlToPdfConverter.PdfSettings pdfSetting1 = pdfSetting;
            if (!String.IsNullOrEmpty(coverHtml))
            {
                str = this.CreateTempFile(coverHtml, tempPath, strs);
            }
            else
            {
                str = null;
            }
            pdfSetting1.CoverFilePath = str;
            HtmlToPdfConverter.PdfSettings pdfSetting2 = pdfSetting;
            if (!String.IsNullOrEmpty(this.PageHeaderHtml))
            {
                str1 = this.CreateTempFile(String.Format("<!DOCTYPE html><html><head>\r\n<meta http-equiv=\"content-type\" content=\"text/html; charset=utf-8\" />\r\n<script>\r\nfunction subst() {{\r\n    var vars={{}};\r\n    var x=document.location.search.substring(1).split('&');\r\n\r\n    for(var i in x) {{var z=x[i].split('=',2);vars[z[0]] = unescape(z[1]);}}\r\n    var x=['frompage','topage','page','webpage','section','subsection','subsubsection'];\r\n    for(var i in x) {{\r\n      var y = document.getElementsByClassName(x[i]);\r\n      for(var j=0; j<y.length; ++j) y[j].textContent = vars[x[i]];\r\n    }}\r\n}}\r\n</script></head><body style=\"border:0; margin: 0;\" onload=\"subst()\">{0}</body></html>\r\n", this.PageHeaderHtml), tempPath, strs);
            }
            else
            {
                str1 = null;
            }
            pdfSetting2.HeaderFilePath = str1;
            HtmlToPdfConverter.PdfSettings pdfSetting3 = pdfSetting;
            if (!String.IsNullOrEmpty(this.PageFooterHtml))
            {
                str2 = this.CreateTempFile(String.Format("<!DOCTYPE html><html><head>\r\n<meta http-equiv=\"content-type\" content=\"text/html; charset=utf-8\" />\r\n<script>\r\nfunction subst() {{\r\n    var vars={{}};\r\n    var x=document.location.search.substring(1).split('&');\r\n\r\n    for(var i in x) {{var z=x[i].split('=',2);vars[z[0]] = unescape(z[1]);}}\r\n    var x=['frompage','topage','page','webpage','section','subsection','subsubsection'];\r\n    for(var i in x) {{\r\n      var y = document.getElementsByClassName(x[i]);\r\n      for(var j=0; j<y.length; ++j) y[j].textContent = vars[x[i]];\r\n    }}\r\n}}\r\n</script></head><body style=\"border:0; margin: 0;\" onload=\"subst()\">{0}</body></html>\r\n", this.PageFooterHtml), tempPath, strs);
            }
            else
            {
                str2 = null;
            }
            pdfSetting3.FooterFilePath = str2;
            if (pdfSetting.InputFiles != null)
            {
                WkHtmlInput[] inputFiles = pdfSetting.InputFiles;
                for (int i = 0; i < (int)inputFiles.Length; i++)
                {
                    WkHtmlInput wkHtmlInput = inputFiles[i];
                    WkHtmlInput wkHtmlInput1 = wkHtmlInput;
                    if (!String.IsNullOrEmpty(wkHtmlInput.PageHeaderHtml))
                    {
                        str3 = this.CreateTempFile(String.Format("<!DOCTYPE html><html><head>\r\n<meta http-equiv=\"content-type\" content=\"text/html; charset=utf-8\" />\r\n<script>\r\nfunction subst() {{\r\n    var vars={{}};\r\n    var x=document.location.search.substring(1).split('&');\r\n\r\n    for(var i in x) {{var z=x[i].split('=',2);vars[z[0]] = unescape(z[1]);}}\r\n    var x=['frompage','topage','page','webpage','section','subsection','subsubsection'];\r\n    for(var i in x) {{\r\n      var y = document.getElementsByClassName(x[i]);\r\n      for(var j=0; j<y.length; ++j) y[j].textContent = vars[x[i]];\r\n    }}\r\n}}\r\n</script></head><body style=\"border:0; margin: 0;\" onload=\"subst()\">{0}</body></html>\r\n", wkHtmlInput.PageHeaderHtml), tempPath, strs);
                    }
                    else
                    {
                        str3 = null;
                    }
                    wkHtmlInput1.HeaderFilePath = str3;
                    WkHtmlInput wkHtmlInput2 = wkHtmlInput;
                    if (!String.IsNullOrEmpty(wkHtmlInput.PageFooterHtml))
                    {
                        str4 = this.CreateTempFile(String.Format("<!DOCTYPE html><html><head>\r\n<meta http-equiv=\"content-type\" content=\"text/html; charset=utf-8\" />\r\n<script>\r\nfunction subst() {{\r\n    var vars={{}};\r\n    var x=document.location.search.substring(1).split('&');\r\n\r\n    for(var i in x) {{var z=x[i].split('=',2);vars[z[0]] = unescape(z[1]);}}\r\n    var x=['frompage','topage','page','webpage','section','subsection','subsubsection'];\r\n    for(var i in x) {{\r\n      var y = document.getElementsByClassName(x[i]);\r\n      for(var j=0; j<y.length; ++j) y[j].textContent = vars[x[i]];\r\n    }}\r\n}}\r\n</script></head><body style=\"border:0; margin: 0;\" onload=\"subst()\">{0}</body></html>\r\n", wkHtmlInput.PageFooterHtml), tempPath, strs);
                    }
                    else
                    {
                        str4 = null;
                    }
                    wkHtmlInput2.FooterFilePath = str4;
                }
            }
            try
            {
                try
                {
                    if (inputContent != null)
                    {
                        pdfSetting.InputFiles = new WkHtmlInput[] { new WkHtmlInput(this.CreateTempFile(inputContent, tempPath, strs)) };
                    }
                    if (outputStream != null)
                    {
                        pdfSetting.OutputFile = this.CreateTempFile(null, tempPath, strs);
                    }
                    if (!this.batchMode)
                    {
                        this.InvokeWkHtmlToPdf(pdfSetting, null, null);
                    }
                    else
                    {
                        this.InvokeWkHtmlToPdfInBatch(pdfSetting);
                    }
                    if (outputStream != null)
                    {
                        using (FileStream fileStream = new FileStream(pdfSetting.OutputFile, FileMode.Open, FileAccess.Read, FileShare.Read))
                        {
                            this.CopyStream(fileStream, outputStream, 65536);
                        }
                    }
                }
                catch (Exception exception1)
                {
                    Exception exception = exception1;
                    if (!this.batchMode)
                    {
                        this.EnsureWkHtmlProcessStopped();
                    }
                    throw new Exception(String.Concat("Cannot generate PDF: ", exception.Message), exception);
                }
            }
            finally
            {
                foreach (string str5 in strs)
                {
                    this.DeleteFileIfExists(str5);
                }
            }
        }

        private string GetTempPath()
        {
            if (!String.IsNullOrEmpty(this.TempFilesPath) && !Directory.Exists(this.TempFilesPath))
            {
                Directory.CreateDirectory(this.TempFilesPath);
            }
            return this.TempFilesPath ?? Path.GetTempPath();
        }

        private string GetToolExePath()
        {
            if (String.IsNullOrEmpty(this.PdfToolPath))
            {
                throw new ArgumentException("PdfToolPath property is not initialized with path to wkhtmltopdf binaries");
            }
            string str = Path.Combine(this.PdfToolPath, this.WkHtmlToPdfExeName);
            if (!File.Exists(str))
            {
                throw new FileNotFoundException(String.Concat("Cannot find wkhtmltopdf executable: ", str));
            }
            return str;
        }

        private WkHtmlInput[] GetWkHtmlInputFromFiles(string[] files)
        {
            WkHtmlInput[] wkHtmlInput = new WkHtmlInput[(int)files.Length];
            for (int i = 0; i < (int)wkHtmlInput.Length; i++)
            {
                wkHtmlInput[i] = new WkHtmlInput(files[i]);
            }
            return wkHtmlInput;
        }

        private void InvokeWkHtmlToPdf(HtmlToPdfConverter.PdfSettings pdfSettings, string inputContent, Stream outputStream)
        {
            byte[] bytes;
            string empty = String.Empty;
            DataReceivedEventHandler data = (object o, DataReceivedEventArgs args) => {
                if (args.Data == null)
                {
                    return;
                }
                if (!String.IsNullOrEmpty(args.Data))
                {
                    empty = args.Data;
                }
                if (this.LogReceived != null)
                {
                    this.LogReceived(this, args);
                }
            };
            if (inputContent != null)
            {
                bytes = Encoding.UTF8.GetBytes(inputContent);
            }
            else
            {
                bytes = null;
            }
            byte[] numArray = bytes;
            try
            {
                string str = this.ComposeArgs(pdfSettings);
                ProcessStartInfo processStartInfo = new ProcessStartInfo(this.GetToolExePath(), str)
                {
                    WindowStyle = ProcessWindowStyle.Hidden,
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    WorkingDirectory = Path.GetDirectoryName(this.PdfToolPath),
                    RedirectStandardInput = numArray != null,
                    RedirectStandardOutput = outputStream != null,
                    RedirectStandardError = true
                };
                this.WkHtmlToPdfProcess = Process.Start(processStartInfo);
                if (this.ProcessPriority != ProcessPriorityClass.Normal)
                {
                    this.WkHtmlToPdfProcess.PriorityClass = this.ProcessPriority;
                }
                if (this.ProcessProcessorAffinity.HasValue)
                {
                    this.WkHtmlToPdfProcess.ProcessorAffinity = this.ProcessProcessorAffinity.Value;
                }
                this.WkHtmlToPdfProcess.ErrorDataReceived += data;
                this.WkHtmlToPdfProcess.BeginErrorReadLine();
                if (numArray != null)
                {
                    this.WkHtmlToPdfProcess.StandardInput.BaseStream.Write(numArray, 0, (int)numArray.Length);
                    this.WkHtmlToPdfProcess.StandardInput.BaseStream.Flush();
                    this.WkHtmlToPdfProcess.StandardInput.Close();
                }
                long stream = (long)0;
                if (outputStream != null)
                {
                    stream = (long)this.ReadStdOutToStream(this.WkHtmlToPdfProcess, outputStream);
                }
                this.WaitWkHtmlProcessForExit();
                if (outputStream == null && File.Exists(pdfSettings.OutputFile))
                {
                    stream = (new FileInfo(pdfSettings.OutputFile)).Length;
                }
                this.CheckExitCode(this.WkHtmlToPdfProcess.ExitCode, empty, stream > (long)0);
            }
            finally
            {
                this.EnsureWkHtmlProcessStopped();
            }
        }

        private void InvokeWkHtmlToPdfInBatch(HtmlToPdfConverter.PdfSettings pdfSettings)
        {
            this.License.Check();
            string empty = String.Empty;
            DataReceivedEventHandler data = (object o, DataReceivedEventArgs args) => {
                if (args.Data == null)
                {
                    return;
                }
                if (!String.IsNullOrEmpty(args.Data))
                {
                    empty = args.Data;
                }
                if (this.LogReceived != null)
                {
                    this.LogReceived(this, args);
                }
            };
            if (this.WkHtmlToPdfProcess == null || this.WkHtmlToPdfProcess.HasExited)
            {
                ProcessStartInfo processStartInfo = new ProcessStartInfo(this.GetToolExePath(), "--read-args-from-stdin")
                {
                    WindowStyle = ProcessWindowStyle.Hidden,
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    WorkingDirectory = Path.GetDirectoryName(this.PdfToolPath),
                    RedirectStandardInput = true,
                    RedirectStandardOutput = false,
                    RedirectStandardError = true
                };
                this.WkHtmlToPdfProcess = Process.Start(processStartInfo);
                if (this.ProcessPriority != ProcessPriorityClass.Normal)
                {
                    this.WkHtmlToPdfProcess.PriorityClass = this.ProcessPriority;
                }
                if (this.ProcessProcessorAffinity.HasValue)
                {
                    this.WkHtmlToPdfProcess.ProcessorAffinity = this.ProcessProcessorAffinity.Value;
                }
                this.WkHtmlToPdfProcess.BeginErrorReadLine();
            }
            this.WkHtmlToPdfProcess.ErrorDataReceived += data;
            try
            {
                if (File.Exists(pdfSettings.OutputFile))
                {
                    File.Delete(pdfSettings.OutputFile);
                }
                string str = this.ComposeArgs(pdfSettings).Replace('\\', '/');
                this.WkHtmlToPdfProcess.StandardInput.WriteLine(str);
                bool flag = true;
                while (flag)
                {
                    Thread.Sleep(25);
                    if (this.WkHtmlToPdfProcess.HasExited)
                    {
                        flag = false;
                    }
                    if (!File.Exists(pdfSettings.OutputFile))
                    {
                        continue;
                    }
                    flag = false;
                    this.WaitForFile(pdfSettings.OutputFile);
                }
                if (this.WkHtmlToPdfProcess.HasExited)
                {
                    this.CheckExitCode(this.WkHtmlToPdfProcess.ExitCode, empty, File.Exists(pdfSettings.OutputFile));
                }
            }
            finally
            {
                if (this.WkHtmlToPdfProcess == null || this.WkHtmlToPdfProcess.HasExited)
                {
                    this.EnsureWkHtmlProcessStopped();
                }
                else
                {
                    this.WkHtmlToPdfProcess.ErrorDataReceived -= data;
                }
            }
        }

        private int ReadStdOutToStream(Process proc, Stream outputStream)
        {
            byte[] numArray = new Byte[32768];
            int num = 0;
            while (true)
            {
                int num1 = proc.StandardOutput.BaseStream.Read(numArray, 0, (int)numArray.Length);
                int num2 = num1;
                if (num1 <= 0)
                {
                    break;
                }
                outputStream.Write(numArray, 0, num2);
                num += num2;
            }
            return num;
        }

        private void WaitForFile(string fullPath)
        {
            double num = (!this.ExecutionTimeout.HasValue || !(this.ExecutionTimeout.Value != TimeSpan.Zero) ? 60000 : this.ExecutionTimeout.Value.TotalMilliseconds);
            int num1 = 0;
            while (num > 0)
            {
                num1++;
                num -= 50;
                try
                {
                    using (FileStream fileStream = new FileStream(fullPath, FileMode.Open, FileAccess.ReadWrite, FileShare.None, 100))
                    {
                        fileStream.ReadByte();
                        break;
                    }
                }
                catch (Exception exception)
                {
                    Thread.Sleep((num1 < 10 ? 50 : 100));
                }
            }
            if (num == 0 && this.WkHtmlToPdfProcess != null && !this.WkHtmlToPdfProcess.HasExited)
            {
                this.WkHtmlToPdfProcess.StandardInput.Close();
                this.WkHtmlToPdfProcess.WaitForExit();
            }
        }

        private void WaitWkHtmlProcessForExit()
        {
            if (!this.ExecutionTimeout.HasValue)
            {
                this.WkHtmlToPdfProcess.WaitForExit();
            }
            else if (!this.WkHtmlToPdfProcess.WaitForExit((int)this.ExecutionTimeout.Value.TotalMilliseconds))
            {
                this.EnsureWkHtmlProcessStopped();
                throw new WkHtmlToPdfException(-2, String.Format("WkHtmlToPdf process exceeded execution timeout ({0}) and was aborted", this.ExecutionTimeout));
            }
        }

        public event EventHandler<DataReceivedEventArgs> LogReceived;

        private class PdfSettings
        {
            public string CoverFilePath;

            public string HeaderFilePath;

            public string FooterFilePath;

            public WkHtmlInput[] InputFiles;

            public string OutputFile;

            public PdfSettings()
            {
            }
        }
    }
}