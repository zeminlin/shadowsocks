using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace Shadowsocks.Util
{
    public class SSExtend
    {
        /// <summary>
        /// 加载ISS测试账号
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public List<SSItem> LoadISSAccountList(string url)
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

        /// <summary>
        /// 加载IFanQiang测试账号
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public List<SSItem> LoadIFanQiangAccountList(string url)
        {
            string jsonStr;
            List<SSItem> list = new List<SSItem>();
            try
            {
                WebRequest request = WebRequest.Create(url.Trim());
                WebResponse response = request.GetResponse();
                Stream stream = response.GetResponseStream();
                StreamReader sReader = new StreamReader(stream, Encoding.UTF8);
                jsonStr = sReader.ReadToEnd();
                sReader.Close();
                stream.Close();

                jsonStr = jsonStr.TrimStart('[').TrimEnd(']');
                jsonStr = jsonStr.Replace("},{", "}@{");
                string[] jsonArray = jsonStr.Split('@');
                List<Model> modelList = new List<Model>();
                foreach (var json in jsonArray)
                {
                    var jsonModel = JsonConvert.DeserializeObject<Model>(json);
                    if (jsonModel.st)
                    {
                        SSItem ssItem = new SSItem()
                        {
                            Server = jsonModel.i,
                            Port = jsonModel.p,
                            Password = jsonModel.pw,
                            EncryptType = jsonModel.m,
                            ServerState = jsonModel.l,
                            Notes = string.Format("{0}_来源于ifq", jsonModel.r)
                        };
                        list.Add(ssItem);
                    }
                }
            }
            catch (Exception e)
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

    /// <summary>
    /// ifaniang 对象
    /// http://api.ifanqiang.cn/ajax.php?verify=true&mod=getfreess&t=1479829563648
    /// </summary>
    public class Model
    {
        public string i { get; set; }
        public string p { get; set; }
        public string m { get; set; }
        public string pw { get; set; }
        public string r { get; set; }
        public string l { get; set; }
        public bool st { get; set; }
    }
}
