using UnityEngine;
using System;

public class LandlordsHandler : IHandler 
{
    int gameStatus = 0;
    public bool Handler(NetPacket packet)
    {
        if(!(packet.GetMainCmdID() == NetManager.AppGate && packet.GetSubCmdID() == (UInt32)Bs.Gateway.CMDGateway.IdtransferDataReq))
        {
            Debug.LogError("游戏状态收到异常消息,MainCmdID=" + packet.GetMainCmdID() + ",SubCmdID=" + packet.GetSubCmdID());
            return false;
        }
        //const uint MDM_GF_GAME = 200;
        Bs.Gateway.TransferDataReq req = packet.Deserialize<Bs.Gateway.TransferDataReq>();
        //Debug.Log("收到游戏状态,MainCmdId=" + req.MainCmdId + ",SubCmdId=" + req.SubCmdId);
        //Debug.Log("收到游戏状态,MainCmdID=" + packet.GetMainCmdID() + ",SubCmdID=" + packet.GetSubCmdID() + ",MainCmdId=" + req.MainCmdId + ",SubCmdId=" + req.SubCmdId);

        switch (req.MainCmdId)
        {
            case NetManager.AppRoom:
                {
                    switch(req.SubCmdId)
                    {
                        case (uint)Bs.Room.CMDRoom.IdgameStatus:
                            {
                                Bs.Room.GameStatus rsp = NetPacket.Deserialize<Bs.Room.GameStatus>(req.Data.ToByteArray());
                                gameStatus = (int)rsp.GameStatus_;
                                return true;
                            }
                        case (uint)Bs.Room.CMDRoom.IdgameScene:
                            {
                                //游戏状态信息
                                switch (gameStatus)
                                {
                                    case 0:     //等待开始
                                        Bs.Gameddz.S_StatusFree rspFree = NetPacket.Deserialize<Bs.Gameddz.S_StatusFree>(req.Data.ToByteArray());
                                        LandlordsService.Instance.S2C_StateFree(rspFree);
                                        break;
                                    case 100:   //叫分状态
                                        Bs.Gameddz.S_StatusCall rspCall = NetPacket.Deserialize<Bs.Gameddz.S_StatusCall>(req.Data.ToByteArray());
                                        LandlordsService.Instance.S2C_StatusCall(rspCall);
                                        break;
                                    case 101:   //加倍状态
                                        Bs.Gameddz.S_StatusAddTimes rspAddTimes = NetPacket.Deserialize<Bs.Gameddz.S_StatusAddTimes>(req.Data.ToByteArray());
                                        LandlordsService.Instance.S2C_StatusAddTime(rspAddTimes);
                                        break;
                                    case 102:   //正在游戏
                                        Bs.Gameddz.S_StatusPlay rspPlay = NetPacket.Deserialize<Bs.Gameddz.S_StatusPlay>(req.Data.ToByteArray());
                                        LandlordsService.Instance.S2C_StatusPlay(rspPlay);
                                        break;
                                }
                                return true;
                            }
                    }

                    return false;
                }
            case NetManager.MDM_GF_GAME:
                {
                    switch (req.SubCmdId)
                    {
                        case 100:
                            //游戏开始
                            Bs.Gameddz.S_GameStart rspGameStart = NetPacket.Deserialize<Bs.Gameddz.S_GameStart>(req.Data.ToByteArray());
                            LandlordsService.Instance.S2C_GameStart(rspGameStart);
                            return true;
                        case 101:
                            //用户叫分
                            Bs.Gameddz.S_RobLand rspRobLand = NetPacket.Deserialize<Bs.Gameddz.S_RobLand>(req.Data.ToByteArray());
                            LandlordsService.Instance.S2C_CallScore(rspRobLand);
                            return true;
                        case 102:
                            //庄家信息
                            Bs.Gameddz.S_BankerInfo rspBankerInfo = NetPacket.Deserialize<Bs.Gameddz.S_BankerInfo>(req.Data.ToByteArray());
                            LandlordsService.Instance.S2C_BankerInfo(rspBankerInfo);
                            return true;
                        case 103:
                            //用户出牌
                            Bs.Gameddz.S_OutCard rspOutCard = NetPacket.Deserialize<Bs.Gameddz.S_OutCard>(req.Data.ToByteArray());
                            LandlordsService.Instance.S2C_OutCard(rspOutCard);
                            return true;
                        case 104:
                            //用户放弃
                            Bs.Gameddz.S_PassCard rspPassCard = NetPacket.Deserialize<Bs.Gameddz.S_PassCard>(req.Data.ToByteArray());
                            LandlordsService.Instance.S2C_PassCard(rspPassCard);
                            return true;
                        case 105:
                            //游戏结束
                            Bs.Gameddz.S_GameConclude rspGameConclude = NetPacket.Deserialize<Bs.Gameddz.S_GameConclude>(req.Data.ToByteArray());
                            LandlordsService.Instance.S2C_GameEnd(rspGameConclude);
                            return true;
                        //case 106:
                        //    //设置基数
                        //    CMD_HL_S_OutCardError pro_200_106 = new CMD_HL_S_OutCardError();
                        //    pro_200_106.UnPack(packet.bytes);
                        //    LandlordsService.Instance.OnOutCardError(pro_200_106);
                        //    return true;
                        case 107:
                            //作弊扑克
                            Bs.Gameddz.S_CheatCard rspCheatCard = NetPacket.Deserialize<Bs.Gameddz.S_CheatCard>(req.Data.ToByteArray());
                            LandlordsService.Instance.S2C_CheatCard(rspCheatCard);
                            return true;
                        case 108:
                            //托管
                            Bs.Gameddz.S_TRUSTEE rspTrustee = NetPacket.Deserialize<Bs.Gameddz.S_TRUSTEE>(req.Data.ToByteArray());
                            LandlordsService.Instance.S2C_Trustee(rspTrustee);
                            return true;
                        case 109:
                            //用户加倍
                            Bs.Gameddz.S_AddTimes rspAddTimes = NetPacket.Deserialize<Bs.Gameddz.S_AddTimes>(req.Data.ToByteArray());
                            LandlordsService.Instance.S2C_AddTimes(rspAddTimes);
                            return true;
                        case 110:
                            //出牌错误
                            Bs.Gameddz.S_OutCardFail rspOutCardFail = NetPacket.Deserialize<Bs.Gameddz.S_OutCardFail>(req.Data.ToByteArray());
                            LandlordsService.Instance.S2C_OutCardFail(rspOutCardFail);
                            return true;
                        case 111:
                            //重新发牌
                            Bs.Gameddz.S_ReOutCard rspReOutCard = NetPacket.Deserialize<Bs.Gameddz.S_ReOutCard>(req.Data.ToByteArray());
                            LandlordsService.Instance.S2C_ReSendCard(rspReOutCard);
                            return true;
                    }
                    return false;
                }
        }
        return false;

        //switch (req.MainCmdId + "-" + req.SubCmdId)
        //{
        //    case "100-100":
        //        //收到游戏状态
        //        CMD_Game_S_GameStatus pro_100_100 = new CMD_Game_S_GameStatus();
        //        pro_100_100.UnPack(packet.bytes);
        //        gameStatus = pro_100_100.cbGameStatus;
        //        return true;
        //    case "100-101":
        //        //游戏状态信息
        //        switch (gameStatus)
        //        {
        //            case 0:     //等待开始
        //                CMD_Landlords_S_StatusFree pro_101_0 = new CMD_Landlords_S_StatusFree();
        //                pro_101_0.UnPack(packet.bytes);
        //                LandlordsService.Instance.S2C_StateFree(pro_101_0);
        //                break;
        //            case 100:   //叫分状态
        //                CMD_Landlords_S_StatusCall pro_101_100 = new CMD_Landlords_S_StatusCall();
        //                pro_101_100.UnPack(packet.bytes);
        //                LandlordsService.Instance.S2C_StatusCall(pro_101_100);
        //                break;
        //            case 101:   //加倍状态
        //                CMD_Landlords_S_StatusAddTime pro_101_101 = new CMD_Landlords_S_StatusAddTime();
        //                pro_101_101.UnPack(packet.bytes);
        //                LandlordsService.Instance.S2C_StatusAddTime(pro_101_101);
        //                break;                        
        //            case 102:   //正在游戏
        //                CMD_Landlords_S_StatusPlay pro_101_102 = new CMD_Landlords_S_StatusPlay();
        //                pro_101_102.UnPack(packet.bytes);
        //                LandlordsService.Instance.S2C_StatusPlay(pro_101_102);
        //                break;
        //        }
        //        return true;

        //    case "200-100":
        //        //游戏开始
        //        CMD_Landlords_S_GameStart pro_200_100 = new CMD_Landlords_S_GameStart();
        //        pro_200_100.UnPack(packet.bytes);
        //        LandlordsService.Instance.S2C_GameStart(pro_200_100);
        //        return true;
        //    case "200-101":
        //        //用户叫分
        //        CMD_Landlords_S_CallScore pro_200_101 = new CMD_Landlords_S_CallScore();
        //        pro_200_101.UnPack(packet.bytes);
        //        LandlordsService.Instance.S2C_CallScore(pro_200_101);
        //        return true;
        //    case "200-102":
        //        //庄家信息
        //        CMD_Landlords_S_BankerInfo pro_200_102 = new CMD_Landlords_S_BankerInfo();
        //        pro_200_102.UnPack(packet.bytes);
        //        LandlordsService.Instance.S2C_BankerInfo(pro_200_102);
        //        return true;
        //    case "200-103":
        //        //用户出牌
        //        CMD_Landlords_S_OutCard pro_200_103 = new CMD_Landlords_S_OutCard();
        //        pro_200_103.UnPack(packet.bytes);
        //        LandlordsService.Instance.S2C_OutCard(pro_200_103);
        //        return true;
        //    case "200-104":
        //        //用户放弃
        //        CMD_Landlords_S_PassCard pro_200_104 = new CMD_Landlords_S_PassCard();
        //        pro_200_104.UnPack(packet.bytes);
        //        LandlordsService.Instance.S2C_PassCard(pro_200_104);
        //        return true;
        //    case "200-105":
        //        //游戏结束
        //        CMD_Landlords_S_GameEnd pro_200_105 = new CMD_Landlords_S_GameEnd();
        //        pro_200_105.UnPack(packet.bytes);
        //        LandlordsService.Instance.S2C_GameEnd(pro_200_105);
        //        return true;
        //    //case "200-106":
        //    //    //设置基数
        //    //    CMD_HL_S_OutCardError pro_200_106 = new CMD_HL_S_OutCardError();
        //    //    pro_200_106.UnPack(packet.bytes);
        //    //    LandlordsService.Instance.OnOutCardError(pro_200_106);
        //    //    return true;
        //    case "200-107":
        //        //作弊扑克
        //        CMD_Landlords_S_CheatCard pro_200_107 = new CMD_Landlords_S_CheatCard();
        //        pro_200_107.UnPack(packet.bytes);
        //        LandlordsService.Instance.S2C_CheatCard(pro_200_107);
        //        return true;
        //    case "200-108":
        //        //托管
        //        CMD_Landlords_S_Trustee pro_200_108 = new CMD_Landlords_S_Trustee();
        //        pro_200_108.UnPack(packet.bytes);
        //        LandlordsService.Instance.S2C_Trustee(pro_200_108);
        //        return true;
        //    case "200-109":
        //        //用户加倍
        //        CMD_Landlords_S_AddTimes pro_200_109 = new CMD_Landlords_S_AddTimes();
        //        pro_200_109.UnPack(packet.bytes);
        //        LandlordsService.Instance.S2C_AddTimes(pro_200_109);
        //        return true;
        //    case "200-110":
        //        //出牌错误
        //        CMD_Landlords_S_OutCardFail pro_200_110 = new CMD_Landlords_S_OutCardFail();
        //        pro_200_110.UnPack(packet.bytes);
        //        LandlordsService.Instance.S2C_OutCardFail(pro_200_110);
        //        return true;
        //    case "200-111":
        //        //出牌错误
        //        CMD_Landlords_S_ReSendCard pro_200_111 = new CMD_Landlords_S_ReSendCard();
        //        pro_200_111.UnPack(packet.bytes);
        //        LandlordsService.Instance.S2C_ReSendCard(pro_200_111);
        //        return true;
                
        //}
        //return false;
    }
}
