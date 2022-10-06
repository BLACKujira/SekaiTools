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

namespace BilibiliUtilities.Utils.LiveUtils
{
    public class RoomUtil
    {
        private static readonly HttpClient _httpClient = new HttpClient {Timeout = TimeSpan.FromSeconds(20)};

        public static async Task<int> GetLongRoomId(long shortRoomId)
        {
            var tmpData = JObject.Parse(await _httpClient.GetStringAsync($"https://api.live.bilibili.com/room/v1/Room/room_init?id={shortRoomId}"));
            if (int.Parse(tmpData["code"].ToString())!=0)
            {
                return 0;
            }
            return int.Parse(tmpData["data"]["room_id"].ToString());
        }

        public static async Task<string> GetRoomTokenByShortRoomId(long shortRoomId)
        {
            var tmpData = JObject.Parse(await _httpClient.GetStringAsync($"https://api.live.bilibili.com/room/v1/Room/room_init?id={shortRoomId}"));
            if (int.Parse(tmpData["code"].ToString()) != 0)
            {
                return "";
            }
            var roomId = int.Parse(tmpData["data"]["room_id"].ToString());
            tmpData = JObject.Parse(await _httpClient.GetStringAsync(
                $"https://api.live.bilibili.com/room/v1/Danmu/getConf?room_id={roomId}&platform=pc&player=web"));
            //连接的令牌
            var token = tmpData["data"]["token"].ToString();
            return token;
        }

        public static async Task<bool> ConnectRoomByShortRoomId(TcpClient tcpClient,long shortRoomId)
        {
            var roomId = await GetRoomTokenByShortRoomId(shortRoomId);
            if (roomId.Equals(""))
            {
                return false;
            }
            var tmpData = JObject.Parse(await _httpClient.GetStringAsync(
                $"https://api.live.bilibili.com/room/v1/Danmu/getConf?room_id={roomId}&platform=pc&player=web"));
            //解析域名,拿取IP地址,用于连接
            var chatHost = tmpData["data"]["host"].ToString();
            var ips = await Dns.GetHostAddressesAsync(chatHost);
            //连接的端口
            var chatPort = int.Parse(tmpData["data"]["port"].ToString());
            Random random = new Random();
            //随机一个选择域名解析出来的IP,负载均衡
            await tcpClient.ConnectAsync(ips[random.Next(ips.Length)], chatPort);
            if (!tcpClient.Connected)
            {
                return false;
            }
            if (!tcpClient.GetStream().CanWrite)
            {
                return false;
            }
            return true;
        }
    }
}