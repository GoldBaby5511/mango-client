using UnityEngine;
using System.Collections;

public class HallDlgLogin : View 
{
    private GameObject help;

    private Transform bg;
    private Transform shade;

    private UIInput inputPhoneNum;
    private UIInput inputPassword;

    private GameObject btnRememberAccount;
    private GameObject btnRememberPassword;

    private UIButton btnLoginHelp;
    private UIButton btnConfirm;
    private UIButton btnGetPassword;
    private UIButton btnClose;

    public override void Init()
    {
        help = transform.Find("help").gameObject;

        bg = transform.Find("bg");
        shade = transform.Find("shade");

        inputPhoneNum = transform.Find("bg/inputPhoneNum").GetComponent<UIInput>();
        inputPassword = transform.Find("bg/inputPassword").GetComponent<UIInput>();
        btnRememberAccount = transform.Find("bg/btnRememberAccount").gameObject;
        btnRememberPassword = transform.Find("bg/btnRememberPassword").gameObject;

        btnLoginHelp = transform.Find("bg/btnLoginHelp").GetComponent<UIButton>();
        btnConfirm = transform.Find("bg/btnConfirm").GetComponent<UIButton>();
        btnGetPassword = transform.Find("bg/btnGetPassword").GetComponent<UIButton>();
        btnClose = transform.Find("bg/btnClose").GetComponent<UIButton>();

        EventDelegate.Add(btnLoginHelp.onClick, OnBtnLoginHelpClick);
        EventDelegate.Add(btnConfirm.onClick, OnBtnConfirmClick);
        EventDelegate.Add(btnGetPassword.onClick, OnBtnGetPasswordClick);
        EventDelegate.Add(btnClose.onClick, OnBtnCloseClick);
        UIEventListener.Get(btnRememberAccount).onClick = OnBtnRememberAccountClick;
        UIEventListener.Get(btnRememberPassword).onClick = OnBtnRememberPasswordClick;

        gameObject.SetActive(false);
        help.SetActive(false);
        bg.GetComponent<TweenScale>().ResetToBeginning();
        bg.GetComponent<TweenAlpha>().ResetToBeginning();
        shade.GetComponent<TweenAlpha>().ResetToBeginning();
    }

    public override void RegisterAction()
    {
        HallEvent.V_OpenDlgLogin += Open;
    }

    public override void RemoveAction()
    {
        HallEvent.V_OpenDlgLogin -= Open;
    }

    #region UI操作

    public void Open()
    {
        gameObject.SetActive(true);
        bg.GetComponent<TweenScale>().PlayForward();
        bg.GetComponent<TweenAlpha>().PlayForward();
        shade.GetComponent<TweenAlpha>().PlayForward();

        help.SetActive(true);
        help.GetComponent<TweenPosition>().ResetToBeginning();
        help.GetComponent<TweenPosition>().PlayForward();

        inputPhoneNum.value = "";
        inputPassword.value = "";
        btnRememberAccount.GetComponent<UISprite>().spriteName = HallModel.isRememberUserAccount ? "多选02" : "多选01";
        btnRememberPassword.GetComponent<UISprite>().spriteName = HallModel.isRememberUserPassword ? "多选02" : "多选01";
        if (HallModel.isRememberUserAccount)
        {
            inputPhoneNum.value = HallModel.userAccount;
        }
        if (HallModel.isRememberUserPassword)
        {
            inputPassword.value = HallModel.userPassword;
        }
    }

    public void Close()
    {
        help.SetActive(false);
        bg.GetComponent<TweenScale>().PlayReverse();
        bg.GetComponent<TweenAlpha>().PlayReverse();
        shade.GetComponent<TweenAlpha>().PlayReverse();
        DoActionDelay(CloseCor, 0.4f);
    }

    public void CloseCor()
    {
        gameObject.SetActive(false);
    }

    #endregion

    #region UI响应

    void OnBtnLoginHelpClick()
    {
        if (help.activeSelf)
        {
            help.SetActive(false);
        }
        else
        {
            help.SetActive(true);
            help.GetComponent<TweenPosition>().ResetToBeginning();
            help.GetComponent<TweenPosition>().PlayForward();
        }
    }

    void OnBtnConfirmClick()
    {
        string account = inputPhoneNum.value;
        string password = inputPassword.value;
        //if (account.Length != 11)
        //{
        //    AudioManager.Instance.PlaySound(GameModel.audioTipWarn);
        //    DoAction(GameEvent.V_OpenShortTip, "请输入11位手机号码！");
        //    return;
        //}
        AudioManager.Instance.PlaySound(GameModel.audioButtonOp);
        HallService.Instance.InitAccountInfo(LoginType.Account, account, password, "", "", "", "", "", "");
        HallService.Instance.Connect(ConnectType.Normal);
        Close();
    }

    void OnBtnGetPasswordClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonOp);
        DoAction(HallEvent.V_OpenDlgGetPassword);
        Close();
    }

    void OnBtnRememberAccountClick(GameObject obj)
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonSwitch);
        HallModel.isRememberUserAccount = !HallModel.isRememberUserAccount;
        btnRememberAccount.GetComponent<UISprite>().spriteName = HallModel.isRememberUserAccount ? "多选02" : "多选01";
    }

    void OnBtnRememberPasswordClick(GameObject obj)
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonSwitch);
        HallModel.isRememberUserPassword = !HallModel.isRememberUserPassword;
        btnRememberPassword.GetComponent<UISprite>().spriteName = HallModel.isRememberUserPassword ? "多选02" : "多选01";
    }

    void OnBtnCloseClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonClose);
        Close();
    }

    #endregion
}
