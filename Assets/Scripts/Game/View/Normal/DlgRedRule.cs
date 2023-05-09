using UnityEngine;
using System.Collections;

public class DlgRedRule : View 
{
    private Transform bg;
    private Transform shade;

    private UIScrollView pnlRule;
    private UIButton btnClose;

    public override void Init()
    {
        bg = transform.Find("bg");
        shade = transform.Find("shade");

        pnlRule = transform.Find("bg/pnlRule").GetComponent<UIScrollView>();
        btnClose = transform.Find("bg/btnClose").GetComponent<UIButton>();

        EventDelegate.Add(btnClose.onClick, OnBtnCloseClick);

        gameObject.SetActive(false);
        bg.GetComponent<TweenScale>().ResetToBeginning();
        bg.GetComponent<TweenAlpha>().ResetToBeginning();
        shade.GetComponent<TweenAlpha>().ResetToBeginning();
    }

    public override void RegisterAction()
    {
        GameEvent.V_OpenDlgRedRule += Open;
    }

    public override void RemoveAction()
    {
        GameEvent.V_OpenDlgRedRule -= Open;
    }

    #region UI操作

    public void Open()
    {
        gameObject.SetActive(true);
        bg.GetComponent<TweenScale>().PlayForward();
        bg.GetComponent<TweenAlpha>().PlayForward();
        shade.GetComponent<TweenAlpha>().PlayForward();

        pnlRule.ResetPosition();
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

    void OnBtnCloseClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonClose);
        Close();
    }

    #endregion
}
