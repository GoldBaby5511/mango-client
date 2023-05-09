using UnityEngine;
using System.Collections;

public class HallDlgRealAuth : View
{
    private Transform bg;
    private Transform shade;
    private UIInput inputName;
    private UIInput inputId;
    private UIButton btnConfirm;
    private UIButton btnClose;

    public override void Init()
    {
        bg = transform.Find("bg");
        shade = transform.Find("shade");
        inputName = transform.Find("bg/inputName").GetComponent<UIInput>();
        inputId = transform.Find("bg/inputId").GetComponent<UIInput>();
        btnConfirm = transform.Find("bg/btnConfirm").GetComponent<UIButton>();
        btnClose = transform.Find("bg/btnClose").GetComponent<UIButton>();

        EventDelegate.Add(btnConfirm.onClick, OnBtnConfirmClick);
        EventDelegate.Add(btnClose.onClick, OnBtnCloseClick);

        gameObject.SetActive(false);
        bg.GetComponent<TweenScale>().ResetToBeginning();
        bg.GetComponent<TweenAlpha>().ResetToBeginning();
        shade.GetComponent<TweenAlpha>().ResetToBeginning();
    }

    public override void RegisterAction()
    {
        HallEvent.V_OpenDlgRealAuth += Open;
    }

    public override void RemoveAction()
    {
        HallEvent.V_OpenDlgRealAuth -= Open;
    }

    #region UI操作

    public void Open()
    {
        gameObject.SetActive(true);
        bg.GetComponent<TweenScale>().PlayForward();
        bg.GetComponent<TweenAlpha>().PlayForward();
        shade.GetComponent<TweenAlpha>().PlayForward();

        inputName.value = "";
        inputId.value = "";
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
        string id = inputId.value;
        string name = inputName.value;
        if (name.Length == 0)
        {
            AudioManager.Instance.PlaySound(GameModel.audioTipWarn);
            DoAction(GameEvent.V_OpenShortTip, "姓名不能为空！");
            return;
        }
        if (!Util.CheckIdCard(id))
        {
            AudioManager.Instance.PlaySound(GameModel.audioTipWarn);
            DoAction(GameEvent.V_OpenShortTip, "请输入正确的身份证号！");
            return;
        }
        AudioManager.Instance.PlaySound(GameModel.audioButtonNormal);
        HallService.Instance.RealAuth(name, id);
        Close();
    }

    void OnBtnCloseClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonClose);
        Close();
    }

    #endregion
}
