using System;
using System.IO;
using HtmlAgilityPack;

namespace Proxy
{
    public static class HtmlToText
    {
        public static string Convert(string path)
        {
            var doc = new HtmlDocument();
            doc.Load(path);

            var sw = new StringWriter();
            ConvertTo(doc.DocumentNode, sw);
            sw.Flush();
            return sw.ToString();
        }

        public static string ConvertHtml(string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var sw = new StringWriter();
            ConvertTo(doc.DocumentNode, sw);
            sw.Flush();
            return sw.ToString();
        }

        private static void ConvertContentTo(HtmlNode node, TextWriter outText)
        {
            foreach (var subnode in node.ChildNodes)
            {
                ConvertTo(subnode, outText);
            }
        }

        public static void ConvertTo(HtmlNode node, TextWriter outText)
        {
            switch (node.NodeType)
            {
                case HtmlNodeType.Comment:
                    break;

                case HtmlNodeType.Document:
                    ConvertContentTo(node, outText);
                    break;

                case HtmlNodeType.Text:
                    var parentName = node.ParentNode.Name;
                    if ((parentName == "script") || (parentName == "style"))
                        break;

                    var html = ((HtmlTextNode)node).Text;
                    if (HtmlNode.IsOverlappedClosingElement(html))
                        break;

                    if (html.Trim().Length > 0)
                    {
                        outText.Write(HtmlEntity.DeEntitize(html));
                    }
                    break;

                case HtmlNodeType.Element:
                    if (node.Name == "p")
                    {
                        outText.Write("\r\n");
                    }

                    if (node.HasChildNodes)
                    {
                        ConvertContentTo(node, outText);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

}
