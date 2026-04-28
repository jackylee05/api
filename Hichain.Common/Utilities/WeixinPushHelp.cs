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
        public static void SendToMessage(WeixinConfig cfg, string message)
        {
            DingdingHelpEx.SendoToMessage("SEV"+ message, new List<string>() { "18136139095" }, true, cfg.dingdingtoken);
        }
    }
}
