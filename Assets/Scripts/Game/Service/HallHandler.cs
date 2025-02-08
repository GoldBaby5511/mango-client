using UnityEngine;
using System.Collections;
using Bs.Gateway;
using System;
using System.IO;

public class HallHandler : IHandler 
{
    public bool Handler(NetPacket packet)
    {
        //NGUIDebug.Log("HallHandler ： " + packet.header.wMainCmdID + "-" + packet.header.wSubCmdID);
        //packet.GetHeader();

        switch (packet.GetMainCmdID())
        {
            case NetManager.AppGate:
                return handlerGateMessage(packet);
            default:
                Debug.LogError("异常,竟然收到网关以外的消息,appType="+packet.GetMainCmdID()+",cmdId="+packet.GetSubCmdID());
                break;
        }

        return false;


        switch (packet.header.wMainCmdID + "-" + packet.header.wSubCmdID)
        {
            case "0-1":
                //收到心跳包 - 心跳不在此处处理
                HallService.Instance.SendHeartPacket();
                return true;

            case "3-120":
                //修改头像 - 系统头像
                //CMD_Hall_S_FaceInfo pro_3_120 = new CMD_Hall_S_FaceInfo();
                //pro_3_120.UnPack(packet.bytes);
                //HallService.Instance.OnReceiveFaceInfo(pro_3_120);
            case "3-140":
                //收到用户信息
                CMD_Hall_S_QueryUserInfo pro_3_140 = new CMD_Hall_S_QueryUserInfo();
                pro_3_140.UnPack(packet.bytes);
                HallService.Instance.OnGetQueryUserInfo(pro_3_140);
                return true;
            case "3-153":
                //实名认证结果
                CMD_Hall_S_IndividuaResult pro_3_153 = new CMD_Hall_S_IndividuaResult();
                pro_3_153.UnPack(packet.bytes);
                HallService.Instance.OnReceiveRealAuthResult(pro_3_153);
                return true;

            case "3-164":
                //银行信息
                CMD_Hall_S_BankInfo pro_3_164 = new CMD_Hall_S_BankInfo();
                pro_3_164.UnPack(packet.bytes);
                HallService.Instance.OnGetBankInfo(pro_3_164);
                return true;
            case "3-166":
                //银行操作 - 成功
                CMD_Hall_S_BankOpSuccess pro_3_166 = new CMD_Hall_S_BankOpSuccess();
                pro_3_166.UnPack(packet.bytes);
                HallService.Instance.OnBankOpSuccess(pro_3_166);
                return true;
            case "3-167":
                //银行操作 - 失败
                CMD_Hall_S_BankOpFail pro_3_167 = new CMD_Hall_S_BankOpFail();
                pro_3_167.UnPack(packet.bytes);
                HallService.Instance.OnBankOpFail(pro_3_167);
                return true;
            case "3-169":
                //查询赠送用户信息结果
                CMD_Hall_S_QuerySendUserInfoResult pro_3_169 = new CMD_Hall_S_QuerySendUserInfoResult();
                pro_3_169.UnPack(packet.bytes);
                HallService.Instance.OnGetSendUserInfo(pro_3_169);
                return true;
            case "3-170":
                //开通银行结果
                CMD_Hall_S_EnableBankResult pro_3_170 = new CMD_Hall_S_EnableBankResult();
                pro_3_170.UnPack(packet.bytes);
                HallService.Instance.OnGetEnableBankResult(pro_3_170);
                return true;


            case "3-221":
                //签到信息
                CMD_Hall_S_SignInfo pro_3_221 = new CMD_Hall_S_SignInfo();
                pro_3_221.UnPack(packet.bytes);
                HallService.Instance.OnGetSignInfo(pro_3_221);
                return true;
            case "3-223":
                //签到结果
                CMD_Hall_S_SignInResult pro_3_223 = new CMD_Hall_S_SignInResult();
                pro_3_223.UnPack(packet.bytes);
                HallService.Instance.OnGetSignResult(pro_3_223);
                return true;


            case "3-262":
                //低保参数
                CMD_Hall_S_BaseEnsureParamter pro_3_262 = new CMD_Hall_S_BaseEnsureParamter();
                pro_3_262.UnPack(packet.bytes);
                HallService.Instance.OnGetBaseEnsurePara(pro_3_262);
                return true;
            case "3-263":
                //低保结果
                CMD_Hall_S_BaseEnsureResult pro_3_263 = new CMD_Hall_S_BaseEnsureResult();
                pro_3_263.UnPack(packet.bytes);
                HallService.Instance.OnGetBaseEnsure(pro_3_263);
                return true;



            case "3-326":
                //兑换结果
                CMD_Hall_S_ExchangeResult pro_3_326 = new CMD_Hall_S_ExchangeResult();
                pro_3_326.UnPack(packet.bytes);
                HallService.Instance.OnGetExchangeReuslt(pro_3_326);
                return true;

            case "3-500":
                //操作成功
                CMD_Hall_S_OpSuccess pro_3_500 =new CMD_Hall_S_OpSuccess();
                pro_3_500.UnPack(packet.bytes);
                HallService.Instance.OnOpSuccess(pro_3_500);
                return true;
            case "3-501":
                //操作失败
                CMD_Hall_S_OpFail pro_3_501 = new CMD_Hall_S_OpFail();
                pro_3_501.UnPack(packet.bytes);
                HallService.Instance.OnOpFail(pro_3_501);
                return true;

            case "3-710":
                //获取游戏记录
                CMD_Hall_S_GameRecord pro_3_710 = new CMD_Hall_S_GameRecord();
                pro_3_710.UnPack(packet.bytes);
                HallService.Instance.OnGetGameRecord(pro_3_710);
                return true;
            case "3-711":
                //获取游戏记录详情
                CMD_Hall_S_RecordInfo pro_3_711 = new CMD_Hall_S_RecordInfo();
                pro_3_711.UnPack(packet.bytes);
                HallService.Instance.OnGetRecordInfo(pro_3_711);
                return true;
            case "3-712":
                //获取分享结果
                CMD_Hall_S_ShareResult pro_3_712 = new CMD_Hall_S_ShareResult();
                pro_3_712.UnPack(packet.bytes);
                HallService.Instance.OnGetShareResult(pro_3_712);
                return true;
            case "3-714":
                //游戏记录结束标志
                HallService.Instance.OnGameRecordFinish();
                return true;

            case "3-721":
                //排行信息
                CMD_Hall_S_RankInfo pro_3_721 = new CMD_Hall_S_RankInfo();
                pro_3_721.UnPack(packet.bytes);
                HallService.Instance.OnGetRankInfo(pro_3_721);
                return true;


            case "4-201":
                //创建/加入房间时，收到服务器信息
                CMD_Hall_S_CardRoomServerInfo pro_4_201 = new CMD_Hall_S_CardRoomServerInfo();
                pro_4_201.UnPack(packet.bytes);
                HallService.Instance.OnGetRoomServerInfo(pro_4_201);
                return true;
            case "4-202":
                //收到玩家游戏状态
                CMD_Hall_S_GetUserGameState pro_4_202 = new CMD_Hall_S_GetUserGameState();
                pro_4_202.UnPack(packet.bytes);
                HallService.Instance.OnGetUserStateInGame(pro_4_202);
                return true;
            case "6-9":
                //收到大喇叭消息
                CMD_Hall_S_HornMessage pro_6_9 = new CMD_Hall_S_HornMessage();
                pro_6_9.UnPack(packet.bytes);
                HallService.Instance.OnReceiveHornMessage(pro_6_9);
                return true;

            case "8-100":
                //彩金/红包
                CMD_Hall_S_Handsel pro_8_100 = new CMD_Hall_S_Handsel();
                pro_8_100.UnPack(packet.bytes);
                HallService.Instance.OnReceiveHandsel(pro_8_100);
                return true;

            case "8-102":
                //系统消息 - 管理员消息
                CMD_Hall_S_SystemMessage pro_8_102 = new CMD_Hall_S_SystemMessage();
                pro_8_102.UnPack(packet.bytes);
                HallService.Instance.OnReceiveSystemMessage(pro_8_102);
                return true;
            case "100-102":
                //登陆完成
                HallService.Instance.OnLoginFinish();
                return true;
            case "100-200":
                //升级提示

                return true;

            case "101-100":
                //游戏分类列表 - 游戏排序
                CMD_Hall_S_GameKindList pro_101_100 = new CMD_Hall_S_GameKindList();
                pro_101_100.UnPack(packet.bytes);
                HallService.Instance.OnReceiveServerKindList(pro_101_100);
                return true;
            case "101-101":
                //收到服务器列表
                CMD_Hall_S_GameServerList pro_3 = new CMD_Hall_S_GameServerList();
                pro_3.UnPack(packet.bytes);
                HallService.Instance.OnReceiveServerList(pro_3);
                return true;
            case "101-103":
                //收到房卡游戏服务器信息--创建参数
                CMD_Hall_S_CardGameServer pro_101_103 = new CMD_Hall_S_CardGameServer();
                pro_101_103.UnPack(packet.bytes);
                HallService.Instance.OnReceiveCardServerInfo(pro_101_103);
                return true;
            case "101-200":
                //收到服务器列表完成
                HallService.Instance.OnReceiveServerListFinish();
                return true;

            case "1000-1":
                //收到系统消息 - 全局消息
                CMD_Hall_S_SystemInfo pro_1000_1 = new CMD_Hall_S_SystemInfo();
                pro_1000_1.UnPack(packet.bytes);
                HallService.Instance.OnReceiveSystemInfo(pro_1000_1);
                return true;
            case "1000-4":
                //收到充值消息 - 全局消息
                CMD_CM_S_RabbitMQInfo pro_1000_4 = new CMD_CM_S_RabbitMQInfo();
                pro_1000_4.UnPack(packet.bytes);
                HallService.Instance.OnReceiveRabbitMQInfo(pro_1000_4);
                return true;

            case "1001-11":
                //任务信息
                CMD_CM_S_TaskInfo pro_1001_11 = new CMD_CM_S_TaskInfo();
                pro_1001_11.UnPack(packet.bytes);
                HallService.Instance.On_S_TaskInfo(pro_1001_11);
                return true;
            case "1001-12":
                //任务完成
                CMD_CM_S_TaskProgress pro_1001_12 = new CMD_CM_S_TaskProgress();
                pro_1001_12.UnPack(packet.bytes);
                HallService.Instance.On_CM_S_TaskProgress(pro_1001_12);
                return true;
            case "1001-13":
                //任务列表
                CMD_CM_S_TaskParameter pro_1001_13 = new CMD_CM_S_TaskParameter();
                pro_1001_13.UnPack(packet.bytes);
                HallService.Instance.On_S_TaskParameter(pro_1001_13);
                return true;
            case "1001-14":
                //任务结果
                CMD_CM_S_TaskResult pro_1001_14 = new CMD_CM_S_TaskResult();
                pro_1001_14.UnPack(packet.bytes);
                HallService.Instance.On_CM_S_TaskResult(pro_1001_14);
                return true;
        }
        return false;
    }

    private bool handlerGateMessage(NetPacket packet)
    {
        switch (packet.GetSubCmdID())
        {
            case (UInt32)Bs.Gateway.CMDGateway.IdhelloRsp:
                {
                    HelloRsp rsp = packet.Deserialize<HelloRsp>();
                    HallService.Instance.OnHelloRsp();
                    return true;
                }
            case (UInt32)Bs.Gateway.CMDGateway.IdtransferDataReq:
                {
                    Bs.Gateway.TransferDataReq req = packet.Deserialize<Bs.Gateway.TransferDataReq>();
                    Debug.Log("收到转发,MainCmdId=" + req.MainCmdId + ",SubCmdId=" + req.SubCmdId);

                    switch (req.MainCmdId)
                    {
                        case NetManager.AppLobby:
                            return handlerLoginMessage(req);
                        case NetManager.AppList:
                            return handlerListMessage(req);
                        case NetManager.AppRoom:
                            return handlerRoomMessage(req);
                        default:
                            Debug.LogError("异常,没有处理的网关消息,MainCmdId=" + req.MainCmdId + ",SubCmdId=" + req.SubCmdId);
                            break;
                    }
                }
                return true;
            default:
                Debug.LogError("异常,没有处理的消息,mainCmdId=" + packet.GetMainCmdID() + ",cmdId=" + packet.GetSubCmdID());
                break;
        }
        return false;
    }

    private bool handlerLoginMessage(Bs.Gateway.TransferDataReq req)
    {
        switch (req.SubCmdId)
        {
            case (uint)Bs.Lobby.CMDLobby.IdloginRsp:
                {
                    HallService.Instance.OnLoginRsp(req);
                    return true;
                }
            default:
                Debug.LogError("异常,login没有处理的消息,data_cmdid=" + req.SubCmdId);
                break;
        }
        return false;
    }

    private bool handlerListMessage(Bs.Gateway.TransferDataReq req)
    {
        switch (req.SubCmdId)
        {
            case (uint)Bs.List.CMDList.IdroomListRsp:
                {
                    HallService.Instance.OnRoomListRsp(req);
                    return true;
                }
            default:
                Debug.LogError("异常,list,没有处理的消息,SubCmdId=" + req.SubCmdId);
                break;
        }
        return false;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="req"></param>
    /// <returns>true 本层处理,false 其他Handler处理</returns>
    private bool handlerRoomMessage(Bs.Gateway.TransferDataReq req)
    {
        switch(req.SubCmdId)
        {
            case (uint)Bs.Room.CMDRoom.IdenterRsp:
                {
                    return GameService.Instance.EnterRoomRsp(req);
                }
            case (uint)Bs.Room.CMDRoom.IduserStatus:
                {
                    return GameService.Instance.OnReceiveUserState(req);
                }
            case (uint)Bs.Room.CMDRoom.IdrequestFailure:
                {
                    return GameService.Instance.OnRequestFailure(req);
                }
            case (uint)Bs.Room.CMDRoom.IdconfigServer:
                {
                    return GameService.Instance.OnReceiveGameServerConfig(req);
                }
            case (uint)Bs.Room.CMDRoom.IduserEnter:
                {
                    return GameService.Instance.OnUserCome(req);
                }
            case (uint)Bs.Room.CMDRoom.IdtableInfo:
                {
                    return GameService.Instance.OnReceiveTableInfo(req);
                }
            case (uint)Bs.Room.CMDRoom.IdtableStatus:
                {
                    return GameService.Instance.OnReceiveTableState(req);
                }
            case (uint)Bs.Room.CMDRoom.IdlogonFinish:
                {
                    return GameService.Instance.OnLoginFinish(req);
                }
            default:
                Debug.LogError("异常,room,没有处理的消息,SubCmdId=" + req.SubCmdId);
                break;
        }
        return false;
    }
}
