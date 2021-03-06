﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NeoToMongo
{
    static class neoContractHelper
    {
        public static string getNEP5ContractInfo(string apiUrl, string scripthash, string method)
        {

            string result = string.Empty;
            try
            {
                //JObject postData = new JObject();
                //postData.Add("jsonrpc", "2.0");
                //postData.Add("method", "callcontractfortest");
                //postData.Add("params", JArray.Parse("['" + scripthash + "',['(str)" + method + "',[]]]"));
                //postData.Add("id", 1);
                //string postDataStr = Newtonsoft.Json.JsonConvert.SerializeObject(postData);

                ////json格式post
                //string resNotify = chh.Post(apiUrl, postDataStr, Encoding.UTF8,1);

                var resNotify=Rpc.invokescript(apiUrl,scripthash, method).Result;
                string valueHex = resNotify.AsDict()["stack"].AsList()[0].AsDict()["value"].AsString();
                //string valueHex = (string)JObject.Parse(resNotify)["result"][0]["stack"][0]["value"];

                result = valueHex;
            }
            catch (Exception ex)
            {
                Console.WriteLine("fail to get nep5 contract info.");
                Log.WriteLog("fail to get nep5 contract info.");
            }

            Thread.Sleep(50);//防止过度调用接口导致cli卡死

            return result;
        }

        public static string getStrFromHexstr(string hexStr)
        {
            List<byte> byteArray = new List<byte>();

            for (int i = 0; i < hexStr.Length; i = i + 2)
            {
                string s = hexStr.Substring(i, 2);
                byteArray.Add(Convert.ToByte(s, 16));
            }

            string str = System.Text.Encoding.UTF8.GetString(byteArray.ToArray());

            return str;
        }
    }
}
