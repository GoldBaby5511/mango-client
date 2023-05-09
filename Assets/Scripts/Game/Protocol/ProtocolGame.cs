using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

//SCORE-Int64   WORD-UInt16     DWORD-UInt32    TCHAR-string   LONG - int LONGLONG-int64




//网络速度测试 0-3
public class CMD_Game_CS_NetSpeed : DataBase
{
    public float time;

    public CMD_Game_CS_NetSpeed()
    {
        header.wMainCmdID = 0;
        header.wSubCmdID = 3;
    }

    protected override void PackBody()
    {
        WriteFloat(time);
    }

    protected override void UnPackBody()
    {
        time = ReadFloat();
    }
}

/// 登陆游戏 1-2
public class CMD_Game_C_LoginGame : DataBase
{ 
    //版本信息
    public UInt32 wGameID;					//游戏标识
	public UInt32 dwProcessVersion;			//进程版本

	//桌子区域
	public Byte cbDeviceType;                   //设备类型
    public UInt16 wBehaviorFlags = 0;           //行为标识
    public UInt16 wPageTableCount = 0;          //分页桌数

	//登录信息
	public UInt32 dwUserID;				    	//用户 I D
	public string szPassword = "";				//登录密码
	public string szServerPasswd = "";          //房间密码
	public string szMachineID = "";		        //机器标识

    //GPS信息
    public float fLatitude;										//用户纬度
    public float fLongitude;									//用户经度
    public float fHeight;										//用户高度


    public CMD_Game_C_LoginGame()
    {
        header.wMainCmdID = 1;
        header.wSubCmdID = 2;
    }

    protected override void PackBody()
    {
        WriteUInt32(wGameID);
        WriteUInt32(dwProcessVersion);
        WriteByte(cbDeviceType);
        WriteUInt16(wBehaviorFlags);
        WriteUInt16(wPageTableCount);
        WriteUInt32(dwUserID);
        WriteString(szPassword, 66);
        WriteString(szServerPasswd, 66);
        WriteString(szMachineID, 66);

        WriteFloat(fLatitude);
        WriteFloat(fLongitude);
        WriteFloat(fHeight);
    }
}

/// 登录游戏成功 1-100
public class CMD_Game_S_LoginGameSuccess : DataBase
{
    public UInt32 dwUserRight;						    //用户权限
    public UInt32 dwMasterRight;						//管理权限

    protected override void UnPackBody()
    {
        dwUserRight = ReadUInt32();
        dwMasterRight = ReadUInt32();
    }
}

/// 登陆游戏失败 1-101
public class CMD_Game_S_LoginGameFail : DataBase
{ 
    public int lErrorCode;							//错误代码
	public string szDescribeString;				    //描述消息

    protected override void UnPackBody()
    {
        lErrorCode = ReadInt();
        szDescribeString = ReadString(256);
    }    
}

/// 游戏服务器配置 2-101
public class CMD_Game_S_GameServerConfig : DataBase
{
    //房间属性
    public UInt16 wTableCount;						//桌子数目
    public UInt16 wChairCount;						//椅子数目

    //房间配置
    public UInt16 wServerType;						//房间类型 
    public UInt32 dwServerRule;						//房间规则

    public Int64 roomCoinLimit;                     //房间金币（钻石）限制

    protected override void UnPackBody()
    {
        wTableCount = ReadUInt16();
        wChairCount = ReadUInt16();
        wServerType = ReadUInt16();
        dwServerRule = ReadUInt32();
        roomCoinLimit = ReadInt64();
    }
}





/// 用户进入房间
public class CMD_Game_S_UserCome : DataBase
{
    public PlayerInRoom player = new PlayerInRoom();

    protected override void UnPackBody()
    {
        player.dwGameID = ReadUInt32();  
        player.dwUserID = ReadUInt32();  
        player.dwGroupID = ReadUInt32(); 
        player.wFaceID = ReadUInt16();   
        player.dwCustomID = ReadUInt32();
        player.bIsAndroid = (ReadByte()==1)?true:false;
        player.cbGender = ReadByte();
        player.cbMemberOrder = ReadByte(); 
        player.cbMasterOrder = ReadByte(); 
        player.dwUserType = ReadUInt32();  
        player.wTableID = ReadUInt16();
        player.wChairID = ReadUInt16();    
        player.cbUserStatus = ReadByte();

        player.fLatitude = ReadFloat();
        player.fLongitude = ReadFloat();
        player.fHeight = ReadFloat();

        player.lScore = ReadInt64();
        player.lGrade = ReadInt64();
        player.lInsure = ReadInt64();
        player.lIngot = ReadInt64();
        player.redPack = ReadInt64();
        player.dBeans = ReadDouble();      
        player.dwWinCount = ReadUInt32();
        player.dwLostCount = ReadUInt32(); 
        player.dwDrawCount = ReadUInt32();
        player.dwFleeCount = ReadUInt32();
        player.dwExperience = ReadUInt32();
        player.lLoveLiness = ReadUInt32(); 
        player.lIntegralCount = ReadInt64();
        player.dwAgentID = ReadUInt32();    

        player.dataSize = ReadUInt16();
        player.dataDescrible = ReadUInt16();
        player.nickName = ReadString(player.dataSize * 2);
        int k = player.nickName.IndexOf('\0');
        if (k > 0) player.nickName = player.nickName.Substring(0, k);
    }
}

/// 用户状态
public class CMD_Game_S_UserState : DataBase
{						
    public UInt32 dwUserId;                                 //用户标识
    public TagUserState userState = new TagUserState();         //用户状态

    protected override void UnPackBody()
    {
        dwUserId = ReadUInt32();
        userState.wTableID = ReadUInt16();
        userState.wChairID = ReadUInt16();
        userState.cbUserStatus = ReadByte();
    }
}

//用户积分
public class CMD_Game_S_UserScore : DataBase
{
    public UInt32 dwUserID;							//用户标识
    public TagUserScore UserScore = new TagUserScore();					//积分信息

    protected override void UnPackBody()
    {
        dwUserID = ReadUInt32();

        UserScore.lScore = ReadInt64();
        UserScore.lGrade = ReadInt64();
        UserScore.lInsure = ReadInt64();
        UserScore.lIngot = ReadInt64();
        UserScore.repackCount = ReadInt64();
        UserScore.dBeans = ReadDouble();

        UserScore.dwWinCount = ReadUInt32();
        UserScore.dwLostCount = ReadUInt32();
        UserScore.dwDrawCount = ReadUInt32();
        UserScore.dwFleeCount = ReadUInt32();
        UserScore.lIntegralCount = ReadInt64();

        UserScore.dwExperience = ReadUInt32();
        UserScore.lLoveLiness = ReadUInt32();
    }
}





//桌子信息 4-100
public class CMD_Game_S_TableInfo : DataBase
{ 
    public UInt16 wTableCount;						                        //桌子数目
    public TagTableState[] TableStatusArray = new TagTableState[512];				//桌子状态

    protected override void UnPackBody()
    {
 	    wTableCount = ReadUInt16();
        for (int i = 0; i < wTableCount; i++)
        {
            TagTableState table = new TagTableState();
            table.cbTableLock = ReadByte();
            table.cbPlayStatus = ReadByte();
            table.lCellScore = ReadInt();
            TableStatusArray[i] = table;
        }
    }
}

//桌子状态 4-101
public class CMD_Game_S_TableState : DataBase
{ 
    public UInt16 wTableID;							        //桌子号码
    public TagTableState TableStatus = new TagTableState();	    //桌子状态

    protected override void UnPackBody()
    {
        wTableID = ReadUInt16();
        TableStatus.cbTableLock = ReadByte();
        TableStatus.cbPlayStatus = ReadByte();
        TableStatus.lCellScore = ReadInt();
    }
}

//坐下 3-3
public class CMD_Game_C_UserSit : DataBase
{
    public UInt16 wTableID;							//桌子位置
	public UInt16 wChairID;							//椅子位置
	public string szPassword = "";			        //桌子密码 66

    public CMD_Game_C_UserSit()
    {
        header.wMainCmdID = 3;
        header.wSubCmdID = 3;
    }

    protected override void PackBody()
    {
        WriteUInt16(wTableID);
        WriteUInt16(wChairID);
        WriteString(szPassword, 66);
    }
}

// 起立请求
public class CMD_Game_C_UserStand : DataBase
{
    public UInt16 wTableID;							//桌子位置
    public UInt16 wChairID;							//椅子位置
    public byte cbForceLeave;						//强行离开

    public CMD_Game_C_UserStand()
    { 
        header.wMainCmdID = 3;
        header.wSubCmdID = 4;
    }

    protected override void PackBody()
    {
        WriteUInt16(wTableID);
        WriteUInt16(wChairID);
        WriteByte(cbForceLeave);
    }
}

//请求失败
public class CMD_Game_S_RequestFailure : DataBase
{ 
    public int lErrorCode;							//错误代码
	public string szDescribeString;				//描述信息

    protected override void UnPackBody()
    {
        lErrorCode = ReadInt();
        szDescribeString = ReadString(512);
    }
}




#region 断线续连

//请求当前游戏状态
public class CMD_Game_C_GetGameStatus : DataBase
{
    public byte cbAllowLookon = 0;						//旁观标志
    public UInt32 dwFrameVersion = 0;					//框架版本
    public UInt32 dwClientVersion = 0;					//游戏版本0，101122049， 101122049

    public CMD_Game_C_GetGameStatus()
    {
        header.wMainCmdID = 100;
        header.wSubCmdID = 1;
    }

    protected override void PackBody()
    {
        WriteByte(cbAllowLookon);
        WriteUInt32(dwFrameVersion);
        WriteUInt32(dwClientVersion);
    }
}

//游戏状态
public class CMD_Game_S_GameStatus : DataBase
{ 
    public byte cbGameStatus;						//游戏状态
    public byte cbAllowLookon;						//旁观标志

    protected override void UnPackBody()
    {
        cbGameStatus = ReadByte();
        cbAllowLookon = ReadByte();
    }
}

#endregion

#region 房间操作

//创建房间
public class CMD_Game_C_CreateRoom : DataBase
{
    public byte cbGameType = 0;							//游戏类型
    public byte bPlayCoutIdex;							//游戏局数,此处传递数组下标
    public UInt32 dwClubNum = 0;						//(XS 俱乐部号码)
    public UInt32 dwFanTimes = 0;						//游戏蕃数
    public UInt32 dwMaxTimes = 0;						//游戏封顶
    public UInt32 dwGameRule = 0;						//游戏规则
    public UInt32 dwRevenue;							//游戏税收 (XS税收)
    public Int64 lCostGold;							    //消耗点数 (XS最低分数)

    public CMD_Game_C_CreateRoom()
    {
        header.wMainCmdID = 14;
        header.wSubCmdID = 1;
    }

    protected override void PackBody()
    {
        WriteByte(cbGameType);
        WriteByte(bPlayCoutIdex);
        WriteUInt32(dwClubNum);
        WriteUInt32(dwFanTimes);
        WriteUInt32(dwMaxTimes);
        WriteUInt32(dwGameRule);
        WriteUInt32(dwRevenue);
        WriteInt64(lCostGold);
    }
}

//创建房间结果
public class CMD_Game_S_CreateRoomResult : DataBase
{
    public UInt32 cardCount;    //剩余房卡
    public UInt32 dwRoomNum;    //房间ID

    public CMD_Game_S_CreateRoomResult()
    {
        header.wMainCmdID = 14;
        header.wSubCmdID = 403;
    }

    protected override void UnPackBody()
    {
        cardCount = ReadUInt32();
        dwRoomNum = ReadUInt32();
    }
}

//加入房间
public class CMD_Game_C_JoinRoom : DataBase
{
    public byte cbRoomType;								//游戏类型
    public UInt32 dwRoomNum;        //房间ID

    public CMD_Game_C_JoinRoom()
    {
        header.wMainCmdID = 14;
        header.wSubCmdID = 2;
    }

    protected override void PackBody()
    {
        WriteByte(cbRoomType);
        WriteUInt32(dwRoomNum);
    }
}

//解散房间
public class CMD_Game_C_PrivateDismiss : DataBase
{
    public byte cbRoomType = 0;
    public byte cbResetPrivateTable = 0;                   //是否还原
    public UInt32 dwRoomNum = 0;                                //
    public UInt32 dwClubNum = 0;							//

    public CMD_Game_C_PrivateDismiss()
    {
        header.wMainCmdID = 14;
        header.wSubCmdID = 3;
    }

    protected override void PackBody()
    {
        WriteByte(cbRoomType);
        WriteByte(cbResetPrivateTable);
        WriteUInt32(dwRoomNum);
        WriteUInt32(dwClubNum);
    }
}

//房间信息
public class CMD_Game_S_RoomInfo : DataBase
{
    public byte cbStartGame;                                                    //游戏开始							
    public UInt32 dwPlayCout;                                                   //当前局数
    public UInt32 dwRoomID;                                                     //房间 I D
    public UInt32 dwRoomNum;                                                    //房间号码
    public UInt32 dwCreateUserID;                                               //用户标识
    public UInt32 dwPlayTotal;                                                  //游戏总局
    public byte cbRoomType;                                                     //房间类型
    public byte cbPlayCountIndex;                                               //玩家局数
    public UInt32 dwFanTimes;                                                   //游戏蕃数
    public UInt32 dwMaxTimes;                                                   //游戏底分
    public UInt32 dwGameRule;                                                   //游戏规则
    public Int64[] lPlayerWinLose = new Int64[100];                             //输赢积分
    public UInt32[] dwDissUserID = new UInt32[4];                                              //同意用户
    public UInt32[] dwNotAgreeUserID = new UInt32[4];					                        //否定用户


    protected override void UnPackBody()
    {
        cbStartGame = ReadByte();
        dwPlayCout = ReadUInt32();
        dwRoomID = ReadUInt32();
        dwRoomNum = ReadUInt32();
        dwCreateUserID = ReadUInt32();
        dwPlayTotal = ReadUInt32();
        cbRoomType = ReadByte();
        cbPlayCountIndex = ReadByte();

        dwFanTimes = ReadUInt32();
        dwMaxTimes = ReadUInt32();
        dwGameRule = ReadUInt32();

        for (int i = 0; i < 100; i++)
        {
            lPlayerWinLose[i] = ReadInt64();
        }
        for (int i = 0; i < 4; i++)
        {
            dwDissUserID[i] = ReadUInt32();
        }
        for (int i = 0; i < 4; i++)
        {
            dwNotAgreeUserID[i] = ReadUInt32();
        }
    }
}

//解散房间
public class CMD_Game_C_DisRoom : DataBase
{
    public byte isDisRoom;

    public CMD_Game_C_DisRoom()
    {
        header.wMainCmdID = 14;
        header.wSubCmdID = 5;
    }

    protected override void PackBody()
    {
        WriteByte(isDisRoom);
    }
}

//解散房间信息
public class CMD_Game_S_DisRoomInfo : DataBase
{
    public UInt32 dwUserId;                                 //发起投票用户ID
    public UInt32 agreeUserCount;                           //同意解散的人数
    public UInt32 disAgreeUserCount;                        //不同意解散的人数
    public List<UInt32> agreeUserList = new List<UInt32>();       //同意解散的用户的userId
    public List<UInt32> disAgreeUserList = new List<UInt32>();    //不同意解散用户的userId

    protected override void UnPackBody()
    {
       
        agreeUserList.Clear();
        disAgreeUserList.Clear();
        dwUserId = ReadUInt32();
        agreeUserCount = ReadUInt32();
        disAgreeUserCount = ReadUInt32();
        for (int i = 0; i < 100; i++)
        {
            agreeUserList.Add(ReadUInt32());
        }

        for (int i = 0; i < 100; i++)
        {
            disAgreeUserList.Add(ReadUInt32());
        }
    }
}

// 解散房间结果
public class CMD_Game_S_DisRoomResult : DataBase
{
    public UInt32 dwUserID;                                 //用户标识
    public UInt32 dwRoomID;                                 //房间标识
    public UInt32 dwRoomNumber;								//房间号码

    protected override void UnPackBody()
    {
        dwUserID = ReadUInt32();
        dwRoomID = ReadUInt32();
        dwRoomNumber = ReadUInt32();
    }
}

//房卡数量
public class CMD_Game_S_CardCount : DataBase
{
    public UInt32 dwUserID;                     //用户标识
    public UInt32 dwRoomID;                     //房间标识
    public UInt32 dwRoomNumber;                 //房间号码
    public UInt32 dwFKPropCount;                //房卡数量
    public string szDescribeString;				//描述消息 128

    protected override void UnPackBody()
    {
        dwUserID = ReadUInt32();
        dwRoomID = ReadUInt32();
        dwRoomNumber = ReadUInt32();
        dwFKPropCount = ReadUInt32();
        szDescribeString = ReadString(128);
    }
}


//房卡结束，总结算消息
public class CMD_Game_S_TotalScore : DataBase
{
    public byte[] cbWinConut = new byte[20];													//胡牌次数
    public byte[] cbMaxTaiCount = new byte[20];												//最大台数
    public Int64[] totalScore = new Int64[20];
    public byte[] customData = new byte[1024];                                              //自定义数据

    protected override void UnPackBody()
    {
        cbWinConut = ReadByteArray(20);
        cbMaxTaiCount = ReadByteArray(20);
        totalScore = ReadInt64Ary(20);
        customData = ReadByteArray(1024);
    }
}

#endregion

#region 聊天

/// 聊天信息-文字
public class CMD_Game_C_ChatMessage : DataBase
{
    public UInt16 wChatLength;                       //信息长度
    public UInt32 dwChatColor;                      //信息颜色
    public UInt32 dwTargetUserID;                       //目标用户
    public string szChatString;		//聊天信息 256

    public CMD_Game_C_ChatMessage()
    {
        header.wMainCmdID = 100;
        header.wSubCmdID = 10;
    }


    protected override void PackBody()
    {
        WriteUInt16(wChatLength);
        WriteUInt32(dwChatColor);
        WriteUInt32(dwTargetUserID);
        WriteString(szChatString, 256);
    }
}

/// 聊天信息-文字
public class CMD_Game_S_ChatMessage : DataBase
{
    public UInt16 wChatLength;                       //信息长度
    public UInt32 dwChatColor;                      //信息颜色
    public UInt32 dwSendUserID;                     //发送用户
    public UInt32 dwTargetUserID;                       //目标用户
    public string szChatString;		//聊天信息 256

    protected override void UnPackBody()
    {
        wChatLength = ReadUInt16();
        dwChatColor = ReadUInt32();
        dwSendUserID = ReadUInt32();
        dwTargetUserID = ReadUInt32();
        szChatString = ReadString(256);
    }
}

//聊天信息-语音
public class CMD_Game_C_AudioMessage : DataBase
{
    public UInt32 dwSendUserID;                     //发送用户
    public UInt32 dwTargetUserID;                       //目标用户
    public byte[] cbVoiceData = new byte[1024 * 8];				//语音数据

    public CMD_Game_C_AudioMessage()
    {
        header.wMainCmdID = 100;
        header.wSubCmdID = 11;
    }

    protected override void PackBody()
    {
        WriteUInt32(dwSendUserID);
        WriteUInt32(dwTargetUserID);
        for (int i = 0; i < 1024 * 8; i++)
        {
            WriteByte(cbVoiceData[i]);
        }
    }
}

/// 聊天信息-语音
public class CMD_Game_S_AudioMessage : DataBase
{
    public UInt32 dwSendUserID;                     //发送用户
    public UInt32 dwTargetUserID;                       //目标用户
    public byte[] cbVoiceData = new byte[8000];				//语音数据

    protected override void UnPackBody()
    {
        dwSendUserID = ReadUInt32();
        dwTargetUserID = ReadUInt32();
        for (int i = 0; i < 8000; i++)
        {
            cbVoiceData[i] = ReadByte();
        }
    }
}

#endregion

#region 红包

//收到红包通知
public class CMD_Game_S_GetRedPack : DataBase
{
    public byte cbSendType;                                                     //发送类型 0红包雨、1首局赠送
    public UInt32[] dwUserID = new UInt32[100];							//用户标识

    protected override void UnPackBody()
    {
        cbSendType = ReadByte();
        for (int i = 0; i < 100; i++)
        {
            dwUserID[i] = ReadUInt32();
        }
    }
}

//开红包结果
public class CMD_Game_S_OpenRedPackResult : DataBase
{
    public UInt32 dwUserID;						//用户标识
    public Int64 lUserRedEnvelopes;				//用户红包
    public Int64 lAwardRedEnvelopes;			//奖励红包
    public string szNotifyContent;				//提示消息 256

    protected override void UnPackBody()
    {
        dwUserID = ReadUInt32();
        lUserRedEnvelopes = ReadInt64();
        lAwardRedEnvelopes = ReadInt64();
        szNotifyContent = ReadString(256);
    }
}


//红包进度信息
public class CMD_Game_S_RedPackState : DataBase
{
    public UInt32 dwUserID;							//用户 I D
    public UInt32 nIntervalTime;					//最小间隔
    public UInt16 nMinDrawCount;					//最小局数
    public UInt32 nCurIntervalTime;					//当前时间
    public UInt16 nCurMinDrawCount;				    //当前局数
    public string szDescribeString;				//提示消息 256

    protected override void UnPackBody()
    {
        dwUserID = ReadUInt32();
        nIntervalTime = ReadUInt32();
        nMinDrawCount = ReadUInt16();
        nCurIntervalTime = ReadUInt32();
        nCurMinDrawCount = ReadUInt16();
        szDescribeString = ReadString(256);
    }
}

//低保信息
public class CMD_Game_S_BaseEnsure : DataBase
{
    public byte cbType;							//低保类型 0充值，1复活兑换
    public string szNotifyContent;					//消息内容

    protected override void UnPackBody()
    {
        cbType = ReadByte();
        szNotifyContent = ReadString(256);
    }
}

#endregion

#region 低保
//流程：
//发送 加载低保-> 返回 低保参数
//发送 领取低保-> 返回 低保结果

////低保命令
//#define MDM_GR_BASEENSURE				11

//#define SUB_GR_C_BASEENSURE_LOAD			260								//加载低保
//#define SUB_GR_C_BASEENSURE_TAKE			261								//领取低保
//#define SUB_GR_S_BASEENSURE_PARAMETER		262								//低保参数
//#define SUB_GR_S_BASEENSURE_RESULT		263								//低保结果

//领取低保
public class CMD_Game_C_BaseEnsureTake : DataBase
{
    public UInt32 dwUserID;				//用户 I D
    public int nBaseEnsureType;			//低保类型 0,红包场,1,金币场
    public string szPassword;			//登录密码 66
    public string szMachineID;		    //机器序列 66

    public CMD_Game_C_BaseEnsureTake()
    {
        header.wMainCmdID = 11;
        header.wSubCmdID = 261;
    }

    protected override void PackBody()
    {
        WriteUInt32(dwUserID);
        WriteInt(nBaseEnsureType);
        WriteString(szPassword, 66);
        WriteString(szMachineID, 66);
    }
}


//低保结果
public class CMD_Game_S_BaseEnsureResult : DataBase
{
    
    public bool bSuccessed;							//成功标识
    public int nBaseEnsureType;     				//低保类型 0,红包场,1,金币场
    public Int64 lGameScore;					    //当前游戏币
    public Int64 lGameIngot;
    public string szNotifyContent;				    //提示内容 256

    protected override void UnPackBody()
    {
        bSuccessed = ReadByte() > 0;
        nBaseEnsureType = ReadInt();
        lGameScore = ReadInt64();
        lGameIngot = ReadInt64();
        szNotifyContent = ReadString(256);
    }
}



#endregion








