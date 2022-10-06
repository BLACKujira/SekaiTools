using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using BilibiliUtilities.Live.Lib;
using EndianBitConverter;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using BilibiliUtilities.Utils.LiveUtils;

namespace BilibiliUtilities.Live
{
    public class LiveRoom
    {
        //直播页面的房间ID
        private int _shotRoomId;
        
        //真正的直播间ID
        private int _roomId;
        private readonly TcpClient _tcpClient = new TcpClient();

        private Stream _roomStream;

        //消息版本号,现在固定为2
        private const short ProtocolVersion = 2;

        //消息头的长度,现在版本固定为16
        //DanmuHead的方法BufferToDanmuHead中有个写死的16,如果后续有修改要一起修改
        private const int ProtocolHeadLength = 16;
        private readonly IMessageHandler _messageHandler;

        private readonly IMessageDispatcher _messageDispatcher;

        //连接状态
        private bool _connected = false;

        public LiveRoom(int roomId, IMessageHandler messageHandler)
        {
            _shotRoomId = roomId;
            _messageDispatcher = new MessageDispatcher();
            _messageHandler = messageHandler;
        }

        public LiveRoom(int roomId, IMessageHandler messageHandler, IMessageDispatcher messageDispatcher)
        {
            _shotRoomId = roomId;
            _messageDispatcher = messageDispatcher;
            _messageHandler = messageHandler;
        }

        /// <summary>
        /// 开启连接
        /// </summary>
        /// <returns></returns>
        public async Task<bool> ConnectAsync()
        {
            _roomId = await RoomUtil.GetLongRoomId(_shotRoomId);
            if (_roomId == 0)
            {
                return false;
            }
            var token = await RoomUtil.GetRoomTokenByShortRoomId(_shotRoomId);
            if (token.Equals(""))
            {
                return false;
            }
            if (!await RoomUtil.ConnectRoomByShortRoomId(_tcpClient, _shotRoomId))
            {
                return false;
            }
            _roomStream = _tcpClient.GetStream();
            if (!await SendJoinMsgAsync(token))
            {
                //这是错误处理的代码
                return false;
            }

            var headBuffer = new byte[ProtocolHeadLength];
            await _roomStream.ReadAsync(headBuffer, 0, headBuffer.Length);
            DanmuHead danmuHead = DanmuHead.BufferToDanmuHead(headBuffer);
            if (danmuHead.HeaderLength != ProtocolHeadLength || danmuHead.Action != 8)
            {
                //如果头信息的长度不是16,或者Action的的值不是8 (服务器接受认证包后回应的第一个数据)
                //这是错误处理的代码
                return false;
            }

            var dataBuffer = new byte[danmuHead.PacketLength - danmuHead.HeaderLength];
            await _roomStream.ReadAsync(dataBuffer, 0, danmuHead.MessageLength());
            var s = Encoding.Default.GetString(dataBuffer);
            var data = JObject.Parse(s);
            if (int.Parse(data["code"].ToString()) != 0)
            {
                return false;
            }

            _connected = true;
            //循环发送心跳信息
#pragma warning disable 4014
            SendHeartbeatLoop();
#pragma warning restore 4014
            return true;
        }

        /// <summary>
        /// 循环读取消息,禁止重复调用
        /// </summary>
        /// <returns></returns>
        public async Task ReadMessageLoop()
        {
            while (_connected)
            {
                var headBuffer = new byte[ProtocolHeadLength];
                //先读取一次头信息
                await _roomStream.ReadAsync(headBuffer, 0, ProtocolHeadLength);
                //解析头信息
                DanmuHead danmuHead = DanmuHead.BufferToDanmuHead(headBuffer);
                //判断协议
                if (danmuHead.HeaderLength != ProtocolHeadLength)
                {
                    continue;
                }

                //初始化一个放数据的byte数组
                byte[] dataBuffer;
                if (danmuHead.Action == 3)
                {
                    //给服务器发送心跳信息后的回应信息,所带的数据是直播间的观看人数(人气值)
                    dataBuffer = new byte[danmuHead.MessageLength()];
                    await _roomStream.ReadAsync(dataBuffer, 0, danmuHead.MessageLength());
                    var audiences = EndianBitConverter.EndianBitConverter.BigEndian.ToInt32(dataBuffer, 0);
                    _messageHandler.AudiencesHandlerAsync(audiences);
                    continue;
                }

                string tmpData;
                JObject json = null;
                if (danmuHead.Action == 5 && danmuHead.Version == ProtocolVersion)
                {
                    //有效负载为礼物、弹幕、公告等内容数据
                    //读取数据放入缓冲区
                    dataBuffer = new byte[danmuHead.MessageLength()];
                    await _roomStream.ReadAsync(dataBuffer, 0, danmuHead.MessageLength());
                    //之后把数据放入到内存流
                    string jsonStr;
                    using (var ms = new MemoryStream(dataBuffer, 2, danmuHead.MessageLength() - 2))
                    {
                        //使用内存流生成解压流(压缩流) 
                        var deflate = new DeflateStream(ms, CompressionMode.Decompress);
                        var headerbuffer = new byte[ProtocolHeadLength];
                        try
                        {
                            while (true)
                            {
                                await deflate.ReadAsync(headerbuffer, 0, ProtocolHeadLength);
                                danmuHead = DanmuHead.BufferToDanmuHead(headerbuffer);
                                var messageBuffer = new byte[danmuHead.MessageLength()];
                                var readLength = await deflate.ReadAsync(messageBuffer, 0, danmuHead.MessageLength());
                                jsonStr = Encoding.UTF8.GetString(messageBuffer, 0, danmuHead.MessageLength());
                                if (readLength == 0)
                                {
                                    break;
                                }
                                json = JObject.Parse(jsonStr);
                                _messageDispatcher.DispatchAsync(json, _messageHandler);
                            }
                            continue;
                        }
                        catch (Exception e)
                        {
                            //读数据超出长度
                            Debug.WriteLine(e);
                            throw;
                        }
                    }
                }

                dataBuffer = new byte[danmuHead.MessageLength()];
                await _roomStream.ReadAsync(dataBuffer, 0, danmuHead.MessageLength());
                tmpData = Encoding.UTF8.GetString(dataBuffer);
                try
                {
                    File.WriteAllText(@"C:\Users\KUROKAWA_KUJIRA\Desktop\255\1.json", tmpData);
                    json = JObject.Parse(tmpData);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                    throw e;
                }
                if (!"DANMU_MSG".Equals(json["cmd"].ToString()) && !"SEND_GIFT".Equals(json["cmd"].ToString()))
                {
                    _messageDispatcher.DispatchAsync(json, _messageHandler);
                }
            }
        }

        /// <summary>
        /// 发送加入房间的消息
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<bool> SendJoinMsgAsync(string token)
        {
            var packageModel = new Dictionary<string, object>
            {
                {"roomid", _roomId},
                {"uid", 0},
                {"protover", ProtocolVersion},
                {"token", token},
                {"platform", "web"},
                {"type", 2}
            };
            var body = JsonConvert.SerializeObject(packageModel);
            await SendSocketDataAsync(7, body);
            return true;
        }

        /// <summary>
        /// 发送消息的方法
        /// </summary>
        /// <param name="action"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public Task SendSocketDataAsync(int action, string body)
        {
            return SendSocketDataAsync(ProtocolHeadLength, ProtocolVersion, action, 1, body);
        }

        /// <summary>
        /// 发送消息的方法
        /// </summary>
        /// <param name="headLength"></param>
        /// <param name="version"></param>
        /// <param name="action"></param>
        /// <param name="param"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public async Task SendSocketDataAsync(short headLength, short version, int action, int param,
            string body)
        {
            var data = Encoding.UTF8.GetBytes(body);
            var packageLength = data.Length + headLength;

            var buffer = new byte[packageLength];
            var ms = new MemoryStream(buffer);
            await ms.WriteAsync(EndianBitConverter.EndianBitConverter.BigEndian.GetBytes(buffer.Length), 0, 4);
            await ms.WriteAsync(EndianBitConverter.EndianBitConverter.BigEndian.GetBytes(headLength), 0, 2);
            await ms.WriteAsync(EndianBitConverter.EndianBitConverter.BigEndian.GetBytes(version), 0, 2);
            await ms.WriteAsync(EndianBitConverter.EndianBitConverter.BigEndian.GetBytes(action), 0, 4);
            await ms.WriteAsync(EndianBitConverter.EndianBitConverter.BigEndian.GetBytes(param), 0, 4);
            if (data.Length > 0)
            {
                await ms.WriteAsync(data, 0, data.Length);
            }

            await _roomStream.WriteAsync(buffer, 0, buffer.Length);
        }

        /// <summary>
        /// 循环发送心跳,禁止重复调用
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private async Task SendHeartbeatLoop()
        {
            try
            {
                _connected = _tcpClient.Connected;

                while (_connected)
                {
                    try
                    {
                        await SendSocketDataAsync(ProtocolHeadLength, ProtocolVersion, 2, 1, "");
                        //休眠30秒
                        await Task.Delay(30000);
                    }
                    catch (Exception e)
                    {
                        _connected = false;
                        Console.WriteLine("发送心跳失败");
                        throw e;
                    }
                }
            }
            catch (Exception e)
            {
                Disconnect();
                throw e;
            }
        }

        /// <summary>
        /// 关闭连接的方法
        /// </summary>
        /// <exception cref="Exception"></exception>
        public void Disconnect()
        {
            try
            {
                _connected = false;
                _tcpClient.Dispose();
                _roomStream = null;
            }
            catch (Exception e)
            {
                //错误处理
                throw e;
            }
        }

        /// <summary>
        /// 反回连接的状态
        /// </summary>
        /// <returns></returns>
        public bool Connected()
        {
            return _connected;
        }
    }
}