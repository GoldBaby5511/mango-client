 using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

//网络包头
public class Header
{
    public UInt16 wVersion;
    public UInt16 wEncrypt;
    public UInt32 dwAppType;
    public UInt32 dwAppID;
    public UInt16 wMainCmdID;						//主命令码
    public UInt16 wSubCmdID;						//子命令码

    public Header()
    {
        wVersion = 0x01;
        wEncrypt = 0;
        dwAppType = 0;
        dwAppID = 0;
        wMainCmdID = 0;
        wSubCmdID = 0;
    }
}

/// <summary>
/// 消息基类
/// </summary>
public class DataBase
{
    public Header header;                    //包头

    private int ipos;                          //位置
    //public byte[] bytes = new byte[ClientSocket.PACKET_LEN];
    public byte[] bytes = new byte[0];

    public DataBase()
    {
        header = new Header();
    }

    private bool IsEmpty
    {
        get
        {
            if (bytes == null)
            {
                return true;
            }
            return false;
        }
    }

    /// <summary>
    /// 打包操作，协议类转化为字节流
    /// </summary>
    public void Pack()
    {
//         ipos = 8;
//         PackBody();
//         header.wPacketSize = (UInt16)ipos;
//         ipos = 0;
//         WriteByte(header.cbDataKind);
//         WriteByte(header.cbCheckCode);
//         WriteUInt16(header.wPacketSize);
//         WriteUInt16(header.wMainCmdID);
//         WriteUInt16(header.wSubCmdID);
    }

    /// <summary>
    /// 解包操作
    /// </summary>
    public void UnPack(byte[] bs)
    {
//         Array.Copy(bs, 0, bytes, 0, ClientSocket.PACKET_LEN);
// 
//         header.cbDataKind = ReadByte();
//         header.cbCheckCode = ReadByte();
//         header.wPacketSize = ReadUInt16();
//         header.wMainCmdID = ReadUInt16();
//         header.wSubCmdID = ReadUInt16();
//         
//         UnPackBody();
    }

    /// <summary>
    /// 打包操作
    /// </summary>
    protected virtual void PackBody(){}

    /// <summary>
    /// 解包操作
    /// </summary>
    protected virtual void UnPackBody(){}



    #region 数据读取

    /// <summary>
    /// 读取byte
    /// </summary>
    protected byte ReadByte()
    {
        if (IsEmpty) return 0;
        byte b = bytes[ipos];
        ipos++;
        return  b;
    }

    /// <summary>
    /// 读取Uint型
    /// </summary>
    protected UInt32 ReadUInt()
    {
        if (IsEmpty) return 0;
        UInt32 iRet = 0;
        byte[] bs = new byte[4];
        Array.Copy(bytes, ipos, bs, 0, 4);
        ipos = ipos + 4;
		if (BitConverter.IsLittleEndian == false) {
			Array.Reverse (bs);
		}

        iRet = System.BitConverter.ToUInt32(bs, 0);
        return iRet;
    }

    /// <summary>
    /// 读取Int型
    /// </summary>
    protected int ReadInt()
    {
        if (IsEmpty) return 0;
        int iRet = 0;
        byte[] bs = new byte[4];
        Array.Copy(bytes, ipos, bs, 0, 4);
        ipos = ipos + 4;
		if (BitConverter.IsLittleEndian == false) 
		{
			Array.Reverse (bs);
		}
        iRet =  System.BitConverter.ToInt32(bs, 0);
        return iRet; 
    }

    /// <summary>
    /// 读取Int16
    /// </summary>
    protected Int16 ReadInt16()
    {
        if (IsEmpty) return 0;
        Int16 iRet = 0;
        byte[] bs = new byte[2];
        Array.Copy(bytes, ipos, bs, 0, 2);
        ipos = ipos + 2;
        if (BitConverter.IsLittleEndian == false)
        {
            Array.Reverse(bs);
        }
        iRet = System.BitConverter.ToInt16(bs, 0);
        return iRet;
    }

    //读取UInt16类型
    protected UInt16 ReadUInt16()
    {
        if (IsEmpty) return 0;

        UInt16 iRet = 0;
        byte[] bs = new byte[2];
        Array.Copy(bytes, ipos, bs, 0, 2);
        ipos = ipos + 2;
        if (BitConverter.IsLittleEndian == false)
        {
            Array.Reverse(bs);
        }
        iRet = System.BitConverter.ToUInt16(bs, 0);
        return iRet;
    }

    //读取UInt32类型
    protected UInt32 ReadUInt32()
    {
        if (IsEmpty) return 0;

        UInt32 iRet = 0;
        byte[] bs = new byte[4];
        Array.Copy(bytes, ipos, bs, 0, 4);
        ipos = ipos + 4;
        if (BitConverter.IsLittleEndian == false)
        {
            Array.Reverse(bs);
        }
        iRet = System.BitConverter.ToUInt32(bs, 0);
        return iRet;
    }

    //读取int64类型
    protected Int64 ReadInt64()
    {
        if (IsEmpty) return 0;
        Int64 iRet = 0;
        byte[] bs = new byte[8];
        Array.Copy(bytes, ipos, bs, 0, 8);
        ipos = ipos + 8;
		if (BitConverter.IsLittleEndian == false) {
			Array.Reverse (bs);
		}
        iRet = System.BitConverter.ToInt64(bs, 0);
        return iRet;
    }

    //读取UInt64类型
    protected UInt64 ReadUInt64()
    {
        if (IsEmpty) return 0;
        UInt64 iRet = 0;
        byte[] bs = new byte[8];
        Array.Copy(bytes, ipos, bs, 0, 8);
        ipos = ipos + 8;
        if (BitConverter.IsLittleEndian == false)
        {
            Array.Reverse(bs);
        }
        iRet = System.BitConverter.ToUInt64(bs, 0);
        return iRet;
    }

    //读取byte数组
    protected byte[] ReadByteArray(int len)
    {
        byte[] array = new byte[len];
        if (IsEmpty) return array;
        for (int i = 0; i < len; i++)
        {
            array[i] = ReadByte();
        }
        return array;
    }

    //读取int16数据
    protected Int16[] ReadInt16Ary(int len)
    {
        Int16[] ary = new Int16[len];
        if (IsEmpty) return ary;
        for (int i = 0; i < len; i++)
        {
            ary[i] = ReadInt16();
        }
        return ary;
    }

    //读取uint16数据
    protected UInt16[] ReadUInt16Ary(int len)
    {
        UInt16[] ary = new UInt16[len];
        if (IsEmpty) return ary;
        for (int i = 0; i < len; i++)
        {
            ary[i] = ReadUInt16();
        }
        return ary;
    }

    //读取int数组
    protected int[] ReadIntArray(int len)
    {
        int[] array = new int[len];
        if (IsEmpty) return array;
        for (int i = 0; i < len; i++)
        {
            array[i] = ReadInt();
        }
        return array;
    }

    //读取int64数组
    protected Int64[] ReadInt64Ary(int len)
    {
        Int64[] array = new Int64[len];
        if (IsEmpty) return array;
        for (int i = 0; i < len; i++)
        {
            array[i] = ReadInt64();
        }
        return array;
    }

    //读取float
    protected float ReadFloat()
    {
        if (IsEmpty) return 0;
        float iRet = 0;
        byte[] bs = new byte[4];
        Array.Copy(bytes, ipos, bs, 0, 4);
        ipos = ipos + 4;
        if (BitConverter.IsLittleEndian == false)
        {
            Array.Reverse(bs);
        }
        iRet = System.BitConverter.ToSingle(bs, 0);
        return iRet;
    }

    protected float[] ReadFloatArray(int len)
    {
        float[] array = new float[len];
        if (IsEmpty) return array;
        for (int i = 0; i < len; i++)
        {
            array[i] = ReadFloat();
        }
        return array;
    }

    //读取Double类型
    protected double ReadDouble()
    {
        if (IsEmpty) return 0;
        double iRect = 0;
        byte[] bs = new byte[8];
        Array.Copy(bytes, ipos, bs, 0, 8);
        ipos = ipos + 8;
        if (BitConverter.IsLittleEndian == false)
        {
            Array.Reverse(bs);
        }
        iRect = System.BitConverter.ToInt64(bs, 0);
        return iRect;
    }

    //读取string类型，参数为string长度
    protected string ReadString(int iLen)
    {
        if (IsEmpty) return "";
	    string sRet = "";
	    byte[] bs = new byte[iLen];

	    Array.Copy(bytes, ipos, bs, 0, iLen);
	    ipos = ipos + iLen;
        sRet = Encoding.Unicode.GetString(bs, 0, bs.Length);
	    int k = sRet.IndexOf('\0');
	    if (k > 0)
	    {
	        sRet = sRet.Substring(0, k);
	    }
	    return sRet;
    }

    #endregion

    #region 数据写入
        
    //写入byte
    protected void WriteByte(byte value)
    {
        if (IsEmpty) return ;
        bytes[ipos] = value;
        ipos++;
    }

    //写入int
    protected void WriteInt(int value)
    {
        if (IsEmpty) return;
        byte[] bs = System.BitConverter.GetBytes(value);
		if (BitConverter.IsLittleEndian == false) {
			Array.Reverse (bs);
		}
        Array.Copy(bs, 0, bytes, ipos, 4);
        ipos = ipos + 4;
    }

    //写UInt16型
    protected void WriteUInt16(UInt16 value, int iSpecifiedPos = -1)
    {
        if (IsEmpty) return ;
        byte[] bs = System.BitConverter.GetBytes(value);
        if (BitConverter.IsLittleEndian == false)
        {
            Array.Reverse(bs);
        }
        if (iSpecifiedPos == -1)
        {
            Array.Copy(bs, 0, bytes, ipos, 2);
            ipos = ipos + 2;
        }
        else
            Array.Copy(bs, 0, bytes, iSpecifiedPos, 2);
    }

    //写UInt32型
    protected void WriteUInt32(UInt32 value)
    {
        if (IsEmpty) return ;
        byte[] bs = System.BitConverter.GetBytes(value);
        if (BitConverter.IsLittleEndian == false)
        {
            Array.Reverse(bs);
        }
        Array.Copy(bs, 0, bytes, ipos, 4);
        ipos = ipos + 4;
    }

    //写int16型
    protected void WriteInt16(Int16 value)
    {
        if (IsEmpty) return ;
        byte[] bs = System.BitConverter.GetBytes(value);
        if (BitConverter.IsLittleEndian == false)
        {
            Array.Reverse(bs);
        }
        Array.Copy(bs, 0, bytes, ipos, 2);
        ipos = ipos + 2;
    }

    //写Int64类型
    protected void WriteInt64(Int64 value)
    {
        if (IsEmpty) return ;
        byte[] bs = System.BitConverter.GetBytes(value);
		if (BitConverter.IsLittleEndian == false) {
			Array.Reverse (bs);
		}
        Array.Copy(bs, 0, bytes, ipos, 8);
        ipos = ipos + 8;
    }

    //写入Float
    protected void WriteFloat(float value)
    {
        if (IsEmpty) return ;
        byte[] bs = System.BitConverter.GetBytes(value);
        if (BitConverter.IsLittleEndian == false)
        {
            Array.Reverse(bs);
        }
        Array.Copy(bs, 0, bytes, ipos, 4);
        ipos = ipos + 4;
    }

    //写double类型
    protected void WriteDouble(double value)
    {
        if (IsEmpty) return ;
        byte[] bs = System.BitConverter.GetBytes(value);
        if (BitConverter.IsLittleEndian == false)
        {
            Array.Reverse(bs);
        }
        Array.Copy(bs, 0, bytes, ipos, 8);
        ipos = ipos + 8;
    }

    //写入UInt
    protected void WriteUInt(uint value)
    {
        if (IsEmpty) return ;
            
        byte[] bs = System.BitConverter.GetBytes(value);
		if (BitConverter.IsLittleEndian == false) {
			Array.Reverse (bs);
		}
        Array.Copy(bs, 0, bytes, ipos, 4);
        ipos = ipos + 4;
    }

    //写入string，参数为长度
    protected void WriteString(string s, int iLen)
    {
        if (IsEmpty) return ;
        byte[] bs = Encoding.Unicode.GetBytes(s);
        Array.Copy(bs, 0, bytes, ipos, bs.Length);
	    ipos = ipos + iLen;
    }

    #endregion



    //数据补齐
    protected void Pad(int iMod)
    {
        if (ipos >= 20)
        {
            int iModLen = (ipos - 20) % iMod;
			if (iModLen != 0) 
            {
				ipos = ipos + (iMod - iModLen);
			}
        }
    }

    //跳转
    protected void SkipTo(int pos)
    {
        ipos = pos;
    }

    //补位
    protected void IncreasePos(int nCount)
    {
        ipos += nCount;
    }
    
}
