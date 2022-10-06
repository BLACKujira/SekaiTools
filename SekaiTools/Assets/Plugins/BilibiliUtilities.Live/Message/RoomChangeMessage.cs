using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BilibiliUtilities.Live.Message
{
    public class RoomChangeMessage : BaseMessage
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title;

        /// <summary>
        /// 子分区ID
        /// </summary>
        public int AreaId;

        /// <summary>
        /// 分区ID
        /// </summary>
        public int ParentAreaId;

        /// <summary>
        /// 子分区名称
        /// </summary>
        public string AreaName;

        /// <summary>
        /// 分区名称
        /// </summary>
        public string ParentAreaName;

        public static RoomChangeMessage JsonToRoomChangeMessage(JObject json)
        {
            return new RoomChangeMessage
            {
                Title = json["data"]["title"].ToString(),
                AreaId = int.Parse(json["data"]["area_id"].ToString()),
                ParentAreaId = int.Parse(json["data"]["parent_area_id"].ToString()),
                AreaName = json["data"]["area_name"].ToString(),
                ParentAreaName = json["data"]["parent_area_name"].ToString(),
                Metadata = JsonConvert.SerializeObject(json)
            };
        }

        public static RoomChangeMessage JsonToRoomChangeMessage(string jsonStr)
        {
            try
            {
                return JsonToRoomChangeMessage(JObject.Parse(jsonStr));
            }
            catch (JsonReaderException)
            {
                throw new AggregateException("JSON字符串没有成功转换成Json对象");
            }
        }
    }
}