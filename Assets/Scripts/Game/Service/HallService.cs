using UnityEngine;
using System.Collections;
using System.Net;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using Bs.Gateway;
using System.IO;
using Bs.Lobby;

public class HallService : MonoBehaviour
{

    private static HallService _instance;
    public static HallService Instance
    {
        get
        {
            if (_instance == null)
            {
                string name = "HallService";
                GameObject obj = GameObject.Find(name);
                if (obj == null)
                {
                    obj = new GameObject(name);
                    _instance = obj.AddComponent<HallService>();
                }
                else
                {
                    _instance = obj.GetComponent<HallService>();
                    if (_instance == null)
                    {
                        _instance = obj.AddComponent<HallService>();
                    }
                }
            }
            return _instance;
        }
    }

    private ClientSocket client;
    [HideInInspector]
    public bool isConnect = false;                             //与服务器是否连接正常
    private ConnectType connectType = ConnectType.Normal;       //连接模式

    //用户信息
    private string userAccount = "";        //用户账号
    private string userPassword = "";       //用户密码
    private string userOpenId = "";         //微信登录openId
    private string userNickName = "";       //昵称
    private string userSex = "0";           //性别
    private string userImgUrl = "";         //头像url
    private string userReportId = "";       //推荐人ID
    private string userPhoneCode = "";          //验证码

    private string bankPassword = "";    //银行密码

    private OpCode opCode = OpCode.OpenBank;        //当前操作

    void Awake()
    {
        client = new ClientSocket();
        client.netManager.AddHandler(new HallHandler());
        client.connectSuccessEvent += OnConnectSuccess;
        client.connectErrorEvent += OnConnectError;
        DontDestroyOnLoad(this.gameObject);

        AppConfig.serverIndex = 0;
        AppConfig.InitApp();
    }

    //实时接收网络消息
    void FixedUpdate()
    {
        client.netManager.OnUpdate();
    }

    //网络监测
    void ConnectDetection()
    {
        if (!isConnect)
        {
            client.CloseConnect();
            Connect(ConnectType.ConnectOnBreak);
        }
    }

    void OnApplicationFocus(bool focusStatus)
    {
        HallModel.isBackground = !focusStatus;
        if (focusStatus)
        {
            
            //开始网络监测
            CancelInvoke("ConnectDetection");
            InvokeRepeating("ConnectDetection", 3f, 5f);
            //发送心跳包
            CancelInvoke("SendHeartPacket");
            InvokeRepeating("SendHeartPacket", 1f, 5f);
            //刷新银行
            Invoke("QueryBankInfo", 5f);
        }
        else
        {
            //停止网络监测,心跳
            CancelInvoke("ConnectDetection");
            CancelInvoke("SendHeartPacket");
        }
    }

    #region 登录

    //初始化账号信息
    public void InitAccountInfo(LoginType loginType, string account, string password, string openId, string nickName, string sex, string imgUrl, string reportId, string phoneCode)
    {
        HallModel.loginType = loginType;
        this.userAccount = account;
        this.userPassword = password;
        this.userOpenId = openId;
        this.userNickName = nickName;
        this.userSex = sex;
        this.userImgUrl = imgUrl;
        this.userReportId = reportId;
        this.userPhoneCode = phoneCode;
    }


    public void Connect(ConnectType type)
    {
        opCode = OpCode.Null;
        connectType = type;
        //连接提示
        if (SceneManager.GetActiveScene().name.ToLower().Contains("hall"))
        {
            Util.Instance.DoAction(GameEvent.V_OpenConnectTip, "正在连接服务器...");
        }

        try
        {
            IPAddress[] ips = Dns.GetHostAddresses(AppConfig.serverUrl[AppConfig.serverIndex]);
            client.Connect(ips[0].ToString(), AppConfig.serverPort[AppConfig.serverIndex]);
        }
        catch
        {
            AppConfig.serverIndex++;
            AppConfig.serverIndex = AppConfig.serverIndex % 3;
            Invoke("Connect", 1f);
        }
    }

    //发送网络心跳包
    public void SendHeartPacket()
    {
        //Debug.Log("发送网络心跳包");

        PulseReq req = new PulseReq();
        client.SendDate2Gate(NetManager.AppGate, (UInt16)CMDGateway.IdpulseReq, req);
    }

    //断开连接
    public void BreakConnect()
    {
        HallModel.serverList.Clear();
        HallModel.cardServerList.Clear();
        HallModel.gameKindList.Clear();

        isConnect = false;
        client.CloseConnect();
        Util.Instance.DoAction(GameEvent.V_CloseConnectTip);
        CancelInvoke("ConnectDetection");
        CancelInvoke("SendHeartPacket");
    }

    //连接服务器成功
    public void OnConnectSuccess()
    {
        isConnect = true;
        

        Debug.Log("准备发送Hello");

        HelloReq req = new HelloReq();
        req.AdId = 110;
        client.SendDate2Gate(NetManager.AppGate, (UInt16)CMDGateway.IdhelloReq, req);
        return;



        //登录
        if (connectType == ConnectType.Normal)
        {
            switch (HallModel.loginType)
            {
                case LoginType.Register:
                    Register();
                    break;
                case LoginType.Account:
                    LoginAccount();
                    break;
                case LoginType.OtherSDK:
                    LoginOther();
                    break;
                case LoginType.Visitor:
                    LoginVisitor();
                    break;
            }
        }
        else
        {
            LoginOnBreak();
        }
    }


    //网络连接异常
    public void OnConnectError()
    {
        isConnect = false;
        client.CloseConnect();
    }


    //账户登录
    void LoginAccount()
    {
        CMD_Hall_C_LoginAccount pro = new CMD_Hall_C_LoginAccount();
        pro.cbDeviceType = Util.GetDeviceType();
        pro.szPassword = Util.GetMd5(userPassword);
        pro.szAccounts = userAccount;
        pro.szMachineID = Util.GetMacCode();
        pro.szMobilePhone = "";
        pro.veriCode = userPhoneCode;
        client.SendPro(pro);
    }

    //账号登录 - 验证码
    public void LoginWithCode(string code)
    {
        CMD_Hall_C_LoginAccount pro = new CMD_Hall_C_LoginAccount();
        pro.cbDeviceType = Util.GetDeviceType();
        pro.szPassword = Util.GetMd5(userPassword);
        pro.szAccounts = userAccount;
        pro.szMachineID = Util.GetMacCode();
        pro.szMobilePhone = "";
        pro.veriCode = code;
        client.SendPro(pro);
    }

    //游客登陆
    void LoginVisitor()
    {
        CMD_Hall_C_LoginVisitor pro = new CMD_Hall_C_LoginVisitor();
        pro.szMachineID = Util.GetMacCode();
        client.SendPro(pro);
    }

    //第三方登录
    void LoginOther()
    {
        CMD_Hall_C_LoginOther pro = new CMD_Hall_C_LoginOther();
        pro.cbDeviceType = Util.GetDeviceType();
        pro.cbChannelPartner = (byte)AppConfig.channelCode;
        pro.szUserUin = userOpenId;
        pro.cbPlatformID = 4;
        pro.szNickName = userNickName;
        byte.TryParse(userSex, out pro.cbGender);
        pro.szMachineID = Util.GetMacCode();
        pro.szMobilePhone = "";

        client.SendPro(pro);

        //自适应翻转
        Util.AdaptScreenOrientation();
    }

    //断线后登录 - ID登录
    public void LoginOnBreak()
    {
        CMD_Hall_C_LoginById pro = new CMD_Hall_C_LoginById();
        pro.dwGameID = (uint)HallModel.gameId;
        pro.szPassword = HallModel.dynamicPassword;
        pro.szMachineID = Util.GetMacCode();
        client.SendPro(pro);
    }

    //注销登录
    public void LoginOut()
    {
        CMD_Hall_C_LoginOut pro = new CMD_Hall_C_LoginOut();
        pro.dwUserID = (uint)HallModel.userId;
        pro.dwGameID = (uint)HallModel.gameId;
        client.SendPro(pro);
    }

    //注册账号
    public void Register()
    {
        CMD_Hall_C_Register pro = new CMD_Hall_C_Register();
        pro.szAccounts = userAccount;
        pro.szLogonPass = Util.GetMd5(userPassword);
        pro.szNickName = userNickName;
        pro.szMachineID = Util.GetMacCode();
        UInt32.TryParse(userReportId, out pro.szSprederId);

        client.SendPro(pro);
    }

    //登陆成功
    public void OnHelloRsp()
    {
        Util.Instance.DoAction(GameEvent.V_CloseConnectTip);

        //开始网络监测,开始心跳
        CancelInvoke("ConnectDetection");
        CancelInvoke("SendHeartPacket");
        InvokeRepeating("ConnectDetection", 3f, 5f);
        InvokeRepeating("SendHeartPacket", 1f, 5f);

        LoginReq req = new LoginReq();
        req.GameKind = 120;
        req.Account = userAccount;
        req.Password = Util.GetMd5(userPassword);
        client.SendTransferData2Gate(NetManager.AppLogin, NetManager.Send2AnyOne, NetManager.AppLogin, (UInt32)(CMDLobby.IdloginReq), req);
    }

    public void OnLoginRsp(Bs.Gateway.TransferDataReq transferData)
    {
        //Bs.Lobby.LoginRsp rsp = ProtoBuf.Serializer.Deserialize<Bs.Lobby.LoginRsp>(new System.IO.MemoryStream(transferData.data));
        LoginRsp rsp = NetPacket.Deserialize<LoginRsp>(transferData.Data.ToByteArray());
        if (rsp.ErrInfo.Code == 0)
        {
            //微信登录时保存openId
            if (HallModel.loginType == LoginType.OtherSDK)
            {
                HallModel.userOpenId = userOpenId;
            }
            if (HallModel.loginType == LoginType.Account)
            {
                HallModel.userAccount = userAccount;
                if (HallModel.isRememberUserPassword)
                {
                    HallModel.userPassword = userPassword;
                }
                else
                {
                    HallModel.userPassword = "";
                }
            }
            //数据缓存
            HallModel.userId = (int)rsp.BaseInfo.UserId;
            HallModel.gameId = (int)rsp.BaseInfo.GameId;
            HallModel.userName = rsp.BaseInfo.NickName;
            HallModel.userSex = 0;
            HallModel.faceId = (int)rsp.BaseInfo.FaceId;
            HallModel.dynamicPassword = "";
            HallModel.userDiamondCount = 0;
            HallModel.userCoinInGame = 0;
            HallModel.userCoinInBank = 0;
            HallModel.isBankEnable = false;
            //HallModel.userRateInfo = "胜 " + pro.dwWinCount + "，负 " + pro.dwLostCount + "，平 " + pro.dwDrawCount;

            HallModel.userIP = Util.GetIPAddress();
            //             HallModel.spreaderId = (int)pro.spreaderId;
            //             HallModel.spreaderName = pro.spreaderName;
            //清空游戏记录信息
            HallModel.gameRecordList.Clear();

            if (connectType == ConnectType.Normal)
            {
                //登录成功提示
                Util.Instance.DoAction(GameEvent.V_OpenShortTip, "登录游戏大厅成功！");
                //查询任务
                QueryTaskInfo();
                //查询签到
                QuerySignInfo();
                //请求排行信息
                GetRankInfo();
                //上传头像url
                UploadHeadImgInfo();
                //请求商城配置信息
                //Util.Instance.DoActionDelay(WebService.Instance.GetStoreInfo, 1f);
                //请求兑换配置信息
                //Util.Instance.DoActionDelay(WebService.Instance.GetExchangeInfo, 2f);
                //请求游戏记录
                GetGameRecord(GameFlag.Landlords3);

                //请求道具信息
                foreach (ExchangeInfo info in AppConfig.exchangeDic.Values)
                {
                    //月卡是否存在
                    if (info.ID != 620) continue;

                    WebService.Instance.GetPropInfo();
                    break;
                }
            }
            //查询银行
            QueryBankInfo();
            //低保参数
            GetBaseEnsureInfo();
            //
            QueryRoomList();
            //查询用户信息 - 手机号，头像
            Util.Instance.DoActionDelay(QueryUserInfo, 0.2f, (uint)HallModel.userId);

            //界面操作
            Util.Instance.DoAction(HallEvent.V_ClosePnlLogin);
            Util.Instance.DoActionDelay(HallEvent.V_OpenPnlMain, 0.1f);
        }
        else
        {
            if (GameEvent.V_OpenDlgTip != null)
            {
                GameEvent.V_OpenDlgTip.Invoke(rsp.ErrInfo.Info, "", null, null);
            }

            //断开连接
            BreakConnect();
        }
    }

    public void OnRoomListRsp(Bs.Gateway.TransferDataReq transferData)
    {
//         Google.Protobuf.IMessage.Descriptor.Parser.ParseFrom(new System.IO.MemoryStream(GetData()));
//         Bs.List.RoomListRsp rsp = ProtoBuf.Serializer.Deserialize<Bs.List.RoomListRsp>(new System.IO.MemoryStream(transferData.data));
//         for (int i = 0; i < rsp.rooms.Count; i++)
//         {
//             HallModel.roomList[rsp.rooms[i].app_info.id] = rsp.rooms[i];
//             Debug.Log("插入房间,id=" + rsp.rooms[i].app_info.id);
//         }
    }

    //登陆完成
    public void OnLoginFinish()
    {

    }
    #endregion

    #region 断线续连


    //取得玩家当前游戏中状态
    public void GetUserGameState()
    {
        CMD_Hall_C_GetUserGameState pro = new CMD_Hall_C_GetUserGameState();
        pro.dwGameID = (uint)HallModel.gameId;
        pro.nickName = "";
        client.SendPro(pro);
    }

    //收到玩家游戏中状态
    public void OnGetUserStateInGame(CMD_Hall_S_GetUserGameState pro)
    {
        if (pro.userCount > 0)
        {
            if (HallModel.serverList.ContainsKey(pro.players[0].wServerID))
            {
                HallModel.currentGameFlag = (GameFlag)pro.players[0].wKindID;
                HallModel.currentServerId = pro.players[0].wServerID;
                HallModel.opOnLoginGame = OpOnLginGame.Null;
                //链接游戏服务器
                GameService.Instance.Connect();
            }
            else
            {
                Debug.LogError("断线重连失败，没有当前玩家所在游戏服务器信息！");
            }
        }
        else if (HallModel.isStartByURL)
        {
            GetRoomServerInfo(GameModel.ServerKind_Private, 0);
        }
        else if (!SceneManager.GetActiveScene().name.ToLower().Contains("hall"))
        {
            GameService.Instance.ReturnToHall();
        }
    }

    #endregion

    #region 个人资料

    //修改登录密码
    //public void ModifyUserPwd(string oldPwd, string newPwd)
    //{
    //    opCode = OpCode.ChangeUserPassword;
    //    userPassword = newPwd;

    //    CMD_Hall_C_ModifyUserPwd pro = new CMD_Hall_C_ModifyUserPwd();
    //    pro.dwUserID = (uint)HallModel.userId;
    //    pro.szScrPassword = Util.GetMd5(oldPwd);
    //    pro.szDesPassword = Util.GetMd5(newPwd);
    //    client.SendPro(pro);
    //}

    //修改银行密码
    //public void ModifyBankPwd(string oldPwd, string newPwd)
    //{
    //    opCode = OpCode.ChangeBankPassword;
    //    bankPassword = newPwd;

    //    CMD_Hall_C_ModifyBankPwd pro = new CMD_Hall_C_ModifyBankPwd();
    //    pro.dwUserID = (uint)HallModel.userId;
    //    pro.szScrPassword = Util.GetMd5(oldPwd);
    //    pro.szDesPassword = Util.GetMd5(newPwd);
    //    client.SendPro(pro);
    //}

    //修改用户头像 - 系统头像
    //public void ModifyUserPhoto(int faceId)
    //{
    //    CMD_Hall_C_ModifySystemFaceInfo pro = new CMD_Hall_C_ModifySystemFaceInfo();
    //    pro.dwUserID = (uint)HallModel.userId;
    //    pro.wFaceID = (UInt16)faceId;
    //    pro.szPassword = "";
    //    pro.szMachineID = Util.GetMacCode();
    //    client.SendPro(pro);
    //}

    //用户头像 - 系统头像
    //public void OnReceiveFaceInfo(CMD_Hall_S_FaceInfo pro)
    //{
    //    HallModel.faceId = pro.wFaceID;
    //    Util.Instance.DoAction(GameEvent.V_RefreshUserInfo);
    //}

    //修改用户昵称
    public void ModifyUserNickName(string nickName)
    {
   
    }

    //上传头像信息
    public void UploadHeadImgInfo()
    {

    }

    //设置推荐人ID
    public void SetSpreaderUser(uint gameId)
    {

    }



    //查询个人信息
    public void QueryUserInfo(uint userId)
    {

    }

    //收到个人信息
    public void OnGetQueryUserInfo(CMD_Hall_S_QueryUserInfo pro)
    {
        //保存自己手机号信息
        if (pro.dwUserID == HallModel.userId)
        {
            HallModel.userPhone = pro.szMobilePhone;
        }
        //加载用户头像
        if (!HallModel.userPhotos.ContainsKey((int)pro.dwUserID))
        {
            WebService.Instance.LoadUserPhoto((int)pro.dwUserID, pro.szWeChatURL);
        }
    }

    //IEnumerator LoadUserPhoto(UInt32 userId, string url)
    //{
    //    yield return new WaitForSeconds(UnityEngine.Random.Range(0.1f, 0.3f));
    //    WWW www = new WWW(url);
    //    yield return www;
    //    //保存自己头像信息
    //    if (userId == HallModel.userId)
    //    {
    //        if (www.error == null)
    //        {
    //            HallModel.userPhoto = www.texture;
    //        }
    //        else
    //        {
    //            HallModel.userPhoto = HallModel.defaultPhoto;
    //        }
    //    }
    //    yield return new WaitForEndOfFrame();
    //    //游戏中，保存查询玩家的头像信息
    //    int chairId = -1;
    //    for (int i = 0; i < 8; i++)
    //    {
    //        if (GameModel.GetDeskUser(i) != null && GameModel.GetDeskUser(i).dwUserID == userId)
    //        {
    //            chairId = i;
    //        }
    //    }
    //    if (chairId != -1)
    //    {
    //        if (www.error == null)
    //        {
    //            GameModel.userPhotos[chairId] = www.texture;
    //        }
    //    }
    //    yield return new WaitForEndOfFrame();
    //    //刷新
    //    Util.Instance.DoAction(GameEvent.V_RefreshUserInfo, false);
    //}

    //实名认证
    public void RealAuth(string name, string id)
    {
        CMD_Hall_C_RealAuth pro = new CMD_Hall_C_RealAuth();
        pro.dwUserID = (uint)HallModel.userId;
        pro.szPassword = HallModel.dynamicPassword;
        pro.szCompellation = name;
        pro.szPassPortID = id;
        client.SendPro(pro);
    }

    //实名认证结果
    public void OnReceiveRealAuthResult(CMD_Hall_S_IndividuaResult pro)
    {
        if (pro.bSuccessed == 1)
        {
            HallModel.isRealAuth = true;
        }
        if (GameEvent.V_OpenDlgTip != null)
        {
            GameEvent.V_OpenDlgTip.Invoke(pro.szNotifyContent, "", null, null);
        }
    }

    #endregion

    #region 签到

    //查询签到信息
    public void QuerySignInfo()
    {
        CMD_Hall_C_QuerySignInfo pro = new CMD_Hall_C_QuerySignInfo();
        pro.dwUserID = (uint)HallModel.userId;
        pro.szPassword = HallModel.dynamicPassword;
        client.SendPro(pro);
    }

    //收到签到信息
    public void OnGetSignInfo(CMD_Hall_S_SignInfo pro)
    {
        HallModel.isSign = pro.bTodayChecked;
        HallModel.signDay = pro.wSeriesDate;
        HallModel.RewardCheckIn = pro.RewardCheckIn;
        HallModel.SeriesRewardInfo = pro.SeriesRewardInfo;

        //弹签到
        if(HallModel.isSign == false)
        {
            Util.Instance.DoAction(HallEvent.V_OpenDlgWheelSignDay, null);
        }
        else
        {
            //弹月卡
            if (!HallModel.isBuyMonthCard)
            {
                Util.Instance.DoAction(HallEvent.V_OpenDlgMonthCard, 0);
            }
            else
            {
                //弹领取
                if (!HallModel.isGetMonthCardGift)
                {
                    Util.Instance.DoAction(HallEvent.V_OpenDlgMonthCard, 0);
                }
                else
                {
                    //弹分享任务
                    Util.Instance.DoActionDelay(HallEvent.V_OpenDlgShareTask, 0.2f);
                }
            }
        }
    }

    //签到
    public void SignIn()
    {
        CMD_Hall_C_SignIn pro = new CMD_Hall_C_SignIn();
        pro.dwUserID = (uint)HallModel.userId;
        //pro.szPassword = HallModel.dynamicPassword;
        pro.szMachineID = Util.GetMacCode();
        client.SendPro(pro);
    }


    //收到签到结果
    public void OnGetSignResult(CMD_Hall_S_SignInResult pro)
    {
        if (pro.bSuccessed)
        {
            HallModel.isSign = true;
            HallModel.signDay = pro.nSeriesDays;

            Util.Instance.DoAction(HallEvent.V_OpenDlgWheelSignDay, pro);

            //刷新签到界面信息
            Util.Instance.DoAction(HallEvent.V_RefreshPnlSign);
            //查询个人信息
            QueryBankInfo();
        }
        else if (GameEvent.V_OpenDlgTip != null)
        {
            AudioManager.Instance.PlaySound(GameModel.audioTipWrong);
            GameEvent.V_OpenDlgTip.Invoke("签到失败\n" + pro.szNotifyContent, "", null, null);
            return;
        }
    }



    #endregion

    #region 分享 - TODO

    //分享游戏成功
    public void ShareSuccess()
    {
        CMD_Hall_C_ShareGame pro = new CMD_Hall_C_ShareGame();
        pro.szMachineID = SystemInfo.deviceUniqueIdentifier;
        client.SendPro(pro);
    }

    //收到分享结果
    public void OnGetShareResult(CMD_Hall_S_ShareResult pro)
    {
        if (pro.lScore > 0)
        {
            Util.Instance.DoAction(GameEvent.V_OpenShortTip, pro.szDescribeString);
            QueryBankInfo();
        }
        else
        {
            Util.Instance.DoAction(GameEvent.V_OpenShortTip, "分享成功！");
        }

    }

    #endregion

    #region 银行

    //开通银行
    public void EnableBank(string password)
    {
        bankPassword = password;
        CMD_Hall_C_EnableBank pro = new CMD_Hall_C_EnableBank();
        pro.dwUserID = (uint)HallModel.userId;
        pro.szLogonPass = HallModel.dynamicPassword;
        pro.szInsurePass = Util.GetMd5(password);
        pro.szMachineID = Util.GetMacCode();
        client.SendPro(pro);
    }

    //开通银行结果
    public void OnGetEnableBankResult(CMD_Hall_S_EnableBankResult pro)
    {

    }


    //打开银行
    public void OpenBank(string password)
    {

    }



    //查询银行信息
    public void QueryBankInfo()
    {
        
    }

    public void QueryRoomList()
    {
        Bs.List.RoomListReq req = new Bs.List.RoomListReq();
        req.ListId = 0;
        client.SendTransferData2Gate(NetManager.AppList, NetManager.Send2AnyOne, NetManager.AppList, (UInt32)(Bs.List.CMDList.IdroomListReq), req);
    }

    //收到银行信息
    public void OnGetBankInfo(CMD_Hall_S_BankInfo pro)
    {
        HallModel.userCoinInBank = pro.lUserInsure;
        HallModel.userCoinInGame = pro.lUserScore;
        HallModel.userDiamondCount = pro.lUserIngot;
        HallModel.userCardCount = pro.lUserRoomCard;
        HallModel.userRedPackCount = pro.lUserRedRevelopes;
        Util.Instance.DoAction(GameEvent.V_RefreshUserInfo, true);
    }



    //银行操作 - 存钱
    public void BankSave(Int64 count)
    {

    }

    //银行操作 - 取钱
    public void BankGet(Int64 count)
    {

    }

    //银行操作 - 赠送
    public void BankSend(Int64 coinCount, UInt32 targetUserId)
    {

    }

    //银行操作 - 成功
    public void OnBankOpSuccess(CMD_Hall_S_BankOpSuccess pro)
    {
        HallModel.userCoinInGame = pro.lUserScore;
        HallModel.userCoinInBank = pro.lUserInsure;
        //刷新
        Util.Instance.DoAction(GameEvent.V_RefreshUserInfo, true);
        //弹窗提示
        if (GameEvent.V_OpenDlgTip != null)
        {
            GameEvent.V_OpenDlgTip.Invoke(pro.szDescribeString, "", null, null);
        }
    }

    //银行操作 - 失败
    public void OnBankOpFail(CMD_Hall_S_BankOpFail pro)
    {
        if (GameEvent.V_OpenDlgTip != null)
        {
            GameEvent.V_OpenDlgTip.Invoke(pro.szDescribeString, "", null, null);
        }
    }

    //查询赠送用户信息
    public void QuerySendUserInfo(UInt32 gameId)
    {
        CMD_Hall_C_QuerySendUserInfo pro = new CMD_Hall_C_QuerySendUserInfo();
        pro.cbByNickName = 0;
        pro.dwTargetGameID = gameId;
        client.SendPro(pro);
    }

    //收到赠送用户信息
    public void OnGetSendUserInfo(CMD_Hall_S_QuerySendUserInfoResult pro)
    {
        //if (HallEvent.V_OnGetSendUserInfo != null)
        //{
        //    HallEvent.V_OnGetSendUserInfo.Invoke(pro.szNickName, pro.dwTargetUserID.ToString());
        //}
    }

    #endregion

    #region 兑换

    //兑换金币
    public void ExchangeCoin(Int64 diamondCount)
    {
        CMD_Hall_C_ExchangeCoin pro = new CMD_Hall_C_ExchangeCoin();
        pro.dwUserID = (uint)HallModel.userId;
        pro.lExchangeIngot = diamondCount;
        pro.szMachineID = Util.GetMacCode();
        client.SendPro(pro);
    }

    //兑换结果
    public void OnGetExchangeReuslt(CMD_Hall_S_ExchangeResult pro)
    {
        if (pro.bSuccessed)
        {
            HallModel.userCoinInBank = pro.bankScore;
            HallModel.userDiamondCount = pro.lCurrIngot;
            Util.Instance.DoAction(GameEvent.V_RefreshUserInfo, true);
            Util.Instance.DoAction(GameEvent.V_OpenShortTip, "兑换成功！");
        }
        else
        {
            if (GameEvent.V_OpenDlgTip != null)
            {
                GameEvent.V_OpenDlgTip.Invoke(pro.szNotifyContent, "", null, null);
            }
        }
    }

    #endregion

    #region 列表消息

    //游戏排序信息
    public void OnReceiveServerKindList(CMD_Hall_S_GameKindList pro)
    {
        for (int i = 0; i < pro.count; i++)
        {
            HallModel.gameKindList.Add(pro.gameKindList[i]);
        }
        HallModel.gameKindList.Sort(delegate(GameKindInfo info_1, GameKindInfo info_2) { return info_1.wSortID.CompareTo(info_2.wSortID); });
    }

    //收到房卡游戏服务器信息-创建参数
    public void OnReceiveCardServerInfo(CMD_Hall_S_CardGameServer pro)
    {
        for (int i = 0; i < pro.serverCount; i++)
        {
            if (HallModel.cardServerList.ContainsKey(pro.cardServerList[i].wKindID))
            {
                HallModel.cardServerList[pro.cardServerList[i].wKindID] = pro.cardServerList[i];
            }
            else
            {
                HallModel.cardServerList.Add(pro.cardServerList[i].wKindID, pro.cardServerList[i]);
            }
        }
    }

    //收到服务器列表
    public void OnReceiveServerList(CMD_Hall_S_GameServerList pro)
    {
        for (int i = 0; i < pro.serverCount; i++)
        {
            if (HallModel.serverList.ContainsKey(pro.serverList[i].wServerID))
            {
                HallModel.serverList[pro.serverList[i].wServerID] = pro.serverList[i];
            }
            else
            {
                HallModel.serverList.Add(pro.serverList[i].wServerID, pro.serverList[i]);
            }
        }
    }


    //收到服务器列表完成
    public void OnReceiveServerListFinish()
    {
        //请求玩家游戏状态
        GetUserGameState();

        if (connectType == ConnectType.Normal)
        {
            //界面操作
            Util.Instance.DoAction(HallEvent.V_ClosePnlLogin);
            Util.Instance.DoActionDelay(HallEvent.V_OpenPnlMain, 0.1f);
            //打开活动页面
            //Util.Instance.DoActionDelay(HallEvent.V_OpenDlgActivity, 0.6f);
        }

        Util.Instance.DoAction(GameEvent.V_RefreshUserInfo, true);
    }

    #endregion

    #region 红包

    //收到彩金
    public void OnReceiveHandsel(CMD_Hall_S_Handsel pro)
    {
        
    }

    #endregion

    #region 排行榜

    public void GetRankInfo()
    {
        //日榜
        CMD_Hall_C_GetRank pro_1 = new CMD_Hall_C_GetRank();
        pro_1.cbRankType = 1;
        client.SendPro(pro_1);
        //周榜
        CMD_Hall_C_GetRank pro_2 = new CMD_Hall_C_GetRank();
        pro_2.cbRankType = 2;
        client.SendPro(pro_2);
        //总榜
        CMD_Hall_C_GetRank pro_3 = new CMD_Hall_C_GetRank();
        pro_3.cbRankType = 3;
        client.SendPro(pro_3);
        //金币日榜
        CMD_Hall_C_GetRank pro_4 = new CMD_Hall_C_GetRank();
        pro_4.cbRankType = 4;
        client.SendPro(pro_4);
        //金币总榜
        CMD_Hall_C_GetRank pro_5 = new CMD_Hall_C_GetRank();
        pro_5.cbRankType = 5;
        client.SendPro(pro_5);
    }

    public void OnGetRankInfo(CMD_Hall_S_RankInfo pro)
    {
        if (HallModel.rankDic.ContainsKey(pro.cbRankType))
        {
            HallModel.rankDic[pro.cbRankType] = pro;
        }
        else
        {
            HallModel.rankDic.Add(pro.cbRankType, pro);
        }
        for (int i = 0; i < 20; i++)
        {
            if (!HallModel.userPhotos.ContainsKey(pro.dwUserID[i]) && pro.szWeChatURL[i].Length > 5)
            {
                WebService.Instance.LoadUserPhoto(pro.dwUserID[i], pro.szWeChatURL[i]);
            }
        }
    }

    #endregion

    #region 系统消息

    //操作成功
    public void OnOpSuccess(CMD_Hall_S_OpSuccess pro)
    {
        if (opCode == OpCode.OpenBank)
        {
            QueryBankInfo();
            HallModel.userBankPwd = bankPassword;
            //Util.Instance.DoAction(HallEvent.V_OpenDlgBank);
        }
        if (opCode == OpCode.ChangeBankPassword)
        {
            HallModel.userBankPwd = "";
            Util.Instance.DoAction(GameEvent.V_OpenShortTip, pro.szDescribeString);
        }
        if (opCode == OpCode.ChangeUserPassword)
        {
            if (HallModel.loginType == LoginType.Account || HallModel.loginType == LoginType.Register)
            {
                //修改用户密码成功，保存密码
            }
            Util.Instance.DoAction(GameEvent.V_OpenShortTip, pro.szDescribeString);
        }
        if (opCode == OpCode.ModifyNickName)
        {
            HallModel.userName = userNickName;
            Util.Instance.DoAction(GameEvent.V_RefreshUserInfo, true);
            Util.Instance.DoAction(GameEvent.V_OpenShortTip, pro.szDescribeString);
        }
        if (opCode == OpCode.SetSpreader)
        {
            int.TryParse(userReportId, out HallModel.spreaderId);
            HallModel.spreaderName = "***";
            QueryBankInfo();
            Util.Instance.DoAction(GameEvent.V_OpenShortTip, "绑定推荐人成功，请重新登录后查看详情!");
        }
    }

    //操作失败
    public void OnOpFail(CMD_Hall_S_OpFail pro)
    {
        if (GameEvent.V_OpenDlgTip != null)
        {
            GameEvent.V_OpenDlgTip.Invoke(pro.szDescribeString, "", null, null);
        }
    }

    //收到系统消息 - 管理员消息
    public void OnReceiveSystemMessage(CMD_Hall_S_SystemMessage pro)
    {
        HallModel.messageList.Add(pro.szSystemMessage);
        Util.Instance.DoAction(GameEvent.S_ReceiveSystemMsg);
    }

    //收到系统消息 - 全局消息
    public void OnReceiveSystemInfo(CMD_Hall_S_SystemInfo pro)
    {
        //Debug.LogError("收到系统消息:" + pro.szString + " type: " + pro.wType);
        if ((pro.wType & 0x0010) != 0 || (pro.wType & 0x0004) != 0 || (pro.wType & 0x0008) != 0)
        {
            //滚动消息
            HallModel.messageList.Add(pro.szString);
            Util.Instance.DoAction(GameEvent.S_ReceiveSystemMsg);
        }
        else if ((pro.wType & 0x0002) != 0 && GameEvent.V_OpenDlgTip != null)
        {
            //弹窗消息
            GameEvent.V_OpenDlgTip.Invoke(pro.szString, "", null, null);
        }

        //关闭游戏
        if ((pro.wType & 0x0100) != 0 || (pro.wType & 0x0200) != 0)
        {
            GameService.Instance.BreakConnect();
            if (GameEvent.V_OpenDlgTip != null)
            {
                GameEvent.V_OpenDlgTip.Invoke(pro.szString, "", GameService.Instance.ReturnToHall, GameService.Instance.ReturnToHall);
            }
        }
        //关闭大厅
        if ((pro.wType & 0x0400) != 0 || (pro.wType & 0x1000) != 0)
        {
            GameService.Instance.BreakConnect();
            HallService.Instance.BreakConnect();
            if (GameEvent.V_OpenDlgTip != null)
            {
                GameEvent.V_OpenDlgTip.Invoke(pro.szString, "", ReloadHallScene, ReloadHallScene);
            }
        }
    }

    //收到充值消息 - 全局消息
    public void OnReceiveRabbitMQInfo(CMD_CM_S_RabbitMQInfo pro)
    {
        switch(pro.RabbitMQInfo.nBuyType)
        {
            case 2: //钻石
            case 3: //金币
                //关闭游戏内商城
                Util.Instance.DoAction(GameEvent.V_CloseDlgStoreInGame);

                return;
            case 4: //月卡判断
                //关闭界面
                Util.Instance.DoAction(HallEvent.V_CloseDlgMonthCard);

                //设置标识
                HallModel.isBuyMonthCard = true;
                return;
            case 5: //首充判断
                //关闭界面
                Util.Instance.DoAction(GameEvent.V_CloseDlgFirstCharge);

                //设置标识
                HallModel.isFirstCharge = true;

                return;
        }
    }

    string[] messageColors = new string[] { "3ad121", "00aeff", "ff9000" };
    int colorIndex = 0;

    //收到大喇叭消息
    public void OnReceiveHornMessage(CMD_Hall_S_HornMessage pro)
    {
        string msg = pro.szTrumpetContent.Replace("\n", "");
        msg = "[" + messageColors[colorIndex] + "]【大喇叭】" + msg + "[-]";
        colorIndex++;
        if (colorIndex > 2)
        {
            colorIndex = 0;
        }
        HallModel.messageList.Add(msg);
        Util.Instance.DoAction(GameEvent.S_ReceiveSystemMsg);
    }

    #endregion

    #region 房卡相关

    //请求服务器信息
    public void GetRoomServerInfo(UInt16 serverKind, UInt16 serverLevel)
    {
        CMD_Hall_C_GetCardRoomServerInfo pro = new CMD_Hall_C_GetCardRoomServerInfo();
        pro.wServerKind = serverKind;
        pro.wServerLevel = serverLevel;
        pro.wKindID = (UInt16)HallModel.currentGameKindId;
        pro.dwRoomNumber = (uint)GameModel.currentRoomId;
        client.SendPro(pro);
    }

    //收到服务器信息
    public void OnGetRoomServerInfo(CMD_Hall_S_CardRoomServerInfo pro)
    {
        //房间不存在
        if (pro.wServerID == 0 || !HallModel.serverList.ContainsKey(pro.wServerID))
        {
            if (SceneManager.GetActiveScene().name.ToLower().Contains("hall"))
            {
                if (HallModel.opOnLoginGame == OpOnLginGame.JoinRoom && !HallModel.isStartByURL)
                {
                    Util.Instance.DoAction(HallEvent.V_GetRoomFail, pro.szDescribeString);
                }
                else if (GameEvent.V_OpenDlgTip != null)
                {
                    GameEvent.V_OpenDlgTip.Invoke(pro.szDescribeString, "", null, null);
                }
            }
            else
            {
                if (GameEvent.V_OpenDlgTip != null)
                {
                    GameEvent.V_OpenDlgTip.Invoke(pro.szDescribeString, "", ReloadHallScene, ReloadHallScene);
                }
            }
            return;
        }
        //红包场，钻石不足
        if(HallModel.serverList[pro.wServerID].wServerType == GameModel.ServerKind_RedPack && HallModel.userDiamondCount < HallModel.serverList[pro.wServerID].lEnterScore)
        {
            //金币够复活
            if(HallModel.userCoinInGame >= HallModel.ingotBaseEnsureCoinCost)
            {
                string mainMsg = "抱歉,您的钻石少于" + HallModel.serverList[pro.wServerID].lEnterScore + "颗,是否消耗" + HallModel.ingotBaseEnsureCoinCost + " 金币复活!";
                if (GameEvent.V_OpenDlgTip != null)
                {
                    GameEvent.V_OpenDlgTip.Invoke(mainMsg, "", delegate { GetBaseEnsure(0); }, delegate { HallEvent.V_OpenDlgStore.Invoke(DlgStoreArg.DiamondPage); });
                }
            }
            else
            {
                string mainMsg = "抱歉，您的钻石少于" + HallModel.serverList[pro.wServerID].lEnterScore + "颗，无法继续游戏，请充值！";
                if (GameEvent.V_OpenDlgTip != null)
                {
                    //首充判断
                    if (HallModel.isFirstCharge == false)
                    {
                        GameEvent.V_OpenDlgTip.Invoke(mainMsg, null, delegate { GameEvent.V_OpenDlgFirstCharge.Invoke(delegate { HallEvent.V_OpenDlgStore.Invoke(DlgStoreArg.DiamondPage); }); }, null);   
                    }
                    else
                    {
                        GameEvent.V_OpenDlgTip.Invoke(mainMsg, null, delegate { HallEvent.V_OpenDlgStore.Invoke(DlgStoreArg.DiamondPage); }, null);   
                    }
                }
            }
            
            
            return;
        }
        //金币场，金币不足
        if (HallModel.serverList[pro.wServerID].wServerType == GameModel.ServerKind_Gold && HallModel.userCoinInGame < HallModel.serverList[pro.wServerID].lEnterScore)
        {
            if (HallModel.currentCoinBaseEnsureTimes < HallModel.totalCoinBaseEnsureTimes)
            {
               string mainMsg = "系统第"+(HallModel.currentCoinBaseEnsureTimes + 1)+"次赠送金币,点击 确定 领取!";
               string subMsg = "每天赠送" + HallModel.totalCoinBaseEnsureTimes + "次,每次" + HallModel.coinBaseEnsureCount + "金币";
               if (GameEvent.V_OpenDlgTip != null)
               {
                   GameEvent.V_OpenDlgTip.Invoke(mainMsg, subMsg, delegate { GetBaseEnsure(1); }, null);
               }
            }
            else
            {
               string mainMsg = "抱歉，您的金币不足，无法继续游戏，请充值！";
               if (GameEvent.V_OpenDlgTip != null)
               {
                   GameEvent.V_OpenDlgTip.Invoke(mainMsg, "", delegate { Util.Instance.DoAction(HallEvent.V_OpenDlgStore, DlgStoreArg.CoinPage);}, null);
               }
            }
            // if (GameEvent.V_OpenDlgTip != null)
            // {
            //     GameEvent.V_OpenDlgTip.Invoke("抱歉，您的金币不足，请充值", "", delegate { Util.Instance.DoAction(HallEvent.V_OpenDlgStore, DlgStoreArg.CoinPage); }, null);
            // }
            return;
        }

        //房卡模式下加入房间，且非URL启动，弹出房间信息界面
        if (HallModel.serverList[pro.wServerID].wServerType == GameModel.ServerKind_Private && HallModel.opOnLoginGame == OpOnLginGame.JoinRoom && !HallModel.isStartByURL)
        {
            //房卡模式
            //Util.Instance.DoAction(HallEvent.V_CloseDlgJoinRoom);
            //if (HallEvent.V_OpenDlgRoomInfo != null)
            //{
            //    HallEvent.V_OpenDlgRoomInfo.Invoke(pro.wServerID, pro.szDescribeString);
            //}
            //return;

            HallModel.currentGameFlag = (GameFlag)HallModel.serverList[pro.wServerID].wKindID;
            HallModel.currentServerId = pro.wServerID;
            GameService.Instance.Connect();
            return;
        }

        HallModel.isStartByURL = false;
        HallModel.currentGameFlag = (GameFlag)HallModel.serverList[pro.wServerID].wKindID;
        HallModel.currentServerId = pro.wServerID;
        GameService.Instance.Connect();
    }


    //请求游戏记录
    public void GetGameRecord(GameFlag flag)
    {
        CMD_Hall_C_GetGameRecord pro = new CMD_Hall_C_GetGameRecord();
        pro.kindId = AppConfig.gameDic[flag].kindId;
        client.SendPro(pro);
    }

    //收到游戏记录
    public void OnGetGameRecord(CMD_Hall_S_GameRecord pro)
    {
        if (HallModel.gameRecordList.ContainsKey(pro.dwPrivateDrawID))
        {
            HallModel.gameRecordList[pro.dwPrivateDrawID] = pro;
        }
        else
        {
            HallModel.gameRecordList.Add(pro.dwPrivateDrawID, pro);
        }
    }

    //游戏记录
    public void OnGameRecordFinish()
    {
        Util.Instance.DoAction(HallEvent.V_RefreshDlgRecord);
    }

    //请求游戏记录详情
    public void GetRecordInfo(UInt32 UniId)
    {
        CMD_Hall_C_GetRecordInfo pro = new CMD_Hall_C_GetRecordInfo();
        pro.dwPrivateDrawID = UniId;
        client.SendPro(pro);
    }

    //收到游戏记录详情
    public void OnGetRecordInfo(CMD_Hall_S_RecordInfo pro)
    {
        //HallEvent.S_GetRecordInfo.Invoke(pro);
    }

    #endregion

    #region 低保

    //获取低保参数
    public void GetBaseEnsureInfo()
    {
        CMD_Hall_C_GetBaseEnsurePara pro = new CMD_Hall_C_GetBaseEnsurePara();
        pro.nBaseEnsureType = 0;
        client.SendPro(pro);
    }

    //低保参数
    public void OnGetBaseEnsurePara(CMD_Hall_S_BaseEnsureParamter pro)
    {
        HallModel.coinBaseEnsureCondition = (int)pro.lScoreCondition;
        HallModel.coinBaseEnsureCount = (int)pro.lScoreAmount;
        HallModel.currentCoinBaseEnsureTimes = pro.wCoinCurTakeTimes;
        HallModel.totalCoinBaseEnsureTimes = pro.wCoinTakeTimes;


        HallModel.ingotBaseEnsureCoinCost = (int)pro.lIngotAmount;
        HallModel.ingotBaseEnsureCondition = (int)pro.lIngotCondition;
        HallModel.currentIngotBaseEnsureTimes = pro.wIngotCurTakeTimes;
        HallModel.totalIngotBaseEnsureTimes = pro.wIngotTakeTimes;

    }

    //领取低保
    public void GetBaseEnsure(int type)
    {
        CMD_Hall_C_BaseEnsureTake pro = new CMD_Hall_C_BaseEnsureTake();
        pro.dwUserID = (uint)HallModel.userId;
        pro.nBaseEnsureType = type;
        pro.szPassword = HallModel.dynamicPassword;
        pro.szMachineID = Util.GetMacCode();
        client.SendPro(pro);
    }

    //领取低保
    public void OnGetBaseEnsure(CMD_Hall_S_BaseEnsureResult pro)
    {
        if (GameEvent.V_OpenDlgTip != null)
        {
            GameEvent.V_OpenDlgTip.Invoke(pro.szNotifyContent, "", null, null);
        }
        QueryBankInfo();
        if (pro.bSuccessed && pro.nBaseEnsureType == 1)
        {
            HallModel.currentCoinBaseEnsureTimes++;
        }
        if (pro.bSuccessed && pro.nBaseEnsureType == 0)
        {
            HallModel.currentIngotBaseEnsureTimes++;
        }
    }

    #endregion


    #region 每日任务

    /// <summary>
    /// 加载任务
    /// </summary>
    public void QueryTaskInfo()
    {
        CMD_CM_C_LoadTaskInfo pro = new CMD_CM_C_LoadTaskInfo();
        pro.dwUserID = (uint)HallModel.userId;
        pro.szPassword = HallModel.dynamicPassword;
        client.SendPro(pro);
    }

    /// <summary>
    /// 任务信息
    /// </summary>
    /// <param name="pro"></param>
    public void On_S_TaskInfo(CMD_CM_S_TaskInfo pro)
    {
        HallModel.taskStatus = pro.taskStatus;
    }

    /// <summary>
    /// 任务进度
    /// </summary>
    /// <param name="pro"></param>
    public void On_CM_S_TaskProgress(CMD_CM_S_TaskProgress pro)
    {
        foreach(TagTaskStatus taskItem in HallModel.taskStatus)
        {
            if (pro.wFinishTaskID != taskItem.wTaskID) continue;

            //0 为未完成  1为成功   2为失败  3已领奖
            taskItem.cbTaskStatus = pro.cbTaskStatus;
            taskItem.wTaskProgress = pro.wTaskProgress;

            break;
        }
    }

    public void TakeTaskReward(int taskID)
    {
        CMD_CM_C_TaskReward pro = new CMD_CM_C_TaskReward();
        pro.dwUserID = (uint)HallModel.userId;
        pro.wTaskID = (UInt16)taskID;
        pro.szPassword = HallModel.dynamicPassword;
        pro.szMachineID = Util.GetMacCode();
        client.SendPro(pro);
    }

    /// <summary>
    /// 任务列表
    /// </summary>
    /// <param name="pro"></param>
    public void On_S_TaskParameter(CMD_CM_S_TaskParameter pro)
    {
        HallModel.taskParameter = pro.taskParameter;
    }

    /// <summary>
    /// 领取结果
    /// </summary>
    /// <param name="pro"></param>
    public void On_CM_S_TaskResult(CMD_CM_S_TaskResult pro)
    {
        Util.Instance.DoAction(GameEvent.V_RefreshDlgTask, pro);
        
    }
    #endregion

    void ReloadHallScene()
    {
        HallModel.isReturnFromGame = false;
        SceneManager.LoadScene("Hall", LoadSceneMode.Single);
    }

}


public enum OpCode
{
    Null = 0,
    OpenBank = 1,
    ChangeBankPassword = 2,
    ChangeUserPassword = 3,
    ModifyNickName = 4,
    SetSpreader = 5,
}
