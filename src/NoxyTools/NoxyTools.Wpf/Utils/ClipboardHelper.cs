using System;
using System.Text;
using System.Windows;

namespace NoxyTools.Wpf.Utils
{
    /// <summary>
    /// Helper to encode and set HTML fragment to clipboard.<br/>
    /// See http://theartofdev.com/2014/06/12/setting-htmltext-to-clipboard-revisited/.<br/>
    /// <seealso cref="CreateDataObject"/>.
    /// </summary>
    /// <remarks>
    /// The MIT License (MIT) Copyright (c) 2014 Arthur Teplitzki.
    /// </remarks>
    public static class ClipboardHelper
    {
        #region Fields and Consts

        private const string Header = @"Version:0.9
StartHTML:<<<<<<<<1
EndHTML:<<<<<<<<2
StartFragment:<<<<<<<<3
EndFragment:<<<<<<<<4
StartSelection:<<<<<<<<3
EndSelection:<<<<<<<<4";

        public const string StartFragment = "<!--StartFragment-->";
        public const string EndFragment = @"<!--EndFragment-->";

        private static readonly char[] _byteCount = new char[1];

        #endregion

        /// <summary>
        /// Create <see cref="DataObject"/> with given html and plain-text ready to be used for clipboard or drag and drop.
        /// </summary>
        public static DataObject CreateDataObject(string html, string plainText)
        {
            html = html ?? string.Empty;
            var htmlFragment = GetHtmlDataString(html);

            // re-encode the string so it will work correctly (fixed in CLR 4.0)
            if (Environment.Version.Major < 4 && html.Length != Encoding.UTF8.GetByteCount(html))
                htmlFragment = Encoding.Default.GetString(Encoding.UTF8.GetBytes(htmlFragment));

            var dataObject = new DataObject();
            dataObject.SetData(DataFormats.Html, htmlFragment);
            dataObject.SetData(DataFormats.Text, plainText);
            dataObject.SetData(DataFormats.UnicodeText, plainText);
            return dataObject;
        }

        /// <summary>
        /// Clears clipboard and sets the given HTML and plain text fragment to the clipboard.
        /// </summary>
        public static void CopyToClipboard(string html, string plainText)
        {
            var dataObject = CreateDataObject(html, plainText);
            Clipboard.SetDataObject(dataObject, true);
        }

        private static string GetHtmlDataString(string html)
        {
            var sb = new StringBuilder();
            sb.AppendLine(Header);
            sb.AppendLine(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");

            int fragmentStart, fragmentEnd;
            int fragmentStartIdx = html.IndexOf(StartFragment, StringComparison.OrdinalIgnoreCase);
            int fragmentEndIdx = html.LastIndexOf(EndFragment, StringComparison.OrdinalIgnoreCase);

            int htmlOpenIdx = html.IndexOf("<html", StringComparison.OrdinalIgnoreCase);
            int htmlOpenEndIdx = htmlOpenIdx > -1 ? html.IndexOf('>', htmlOpenIdx) + 1 : -1;
            int htmlCloseIdx = html.LastIndexOf("</html", StringComparison.OrdinalIgnoreCase);

            if (fragmentStartIdx < 0 && fragmentEndIdx < 0)
            {
                int bodyOpenIdx = html.IndexOf("<body", StringComparison.OrdinalIgnoreCase);
                int bodyOpenEndIdx = bodyOpenIdx > -1 ? html.IndexOf('>', bodyOpenIdx) + 1 : -1;

                if (htmlOpenEndIdx < 0 && bodyOpenEndIdx < 0)
                {
                    sb.Append("<html><body>");
                    sb.Append(StartFragment);
                    fragmentStart = GetByteCount(sb);
                    sb.Append(html);
                    fragmentEnd = GetByteCount(sb);
                    sb.Append(EndFragment);
                    sb.Append("</body></html>");
                }
                else
                {
                    int bodyCloseIdx = html.LastIndexOf("</body", StringComparison.OrdinalIgnoreCase);

                    if (htmlOpenEndIdx < 0)
                        sb.Append("<html>");
                    else
                        sb.Append(html, 0, htmlOpenEndIdx);

                    if (bodyOpenEndIdx > -1)
                        sb.Append(html, htmlOpenEndIdx > -1 ? htmlOpenEndIdx : 0, bodyOpenEndIdx - (htmlOpenEndIdx > -1 ? htmlOpenEndIdx : 0));

                    sb.Append(StartFragment);
                    fragmentStart = GetByteCount(sb);

                    var innerHtmlStart = bodyOpenEndIdx > -1 ? bodyOpenEndIdx : (htmlOpenEndIdx > -1 ? htmlOpenEndIdx : 0);
                    var innerHtmlEnd = bodyCloseIdx > -1 ? bodyCloseIdx : (htmlCloseIdx > -1 ? htmlCloseIdx : html.Length);
                    sb.Append(html, innerHtmlStart, innerHtmlEnd - innerHtmlStart);

                    fragmentEnd = GetByteCount(sb);
                    sb.Append(EndFragment);

                    if (innerHtmlEnd < html.Length)
                        sb.Append(html, innerHtmlEnd, html.Length - innerHtmlEnd);

                    if (htmlCloseIdx < 0)
                        sb.Append("</html>");
                }
            }
            else
            {
                if (htmlOpenEndIdx < 0)
                    sb.Append("<html>");
                int start = GetByteCount(sb);
                sb.Append(html);
                fragmentStart = start + GetByteCount(sb, start, start + fragmentStartIdx) + StartFragment.Length;
                fragmentEnd = start + GetByteCount(sb, start, start + fragmentEndIdx);
                if (htmlCloseIdx < 0)
                    sb.Append("</html>");
            }

            sb.Replace("<<<<<<<<1", Header.Length.ToString("D9"), 0, Header.Length);
            sb.Replace("<<<<<<<<2", GetByteCount(sb).ToString("D9"), 0, Header.Length);
            sb.Replace("<<<<<<<<3", fragmentStart.ToString("D9"), 0, Header.Length);
            sb.Replace("<<<<<<<<4", fragmentEnd.ToString("D9"), 0, Header.Length);

            return sb.ToString();
        }

        private static int GetByteCount(StringBuilder sb, int start = 0, int end = -1)
        {
            int count = 0;
            end = end > -1 ? end : sb.Length;
            for (int i = start; i < end; i++)
            {
                _byteCount[0] = sb[i];
                count += Encoding.UTF8.GetByteCount(_byteCount);
            }
            return count;
        }
    }
}
