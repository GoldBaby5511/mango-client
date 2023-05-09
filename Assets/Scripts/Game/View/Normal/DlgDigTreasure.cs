using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.UI;


public class DlgDigTreasure : View
{
    private Transform bg;
    private Transform shade;

    private UIButton btnClose;
    private UIButton btnDig;
    private UIButton[] btnRewardItem = new UIButton[8];
    private Dictionary<int, int> configIDBtnindexPairs = new Dictionary<int, int>();

    //奖励效果
    private GameObject pnlGetReward;
    private GameObject itemReward;
    private float timerReward = 2.0f;

    private float timerLuckDraw = 3.0f;
    private bool luckDraw = false;

    private int rewardConifgID = -1;         //中奖配置ID

    void Update()
    {
        //开始抽奖
        if(luckDraw == true)
        {
            timerLuckDraw -= Time.deltaTime;
            if(timerLuckDraw > 1.0)
            {
                RandShowDigItem();
            }
            else
            {
                if (timerLuckDraw > 0.5)
                {
                    for (int i = 0; i < btnRewardItem.Length; i++)
                    {
                        btnRewardItem[i].transform.Find("SpriteXZ").gameObject.SetActive(false);
                    }
                    int index = configIDBtnindexPairs[rewardConifgID];
                    btnRewardItem[index].transform.Find("SpriteXZ").gameObject.SetActive(true);
                }
                else
                {
                    luckDraw = false;
                    pnlGetReward.SetActive(true);
                }
            }
        }

        //是否显示
        if (pnlGetReward.activeSelf == true)
        {
            timerReward -= Time.deltaTime;
            if (timerReward <= 0)
            {
                pnlGetReward.SetActive(false);
                timerReward = 2.0f;
            }
        }
    }

    public override void Init()
    {
        bg = transform.Find("bg");
        shade = transform.Find("shade");
        btnClose = transform.Find("bg/btnClose").GetComponent<UIButton>();
        btnDig = transform.Find("bg/pnlReward/btnDig").GetComponent<UIButton>();
        for(int i = 0; i < btnRewardItem.Length; i ++)
        {
            btnRewardItem[i] = transform.Find("bg/pnlReward/btnReward_" + i).GetComponent<UIButton>();
        }

        //加载特效
        pnlGetReward = (GameObject)Instantiate(Resources.Load("Prefabs/PanelGetItem"));
        itemReward = pnlGetReward.transform.Find("PanelContent/PanelRewardList/ItemReward").gameObject;
        pnlGetReward.SetActive(false);
        pnlGetReward.transform.parent = transform.Find("bg");
        pnlGetReward.transform.localScale = Vector3.one;


        EventDelegate.Add(btnClose.onClick, OnBtnCloseClick);
        EventDelegate.Add(btnDig.onClick, OnBtnDigClick);

        gameObject.SetActive(false);
        bg.GetComponent<TweenAlpha>().ResetToBeginning();
    }

    public override void RegisterAction()
    {
        GameEvent.V_OpenDlgDigTreasure += Open;
    }

    public override void RemoveAction()
    {
        GameEvent.V_OpenDlgDigTreasure -= Open;
    }

    #region UI操作

    public void Open(CMD_CM_S_DigTreasure pro)
    {
        //常规打开判断
        if (pro == null)
        {
            gameObject.SetActive(true);
            bg.GetComponent<TweenScale>().PlayForward();
            bg.GetComponent<TweenAlpha>().PlayForward();
            shade.GetComponent<TweenAlpha>().PlayForward();

            //隐藏选中状态
            for (int i = 0; i < btnRewardItem.Length; i++)
            {
                btnRewardItem[i].transform.Find("SpriteXZ").gameObject.SetActive(false);
            }

            //构建列表
            for (int i = 0; i < HallModel.digConfig.Count; ++i)
            {
                configIDBtnindexPairs[HallModel.digConfig[i].nConfigID] = i;

                btnRewardItem[i].transform.Find("lblName").GetComponent<UILabel>().text = Util.Instance.GetNameByPropID(HallModel.digConfig[i].nPropID);
                btnRewardItem[i].transform.Find("lblCount").GetComponent<UILabel>().text = "x " + HallModel.digConfig[i].nPropCount.ToString();
                string propName = Util.Instance.GetNameByPropID(HallModel.digConfig[i].nPropID, HallModel.digConfig[i].nPropCount);
                btnRewardItem[i].transform.Find("Sprite").GetComponent<UISprite>().spriteName = propName;
                if (HallModel.digConfig[i].nPropID == 600) btnRewardItem[i].transform.Find("Sprite").GetComponent<UISprite>().MakePixelPerfect();
            }
        }
        else
        {
            //成功判断
            if(pro.bResult == false) { return; }

            rewardConifgID = pro.DigItem.nConfigID;

            //删除物体
            Transform transform = pnlGetReward.transform.Find("PanelContent/PanelRewardList");
            for (int i = transform.childCount - 1; i >= 1; i--)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
            itemReward.SetActive(false);

            GameObject obj = PoolManager.Instance.Spawn(itemReward);
            obj.name = itemReward.name + "_" + rewardConifgID;
            obj.transform.parent = itemReward.transform.parent;
            obj.transform.localScale = Vector3.one;
            obj.transform.Find("PanelContent/TextName").gameObject.SetActive(false);
            obj.transform.Find("PanelContent/Text").GetComponent<Text>().text = pro.DigItem.nPropCount.ToString();
            Image imageReward = obj.transform.Find("PanelContent/ImageIconBg/ImageIcon").GetComponent<Image>();
            if (pro.DigItem.nPropID == 600) imageReward.GetComponent<RectTransform>().sizeDelta = new Vector2(52f, 67f);
            string rewardName = Util.Instance.GetNameByPropID(pro.DigItem.nPropID, pro.DigItem.nPropCount);
            imageReward.sprite = Resources.Load("texture/icons/" + rewardName, typeof(Sprite)) as Sprite;
            obj.SetActive(true);

            pnlGetReward.SetActive(false);

            luckDraw = true;
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

    void OnBtnCloseClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonClose);
        Close();
    }

    void OnBtnDigClick()
    {
        //是否可挖判断
        if(HallModel.curPlayCount >= HallModel.totalPlayCount)
        {
            AudioManager.Instance.PlaySound(GameModel.audioButtonNormal);
            GameService.Instance.DigTreasure();
        }
        else
        {
            AudioManager.Instance.PlaySound(GameModel.audioTipWrong);
            string msg = "再玩 " + (HallModel.totalPlayCount - HallModel.curPlayCount) + " 局就可以挖宝啦!";
            DoAction(GameEvent.V_OpenShortTip, msg);
        }
    }

    void OnBtnDiamondsClick(GameObject obj)
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonNormal);
        int index = int.Parse(obj.name.Split('_')[1]);
        DoAction(GameEvent.V_OpenDlgPayMode, index);
    }

    void OnBtnCoinsClick(GameObject obj)
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonNormal);
        int index = int.Parse(obj.name.Split('_')[1]);
        DoAction(GameEvent.V_OpenDlgPayMode, index);
    }

    void RandShowDigItem()
    {
        for(int i = 0; i < btnRewardItem.Length; i ++)
        {
            btnRewardItem[i].transform.Find("SpriteXZ").gameObject.SetActive(false);
        }
        int index = UnityEngine.Random.Range(0, btnRewardItem.Length);
        btnRewardItem[index].transform.Find("SpriteXZ").gameObject.SetActive(true);
    }

}
