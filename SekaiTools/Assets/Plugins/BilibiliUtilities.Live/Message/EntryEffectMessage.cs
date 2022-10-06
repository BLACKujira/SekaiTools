using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BilibiliUtilities.Live.Message
{
    public class EntryEffectMessage : BaseMessage
    {
        /// <summary>
        /// [未知]既不是用户ID也不是房间ID
        /// </summary>
        public int Id;

        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserId;

        /// <summary>
        /// [未知]TargetId
        /// </summary>
        public long TargetId;

        /// <summary>
        /// [未知]MockEffect
        /// </summary>
        public int MockEffect;

        /// <summary>
        /// 头像的URL
        /// </summary>
        public string FaceUrl;

        /// <summary>
        /// 舰队等级
        /// 0 为非船员 1 为总督 2 为提督 3 为舰长
        /// </summary>
        public int PrivilegeType;

        /// <summary>
        /// 显示出来的通知,如下
        /// 欢迎舰长 <%一个不太好看的用户名%> 进入直播间
        /// </summary>
        public string CopyWriting;

        public static EntryEffectMessage JsonToEntryEffectMessage(JObject json)
        {
            if (!"ENTRY_EFFECT".Equals(json["cmd"].ToString()))
            {
                throw new ArgumentException("'cmd' 的值不是 'ENTRY_EFFECT'");
            }

            var data = json["data"];
            return new EntryEffectMessage
            {
                Id = int.Parse(data["uid"].ToString()),
                UserId = long.Parse(data["uid"].ToString()),
                TargetId = long.Parse(data["target_id"].ToString()),
                MockEffect = int.Parse(data["mock_effect"].ToString()),
                FaceUrl = data["face"].ToString(),
                PrivilegeType = int.Parse(data["privilege_type"].ToString()),
                CopyWriting = data["copy_writing"].ToString(),
                Metadata = JsonConvert.SerializeObject(json)
            };
        }

        public static EntryEffectMessage JsonToEntryEffectMessage(string jsonStr)
        {
            try
            {
                var json = JObject.Parse(jsonStr);
                return JsonToEntryEffectMessage(json);
            }
            catch (JsonReaderException)
            {
                throw new AggregateException("JSON字符串没有成功转换成Json对象");
            }
        }
    }
}