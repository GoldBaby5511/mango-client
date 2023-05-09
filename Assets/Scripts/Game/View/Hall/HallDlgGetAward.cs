using UnityEngine;
using System.Collections;

public class HallDlgGetAward : View 
{
    private Transform bg;
    private Transform shade;

    private Transform[] items = new Transform[3];
    private UILabel lblTip;

    private UIButton btnConfirm;

    public override void Init()
    {
        bg = transform.Find("bg");
        shade = transform.Find("shade");
        for (int i = 0; i < 3; i++)
        {
            items[i] = transform.Find("bg/item_" + i);
        }
        btnConfirm = transform.Find("bg/btnConfirm").GetComponent<UIButton>();
        lblTip = transform.Find("bg/lblTip").GetComponent<UILabel>();

        EventDelegate.Add(btnConfirm.onClick, OnBtnConfirmClick);

        gameObject.SetActive(false);
        bg.GetComponent<TweenScale>().ResetToBeginning();
        bg.GetComponent<TweenAlpha>().ResetToBeginning();
        shade.GetComponent<TweenAlpha>().ResetToBeginning();
    }

    public override void RegisterAction()
    {
        HallEvent.V_OpenDlgGetAward += Open;
    }

    public override void RemoveAction()
    {
        HallEvent.V_OpenDlgGetAward -= Open;
    }

    #region UI操作

    public void Open(float diamondCount, float cardCount, float redPackValue, int redPackCount)
    {
        AudioManager.Instance.PlaySound(GameModel.audioTipWarn);
        gameObject.SetActive(true);
        bg.GetComponent<TweenScale>().PlayForward();
        bg.GetComponent<TweenAlpha>().PlayForward();
        shade.GetComponent<TweenAlpha>().PlayForward();

        
        for (int i = 0; i < 3; i++)
        {
            items[i].gameObject.SetActive(false);
        }

        items[0].Find("Label").GetComponent<UILabel>().text = "钻石 x " + diamondCount;
        items[1].Find("Label").GetComponent<UILabel>().text = "房卡 x " + cardCount;
        items[2].Find("Label").GetComponent<UILabel>().text = "礼券 x " + redPackValue;
        if (redPackCount > 0 && redPackValue > 0)
        {
            lblTip.text = "恭喜你开启" + redPackCount + "个红包，获得" + redPackValue + "个礼券！";
        }
        else
        {
            lblTip.text = "";
        }

        int count = 0;
        if (diamondCount > 0)
        {
            count++;
            items[0].gameObject.SetActive(true);
        }
        if (cardCount > 0)
        {
            count++;
            items[1].gameObject.SetActive(true);
        }
        if (redPackValue > 0)
        {
            count++;
            items[2].gameObject.SetActive(true);
        }

        if (count == 1)
        {
            items[0].localPosition = new Vector3(0f, 35f, 0f);
            items[1].localPosition = new Vector3(0f, 35f, 0f);
            items[2].localPosition = new Vector3(0f, 35f, 0f);
        }
        else if (count == 2)
        {
            items[0].localPosition = new Vector3(-120f, 35f, 0f);
            items[2].localPosition = new Vector3(120f, 35f, 0f);
            if (diamondCount > 0)
            {
                items[1].localPosition = new Vector3(120f, 35f, 0f);
            }
            else
            {
                items[1].localPosition = new Vector3(-120f, 35f, 0f);
            }
        }
        else if (count == 3)
        {
            items[0].localPosition = new Vector3(-180f, 35f, 0f);
            items[1].localPosition = new Vector3(0f, 35f, 0f);
            items[2].localPosition = new Vector3(180f, 35f, 0f);
        }
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
        AudioManager.Instance.PlaySound(GameModel.audioButtonClose);
        Close();
    }


    #endregion
}
