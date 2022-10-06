using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BilibiliUtilities.Live.Message
{
    public class RoomUpdateMessage:BaseMessage
    {
        /// <summary>
        /// 真正的房间号
        /// </summary>
        public int RoomId;

        /// <summary>
        /// 主播当前的粉丝数量
        /// </summary>
        public int Fans;

        /// <summary>
        /// 红色通知?
        /// </summary>
        public int RedNotice;

        public static RoomUpdateMessage JsonToRoomUpdateMessage(JObject json)
        {
            return new RoomUpdateMessage
            {
                RoomId = int.Parse(json["data"]["roomid"].ToString()),
                Fans = int.Parse(json["data"]["fans"].ToString()),
                RedNotice = int.Parse(json["data"]["red_notice"].ToString()),
                Metadata = JsonConvert.SerializeObject(json)
            };
        }
        public static RoomUpdateMessage JsonToRoomUpdateMessage(string jsonStr)
        {
            try
            {
                return JsonToRoomUpdateMessage(JObject.Parse(jsonStr));
            }
            catch (JsonReaderException)
            {
                throw new AggregateException("JSON字符串没有成功转换成Json对象");
            }
        }
    }
}