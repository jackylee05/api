using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using Hichain.Common.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;

namespace GLSCM.Common.Util
{
    public class DingdingHelpEx
    {
        /// <summary>
        /// 发送文件
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="filePath"></param>        
        /// <returns></returns>

        public static string SendToFile(string accessToken,string filePath)
        {
            using (var client = new System.Net.Http.HttpClient())
            {
               var tenanttoken= get_tenant_access_token();
                if (string.IsNullOrEmpty(tenanttoken))
                {
                    return "获取DingDingToken异常";
                }
                var mediaid = UploadFile(tenanttoken, filePath);
                if (string.IsNullOrEmpty(mediaid))
                {
                    return "获取Mediaid异常";
                }
                var message = new
                {
                    msgtype = "file",
                    file = new
                    {
                        mediaId = mediaid,
                        fileName = Path.GetFileName(filePath),
                        fileType = Path.GetExtension(filePath).Substring(1)
                    }                    
                };
                var content = new StringContent(JsonConvert.SerializeObject(message), Encoding.UTF8, "application/json");
                var response = client.PostAsync($"https://oapi.dingtalk.com/robot/send?access_token={accessToken}", content).Result;
                response.EnsureSuccessStatusCode();
                return response.Content.ReadAsStringAsync().Result;               
            }
        }
        /// <summary>
        /// 发送文件
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="filePath"></param>        
        /// <returns></returns>

        public static string SendToPicture(string accessToken, string filePath)
        {
            using (var client = new System.Net.Http.HttpClient())
            {
                var tenanttoken = get_tenant_access_token();
                if (string.IsNullOrEmpty(tenanttoken))
                {
                    return "获取DingDingToken异常";
                }
                var mediaId = UploadFile(tenanttoken, filePath);
                if (string.IsNullOrEmpty(mediaId))
                {
                    return "获取Mediaid异常";
                }
                var message = new
                {
                    msgtype = "image",
                    image = new
                    {
                        picURL = mediaId
                    }
                };
                var content = new StringContent(JsonConvert.SerializeObject(message), Encoding.UTF8, "application/json");
                var response = client.PostAsync($"https://oapi.dingtalk.com/robot/send?access_token={accessToken}", content).Result;
                response.EnsureSuccessStatusCode();
                var r= response.Content.ReadAsStringAsync().Result;
                return r;
            }
        }
        ///<summary>
        /// 发送文本消息
        /// </summary>
        /// <param name="content">文本内容</param>
        /// <param name="atMobiles">@人员电话</param>
        /// <param name="isAtAll">是否@群所有成员</param>
        /// <param name="accessToken">accessToken</param>
        public static string SendoToMessage(string content, List<string> atMobiles,bool isAtAll, string accessToken)
        {
            TextModel tModel = new TextModel();
            tModel.at = new atText();
            tModel.text = new text();
            tModel.at.atMobiles = new List<string>();
            string url = $"https://oapi.dingtalk.com/robot/send?access_token={accessToken}";
            tModel.text.content = content;
            tModel.at.atMobiles.AddRange(atMobiles);
            tModel.at.isAtAll = isAtAll;
            tModel.msgtype = "text";
            string data = JsonConvert.SerializeObject(tModel);
            string json = Request(url, data, "POST");
            return json;
        }
        /// <summary>
        /// 发起请求
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="data">数据</param>
        /// <param name="reqtype">请求类型</param>
        /// <returns></returns>
        public static String Request(string url, string data, string reqtype)
        {
            HttpWebRequest web = (HttpWebRequest)HttpWebRequest.Create(url);
            web.ContentType = "application/json";
            web.Method = reqtype;
            if (data.Length > 0 && reqtype.Trim().ToUpper() == "POST")
            {
                byte[] postBytes = Encoding.UTF8.GetBytes(data);
                web.ContentLength = postBytes.Length;
                using (Stream reqStream = web.GetRequestStream())
                {
                    reqStream.Write(postBytes, 0, postBytes.Length);
                }
            }
            string html = string.Empty;
            using (HttpWebResponse response = (HttpWebResponse)web.GetResponse())
            {
                Stream responseStream = response.GetResponseStream();
                StreamReader streamReader = new StreamReader(responseStream, Encoding.UTF8);
                html = streamReader.ReadToEnd();
            }
            return html;
        }
        public static string get_tenant_access_token()
        {   
            var appKey = "ding5kjbmp68c4imc1ca";
            var appSecret = "lUgVZO4i4zEfCG6f1YLjU6vdaRyiQlutr7rj0jLJEb7TlIMXlforZIXUSqcqAxlW";
            var options = new RestClientOptions("https://api.dingtalk.com")
            {
                Timeout = System.Threading.Timeout.InfiniteTimeSpan,
            };
            var client = new RestClient(options);
            var request = new RestRequest("/v1.0/oauth2/accessToken", Method.Post);
            request.AddHeader("Content-Type", "application/json");
            var body = @"{
                        " + "\n" +
                                    @"  ""appKey"":""@appKey"",
                        " + "\n" +
                                    @"  ""appSecret"":""@appSecret""
                        " + "\n" +
                                    @"}";
            request.AddStringBody(body.Replace("@appKey", appKey).Replace("@appSecret", appSecret), DataFormat.Json);
            RestResponse<DingdingToken> response = client.Execute<DingdingToken>(request);

            if (response.Data != null && response.Data.expireIn > 0)
            {
                return response.Data.accessToken;
            }
            return "";
        }
        public static string UploadFile(string accessToken, string filePath)
        {
            string filekey = "";
            if (!System.IO.File.Exists(filePath)) return "";
            var options = new RestClientOptions("https://oapi.dingtalk.com")
            {
                Timeout = System.Threading.Timeout.InfiniteTimeSpan,
            };

            var client = new RestClient(options);
            var request = new RestRequest("/media/upload?access_token=" + accessToken, Method.Post);
            request.AlwaysMultipartFormData = true;
            request.AddParameter("type", "file");
            request.AddFile("media", filePath);
            RestResponse response = client.Execute(request);
            JObject jo = (JObject)JsonConvert.DeserializeObject(response.Content);
            if (response.IsSuccessful)
            {
                filekey = jo["media_id"].ToString();
            }
            return filekey;
        }
        public class TextModel
        {
            /// <summary>
            /// 此消息类型为固定text
            /// </summary>
            public string msgtype { get; set; }
            /// <summary>
            /// 消息内容
            /// </summary>
            public text text { get; set; }
            /// <summary>
            /// @人
            /// </summary>
            public atText at { get; set; }
        }
        /// <summary>
        /// @人
        /// </summary
        public class atText
        {
            /// <summary>
            /// 被@人的手机号
            /// </summary>
            public List<string> atMobiles { get; set; }
            /// <summary>
            /// @所有人时:true,否则为:false
            /// </summary>
            public bool isAtAll { get; set; }
        }

        /// <summary>
        /// 消息内容
        /// </summary>
        public class text
        {
            /// <summary>
            /// 消息内容
            /// </summary>
            public string content { get; set; }
        }
    }   
}
