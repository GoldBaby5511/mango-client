using System;
using System.Net.Sockets;
using System.Net;
using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.Events;
using bs.gateway;

/// <summary>
/// 实现了对数据的处理以及与服务器端的连接，包括收发数据等
/// </summary>
public class ClientSocket
{
    public Socket socket;
    public NetManager netManager;
    public event UnityAction connectSuccessEvent;      //网络连接成功事件，外部处理
    public event UnityAction connectErrorEvent;      //网络异常事件，外部处理

    public ClientSocket()
    {
        netManager = new NetManager(this);
    }

    /// <summary>
    /// 连接服务器
    /// </summary>
    public void Connect(string ip, int port)
    {
        String newServerIp = "";
        AddressFamily newAddressFamily = AddressFamily.InterNetwork;

        Util.GetIPType(ip, port.ToString(), out newServerIp, out newAddressFamily);

        if (!string.IsNullOrEmpty(newServerIp))
        {
            ip = newServerIp;
        }

        IPEndPoint ipe = new IPEndPoint(IPAddress.Parse(ip), port);

        try
        {
            socket = new Socket(newAddressFamily, SocketType.Stream, ProtocolType.Tcp);
            socket.BeginConnect(ipe, new AsyncCallback(ConnectionCallback), socket);
        }
        catch (SocketException ex)
        {
            if (ex.ErrorCode == 10054 || ex.ErrorCode == 10060)
            {
                if (connectErrorEvent != null)
                {
                    connectErrorEvent.Invoke();
                }
            }
            Debug.LogError("Connect网络连接异常 ： " + ex.ErrorCode + " - " + ex.Message);
        }
        catch (Exception e)
        {
            if (e.Message.Contains("Not connected"))
            {
                if (connectErrorEvent != null)
                {
                    connectErrorEvent.Invoke();
                }
            }
            Debug.LogError("Connect网络连接异常 ： " + e.Message);
        }
    }

    /// <summary>
    /// 关闭与服务器的连接
    /// </summary>
    public void CloseConnect()
    {
        try
        {
            socket.Close();
        }
        catch (Exception e)
        {
            Debug.LogError("CloseConnect网络异常 ： " + e.Message);
        }
    }

    /// 异步回调函数
    void ConnectionCallback(IAsyncResult ar)
    {
        Socket client = (Socket)ar.AsyncState;
        try
        {
            //结束挂起的异步连接
            client.EndConnect(ar);
            //通知业务层服务器连接成功
            NetPacket packet = new NetPacket();
            packet.socket = client;

            if (connectSuccessEvent != null)
            {
                connectSuccessEvent.Invoke();
            }
            //开始异步接收信息
            socket.BeginReceive(packet.bytes, 0, NetPacket.MSG_LEN, SocketFlags.None, new AsyncCallback(ReceiveHeader), packet);
        }
        catch (SocketException ex)
        {
            if (ex.ErrorCode == 10054 || ex.ErrorCode == 10060)
            {
                if (connectErrorEvent != null)
                {
                    connectErrorEvent.Invoke();
                }
            }
            Debug.LogError("ConnectionCallback网络连接异常 ： " + ex.ErrorCode + " - " + ex.Message);
        }
        catch (Exception e)
        {
            if (e.Message.Contains("Not connected"))
            {
                if (connectErrorEvent != null)
                {
                    connectErrorEvent.Invoke();
                }
            }
            Debug.LogError("ConnectionCallback网络连接异常 ： " + e.Message);
        }
    }

    // 接收消息头
    void ReceiveHeader(IAsyncResult ar)
    {
        NetPacket packet = (NetPacket)ar.AsyncState;
        try
        {
            int readLength = packet.socket.EndReceive(ar);
            if (readLength < 1)
            {
                return;
            }
            packet.readLength += readLength;

            //Debug.Log("ReceiveHeader," + packet.readLength + ",readLength=" + readLength);

            if (packet.readLength < NetPacket.MSG_LEN)
            {
                //消息头未满MSG_LEN个字节时，继续接收
                packet.socket.BeginReceive(packet.bytes, packet.readLength, NetPacket.MSG_LEN - packet.readLength, SocketFlags.None, new AsyncCallback(ReceiveHeader), packet);
            }
            else
            {
                //Debug.Log("GetPacketSize=" + packet.GetPacketSize());

                if (packet.GetPacketSize() == NetPacket.MSG_LEN)
                {
                    //获取消息头
                    //packet.header.wMainCmdID = BitConverter.ToUInt16(packet.bytes, 4);
                    //packet.header.wSubCmdID = BitConverter.ToUInt16(packet.bytes, 6);

                    netManager.AddPacket(packet);

                    //重置数据包状态
                    packet.Reset();
                    //递归，继续接收消息
                    packet.socket.BeginReceive(packet.bytes, 0, NetPacket.MSG_LEN, SocketFlags.None, new System.AsyncCallback(ReceiveHeader), packet);
                    return;
                }
                packet.readLength = 0;
                //开始接收消息体
                packet.socket.BeginReceive(packet.bytes, NetPacket.MSG_LEN, (int)packet.GetPacketSize(), SocketFlags.None, new AsyncCallback(ReceiveBody), packet);
            }
        }
        catch (SocketException ex)
        {
            if (ex.ErrorCode == 10054 || ex.ErrorCode == 10060)
            {
                if (connectErrorEvent != null)
                {
                    connectErrorEvent.Invoke();
                }
            }
            Debug.LogError("ReceiveHeader网络连接异常 ： " + ex.ErrorCode + " - " + ex.Message);
        }
        catch (Exception e)
        {
            if (e.Message.Contains("Not connected"))
            {
                if (connectErrorEvent != null)
                {
                    connectErrorEvent.Invoke();
                }
            }
            Debug.LogError("ReceiveHeader网络连接异常 ： " + e.Message);
        }
    }

    /// <summary>
    /// 接收消息体
    /// </summary>
    void ReceiveBody(IAsyncResult ar)
    {
        NetPacket packet = (NetPacket)ar.AsyncState;
        try
        {
            int readLength = packet.socket.EndReceive(ar);   // 返回网络上接收的数据长度
            // 已断开连接
            if (readLength < 1)
            {
                return;
            }
            packet.readLength += readLength;

            // 消息体必须读满指定的长度
            if (packet.readLength < packet.GetPacketSize())
            {
                packet.socket.BeginReceive(packet.bytes,
                    NetPacket.MSG_LEN + packet.readLength,
                    (int)packet.GetPacketSize() - packet.readLength,
                    SocketFlags.None,
                    new System.AsyncCallback(ReceiveBody),
                    packet);
            }
            else
            {
                //packet.GetHeader();


                //Debug.Log("wMainCmdID," + packet.header.wMainCmdID + ",wSubCmdID=" + packet.header.wSubCmdID);

                netManager.AddPacket(packet);      // 将消息传入到逻辑处理队列
                //重置数据包状态
                packet.Reset();
                //开始下一次读取
                packet.socket.BeginReceive(packet.bytes, 0, NetPacket.MSG_LEN, SocketFlags.None, new System.AsyncCallback(ReceiveHeader), packet);
            }
        }
        catch (SocketException ex)
        {
            if (ex.ErrorCode == 10054 || ex.ErrorCode == 10060)
            {
                if (connectErrorEvent != null)
                {
                    connectErrorEvent.Invoke();
                }
            }
            Debug.LogError("ReceiveBody网络连接异常 ： " + ex.ErrorCode + " - " + ex.Message);
        }
        catch (Exception e)
        {
            if (e.Message.Contains("Not connected"))
            {
                if (connectErrorEvent != null)
                {
                    connectErrorEvent.Invoke();
                }
            }
            Debug.LogError("ReceiveBody网络连接异常 ： " + e.Message);
        }
    }

    /// <summary>
    /// 发送数据包
    /// </summary>
    public void SendPro(DataBase pro)
    {

    }

    public void SendDate2Gate<T>(UInt32 appType, UInt16 cmdId, T instance)
    {
        byte[] sendBytes;
        using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
        {
            ProtoBuf.Serializer.Serialize(stream, instance);
            sendBytes = stream.ToArray();
        }

        UInt16 headSize = 1 + 1 + 2 + 2 + 0;
        UInt32 allSize = 2 + (UInt32)headSize + (UInt32)sendBytes.Length;

        int ipos = 0;
        byte[] bytes = new byte[allSize + 4];

        {
            byte[] bs = System.BitConverter.GetBytes(allSize);
            Array.Reverse(bs);
            Array.Copy(bs, 0, bytes, ipos, 4);
            ipos = ipos + 4;
        }

        {
            byte[] bs = System.BitConverter.GetBytes(headSize);
            Array.Reverse(bs);
            Array.Copy(bs, 0, bytes, ipos, 2);
            ipos = ipos + 2;
        }
        {
            bytes[ipos] = 99;
            ipos++;
            bytes[ipos] = 104;
            ipos++;
        }
        {
            byte[] bs = System.BitConverter.GetBytes((UInt16)appType);
            Array.Reverse(bs);
            Array.Copy(bs, 0, bytes, ipos, 2);
            ipos = ipos + 2;
        }
        {
            byte[] bs = System.BitConverter.GetBytes((UInt16)cmdId);
            Array.Reverse(bs);
            Array.Copy(bs, 0, bytes, ipos, 2);
            ipos = ipos + 2;
        }

        Array.Copy(sendBytes, 0, bytes, ipos, sendBytes.Length);
        ipos = ipos + sendBytes.Length;
        string strByte = "";
        for (int i = 0; i < ipos; i++)
        {
            strByte += bytes[i] + ",";
        }

        //Debug.Log("发送长度," + allSize + ",ipos="+ ipos + ",bytes=" + bytes.Length+","+ strByte);

        try
        {
            lock (socket)
            {
                NetworkStream ns = new NetworkStream(socket);
                if (ns.CanWrite)
                {
                    //Debug.Log("确定发送," + allSize + ",bytes=" + bytes.Length);
                    ns.BeginWrite(bytes, 0, (int)ipos, new AsyncCallback(SendCallback), ns);
                }
            }
        }
        catch (SocketException ex)
        {
            if (ex.ErrorCode == 10054 || ex.ErrorCode == 10060)
            {
                if (connectErrorEvent != null)
                {
                    connectErrorEvent.Invoke();
                }
            }
            Debug.LogError("SendPro网络连接异常 ： " + ex.ErrorCode + " - " + ex.Message);
        }
        catch (Exception e)
        {
            if (e.Message.Contains("Not connected"))
            {
                if (connectErrorEvent != null)
                {
                    connectErrorEvent.Invoke();
                }
            }
            Debug.LogError("SendPro网络连接异常 ： " + e.Message);
        }
    }

    public void SendTransferData2Gate<T>(UInt32 destAppType, UInt32 destAppId, UInt32 dataAppType, UInt32 cmdId, T instance)
    {
        byte[] sendBytes;
        using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
        {
            ProtoBuf.Serializer.Serialize(stream, instance);
            sendBytes = stream.ToArray();
        }

        TransferDataReq req = new TransferDataReq();
        req.dest_apptype = destAppType;
        req.dest_appid = destAppId;
        req.data_apptype = dataAppType;
        req.data_cmdid = cmdId;
        req.data = sendBytes;
        SendDate2Gate(NetManager.AppGate, (UInt16)CMDGateway.IDTransferDataReq, req);
    }

    /// <summary>
    /// 发送方法SendPro()的回调函数
    /// </summary>
    void SendCallback(IAsyncResult ar)
    {
        NetworkStream ns = (NetworkStream)ar.AsyncState;
        try
        {
            ns.EndWrite(ar);
            ns.Flush();
            ns.Close();
        }
        catch (SocketException ex)
        {
            if (ex.ErrorCode == 10054 || ex.ErrorCode == 10060)
            {
                if (connectErrorEvent != null)
                {
                    connectErrorEvent.Invoke();
                }
            }
            Debug.LogError("SendCallback网络连接异常 ： " + ex.ErrorCode + " - " + ex.Message);
        }
        catch (Exception e)
        {
            if (e.Message.Contains("Not connected"))
            {
                if (connectErrorEvent != null)
                {
                    connectErrorEvent.Invoke();
                }
            }
            Debug.LogError("SendCallback网络连接异常 ： " + e.Message);
        }
    }

}
