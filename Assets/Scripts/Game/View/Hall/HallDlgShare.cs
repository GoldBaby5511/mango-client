using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

public class HallDlgShare : View
{
    private GameObject bg;
    private GameObject shade;

    private UIButton btnShareFriend;
    private UIButton btnShareCircle;
    private UIButton btnClose;

    public override void Init()
    {
        bg = transform.Find("bg").gameObject;
        shade = transform.Find("shade").gameObject;

        btnShareFriend = transform.Find("bg/btnShareFriend").GetComponent<UIButton>();
        btnShareCircle = transform.Find("bg/btnShareCircle").GetComponent<UIButton>();
        btnClose = transform.Find("bg/btnClose").GetComponent<UIButton>();

        //添加监听
        EventDelegate.Add(btnShareFriend.onClick, OnBtnShareFriendClick);
        EventDelegate.Add(btnShareCircle.onClick, OnBtnShareCircleClick);
        EventDelegate.Add(btnClose.onClick, OnBtnCloseClick);
        //初始化
        gameObject.SetActive(false);
        bg.GetComponent<TweenScale>().ResetToBeginning();
        bg.GetComponent<TweenAlpha>().ResetToBeginning();
        shade.GetComponent<TweenAlpha>().ResetToBeginning();
    }

    public override void RegisterAction()
    {
        HallEvent.V_OpenDlgShare += Open;
    }

    public override void RemoveAction()
    {
        HallEvent.V_OpenDlgShare -= Open;
    }

    void Open()
    {
        gameObject.SetActive(true);
        bg.GetComponent<TweenAlpha>().PlayForward();
        bg.GetComponent<TweenScale>().PlayForward();
        shade.GetComponent<TweenAlpha>().PlayForward();
    }

    void Close()
    {
        bg.GetComponent<TweenAlpha>().PlayReverse();
        bg.GetComponent<TweenScale>().PlayReverse();
        shade.GetComponent<TweenAlpha>().PlayReverse();
        Invoke("CloseCor", 0.4f);
    }

    void CloseCor()
    {
        gameObject.SetActive(false);
    }


    //分享好友
    void OnBtnShareFriendClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonOp);
        StartCoroutine(WeiXinShare(0, null));
    }

    //分享朋友圈
    void OnBtnShareCircleClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonOp);
        StartCoroutine(WeiXinShare(1, HallService.Instance.ShareSuccess));
    }

    //关闭
    void OnBtnCloseClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonClose);
        Close();
    }

    IEnumerator WeiXinShare(int type, UnityAction callback)
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
