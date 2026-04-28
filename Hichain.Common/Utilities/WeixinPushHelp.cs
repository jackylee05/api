using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace GLSCM.Common.Util
{
    public class WeixinConfig
    {
        public string token { get; set; }
        public string dingdingtoken { get; set; }
        public WeixinConfig()
        {
            token = "60B0A2CE-2292-48E7-8DE6-4B10530D04FD";
            dingdingtoken = "0159aae55966c00f58735be02a52852ae4f3ba624364f672330afe9ba244b1b5";

        }
        public WeixinConfig(string dingtoken)
        {
            token = "60B0A2CE-2292-48E7-8DE6-4B10530D04FD";
            dingdingtoken = dingtoken;
        }
    }

    public class WeiXinSendMessage
    {
        public string MessageId { get; set; }

        public string sendToName { get; set; }

        public int messageType { get; set; }

        public string messageContent { get; set; }
    }

    public class WeiXinSendPicture
    {
        public string file { get; set; }

        public string SendTo { get; set; }

        public string MessageContent { get; set; }

    }

    public class WeixinPushHelp
    {
        /// <summary>
        /// SendToMessage
        /// </summary>
        /// <param name="cfg"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        //public static RestResponse SendToMessage(WeixinConfig cfg, string message)
        //{
        //    var options = new RestClientOptions("http://139.196.28.158:5559")
        //    {
        //        MaxTimeout = -1,
        //    };
        //    var client = new RestClient(options);
        //    var request = new RestRequest("WeiXin/SendWeiXinMessage", Method.Post);
        //    List<WeiXinSendMessage> list = new List<WeiXinSendMessage>();
        //    WeiXinSendMessage msg = new WeiXinSendMessage();
        //    msg.messageType = 0;
        //    msg.MessageId = Guid.NewGuid().ToString();
        //    msg.sendToName = "新能源开发组";
        //    msg.messageContent = message;
        //    list.Add(msg);
        //    request.AddHeader("Apitoken", cfg.token);
        //    request.AddHeader("Content-Type", "application/json");
        //    string jsonstring = JsonConvert.SerializeObject(list);
        //    request.AddStringBody(jsonstring, DataFormat.Json);
        //    RestResponse response = client.Execute(request);
        //    return response;
        //}        
        public static void SendToMessage(WeixinConfig cfg, string message)
        {
            DingdingHelpEx.SendoToMessage("SEV"+ message, new List<string>() { "13814558705" }, true, cfg.dingdingtoken);
        }
        /// <summary>
        /// SendoToPictureEx
        /// </summary>
        /// <param name="cfg"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static RestResponse SendoToPicture(WeixinConfig cfg, WeiXinSendPicture message)
        {
            var options = new RestClientOptions("http://139.196.28.158:5559")
            {
                MaxTimeout = -1,
            };
            var client = new RestClient(options);
            var request = new RestRequest("/WeiXin/UploadFile", Method.Post);
            request.AddHeader("ApiToken", cfg.token);
            request.AlwaysMultipartFormData = true;
            request.AddFile("file", message.file);
            request.AddParameter("SendTo", message.SendTo);
            request.AddParameter("MessageContent", message.MessageContent);
            RestResponse response = client.Execute(request);
            return response;
        }
        /// <summary>
        /// SendToMessageEx
        /// </summary>
        /// <param name="cfg"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static RestResponse SendToMessage(WeixinConfig cfg, List<WeiXinSendMessage> message)
        {
            var options = new RestClientOptions("http://139.196.28.158:5559")
            {
                MaxTimeout = -1,
            };
            var client = new RestClient(options);
            var request = new RestRequest("/WeiXin/SendWeiXinMessage", Method.Post);
            request.AddHeader("ApiToken", cfg.token);
            request.AddHeader("Content-Type", "application/json");
            request.AddStringBody(JsonConvert.SerializeObject(message), DataFormat.Json);
            RestResponse response = client.Execute(request);
            return response;
        }
    }
}
