using System;
using System.Collections.Generic;

//SCORE-Int64   WORD-UInt16     DWORD-UInt32    TCHAR-string   LONG - int



//资料数据
//#define LEN_MD5						33									//加密密码
//#define LEN_USERNOTE				32									//备注长度
//#define LEN_ACCOUNTS				32									//帐号长度
//#define LEN_NICKNAME				32									//昵称长度
//#define LEN_PASSWORD				33									//密码长度
//#define LEN_GROUP_NAME			32									//社团名字
//#define LEN_UNDER_WRITE			64									//个性签名
//#define LEN_REMARKS					32									//备注信息
//#define LEN_DATETIME				20									//日期长度
//#define LEN_APPELLATION			32									//用户称谓

////数据长度
//#define LEN_QQ						16									//Q Q 号码
//#define LEN_EMAIL					33									//电子邮件
//#define LEN_USER_NOTE				256									//用户备注
//#define LEN_SEAT_PHONE				33									//固定电话
//#define LEN_MOBILE_PHONE			12									//移动电话
//#define LEN_PASS_PORT_ID			19									//证件号码
//#define LEN_COMPELLATION			16									//真实名字
//#define LEN_DWELLING_PLACE			128									//联系地址
//#define LEN_USER_UIN				33									//UIN长度
//#define LEN_WEEK					7									//星期长度
//#define LEN_TASK_NAME				64									//任务名称
//#define LEN_TRANS_REMARK			32									//转账备注	
//#define LEN_VERIFY_CODE				7									//验证长度
//#define LEN_MOBILE_CODE			7										//手机验证码
//#define LEN_POSTALCODE				8									//邮政编码
//#define LEN_BIRTHDAY				16									//用户生日
//#define LEN_BLOODTYPE				6									//用户血型
//#define LEN_CONSTELLATION			6									//用户星座
//#define LEN_PHONE_MODE				21									//手机型号
//#define LEN_WECHAT_URL				256									//微信URL
//#define LEN_CUSTOM_DATA				512									//自定义数据



/// <summary>
/// 网络心跳包
/// </summary>
//public class CMD_S_NetTest : DataBase
//{
//    public CMD_S_NetTest()
//    {
//        header.wMainCmdID = 0;
//        header.wSubCmdID = 1;
//    }
//}



#region 登录

/// 连接成功
public class CMD_Hall_C_ConnectSuccess : DataBase
{ 
    //属性资料
    public UInt32 dwUserID;						//用户 I D
    public UInt32 dwGameID;						//游戏 I D
    public UInt32 dwPlazaVersion;				//大厅版本
    public string szNickName = "";			    //用户昵称 64

	//连接信息
    public byte cbClientKind;					//连接类型
    public UInt32 dwInoutIndex;					//进出标识
    public string szMachineID = "";		        //机器标识 66

    protected override void PackBody()
    {
        WriteUInt32(dwUserID);
        WriteUInt32(dwGameID);
        WriteUInt32(dwPlazaVersion);
        WriteString(szNickName, 64);

        WriteByte(cbClientKind);
        WriteUInt32(dwInoutIndex);
        WriteString(szMachineID, 66);
    }
}

/// ID登录 100-1
public class CMD_Hall_C_LoginById : DataBase
{ 
    //系统信息
    public UInt16 wModuleID = 65535;			//模块标识
    public UInt32 dwPlazaVersion = 0;			//广场版本
    public byte cbDeviceType = 0x10;            //设备类型

	//登录信息
    public UInt32 dwGameID;					    //游戏 I D
    public string szPassword = "";				//登录密码 - 66

	//连接信息
    public string szMachineID = "";		        //机器标识 - 66
    public string szMobilePhone = "";	        //电话号码 - 24
    public string code = "";                    //验证码 - 14

    public CMD_Hall_C_LoginById()
    {
        header.wMainCmdID = 100;
        header.wSubCmdID = 1;
    }

    protected override void PackBody()
    {
        WriteUInt16(wModuleID);
        WriteUInt32(dwPlazaVersion);
        WriteByte(cbDeviceType);

        WriteUInt32(dwGameID);
        WriteString(szPassword, 66);

        WriteString(szMachineID, 66);
        WriteString(szMobilePhone, 24);
        WriteString(code, 14);
    }
}

/// 账号登陆 100-2
public class CMD_Hall_C_LoginAccount : DataBase
{
    //系统信息
    public UInt16 wModuleID = 65535;			//模块标识
    public UInt32 dwPlazaVersion = 0;		    //广场版本
    public Byte cbDeviceType = 0x10;            //设备类型

    //登录信息
    public string szPassword = "";		//登录密码
    public string szAccounts = "";		//登录帐号

    //连接信息
    public string szMachineID = "";		//机器标识
    public string szMobilePhone = "";	//电话号码

    public string veriCode = "";        //验证码 14
 

    public CMD_Hall_C_LoginAccount()
    {
        header.wMainCmdID = 100;
        header.wSubCmdID = 2;
    }

    protected override void PackBody()
    {
        WriteUInt16(wModuleID);
        WriteUInt32(dwPlazaVersion);
        WriteByte(cbDeviceType);
        WriteString(szPassword, 66);
        WriteString(szAccounts, 64);
        WriteString(szMachineID, 66);
        WriteString(szMobilePhone, 24);
        WriteString(veriCode, 14);
    }
}

/// 游客登陆 100-5
public class CMD_Hall_C_LoginVisitor : DataBase
{
    //系统信息
    public UInt16 wModuleID = 65535;			//模块标识
    public UInt32 dwPlazaVersion = 0;		    //广场版本
    public Byte cbDeviceType = 0x10;            //设备类型

    //连接信息
    public string szMachineID = "";                 //机器标识 - 33 * 2
    public string szMobilePhone = "12312312312";	//电话号码 - 12 * 2

    public CMD_Hall_C_LoginVisitor()
    {
        header.wMainCmdID = 100;
        header.wSubCmdID = 5;
    }

    protected override void PackBody()
    {
        WriteUInt16(wModuleID);
        WriteUInt32(dwPlazaVersion);
        WriteByte(cbDeviceType);

        WriteString(szMachineID, 66);
        WriteString(szMobilePhone, 24);
    }
}

/// 第三方登录 100-4
public class CMD_Hall_C_LoginOther : DataBase
{
    //系统信息
    public UInt16 wModuleID = 65535;			//模块标识
    public UInt32 dwPlazaVersion = 0;		    //广场版本
    public Byte cbDeviceType = 0x10;            //设备类型

    //登录信息
    public byte cbGender;                        //用户性别
    public byte cbChannelPartner;				 //渠道编号
    public byte cbPlatformID;                    //平台编号 微信 = 1
    public string szUserUin = "";                //用户Uin - 33
    public string szNickName = "";               //用户昵称 - 32
    public string szCompellation = "";           //真实名字 - 16

    //连接信息
    public string szMachineID = "";      //机器标识 - 33
    public string szMobilePhone = "";	//电话号码 - 12

    public CMD_Hall_C_LoginOther()
    {
        header.wMainCmdID = 100;
        header.wSubCmdID = 4;
    }

    protected override void PackBody()
    {
        WriteUInt16(wModuleID);
        WriteUInt32(dwPlazaVersion);
        WriteByte(cbDeviceType);

        WriteByte(cbGender);
        WriteByte(cbChannelPartner);
        WriteByte(cbPlatformID);
        WriteString(szUserUin, 33*2);
        WriteString(szNickName, 32*2);
        WriteString(szCompellation, 16*2);

        WriteString(szMachineID, 33*2);
        WriteString(szMobilePhone, 12*2);
    }
}

/// 注册账号 100-3
public class CMD_Hall_C_Register : DataBase
{
    //系统信息
    public UInt16 wModuleID = 65535;						//模块标识
    public UInt32 dwPlazaVersion = 0;						//广场版本
    public Byte cbDeviceType = 0x10;                        //设备类型

    //密码变量
    public string szLogonPass = "";          //登录密码

    //注册信息
    public UInt16 wFaceID;              //头像标识
    public Byte cbGender;               //用户性别
    public string szAccounts = "";			//登录帐号
    public string szNickName = "";			//用户昵称
    public string szSpreader = "";           //推荐帐号
    public UInt32 szSprederId;          //推荐人ID

    //连接信息
    public string szMachineID = "";          //机器标识
    public string szMobilePhone = "";	    //电话号码

    public CMD_Hall_C_Register()
    {
        header.wMainCmdID = 100;
        header.wSubCmdID = 3;
    }

    protected override void PackBody()
    {
        WriteUInt16(wModuleID);
        WriteUInt32(dwPlazaVersion);
        WriteByte(cbDeviceType);
        WriteString(szLogonPass, 66);
        WriteUInt16(wFaceID);
        WriteByte(cbGender);
        WriteString(szAccounts, 64);
        WriteString(szNickName, 64);
        WriteString(szSpreader, 64);
        WriteUInt32(szSprederId);

        WriteString(szMachineID, 66);
        WriteString(szMobilePhone, 24);
    }
}

/// 登录成功 100-100
public class CMD_Hall_S_LoginSuccess : DataBase
{ 
    public UInt16 wFaceID;              //头像标识
	public Byte cbGender;               //用户性别
	public UInt32 dwCustomID;           //自定头像 **X
	public UInt32 dwUserID;             //用户 I D
	public UInt32 dwGameID;             //游戏 I D
	public UInt32 dwExperience;         //经验数值
	public UInt32 dwLoveLiness;         //用户魅力

	public string szNickName;			//用户昵称
	public string szDynamicPass;		//动态密码
	
	//财富信息
	public Int64 lUserScore;            //用户金币
	public Int64 lUserIngot;            //用户钻石
	public Int64 lUserInsure;           //用户银行	
	public Double dUserBeans;           //用户游戏豆
    public Int32 cardCount;             //房卡数量
    //游戏信息
    public UInt32 dwWinCount;           //胜利盘数
    public UInt32 dwLostCount;          //失败盘数
    public UInt32 dwDrawCount;          //和局盘数
    public UInt32 dwFleeCount;          //逃跑盘数

	//扩展信息
    public UInt32 dwGameLogonTimes;     //登录次数
	public Byte cbInsureEnabled;        //使能标识 - 是否开通银行
    public Byte cbIsAgent;              //代理标识
    //推荐人信息
    public UInt32 spreaderId;           //推荐人ID
    public string spreaderName;         //推荐人昵称




    protected override void UnPackBody()
    {
        wFaceID = ReadUInt16();
        cbGender = ReadByte();
        dwCustomID = ReadUInt32();
        dwUserID = ReadUInt32();
        dwGameID = ReadUInt32();
        dwExperience = ReadUInt32();
        dwLoveLiness = ReadUInt32();
        szNickName = ReadString(64);
        szDynamicPass = ReadString(66);

        lUserScore = ReadInt64();
        lUserIngot = ReadInt64();
        lUserInsure = ReadInt64();
        dUserBeans = ReadDouble();
        cardCount = ReadInt();

        dwWinCount = ReadUInt32();
        dwLostCount = ReadUInt32();
        dwDrawCount = ReadUInt32();
        dwFleeCount = ReadUInt32();

        dwGameLogonTimes = ReadUInt32();
        cbInsureEnabled = ReadByte();
        cbIsAgent = ReadByte();

        spreaderId = ReadUInt32();
        spreaderName = ReadString(64);
    }
}

/// 登录失败 100-101
public class CMD_Hall_S_LoginFail : DataBase
{ 
    public long lResultCode;					//错误代码
	public string szDescribeString;				//描述消息

    public CMD_Hall_S_LoginFail()
    {
    }

    protected override void UnPackBody()
    {
        lResultCode = ReadInt();
        szDescribeString = ReadString(256);
    }
}

/// 注销登录 100-7
public class CMD_Hall_C_LoginOut : DataBase
{
    public UInt32 dwUserID;							//用户标识
    public UInt32 dwGameID;							//游戏标识

    public CMD_Hall_C_LoginOut()
    {
        header.wMainCmdID = 100;
        header.wSubCmdID = 7;
    }

    protected override void PackBody()
    {
        dwUserID = ReadUInt32();
        dwGameID = ReadUInt32();
    }
}

/// 游戏分类排序列表信息
public class CMD_Hall_S_GameKindList : DataBase
{ 
    public int count = 0;
    public List<GameKindInfo> gameKindList = new List<GameKindInfo>();

    protected override void UnPackBody()
    {
        gameKindList.Clear();
        count = (header.wPacketSize - 8)/155;
        for (int i = 0; i < count; i++)
        {
            GameKindInfo info = new GameKindInfo();
            info.wTypeID = ReadUInt16();
            info.wJoinID = ReadUInt16();
            info.wSortID = ReadUInt16();
            info.wKindID = ReadUInt16();
            info.wGameID = ReadUInt16();
            info.cbSuportType = ReadByte();
            info.wRecommend = ReadUInt16();
            info.wGameFlag = ReadUInt16();
            info.dwOnLineCount = ReadUInt32();
            info.dwAndroidCount = ReadUInt32();
            info.dwFullCount = ReadUInt32();
            info.szKindName = ReadString(64);
            info.szProcessName = ReadString(64);

            gameKindList.Add(info);
        }
    }
}

/// 服务器<房间>列表信息
public class CMD_Hall_S_GameServerList : DataBase
{
    public int serverCount;
    public List<GameServerInfo> serverList = new List<GameServerInfo>();

    protected override void UnPackBody()
    {
        serverList.Clear();
        serverCount = (header.wPacketSize - 8) / 177;
        for (int i = 0; i < serverCount; i++)
        {
            GameServerInfo server = new GameServerInfo();
            server.wKindID = ReadUInt16();
            server.wNodeID = ReadUInt16();
            server.wSortID = ReadUInt16();
            server.wServerID = ReadUInt16();
            server.wServerKind = ReadUInt16();
            server.wServerType = ReadUInt16();
            server.wServerLevel = ReadUInt16();
            server.wServerPort = ReadUInt16();
            server.lCellScore = ReadInt64();
            server.wRatio = ReadInt16();
            server.serviceMoney = ReadInt64();
            server.cbEnterMember = ReadByte();
            server.lEnterScore = ReadInt64();
            server.dwServerRule = ReadUInt32();
            server.dwOnLineCount = ReadUInt32();
            server.dwAndroidCount = ReadUInt32();
            server.dwFullCount = ReadUInt32();
            server.szServerAddr = ReadString(64);
            server.szServerName = ReadString(64);

            serverList.Add(server);
        }
    }
}

#endregion

#region 绑定手机

/// 获取绑定验证码 3-400
public class CMD_Hall_C_GetBindPhoneCode : DataBase
{
    public UInt32 dwUserID;					//用户 I D
    public string szPhoneNo = "";			//手机号码 - 24

    public CMD_Hall_C_GetBindPhoneCode()
    {
        header.wMainCmdID = 3;
        header.wSubCmdID = 400;
    }

    protected override void PackBody()
    {
 	    WriteUInt32(dwUserID);
        WriteString(szPhoneNo, 24);
    }
}

/// 绑定操作 3-401
public class CMD_Hall_C_BindPhoneOp : DataBase
{
    public UInt32 dwUserID;						//用户 I D
    public string szPhoneNo = "";					//手机号码 - 24
    public string szCode = "";						//验证码 - 14

    public CMD_Hall_C_BindPhoneOp()
    {
        header.wMainCmdID = 3;
        header.wSubCmdID = 401;
    }

    protected override void PackBody()
    {
 	    WriteUInt32(dwUserID);
        WriteString(szPhoneNo, 24);
        WriteString(szCode, 14);
    }
}

/// 绑定成功 3-402
public class CMD_Hall_S_BindSuccess : DataBase
{
    public string szPhoneNo;            //手机号码 - 24
    
    protected override void UnPackBody()
    {
 	    szPhoneNo = ReadString(24);
    }
}

#endregion

#region 抢红包

/// 发放彩金 8-100
public class CMD_Hall_S_Handsel : DataBase
{
    public UInt32 dwUserID;			//用户标识
	public Int64 lMoney;		    //彩金额度
	public string szNickName;		//用户昵称 64
	public string szMessage;		//系统消息 128

    protected override void UnPackBody()
    {
        dwUserID = ReadUInt32();
        lMoney = ReadInt64();
        szNickName = ReadString(64);
        szMessage = ReadString(128);
    }
}

/// 抢红包通知 8-101
public class CMD_Hall_S_RedEnvelopeNotify : DataBase
{
    public string redEnvelopeId;        //红包id

    protected override void UnPackBody()
    {
 	     redEnvelopeId = ReadString(64);
    }
}

/// 拆红包 3-600
public class CMD_Hall_C_OpenRedEnvelope : DataBase
{
    public UInt32 dwUserID;						//用户 I D
    public string szHBid = "";						//红包 I D

    public CMD_Hall_C_OpenRedEnvelope()
    {
        header.wMainCmdID = 3;
        header.wSubCmdID = 600;
    }

    protected override void PackBody()
    {
 	    dwUserID = ReadUInt32();
        szHBid = ReadString(64);
    }
}

/// 拆红包结果 3-602
public class CMD_Hall_S_OpenRedEnvelopeResult : DataBase
{
    public UInt32 dwUserID;					//用户 I D
	public Int64 lMoney;					//金额
	public int lResultCode;					//结果代码
    public string szDescribeString = "";	        //错误信息 - 256

    protected override void UnPackBody()
    {
 	    dwUserID = ReadUInt32();
        lMoney = ReadInt64();
        lResultCode = ReadInt();
        szDescribeString = ReadString(256);
    }
}

/// 查询抢红包结果 3-601
public class CMD_Hall_C_GetRedEnvelopeRecord : DataBase
{
    public UInt32 dwUserID;         //用户ID
    public string szHBid = "";           //红包ID

    public CMD_Hall_C_GetRedEnvelopeRecord()
    {
        header.wMainCmdID = 3;
        header.wSubCmdID = 601;
    }

    protected override void PackBody()
    {
 	    WriteUInt32(dwUserID);
        WriteString(szHBid, 64);
    }
}

/// 抢红包结果 3-603
public class CMD_Hall_S_RedEnvelopeRecord : DataBase
{
    public UInt32 dwUserID;							//用户 I D
	public string[] szNickName = new string[10];		//Top10  昵称 - 64
	public Int64[] lMoney = new Int64[10];		

    protected override void UnPackBody()
    {
 	    dwUserID = ReadUInt32();
        for(int i = 0; i < 10; i++)
        {
            szNickName[i] = ReadString(64);
        }
        for(int i = 0; i < 10; i++)
        {
            lMoney[i] = ReadInt64();
        }
    }
}

#endregion

#region 用户资料

/// 修改用户密码 3-101
public class CMD_Hall_C_ModifyUserPwd : DataBase
{ 
    public UInt32 dwUserID;						    //用户 I D
    public string szDesPassword = "";		            //用户密码 66(新)
    public string szScrPassword = "";		            //用户密码 66(旧)

    public CMD_Hall_C_ModifyUserPwd()
    {
        header.wMainCmdID = 3;
        header.wSubCmdID = 101;
    }

    protected override void PackBody()
    {
        WriteUInt32(dwUserID);
        WriteString(szDesPassword, 66);
        WriteString(szScrPassword, 66);
    }
}

/// 修改保险箱密码 3-102
public class CMD_Hall_C_ModifyBankPwd : DataBase
{
    public UInt32 dwUserID;						        //用户 I D
    public string szDesPassword = "";		                //用户密码
    public string szScrPassword = "";		                //用户密码

    public CMD_Hall_C_ModifyBankPwd()
    {
        header.wMainCmdID = 3;
        header.wSubCmdID = 102;
    }

    protected override void PackBody()
    {
        WriteUInt32(dwUserID);
        WriteString(szDesPassword, 66);
        WriteString(szScrPassword, 66);
    }
}

/// 修改个性签名 3-103
public class CMD_Hall_C_ModifySignature : DataBase
{ 
    public UInt32 dwUserID;				//用户 I D
    public string szPassword = "";			//用户密码 66
    public string szUnderWrite = "";		    //个性签名 128

    public CMD_Hall_C_ModifySignature()
    {
        header.wMainCmdID = 3;
        header.wSubCmdID = 103;
    }

    protected override void PackBody()
    {
        WriteUInt32(dwUserID);
        WriteString(szPassword, 66);
        WriteString(szUnderWrite, 128);
    }
}

/// 实名认证 3-154
public class CMD_Hall_C_RealAuth : DataBase
{
    public UInt32 dwUserID;				//用户 I D
    public string szPassword;			    //用户密码 66
    public string szCompellation;	        //真实名字 32
    public string szPassPortID;		    //证件号码 38

    public CMD_Hall_C_RealAuth()
    {
        header.wMainCmdID = 3;
        header.wSubCmdID = 154;
    }

    protected override void PackBody()
    {
        WriteUInt32(dwUserID);
        WriteString(szPassword, 66);
        WriteString(szCompellation, 32);
        WriteString(szPassPortID, 38);
    }
}

/// 修改头像 3-122
public class CMD_Hall_C_ModifySystemFaceInfo : DataBase
{
    public UInt16 wFaceID;			    //头像标识
    public UInt32 dwUserID;			    //用户 I D
    public string szPassword = "";	    //用户密码 66
    public string szMachineID = "";	    //机器序列 66

    public CMD_Hall_C_ModifySystemFaceInfo()
    {
        header.wMainCmdID = 3;
        header.wSubCmdID = 122;
    }

    protected override void PackBody()
    {
        WriteUInt16(wFaceID);
        WriteUInt32(dwUserID);
        WriteString(szPassword, 66);
        WriteString(szMachineID, 66);
    }
}

/// （修改）头像信息 3-120
public class CMD_Hall_S_FaceInfo : DataBase
{
    public UInt16 wFaceID;						//头像标识
    public UInt32 dwCustomID;					//自定标识

    protected override void UnPackBody()
    {
        wFaceID = ReadUInt16();
        dwCustomID = ReadUInt32();
    }
}

//认证结果
public class CMD_Hall_S_IndividuaResult : DataBase
{
	public byte	bSuccessed;							//成功标识
	public Int64 lCurrScore;						//当前游戏币
	public string szNotifyContent;				    //提示内容 256

    protected override void UnPackBody()
    {
        bSuccessed = ReadByte();
        lCurrScore = ReadInt64();
        szNotifyContent = ReadString(256);
    }
}


//修改用户资料 3-152
public class CMD_Hall_C_ModifyUserInfo : DataBase
{ 
    //验证资料
	public UInt32 dwUserID;							//用户ID
	public UInt32 dwClientAddr;						//连接地址
    public UInt32 dwSpreaderGameID;			//推荐人ID
	public string szPassword = "";			            //用户密码 66

	public byte	cbGender;							//用户性别
    public string szNickName = "";			            //用户昵称 64
    public string szUnderWrite = "";		                //个性签名 128

    public string szUserNote = "";		                //用户说明 512
    public string szCompellation = "";	                //真实名字 32
    public string szPassPortID = "";		                //证件号码 38
    //public string szSpreader = "";			            //推荐帐号 64
    public string szWeChatURL = "";		                //微信头像URL 512

    public string szSeatPhone = "";		                //固定电话 66
    public string szMobilePhone = "";	                //移动电话 24

    public string szQQ = "";						        //QQ号码 32
    public string szEMail = "";					        //电子邮件 66
    public string szDwellingPlace = "";                  //联系地址 256

    public string szCustomData = "";		                //微信URL-自定义数据 1024 

    public CMD_Hall_C_ModifyUserInfo()
    {
        header.wMainCmdID = 3;
        header.wSubCmdID = 152;
    }

    protected override void PackBody()
    {
        WriteUInt32(dwUserID);
        WriteUInt32(dwClientAddr);
        WriteUInt32(dwSpreaderGameID);
        WriteString(szPassword, 66);

        WriteByte(cbGender);
        WriteString(szNickName, 64);
        WriteString(szUnderWrite, 128);

        WriteString(szUserNote, 512);
        WriteString(szCompellation, 32);
        WriteString(szPassPortID, 38);
        //WriteString(szSpreader, 64);
        WriteString(szWeChatURL, 512);

        WriteString(szSeatPhone, 66);
        WriteString(szMobilePhone, 24);

        WriteString(szQQ, 32);
        WriteString(szEMail, 66);
        WriteString(szDwellingPlace, 256);

        WriteString(szCustomData, 1024);
    }

}

//用户信息
public class CMD_Hall_C_QueryUserInfo : DataBase
{ 
    public UInt32 dwUserID;				//用户 I D
	public string szPassword;			//用户密码 66

    public CMD_Hall_C_QueryUserInfo()
    {
        header.wMainCmdID = 3;
        header.wSubCmdID = 141;
    }

    protected override void PackBody()
    {
        WriteUInt32(dwUserID);
        WriteString(szPassword, 66);
    }
}

//用户信息
public class CMD_Hall_S_QueryUserInfo : DataBase
{ 
    public UInt32 dwUserID;					//用户 I D
	public string szUserNote;			    //用户说明 512
	public string szCompellation;	        //真实名字 32
	public string szPassPortID;		        //证件号码 38
	public string szUnderWrite;		        //个性签名 128
	public string szWeChatURL;		        //微信URL 512
            
	public string szSeatPhone;		        //固定电话 66
	public string szMobilePhone;	        //移动电话 24
            
	public string szQQ;						//Q Q 号码 32
	public string szEMail;					//电子邮件 66
	public string szDwellingPlace;          //联系地址 256
            
	public string szSpreader;			    //推广信息 64
            
	public string szCustomData;		        //自定义数据 1024

    protected override void UnPackBody()
    {
        dwUserID = ReadUInt32();
        szUserNote = ReadString(512);
        szCompellation = ReadString(32);
        szPassPortID = ReadString(38);
        szUnderWrite = ReadString(128);
        szWeChatURL	= ReadString(512);

        szSeatPhone = ReadString(66);
        szMobilePhone = ReadString(24);

        szQQ = ReadString(32);
        szEMail = ReadString(66);
        szDwellingPlace = ReadString(256);

        szSpreader = ReadString(64);

        szCustomData = ReadString(1024);
    }

}


#endregion

#region 签到

/// <summary>
/// 查询签到 3-220
/// </summary>
public class CMD_Hall_C_QuerySignInfo : DataBase
{
    public UInt32 dwUserID;				//用户标识
    public string szPassword = "";			//登录密码 66

    public CMD_Hall_C_QuerySignInfo()
    {
        header.wMainCmdID = 3;
        header.wSubCmdID = 220;
    }

    protected override void PackBody()
    {
        WriteUInt32(dwUserID);
        WriteString(szPassword, 66);
    }
}

/// <summary>
/// 签到信息 3-221
/// </summary>
public class CMD_Hall_S_SignInfo : DataBase
{
    public UInt16 wSeriesDate;						//连续日期
	public bool	bTodayChecked;						//签到标识
    public TagCheckInItem [] RewardCheckIn = new TagCheckInItem[10];
    public TagSeriesCheckInReward [] SeriesRewardInfo = new TagSeriesCheckInReward[5];

    protected override void UnPackBody()
    {
        wSeriesDate = ReadUInt16();
        bTodayChecked = (ReadByte() == 1);
        for(int i = 0; i <  RewardCheckIn.Length; ++ i)
        {
            RewardCheckIn[i] = new TagCheckInItem();
            RewardCheckIn[i].nConfigID = ReadInt();
            RewardCheckIn[i].nPropID = ReadInt();
            RewardCheckIn[i].nGiveCount = ReadInt();
            RewardCheckIn[i].cbBigReward = ReadByte();
        }
        for(int i = 0; i < SeriesRewardInfo.Length; ++ i)
        {
            SeriesRewardInfo[i] = new TagSeriesCheckInReward();
            SeriesRewardInfo[i].nSeriesDays = ReadInt();
            SeriesRewardInfo[i].RewardItem.nConfigID = ReadInt();
            SeriesRewardInfo[i].RewardItem.nPropID = ReadInt();
            SeriesRewardInfo[i].RewardItem.nGiveCount = ReadInt();
            SeriesRewardInfo[i].RewardItem.cbBigReward = ReadByte();
        }
    }
}

/// <summary>
/// 签到 3-222
/// </summary>
public class CMD_Hall_C_SignIn : DataBase
{
    public UInt32 dwUserID;				//用户标识
    //public string szPassword = "";			//登录密码 66
    public string szMachineID = "";		    //机器序列 66

    public CMD_Hall_C_SignIn()
    {
        header.wMainCmdID = 3;
        header.wSubCmdID = 222;
    }

    protected override void PackBody()
    {
        WriteUInt32(dwUserID);
        //WriteString(szPassword, 66);
        WriteString(szMachineID, 66);
    }
}

/// <summary>
/// 签到结果 3-223
/// </summary>
public class CMD_Hall_S_SignInResult : DataBase
{
    public bool	bSuccessed;                         //成功标识
    public int nResultIndex;                       //中奖索引
    public int nSeriesDays;                         //连续天数
    public int nConfigID;                              //当前道具
    public int nAddConfigID;                               //当前道具
    public string szNotifyContent = "";				//提示内容 256

    protected override void UnPackBody()
    {
        bSuccessed = (ReadByte() == 1);
        nResultIndex = ReadInt();
        nSeriesDays = ReadInt();
        nConfigID = ReadInt();
        nAddConfigID = ReadInt();
        szNotifyContent = ReadString(256);
    }
}

#endregion

#region 银行

/// <summary>
/// 打开银行 3-104
/// </summary>
public class CMD_Hall_C_LoginBank : DataBase
{ 
    public UInt32	dwUserID;			//用户标识
    public string szPassword = "";			//用户密码 66

    public CMD_Hall_C_LoginBank()
    {
        header.wMainCmdID = 3;
        header.wSubCmdID = 104;
    }

    protected override void PackBody()
    {
        WriteUInt32(dwUserID);
        WriteString(szPassword, 66);
    }
}

/// <summary>
/// 查询银行 3-165
/// </summary>
public class CMD_Hall_C_QueryBank : DataBase
{
    public UInt32 dwUserID;                     //用户 I D
    public string szPassword = "";				//登录密码 33*2

    public CMD_Hall_C_QueryBank()
    {
        header.wMainCmdID = 3;
        header.wSubCmdID = 165;
    }

    protected override void PackBody()
    {
        WriteUInt32(dwUserID);
        WriteString(szPassword, 66);
    }
}

/// <summary>
/// 银行信息 3-164
/// </summary>
public class CMD_Hall_S_BankInfo : DataBase
{
    public byte cbEnjoinTransfer;                  //转账开关
    public UInt16 wRevenueTake;                    //税收比例
    public UInt16 wRevenueTransfer;                //税收比例
    public UInt16 wRevenueTransferMember;          //税收比例
    public UInt16 wServerID;                       //房间标识
    public Int64 lUserScore;                       //用户金币
    public Int64 lUserIngot;				       //用户元宝
    public Int64 lUserRoomCard;                    //用户房卡
    public Int64 lUserRedRevelopes;				   //用户红包
    public Int64 lUserInsure;                      //银行金币
    public Int64 lTransferPrerequisite;			   //转账条件

    protected override void UnPackBody()
    {
        cbEnjoinTransfer = ReadByte();
        wRevenueTake = ReadUInt16();
        wRevenueTransfer = ReadUInt16();
        wRevenueTransferMember = ReadUInt16();
        wServerID = ReadUInt16();
        lUserScore = ReadInt64();
        lUserIngot = ReadInt64();
        lUserRoomCard = ReadInt64();
        lUserRedRevelopes = ReadInt64();
        lUserInsure = ReadInt64();
        lTransferPrerequisite = ReadInt64();
    }
}

/// <summary>
/// 开通银行 3-160
/// </summary>
public class CMD_Hall_C_EnableBank : DataBase
{ 
    public UInt32	dwUserID;					//用户I D
    public string szLogonPass = "";			    //登录密码 66
    public string szInsurePass = "";			//银行密码 66
    public string szMachineID = "";		        //机器序列 66

    public CMD_Hall_C_EnableBank()
    {
        header.wMainCmdID = 3;
        header.wSubCmdID = 160;
    }

    protected override void PackBody()
    {
 	    WriteUInt32(dwUserID);
        WriteString(szLogonPass, 66);
        WriteString(szInsurePass, 66);
        WriteString(szMachineID, 66);
    }
}

/// <summary>
/// 开通银行结果 3-170
/// </summary>
public class CMD_Hall_S_EnableBankResult : DataBase
{ 
    public byte cbInsureEnabled;					    //使能标识
	public string szDescribeString;				        //描述消息 256

    protected override void UnPackBody()
    {
        cbInsureEnabled = ReadByte();
        szDescribeString = ReadString(256);
    }
}

/// <summary>
/// 银行操作 - 存钱  3-161
/// </summary>
public class CMD_Hall_C_BankSave : DataBase
{ 
    public UInt32 dwUserID;			//用户 I D
	public Int64 lSaveScore;		//存入金币
    public string szMachineID = "";		//机器序列

    public CMD_Hall_C_BankSave()
    {
        header.wMainCmdID = 3;
        header.wSubCmdID = 161;
    }

    protected override void PackBody()
    {
        WriteUInt32(dwUserID);
        WriteInt64(lSaveScore);
        WriteString(szMachineID, 66);
    }
}

/// <summary>
/// 银行操作 - 取钱 3-162
/// </summary>
public class CMD_Hall_C_BankGet : DataBase
{
    public UInt32 dwUserID;							//用户 I D
    public Int64 lTakeScore;							//提取金币
    public string szPassword = "";				            //银行密码 66
    public string szMachineID = "";		                    //机器序列 66

    public CMD_Hall_C_BankGet()
    {
        header.wMainCmdID = 3;
        header.wSubCmdID = 162;
    }

    protected override void PackBody()
    {
        WriteUInt32(dwUserID);
        WriteInt64(lTakeScore);
        WriteString(szPassword, 66);
        WriteString(szMachineID, 66);
    }
}

/// <summary>
/// 银行操作 - 赠送 3-163
/// </summary>
public class CMD_Hall_C_BankSend : DataBase
{ 
    public UInt32 dwUserID;					    //用户 userID
    public UInt32 dwTargetUserID;               //目标用户userID 
	public Int64 lTransferScore;			    //转账金币
    public string szPassword = "";				//银行密码 66
    public string dynamicPwd = "";              //动态密码 66
    public string szMachineID = "";		        //机器序列 66
    public string szTransRemark = "";	        //转账备注 64

    public CMD_Hall_C_BankSend()
    {
        header.wMainCmdID = 3;
        header.wSubCmdID = 163;
    }

    protected override void PackBody()
    {
        WriteUInt32(dwUserID);
        WriteUInt32(dwTargetUserID);
        WriteInt64(lTransferScore);
        WriteString(szPassword, 66);
        WriteString(dynamicPwd, 66);
        WriteString(szMachineID, 66);
        WriteString(szTransRemark, 64);
    }
}

/// <summary>
/// 银行操作 - 成功 3-166
/// </summary>
public class CMD_Hall_S_BankOpSuccess : DataBase
{
    public UInt32 dwUserID;						//用户 I D
    public Int64 lUserScore;					//用户金币
    public Int64 lUserInsure;					//银行金币
    public TimeStruct opTime = new TimeStruct();			//操作时间
    public string szDescribeString;		        //描述消息 256

    protected override void UnPackBody()
    {
        dwUserID = ReadUInt32();
        lUserScore = ReadInt64();
        lUserInsure = ReadInt64();
        opTime.wYear = ReadUInt16();
        opTime.wMonth = ReadUInt16();
        opTime.wDayOfWeek = ReadUInt16();
        opTime.wDay = ReadUInt16();
        opTime.wHour = ReadUInt16();
        opTime.wMinute = ReadUInt16();
        opTime.wSecond = ReadUInt16();
        opTime.wMilliseconds = ReadUInt16();
        szDescribeString = ReadString(256);
    }
}

/// <summary>
/// 银行操作 - 失败 3 - 167
/// </summary>
public class CMD_Hall_S_BankOpFail : DataBase
{
    public int lResultCode;						//错误代码
    public string szDescribeString;				//描述消息 256

    protected override void UnPackBody()
    {
        lResultCode = ReadInt();
        szDescribeString = ReadString(256);
    }
}

/// <summary>
/// 查询用户信息 3-168 
/// </summary>
public class CMD_Hall_C_QuerySendUserInfo : DataBase
{ 
    public byte cbByNickName;               //昵称赠送
    public UInt32 dwTargetGameID;           //目标用户gameId
    public string szAccounts = "";			//目标用户 64

    public CMD_Hall_C_QuerySendUserInfo()
    {
        header.wMainCmdID = 3;
        header.wSubCmdID = 168;
    }

    protected override void PackBody()
    {
        WriteByte(cbByNickName);
        WriteUInt32(dwTargetGameID);
        WriteString(szAccounts, 64);
    }
}

/// <summary>
/// 用户信息 3-169
/// </summary>
public class CMD_Hall_S_QuerySendUserInfoResult : DataBase
{
    public UInt32 dwTargetUserID;       //目标用户userId
    public UInt32 dwTargetGameID;		//目标用户gameId   
	public string szNickName;			//目标用户昵称    64

    protected override void UnPackBody()
    {
        dwTargetUserID = ReadUInt32();
        dwTargetGameID = ReadUInt32();
        szNickName = ReadString(64);
    }
}

#endregion

#region 兑换

/// <summary>
/// 兑换游戏币 3-324
/// </summary>
public class CMD_Hall_C_ExchangeCoin : DataBase
{
    public UInt32 dwUserID;							//用户标识
    public Int64 lExchangeIngot;					//元宝数量
    public string szMachineID = "";		            //机器标识 66


    public CMD_Hall_C_ExchangeCoin()
    {
        header.wMainCmdID = 3;
        header.wSubCmdID = 324;
    }

    protected override void PackBody()
    {
        WriteUInt32(dwUserID);
        WriteInt64(lExchangeIngot);
        WriteString(szMachineID, 66);
    }
}

/// <summary>
/// 兑换结果 3-326
/// </summary>
public class CMD_Hall_S_ExchangeResult : DataBase
{
    public bool bSuccessed;							//成功标识
    public Int64 lCurrScore;						//当前游戏币
    public Int64 bankScore;                         //保险箱金币
    public Int64 lCurrIngot;						//当前元宝
    public double dCurrBeans;						//当前游戏豆
    public string szNotifyContent;				    //提示内容 256

    protected override void UnPackBody()
    {
        bSuccessed = (ReadByte() == 1);
        lCurrScore = ReadInt64();
        bankScore = ReadInt64();
        lCurrIngot = ReadInt64();
        dCurrBeans = ReadDouble();
        szNotifyContent = ReadString(256);
    }
}

#endregion

#region 系统操作

/// <summary>
/// 操作成功 3-500
/// </summary>
public class CMD_Hall_S_OpSuccess : DataBase
{
    public int opCode = 0;
    public int lResultCode;						//操作代码
    public string szDescribeString;				//成功消息 256

    protected override void UnPackBody()
    {
        opCode = ReadInt();
        lResultCode = ReadInt();
        szDescribeString = ReadString(256);
    }
}

/// <summary>
/// 操作失败 3-501
/// </summary>
public class CMD_Hall_S_OpFail : DataBase
{
    public int opCode = 0;
    public int lResultCode;						//错误代码
    public string szDescribeString;				//描述消息 256

    protected override void UnPackBody()
    {
        opCode = ReadInt();
        lResultCode = ReadInt();
        szDescribeString = ReadString(256);
    }
}

/// <summary>
/// 系统消息 - 管理员消息 8-102
/// </summary>
public class CMD_Hall_S_SystemMessage : DataBase
{
    public byte cbGame;								//游戏消息
    public byte cbRoom;								//游戏消息
    public byte cbAllRoom;							//游戏消息
    public byte cbLoop;                             //循环标志
    public UInt32 dwTimeRate;                       //循环间隔
    public Int64 tConcludeTime;                     //结束时间
    public UInt16 wChatLength;						//信息长度
    public string szSystemMessage;		            //系统消息 256

    protected override void UnPackBody()
    {
        cbGame = ReadByte();
        cbRoom = ReadByte();
        cbAllRoom = ReadByte();
        cbLoop = ReadByte();
        dwTimeRate = ReadUInt32();
        tConcludeTime = ReadInt64();
        wChatLength = ReadUInt16();
        szSystemMessage = ReadString(256);
    }
}

/// <summary>
/// 系统消息 - 全局消息
/// </summary>
public class CMD_Hall_S_SystemInfo : DataBase
{
    public UInt16 wType;        //消息类型
    public UInt16 wLength;      //消息长度
    public string szString;     //消息内容，1024

    protected override void UnPackBody()
    {
        wType = ReadUInt16();
        wLength = ReadUInt16();
        szString = ReadString(wLength * 2);
    }

    //类型掩码
    //#define SMT_CHAT					0x0001								//聊天消息
    //#define SMT_EJECT					0x0002								//弹出消息
    //#define SMT_GLOBAL				0x0004								//全局消息
    //#define SMT_PROMPT				0x0008								//提示消息
    //#define SMT_TABLE_ROLL			0x0010								//滚动消息

    //控制掩码
    //#define SMT_CLOSE_ROOM			0x0100								//关闭房间
    //#define SMT_CLOSE_GAME			0x0200								//关闭游戏
    //#define SMT_CLOSE_LINK			0x0400								//中断连接
    //#define SMT_CLOSE_INSURE			0x0800								//关闭银行
    //#define SMT_CLOSE_PLAZA			0x1000								//关闭大厅

}

/// <summary>
/// 系统消息 - 大喇叭消息
/// </summary>
public class CMD_Hall_S_HornMessage : DataBase
{ 
    public UInt16 wPropertyIndex;                   //道具索引 
	public UInt32 dwSendUserID;                     //用户 I D
	public UInt32 TrumpetColor;                     //喇叭颜色
	public string szSendNickName;				    //玩家昵称 64
	public string szTrumpetContent;                 //喇叭内容 256

    protected override void UnPackBody()
    {
        wPropertyIndex = ReadUInt16();
        dwSendUserID = ReadUInt32();
        TrumpetColor = ReadUInt32();
        szSendNickName = ReadString(64);
        szTrumpetContent = ReadString(256);
    }
}

#endregion

#region 分享

/// <summary>
/// 分享游戏 3-702
/// </summary>
public class CMD_Hall_C_ShareGame : DataBase
{
    public string szMachineID = "";

    public CMD_Hall_C_ShareGame()
    {
        header.wMainCmdID = 3;
        header.wSubCmdID = 702;
    }

    protected override void PackBody()
    {
        WriteString(szMachineID, 66);
    }
}

/// <summary>
/// 分享游戏成功  3-712
/// </summary>
public class CMD_Hall_S_ShareResult : DataBase
{
    public Int64 lScore;                           //奖励分数
    public string szDescribeString = "";                //描述信息 256

    protected override void UnPackBody()
    {
        lScore = ReadInt64();
        szDescribeString = ReadString(256);
    }
}

#endregion

#region 断线续连

/// <summary>
/// 取得玩家当前游戏状态
/// </summary>
public class CMD_Hall_C_GetUserGameState : DataBase
{ 
    public UInt32 dwGameID;						//游戏标识
	public string nickName = "";			    //用户昵称 64

    public CMD_Hall_C_GetUserGameState()
    {
        header.wMainCmdID = 4;
        header.wSubCmdID = 102;
    }

    protected override void PackBody()
    {
        WriteUInt32(dwGameID);
        WriteString(nickName, 64);
    }
}

/// <summary>
/// 玩家当前游戏状态
/// </summary>
public class CMD_Hall_S_GetUserGameState : DataBase
{
    public UInt16 userCount;        //玩家数量
    public PlayerStateInGame[] players = new PlayerStateInGame[16];

    protected override void UnPackBody()
    {
        userCount = ReadUInt16();
        for (int i = 0; i < userCount; i++)
        {
            PlayerStateInGame player = new PlayerStateInGame();
            player.dwUserID = ReadUInt32();
            player.dwGameID = ReadUInt32();
            player.szNickName = ReadString(64);
            player.wFaceID = ReadUInt16();

            player.cbGender = ReadByte();
            player.cbMemberOrder = ReadByte();
            player.cbMasterOrder = ReadByte();

            player.wKindID = ReadUInt16();
            player.wServerID = ReadUInt16();
            player.szGameServer = ReadString(32);

            player.wTableID = ReadUInt16();
            player.wLastTableID = ReadUInt16();
            player.wChairID = ReadUInt16();
            player.cbUserStatus = ReadByte();

            players[i] = player;
        }
    }
}

#endregion

#region 房卡相关

/// <summary>
/// 房卡游戏服务器信息
/// </summary>
public class CMD_Hall_S_CardGameServer : DataBase
{
    public int serverCount;
    public List<CardGameServerInfo> cardServerList = new List<CardGameServerInfo>();

    protected override void UnPackBody()
    {
        cardServerList.Clear();
        serverCount = (header.wPacketSize - 8) / 74;
        for (int i = 0; i < serverCount; i++)
        {
            CardGameServerInfo cardServer = new CardGameServerInfo();

            cardServer.dwUserID = ReadUInt32();
            cardServer.wKindID = ReadUInt16();
            cardServer.wServerID = ReadUInt16();
            cardServer.dwVersion = ReadUInt32();
            cardServer.dwRoomID = ReadUInt32();
            cardServer.dwRoomNum = ReadUInt32();
            cardServer.dwFanTimes = ReadUInt32();
            cardServer.dwMaxTimes = ReadUInt32();
            cardServer.dwGameRule = ReadUInt32();
            cardServer.cbRoomType = ReadByte();
            for (int j = 0; j < 4; j++)
            {
                cardServer.cbPlayCout[j] = ReadByte();
            }
            for (int j = 0; j < 4; j++)
            {
                cardServer.lPlayCost[j] = ReadInt64();
            }
            for (int j = 0; j < 4; j++)
            {
                cardServer.lAAPlayCost[j] = ReadInt64();
            }
            cardServer.lCostGold = ReadInt64();
            cardServer.cbPlayCountIndex = ReadByte();

            cardServerList.Add(cardServer);
        }
    }
}

/// <summary>
/// 创建/加入房间时，取得服务器信息
/// </summary>
public class CMD_Hall_C_GetCardRoomServerInfo : DataBase
{

    public UInt16 wKindID;								//游戏类型
    public UInt16 wServerKind;                          //房间类型
    public UInt16 wServerLevel = 1;						//服务等级 0-练习场 1-初级 2-中级 3-高级
    public byte cbRoomType;				//房间类型	
    public UInt32 dwRoomNumber;							//房间号码

    public CMD_Hall_C_GetCardRoomServerInfo()
    {
        header.wMainCmdID = 4;
        header.wSubCmdID = 101;
    }

    protected override void PackBody()
    {
        WriteUInt16(wKindID);
        WriteUInt16(wServerKind);
        WriteUInt16(wServerLevel);
        WriteByte(cbRoomType);
        WriteUInt32(dwRoomNumber);
    }

}

/// <summary>
/// 创建/加入 房间服务器信息
/// </summary>
public class CMD_Hall_S_CardRoomServerInfo : DataBase
{
    public UInt16 wServerID;				//房间标识
    public string szDescribeString;        //描述信息 256

    protected override void UnPackBody()
    {
        wServerID = ReadUInt16();
        szDescribeString = ReadString(256);
    }
}

/// <summary>
/// 取得游戏记录
/// </summary>
public class CMD_Hall_C_GetGameRecord : DataBase
{
    public UInt16 kindId;

    public CMD_Hall_C_GetGameRecord()
    {
        header.wMainCmdID = 3;
        header.wSubCmdID = 700;
    }

    protected override void PackBody()
    {
        WriteUInt16(kindId);
    }
}

/// <summary>
/// 游戏记录信息
/// </summary>
public class CMD_Hall_S_GameRecord : DataBase
{
    public UInt16 wKindID;								//游戏类型
	public int dwPrivateDrawID;							//记录标识
    public int dwRoomNumber;							//房间号码																	
    public int[] dwUserID = new int[10];				//用户 I D
    public int dwGameCount;								//游戏局数
	public Int64[] lUserScore = new Int64[10];			//用户得分
    public int[] dwCreateUser = new int[10];			//创建标识
    public int[] dwWinCount = new int[10];			    //输赢场次
	public string[] szUserNickName = new string[10];	//用户昵称 64
	public TimeStruct InsertTime = new TimeStruct();	//时间信息							



    protected override void UnPackBody()
    {
        wKindID = ReadUInt16();
        dwPrivateDrawID = ReadInt();
        dwRoomNumber = ReadInt();
        dwUserID = ReadIntArray(10);
        dwGameCount = ReadInt();
        lUserScore = ReadInt64Ary(10);
        dwCreateUser = ReadIntArray(10);
        dwWinCount = ReadIntArray(10);
        for (int i = 0; i < 10; i++)
        {
            szUserNickName[i] = ReadString(64);
        }

        InsertTime.wYear = ReadUInt16();
        InsertTime.wMonth = ReadUInt16();
        InsertTime.wDayOfWeek = ReadUInt16();
        InsertTime.wDay = ReadUInt16();
        InsertTime.wHour = ReadUInt16();
        InsertTime.wMinute = ReadUInt16();
        InsertTime.wSecond = ReadUInt16();
        InsertTime.wMilliseconds = ReadUInt16();
    }

}

/// <summary>
/// 获取游戏记录详情
/// </summary>
public class CMD_Hall_C_GetRecordInfo : DataBase
{
    public UInt32 dwPrivateDrawID;	 //记录标识

    public CMD_Hall_C_GetRecordInfo()
    {
        header.wMainCmdID = 3;
        header.wSubCmdID = 701;
    }

    protected override void PackBody()
    {
        WriteUInt32(dwPrivateDrawID);
    }
}

/// <summary>
/// 游戏记录详情
/// </summary>
public class CMD_Hall_S_RecordInfo : DataBase
{
    public UInt32 dwPrivateDrawID;                  //记录标识
    public UInt32[] dwUserID = new UInt32[10];      //用户 I D
    public UInt32 dwGameCount;                      //游戏局数
    public Int64[] lUserScore = new Int64[10];      //用户得分
    public UInt32[] dwCreateUser = new UInt32[10];  //创建标识
    public UInt32[] dwWinCount = new UInt32[10];    //输赢场次
    public string[] szUserNickName = new string[10];	 //用户昵称 64

    protected override void UnPackBody()
    {
        dwPrivateDrawID = ReadUInt32();
        for (int i = 0; i < 10; i++)
        {
            dwUserID[i] = ReadUInt32();
        }
        dwGameCount = ReadUInt32();
        for (int i = 0; i < 10; i++)
        {
            lUserScore[i] = ReadInt64();
        }
        for (int i = 0; i < 10; i++)
        {
            dwCreateUser[i] = ReadUInt32();
        }
        for (int i = 0; i < 10; i++)
        {
            dwWinCount[i] = ReadUInt32();
        }
        for (int i = 0; i < 10; i++)
        {
            szUserNickName[i] = ReadString(64);
        }
    }
}


#endregion


#region 排行

//请求排行信息
public class CMD_Hall_C_GetRank : DataBase
{ 
    public byte	cbRankType;					//排行类型  1-日 2-周 3-总    4-金币日榜  5-金币总榜

    public CMD_Hall_C_GetRank()
    {
        header.wMainCmdID = 3;
        header.wSubCmdID = 720;
    }

    protected override void PackBody()
    {
        WriteByte(cbRankType);
    }
}

//排行信息
public class CMD_Hall_S_RankInfo : DataBase
{ 
    public byte cbRankType;									//排行类型
    public int[] dwUserID = new int[20];
    public int[] dwGameID = new int[20];
	public string[] szNickName = new string[20];               //64
	public Int64[]	lCount = new Int64[20];
	public string[] szWeChatURL = new string[20];		        //微信URL 512

    protected override void UnPackBody()
    {
        cbRankType = ReadByte();
        dwUserID = ReadIntArray(20);
        dwGameID = ReadIntArray(20);
        for (int i = 0; i < 20; i++)
        {
            szNickName[i] = ReadString(64);
        }
        lCount = ReadInt64Ary(20);
        for (int i = 0; i < 20; i++)
        {
            szWeChatURL[i] = ReadString(512);
        }
    }
}

#endregion

#region 低保
//流程：
//发送 加载低保-> 返回 低保参数
//发送 领取低保-> 返回 低保结果

////低保命令
//#define MDM_GR_BASEENSURE				3

//#define SUB_GR_C_BASEENSURE_LOAD			260								//加载低保
//#define SUB_GR_C_BASEENSURE_TAKE			261								//领取低保
//#define SUB_GR_S_BASEENSURE_PARAMETER		262								//低保参数
//#define SUB_GR_S_BASEENSURE_RESULT		263								//低保结果

//加载低保
public class CMD_Hall_C_GetBaseEnsurePara : DataBase
{
    public int nBaseEnsureType;				//低保类型 0,红包场,1,金币场

    public CMD_Hall_C_GetBaseEnsurePara()
    {
        header.wMainCmdID = 3;
        header.wSubCmdID = 260;
    }

    protected override void PackBody()
    {
        WriteInt(nBaseEnsureType);
    }
}

//领取低保
public class CMD_Hall_C_BaseEnsureTake : DataBase
{
    public UInt32 dwUserID;				//用户 I D
    public int nBaseEnsureType;			//低保类型 0,红包场,1,金币场
    public string szPassword;			//登录密码 66
    public string szMachineID;		    //机器序列 66

    public CMD_Hall_C_BaseEnsureTake()
    {
        header.wMainCmdID = 3;
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

//低保参数
public class CMD_Hall_S_BaseEnsureParamter : DataBase
{
    public int nBaseEnsureType;     				//低保类型 0,红包场,1,金币场
    public Int64 lScoreCondition;					//游戏币条件
    public Int64 lScoreAmount;						//游戏币数量
    public Int64 lIngotCondition;					//元宝条件
    public Int64 lIngotAmount;						//元宝数量
    public UInt16 wCoinTakeTimes;						//领取次数	
    public UInt16 wCoinCurTakeTimes;					//领取次数
    public UInt16 wIngotTakeTimes;						//领取次数	
    public UInt16 wIngotCurTakeTimes;					//领取次数

    protected override void UnPackBody()
    {
        nBaseEnsureType = ReadInt();
        lScoreCondition = ReadInt64();
        lScoreAmount = ReadInt64();
        lIngotCondition = ReadInt64();
        lIngotAmount = ReadInt64();

        wCoinTakeTimes = ReadUInt16();
        wCoinCurTakeTimes = ReadUInt16();

        wIngotTakeTimes = ReadUInt16();
        wIngotCurTakeTimes = ReadUInt16();
    }
}

//低保结果
public class CMD_Hall_S_BaseEnsureResult : DataBase
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