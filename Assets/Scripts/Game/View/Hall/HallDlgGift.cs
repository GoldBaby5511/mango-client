using UnityEngine;
using System.Collections;
using System.IO;
using System;
using UnityEngine.Events;

public class HallDlgGift : View
{
    private GameObject gift;
    private GameObject monthCardBuy;
    private GameObject monthCardGetOld;
    private GameObject monthCardGetNew;
    private Transform shade;

    private UIButton btnShare_gift;
    private UIButton btnBuy_gift;
    private UIButton btnClose_gift;
    private UIButton btnGet_gift;

    private UIButton btnBuy_monthCardBuy;
    private UIButton btnClose_monthCardBuy;
    

    private UIButton btnGet_monthCardGetOld;
    private UIButton btnClose_monthCardGetOld;
    private UILabel lblLastDay_monthCardGetOld;

    private UIButton btnGet_monthCardGetNew;
    private UIButton btnClose_monthCardGetNew;
    private UILabel lblLastDay_monthCardGetNew;

    private int openType = 0;//0无月卡、1老月卡、2新月卡、4大厅内打开关闭时不弹分享

    public override void Init()
    {
        gift = transform.Find("gift").gameObject;
        monthCardBuy = transform.Find("monthCardBuy").gameObject;
        monthCardGetOld = transform.Find("monthCardGetOld").gameObject;
        monthCardGetNew = transform.Find("monthCardGetNew").gameObject;
        shade = transform.Find("shade");

        btnClose_gift = transform.Find("gift/pnlClose/btnClose").GetComponent<UIButton>();
        btnShare_gift = transform.Find("gift/Panel/gift_0/btnShare").GetComponent<UIButton>();
        btnBuy_gift = transform.Find("gift/Panel/gift_1/btnBuy").GetComponent<UIButton>();
        btnGet_gift = transform.Find("gift/Panel/gift_1/btnGet").GetComponent<UIButton>();

        btnBuy_monthCardBuy = transform.Find("monthCardBuy/btnBuy").GetComponent<UIButton>();
        btnClose_monthCardBuy = transform.Find("monthCardBuy/btnClose").GetComponent<UIButton>();

        btnGet_monthCardGetOld = transform.Find("monthCardGetOld/btnGet").GetComponent<UIButton>();
        btnClose_monthCardGetOld = transform.Find("monthCardGetOld/btnClose").GetComponent<UIButton>();
        lblLastDay_monthCardGetOld = transform.Find("monthCardGetOld/lblLastDay").GetComponent<UILabel>();

        btnGet_monthCardGetNew = transform.Find("monthCardGetNew/btnGet").GetComponent<UIButton>();
        btnClose_monthCardGetNew = transform.Find("monthCardGetNew/btnClose").GetComponent<UIButton>();
        lblLastDay_monthCardGetNew = transform.Find("monthCardGetNew/lblLastDay").GetComponent<UILabel>();

        EventDelegate.Add(btnShare_gift.onClick, OnBtnShareClick);
        EventDelegate.Add(btnBuy_gift.onClick, OnBtnBuyClick);
        EventDelegate.Add(btnClose_gift.onClick, OnBtnCloseClickGift);
        EventDelegate.Add(btnGet_gift.onClick, OnBtnGetClick);

        EventDelegate.Add(btnBuy_monthCardBuy.onClick, OnBtnBuyClick);
        EventDelegate.Add(btnClose_monthCardBuy.onClick, OnBtnCloseClickMonthCard);

        EventDelegate.Add(btnGet_monthCardGetOld.onClick, OnBtnGetClick);
        EventDelegate.Add(btnClose_monthCardGetOld.onClick, OnBtnCloseClickMonthCard);

        EventDelegate.Add(btnGet_monthCardGetNew.onClick, OnBtnGetClick);
        EventDelegate.Add(btnClose_monthCardGetNew.onClick, OnBtnCloseClickMonthCard);

        gameObject.SetActive(false);
        gift.SetActive(false);
        monthCardBuy.SetActive(false);
        monthCardGetOld.SetActive(false);
        monthCardGetNew.SetActive(false);
        gift.GetComponent<TweenScale>().ResetToBeginning();
        gift.GetComponent<TweenAlpha>().ResetToBeginning();
        shade.GetComponent<TweenAlpha>().ResetToBeginning();

        monthCardBuy.GetComponent<TweenScale>().ResetToBeginning();
        monthCardBuy.GetComponent<TweenAlpha>().ResetToBeginning();
        monthCardGetOld.GetComponent<TweenScale>().ResetToBeginning();
        monthCardGetOld.GetComponent<TweenAlpha>().ResetToBeginning();
        monthCardGetNew.GetComponent<TweenScale>().ResetToBeginning();
        monthCardGetNew.GetComponent<TweenAlpha>().ResetToBeginning();
    }

    public override void RegisterAction()
    {
        HallEvent.V_OpenDlgGift += OpenGift;
        HallEvent.V_OpenDlgMonthCard += OpenMonthCard;
        HallEvent.V_CloseDlgMonthCard += Close;
    }

    public override void RemoveAction()
    {
        HallEvent.V_OpenDlgGift -= OpenGift;
        HallEvent.V_OpenDlgMonthCard -= OpenMonthCard;
        HallEvent.V_CloseDlgMonthCard -= Close;
    }

    void OpenGift()
    {
        gameObject.SetActive(true);
        gift.SetActive(true);
        monthCardBuy.SetActive(false);
        monthCardGetOld.SetActive(false);
        monthCardGetNew.SetActive(false);
        gift.GetComponent<TweenScale>().PlayForward();
        gift.GetComponent<TweenAlpha>().PlayForward();
        shade.GetComponent<TweenAlpha>().PlayForward();

        btnBuy_gift.gameObject.SetActive(!HallModel.isBuyMonthCard);
        btnGet_gift.gameObject.SetActive(HallModel.isBuyMonthCard);
        btnGet_gift.isEnabled = !HallModel.isGetMonthCardGift;

        gift.transform.Find("Panel/gift_1/lblDayInfo").GetComponent<UILabel>().text = HallModel.isBuyMonthCard ? "月卡（剩余" + HallModel.lastMonthCardDay + "天）" : "月卡福利";
        
    }

    void OpenMonthCard(int type)
    {
        openType = type;

        //偷懒，月卡购买时所得钻石、金币数存在充值配置表，每次使用所得存在道具表
        //月卡描述信息
        foreach (GoodInfo info in AppConfig.goodDic_android.Values)
        {
            if (info.Type != 4) continue;
            //月卡描述信息
            btnBuy_monthCardBuy.transform.Find("Label").GetComponent<UILabel>().text = info.RechargePrice.ToString();
            monthCardBuy.transform.Find("Label0").GetComponent<UILabel>().text = "金币*"+info.GiveCount;
            monthCardBuy.transform.Find("Label1").GetComponent<UILabel>().text = "钻石*"+info.Count;
        }

        //月卡ID 620
        ExchangeInfo yueCard = null;
        foreach(ExchangeInfo info in AppConfig.exchangeDic.Values)
        {
            if(info.ID != 620) continue;
            
            yueCard = info;
            break;
        }
        if (yueCard == null) return;

        //描述信息
        monthCardBuy.transform.Find("Label").GetComponent<UILabel>().text = yueCard.RegulationsInfo;
        monthCardBuy.transform.Find("Label2").GetComponent<UILabel>().text = "金币*"+yueCard.UseResultsGold;
        monthCardBuy.transform.Find("Label3").GetComponent<UILabel>().text = "钻石*"+yueCard.UseResultsIngot;
        monthCardBuy.transform.Find("Label4").GetComponent<UILabel>().text = "礼券*"+yueCard.UseResultsCash;

        gameObject.SetActive(true);
        gift.SetActive(false);
        monthCardBuy.SetActive(false);
        monthCardGetOld.SetActive(false);
        monthCardGetNew.SetActive(false);
        if (!HallModel.isBuyMonthCard)
        {
            monthCardBuy.SetActive(true);
            monthCardBuy.GetComponent<TweenScale>().PlayForward();
            monthCardBuy.GetComponent<TweenAlpha>().PlayForward();
        }
        else
        {
            if (type == 1)
            {
                monthCardGetOld.SetActive(true);
                monthCardGetOld.GetComponent<TweenScale>().PlayForward();
                monthCardGetOld.GetComponent<TweenAlpha>().PlayForward();

                btnGet_monthCardGetOld.isEnabled = !HallModel.isGetMonthCardGift;
                lblLastDay_monthCardGetOld.text = "剩余" + HallModel.lastMonthCardDay + "天";
            }
            else
            {
                //描述信息
                monthCardGetNew.transform.Find("Label").GetComponent<UILabel>().text = yueCard.RegulationsInfo;
                monthCardGetNew.transform.Find("Label0").GetComponent<UILabel>().text = "金币*"+yueCard.UseResultsGold;
                monthCardGetNew.transform.Find("Label1").GetComponent<UILabel>().text = "钻石*"+yueCard.UseResultsIngot;
                monthCardGetNew.transform.Find("Label2").GetComponent<UILabel>().text = "礼券*"+yueCard.UseResultsCash;

                monthCardGetNew.SetActive(true);
                monthCardGetNew.GetComponent<TweenScale>().PlayForward();
                monthCardGetNew.GetComponent<TweenAlpha>().PlayForward();

                btnGet_monthCardGetNew.isEnabled = !HallModel.isGetMonthCardGift;
                lblLastDay_monthCardGetNew.text = "剩余" + HallModel.lastMonthCardDay + "天";
            }
        }
        shade.GetComponent<TweenAlpha>().PlayForward();
    }

    void Close()
    {
        if (monthCardBuy.gameObject.activeSelf)
        {
            monthCardBuy.GetComponent<TweenScale>().PlayReverse();
            monthCardBuy.GetComponent<TweenAlpha>().PlayReverse();
        }
        if (monthCardGetOld.gameObject.activeSelf)
        {
            monthCardGetOld.GetComponent<TweenScale>().PlayReverse();
            monthCardGetOld.GetComponent<TweenAlpha>().PlayReverse();
        }
        if (monthCardGetNew.gameObject.activeSelf)
        {
            monthCardGetNew.GetComponent<TweenScale>().PlayReverse();
            monthCardGetNew.GetComponent<TweenAlpha>().PlayReverse();
        }
        if (gift.gameObject.activeSelf)
        {
            gift.GetComponent<TweenScale>().PlayReverse();
            gift.GetComponent<TweenAlpha>().PlayReverse();
        }
        shade.GetComponent<TweenAlpha>().PlayReverse();
        DoActionDelay(CloseCor, 0.4f);
    }

    void CloseCor()
    {
        gameObject.SetActive(false);
        gift.SetActive(false);
        monthCardBuy.SetActive(false);
        monthCardGetOld.SetActive(false);
        monthCardGetNew.SetActive(false);
    }


    //分享
    void OnBtnShareClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonOp);
        StartCoroutine(WeiXinShare(1, HallService.Instance.ShareSuccess));
    }

    //购买月卡
    void OnBtnBuyClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonNormal);
        int goodId = -1;
        foreach (GoodInfo info in AppConfig.goodDic_android.Values)
        {
            if (info.Type == 4)
            {
                goodId = info.RechargeID;
                break;
            }
        }
        if (goodId < 0)
        {
            DoAction(GameEvent.V_OpenShortTip, "没有商品信息！");
            return;
        }
        DoAction(GameEvent.V_OpenDlgPayMode, goodId);
    }

    //领取月卡福利
    void OnBtnGetClick()
    {
        if (HallModel.isGetMonthCardGift)
        {
            AudioManager.Instance.PlaySound(GameModel.audioTipWarn);
            DoAction(GameEvent.V_OpenShortTip, "今日已领取！");
            return;
        }
        AudioManager.Instance.PlaySound(GameModel.audioButtonOp);
        GetGift();
    }

    //关闭福利面板
    void OnBtnCloseClickGift()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonClose);
        Close();
    }

    //关闭月卡领取面板
    void OnBtnCloseClickMonthCard()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonClose);
        Close();
        
        //弹分享
        if((openType & 4) == 0 && HallEvent.V_OpenDlgShareTask != null)
        {
            Util.Instance.DoActionDelay(HallEvent.V_OpenDlgShareTask, 0.2f);
        }
        // if (HallModel.isFirstCharge)
        // {
        //     DoActionDelay(HallEvent.V_OpenDlgActivity, 0.2f);
        // }
        // else
        // {
        //     DoActionDelay(HallEvent.V_OpenDlgFirstCharge, 0.2f);
        // }
    }



    //领取月卡福利
    void GetGift()
    {
        int exchangeId = 0;
        foreach (ExchangeInfo info in AppConfig.exchangeDic.Values)
        {
            if (info.Kind == 8)
            {
                exchangeId = info.ID;
                break;
            }
        }
        
        //if (exchangeId != 0)
        //{
        //    Web_C_Exchange pro = new Web_C_Exchange();
        //    pro.userId = HallModel.userId;
        //    pro.goodsId = exchangeId;
        //    WebService.Instance.Send<Web_S_Exchange>(AppConfig.url_Exchange, pro, OnGetGiftResult);
        //}
    }

    //领取结果
    void OnGetGiftResult(Web_S_Exchange pro)
    {
        if (pro == null)
        {
            DoAction(GameEvent.V_OpenShortTip, "领取失败！");
        }
        else if (pro.return_code != 10000)
        {
            DoAction(GameEvent.V_OpenShortTip, pro.return_message);
        }
        else
        {
            HallModel.isGetMonthCardGift = true;
            HallModel.lastMonthCardDay--;
            DoAction(GameEvent.V_OpenShortTip, "领取成功！");
            HallService.Instance.QueryBankInfo();

            btnGet_gift.isEnabled = false;
            btnGet_monthCardGetOld.isEnabled = false;
            btnGet_monthCardGetNew.isEnabled = false;
        }
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
