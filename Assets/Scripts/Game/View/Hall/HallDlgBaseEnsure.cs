using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.IO;
using System;

public class HallDlgBaseEnsure : View
{
    private GameObject bg;
    private GameObject shade;
    private UIButton btnGetBaseEnsure;
    private UIButton btnConfirm;
    private UIButton btnClose;
    private UILabel lblMainTip;
    private UILabel lblSubTip;

    private UnityAction callback;

    public override void Init()
    {
        bg = transform.Find("bg").gameObject;
        shade = transform.Find("shade").gameObject;
        btnGetBaseEnsure = transform.Find("bg/btnGetBaseEnsure").GetComponent<UIButton>();
        btnConfirm = transform.Find("bg/btnConfirm").GetComponent<UIButton>();
        btnClose = transform.Find("bg/btnClose").GetComponent<UIButton>();
        lblMainTip = transform.Find("bg/lblMainTip").GetComponent<UILabel>();
        lblSubTip = transform.Find("bg/lblSubTip").GetComponent<UILabel>();

        //添加监听
        EventDelegate.Add(btnGetBaseEnsure.onClick, OnBtnGetBaseEnsureClick);
        EventDelegate.Add(btnConfirm.onClick, OnBtnConfirmClick);
        EventDelegate.Add(btnClose.onClick, OnBtnCloseClick);
        //初始化
        gameObject.SetActive(false);
        bg.GetComponent<TweenScale>().ResetToBeginning();
        bg.GetComponent<TweenAlpha>().ResetToBeginning();
        shade.GetComponent<TweenAlpha>().ResetToBeginning();
    }

    public override void RegisterAction()
    {
        GameEvent.V_OpenDlgBaseEnsure += OpenDlgTip;
    }

    public override void RemoveAction()
    {
        GameEvent.V_OpenDlgBaseEnsure -= OpenDlgTip;
    }

    void OpenDlgTip(string mainMsg, string subMsg, UnityAction callBack)
    {
        gameObject.SetActive(true);
        bg.GetComponent<TweenAlpha>().PlayForward();
        bg.GetComponent<TweenScale>().PlayForward();
        shade.GetComponent<TweenAlpha>().PlayForward();
        lblMainTip.text = mainMsg;
        lblSubTip.text = subMsg;
        callback = callBack;
        if (HallModel.currentCoinBaseEnsureTimes < HallModel.totalCoinBaseEnsureTimes)
        {
            btnConfirm.gameObject.SetActive(true);
            btnGetBaseEnsure.gameObject.SetActive(true);
            btnConfirm.transform.localPosition = new Vector3(-130f, -135f, 0f);
            btnGetBaseEnsure.transform.localPosition = new Vector3(130f, -135f, 0f);
        }
        else
        {
            btnConfirm.gameObject.SetActive(true);
            btnGetBaseEnsure.gameObject.SetActive(false);
            btnConfirm.transform.localPosition = new Vector3(0f, -135f, 0f);
        }
    }

    void CloseDlgTip()
    {
        bg.GetComponent<TweenAlpha>().PlayReverse();
        bg.GetComponent<TweenScale>().PlayReverse();
        shade.GetComponent<TweenAlpha>().PlayReverse();
        Invoke("CloseDlgTipCor", 1f);
    }

    void CloseDlgTipCor()
    {
        gameObject.SetActive(false);
    }


    //领取低保
    void OnBtnGetBaseEnsureClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonNormal);
        CloseDlgTip();
        if (callback != null)
        {
            callback.Invoke();
            callback = null;
        }
        StartCoroutine(WeiXinShare(1));
    }

    void OnBtnConfirmClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonNormal);
        CloseDlgTip();
        DoActionDelay(HallEvent.V_OpenDlgStore, 0.3f, DlgStoreArg.DiamondPage);
    }

    void OnBtnCloseClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonClose);
        CloseDlgTip();
    }




    IEnumerator WeiXinShare(int type)
    {
        //1.获取分享图片下载地址
        WWW www01 = new WWW(AppConfig.weixinShareTextureUrl);
        yield return www01;
        if (www01.error == null)
        {
            string textureUrl = www01.text;
            yield return new WaitForEndOfFrame();
            //2.下载分享图片
            WWW www02 = new WWW(textureUrl);
            yield return www02;
            if (www02.error == null)
            {
                //3.保存分享图片到本地
                string savePath = Application.persistentDataPath + "/wxShare_" + HallModel.gameId + ".jpg";

                if (File.Exists(savePath))
                {
                    File.Delete(savePath);
                }
                yield return new WaitForEndOfFrame();
                try
                {
                    File.WriteAllBytes(savePath, www02.bytes);
                }
                catch (Exception e)
                {
                    Debug.LogError("保存图片异常 ： " + e.ToString());
                }
                yield return new WaitForSeconds(0.1f);
                //4.分享到朋友圈
                PluginManager.Instance.WxShareImage(type, savePath, callback);
            }
            else
            {
                Debug.LogError("下载分享图片错误 ： " + www02.error);
            }
        }
        else
        {
            Debug.LogError(www01.error);
        }
    }

}
