using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BilibiliUtilities.Live.Message
{
    public class InteractWordMessage : BaseMessage
    {

        /// <summary>
        /// 用户UID
        /// </summary>
        public long UserId;

        /// <summary>
        /// 用户名称
        /// </summary>
        public string Username;

        /// <summary>
        /// 勋章名称
        /// </summary>
        public string Medal;

        /// <summary>
        /// 勋章等级
        /// </summary>
        public int MedalLevel;

        /// <summary>
        /// 勋章所有者
        /// </summary>
        public long MedalOwnerId;

        public static InteractWordMessage JsonToInteractWordMessage(JObject json)
        {
            if (!"INTERACT_WORD".Equals(json["cmd"].ToString()))
            {
                throw new ArgumentException("'cmd' 的值不是 'INTERACT_WORD'");
            }

            var data = json["data"];
            return new InteractWordMessage
            {
                UserId = long.Parse(data["uid"].ToString()),
                Username = data["uname"]?.ToString(),
                Medal = data["fans_medal"]["medal_name"]?.ToString(),
                MedalLevel = int.Parse(data["fans_medal"]["medal_level"]?.ToString()),
                MedalOwnerId = long.Parse(data["fans_medal"]["target_id"].ToString()),
                Metadata = JsonConvert.SerializeObject(json)
            };
        }

        public static InteractWordMessage JsonToInteractWordMessage(string jsonStr)
        {
            try
            {
                var json = JObject.Parse(jsonStr);
                return JsonToInteractWordMessage(json);
            }
            catch (JsonReaderException)
            {
                throw new AggregateException("JSON字符串没有成功转换成Json对象");
            }
        }
    }
}