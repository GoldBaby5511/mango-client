using UnityEngine;
using System.Collections;
using System;

public class GameHandler : IHandler
{
    public bool Handler(NetPacket packet)
    {
        //Debug.Log("收到数据包 ： " + packet.header.wMainCmdID + "-" + packet.header.wSubCmdID);
        
        switch (packet.header.wMainCmdID + "-" + packet.header.wSubCmdID)
        {
            case "0-1":
                return true;
            case "0-3":
                //网络测速
                CMD_Game_CS_NetSpeed pro_0_3 = new CMD_Game_CS_NetSpeed();
                pro_0_3.UnPack(packet.bytes);
                GameService.Instance.OnGetTestSpeedPack(pro_0_3);
                return true;
            case "1-3":
                //连接服务器成功
                GameService.Instance.OnConnectSuccess();
                return true;
            case "1-100":
                //登录成功 -- 弃用       
                return true;
            case "1-101":
                //登陆失败
                CMD_Game_S_LoginGameFail pro_1 = new CMD_Game_S_LoginGameFail();
                pro_1.UnPack(packet.bytes);
                GameService.Instance.OnLoginFail(pro_1);
                return true;
            case "1-102":
                //登陆完成
                GameService.Instance.OnLoginFinish();
                return true;
            case "1-200":
                //更新提示
                return true;



            case "2-101":
                //收到房间配置信息
                CMD_Game_S_GameServerConfig pro_2_101 = new CMD_Game_S_GameServerConfig();
                pro_2_101.UnPack(packet.bytes);
                GameService.Instance.OnReceiveGameServerConfig(pro_2_101);
                return true;
            case "2-103":
                //配置完成
                return true;



            case "3-100":
                //用户进入
                CMD_Game_S_UserCome pro_3_100 = new CMD_Game_S_UserCome();
                pro_3_100.UnPack(packet.bytes);
                GameService.Instance.OnUserCome(pro_3_100);
                return true;
            case "3-101":
                //用户积分
                CMD_Game_S_UserScore pro_3_101 = new CMD_Game_S_UserScore();
                pro_3_101.UnPack(packet.bytes);
                GameService.Instance.OnReceiveUserScore(pro_3_101);
                return true;
            case "3-102":
                //用户状态
                CMD_Game_S_UserState pro_3_102 = new CMD_Game_S_UserState();
                pro_3_102.UnPack(packet.bytes);
                GameService.Instance.OnReceiveUserState(pro_3_102);
                return true;
            case "3-103":
                //请求失败
                CMD_Game_S_RequestFailure pro_3_103 = new CMD_Game_S_RequestFailure();
                pro_3_103.UnPack(packet.bytes);
                GameService.Instance.OnRequestFailure(pro_3_103);
                return true;
            case "3-104":
                //用户游戏数据

                return true;
            case "3-201":
                //用户聊天

                return true;
            case "3-202":
                //用户表情

                return true;
            case "3-203":
                //用户私聊

                return true;
            case "3-204":
                //私聊表情

                return true;
            case "3-301":
                //道具成功

                return true;
            case "3-302":
                //道具失败

                return true;
            case "3-303":
                //礼物消息

                return true;
            case "3-304":
                //道具效应

                return true;
            case "3-305":
                //喇叭消息

                return true;
            case "3-5":
                //邀请玩家

                return true;
            case "3-12":
                //等待分配
                GameService.Instance.OnReceiveWaitDistribute();
                return true;

            case "3-415":
                //开红包结果
                CMD_Game_S_OpenRedPackResult pro_3_415 = new CMD_Game_S_OpenRedPackResult();
                pro_3_415.UnPack(packet.bytes);
                GameService.Instance.OnGetRedPackResult(pro_3_415);
                return true;


            case "100-10":
                //收到聊天消息-文字
                CMD_Game_S_ChatMessage pro_100_10 = new CMD_Game_S_ChatMessage();
                pro_100_10.UnPack(packet.bytes);
                GameService.Instance.OnReceiveChatMessage(pro_100_10);
                return true;
            case "100-11":
                //收到聊天消息-语音
                CMD_Game_S_AudioMessage pro_100_11 = new CMD_Game_S_AudioMessage();
                pro_100_11.UnPack(packet.bytes);
                GameService.Instance.OnReceiveAudioMessage(pro_100_11);
                return true;

            case "100-103":
                //红包进度信息
                CMD_Game_S_RedPackState pro_100_103 = new CMD_Game_S_RedPackState();
                pro_100_103.UnPack(packet.bytes);
                GameService.Instance.OnGetRedPackState(pro_100_103);
                return true;

            case "100-104":
                //低保信息
                CMD_Game_S_BaseEnsure pro_100_104 = new CMD_Game_S_BaseEnsure();
                pro_100_104.UnPack(packet.bytes);
                GameService.Instance.OnBaseEnsure(pro_100_104);
                return true; 

            case "100-200":
                //收到系统消息
                CMD_Hall_S_SystemInfo pro_100_200 = new CMD_Hall_S_SystemInfo();
                pro_100_200.UnPack(packet.bytes);
                GameService.Instance.OnReceiveSystemInfo(pro_100_200);
                return true;

            case "4-100":
                //桌子信息
                CMD_Game_S_TableInfo pro_4_100 = new CMD_Game_S_TableInfo();
                pro_4_100.UnPack(packet.bytes);
                GameService.Instance.OnReceiveTableInfo(pro_4_100);
                return true;
            case "4-101":
                //桌子状态
                CMD_Game_S_TableState pro_4_101 = new CMD_Game_S_TableState();
                pro_4_101.UnPack(packet.bytes);
                GameService.Instance.OnReceiveTableState(pro_4_101);
                return true;



            case "11-262":
                //低保参数
                //CMD_Game_S_BaseEnsureParamter pro_11_262 = new CMD_Game_S_BaseEnsureParamter();
                //pro_11_262.UnPack(packet.bytes);
                //GameService.Instance.OnGetBaseEnsurePara(pro_11_262);
                return true;
            case "11-263":
                //低保结果
                CMD_Game_S_BaseEnsureResult pro_11_263 = new CMD_Game_S_BaseEnsureResult();
                pro_11_263.UnPack(packet.bytes);
                GameService.Instance.OnGetBaseEnsure(pro_11_263);
                return true;

            case "14-100":
                //收到创建房间结果
                CMD_Game_S_CreateRoomResult pro_14_403 = new CMD_Game_S_CreateRoomResult();
                pro_14_403.UnPack(packet.bytes);
                GameService.Instance.OnGetCreateRoomResult(pro_14_403);
                return true;
            case "14-101":
                //收到房间信息
                CMD_Game_S_RoomInfo pro_14_101 = new CMD_Game_S_RoomInfo();
                pro_14_101.UnPack(packet.bytes);
                GameService.Instance.OnReceiveRoomInfo(pro_14_101);
                return true;
            case "14-102":
                //房卡游戏结束，总结算消息
                CMD_Game_S_TotalScore pro_14_102 = new CMD_Game_S_TotalScore();
                pro_14_102.UnPack(packet.bytes);
                GameService.Instance.OnGetTotalScore(pro_14_102);
                return true;
            case "14-103":
                //收到解散房间信息
                CMD_Game_S_DisRoomInfo pro_14_406 = new CMD_Game_S_DisRoomInfo();
                pro_14_406.UnPack(packet.bytes);
                GameService.Instance.OnReceiveDisRoomInfo(pro_14_406);
                return true;
            case "14-104":
                //解散房间结果
                CMD_Game_S_DisRoomResult pro_14_404 = new CMD_Game_S_DisRoomResult();
                pro_14_404.UnPack(packet.bytes);
                GameService.Instance.OnReceiveDisRoomResult(pro_14_404);
                return true;
            case "14-105":
                //收到房卡数量信息
                CMD_Game_S_CardCount pro_14_105 = new CMD_Game_S_CardCount();
                pro_14_105.UnPack(packet.bytes);
                GameService.Instance.OnReceiveCardCount(pro_14_105);
                return true;

            case "200-202":
                //红包通知
                CMD_Game_S_GetRedPack pro_200_202 = new CMD_Game_S_GetRedPack();
                pro_200_202.UnPack(packet.bytes);
                GameService.Instance.OnGetRedPack(pro_200_202);
                return true;
            

            case "1000-1":
                //收到系统消息
                CMD_Hall_S_SystemInfo pro_1000_1 = new CMD_Hall_S_SystemInfo();
                pro_1000_1.UnPack(packet.bytes);
                GameService.Instance.OnReceiveSystemInfo(pro_1000_1);
                return true;

            case "1001-12":
                //任务进度
                CMD_CM_S_TaskProgress pro_1001_12 = new CMD_CM_S_TaskProgress();
                pro_1001_12.UnPack(packet.bytes);
                GameService.Instance.On_CM_S_TaskProgress(pro_1001_12);
                return true;

            case "1002-2":
                //任务进度
                CMD_CM_S_LoadDigInfo pro_1002_2 = new CMD_CM_S_LoadDigInfo();
                pro_1002_2.UnPack(packet.bytes);
                GameService.Instance.On_CM_S_LoadDigInfo(pro_1002_2);
                return true;
            case "1002-4":
                //任务进度
                CMD_CM_S_OperateDig pro_1002_4 = new CMD_CM_S_OperateDig();
                pro_1002_4.UnPack(packet.bytes);
                GameService.Instance.On_CM_S_OperateDig(pro_1002_4);
                return true;
            case "1002-6":
                //任务进度
                CMD_CM_S_DigTreasure pro_1002_6 = new CMD_CM_S_DigTreasure();
                pro_1002_6.UnPack(packet.bytes);
                GameService.Instance.On_CM_S_DigTreasure(pro_1002_6);
                return true;

        }
        return false;
    }
}
