using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//充值结果
public class CMD_CM_S_RabbitMQInfo : DataBase
{
    public TagRabbitMQInfo RabbitMQInfo = new TagRabbitMQInfo();

    protected override void UnPackBody()
    {
        RabbitMQInfo.dwUserID = ReadUInt32();
        RabbitMQInfo.nBuyType = ReadInt();
        RabbitMQInfo.nAwardID = ReadInt();
        RabbitMQInfo.nPayType = ReadInt();
    }
}


#region 每日任务
//加载任务
public class CMD_CM_C_LoadTaskInfo : DataBase
{
    public UInt32 dwUserID;                         //用户标识
    public string szPassword;			//用户密码 66

    public CMD_CM_C_LoadTaskInfo()
    {
        header.wMainCmdID = 1001;
        header.wSubCmdID = 1;
    }

    protected override void PackBody()
    {
        WriteUInt32(dwUserID);
        WriteString(szPassword, 66);
    }
};

//领取奖励
public class CMD_CM_C_TaskReward : DataBase
{
    public UInt16 wTaskID;                           //任务标识
    public UInt32 dwUserID;                         //用户标识
    public string szPassword;         //登录密码 66
    public string szMachineID;		//机器序列 66

    public CMD_CM_C_TaskReward()
    {
        header.wMainCmdID = 1001;
        header.wSubCmdID = 2;
    }

    protected override void PackBody()
    {
        WriteUInt16(wTaskID);
        WriteUInt32(dwUserID);
        WriteString(szPassword, 66);
        WriteString(szMachineID, 66);
    }
};

public class CMD_CM_S_TaskParameter : DataBase
{
    public int taskCount = 0;
    public List<TagTaskParameter> taskParameter = new List<TagTaskParameter>();

    protected override void UnPackBody()
    {
        taskParameter.Clear();
        //taskCount = ((header.wPacketSize - 8) / 797);
        taskCount = ReadUInt16();
        for (int i = 0; i < taskCount; i++)
        {
            int desSize = ReadUInt16() - 161;
            TagTaskParameter parameter = new TagTaskParameter();
            parameter.wTaskID = ReadUInt16();
            parameter.wTaskType = ReadUInt16();
            parameter.wTaskObject = ReadUInt16();
            parameter.cbPlayerType = ReadByte();
            parameter.wKindID = ReadUInt16();
            parameter.dwTimeLimit = ReadUInt32();

            parameter.nStandardAwardPropID = ReadInt();
            parameter.nStandardAwardPropCount = ReadInt();
            parameter.nMemberAwardPropID = ReadInt();
            parameter.nMemberAwardPropCount = ReadInt();
            parameter.nActivityCount = ReadInt();

            parameter.szTaskName = ReadString(128);
            parameter.szTaskDescribe = ReadString(desSize);

            taskParameter.Add(parameter);
        }
    }
};

/// <summary>
/// 任务信息
/// </summary>
public class CMD_CM_S_TaskInfo : DataBase
{
    public UInt16 wTaskCount;                            //任务数量
    public List<TagTaskStatus> taskStatus = new List<TagTaskStatus>(); //任务状态 128

    protected override void UnPackBody()
    {
        taskStatus.Clear();
        wTaskCount = ReadUInt16();
        for (int i = 0; i < wTaskCount; i++)
        {
            TagTaskStatus status = new TagTaskStatus();
            status.wTaskID = ReadUInt16();
            status.wTaskProgress = ReadUInt16();
            status.cbTaskStatus = ReadByte();

            taskStatus.Add(status);
        }
    }
};

//任务完成
public class CMD_CM_S_TaskProgress : DataBase
{
    public byte cbTaskStatus;                           //任务状态 0 为未完成  1为成功   2为失败  3已领奖
    public UInt16 wFinishTaskID;                     //任务标识
    public UInt16 wTaskProgress;                     //任务进度

    protected override void UnPackBody()
    {
        cbTaskStatus = ReadByte();
        wFinishTaskID = ReadUInt16();
        wTaskProgress = ReadUInt16();
    }
};

//任务结果
public class CMD_CM_S_TaskResult : DataBase
{
    //结果信息
    public bool bSuccessed;                            //成功标识
    public UInt16 wTaskID;                            //任务ID
    public UInt16 wCommandID;                            //命令标识

    //财富信息
    public int nPropID;                           //道具ID
    public int nPropCount;                           //道具数量

    //提示信息
    public string szNotifyContent;				//提示内容 256

    protected override void UnPackBody()
    {
        bSuccessed = (ReadByte() == 1);
        wTaskID = ReadUInt16();
        wCommandID = ReadUInt16();

        nPropID = ReadInt();
        nPropCount = ReadInt();

        szNotifyContent = ReadString(256);
    }
};
#endregion

#region 挖宝

//
public class CMD_CM_C_LoadDigInfo : DataBase
{
    public UInt16  wKindID;
    public UInt16  wServerID;

    public CMD_CM_C_LoadDigInfo()
    {
        header.wMainCmdID = 1002;
        header.wSubCmdID = 1;
    }

    protected override void PackBody()
    {
        WriteUInt16(wKindID);
        WriteUInt16(wServerID);
    }
};

//
public class CMD_CM_S_LoadDigInfo : DataBase
{
    public int nConfigCount;
    public List<TagDigConigItem> digConfig = new List<TagDigConigItem>();
    //tagDigConigItem DigConfig[MAX_DIG_COUNT];

    protected override void UnPackBody()
    {
        nConfigCount = ReadInt();
        for(int i = 0; i < nConfigCount; ++ i)
        {
            TagDigConigItem digItem = new TagDigConigItem();
            digItem.nConfigID = ReadInt();
            digItem.nPropID = ReadInt();
            digItem.nPropCount = ReadInt();
            digConfig.Add(digItem);
        }
    }
};

//
public class CMD_CM_C_OperateDig : DataBase
{
    public int nOperateType;

    public CMD_CM_C_OperateDig()
    {
        header.wMainCmdID = 1002;
        header.wSubCmdID = 3;
    }

    protected override void PackBody()
    {
        WriteInt(nOperateType);
    }
};

//
public class CMD_CM_S_OperateDig : DataBase
{
    public int nOperateType;
    public int nTotalDrawCount;
    public int nCurDrawCount;

    protected override void UnPackBody()
    {
        nOperateType = ReadInt();
        nTotalDrawCount = ReadInt();
        nCurDrawCount = ReadInt();
    }
};

//
public class CMD_CM_C_DigTreasure : DataBase
{
    public CMD_CM_C_DigTreasure()
    {
        header.wMainCmdID = 1002;
        header.wSubCmdID = 5;
    }
};

//
public class CMD_CM_S_DigTreasure : DataBase
{
    public bool bResult;
    public TagDigConigItem DigItem = new TagDigConigItem();
    //tagDigConigItem DigItem;
    public string szDescribeString;				//描述信息 [128] 256

    protected override void UnPackBody()
    {
        bResult = (ReadByte() == 1);
        DigItem.nConfigID = ReadInt();
        DigItem.nPropID = ReadInt();
        DigItem.nPropCount = ReadInt();
        szDescribeString = ReadString(256);
    }
};

#endregion