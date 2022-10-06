using System;
using System.Diagnostics;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BilibiliUtilities.Live.Message
{
    public class DanmuMessage : BaseMessage
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
        /// 弹幕内容
        /// </summary>
        public string Content;

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
        public string MedalOwnerName;

        /// <summary>
        /// 舰队等级
        /// 0 为非船员 1 为总督 2 为提督 3 为舰长
        /// </summary>
        public int UserGuardLevel;
        /// <summary>
        /// 是不是房管
        /// </summary>
        public bool Admin;
        /// <summary>
        /// 是不是老爷
        /// </summary>
        public bool Vip;
        public DanmuMessage()
        {
        }

        public static DanmuMessage JsonToDanmuMessage(JObject json)
        {
            if (!"DANMU_MSG".Equals(json["cmd"].ToString()))
            {
                throw new ArgumentException("'cmd' 的值不是 'DANMU_MSG'");
            }

            var info = json["info"];
            try
            {
                var medal = "";
                var medalLevel = 0;
                var medalOwnerName = "";
                //判断有没有佩戴粉丝勋章
                if (info[3].ToArray().Length != 0)
                {
                    medal = info[3][1].ToString();
                    medalLevel = int.Parse(info[3][0].ToString());
                    medalOwnerName = info[3][2].ToString();
                }

                return new DanmuMessage
                {
                    UserId = long.Parse(info[2][0].ToString()),
                    Username = info[2][1].ToString(),
                    Content = info[1].ToString(),
                    Medal = medal,
                    MedalLevel = medalLevel,
                    MedalOwnerName = medalOwnerName,
                    Admin = info[2][2].ToString().Equals("1"),
                    Vip = info[2][3].ToString().Equals("1"),
                    UserGuardLevel = int.Parse(info[7].ToString()),
                    Metadata = JsonConvert.SerializeObject(json)
                };
            }
            catch (ArgumentOutOfRangeException e)
            {
                Debug.WriteLine(e);
                throw;
            }
        }

        public static DanmuMessage JsonToDanmuMessage(string jsonStr)
        {
            try
            {
                var json = JObject.Parse(jsonStr);
                return JsonToDanmuMessage(json);
            }
            catch (JsonReaderException)
            {
                throw new AggregateException("JSON字符串没有成功转换成Json对象");
            }
        }
    }
}