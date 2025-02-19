using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class LandlordsEvent
{

    public static UnityAction<Bs.Gameddz.S_StatusFree> S_StateFree;
    public static UnityAction<Bs.Gameddz.S_StatusCall> S_StateCall;
    public static UnityAction<Bs.Gameddz.S_StatusAddTimes> S_StateAddTime;
    public static UnityAction<Bs.Gameddz.S_StatusPlay> S_StatePlay;

    public static UnityAction<Bs.Gameddz.S_GameStart> S_GameStart;
    public static UnityAction<Bs.Gameddz.S_RobLand> S_UserCall;
    public static UnityAction<Bs.Gameddz.S_ReOutCard> S_ReSendCard;
    public static UnityAction<Bs.Gameddz.S_BankerInfo> S_BankerStartOutCard;
    public static UnityAction<Bs.Gameddz.S_AddTimes> S_UserAddTime;
    public static UnityAction<Bs.Gameddz.S_OutCard> S_UserOutCard;
    public static UnityAction<Bs.Gameddz.S_OutCardFail> S_OutCardFail;
    public static UnityAction<Bs.Gameddz.S_PassCard> S_PassCard;
    public static UnityAction<Bs.Gameddz.S_GameConclude> S_GameEnd;

    public static UnityAction<Bs.Gameddz.S_TRUSTEE> OnUserTrustee;

    public static UnityAction V_ReStartNewGame;
    public static UnityAction V_ShowContinueBtn;
    public static UnityAction<Landlords3Result, float> V_GameOver;
    public static UnityAction V_CloseDlgResult;
}
