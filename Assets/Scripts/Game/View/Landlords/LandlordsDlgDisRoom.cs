using UnityEngine;
using System.Collections;

public class LandlordsDlgDisRoom : View
{
    private Transform bg;
    private Transform shade;

    private UILabel lblTip;

    private Transform[] users = new Transform[3];

    private UIButton btnAgree;
    private UIButton btnDisAgree;
    private UIButton btnClose;
    public override void Init()
    {
        bg = transform.Find("bg");
        shade = transform.Find("shade");

        lblTip = transform.Find("bg/lblTip").GetComponent<UILabel>();
        btnAgree = transform.Find("bg/btnAgree").GetComponent<UIButton>();
        btnDisAgree = transform.Find("bg/btnDisAgree").GetComponent<UIButton>();
        btnClose = transform.Find("bg/btnClose").GetComponent<UIButton>();

        for (int i = 0; i < 3; i++)
        {
            users[i] = transform.Find("bg/user_" + i);
        }

        EventDelegate.Add(btnAgree.onClick, OnBtnAgreeClick);
        EventDelegate.Add(btnDisAgree.onClick, OnBtnDisAgreeClick);
        EventDelegate.Add(btnClose.onClick, OnBtnCloseClick);

        gameObject.SetActive(false);
        bg.GetComponent<TweenScale>().ResetToBeginning();
        bg.GetComponent<TweenAlpha>().ResetToBeginning();
        shade.GetComponent<TweenAlpha>().ResetToBeginning();
    }

    public override void RegisterAction()
    {
        GameEvent.V_OpenDlgDisRoom += Open;
        GameEvent.V_CloseDlgDisRoom += Close;
    }

    public override void RemoveAction()
    {
        GameEvent.V_OpenDlgDisRoom -= Open;
        GameEvent.V_CloseDlgDisRoom -= Close;
    }

    #region UI方法

    public void Open(CMD_Game_S_DisRoomInfo pro)
    {
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
            bg.GetComponent<TweenScale>().PlayForward();
            bg.GetComponent<TweenAlpha>().PlayForward();
            shade.GetComponent<TweenAlpha>().PlayForward();
        }
        lblTip.text = "开始解散房间投票，是否同意？";
        if (GameModel.playerInRoom.ContainsKey(pro.dwUserId))
        {
            lblTip.text = "玩家【" + GameModel.playerInRoom[pro.dwUserId].nickName + "】申请解散房间，是否同意？";
        }
        for (int i = 0; i < 3; i++)
        {
            PlayerInRoom player = GameModel.GetDeskUser(i);
            if (player != null)
            {
                users[i].Find("photo").GetComponent<UITexture>().mainTexture = GameModel.GetUserPhoto(i);
                users[i].Find("lblUserName").GetComponent<UILabel>().text = player.nickName;
                if (pro.agreeUserList.Contains(player.dwUserID))
                {
                    users[i].Find("lblState").GetComponent<UILabel>().text = "";
                    users[i].Find("flagResult").GetComponent<UISprite>().spriteName = "flag同意";
                }
                else if (pro.disAgreeUserList.Contains(player.dwUserID))
                {
                    users[i].Find("lblState").GetComponent<UILabel>().text = "";
                    users[i].Find("flagResult").GetComponent<UISprite>().spriteName = "flag不同意";
                }
                else
                {
                    users[i].Find("lblState").GetComponent<UILabel>().text = "正在思考...";
                    users[i].Find("flagResult").GetComponent<UISprite>().spriteName = "";
                }
            }
            else
            {
                users[i].Find("photo").GetComponent<UITexture>().mainTexture = HallModel.defaultPhoto;
                users[i].Find("lblUserName").GetComponent<UILabel>().text = "***";
                users[i].Find("lblState").GetComponent<UILabel>().text = "...";
                users[i].Find("flagResult").GetComponent<UISprite>().spriteName = "";
            }
        }
        //用户已投票
        if (pro.agreeUserList.Contains((uint)HallModel.userId) || pro.disAgreeUserList.Contains((uint)HallModel.userId))
        {
            btnAgree.isEnabled = false;
            btnDisAgree.isEnabled = false;
        }
        else
        {
            btnAgree.isEnabled = true;
            btnDisAgree.isEnabled = true;
        }
    }

    public void Close()
    {
        bg.GetComponent<TweenScale>().ResetToBeginning();
        bg.GetComponent<TweenAlpha>().ResetToBeginning();
        shade.GetComponent<TweenAlpha>().ResetToBeginning();
        DoActionDelay(CloseCor, 0.4f);
    }

    void CloseCor()
    {
        gameObject.SetActive(false);
    }

    #endregion



    #region UI响应

    public void OnBtnAgreeClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonNormal);
        btnAgree.isEnabled = false;
        btnDisAgree.isEnabled = false;
        GameService.Instance.DisRoom(true);
    }

    public void OnBtnDisAgreeClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonNormal);
        btnAgree.isEnabled = false;
        btnDisAgree.isEnabled = false;
        GameService.Instance.DisRoom(false);
    }

    public void OnBtnCloseClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonClose);
        Close();
    }

    #endregion


}
