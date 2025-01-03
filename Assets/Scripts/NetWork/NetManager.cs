using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using UnityEngine;

/// <summary>
/// 此类主要用于逻辑处理，包含两个功能
/// 1.将收到的数据包加入队列
/// 2.将数据包从队列中取出
/// </summary>
public class NetManager
{
    public const UInt32 Send2All = 1;
    public const UInt32 Send2AnyOne = 2;

    public const UInt32 AppLogger = 1;
    public const UInt32 AppCenter = 2;
    public const UInt32 AppConfig = 3;
    public const UInt32 AppGate = 4;
    public const UInt32 AppLobby = 5;
    public const UInt32 AppProperty = 6;
    public const UInt32 AppBattle = 7;
    public const UInt32 AppLogic = 8;
    public const UInt32 AppRobot = 9;
    public const UInt32 AppList = 10;
    public const UInt32 AppTable = 11;
    public const UInt32 AppRoom = 12;
    public const UInt32 AppDaemon = 100;

    private Queue packetList = new Queue();                         // 存储网络数据包的队列
    private List<IHandler> handlerList = new List<IHandler>();      // 数据包处理类列表

    public ClientSocket clientSocket;

    /// <summary>
    /// 构造方法，初始化ClientSocket对象
    /// </summary>
    public NetManager(ClientSocket cs)
    {
        this.clientSocket = cs;
    }

    /// <summary>
    /// 将数据处理类加入队列
    /// </summary>
    public void AddHandler(IHandler hd)
    {
        handlerList.Add(hd);
    }

    /// <summary>
    /// 移除队列
    /// </summary>
    public void RemoveHadler(IHandler hd) 
    {
        if (handlerList.Contains(hd))
        {
            handlerList.Remove(hd);
        }
    }

    /// <summary>
    /// 清除列表
    /// </summary>
    public void ClearHanderList()
    {
        if (handlerList != null || handlerList.Count > 0)
        {
            handlerList.Clear();
        }
    }

    /// <summary>
    /// 对数据进行处理
    /// </summary>
    public void OnUpdate()
    {
        Handler();
    }

    /// <summary>
    /// 数据包入队
    /// </summary>
    public void AddPacket(NetPacket packet)
    {
        lock (packetList)
        {
            NetPacket np = new NetPacket(packet);   //复制一个数据包对象
            packetList.Enqueue(np);
        }
    }

    /// <summary>
    /// 数据包出队
    /// </summary>
    public NetPacket GetPacket()
    {
        try
        {
            lock (packetList)
            {
                if (packetList.Count == 0)
                {
                    return null;
                }
                NetPacket np = (NetPacket)packetList.Dequeue();
                return np;
            }
        }
        catch (Exception e)
        {
            Debug.LogError("数据包出队异常-" + e.Message);
        }
        return null;
    }

    /// <summary>
    /// 对数据进行处理
    /// </summary>
    public void Handler()
    {
        //try
        //{
            NetPacket packet = null;
            for (packet = GetPacket(); packet != null; )
            {
                for (int i = 0; i < handlerList.Count; i++)
                {
                    if (handlerList[i].Handler(packet))
                    {
                        break;
                    }
                }
                packet = null;
            }
        //}
        //catch (Exception e)
        //{
        //    Debug.LogError("数据包解包异常 : " + e.Message);
        //}
    }

}
