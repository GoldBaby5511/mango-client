using UnityEngine;
using System.Collections;

public class SceneHall : MonoBehaviour 
{
    void Awake()
    {
        AudioManager.Instance.PlayMusic(GameModel.musicHall);
        HallModel.isSwitchAccount = false;
        HallModel.defaultPhoto = Resources.Load<Texture>("texture/默认头像");



        if (HallModel.isReturnFromGame && HallService.Instance.isConnect)
        {
            Util.Instance.DoAction(HallEvent.V_ClosePnlLogin);
            Util.Instance.DoAction(HallEvent.V_OpenPnlMain);

            HallModel.userCoinInGame = 0;
            Util.Instance.DoAction(GameEvent.V_RefreshUserInfo, true);
            Invoke("RefreshUserCoin", 0.5f);
        }
        else
        {
            Util.Instance.DoAction(HallEvent.V_OpenPnlLogin);
            Util.Instance.DoAction(HallEvent.V_ClosePnlMain);

            if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            {
                if (HallModel.userOpenId != "")
                {
                    HallService.Instance.InitAccountInfo(LoginType.OtherSDK, "", "", HallModel.userOpenId, "", "", "", "", "");
                    HallService.Instance.Connect(ConnectType.Normal);
                }
                else if(HallModel.userAccount != "" && HallModel.userPassword != "")
                {
                    HallService.Instance.InitAccountInfo(LoginType.Account, HallModel.userAccount, HallModel.userPassword, "", "", "", "", "", "");
                    HallService.Instance.Connect(ConnectType.Normal);
                }
                else if (HallModel.isStartByURL)
                {
                    PluginManager.Instance.WxLogin();
                }
            }
        }
    }

    void RefreshUserCoin()
    {
        HallService.Instance.QueryBankInfo();
    }

    //URL启动app
    void OnStartByURL(string msg)
    {
        //初始化，获取房间ID
        if (Application.platform == RuntimePlatform.Android)
        {
            GameModel.currentRoomId = uint.Parse(msg);
        }
        else if(Application.platform == RuntimePlatform.IPhonePlayer)
        {
            if (!msg.Contains("xianjvmj"))
            {
                return;
            }
            try
            {
                GameModel.currentRoomId = uint.Parse(msg.Split('=')[1]);
            }
            catch
            {
                return;
            }
        }
        HallModel.isStartByURL = true;
        HallModel.opOnLoginGame = OpOnLginGame.JoinRoom;
        //进入游戏
        if (HallService.Instance.isConnect)
        {
            //HallModel.isStartByURL = false;
            HallService.Instance.GetRoomServerInfo(GameModel.ServerKind_Private, 0);
        }
        else
        {
            if (HallModel.userOpenId != "")
            {
                HallService.Instance.InitAccountInfo(LoginType.OtherSDK, "", "", HallModel.userOpenId, "", "", "", "", "");
                HallService.Instance.Connect(ConnectType.Normal);
            }
            else
            {
                PluginManager.Instance.WxLogin();
            }
        }
    }

}
