using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BilibiliUtilities.Live.Message
{
    /// <summary>
    /// 相对UserToastMessage,这个信息较少且不准确,建议使用UserToastMessage
    /// </summary>
    public class GuardBuyMessage : BaseMessage
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
        /// 数量
        /// </summary>
        public int Num;

        /// <summary>
        /// 舰队等级
        /// 0 为非船员 1 为总督 2 为提督 3 为舰长
        /// </summary>
        public int GuardLevel;

        /// <summary>
        /// 价格,不打折的金瓜子数量
        /// 比如舰长原价是198R,但是后续续费是138R,那么Price还是198000金瓜子
        /// </summary>
        public int Price;

        /// <summary>
        /// RoleName
        /// 其实只会是数字,但是不知道为什么B站给的信息是"Name",而不是"ID"
        /// </summary>
        public string RoleName;

        /// <summary>
        /// 礼物名称,舰长 提督 总督
        /// </summary>
        public string GiftName;

        public static GuardBuyMessage JsonToGuardBuyMessage(JObject json)
        {
            if (!"GUARD_BUY".Equals(json["cmd"].ToString()))
            {
                throw new ArgumentException("'cmd' 的值不是 'GUARD_BUY'");
            }

            var data = json["data"];
            return new GuardBuyMessage
            {
                UserId = long.Parse(data["uid"].ToString()),
                Username = data["username"].ToString(),
                GuardLevel = int.Parse(data["guard_level"].ToString()),
                Num = int.Parse(data["num"].ToString()),
                Price = int.Parse(data["price"].ToString()),
                RoleName = data["role_name"].ToString(),
                GiftName = data["gift_name"].ToString(),
                Metadata = JsonConvert.SerializeObject(json)
            };
        }

        public static GuardBuyMessage JsonToGuardBuyMessage(string jsonStr)
        {
            try
            {
                var json = JObject.Parse(jsonStr);
                return JsonToGuardBuyMessage(json);
            }
            catch (JsonReaderException)
            {
                throw new AggregateException("JSON字符串没有成功转换成Json对象");
            }
        }
    }
}