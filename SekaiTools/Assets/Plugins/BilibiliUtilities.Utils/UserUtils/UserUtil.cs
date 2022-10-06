using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BilibiliUtilities.Utils.UserUtils
{
    public class UserUtil
    {
        private static readonly HttpClient _httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(20) };

        public static async Task<JToken> GetUserInfoByUid(long uid)
        {
            var tmpData = JObject.Parse(await _httpClient.GetStringAsync($"https://api.bilibili.com/x/space/acc/info?mid={uid}"));
            if (int.Parse(tmpData["code"].ToString()) != 0)
            {
                return "";
            }

            //用户信息
            var data = tmpData["data"];
            return data;
        }
    }
}