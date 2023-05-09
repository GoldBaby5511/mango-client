using UnityEngine;
using System.Collections;

public class HallDlgJoinRoom : View
{
    private Transform bg;
    private Transform shade;

    private UILabel lblInfo;
    private UILabel[] lblNums = new UILabel[6];
    private GameObject[] btnNums = new GameObject[10];

    private UIButton btnReset;
    private UIButton btnDelete;
    private UIButton btnClose;

    private string roomNum = "";

    public override void Init()
    {
        bg = transform.Find("bg");
        shade = transform.Find("shade");

        lblInfo = transform.Find("bg/lblInfo").GetComponent<UILabel>();
        btnReset = transform.Find("bg/btnReset").GetComponent<UIButton>();
        btnDelete = transform.Find("bg/btnDelete").GetComponent<UIButton>();
        btnClose = transform.Find("bg/btnClose").GetComponent<UIButton>();
        for (int i = 0; i < 6; i++)
        {
            lblNums[i] = transform.Find("bg/lblNum_" + i).GetComponent<UILabel>();
        }
        for (int i = 0; i < 10; i++)
        {
            btnNums[i] = transform.Find("bg/btnNum_" + i).gameObject;
        }

        EventDelegate.Add(btnReset.onClick, OnBtnResetClick);
        EventDelegate.Add(btnDelete.onClick, OnBtnDeleteClick);
        EventDelegate.Add(btnClose.onClick, OnBtnCloseClick);
        for (int i = 0; i < 10; i++)
        {
            UIEventListener.Get(btnNums[i]).onClick = OnBtnNumClick;
        }
        gameObject.SetActive(false);
        bg.GetComponent<TweenScale>().ResetToBeginning();
        bg.GetComponent<TweenAlpha>().ResetToBeginning();
        shade.GetComponent<TweenAlpha>().ResetToBeginning();
    }

    public override void RegisterAction()
    {
        HallEvent.V_OpenDlgJoinRoom += Open;
        HallEvent.V_CloseDlgJoinRoom += Close;
        HallEvent.V_GetRoomFail += OnGetRoomFail;
    }

    public override void RemoveAction()
    {
        HallEvent.V_OpenDlgJoinRoom -= Open;
        HallEvent.V_CloseDlgJoinRoom -= Close;
        HallEvent.V_GetRoomFail -= OnGetRoomFail;
    }

    void Open()
    {
        gameObject.SetActive(true);
        bg.GetComponent<TweenScale>().PlayForward();
        bg.GetComponent<TweenAlpha>().PlayForward();
        shade.GetComponent<TweenAlpha>().PlayForward();

        lblInfo.text = "请输入6位房间ID";
        lblInfo.color = new Color(24f/255f, 190/255f, 80/255f);

        roomNum = "";
        ShowRoomNum();
    }

    void Close()
    {
        bg.GetComponent<TweenScale>().PlayReverse();
        bg.GetComponent<TweenAlpha>().PlayReverse();
        shade.GetComponent<TweenAlpha>().PlayReverse();

        DoActionDelay(CloseCor, 0.3f);
    }

    void CloseCor()
    {
        gameObject.SetActive(false);
    }
    
    //房间ID不存在
    void OnGetRoomFail(string res)
    {
        lblInfo.text = "房间ID不存在！";
        lblInfo.color = new Color(244/255f, 52/255f, 52/255f);
    }

    //显示房间ID
    void ShowRoomNum()
    {
        char[] nums = roomNum.ToCharArray();
        for (int i = 0; i < 6; i++)
        {
            if (i < nums.Length)
            {
                lblNums[i].text = nums[i].ToString();
            }
            else
            {
                lblNums[i].text = "";
            }
        }
    }

    void OnBtnNumClick(GameObject obj)
    {
        
        if (roomNum.Length >= 6)
        {
            AudioManager.Instance.PlaySound(GameModel.audioTipWrong);
            DoAction(GameEvent.V_OpenShortTip, "房间ID不能超过6位");
            return;
        }
        AudioManager.Instance.PlaySound(GameModel.audioButtonNormal);
        string num = obj.name.Split('_')[1];
        roomNum += num;
        ShowRoomNum();

        if (roomNum.Length == 6)
        {
            if (roomNum == "000000")
            {
                lblInfo.text = "房间ID不存在！";
                lblInfo.color = new Color(244 / 255f, 52 / 255f, 52 / 255f);
                return;
            }
            AudioManager.Instance.PlaySound(GameModel.audioButtonOp);
            GameModel.currentRoomId = uint.Parse(roomNum);
            HallModel.opOnLoginGame = OpOnLginGame.JoinRoom;
            HallService.Instance.GetRoomServerInfo(GameModel.ServerKind_Private, 0);
        }
    }

    //重输
    void OnBtnResetClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonNormal);
        roomNum = "";
        ShowRoomNum();

        lblInfo.text = "请输入6位房间ID";
        lblInfo.color = new Color(24f / 255f, 190 / 255f, 80 / 255f);
    }

    //删除
    void OnBtnDeleteClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonClose);
        
        if (roomNum.Length >= 1)
        {
            roomNum = roomNum.Remove(roomNum.Length - 1);
        }
        ShowRoomNum();

        lblInfo.text = "请输入6位房间ID";
        lblInfo.color = new Color(24f / 255f, 190 / 255f, 80 / 255f);
    }

    void OnBtnCloseClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonClose);
        Close();
    }
}
