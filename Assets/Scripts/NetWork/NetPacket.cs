using System;
using UnityEngine;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;

/// <summary>
/// 数据包类
/// </summary>
public class NetPacket
{
    // |	msgSize	 |	headSize		| 						header 												| msgData
    // |4bit(msgSize)| 2bit(headSize) 	| 1bit(version) + 1bit(encrypt) + 2bit(AppType) + 2bit(CmdId) + Xbit(other) | msgData
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
        this.header.cbDataKind = np.header.cbDataKind;
        this.header.cbCheckCode = np.header.cbCheckCode;
        this.header.wPacketSize = np.header.wPacketSize;
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
        header.cbDataKind = 0;
        header.cbCheckCode = 0;
        header.wPacketSize = 0;
        header.wMainCmdID = 0;
        header.wSubCmdID = 0;
        readLength = 0;
        bodyLenght = 0;
        for (int i = 0; i < PACKET_LEN; ++i) bytes[i] = 0;
    }

    // 取得数据头
    public void GetHeader()
    {
        header.wMainCmdID = ReadUInt16(8);
        header.wSubCmdID = ReadUInt16(10);
    }

    public UInt32 GetMsgAppType()
    {
        return ReadUInt16(8);
    }

    public UInt32 GetMsgCmdId()
    {
        return ReadUInt16(10);
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

    public T Deserialize<T>()
    {
        return ProtoBuf.Serializer.Deserialize<T>(new System.IO.MemoryStream(GetData()));
    }

}

