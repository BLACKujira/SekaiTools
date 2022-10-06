using System;

namespace BilibiliUtilities.Live.Lib
{
    /// <summary>
    /// 发送数据的消息的头信息
    /// </summary>
    public class DanmuHead
    {
        /// <summary>
        /// 总长度 (协议头 + 数据长度)
        /// </summary>
        public int PacketLength;

        /// <summary>
        /// 头长度 
        /// </summary>
        public short HeaderLength;

        /// <summary>
        /// 版本
        /// </summary>
        public short Version;

        /// <summary>
        /// 操作类型 (消息类型)
        /// </summary>
        public int Action;

        /// <summary>
        /// 参数, 固定为1
        /// </summary>
        public int Parameter;

        /// <summary>
        /// 将从流中读取的内容转换为消息头
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static DanmuHead BufferToDanmuHead(byte[] buffer)
        {
            if (buffer.Length < 16)
            {
                throw new ArgumentException();
            }

            return new DanmuHead
            {
                PacketLength = EndianBitConverter.EndianBitConverter.BigEndian.ToInt32(buffer, 0),
                HeaderLength = EndianBitConverter.EndianBitConverter.BigEndian.ToInt16(buffer, 4),
                Version = EndianBitConverter.EndianBitConverter.BigEndian.ToInt16(buffer, 6),
                Action = EndianBitConverter.EndianBitConverter.BigEndian.ToInt32(buffer, 8),
                Parameter = EndianBitConverter.EndianBitConverter.BigEndian.ToInt32(buffer, 12),
            };
        }

        /// <summary>
        /// 计算数据部分的长度
        /// </summary>
        /// <returns>
        ///    PacketLength - HeaderLength 类型数据:int
        /// </returns>
        public int MessageLength()
        {
            return PacketLength - HeaderLength;
        }
    }
}