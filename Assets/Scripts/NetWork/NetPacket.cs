using System;
using UnityEngine;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using Google.Protobuf;

/// <summary>
/// 数据包类
/// </summary>
public class NetPacket
{
    // |	msgSize	 |	headSize		| 						header 																				   | msgData
    // |4bit(msgSize)| 2bit(headSize) 	| 2bit(version) + 2bit(encrypt) + 4bit(AppType) + 4bit(AppId) + 2bit(MainCmdID) + 2bit(SubCmdID) + Xbit(other) | msgData
    public const int MSG_LEN = 4;                       //消息包长度
    public const int HEAD_LEN = 2;                      //记录消息头所占长度
    public const int BODY_LEN = 16384 - 8;              //最大消息体包
    public const int PACKET_LEN = 16384;                //最大数据包长度

    public Header header = new Header();        // 包头
    public Socket socket;                       // 发送这个数据包的socket

    public int readLength { get; set; }         // 从网络上读到的数据长度
    public int bodyLenght { get; set; }         // 当前数据体长

    public byte[] bytes { get; set; }           // 缓存流，接收到的数据存放在此byte数组中

    private int ipos = 0;

    public NetPacket()
    {
        readLength = 0;
        bytes = new byte[PACKET_LEN];
    }


    // 构造函数，克隆一个新的NetPacket
    public NetPacket(NetPacket np)
    {
        this.header.wVersion = np.header.wVersion;
        this.header.wEncrypt = np.header.wEncrypt;
        this.header.dwAppType = np.header.dwAppType;
        this.header.dwAppID = np.header.dwAppID;
        this.header.wMainCmdID = np.header.wMainCmdID;
        this.header.wSubCmdID = np.header.wSubCmdID;

        this.socket = np.socket;
        if (this.bytes == null)
        {
            this.bytes = new byte[PACKET_LEN];
        }
        Array.Copy(np.bytes, this.bytes, PACKET_LEN);
        this.readLength = np.readLength;
        this.bodyLenght = np.bodyLenght;
        this.ipos = np.ipos;
    }

    // 从数据流中读取uint32
    protected UInt32 ReadUInt()
    {
        UInt32 iRet = 0;
        byte[] bs = new byte[4];
        Array.Copy(bytes, ipos, bs, 0, 4);
        ipos = ipos + 4;

        Array.Reverse(bs);

        iRet = System.BitConverter.ToUInt32(bs, 0);
        return iRet;
    }

    //从数据流中读取Byte
    protected byte ReadByte()
    {
        try
        {
            byte b = bytes[ipos];
            ipos++;
            return b;
        }
        catch
        {
            return 0;
        }
    }

    // 从数据流中读取UInt16
    protected UInt16 ReadUInt16(int sourceIndex = 0)
    {
        UInt16 iRet = 0;
        byte[] bs = new byte[2];
        if (sourceIndex == 0)
        {
            Array.Copy(bytes, ipos, bs, 0, 2);
            ipos = ipos + 2;
        }
        else
        {
            Array.Copy(bytes, sourceIndex, bs, 0, 2);
        }
        Array.Reverse(bs);
        iRet = System.BitConverter.ToUInt16(bs, 0);
        return iRet;
    }

    // 对数据包进行重置
    public void Reset()
    {
        ipos = 0;
        header.wVersion = 0;
        header.wEncrypt = 0;
        header.dwAppType = 0;
        header.dwAppID = 0;
        header.wMainCmdID = 0;
        header.wSubCmdID = 0;
        readLength = 0;
        bodyLenght = 0;
        for (int i = 0; i < PACKET_LEN; ++i) bytes[i] = 0;
    }

    // 取得数据头
    public void GetHeader()
    {
        header.wMainCmdID = ReadUInt16(18);
        header.wSubCmdID = ReadUInt16(20);
    }

    public UInt32 GetMainCmdID()
    {
        return ReadUInt16(18);
    }

    public UInt32 GetSubCmdID()
    {
        return ReadUInt16(20);
    }

    // 获取数据包大小
    public UInt32 GetPacketSize()
    {
        UInt32 iRet = 0;
        byte[] bs = new byte[4];
        Array.Copy(bytes, 0, bs, 0, 4);
        Array.Reverse(bs);
        iRet = System.BitConverter.ToUInt32(bs, 0);
        return iRet;
    }

    public UInt16 GetHeaderLen()
    {
        return ReadUInt16(4);
    }

    public byte[] GetData()
    {
        //Debug.Log("获取data时,GetPacketSize，" + GetPacketSize() + "GetHeaderLen，" + GetHeaderLen());

        byte[] data = new byte[GetPacketSize() - (GetHeaderLen() + HEAD_LEN)];
        System.Array.Copy(bytes, MSG_LEN + (GetHeaderLen() + HEAD_LEN), data, 0, data.Length);
        return data;
    }

    //public T Deserialize<T>()
    //{
    //    return ProtoBuf.Serializer.Deserialize<T>(new System.IO.MemoryStream(GetData()));
    //}

    /// <summary>
    /// 序列化
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public static byte[] Serialize(IMessage message)
    {
        return message.ToByteArray();
    }

    public T Deserialize<T>() where T : IMessage, new()
    {
        IMessage message = new T();
        try
        {
            return (T)message.Descriptor.Parser.ParseFrom(new System.IO.MemoryStream(GetData()));
        }
        catch (System.Exception e)
        {
            throw e;
        }
    }

    /// <summary>
    /// 反序列化
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="packct"></param>
    /// <returns></returns>
    public static T Deserialize<T>(byte[] packct) where T : IMessage, new()
    {
        IMessage message = new T();
        try
        {
            return (T)message.Descriptor.Parser.ParseFrom(packct);
        }
        catch (System.Exception e)
        {
            throw e;
        }
    }

}

