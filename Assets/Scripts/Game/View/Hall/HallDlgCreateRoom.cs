using UnityEngine;
using System.Collections;

public class HallDlgCreateRoom : View
{
    private Transform bg;
    private Transform shade;

    private UIButton btnConfirm;
    private UIButton btnClose;

    private UILabel lblPlayerCount;
    private UIButton btnGameCount_0_Landlords;
    private UIButton btnGameCount_1_Landlords;
    private UIButton btnGameCount_2_Landlords;
    private UIButton btnBaseScore_0_Landlords;
    private UIButton btnBaseScore_1_Landlords;
    private UIButton btnBaseScore_2_Landlords;

    public override void Init()
    {
        bg = transform.Find("bg");
        shade = transform.Find("shade");
        btnConfirm = transform.Find("bg/btnConfirm").GetComponent<UIButton>();
        btnClose = transform.Find("bg/btnClose").GetComponent<UIButton>();

        btnGameCount_0_Landlords = transform.Find("bg/btnGameCount_0").GetComponent<UIButton>();
        btnGameCount_1_Landlords = transform.Find("bg/btnGameCount_1").GetComponent<UIButton>();
        btnGameCount_2_Landlords = transform.Find("bg/btnGameCount_2").GetComponent<UIButton>();
        btnBaseScore_0_Landlords = transform.Find("bg/btnBaseScore_0").GetComponent<UIButton>();
        btnBaseScore_1_Landlords = transform.Find("bg/btnBaseScore_1").GetComponent<UIButton>();
        btnBaseScore_2_Landlords = transform.Find("bg/btnBaseScore_2").GetComponent<UIButton>();

        EventDelegate.Add(btnConfirm.onClick, OnBtnConfirmClick);
        EventDelegate.Add(btnClose.onClick, OnBtnCloseClick);

        //斗地主
        EventDelegate.Add(btnGameCount_0_Landlords.onClick, OnBtnGameCount0Click_Landlords);
        EventDelegate.Add(btnGameCount_1_Landlords.onClick, OnBtnGameCount1Click_Landlords);
        EventDelegate.Add(btnGameCount_2_Landlords.onClick, OnBtnGameCount2Click_Landlords);
        EventDelegate.Add(btnBaseScore_0_Landlords.onClick, OnBtnBaseScore0Click_Landlords);
        EventDelegate.Add(btnBaseScore_1_Landlords.onClick, OnBtnBaseScore1Click_Landlords);
        EventDelegate.Add(btnBaseScore_2_Landlords.onClick, OnBtnBaseScore2Click_Landlords);

        gameObject.SetActive(false);
        bg.GetComponent<TweenScale>().ResetToBeginning();
        bg.GetComponent<TweenAlpha>().ResetToBeginning();
    }

    public override void RegisterAction()
    {
        HallEvent.V_OpenDlgCreateRoom += Open;
    }

    public override void RemoveAction()
    {
        HallEvent.V_OpenDlgCreateRoom -= Open;
    }

    void Open()
    {
        gameObject.SetActive(true);
        bg.GetComponent<TweenScale>().PlayForward();
        bg.GetComponent<TweenAlpha>().PlayForward();
        shade.GetComponent<TweenAlpha>().PlayForward();

        RefreshPanel();
    }

    void Close()
    {
        bg.GetComponent<TweenScale>().PlayReverse();
        bg.GetComponent<TweenAlpha>().PlayReverse();

        DoActionDelay(CloseCor, 0.3f);
    }

    void CloseCor()
    {
        gameObject.SetActive(false);
    }

    void RefreshPanel()
    {
        btnGameCount_0_Landlords.GetComponent<UISprite>().spriteName = GameModel.ruleGameCountIndex == 0 ? "select_open" : "select_close";
        btnGameCount_1_Landlords.GetComponent<UISprite>().spriteName = GameModel.ruleGameCountIndex == 1 ? "select_open" : "select_close";
        btnGameCount_2_Landlords.GetComponent<UISprite>().spriteName = GameModel.ruleGameCountIndex == 2 ? "select_open" : "select_close";

        btnGameCount_0_Landlords.normalSprite = GameModel.ruleGameCountIndex == 0 ? "select_open" : "select_close";
        btnGameCount_1_Landlords.normalSprite = GameModel.ruleGameCountIndex == 1 ? "select_open" : "select_close";
        btnGameCount_2_Landlords.normalSprite = GameModel.ruleGameCountIndex == 2 ? "select_open" : "select_close";

        btnBaseScore_0_Landlords.GetComponent<UISprite>().spriteName = GameModel.ruleBaseScore == 1 ? "select_open" : "select_close";
        btnBaseScore_1_Landlords.GetComponent<UISprite>().spriteName = GameModel.ruleBaseScore == 5 ? "select_open" : "select_close";
        btnBaseScore_2_Landlords.GetComponent<UISprite>().spriteName = GameModel.ruleBaseScore == 10 ? "select_open" : "select_close";
        btnBaseScore_0_Landlords.normalSprite = GameModel.ruleBaseScore == 1 ? "select_open" : "select_close";
        btnBaseScore_1_Landlords.normalSprite = GameModel.ruleBaseScore == 5 ? "select_open" : "select_close";
        btnBaseScore_2_Landlords.normalSprite = GameModel.ruleBaseScore == 10 ? "select_open" : "select_close";
    }


    void OnBtnConfirmClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonNormal);
        HallModel.currentGameFlag = GameFlag.Landlords3;
        GameModel.rulePlayerCount = 3;
        HallModel.opOnLoginGame = OpOnLginGame.CreateRoom;
        GameModel.currentRoomId = 0xFFFFFFFF;
        HallService.Instance.GetRoomServerInfo(GameModel.ServerKind_Private, 0);
        Close();
    }

    void OnBtnCloseClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonClose);
        Close();
    }


    //斗地主 局数
    void OnBtnGameCount0Click_Landlords()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonSwitch);
        GameModel.ruleGameCountIndex = 0;
        RefreshPanel();
    }

    //斗地主 局数
    void OnBtnGameCount1Click_Landlords()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonSwitch);
        GameModel.ruleGameCountIndex = 1;
        RefreshPanel();
    }

    //斗地主 局数
    void OnBtnGameCount2Click_Landlords()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonSwitch);
        GameModel.ruleGameCountIndex = 2;
        RefreshPanel();
    }

    //斗地主 底分
    void OnBtnBaseScore0Click_Landlords()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonSwitch);
        GameModel.ruleBaseScore = 1;
        RefreshPanel();
    }

    void OnBtnBaseScore1Click_Landlords()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonSwitch);
        GameModel.ruleBaseScore = 5;
        RefreshPanel();
    }

    void OnBtnBaseScore2Click_Landlords()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonSwitch);
        GameModel.ruleBaseScore = 10;
        RefreshPanel();
    }


}
