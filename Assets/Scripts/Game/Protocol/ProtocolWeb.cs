using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

//版本信息
[Serializable]
public class Web_C_Version
{
    public int code = 0;    //平台 1-Android   2-IOS
}

//版本信息
[Serializable]
public class Web_S_Version
{
    public string version;      //版本号
    public string content;      //更新内容
    public string downurl;      //更新地址
    public int logontype;       //1-微信登录， 2-手机登录
}


//微信登录
[Serializable]
public class Web_C_WeiXinLogin
{
    public string code;
}

/// 微信登录返回
[Serializable]
public class Web_S_WeiXinLogin
{
    public string unionid;
    public string nickName;
    public string sex;
    public string headimgurl;
}


//商城信息
[Serializable]
public class Web_S_StoreInfo
{ 
    public string return_code;
    public string message;
    public List<GoodInfo> return_message;
}



//绑定手机操作
[Serializable]
public class Web_C_BindPhoneOp
{
    public int userid;
    public string phone;
    public string code;         
    public int type;            //0:发送验证码，1：绑定手机,2：解除手机绑定 
    public int codeType;        //1-绑定手机号码 2-解绑手机号码
    public string sigin;        //加密字符
}

//绑定手机结果
[Serializable]
public class Web_S_BindPhoneOp
{
    public int return_code;
    public string return_message;
}



//邮件信息
[Serializable]
public class Web_C_EmailInfo
{
    public int type;    //1，查询，2，更新阅读
    public int userId;
    public int emailId;
}

//邮件信息
[Serializable]
public class Web_S_EmailInfo
{
    public int return_code;
    public string message;
    public List<EmailInfo> return_message;
}



//兑换列表
[Serializable]
public class Web_C_ExchangeList
{
    public string type = "GameProperty";
}

//兑换列表
[Serializable]
public class Web_S_ExchangeList
{
    public int return_code;
    public string message;         
    public List<ExchangeInfo> return_message = new List<ExchangeInfo>();
}


//道具信息
[Serializable]
public class Web_C_PropInfo
{
    public string type = "GetPackageCount";
    public int userId;
}

[Serializable]
public class Web_S_PropInfo
{
    public int return_code;
    public string message;
    public List<PropInfo> return_message = new List<PropInfo>();
}


//月卡信息
[Serializable]
public class Web_C_MonthCardInfo
{
    public string type = "GetMonthCardUseCount";
    public int userId;
}


[Serializable]
public class Web_MonthCardInfo
{
    public int UserID;
    public int MonthCardUseCount;
    public int LastDayCount;
    public int MonthCardType;           //0-没有月卡   1-老月卡    2-新月卡
    public int FirstChargeCount;
    public int NeedSendCircle;           //是否需要分享朋友圈 0不需要，1需要
}

[Serializable]
public class Web_S_MonthCardInfo
{
    public int return_code;
    public string message;
    public Web_MonthCardInfo return_message = new Web_MonthCardInfo();
}



//红包信息
[Serializable]
public class Web_C_RedPackInfo
{
    public string type = "GetPackageCount";
    public int userId;
}

//红包信息
[Serializable]
public class Web_S_RedPackInfo
{ 
    public int return_code;
    public string return_message;
}




//兑换商品
[Serializable]
public class Web_C_Exchange
{
    public string type = "ExchangeProperty";
    public int userId;
    public int goodsId;
}

//兑换商品
[Serializable]
public class Web_S_Exchange
{ 
    public int return_code;
    public string return_message;
}





//兑换记录
[Serializable]
public class Web_C_ExchangRecord
{
    public string type = "ExchangePropertyList";
    public int userId;
    public int pageIndex;
}

//兑换记录
[Serializable]
public class Web_S_ExchangeRecord
{ 
    public int return_code;
    public string message;
    public int pageIndex;       //当前页
    public int count;           //总页数
    public List<ExchangeRecord> return_message = new List<ExchangeRecord>();
}




//广告图
[Serializable]
public class Web_S_AdTextureInfo
{
    public int return_code;
    public string message;
    public List<AdTextureInfo> return_message = new List<AdTextureInfo>();
}




#region 邀请有礼

[Serializable]
public class Web_C_InviteConfig
{
    public string type = "GetSystemStatusInfo";
}

[Serializable]
public class Web_S_InviteConfig
{ 
    public int AgentInvitationCount;  //代理商申请需要邀请人数，
    public int BigGiftAccountsCount;  //获得惊喜大礼包需要邀请人数
}


[Serializable]
public class Web_C_PlayerTaskConfig
{ 
     public string type = "TaskInfoList";
}

[Serializable]
public class Web_S_PlayerTaskConfig
{ 
    public int return_code;
    public string essage;
    public List<Web_TaskConfig> return_message = new List<Web_TaskConfig>();
}

[Serializable]
public class Web_TaskConfig
{ 
    public int TaskID;
    public string TaskName;              //任务名称,
    public int StandardAwardIngot;       //奖励钻石,
    public float AwardPropertyCount;       //奖励红包,
}




//邀请信息
[Serializable]
public class Web_C_InviteFriendInfo
{
    public string type = "TaskCount";
    public int userId;
}

//邀请信息
[Serializable]
public class Web_InviteFriendInfo
{
    public int UserCount;          //好友数
    public int TaskCount1;           //任务1可领数
    public int TaskCount2;      //任务2可领数 
    public int CurCount;   //当前礼券数
    public int TakeCount;   //已领取总数
    public int Task1RewardCount;   //每完成任务1所获得奖励数
    public int Task2RewardCount;   //每完成任务2所获得奖励数
    public string Task1Describe;      //任务1描述
    public string Task2Describe;      //任务2描述
}

//邀请信息
[Serializable]
public class Web_S_InviteFriendInfo
{ 
    public int return_code;
    public string message;
    public Web_InviteFriendInfo return_message = new Web_InviteFriendInfo();
    
}



//领取邀请奖励
[Serializable]
public class Web_C_GetInviteAward
{
    public string type = "TaskReward";
    public int userId;
    public int taskid;
}

//领取邀请奖励
[Serializable]
public class Web_S_GetInviteAward
{ 
    public int return_code;
    public string message;   
    public int IngotCount;          //钻石数
    public float RedEnvelopesCount;   //红包数
    public int RoomCardCount;       //房卡数
}






//好友任务记录
[Serializable]
public class Web_C_PlayerTaskRecord
{
    public string type = "RecordList";
    public int userId;
    public int pageIndex;
}

//好友任务记录
[Serializable]
public class Web_S_PlayerTaskRecord
{
    public int return_code;
    public string message;
    public int pageIndex;
    public int count;
    public List<Web_PlayerTaskRecord> return_message = new List<Web_PlayerTaskRecord>();
}

[Serializable]
public class Web_PlayerTaskRecord
{
    public int UserID;
    public string NickName;
    public int GameID;
    public int TaskStatus1;     //0未完成，1待领取，2已领取
    public double PayCount;
    public string WeChatURL;
}




//重置密码
[Serializable]
public class Web_C_ResetPassword
{
    public string loginName = "";
    public string mobiePhone = "";
    public int type = 0;            //0-发送验证码  1-修改密码
    public string passWord = "";
    public string mobileCode = "";
    public string LastLogonIP = "";
    public string sigin = "";
}

//重置密码
[Serializable]
public class Web_S_ResetPassword
{
    public int return_code;
    public string return_message;
}


#endregion




[Serializable]
public class Web_C_Test
{
    public string sign = "";
}

[Serializable]
public class Web_S_Test
{
    public int return_code;
    public string return_message;


}


