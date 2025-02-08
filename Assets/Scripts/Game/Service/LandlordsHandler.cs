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
        Debug.Log("收到游戏状态,MainCmdID=" + packet.GetMainCmdID() + ",SubCmdID=" + packet.GetSubCmdID());
        Bs.Gateway.TransferDataReq req = packet.Deserialize<Bs.Gateway.TransferDataReq>();
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
                    }

                    return false;
                }
        }
        return false;

        switch (req.MainCmdId + "-" + req.SubCmdId)
        {
            case "100-100":
                //收到游戏状态
                Debug.Log("收到游戏状态");
                return true;
                CMD_Game_S_GameStatus pro_100_100 = new CMD_Game_S_GameStatus();
                pro_100_100.UnPack(packet.bytes);
                gameStatus = pro_100_100.cbGameStatus;
                return true;
            case "100-101":
                //游戏状态信息
                switch (gameStatus)
                {
                    case 0:     //等待开始
                        CMD_Landlords_S_StatusFree pro_101_0 = new CMD_Landlords_S_StatusFree();
                        pro_101_0.UnPack(packet.bytes);
                        LandlordsService.Instance.S2C_StateFree(pro_101_0);
                        break;
                    case 100:   //叫分状态
                        CMD_Landlords_S_StatusCall pro_101_100 = new CMD_Landlords_S_StatusCall();
                        pro_101_100.UnPack(packet.bytes);
                        LandlordsService.Instance.S2C_StatusCall(pro_101_100);
                        break;
                    case 101:   //加倍状态
                        CMD_Landlords_S_StatusAddTime pro_101_101 = new CMD_Landlords_S_StatusAddTime();
                        pro_101_101.UnPack(packet.bytes);
                        LandlordsService.Instance.S2C_StatusAddTime(pro_101_101);
                        break;                        
                    case 102:   //正在游戏
                        CMD_Landlords_S_StatusPlay pro_101_102 = new CMD_Landlords_S_StatusPlay();
                        pro_101_102.UnPack(packet.bytes);
                        LandlordsService.Instance.S2C_StatusPlay(pro_101_102);
                        break;
                }
                return true;

            case "200-100":
                //游戏开始
                CMD_Landlords_S_GameStart pro_200_100 = new CMD_Landlords_S_GameStart();
                pro_200_100.UnPack(packet.bytes);
                LandlordsService.Instance.S2C_GameStart(pro_200_100);
                return true;
            case "200-101":
                //用户叫分
                CMD_Landlords_S_CallScore pro_200_101 = new CMD_Landlords_S_CallScore();
                pro_200_101.UnPack(packet.bytes);
                LandlordsService.Instance.S2C_CallScore(pro_200_101);
                return true;
            case "200-102":
                //庄家信息
                CMD_Landlords_S_BankerInfo pro_200_102 = new CMD_Landlords_S_BankerInfo();
                pro_200_102.UnPack(packet.bytes);
                LandlordsService.Instance.S2C_BankerInfo(pro_200_102);
                return true;
            case "200-103":
                //用户出牌
                CMD_Landlords_S_OutCard pro_200_103 = new CMD_Landlords_S_OutCard();
                pro_200_103.UnPack(packet.bytes);
                LandlordsService.Instance.S2C_OutCard(pro_200_103);
                return true;
            case "200-104":
                //用户放弃
                CMD_Landlords_S_PassCard pro_200_104 = new CMD_Landlords_S_PassCard();
                pro_200_104.UnPack(packet.bytes);
                LandlordsService.Instance.S2C_PassCard(pro_200_104);
                return true;
            case "200-105":
                //游戏结束
                CMD_Landlords_S_GameEnd pro_200_105 = new CMD_Landlords_S_GameEnd();
                pro_200_105.UnPack(packet.bytes);
                LandlordsService.Instance.S2C_GameEnd(pro_200_105);
                return true;
            //case "200-106":
            //    //设置基数
            //    CMD_HL_S_OutCardError pro_200_106 = new CMD_HL_S_OutCardError();
            //    pro_200_106.UnPack(packet.bytes);
            //    LandlordsService.Instance.OnOutCardError(pro_200_106);
            //    return true;
            case "200-107":
                //作弊扑克
                CMD_Landlords_S_CheatCard pro_200_107 = new CMD_Landlords_S_CheatCard();
                pro_200_107.UnPack(packet.bytes);
                LandlordsService.Instance.S2C_CheatCard(pro_200_107);
                return true;
            case "200-108":
                //托管
                CMD_Landlords_S_Trustee pro_200_108 = new CMD_Landlords_S_Trustee();
                pro_200_108.UnPack(packet.bytes);
                LandlordsService.Instance.S2C_Trustee(pro_200_108);
                return true;
            case "200-109":
                //用户加倍
                CMD_Landlords_S_AddTimes pro_200_109 = new CMD_Landlords_S_AddTimes();
                pro_200_109.UnPack(packet.bytes);
                LandlordsService.Instance.S2C_AddTimes(pro_200_109);
                return true;
            case "200-110":
                //出牌错误
                CMD_Landlords_S_OutCardFail pro_200_110 = new CMD_Landlords_S_OutCardFail();
                pro_200_110.UnPack(packet.bytes);
                LandlordsService.Instance.S2C_OutCardFail(pro_200_110);
                return true;
            case "200-111":
                //出牌错误
                CMD_Landlords_S_ReSendCard pro_200_111 = new CMD_Landlords_S_ReSendCard();
                pro_200_111.UnPack(packet.bytes);
                LandlordsService.Instance.S2C_ReSendCard(pro_200_111);
                return true;
                
        }
        return false;
    }
}
