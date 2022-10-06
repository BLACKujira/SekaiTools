using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BilibiliUtilities.Live.Message
{
    // TODO 还需要收集数据
    public class ComboEndMessage:BaseMessage
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
        /// 连续送礼 数量
        /// </summary>
        public int ComboNum;

        /// <summary>
        /// 礼物数量
        /// </summary>
        public int GiftNum;

        /// <summary>
        /// 未知
        /// </summary>
        public int BatchComboNum;

        /// <summary>
        /// 价格,即是银瓜子也是金瓜子
        /// </summary>
        public int Price;

        /// <summary>
        /// 礼物名称
        /// </summary>
        public string GiftName;

        /// <summary>
        /// 礼物ID
        /// </summary>
        public int GiftId;

        /// <summary>
        /// 舰长等级?
        /// </summary>
        public int GuardLevel;

        public static ComboEndMessage JsonToComboEndMessage(JObject json)
        {
            if (!"COMBO_SEND".Equals(json["cmd"].ToString()))
            {
                throw new ArgumentException("'cmd' 的值不是 'COMBO_END'");
            }

            var data = json["data"];
            return new ComboEndMessage
            {
                UserId = long.Parse(data["uid"].ToString()),
                Username = data["uname"]?.ToString(),
                ComboNum = int.Parse(data["combo_num"].ToString()),
                GiftNum = int.Parse(data["gift_num"].ToString()),
                BatchComboNum = int.Parse(data["gift_num"].ToString()),
                Price = int.Parse(data["gift_num"].ToString()),
                GiftName = data["gift_name"].ToString(),
                GiftId = int.Parse(data["gift_id"].ToString()),
                Metadata = JsonConvert.SerializeObject(json)
            };
        }

        public static ComboEndMessage JsonToComboEndMessage(string jsonStr)
        {
            try
            {
                var json = JObject.Parse(jsonStr);
                return JsonToComboEndMessage(json);
            }
            catch (JsonReaderException)
            {
                throw new AggregateException("JSON字符串没有成功转换成Json对象");
            }
        }
    }
}