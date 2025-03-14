﻿using UnityEngine;
using System.Collections;
using System.Net;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using System.Collections.Generic;

public class GameService : MonoBehaviour
{
    private static GameService _instance;
    public static GameService Instance
    {
        get
        {
            if (_instance == null)
            {
                string name = "GameService";
                GameObject obj = GameObject.Find(name);
                if (obj == null)
                {
                    obj = new GameObject(name);
                    _instance = obj.AddComponent<GameService>();
                }
                else
                {
                    _instance = obj.GetComponent<GameService>();
                    if (_instance == null)
                    {
                        _instance = obj.AddComponent<GameService>();
                    }
                }
            }
            return _instance;
        }
    }

    [HideInInspector]
    public ClientSocket client = HallService.Instance.client;
    [HideInInspector]
    public bool isConnect = false;

    private bool isAutoConnectOnBreak = false;       //网络断开后是否主动断开连接

    void Awake()
    {
        //client = new ClientSocket();
        //client.netManager.AddHandler(new GameHandler());
        //client.connectErrorEvent += OnConnectError;
        DontDestroyOnLoad(this.gameObject);
    }

    void FixedUpdate()
    {
        //client.netManager.OnUpdate();   //实时接收网络消息
    }

    //网络监测
    //void ConnectDetection()
    //{
    //    if (!isConnect)
    //    {
    //        BreakConnect();
    //        if (isAutoConnectOnBreak)
    //        {
    //            //Connect();
    //            HallService.Instance.GetUserGameState();
    //        }
    //    }
    //}

    //游戏状态监测
    void OnApplicationPause(bool state)
    {
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            if (!state)
            {
                //开始网络监测
                //CancelInvoke("ConnectDetection");
                //InvokeRepeating("ConnectDetection", 1f, 5f);
                //发送心跳包
                CancelInvoke("SendHeartPacket");
                InvokeRepeating("SendHeartPacket", 1f, 5f);
                //网络测试
                CancelInvoke("TestSpeed");
                InvokeRepeating("TestSpeed", 1f, 5f);
            }
            else
            {
                //停止网络监测
                //CancelInvoke("ConnectDetection");
                //CancelInvoke("SendHeartPacket");

                //关闭网络连接
                //if (Application.platform == RuntimePlatform.Android)
                //{
                //    BreakConnect();
                //}
            }
        }
    }

    #region 登录

    //连接服务器
    public void Connect()
    {
        try
        {
            isAutoConnectOnBreak = true;
            if (!HallModel.serverList.ContainsKey(HallModel.currentServerId))
            {
                AudioManager.Instance.PlaySound(GameModel.audioTipWarn);
                Util.Instance.DoAction(GameEvent.V_OpenShortTip, "所选游戏服务器不存在 ： " + HallModel.currentServerId);
                return;
            }
            if (SceneManager.GetActiveScene().name.ToLower().Contains("hall"))
            {
                Util.Instance.DoAction(GameEvent.V_OpenConnectTip, "正在进入游戏...");
            }
            //string gameIp = HallModel.serverList[HallModel.currentServerId].szServerAddr;
            string gameIp = AppConfig.serverUrl[AppConfig.serverIndex];
            UInt16 gamePort = HallModel.serverList[HallModel.currentServerId].wServerPort;
            IPAddress[] ips = Dns.GetHostAddresses(gameIp);
            client.Connect(ips[0].ToString(), gamePort);
        }
        catch
        {
            if (GameEvent.V_OpenDlgTip != null)
            {
                GameEvent.V_OpenDlgTip.Invoke("连接游戏服务器失败 ： " + HallModel.currentServerId, "", null, null);
            }
        }
    }

    //断开连接
    //public void BreakConnect()
    //{
    //    isConnect = false;
    //    client.CloseConnect();
    //    Util.Instance.DoAction(GameEvent.V_CloseConnectTip);
    //    CancelInvoke("ConnectDetection");
    //    CancelInvoke("TestSpeed");
    //}

    //网速测试
    public void TestSpeed()
    {
        CMD_Game_CS_NetSpeed pro = new CMD_Game_CS_NetSpeed();
        pro.time = Time.time;
        client.SendPro(pro);
    }

    public void OnGetTestSpeedPack(CMD_Game_CS_NetSpeed pro)
    {
        float deltaTime = Time.time - pro.time;
        if (deltaTime * 1000 < 100)
        {
            GameModel.netSpeed = 3;
        }
        else if (deltaTime * 1000 < 200)
        {
            GameModel.netSpeed = 2;
        }
        else if (deltaTime * 1000 < 500)
        {
            GameModel.netSpeed = 1;
        }
        else
        {
            GameModel.netSpeed = 0;
        }
    }

    //链接服务器成功
    //public void OnConnectSuccess()
    //{
    //    isConnect = true;
    //    //开始网络连接状态监测
    //    CancelInvoke("ConnectDetection");
    //    InvokeRepeating("ConnectDetection", 3f, 5f);
    //    //发送心跳包
    //    CancelInvoke("SendHeartPacket");
    //    InvokeRepeating("SendHeartPacket", 1f, 5f);
    //    //网络测试
    //    CancelInvoke("TestSpeed");
    //    InvokeRepeating("TestSpeed", 1f, 5f);
    //    //关闭连接提示
    //    //Util.Instance.DoAction(GameEvent.V_CloseConnectTip);
    //    //登录游戏
    //    LoginGame();
    //}

    //网络连接异常
    public void OnConnectError()
    {
        Debug.Log("网络链接异常....");
        isConnect = false;
        client.CloseConnect();
    }


    //登录游戏
    //public void LoginGame()
    //{
    //    CMD_Game_C_LoginGame pro = new CMD_Game_C_LoginGame();

    //    pro.wGameID = (uint)HallModel.gameId;
    //    pro.cbDeviceType = Util.GetDeviceType();
    //    pro.wBehaviorFlags = 0;        //0x1000表示登录成功后自动坐下   0-表示不坐下
    //    pro.dwUserID = (uint)HallModel.userId;
    //    pro.szPassword = HallModel.dynamicPassword;
    //    pro.szServerPasswd = "";
    //    pro.szMachineID = Util.GetMacCode();

    //    pro.fLatitude = GPSManager.latitude;
    //    pro.fLongitude = GPSManager.longitude;

    //    client.SendPro(pro);
    //    //初始化用户列表
    //    GameModel.deskId = 65535;
    //    GameModel.chairId = 65535;
    //    GameModel.playerInRoom.Clear();
    //    GameModel.playerInDesk.Clear();
    //}

    //收到游戏服务器配置信息
    public bool OnReceiveGameServerConfig(Bs.Gateway.TransferDataReq req)
    {
        Bs.Room.ConfigServer rsp = NetPacket.Deserialize<Bs.Room.ConfigServer>(req.Data.ToByteArray());

        GameModel.roomDeskCount = (int)rsp.TableCount;
        GameModel.deskPlayerCount = (int)rsp.ChairCount;
        GameModel.roomCoinLimit = rsp.MinTableScore;
        return true;
    }

    //收到桌子信息
    public bool OnReceiveTableInfo(Bs.Gateway.TransferDataReq req)
    {
        Bs.Room.TableInfo rsp = NetPacket.Deserialize<Bs.Room.TableInfo>(req.Data.ToByteArray());
        GameModel.tableList.Clear();
        foreach (var t in rsp.TableStatusArray)
        {
            TagTableState ts = new TagTableState();
            ts.cbTableLock = (byte)t.TableLock;
            ts.cbPlayStatus = (byte)t.PlayStatus;
            ts.lCellScore = t.CellScore;
            GameModel.tableList.Add(ts);
        }
        return true;
    }

    //收到桌子状态
    public bool OnReceiveTableState(Bs.Gateway.TransferDataReq req)
    {
        Bs.Room.TableStatus rsp = NetPacket.Deserialize<Bs.Room.TableStatus>(req.Data.ToByteArray());

        TagTableState ts = new TagTableState();
        ts.cbTableLock = (byte)rsp.TableStatus_.TableLock;
        ts.cbPlayStatus = (byte)rsp.TableStatus_.PlayStatus;
        ts.lCellScore = (byte)rsp.TableStatus_.CellScore;
        GameModel.tableList[(int)rsp.TableId] = ts;

        //刷新桌子状态
        if (SceneManager.GetActiveScene().name.ToLower().Contains("hall"))
        {
            Util.Instance.DoAction(GameEvent.S_RefreshDeskState);
        }

        return true;
    }

    // 用户进入房间
    public bool OnUserCome(Bs.Gateway.TransferDataReq req)
    {
        Bs.Room.UserEnter rsp = NetPacket.Deserialize<Bs.Room.UserEnter>(req.Data.ToByteArray());

        PlayerInRoom player = new PlayerInRoom();
        player.dwUserID = rsp.UserId;
        player.dwGameID = rsp.GameId;
        player.dwGroupID = rsp.GameId;
        player.wFaceID = (UInt16)rsp.FaceId;

        player.cbUserStatus = (Byte)rsp.UserStatus;
        player.wTableID = (UInt16)rsp.TableId;
        player.wChairID = (UInt16)rsp.ChairId;

        player.lScore = rsp.Score;
        player.lIngot = rsp.Ingot;
        player.lInsure = rsp.Insure;
        player.redPack = rsp.RedRevelopes;

        player.nickName = rsp.NickName;


        Debug.Log("用户进入,player=" + JsonUtility.ToJson(player) + ",userId=" + HallModel.userId);

        //自己进入房间，初始化自己的桌号、座位号
        if (rsp.UserId == HallModel.userId)
        {
            GameModel.deskId = player.wTableID;
            GameModel.chairId = player.wChairID;
        }
        //维护房间用户列表
        if (GameModel.playerInRoom.ContainsKey(rsp.UserId))
        {
            GameModel.playerInRoom[rsp.UserId] = player;
        }
        else
        {
            GameModel.playerInRoom.Add(rsp.UserId, player);
        }
        //维护本桌用户列表
        if (GameModel.deskId != GameModel.INVALID_TABLE && player.wTableID == GameModel.deskId)
        {
            if (GameModel.playerInDesk.ContainsKey(player.wChairID))
            {
                GameModel.playerInDesk[player.wChairID] = rsp.UserId;
                Util.Instance.DoAction(GameEvent.V_RefreshUserInfo, false);
            }
            else
            {
                GameModel.playerInDesk.Add(player.wChairID, rsp.UserId);
                Util.Instance.DoAction(GameEvent.V_RefreshUserInfo, true);
            }
        }
        return true;
    }

    //登陆完成
    public bool OnLoginFinish(Bs.Gateway.TransferDataReq req)
    {
        Bs.Room.LogonFinish rsp = NetPacket.Deserialize<Bs.Room.LogonFinish>(req.Data.ToByteArray());
        Debug.Log("登陆完成,rsp.GuideTask=" + rsp.GuideTask + ",deskId=" + GameModel.deskId + ",opOnLoginGame=" + HallModel.opOnLoginGame);

        GameModel.serverType = HallModel.serverList[HallModel.currentServerId].wServerType;
        GameModel.serverRule = (int)HallModel.serverList[HallModel.currentServerId].dwServerRule;
        if (GameModel.deskId == GameModel.INVALID_TABLE)
        {
            //用户状态为空
            switch (HallModel.opOnLoginGame)
            {
                case OpOnLginGame.GetChair:
                    GetChair();
                    break;
                case OpOnLginGame.CreateRoom:
                    CreateRoom();
                    break;
                case OpOnLginGame.JoinRoom:
                    JoinRoom();
                    break;
            }
        }
        else
        {
            //查找同桌用户
            foreach (PlayerInRoom player in GameModel.playerInRoom.Values)
            {
                if (player.wTableID == GameModel.deskId)
                {
                    if (GameModel.playerInDesk.ContainsKey(player.wChairID))
                    {
                        GameModel.playerInDesk[player.wChairID] = player.dwUserID;
                    }
                    else
                    {
                        GameModel.playerInDesk.Add(player.wChairID, player.dwUserID);
                    }
                }
            }
        }

        //跳游戏界面
        if (SceneManager.GetActiveScene().name.ToLower().Contains("hall"))
        {
            LoadGameScene();
        }
        else
        {
            GetGameStatus();
        }

        //获取房间宝藏信息
        QueryDigTreasure();

        return true;
    }

    //登陆失败
    public void OnLoginFail(CMD_Game_S_LoginGameFail pro)
    {
        Util.Instance.DoAction(GameEvent.V_CloseConnectTip);
        //尝试各种方法对服务端传过来的空字符串做判断全部是失败,暂时使用特殊字符串
        //string.IsNullOrEmpty(pro.szDescribeString)
        //pro.szDescribeString.Trim() != string.Empty
        //pro.szDescribeString != ""
        if (GameEvent.V_OpenDlgTip != null && pro.szDescribeString != "不要弹框")
        {
            GameEvent.V_OpenDlgTip.Invoke(pro.szDescribeString, "", null, null);
        }
    }

    #endregion

    #region 状态

    //等待分配
    public bool OnReceiveWaitDistribute()
    {
        Util.Instance.DoAction(GameEvent.V_ShowWaitMatch, true);
        Util.Instance.DoAction(LandlordsEvent.V_CloseDlgResult);
        Util.Instance.DoAction(LandlordsEvent.V_ReStartNewGame);
        return true;
    }

    //收到用户状态
    public bool OnReceiveUserState(Bs.Gateway.TransferDataReq req)
    {
        Bs.Room.UserStatus rsp = NetPacket.Deserialize<Bs.Room.UserStatus>(req.Data.ToByteArray());
        uint dwUserId = (uint)rsp.UserInfo.UserId;
        byte cbUserStatus = (byte)rsp.UserInfo.Status;
        UInt16 wTableID = (UInt16)rsp.UserInfo.TableId;
        UInt16 wChairID = (UInt16)rsp.UserInfo.SeatId;

        Debug.Log("收到用户状态,dwUserId=" + dwUserId + ",HallModel.userId=" + HallModel.userId
            + ",cbUserStatus=" + cbUserStatus
            + ",wTableID=" + wTableID
            + ",wChairID=" + wChairID
            + ",in=" + GameModel.playerInRoom.ContainsKey((uint)HallModel.userId));

        //自己
        if (dwUserId == HallModel.userId)
        {
            //初始化自己的座位号，桌号
            GameModel.deskId = wTableID;
            GameModel.chairId = wChairID;
            //坐下时
            if (GameModel.playerInRoom.ContainsKey((uint)HallModel.userId) && GameModel.playerInRoom[(uint)HallModel.userId].cbUserStatus == UserState.US_FREE)
            {
                if (cbUserStatus == UserState.US_SIT || cbUserStatus == UserState.US_READY || cbUserStatus == UserState.US_PLAYING)
                {
                    //清空数据
                    GameModel.playerInDesk.Clear();
                    //查找同桌用户
                    foreach (PlayerInRoom player in GameModel.playerInRoom.Values)
                    {
                        if (player.wTableID == GameModel.deskId)
                        {
                            if (GameModel.playerInDesk.ContainsKey(player.wChairID))
                            {
                                GameModel.playerInDesk[player.wChairID] = player.dwUserID;
                            }
                            else
                            {
                                GameModel.playerInDesk.Add(player.wChairID, player.dwUserID);
                            }
                            HallService.Instance.QueryUserInfo(player.dwUserID);
                        }
                    }

                    Debug.Log("收到用户状态,坐下,GameModel.playerInDesk.Count=" + GameModel.playerInDesk.Count
                        + ",GameModel.playerInRoom.Count=" + GameModel.playerInRoom.Count
                        + ",HallModel.userId=" + HallModel.userId);

                    if (SceneManager.GetActiveScene().name.ToLower().Contains("hall"))
                    {
                        //初次坐下
                        LoadGameScene();
                    }
                    else
                    {
                        Debug.LogError("请求状态");
                        //换桌时坐下
                        GetGameStatus();
                    }
                }
            }
        }

        //更新用户列表中的状态
        if (GameModel.playerInRoom.ContainsKey(dwUserId))
        {
            GameModel.playerInRoom[dwUserId].cbUserStatus = cbUserStatus;
            GameModel.playerInRoom[dwUserId].wTableID = wTableID;
            GameModel.playerInRoom[dwUserId].wChairID = wChairID;

            //用户状态为空时，表示离开房间
            if (cbUserStatus == UserState.US_NULL)
            {
                GameModel.playerInRoom.Remove(dwUserId);
            }
        }
        //维护本桌用户列表
        if (cbUserStatus == UserState.US_FREE || cbUserStatus == UserState.US_NULL)
        {
            //站起时从本桌用户列表中移除用户
            int chairID = -1;
            foreach (int key in GameModel.playerInDesk.Keys)
            {
                if (GameModel.playerInDesk[key] == dwUserId)
                {
                    chairID = key;
                }
            }
            if (chairID != -1)
            {
                GameModel.playerInDesk.Remove(chairID);
                Util.Instance.DoAction(GameEvent.V_RefreshUserInfo, false);
            }
        }
        else if (GameModel.deskId != GameModel.INVALID_TABLE && GameModel.deskId == wTableID)
        {
            if (GameModel.playerInDesk.ContainsKey(wChairID))
            {
                if (GameModel.playerInDesk[wChairID] != dwUserId)
                {
                    GameModel.playerInDesk[wChairID] = dwUserId;
                    HallService.Instance.QueryUserInfo(dwUserId);
                }
                Util.Instance.DoAction(GameEvent.V_RefreshUserInfo, false);
            }
            else
            {
                //玩家坐在空位
                GameModel.playerInDesk.Add(wChairID, dwUserId);

                Debug.Log("收到用户状态,添加,dwUserId=" + dwUserId + ",HallModel.userId=" + HallModel.userId
                    + ",cbUserStatus=" + cbUserStatus
                    + ",wTableID=" + wTableID
                    + ",wChairID=" + wChairID
                    + ",GameModel.playerInDesk.Count=" + GameModel.playerInDesk.Count);

                HallService.Instance.QueryUserInfo(dwUserId);
                Util.Instance.DoAction(GameEvent.V_RefreshUserInfo, true);
            }
        }

        //刷新桌子状态
        if (SceneManager.GetActiveScene().name.ToLower().Contains("hall"))
        {
            Util.Instance.DoAction(GameEvent.S_RefreshDeskState);
        }

        return true;
    }

    //用户分数变化
    public bool OnReceiveUserScore(Bs.Gateway.TransferDataReq req)
    {
        Bs.Room.UserScore rsp = NetPacket.Deserialize<Bs.Room.UserScore>(req.Data.ToByteArray());

        //更新用户列表中的玩家积分
        if (GameModel.playerInRoom.ContainsKey(rsp.UserId))
        {
            GameModel.playerInRoom[rsp.UserId].lScore = rsp.UserScore_.Score;
            GameModel.playerInRoom[rsp.UserId].lGrade = rsp.UserScore_.Grade;
            GameModel.playerInRoom[rsp.UserId].lInsure = rsp.UserScore_.Insure;
            GameModel.playerInRoom[rsp.UserId].lIngot = rsp.UserScore_.Ingot;
            GameModel.playerInRoom[rsp.UserId].redPack = rsp.UserScore_.RedRevelopes;
            GameModel.playerInRoom[rsp.UserId].dBeans = rsp.UserScore_.Beans;

            GameModel.playerInRoom[rsp.UserId].dwWinCount = rsp.UserScore_.WinCount;
            GameModel.playerInRoom[rsp.UserId].dwLostCount = rsp.UserScore_.LostCount;
            GameModel.playerInRoom[rsp.UserId].dwDrawCount = rsp.UserScore_.DrawCount;
            GameModel.playerInRoom[rsp.UserId].dwFleeCount = rsp.UserScore_.FleeCount;
            GameModel.playerInRoom[rsp.UserId].lIntegralCount = rsp.UserScore_.IntegralCount;

            GameModel.playerInRoom[rsp.UserId].dwExperience = rsp.UserScore_.Experience;
            GameModel.playerInRoom[rsp.UserId].lLoveLiness = rsp.UserScore_.LoveLiness;
        }

        return true;
    }

    #endregion

    #region 用户操作

    //进入房间
    public void EnterRoom(UInt16 serverLevel)
    {
        // 从符合条件的元素中随机选取一个
        KeyValuePair<UInt64, GameServerInfo>? randomServer = GetRandomServer(HallModel.serverList, info => (info.wServerLevel == serverLevel));
        if (!randomServer.HasValue)
        {
            if (GameEvent.V_OpenDlgTip != null)
            {
                GameEvent.V_OpenDlgTip.Invoke("未找到合适房间,请尝试其他房间或重新登录刷新列表", "", null, null);
            }
            return;
        }

        //初始化用户列表
        GameModel.deskId = GameModel.INVALID_TABLE;
        GameModel.chairId = GameModel.INVALID_CHAIR;
        GameModel.playerInRoom.Clear();
        GameModel.playerInDesk.Clear();

        var baseInfo = randomServer.Value.Value.baseInfo;
        HallModel.currentServerId = Util.CombineUInt64(baseInfo.dwType, baseInfo.dwID);
        Debug.Log("进入房间,type=" + baseInfo.dwType + ",ID=" + baseInfo.dwID + ",currentServerId=" + HallModel.currentServerId);

        Bs.Room.EnterReq req = new Bs.Room.EnterReq();
        client.SendTransferData2Gate(baseInfo.dwType, baseInfo.dwID, NetManager.AppRoom, (UInt32)(Bs.Room.CMDRoom.IdenterReq), req);

        if (SceneManager.GetActiveScene().name.ToLower().Contains("hall"))
        {
            Util.Instance.DoAction(GameEvent.V_OpenConnectTip, "正在进入游戏...");
        }
    }

    private KeyValuePair<UInt64, GameServerInfo>? GetRandomServer(Dictionary<UInt64, GameServerInfo> servers, Func<GameServerInfo, bool> condition)
    {
        // 筛选出符合条件的元素
        List<KeyValuePair<UInt64, GameServerInfo>> availableServers = new List<KeyValuePair<UInt64, GameServerInfo>>();
        foreach (var server in servers)
        {
            if (condition(server.Value))
            {
                availableServers.Add(server);
            }
        }

        // 如果没有符合条件的元素，返回 null
        if (availableServers.Count == 0)
        {
            return null;
        }

        // 创建一个随机数生成器
        System.Random random = new System.Random();
        // 随机选取一个索引
        int randomIndex = random.Next(0, availableServers.Count);
        // 返回随机选取的元素
        return availableServers[randomIndex];
    }

    //进入回复
    public bool EnterRoomRsp(Bs.Gateway.TransferDataReq req)
    {
        Bs.Room.EnterRsp rsp = NetPacket.Deserialize<Bs.Room.EnterRsp>(req.Data.ToByteArray());
        Debug.Log("进入回复,rsp.ErrInfo.Code=" + rsp.ErrInfo.Code +",info=" + rsp.ErrInfo.Info);

        if(rsp.ErrInfo.Code != Bs.Types.ErrorInfo.Types.ResultCode.Success)
        {
            Util.Instance.DoAction(GameEvent.V_CloseConnectTip);
            GameEvent.V_OpenDlgTip.Invoke(rsp.ErrInfo.Info, "", null, null);
        }

        return true;

        if (rsp.ErrInfo.Code == Bs.Types.ErrorInfo.Types.ResultCode.Success)
        {
            GetChair();
        }
        else
        {
            GameEvent.V_OpenDlgTip.Invoke(rsp.ErrInfo.Info, "", null, null);
        }

        return true;
    }

    //请求游戏状态
    public void GetGameStatus()
    {
        Bs.Room.GameOption req = new Bs.Room.GameOption();
        client.SendTransferData2Gate(Util.GetHighUint32(HallModel.currentServerId), Util.GetLowUint32(HallModel.currentServerId), NetManager.AppRoom, (UInt32)(Bs.Room.CMDRoom.IdgameOption), req);

        //CMD_Game_C_GetGameStatus pro = new CMD_Game_C_GetGameStatus();
        //client.SendPro(pro);
    }

    // 用户坐下
    public void UserSit(int tableId, int chairId)
    {
        if (GameModel.deskId != 65535 || GameModel.chairId != 65535)
        {
            return;
        }
        CMD_Game_C_UserSit pro = new CMD_Game_C_UserSit();
        pro.wTableID = (UInt16)tableId;
        pro.wChairID = (UInt16)chairId;
        pro.szPassword = "";
        client.SendPro(pro);
    }

    //请求座位
    public void GetChair()
    {
        Debug.Log("请求座位,GetChair,deskId=" + GameModel.deskId + ",chairId=" + GameModel.chairId + ",currentServerId=" + HallModel.currentServerId);
        if (GameModel.deskId != GameModel.INVALID_TABLE || GameModel.chairId != GameModel.INVALID_CHAIR)
        {
            return;
        }

        Bs.Room.GetChairReq req = new Bs.Room.GetChairReq();
        req.TableId = GameModel.INVALID_TABLE;
        req.ChairId = GameModel.INVALID_CHAIR;
        req.Password = "";
        client.SendTransferData2Gate(Util.GetHighUint32(HallModel.currentServerId), Util.GetLowUint32(HallModel.currentServerId), NetManager.AppRoom, (UInt32)(Bs.Room.CMDRoom.IdgetChairReq), req);

        //CMD_Game_C_UserSit pro = new CMD_Game_C_UserSit();
        //pro.wTableID = 65535;
        //pro.wChairID = 65535;
        //pro.szPassword = "";
        //client.SendPro(pro);
        //清空本桌用户列表
        GameModel.playerInDesk.Clear();
    }

    //请求换桌
    public void ChangeChair()
    {
    }

    //请求起立
    public void UserStand()
    {
        if (GameModel.playerInRoom.ContainsKey((uint)HallModel.userId))
        {
            //红包场必须先发起立消息，否则无法判断是否因为充值断线，还是普通离开
            if (GameModel.serverType != GameModel.ServerKind_RedPack && (GameModel.playerInRoom[(uint)HallModel.userId].cbUserStatus == UserState.US_NULL || GameModel.playerInRoom[(uint)HallModel.userId].cbUserStatus == UserState.US_FREE))
            {
                ReturnToHall();
            }
            else
            {
                CMD_Game_C_UserStand pro = new CMD_Game_C_UserStand();
                pro.wTableID = (ushort)GameModel.deskId;
                pro.wChairID = (ushort)GameModel.chairId;
                pro.cbForceLeave = (GameModel.isInGame == true) ? (byte)0 : (byte)1;
                client.SendPro(pro);

                Invoke("ReturnToHall", 0.5f);
            }
        }
    }

    //准备
    public void UserAgree()
    {
        Debug.Log("准备");
    }

    #endregion

    #region 房间操作

    //创建房间
    public void CreateRoom()
    {
        CMD_Game_C_CreateRoom pro = new CMD_Game_C_CreateRoom();
        pro.cbGameType = (byte)GameModel.rulePayMode;
        pro.bPlayCoutIdex = (byte)GameModel.ruleGameCountIndex;
        pro.dwGameRule = (uint)GameModel.gameRule;
        pro.dwMaxTimes = (uint)GameModel.ruleGameGrade;
        pro.dwFanTimes = (uint)GameModel.ruleBaseScore;
        client.SendPro(pro);
    }

    //加入房间
    public void JoinRoom()
    {
        CMD_Game_C_JoinRoom pro = new CMD_Game_C_JoinRoom();
        pro.cbRoomType = 0; //房间类型 0普通类型、1自定类型、2 AA 类型、4代开类型、8萧山类型
        pro.dwRoomNum = (uint)GameModel.currentRoomId;
        client.SendPro(pro);
    }

    //解散房间<游戏开始前，房主发起>
    public void DisRoomBeforeGame()
    {
        CMD_Game_C_PrivateDismiss pro = new CMD_Game_C_PrivateDismiss();
        client.SendPro(pro);
    }

    //发起解散房间申请
    public void DisRoomInGame()
    {
    }

    //是否解散房间<用户选择>
    public void DisRoom(bool isDisRoom)
    {
        CMD_Game_C_DisRoom pro = new CMD_Game_C_DisRoom();
        int isDis = isDisRoom ? 1 : 0;
        pro.isDisRoom = (byte)isDis;
        client.SendPro(pro);
    }

    //收到创建房间结果
    public void OnGetCreateRoomResult(CMD_Game_S_CreateRoomResult pro)
    {
        Util.Instance.DoAction(GameEvent.V_OpenShortTip, "创建房间成功，邀请好友来游戏吧！");
    }

    //收到房间信息
    public void OnReceiveRoomInfo(CMD_Game_S_RoomInfo pro)
    {
        GameModel.currentRoomId = pro.dwRoomNum;

        GameModel.currentGameCount = (int)pro.dwPlayCout;
        GameModel.totalGameCount = (int)pro.dwPlayTotal;

        GameModel.gameRule = (int)pro.dwGameRule;
        GameModel.ruleGameGrade = (int)pro.dwMaxTimes;

        GameModel.hostUserId = (ulong)pro.dwCreateUserID;

        Util.Instance.DoAction(GameEvent.V_RefreshUserInfo, false);
        Util.Instance.DoAction(GameEvent.V_RefreshRoomInfo);
    }

    //收到解散房间信息，等待用户选择是否同意解散
    public void OnReceiveDisRoomInfo(CMD_Game_S_DisRoomInfo pro)
    {
        //开始解散房间投票
        Util.Instance.DoAction(GameEvent.V_OpenDlgDisRoom, pro);
        //可参与投票人数
        int playerCount = 0;
        for (int i = 0; i < 8; i++)
        {
            PlayerInRoom player = GameModel.GetDeskUser(i);
            if (player != null && player.cbUserStatus != UserState.US_OFFLINE)
            {
                playerCount++;
            }
        }
        //投票结束
        if (pro.agreeUserCount + pro.disAgreeUserCount >= playerCount)
        {
            if (pro.agreeUserCount < playerCount)
            {
                Util.Instance.DoAction(GameEvent.V_OpenShortTip, "投票结束，有" + pro.disAgreeUserCount + "位玩家不同意解散，请继续游戏！");
            }

            Util.Instance.DoActionDelay(GameEvent.V_CloseDlgDisRoom, 0.5f);
        }
    }

    //收到解散房间结果
    public void OnReceiveDisRoomResult(CMD_Game_S_DisRoomResult pro)
    {
        if (pro.dwRoomNumber != GameModel.currentRoomId) { return; }
        //断开连接
        isAutoConnectOnBreak = false;
        //Util.Instance.DoActionDelay(BreakConnect, 0.3f);
        //提示
        Util.Instance.DoAction(GameEvent.V_OpenShortTip, "游戏房间已解散！");
        //解散房间
        GameModel.isInGame = false;
        Util.Instance.DoActionDelay(GameEvent.S_OnDisRoom, 0.5f);
        //数据更新
        GameModel.currentGameCount = 0;
    }

    //收到房卡数量信息
    public void OnReceiveCardCount(CMD_Game_S_CardCount pro)
    {
        if (pro.dwUserID == HallModel.userId)
        {
            HallModel.userCardCount = (int)pro.dwFKPropCount;
        }
    }

    //房卡结束，收到总结算消息
    public void OnGetTotalScore(CMD_Game_S_TotalScore pro)
    {
        Util.Instance.DoAction(GameEvent.S_GetTotalScore, pro);

        HallModel.gameRecordList.Clear();
    }

    #endregion

    #region 聊天

    //发送聊天信息-文字
    public void SendChatMessage(ChatMessage chatMessage)
    {
        chatMessage.EncodeMessage();
        CMD_Game_C_ChatMessage pro = new CMD_Game_C_ChatMessage();
        pro.szChatString = chatMessage.message;
        client.SendPro(pro);
    }

    //发送聊天信息-语音
    public void SendAudioMessage(byte[] data)
    {
        if (data == null || data.Length == 0) { return; }
        CMD_Game_C_AudioMessage pro = new CMD_Game_C_AudioMessage();
        pro.dwSendUserID = (UInt32)HallModel.userId;
        Array.Copy(data, pro.cbVoiceData, data.Length);
        client.SendPro(pro);
    }

    //收到聊天信息-文字
    public void OnReceiveChatMessage(CMD_Game_S_ChatMessage pro)
    {
        ChatMessage message = new ChatMessage();
        message.message = pro.szChatString;
        message.time = DateTime.Now.ToShortTimeString().ToString();


        if (GameModel.playerInRoom.ContainsKey(pro.dwSendUserID))
        {
            message.userName = GameModel.playerInRoom[pro.dwSendUserID].nickName;
            message.sex = GameModel.playerInRoom[pro.dwSendUserID].cbGender;
            message.chairId = GameModel.playerInRoom[pro.dwSendUserID].wChairID;
            message.DecodeMessage();
            if (GameEvent.S_ReceiveChatMessage != null)
            {
                GameEvent.S_ReceiveChatMessage.Invoke(message);
            }
        }
        else
        {
            Util.Instance.DoAction(GameEvent.V_OpenShortTip, "未知用户聊天信息");
        }

    }

    //收到聊天信息-语音
    public void OnReceiveAudioMessage(CMD_Game_S_AudioMessage pro)
    {
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            ChatMessage message = new ChatMessage();
            message.type = ChatMessageType.Audio;
            message.clip = MicroPhoneManager.Instance.GetAudioClip(pro.cbVoiceData);
            message.time = DateTime.Now.ToShortTimeString().ToString();
            if (GameModel.playerInRoom.ContainsKey(pro.dwSendUserID))
            {
                message.userName = GameModel.playerInRoom[pro.dwSendUserID].nickName;
                message.sex = GameModel.playerInRoom[pro.dwSendUserID].cbGender;
                message.chairId = GameModel.playerInRoom[pro.dwSendUserID].wChairID;

                Util.Instance.DoAction(GameEvent.S_ReceiveChatMessage, message);
            }
            else
            {
                Util.Instance.DoAction(GameEvent.V_OpenShortTip, "未知用户聊天信息");
            }
        }
    }

    #endregion

    #region 系统消息

    //请求失败
    public bool OnRequestFailure(Bs.Gateway.TransferDataReq req)
    {
        Bs.Room.RequestFailure rsp = NetPacket.Deserialize<Bs.Room.RequestFailure>(req.Data.ToByteArray());
        if (GameEvent.V_OpenDlgTip != null)
        {
            GameEvent.V_OpenDlgTip.Invoke(rsp.ErrInfo.Info, "", null, null);
        }
        return true;
    }
    //    public void OnRequestFailure(CMD_Game_S_RequestFailure pro)
    //{
    //    if (GameEvent.V_OpenDlgTip != null)
    //    {
    //        GameEvent.V_OpenDlgTip.Invoke(pro.szDescribeString, "", null, null);
    //    }
    //}

    //收到系统消息
    public bool OnReceiveSystemInfo(Bs.Gateway.TransferDataReq req)
    {
        Bs.Room.SystemMessage rsp = NetPacket.Deserialize<Bs.Room.SystemMessage>(req.Data.ToByteArray());

        if ((rsp.Type & 0x0010) != 0 || (rsp.Type & 0x0004) != 0 || (rsp.Type & 0x0008) != 0)
        {
            //滚动消息
            HallModel.messageList.Add(rsp.Info);
            Util.Instance.DoAction(GameEvent.S_ReceiveSystemMsg);
        }
        else if ((rsp.Type & 0x0002) != 0 && GameEvent.V_OpenDlgTip != null)
        {
            //弹窗消息
            GameEvent.V_OpenDlgTip.Invoke(rsp.Info, "", null, null);
        }
        //返回大厅
        if ((rsp.Type & 0x0100) != 0 || (rsp.Type & 0x0200) != 0 || (rsp.Type & 0x0400) != 0)
        {
            //GameService.Instance.BreakConnect();
            if (GameEvent.V_OpenDlgTip != null)
            {
                GameEvent.V_OpenDlgTip.Invoke(rsp.Info, "", ReturnToHall, ReturnToHall);
            }
        }

        return true;
    }

    #endregion

    #region 每日任务
    /// <summary>
    /// 任务进度
    /// </summary>
    /// <param name="pro"></param>
    public void On_CM_S_TaskProgress(CMD_CM_S_TaskProgress pro)
    {
        Debug.LogError("游戏内 任务进度,ID" + pro.wFinishTaskID + ",进度," + pro.wTaskProgress);

        foreach (TagTaskStatus taskItem in HallModel.taskStatus)
        {
            if (pro.wFinishTaskID != taskItem.wTaskID) continue;

            //0 为未完成  1为成功   2为失败  3已领奖
            taskItem.cbTaskStatus = pro.cbTaskStatus;
            taskItem.wTaskProgress = pro.wTaskProgress;

            break;
        }
    }
    #endregion

    #region 挖宝
    /// <summary>
    /// 加载配置
    /// </summary>
    public void QueryDigTreasure()
    {
        //TODO
        return;

        //加载配置
        CMD_CM_C_LoadDigInfo pro = new CMD_CM_C_LoadDigInfo();
        client.SendPro(pro);
    }

    /// <summary>
    /// 开始挖宝
    /// </summary>
    public void DigTreasure()
    {
        CMD_CM_C_DigTreasure pro = new CMD_CM_C_DigTreasure();
        client.SendPro(pro);
    }

    public void On_CM_S_LoadDigInfo(CMD_CM_S_LoadDigInfo pro)
    {
        HallModel.digConfig = pro.digConfig;
    }

    public void On_CM_S_OperateDig(CMD_CM_S_OperateDig pro)
    {
        HallModel.curPlayCount = pro.nCurDrawCount;
        HallModel.totalPlayCount = pro.nTotalDrawCount;

        Util.Instance.DoAction(GameEvent.V_RefreshDigTreasureState);
    }

    public void On_CM_S_DigTreasure(CMD_CM_S_DigTreasure pro)
    {
        //刷新局数
        if (pro.bResult == true)
        {
            HallModel.curPlayCount = 0;
            Util.Instance.DoAction(GameEvent.V_RefreshDigTreasureState);
        }

        //播放奖励
        Util.Instance.DoAction(GameEvent.V_OpenDlgDigTreasure, pro);
    }
    #endregion

    #region 红包

    //收到红包通知
    public void OnGetRedPack(CMD_Game_S_GetRedPack pro)
    {
        if (GameModel.chairId >= 0 && GameModel.chairId < 100)
        {
            Util.Instance.DoAction(GameEvent.V_OpenRedPackButton, pro);
        }
    }

    //开红包
    public void OpenRedPack()
    {
    }

    //开红包结果
    public void OnGetRedPackResult(CMD_Game_S_OpenRedPackResult pro)
    {
        if (pro.lAwardRedEnvelopes == 0)
        {
            return;
        }
        if (pro.dwUserID == HallModel.userId && GameEvent.V_PlayRedPackAnim != null)
        {
            GameEvent.V_PlayRedPackAnim.Invoke(pro.lAwardRedEnvelopes, pro.szNotifyContent);
        }
        //维护用户列表
        if (GameModel.playerInRoom.ContainsKey(pro.dwUserID))
        {
            GameModel.playerInRoom[pro.dwUserID].redPack = pro.lUserRedEnvelopes;
            if (GameModel.playerInRoom[pro.dwUserID].wTableID == GameModel.deskId)
            {
                Util.Instance.DoActionDelay(GameEvent.V_RefreshUserInfo, 2f, true);
            }
        }
    }

    //红包进度信息
    public void OnGetRedPackState(CMD_Game_S_RedPackState pro)
    {
        GameModel.currentRedPackCount = pro.nCurMinDrawCount;
        GameModel.totalRedPackTime = (int)pro.nIntervalTime;
        GameModel.totalRedPackCount = pro.nMinDrawCount;
        GameModel.redPackDescribe = pro.szDescribeString;

        Util.Instance.DoAction(GameEvent.V_RefreshRedPackState);
    }

    //低保信息
    public void OnBaseEnsure(CMD_Game_S_BaseEnsure pro)
    {
        //弹兑换复活
        if (pro.cbType == 1)
        {
            if (GameEvent.V_OpenDlgTip != null)
            {
                GameEvent.V_OpenDlgTip.Invoke(pro.szNotifyContent, "", delegate { GetBaseEnsure(0); }, null);
            }
        }
        else
        {
            //首充判断
            if (HallModel.isFirstCharge == false)
            {
                //弹首充
                Util.Instance.DoActionDelay(GameEvent.V_OpenDlgFirstCharge, 3f, delegate { GameEvent.V_OpenDlgStoreInGame.Invoke(DlgStoreArg.DiamondPage); });
            }
            else
            {
                //弹游戏内充值
                Util.Instance.DoActionDelay(GameEvent.V_OpenDlgStoreInGame, 3f, DlgStoreArg.DiamondPage);
            }
        }
    }


    #endregion

    #region 低保

    //领取低保  0,红包场,1,金币场
    public void GetBaseEnsure(int type)
    {
        CMD_Game_C_BaseEnsureTake pro = new CMD_Game_C_BaseEnsureTake();
        pro.dwUserID = (uint)HallModel.userId;
        pro.nBaseEnsureType = type;
        pro.szPassword = HallModel.dynamicPassword;
        pro.szMachineID = Util.GetMacCode();
        client.SendPro(pro);
    }

    //领取低保
    public void OnGetBaseEnsure(CMD_Game_S_BaseEnsureResult pro)
    {
        if (pro.bSuccessed)
        {
            AudioManager.Instance.PlaySound(GameModel.audioGetAward);
            if (SceneManager.GetActiveScene().name.ToLower().Contains("game"))
            {
                if (GameEvent.V_OpenDlgTip != null)
                {
                    GameEvent.V_OpenDlgTip.Invoke(pro.szNotifyContent, "", GetChair, GetChair);
                }
            }
            HallModel.currentCoinBaseEnsureTimes++;
        }
        else
        {
            if (SceneManager.GetActiveScene().name.ToLower().Contains("game"))
            {
                if (GameEvent.V_OpenDlgTip != null)
                {
                    GameEvent.V_OpenDlgTip.Invoke(pro.szNotifyContent, "", ReturnToHall, ReturnToHall);
                }
            }
            else
            {
                Util.Instance.DoAction(GameEvent.V_OpenShortTip, pro.szNotifyContent);
            }
        }
    }

    #endregion

    /// 返回大厅
    public void ReturnToHall()
    {
        Debug.Log("返回大厅,sceneName=" + AppConfig.gameDic[HallModel.currentGameFlag].sceneName);

        Bs.Room.ExitReq req = new Bs.Room.ExitReq();
        client.SendTransferData2Gate(Util.GetHighUint32(HallModel.currentServerId), Util.GetLowUint32(HallModel.currentServerId), NetManager.AppRoom, (UInt32)(Bs.Room.CMDRoom.IdexitReq), req);

        //BreakConnect();
        HallModel.isReturnFromGame = true;
        SceneManager.LoadScene("hall", LoadSceneMode.Single);

        Destroy(this.gameObject, 0.3f);
    }

    /// 进入游戏
    void LoadGameScene()
    {
        Debug.Log("进入游戏,sceneName=" + AppConfig.gameDic[HallModel.currentGameFlag].sceneName);

        Util.Instance.DoAction(GameEvent.V_CloseConnectTip);
        SceneManager.LoadScene(AppConfig.gameDic[HallModel.currentGameFlag].sceneName, LoadSceneMode.Single);
    }

}
