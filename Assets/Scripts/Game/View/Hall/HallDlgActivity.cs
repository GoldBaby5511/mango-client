using UnityEngine;
using System.Collections;

public class HallDlgActivity : View
{
    private Transform bg;
    private Transform shade;

    private UIButton btnClose;
    //签到活动
    private UIButton btnSign;
    private GameObject pnlSign;
    private UIButton btnSignIn;
    private Transform[] signs = new Transform[7];
    //宣传活动
    private GameObject[] btnActivity = new GameObject[4];
    private GameObject[] pnlActivity = new GameObject[4];

    public override void Init()
    {
        bg = transform.Find("bg");
        shade = transform.Find("shade");

        btnClose = transform.Find("bg/Panel/btnClose").GetComponent<UIButton>();
        btnSign = transform.Find("bg/menu/Panel/btnSign").GetComponent<UIButton>();
        pnlSign = transform.Find("bg/pnlSign").gameObject;
        btnSignIn = transform.Find("bg/pnlSign/btnSignIn").GetComponent<UIButton>();
        for (int i = 0; i < 7; i++)
        {
            signs[i] = transform.Find("bg/pnlSign/sign_" + i);
        }

        for (int i = 0; i < 4; i++)
        {
            btnActivity[i] = transform.Find("bg/menu/Panel/btnActivity_" + i).gameObject;
            pnlActivity[i] = transform.Find("bg/pnlActivity_" + i).gameObject;
            UIEventListener.Get(btnActivity[i]).onClick = OnBtnActivityClick;
        }

        EventDelegate.Add(btnClose.onClick, OnBtnCloseClick);
        EventDelegate.Add(btnSign.onClick, OnBtnSignClick);
        EventDelegate.Add(btnSignIn.onClick, OnBtnSignInClick);

        gameObject.SetActive(false);
        bg.GetComponent<TweenScale>().ResetToBeginning();
        bg.GetComponent<TweenAlpha>().ResetToBeginning();
        shade.GetComponent<TweenAlpha>().ResetToBeginning();
    }

    public override void RegisterAction()
    {
        HallEvent.V_OpenDlgActivity += Open;
        HallEvent.V_RefreshPnlSign += RefreshPnlSign;
    }

    public override void RemoveAction()
    {
        HallEvent.V_OpenDlgActivity -= Open;
        HallEvent.V_RefreshPnlSign -= RefreshPnlSign;
    }

    void Open()
    {
        gameObject.SetActive(true);
        bg.GetComponent<TweenScale>().PlayForward();
        bg.GetComponent<TweenAlpha>().PlayForward();
        shade.GetComponent<TweenAlpha>().PlayForward();

        OpenSignActivity();
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

    //打开宣传活动
    void OpenActivity(int index)
    {
        CloseAllActivity();

        btnActivity[index].GetComponent<UIButton>().isEnabled = false;
        pnlActivity[index].SetActive(true);
    }

    //打开签到活动
    void OpenSignActivity()
    {
        CloseAllActivity();
        btnSign.isEnabled = false;
        pnlSign.SetActive(true);
        RefreshPnlSign();
    }

    void CloseAllActivity()
    {
        for (int i = 0; i < 4; i++)
        {
            btnActivity[i].GetComponent<UIButton>().isEnabled = true;
            pnlActivity[i].SetActive(false);
        }
        btnSign.isEnabled = true;
        pnlSign.SetActive(false);
    }


    //刷新签到面板
    void RefreshPnlSign()
    {
//         for (int i = 0; i < 7; i++)
//         {
//             if (i < 6)
//             {
//                 if (HallModel.signCards[i] > 0)
//                 {
//                     signs[i].Find("lblCount").GetComponent<UILabel>().text = "房卡 x " + HallModel.signCards[i];
//                     signs[i].Find("sptGift").GetComponent<UISprite>().spriteName = "flag房卡";
//                 }
//                 else if (HallModel.signDiamonds[i] > 0)
//                 {
//                     signs[i].Find("lblCount").GetComponent<UILabel>().text = "钻石 x " + HallModel.signDiamonds[i];
//                     signs[i].Find("sptGift").GetComponent<UISprite>().spriteName = "flag钻石";
//                 }
//             }
//             signs[i].Find("Sprite").gameObject.SetActive(i < HallModel.signDay);
//         }
//         btnSignIn.isEnabled = HallModel.signDay < 7 && !HallModel.isSign;
    }





    //签到活动
    void OnBtnSignClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonSwitch);
        OpenSignActivity();
    }

    //宣传活动
    void OnBtnActivityClick(GameObject obj)
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonSwitch);
        int index = int.Parse(obj.name.Split('_')[1]);
        OpenActivity(index);
    }

    void OnBtnSignInClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonNormal);
        HallService.Instance.SignIn();
    }

    void OnBtnCloseClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonClose);
        Close();
    }

}
