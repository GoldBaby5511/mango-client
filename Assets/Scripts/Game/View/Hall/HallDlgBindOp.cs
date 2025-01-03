using UnityEngine;
using System.Collections;

public class HallDlgBindOp : View 
{
    //dlgBindPhone
    private GameObject dlgBindPhone;
    private UIInput inputPhoneNum_bindPhone;
    private UIInput inputPhoneCode_bindPhone;
    private UIButton btnGetCode_bindPhone;
    private UIButton btnConfirm_bindPhone;
    private UIButton btnClose_bindPhone;
    private UILabel lblTimer_bindPhone;
    //dlgUnBindPhone
    private GameObject dlgUnBindPhone;
    private UIInput inputPhoneCode_unBindPhone;
    private UIButton btnGetCode_unBindPhone;
    private UIButton btnConfirm_unBindPhone;
    private UIButton btnClose_unBindPhone;
    private UILabel lblPhoneNum_unBindPhone;
    private UILabel lblTimer_unBindPhone;
    //dlgBindAgency
    private GameObject dlgBindAgency;
    private UIInput inputAgencyAccount_bindAgency;
    private UIButton btnConfirm_bindAgency;
    private UIButton btnClose_bindAgency;
    //dlgBindSpreader
    private GameObject dlgBindSpreader;
    private UIInput inputAgencyAccount_bindSpreader;
    private UIButton btnConfirm_bindSpreader;
    private UIButton btnClose_bindSpreader;

    private Transform shade;

    private int timer = 0;
    private int opCode = 0;     //0:发送验证码，1：绑定手机,2：解除手机绑定 

    public override void Init()
    {
        dlgBindPhone = transform.Find("dlgBindPhone").gameObject;
        inputPhoneNum_bindPhone = transform.Find("dlgBindPhone/inputPhoneNum").GetComponent<UIInput>();
        inputPhoneCode_bindPhone = transform.Find("dlgBindPhone/inputPhoneCode").GetComponent<UIInput>();
        btnGetCode_bindPhone = transform.Find("dlgBindPhone/btnGetCode").GetComponent<UIButton>();
        btnConfirm_bindPhone = transform.Find("dlgBindPhone/btnConfirm").GetComponent<UIButton>();
        btnClose_bindPhone = transform.Find("dlgBindPhone/btnClose").GetComponent<UIButton>();
        lblTimer_bindPhone = transform.Find("dlgBindPhone/lblTimer").GetComponent<UILabel>();

        dlgUnBindPhone = transform.Find("dlgUnBindPhone").gameObject;
        inputPhoneCode_unBindPhone = transform.Find("dlgUnBindPhone/inputPhoneCode").GetComponent<UIInput>();
        btnGetCode_unBindPhone = transform.Find("dlgUnBindPhone/btnGetCode").GetComponent<UIButton>();
        btnConfirm_unBindPhone = transform.Find("dlgUnBindPhone/btnConfirm").GetComponent<UIButton>();
        btnClose_unBindPhone = transform.Find("dlgUnBindPhone/btnClose").GetComponent<UIButton>();
        lblPhoneNum_unBindPhone = transform.Find("dlgUnBindPhone/lblPhoneNum").GetComponent<UILabel>();
        lblTimer_unBindPhone = transform.Find("dlgUnBindPhone/lblTimer").GetComponent<UILabel>();

        dlgBindAgency = transform.Find("dlgBindAgency").gameObject;
        inputAgencyAccount_bindAgency = transform.Find("dlgBindAgency/inputAgencyAccount").GetComponent<UIInput>();
        btnConfirm_bindAgency = transform.Find("dlgBindAgency/btnConfirm").GetComponent<UIButton>();
        btnClose_bindAgency = transform.Find("dlgBindAgency/btnClose").GetComponent<UIButton>();

        dlgBindSpreader = transform.Find("dlgBindSpreader").gameObject;
        inputAgencyAccount_bindSpreader = transform.Find("dlgBindSpreader/inputAgencyAccount").GetComponent<UIInput>();
        btnConfirm_bindSpreader = transform.Find("dlgBindSpreader/btnConfirm").GetComponent<UIButton>();
        btnClose_bindSpreader = transform.Find("dlgBindSpreader/btnClose").GetComponent<UIButton>();

        shade = transform.Find("shade");

        EventDelegate.Add(btnGetCode_bindPhone.onClick, OnBtnGetCodeClick_bindPhone);
        EventDelegate.Add(btnGetCode_unBindPhone.onClick, OnBtnGetCodeClick_unBindPhone);

        EventDelegate.Add(btnConfirm_bindPhone.onClick, OnBtnConfirmClick_bindPhone);
        EventDelegate.Add(btnConfirm_unBindPhone.onClick, OnBtnConfirmClick_unBindPhone);
        EventDelegate.Add(btnConfirm_bindAgency.onClick, OnBtnConfirmClick_BindAgency);
        EventDelegate.Add(btnConfirm_bindSpreader.onClick, OnBtnConfirmClick_BindSpreader);
        EventDelegate.Add(btnClose_bindPhone.onClick, OnBtnCloseClick);
        EventDelegate.Add(btnClose_unBindPhone.onClick, OnBtnCloseClick);
        EventDelegate.Add(btnClose_bindAgency.onClick, OnBtnCloseClick);
        EventDelegate.Add(btnClose_bindSpreader.onClick, OnBtnCloseClick);

        gameObject.SetActive(false);
        dlgBindPhone.SetActive(false);
        dlgUnBindPhone.SetActive(false);
        dlgBindAgency.SetActive(false);
        dlgBindSpreader.SetActive(false);
        dlgBindPhone.GetComponent<TweenScale>().ResetToBeginning();
        dlgBindPhone.GetComponent<TweenAlpha>().ResetToBeginning();
        dlgUnBindPhone.GetComponent<TweenScale>().ResetToBeginning();
        dlgUnBindPhone.GetComponent<TweenAlpha>().ResetToBeginning();
        dlgBindAgency.GetComponent<TweenScale>().ResetToBeginning();
        dlgBindAgency.GetComponent<TweenAlpha>().ResetToBeginning();
        dlgBindSpreader.GetComponent<TweenScale>().ResetToBeginning();
        dlgBindSpreader.GetComponent<TweenAlpha>().ResetToBeginning();
        shade.GetComponent<TweenAlpha>().ResetToBeginning();
    }

    public override void RegisterAction()
    {
        HallEvent.V_OpenDlgBindOp += Open;
    }

    public override void RemoveAction()
    {
        HallEvent.V_OpenDlgBindOp -= Open;
    }

    #region UI操作

    public void Open(DlgBindArg arg)
    {
        gameObject.SetActive(true);
        switch(arg)
        {
            case DlgBindArg.BindPhonePage:
                OpenDlgBindPhone();
                break;
            case DlgBindArg.UnBindPhonePage:
                OpenDlgUnBindPhone();
                break;
            case DlgBindArg.BindAgency:
                OpenDlgBindAgency();
                break;
            case DlgBindArg.BindSpreader:
                OpenDlgBindSpreader();
                break;
        }
        shade.GetComponent<TweenAlpha>().PlayForward();
    }

    public void Close()
    {
        CancelInvoke("TimeCount");
        if (dlgBindPhone.activeSelf)
        {
            dlgBindPhone.GetComponent<TweenScale>().PlayReverse();
            dlgBindPhone.GetComponent<TweenAlpha>().PlayReverse();
        }
        if (dlgUnBindPhone.activeSelf)
        {
            dlgUnBindPhone.GetComponent<TweenScale>().PlayReverse();
            dlgUnBindPhone.GetComponent<TweenAlpha>().PlayReverse();
        }
        if (dlgBindAgency.activeSelf)
        {
            dlgBindAgency.GetComponent<TweenScale>().PlayReverse();
            dlgBindAgency.GetComponent<TweenAlpha>().PlayReverse();
        }
        if (dlgBindSpreader.activeSelf)
        {
            dlgBindSpreader.GetComponent<TweenScale>().PlayReverse();
            dlgBindSpreader.GetComponent<TweenAlpha>().PlayReverse();
        }
        shade.GetComponent<TweenAlpha>().PlayReverse();
        DoActionDelay(CloseCor, 0.4f);
    }

    public void CloseCor()
    {
        gameObject.SetActive(false);
        dlgBindPhone.SetActive(false);
        dlgUnBindPhone.SetActive(false);
        dlgBindAgency.SetActive(false);
        dlgBindSpreader.SetActive(false);
    }

    void OpenDlgBindPhone()
    {
        dlgBindPhone.SetActive(true);
        dlgUnBindPhone.SetActive(false);
        dlgBindAgency.SetActive(false);
        dlgBindSpreader.SetActive(false);
        dlgBindPhone.GetComponent<TweenScale>().PlayForward();
        dlgBindPhone.GetComponent<TweenAlpha>().PlayForward();
        inputPhoneCode_bindPhone.value = "";
        inputPhoneNum_bindPhone.value = "";
        lblTimer_bindPhone.text = "";
        btnGetCode_bindPhone.isEnabled = true;
    }

    void OpenDlgUnBindPhone()
    {
        dlgBindPhone.SetActive(false);
        dlgUnBindPhone.SetActive(true);
        dlgBindAgency.SetActive(false);
        dlgBindSpreader.SetActive(false);
        dlgUnBindPhone.GetComponent<TweenScale>().PlayForward();
        dlgUnBindPhone.GetComponent<TweenAlpha>().PlayForward();
        inputPhoneCode_unBindPhone.value = "";
        string phoneNum = HallModel.userPhone.Remove(3, 4);
        phoneNum = phoneNum.Insert(3, "****");
        lblPhoneNum_unBindPhone.text = phoneNum;
        lblTimer_unBindPhone.text = "";
        btnGetCode_unBindPhone.isEnabled = true;
    }

    void OpenDlgBindAgency()
    {
        dlgBindPhone.SetActive(false);
        dlgUnBindPhone.SetActive(false);
        dlgBindAgency.SetActive(true);
        dlgBindSpreader.SetActive(false);
        dlgBindAgency.GetComponent<TweenScale>().PlayForward();
        dlgBindAgency.GetComponent<TweenAlpha>().PlayForward();
        inputAgencyAccount_bindAgency.value = "";
    }

    void OpenDlgBindSpreader()
    {
        dlgBindPhone.SetActive(false);
        dlgUnBindPhone.SetActive(false);
        dlgBindAgency.SetActive(false);
        dlgBindSpreader.SetActive(true);
        dlgBindSpreader.GetComponent<TweenScale>().PlayForward();
        dlgBindSpreader.GetComponent<TweenAlpha>().PlayForward();
        inputAgencyAccount_bindSpreader.value = "";
    }



    void StartTimeCount(int _timer)
    {
        timer = _timer;
        CancelInvoke("TimeCount");
        InvokeRepeating("TimeCount", 0, 1);
    }

    void StopTimeCount()
    {
        CancelInvoke("TimeCount");
        if (dlgBindPhone.activeSelf)
        {
            lblTimer_bindPhone.text = "";
            btnGetCode_bindPhone.isEnabled = true;
        }
        else if (dlgUnBindPhone.activeSelf)
        {
            lblTimer_unBindPhone.text = "";
            btnGetCode_unBindPhone.isEnabled = true;
        }
    }

    void TimeCount()
    {
        if (dlgBindPhone.activeSelf)
        {
            lblTimer_bindPhone.text = timer + "s";
        }
        else if (dlgUnBindPhone.activeSelf)
        {
            lblTimer_unBindPhone.text = timer + "s";
        }
        timer--;
        if (timer < 0)
        {
            CancelInvoke("TimeCount");
            if (dlgBindPhone.activeSelf)
            {
                lblTimer_bindPhone.text = "";
                btnGetCode_bindPhone.isEnabled = true;
            }
            else if (dlgUnBindPhone.activeSelf)
            {
                lblTimer_unBindPhone.text = "";
                btnGetCode_unBindPhone.isEnabled = true;
            }
        }
    }

    #endregion

    #region UI响应

    void OnBtnGetCodeClick_bindPhone()
    {
        string phoneNum = inputPhoneNum_bindPhone.value;
        if (phoneNum.Length != 11)
        {
            AudioManager.Instance.PlaySound(GameModel.audioTipWarn);
            DoAction(GameEvent.V_OpenShortTip, "手机号码不符合要求！");
            return;
        }

        AudioManager.Instance.PlaySound(GameModel.audioButtonNormal);
        //请求手机验证码
        //Web_C_BindPhoneOp pro = new Web_C_BindPhoneOp();
        //pro.userid = HallModel.userId;
        //pro.phone = phoneNum;
        //pro.code = "";
        //pro.type = 0;
        //pro.codeType = 1;
        //string str = Util.GetMd5(pro.userid + pro.phone + AppConfig.webKey1 + pro.type + pro.codeType);
        //pro.sigin = Util.GetMd5(str + AppConfig.webKey2);
        //WebService.Instance.Send<Web_S_BindPhoneOp>(AppConfig.url_BindPhoneOp, pro, OnGetBindPhoneOpResult);
        ////60秒倒计时
        //opCode = pro.type;
        //btnGetCode_bindPhone.isEnabled = false;
        //StartTimeCount(60);
    }

    void OnBtnConfirmClick_bindPhone()
    {
        string phoneNum = inputPhoneNum_bindPhone.value;
        string code = inputPhoneCode_bindPhone.value;
        if (phoneNum.Length != 11)
        {
            AudioManager.Instance.PlaySound(GameModel.audioTipWarn);
            DoAction(GameEvent.V_OpenShortTip, "手机号码不符合要求！");
            return;
        }
        if (code.Length < 3)
        {
            AudioManager.Instance.PlaySound(GameModel.audioTipWarn);
            DoAction(GameEvent.V_OpenShortTip, "验证码不能为空！");
            return;
        }
        AudioManager.Instance.PlaySound(GameModel.audioButtonNormal);
        //绑定手机
        //Web_C_BindPhoneOp pro = new Web_C_BindPhoneOp();
        //pro.userid = HallModel.userId;
        //pro.phone = phoneNum;
        //pro.code = code;
        //pro.type = 1;
        //pro.codeType = 1;
        //string str = Util.GetMd5(pro.userid + pro.phone + AppConfig.webKey1 + pro.type + pro.codeType);
        //pro.sigin = Util.GetMd5(str + AppConfig.webKey2);
        //WebService.Instance.Send<Web_S_BindPhoneOp>(AppConfig.url_BindPhoneOp, pro, OnGetBindPhoneOpResult);

        //opCode = pro.type;
    }

    void OnBtnGetCodeClick_unBindPhone()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonNormal);
        //请求手机验证码
        //Web_C_BindPhoneOp pro = new Web_C_BindPhoneOp();
        //pro.userid = HallModel.userId;
        //pro.phone = HallModel.userPhone;
        //pro.code = "";
        //pro.type = 0;
        //pro.codeType = 2;
        //string str = Util.GetMd5(pro.userid + pro.phone + AppConfig.webKey1 + pro.type + pro.codeType);
        //pro.sigin = Util.GetMd5(str + AppConfig.webKey2);
        //WebService.Instance.Send<Web_S_BindPhoneOp>(AppConfig.url_BindPhoneOp, pro, OnGetBindPhoneOpResult);
        ////60秒倒计时
        //opCode = pro.type;
        //btnGetCode_unBindPhone.isEnabled = false;
        //StartTimeCount(60);
    }

    void OnBtnConfirmClick_unBindPhone()
    {
        string code = inputPhoneCode_unBindPhone.value;
        if (code.Length < 3)
        {
            AudioManager.Instance.PlaySound(GameModel.audioTipWarn);
            DoAction(GameEvent.V_OpenShortTip, "验证码不能为空！");
            return;
        }
        AudioManager.Instance.PlaySound(GameModel.audioButtonNormal);
        //解绑手机
        //Web_C_BindPhoneOp pro = new Web_C_BindPhoneOp();
        //pro.userid = HallModel.userId;
        //pro.phone = HallModel.userPhone;
        //pro.code = code;
        //pro.type = 2;
        //pro.codeType = 2;
        //string str = Util.GetMd5(pro.userid + pro.phone + AppConfig.webKey1 + pro.type + pro.codeType);
        //pro.sigin = Util.GetMd5(str + AppConfig.webKey2);
        //WebService.Instance.Send<Web_S_BindPhoneOp>(AppConfig.url_BindPhoneOp, pro, OnGetBindPhoneOpResult);

        //opCode = pro.type;
    }

    void OnBtnConfirmClick_BindAgency()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonNormal);
        Close();
        DoAction(GameEvent.V_OpenShortTip, "暂不支持");
    }

    void OnBtnConfirmClick_BindSpreader()
    {
        uint gameId = 0;
        uint.TryParse(inputAgencyAccount_bindSpreader.value, out gameId);
        if (gameId == 0)
        {
            DoAction(GameEvent.V_OpenShortTip, "推荐人ID错误！");
            return;
        }
        AudioManager.Instance.PlaySound(GameModel.audioButtonNormal);
        Close();
        HallService.Instance.SetSpreaderUser(gameId);
    }


    void OnBtnCloseClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonClose);
        Close();
        //DoActionDelay(HallEvent.V_OpenDlgUserInfo, 0.1f);
    }


    #endregion


    //收到绑定结果
    void OnGetBindPhoneOpResult(Web_S_BindPhoneOp pro)
    {
        string msg = "";
        if (pro == null)
        {
            switch (opCode)
            { 
                case 0:
                    msg = "请求验证码失败";
                    StopTimeCount();
                    break;
                case 1:
                    msg = "绑定手机失败";
                    break;
                case 2:
                    msg = "解绑手机失败";
                    break;
            }
        }
        else
        {
            if (pro.return_code == 10000)
            {
                switch (opCode)
                {
                    case 0:
                        msg = "请求验证码成功";
                        break;
                    case 1:
                        msg = "绑定手机成功";
                        break;
                    case 2:
                        msg = "解绑手机成功";
                        break;
                }
            }
            else
            {
                msg = pro.return_message;
                if (opCode == 0)
                {
                    StopTimeCount();
                }
            }
        }
        //绑定手机成功后，关闭界面
        if (pro != null && pro.return_code == 10000 && opCode == 1)
        {
            Close();
        }
        DoAction(GameEvent.V_OpenShortTip, msg);
        HallService.Instance.QueryUserInfo((uint)HallModel.userId);
    }


}
