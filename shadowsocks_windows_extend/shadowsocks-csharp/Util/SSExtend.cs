using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Shadowsocks.Util
{
    public class SSExtend
    {
        public List<SSItem> LoadSSAccountList(string url)
        {
            string result;
            List<SSItem> list = new List<SSItem>();
            try
            {
                WebRequest request = WebRequest.Create(url.Trim());
                WebResponse response = request.GetResponse();
                Stream stream = response.GetResponseStream();
                StreamReader sReader = new StreamReader(stream, Encoding.UTF8);
                result = sReader.ReadToEnd();
                sReader.Close();
                stream.Close();

                var targetHtml = GetTargetHTML(result, "<section id=\"free\">", "<!-- Provider list Section -->");
                result = GetTargetHTML(targetHtml, "<div class=\"row\">", @"</div>");
                if (result.Contains("更多帐号"))
                {
                    targetHtml = targetHtml.Replace(result, string.Empty);
                    result = GetTargetHTML(targetHtml, "<div class=\"row\">", @"</div>");
                }
                HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
                document.LoadHtml(result);
                HtmlNode rootNode = document.DocumentNode;
                HtmlNodeCollection nodeCollection = rootNode.SelectNodes("/div[1]/div");
                HtmlNode tmpNode = null;

                foreach (HtmlNode node in nodeCollection)
                {
                    tmpNode = HtmlNode.CreateNode(node.OuterHtml);
                    var h4Collection = tmpNode.SelectNodes("/div/h4");                    
                    var server = h4Collection[0].InnerHtml.Split(':')[1] ?? h4Collection[0].InnerHtml;
                    var port = h4Collection[1].InnerHtml.Split(':')[1] ?? h4Collection[1].InnerHtml;
                    var pwd = h4Collection[2].InnerHtml.Split(':')[1] ?? h4Collection[2].InnerHtml;
                    var encryptType = h4Collection[3].InnerHtml.Split(':')[1] ?? h4Collection[3].InnerHtml;
                    var status = h4Collection[4].InnerHtml.Split('<', '>');
                    var serverState = status[2] ?? h4Collection[4].InnerHtml;
                    var notes = h4Collection[5].InnerHtml.Split('<', '>');
                    var notesStr = notes[2] ?? h4Collection[5].InnerHtml;

                    if (!serverState.Contains("正常"))
                        continue;
                    SSItem ssItem = new SSItem()
                    {
                        Server = server,
                        Port = port,
                        Password = pwd,
                        EncryptType = encryptType,
                        ServerState = serverState,
                        Notes = notesStr
                    };
                    list.Add(ssItem);
                }
            }
            catch(Exception e)
            {
                WriteTxtLog(e.Message);
            }
            return list;
        }

        private string GetTargetHTML(string input, string startStr, string endStr)
        {
            var result = string.Empty;
            var startIndex = input.LastIndexOf(startStr);
            var endIndex = input.LastIndexOf(endStr);
            result = input.Substring(startIndex, endIndex - startIndex);
            return result;
        }


        public static void WriteTxtLog(string message)
        {
            var now = DateTime.Now;
            string path = AppDomain.CurrentDomain.BaseDirectory + @"RunningLog\";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string fileFullPath = path + now.ToString("yyyy-MM-dd") + ".Log.txt";
            StringBuilder str = new StringBuilder();
            str.Append("Time:    " + now.ToString() + "\r\n");
            str.Append("Message: " + message + "\r\n");
            str.Append("-----------------------------------------------------------\r\n\r\n");
            StreamWriter sw;
            if (!File.Exists(fileFullPath))
            {
                sw = File.CreateText(fileFullPath);
            }
            else
            {
                sw = File.AppendText(fileFullPath);
            }
            sw.WriteLine(str.ToString());
            sw.Close();
        }
    }

    public class SSItem
    {
        public string Server { get; set; }
        public string Port { get; set; }
        public string Password { get; set; }
        public string EncryptType { get; set; }
        public string ServerState { get; set; }
        public string Notes { get; set; }
    }
}
