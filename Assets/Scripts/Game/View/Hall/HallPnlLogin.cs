using UnityEngine;
using System.Collections;

public class HallPnlLogin : View
{
    private Transform bg;
    private UIButton btnLoginWx;
    private UIButton btnLoginGuest;
    private UIButton btnLoginPhone;
    private GameObject loginAccount;
    private UIButton btnTest001;
    private UIButton btnTest002;
    private UIButton btnTest003;
    private UIButton btnTest004;
    private UIButton btnTest005;
    private UIButton btnTest006;
    private UIButton btnTest007;
    private UIButton btnTest008;
    private UIInput inputAddress;

    private UILabel btnNetwork;
    private UILabel lblVersion;

    public override void Init()
    {
        bg = transform.Find("bg");
        btnLoginWx = transform.Find("bg/btnLoginWx").GetComponent<UIButton>();
        btnLoginGuest = transform.Find("bg/btnLoginGuest").GetComponent<UIButton>();
        btnLoginPhone = transform.Find("bg/btnLoginPhone").GetComponent<UIButton>();
        loginAccount = transform.Find("bg/loginAccount").gameObject;
        btnTest001 = transform.Find("bg/loginAccount/btnTest001").GetComponent<UIButton>();
        btnTest002 = transform.Find("bg/loginAccount/btnTest002").GetComponent<UIButton>();
        btnTest003 = transform.Find("bg/loginAccount/btnTest003").GetComponent<UIButton>();
        btnTest004 = transform.Find("bg/loginAccount/btnTest004").GetComponent<UIButton>();
        btnTest005 = transform.Find("bg/loginAccount/btnTest005").GetComponent<UIButton>();
        btnTest006 = transform.Find("bg/loginAccount/btnTest006").GetComponent<UIButton>();
        btnTest007 = transform.Find("bg/loginAccount/btnTest007").GetComponent<UIButton>();
        btnTest008 = transform.Find("bg/loginAccount/btnTest008").GetComponent<UIButton>();
        inputAddress = transform.Find("bg/inputAddress").GetComponent<UIInput>();
        btnNetwork = transform.Find("bg/btnNetwork").GetComponent<UILabel>();
        lblVersion = transform.Find("bg/lblVersion").GetComponent<UILabel>();

        EventDelegate.Add(btnLoginWx.onClick, OnBtnLoginWxClick);
        EventDelegate.Add(btnLoginGuest.onClick, OnBtnLoginGuestClick);
        EventDelegate.Add(btnLoginPhone.onClick, OnBtnLoginPhoneClick);
        EventDelegate.Add(btnTest001.onClick, OnBtnTest001Click);
        EventDelegate.Add(btnTest002.onClick, OnBtnTest002Click);
        EventDelegate.Add(btnTest003.onClick, OnBtnTest003Click);
        EventDelegate.Add(btnTest004.onClick, OnBtnTest004Click);
        EventDelegate.Add(btnTest005.onClick, OnBtnTest005Click);
        EventDelegate.Add(btnTest006.onClick, OnBtnTest006Click);
        EventDelegate.Add(btnTest007.onClick, OnBtnTest007Click);
        EventDelegate.Add(btnTest008.onClick, OnBtnTest008Click);
        EventDelegate.Add(btnNetwork.GetComponent<UIButton>().onClick, OnBtnNetworkClick);

        gameObject.SetActive(false);
        bg.GetComponent<TweenAlpha>().ResetToBeginning();
    }

    public override void RegisterAction()
    {
        HallEvent.V_OpenPnlLogin += Open;
        HallEvent.V_ClosePnlLogin += Close;
    }

    public override void RemoveAction()
    {
        HallEvent.V_OpenPnlLogin -= Open;
        HallEvent.V_ClosePnlLogin -= Close;
    }

    void Open()
    {
        gameObject.SetActive(true);
        loginAccount.SetActive(false);
        bg.GetComponent<TweenAlpha>().PlayForward();
        btnNetwork.text = AppConfig.isLocalNetwork ? "测试服" : "正式服";
        RefreshAddress();
        if ((HallModel.ruleLoginType & 1) != 0 && (HallModel.ruleLoginType & 2) != 0)
        {
            btnLoginWx.gameObject.SetActive(true);
            btnLoginPhone.gameObject.SetActive(true);
            btnLoginWx.transform.localPosition = new Vector3(200, -145, 0);
            btnLoginPhone.transform.localPosition = new Vector3(-200, -145, 0);
        }
        else if ((HallModel.ruleLoginType & 1) != 0)
        {
            btnLoginWx.gameObject.SetActive(true);
            btnLoginPhone.gameObject.SetActive(false);
            btnLoginWx.transform.localPosition = new Vector3(0, -145, 0);
        }
        else if ((HallModel.ruleLoginType & 2) != 0)
        {
            btnLoginWx.gameObject.SetActive(false);
            btnLoginPhone.gameObject.SetActive(true);
            btnLoginPhone.transform.localPosition = new Vector3(0, -145, 0);
        }

        lblVersion.text = "当前版本：" + AppConfig.version;
    }

    void Close()
    {
        bg.GetComponent<TweenAlpha>().PlayReverse();
        if (gameObject.activeSelf)
        {
            DoActionDelay(CloseCor, 0.4f);
        }
    }

    void CloseCor()
    {
        gameObject.SetActive(false);
    }


    //微信登录
    void OnBtnLoginWxClick()
    {
        if (!HallModel.isAgreeUserProtocol)
        {
            AudioManager.Instance.PlaySound(GameModel.audioTipWarn);
            DoAction(GameEvent.V_OpenShortTip, "请先同意用户使用协议！");
            return;
        }

        AudioManager.Instance.PlaySound(GameModel.audioButtonOp);
        if (AppConfig.isLocalNetwork)
        {
            AppConfig.localAddress[0] = inputAddress.value;
        }
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            AudioManager.Instance.PlaySound(GameModel.audioButtonOp);
            if (HallModel.userOpenId == "" || HallModel.isSwitchAccount)
            {
                //微信登录
                PluginManager.Instance.WxLogin();
            }
            else
            {
                HallService.Instance.InitAccountInfo(LoginType.OtherSDK, "", "", HallModel.userOpenId, "", "", "", "", "");
                HallService.Instance.Connect(ConnectType.Normal);
            }
        }
        else
        {
            loginAccount.SetActive(!loginAccount.activeSelf);
        }
    }

    void OnBtnLoginGuestClick()
    {
        if (!HallModel.isAgreeUserProtocol)
        {
            AudioManager.Instance.PlaySound(GameModel.audioTipWarn);
            DoAction(GameEvent.V_OpenShortTip, "请先同意用户使用协议！");
            return;
        }

        AudioManager.Instance.PlaySound(GameModel.audioButtonOp);
        HallService.Instance.InitAccountInfo(LoginType.Visitor, "", "", "", "", "", "", "", "");
        HallService.Instance.Connect(ConnectType.Normal);
    }

    //手机号登录
    void OnBtnLoginPhoneClick()
    {
        if (!HallModel.isAgreeUserProtocol)
        {
            AudioManager.Instance.PlaySound(GameModel.audioTipWarn);
            DoAction(GameEvent.V_OpenShortTip, "请先同意用户使用协议！");
            return;
        }
        AudioManager.Instance.PlaySound(GameModel.audioButtonOp);
        if (AppConfig.isLocalNetwork)
        {
            AppConfig.localAddress[0] = inputAddress.value;
        }

        DoAction(HallEvent.V_OpenDlgLogin);
    }

    #region Test

    void RefreshAddress()
    {
        inputAddress.gameObject.SetActive(AppConfig.isLocalNetwork);
        inputAddress.value = AppConfig.serverUrl[0];
    }

    void OnBtnTest001Click()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonOp);
        if (AppConfig.isLocalNetwork)
        {
            AppConfig.localAddress[0] = inputAddress.value;
        }
        HallService.Instance.InitAccountInfo(LoginType.Account, "111111", "123qwe", "", "Test001", "", "", "", "");
        HallService.Instance.Connect(ConnectType.Normal);
    }

    void OnBtnTest002Click()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonOp);
        if (AppConfig.isLocalNetwork)
        {
            AppConfig.localAddress[0] = inputAddress.value;
        }
        HallService.Instance.InitAccountInfo(LoginType.Account, "222222", "123qwe", "", "Test002", "", "", "", "");
        HallService.Instance.Connect(ConnectType.Normal);
    }

    void OnBtnTest003Click()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonOp);
        if (AppConfig.isLocalNetwork)
        {
            AppConfig.localAddress[0] = inputAddress.value;
        }
        HallService.Instance.InitAccountInfo(LoginType.Account, "test003", "test003", "", "Test003", "", "", "", "");
        HallService.Instance.Connect(ConnectType.Normal);
    }

    void OnBtnTest004Click()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonOp);
        if (AppConfig.isLocalNetwork)
        {
            AppConfig.localAddress[0] = inputAddress.value;
        }
        HallService.Instance.InitAccountInfo(LoginType.Account, "test004", "test004", "", "Test004", "", "", "", "");
        HallService.Instance.Connect(ConnectType.Normal);
    }

    void OnBtnTest005Click()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonOp);
        if (AppConfig.isLocalNetwork)
        {
            AppConfig.localAddress[0] = inputAddress.value;
        }
        HallService.Instance.InitAccountInfo(LoginType.Account, "test005", "test005", "", "Test005", "", "", "", "");
        HallService.Instance.Connect(ConnectType.Normal);
    }

    void OnBtnTest006Click()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonOp);
        if (AppConfig.isLocalNetwork)
        {
            AppConfig.localAddress[0] = inputAddress.value;
        }
        HallService.Instance.InitAccountInfo(LoginType.Account, "test006", "test006", "", "Test006", "", "", "", "");
        HallService.Instance.Connect(ConnectType.Normal);
    }

    void OnBtnTest007Click()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonOp);
        if (AppConfig.isLocalNetwork)
        {
            AppConfig.localAddress[0] = inputAddress.value;
        }
        HallService.Instance.InitAccountInfo(LoginType.Account, "test007", "test007", "", "Test007", "", "", "", "");
        HallService.Instance.Connect(ConnectType.Normal);
    }

    void OnBtnTest008Click()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonOp);
        if (AppConfig.isLocalNetwork)
        {
            AppConfig.localAddress[0] = inputAddress.value;
        }
        HallService.Instance.InitAccountInfo(LoginType.Account, "555555", "123qwe", "", "Test008", "", "", "", "");
        HallService.Instance.Connect(ConnectType.Normal);
    }

    void OnBtnNetworkClick()
    {
        AppConfig.isLocalNetwork = !AppConfig.isLocalNetwork;
        btnNetwork.text = AppConfig.isLocalNetwork ? "测试服" : "正式服";
        RefreshAddress();
    }

    #endregion
}
