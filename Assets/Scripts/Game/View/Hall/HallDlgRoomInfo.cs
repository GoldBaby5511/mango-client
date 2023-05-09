using UnityEngine;
using System.Collections;
using System;

public class HallDlgRoomInfo : View
{
    private Transform bg;
    private Transform shade;

    private UIButton btnConfirm;
    private UIButton btnClose;
    private UILabel lblGameName;
    private UILabel lblHostName;
    private UILabel lblRoomId;
    private UILabel lblPlayerCount;
    private UILabel lblGameCount;
    private UILabel lblRule;

    private UInt16 serverId = 0;

    public override void Init()
    {
        bg = transform.Find("bg");
        shade = transform.Find("shade");
        btnConfirm = transform.Find("bg/btnConfirm").GetComponent<UIButton>();
        btnClose = transform.Find("bg/btnClose").GetComponent<UIButton>();
        lblGameName = transform.Find("bg/lblGameName").GetComponent<UILabel>();
        lblHostName = transform.Find("bg/lblHostName").GetComponent<UILabel>();
        lblRoomId = transform.Find("bg/lblRoomId").GetComponent<UILabel>();
        lblPlayerCount = transform.Find("bg/lblPlayerCount").GetComponent<UILabel>();
        lblGameCount = transform.Find("bg/lblGameCount").GetComponent<UILabel>();
        lblRule = transform.Find("bg/lblRule").GetComponent<UILabel>();

        EventDelegate.Add(btnConfirm.onClick, OnBtnConfirmClick);
        EventDelegate.Add(btnClose.onClick, OnBtnCloseClick);

        gameObject.SetActive(false);
        bg.GetComponent<TweenScale>().ResetToBeginning();
        bg.GetComponent<TweenAlpha>().ResetToBeginning();
        shade.GetComponent<TweenAlpha>().ResetToBeginning();
    }

    public override void RegisterAction()
    {
        HallEvent.V_OpenDlgRoomInfo += Open;
    }

    public override void RemoveAction()
    {
        HallEvent.V_OpenDlgRoomInfo -= Open;
    }

    void Open(UInt16 id, string roomInfo)
    {
        Debug.Log("收到服务器信息， roomInfo ： " + roomInfo + ",   serverId : " + id);
        Debug.Log("serverId : " + id);
        gameObject.SetActive(true);
        bg.GetComponent<TweenScale>().PlayForward();
        bg.GetComponent<TweenAlpha>().PlayForward();
        shade.GetComponent<TweenAlpha>().PlayForward();

        serverId = id;

        string[] args = roomInfo.Split('^');

        if (args.Length >= 7)
        {
            lblGameName.text = args[0];
            lblHostName.text = args[1];
            lblRoomId.text = args[2];
            lblPlayerCount.text = args[3];
            lblGameCount.text = args[4];
            lblRule.text = ParseRule(args[6]);
        }
        else
        {
            args = new string[7];
            lblGameName.text = args[0];
            lblHostName.text = args[1];
            lblRoomId.text = args[2];
            lblPlayerCount.text = args[3];
            lblGameCount.text = args[4];
            lblRule.text = args[6];
        }
    }

    void Close()
    {
        bg.GetComponent<TweenScale>().PlayReverse();
        bg.GetComponent<TweenAlpha>().PlayReverse();
        shade.GetComponent<TweenAlpha>().PlayReverse();

        DoActionDelay(CloseCor, 0.4f);
    }

    void CloseCor()
    {
        gameObject.SetActive(false);        
    }



    void OnBtnConfirmClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonOp);

        HallModel.currentGameFlag = (GameFlag)HallModel.serverList[serverId].wKindID;
        HallModel.currentServerId = serverId;
        GameService.Instance.Connect();

        Close();
    }

    void OnBtnCloseClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonClose);
        Close();
    }


    string ParseRule(string arg)
    {
        string res = "";
        int rule = 0; 
        int.TryParse(arg, out rule);
        GameFlag gameFlag = (GameFlag)HallModel.serverList[serverId].wKindID;
        switch (gameFlag)
        {
            case GameFlag.Landlords3:
                res = "无";
                break;
        }
        return res;
    }

}
