using UnityEngine;
using System.Collections;

public class HallDlgGetPassword : View 
{

    private Transform bg;
    private Transform shade;

    private UIInput inputPhoneNum;
    private UIInput inputPhoneCode;
    private UIInput inputPassword;

    private UIButton btnSendCode;
    private UIButton btnConfirm;
    private UIButton btnClose;

    private UILabel lblTimer;

    public override void Init()
    {
        bg = transform.Find("bg");
        shade = transform.Find("shade");

        inputPhoneNum = transform.Find("bg/inputPhoneNum").GetComponent<UIInput>();
        inputPhoneCode = transform.Find("bg/inputPhoneCode").GetComponent<UIInput>();
        inputPassword = transform.Find("bg/inputPassword").GetComponent<UIInput>();

        btnSendCode = transform.Find("bg/btnSendCode").GetComponent<UIButton>();
        btnConfirm = transform.Find("bg/btnConfirm").GetComponent<UIButton>();
        btnClose = transform.Find("bg/btnClose").GetComponent<UIButton>();

        lblTimer = transform.Find("bg/lblTimer").GetComponent<UILabel>();

        EventDelegate.Add(btnSendCode.onClick, OnBtnSendCodeClick);
        EventDelegate.Add(btnConfirm.onClick, OnBtnConfirmClick);
        EventDelegate.Add(btnClose.onClick, OnBtnCloseClick);

        gameObject.SetActive(false);
        bg.GetComponent<TweenScale>().ResetToBeginning();
        bg.GetComponent<TweenAlpha>().ResetToBeginning();
        shade.GetComponent<TweenAlpha>().ResetToBeginning();
    }

    public override void RegisterAction()
    {
        HallEvent.V_OpenDlgGetPassword += Open;
    }

    public override void RemoveAction()
    {
        HallEvent.V_OpenDlgGetPassword -= Open;
    }

    #region UI操作

    public void Open()
    {
        gameObject.SetActive(true);
        bg.GetComponent<TweenScale>().PlayForward();
        bg.GetComponent<TweenAlpha>().PlayForward();
        shade.GetComponent<TweenAlpha>().PlayForward();

        inputPhoneNum.value = "";
        inputPhoneCode.value = "";
        inputPassword.value = "";
        lblTimer.text = "";
        btnSendCode.isEnabled = true;
    }

    public void Close()
    {
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

    void OnBtnConfirmClick()
    {
        string phoneNum = inputPhoneNum.value;
        string phoneCode = inputPhoneCode.value;
        string password = inputPassword.value;
        if (phoneNum.Length != 11)
        {
            AudioManager.Instance.PlaySound(GameModel.audioTipWarn);
            DoAction(GameEvent.V_OpenShortTip, "请输入11位手机号码！");
            return;
        }
        if (phoneCode.Length == 0)
        {
            AudioManager.Instance.PlaySound(GameModel.audioTipWarn);
            DoAction(GameEvent.V_OpenShortTip, "验证码不能为空！");
            return;
        }
        if (password.Length == 0)
        {
            AudioManager.Instance.PlaySound(GameModel.audioTipWarn);
            DoAction(GameEvent.V_OpenShortTip, "密码不能为空！");
            return;
        }
        AudioManager.Instance.PlaySound(GameModel.audioButtonOp);
        HallService.Instance.InitAccountInfo(LoginType.Account, phoneNum, password, "", "", "", "", "", phoneCode);
        HallService.Instance.Connect(ConnectType.Normal);
        Close();
    }

    void OnBtnSendCodeClick()
    {
        string phoneNum = inputPhoneNum.value;
        if (phoneNum.Length != 11)
        {
            AudioManager.Instance.PlaySound(GameModel.audioTipWarn);
            DoAction(GameEvent.V_OpenShortTip, "请输入11位手机号码！");
            return;
        }
        AudioManager.Instance.PlaySound(GameModel.audioButtonNormal);
        btnSendCode.isEnabled = false;
        StartTimer(30);
        SendPhoneCode(phoneNum);
    }

    void OnBtnCloseClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonClose);
        Close();
    }


    //发送验证码
    void SendPhoneCode(string phoneNum)
    {
        Web_C_ResetPassword pro = new Web_C_ResetPassword();
        pro.loginName = phoneNum;
        pro.mobiePhone = phoneNum;
        pro.type = 0;
        WebService.Instance.Send<Web_S_ResetPassword>(AppConfig.url_ResetPassword, pro, OnSendPhoneCode);
    }

    //发送验证码结果
    void OnSendPhoneCode(Web_S_ResetPassword pro)
    {
        if (pro.return_code == 10000)
        {
            DoAction(GameEvent.V_OpenShortTip, "发送验证码成功！");
        }
        else
        {
            DoAction(GameEvent.V_OpenShortTip, pro.return_message);
            StopTimer();
        }
    }

    #endregion

    private int timer = 30;

    void StartTimer(int time)
    {
        timer = time;
        CancelInvoke("TimeCount");
        InvokeRepeating("TimeCount", 0, 1);
    }

    void StopTimer()
    {
        CancelInvoke("TimeCount");
        lblTimer.text = "";
        btnSendCode.isEnabled = true;
    }

    void TimeCount()
    {
        lblTimer.text = timer + "s";
        timer--;
        if (timer < 0)
        {
            StopTimer();
        }
    }
}
