using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BilibiliUtilities.Live.Message
{
    public class WelcomeMessage:BaseMessage
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserId;

        /// <summary>
        /// 用户名
        /// </summary>
        public string Username;

        /// <summary>
        /// 是不是管理员?
        /// </summary>
        public bool Admin;

        /// <summary>
        /// Svip?
        /// </summary>
        public int Svip;

        /// <summary>
        /// Vip?
        /// </summary>
        public int Vip;

        /// <summary>
        /// 未知参数
        /// </summary>
        public int MockEffect;

        /*
         {
           "cmd": "WELCOME",
           "data": {
             "uid": 86686683,
             "uname": "影之余烬",
             "is_admin": false,
             "svip": 0,
             "vip": 1,
             "mock_effect": 0
           }
         }
         */

        public static WelcomeMessage JsonToWelcomeMessage(JObject json)
        {
            if (!"WELCOME".Equals(json["cmd"].ToString()))
            {
                throw new ArgumentException("'cmd' 的值不是 'WELCOME'");
            }

            var data = json["data"];
            return new WelcomeMessage
            {
                UserId = long.Parse(data["uid"].ToString()),
                Username = data["uname"].ToString(),
                Admin = bool.Parse(data["is_admin"].ToString()),
                Svip = int.Parse(data["svip"].ToString()),
                Vip = int.Parse(data["vip"].ToString()),
                MockEffect = int.Parse(data["mock_effect"].ToString()),
                Metadata = JsonConvert.SerializeObject(json)
            };
        }

        public static WelcomeMessage JsonToWelcomeMessage(string jsonStr)
        {
            try
            {
                var json = JObject.Parse(jsonStr);
                return JsonToWelcomeMessage(json);
            }
            catch (JsonReaderException)
            {
                throw new AggregateException("JSON字符串没有成功转换成Json对象");
            }
        }
    }
}