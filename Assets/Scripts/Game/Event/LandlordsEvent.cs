using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class LandlordsEvent
{

    public static UnityAction<CMD_Landlords_S_StatusFree> S_StateFree;
    public static UnityAction<CMD_Landlords_S_StatusCall> S_StateCall;
    public static UnityAction<CMD_Landlords_S_StatusAddTime> S_StateAddTime;
    public static UnityAction<CMD_Landlords_S_StatusPlay> S_StatePlay;

    public static UnityAction<CMD_Landlords_S_GameStart> S_GameStart;
    public static UnityAction<CMD_Landlords_S_CallScore> S_UserCall;
    public static UnityAction<CMD_Landlords_S_ReSendCard> S_ReSendCard;
    public static UnityAction<CMD_Landlords_S_BankerInfo> S_BankerStartOutCard;
    public static UnityAction<CMD_Landlords_S_AddTimes> S_UserAddTime;
    public static UnityAction<CMD_Landlords_S_OutCard> S_UserOutCard;
    public static UnityAction<CMD_Landlords_S_OutCardFail> S_OutCardFail;
    public static UnityAction<CMD_Landlords_S_PassCard> S_GiveUpOutCard;
    public static UnityAction<CMD_Landlords_S_GameEnd> S_GameEnd;

    public static UnityAction<CMD_Landlords_S_Trustee> OnUserTrustee;

    public static UnityAction V_ReStartNewGame;
    public static UnityAction V_ShowContinueBtn;
    public static UnityAction<Landlords3Result, float> V_GameOver;
    public static UnityAction V_CloseDlgResult;
}
