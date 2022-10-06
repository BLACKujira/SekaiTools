using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BilibiliUtilities.Live.Message
{
    /// <summary>
    /// 相对GuardBuyMessage,这个信息更为全面且准确,建议使用这个
    /// </summary>
    public class UserToastMessage : BaseMessage
    {
        /// <summary>
        /// [未知]操作类型
        /// 可能代表着续费之类的
        /// </summary>
        public int OpType;

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
        /// 价格,实际花费的价格
        /// 比如舰长原价是198R,但是后续续费是158R,那么Price就是158000金瓜子
        /// </summary>
        public int Price;

        /// <summary>
        /// 有效值:舰长 提督 总督
        /// 这里会显示名称,而不再是数字
        /// </summary>
        public string RoleName;

        /// <summary>
        /// 单位,月
        /// </summary>
        public string Unit;

        /// <summary>
        /// 当前大航海人数有多少
        /// </summary>
        public int GuardCount;

        public string ToastMsg;

        public static UserToastMessage JsonToUserToastMessage(JObject json)
        {
            if (!"USER_TOAST_MSG".Equals(json["cmd"].ToString()))
            {
                throw new ArgumentException("'cmd' 的值不是 'USER_TOAST_MSG'");
            }

            var data = json["data"];
            return new UserToastMessage
            {
                OpType = int.Parse(data["op_type"].ToString()),
                UserId = long.Parse(data["uid"].ToString()),
                Username = data["username"].ToString(),
                Num = int.Parse(data["num"].ToString()),
                GuardLevel = int.Parse(data["guard_level"].ToString()),
                Price = int.Parse(data["price"].ToString()),
                RoleName = data["role_name"].ToString(),
                Unit = data["unit"].ToString(),
                ToastMsg = data["toast_msg"].ToString(),
                Metadata = JsonConvert.SerializeObject(json)
            };
        }

        public static UserToastMessage JsonToUserToastMessage(string jsonStr)
        {
            try
            {
                var json = JObject.Parse(jsonStr);
                return JsonToUserToastMessage(json);
            }
            catch (JsonReaderException)
            {
                throw new AggregateException("JSON字符串没有成功转换成Json对象");
            }
        }
    }
}