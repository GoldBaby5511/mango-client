using UnityEngine;
using System.Collections;

public class HallDlgRule : View
{

    private Transform bg;
    private Transform shade;

    private UIButton btnClose;

    private GameObject rule;


    public override void Init()
    {
        bg = transform.Find("bg");
        shade = transform.Find("shade");

        btnClose = transform.Find("bg/Panel/btnClose").GetComponent<UIButton>();

        rule = transform.Find("bg/rule").gameObject;

        EventDelegate.Add(btnClose.onClick, OnBtnCloseClick);

        gameObject.SetActive(false);
        bg.GetComponent<TweenScale>().ResetToBeginning();
        bg.GetComponent<TweenAlpha>().ResetToBeginning();
        shade.GetComponent<TweenAlpha>().ResetToBeginning();
    }

    public override void RegisterAction()
    {
        HallEvent.V_OpenDlgRule += Open;
    }

    public override void RemoveAction()
    {
        HallEvent.V_OpenDlgRule -= Open;
    }

    void Open()
    {
        gameObject.SetActive(true);
        bg.GetComponent<TweenScale>().PlayForward();
        bg.GetComponent<TweenAlpha>().PlayForward();
        shade.GetComponent<TweenAlpha>().PlayForward();

        rule.GetComponent<UIScrollView>().ResetPosition();
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


    void OnBtnCloseClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonClose);
        Close();
    }

}
