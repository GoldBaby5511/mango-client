using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class HallDlgRank : View
{
    private UIButton btnOpen;
    private UIButton btnClose;

    private GameObject bg;

    private GameObject menuRedPack;
    private GameObject menuCoin;

    private UIButton btnRedPack;
    private UIButton btnCoin;
    
    private UIButton btnRank_1;
    private UIButton btnRank_2;
    private UIButton btnRank_3;
    private UIButton btnRank_4;
    private UIButton btnRank_5;

    private GameObject itemRank;
    private Transform panel;

    private byte rankType = 1;
    private List<Transform> itemList = new List<Transform>(); 

    public override void Init()
    {
        btnOpen = transform.Find("btnOpen").GetComponent<UIButton>();
        btnClose = transform.Find("bg/btnClose").GetComponent<UIButton>();

        bg = transform.Find("bg").gameObject;
        menuRedPack = transform.Find("bg/menuRedPack").gameObject;
        menuCoin = transform.Find("bg/menuCoin").gameObject;
        btnRedPack = transform.Find("bg/leftMenu/btnRedPack").GetComponent<UIButton>();
        btnCoin = transform.Find("bg/leftMenu/btnCoin").GetComponent<UIButton>();
        btnRank_1 = transform.Find("bg/menuRedPack/btnRank_1").GetComponent<UIButton>();
        btnRank_2 = transform.Find("bg/menuRedPack/btnRank_2").GetComponent<UIButton>();
        btnRank_3 = transform.Find("bg/menuRedPack/btnRank_3").GetComponent<UIButton>();
        btnRank_4 = transform.Find("bg/menuCoin/btnRank_4").GetComponent<UIButton>();
        btnRank_5 = transform.Find("bg/menuCoin/btnRank_5").GetComponent<UIButton>();


        itemRank = transform.Find("bg/Panel/itemRank").gameObject;
        panel = transform.Find("bg/Panel");

        EventDelegate.Add(btnOpen.onClick, OnBtnOpenClick);
        EventDelegate.Add(btnClose.onClick, OnBtnCloseClick);
        EventDelegate.Add(btnCoin.onClick, OnBtnCoinClick);
        EventDelegate.Add(btnRedPack.onClick, OnBtnRedPackClick);
        EventDelegate.Add(btnRank_1.onClick, OnBtnRankClick_1);
        EventDelegate.Add(btnRank_2.onClick, OnBtnRankClick_2);
        EventDelegate.Add(btnRank_3.onClick, OnBtnRankClick_3);
        EventDelegate.Add(btnRank_4.onClick, OnBtnRankClick_4);
        EventDelegate.Add(btnRank_5.onClick, OnBtnRankClick_5);

        InitPage();
    }

    public override void RegisterAction()
    {
        HallEvent.V_OpenDlgRank += OpenRank;
        HallEvent.V_CloseDlgRank += CloseRank;
    }

    public override void RemoveAction()
    {
        HallEvent.V_OpenDlgRank -= OpenRank;
        HallEvent.V_CloseDlgRank -= CloseRank;
    }

    void InitPage()
    {
        gameObject.SetActive(false);
        itemRank.SetActive(false);
        btnOpen.gameObject.SetActive(true);
        bg.SetActive(false);
        bg.transform.localPosition = new Vector3(-AppConfig.screenWidth/2 - 710f, 0f, 0f);
        itemList.Clear();
        for (int i = 0; i < 20; i++)
        {
            GameObject obj = PoolManager.Instance.Spawn(itemRank);
            obj.name = itemRank.name + "_" + i;
            obj.transform.parent = itemRank.transform.parent;
            obj.transform.localScale = Vector3.one;
            obj.transform.localPosition = new Vector3(0f, 215f - 85f * i, 0f);
            obj.transform.Find("lblRank").gameObject.SetActive(i >= 3);
            obj.transform.Find("sptRank").gameObject.SetActive(i < 3);
            obj.transform.Find("lblRank").GetComponent<UILabel>().text = (i + 1).ToString();
            obj.transform.Find("sptRank").GetComponent<UISprite>().spriteName = "flag_" + i;
            itemList.Add(obj.transform);
        }
    }

    void OpenRank()
    {
        gameObject.SetActive(true);
        btnOpen.gameObject.SetActive(true);
        bg.SetActive(false);
        //刷新按钮头像
        Invoke("RefreshButtonPhoto", 3f);
        Invoke("RefreshButtonPhoto", 10f);
    }

    void CloseRank()
    {
        gameObject.SetActive(false);
        if (bg.transform.localPosition.x > -700)
        {
            bg.transform.localPosition = new Vector3(-AppConfig.screenWidth / 2 - 710f, 0f, 0f);
        }
    }

    void Open()
    {
        btnOpen.gameObject.SetActive(false);
        bg.SetActive(true);
        bg.transform.DOLocalMoveX(-AppConfig.screenWidth/2, 0.3f);
        //打开时，默认为红包榜
        btnRedPack.isEnabled = false;
        btnCoin.isEnabled = true;
        menuRedPack.SetActive(true);
        menuCoin.SetActive(false);
        rankType = 1;
        RefreshPanelInfo();
    }

    void Close()
    {
        bg.transform.DOLocalMoveX(-AppConfig.screenWidth / 2 - 710f, 0.3f);
        DoActionDelay(CloseCor, 0.3f);
    }

    void CloseCor()
    {
        btnOpen.gameObject.SetActive(true);
        RefreshButtonPhoto();
        bg.SetActive(false);
    }

    //刷新按钮中头像
    void RefreshButtonPhoto()
    {
        if (HallModel.rankDic.ContainsKey(1))
        {
            for (int i = 0; i < 3; i++)
            {
                int userId = HallModel.rankDic[1].dwUserID[i];
                if (HallModel.userPhotos.ContainsKey(userId))
                {
                    btnOpen.transform.Find("photo_" + i).GetComponent<UITexture>().mainTexture = HallModel.userPhotos[userId];
                }
                else
                {
                    btnOpen.transform.Find("photo_" + i).GetComponent<UITexture>().mainTexture = HallModel.defaultPhoto;
                }
            }
        }
    }

    //刷新面板信息
    void RefreshPanelInfo()
    {
        CMD_Hall_S_RankInfo pro = null;
        HallModel.rankDic.TryGetValue(rankType, out pro);

        if (pro != null)
        {
            btnRank_1.isEnabled = true;
            btnRank_2.isEnabled = true;
            btnRank_3.isEnabled = true;
            btnRank_4.isEnabled = true;
            btnRank_5.isEnabled = true;
            switch(rankType)
            {
                case 1:
                    btnRank_1.isEnabled = false;
                    break;
                case 2:
                    btnRank_2.isEnabled = false;
                    break;
                case 3:
                    btnRank_3.isEnabled = false;
                    break;
                case 4:
                    btnRank_4.isEnabled = false;
                    break;
                case 5:
                    btnRank_5.isEnabled = false;
                    break;
            }
            for (int i = 0; i < 20; i++)
            {
                //显示玩家信息
                itemList[i].Find("lblUserName").GetComponent<UILabel>().text = pro.szNickName[i];
                itemList[i].Find("lblGameId").GetComponent<UILabel>().text = "ID:" + pro.dwGameID[i];
                if (rankType <= 3)
                {
                    itemList[i].Find("lblCount").GetComponent<UILabel>().text = pro.lCount[i].ToString();
                }
                else if(rankType <= 5)
                {
                    itemList[i].Find("lblCount").GetComponent<UILabel>().text = Util.GetCoinNumStr(pro.lCount[i]);
                }
                //显示头像
                if (HallModel.userPhotos.ContainsKey(pro.dwUserID[i]))
                {
                    itemList[i].Find("photo").GetComponent<UITexture>().mainTexture = HallModel.userPhotos[pro.dwUserID[i]];
                }
                else
                {
                    itemList[i].Find("photo").GetComponent<UITexture>().mainTexture = HallModel.defaultPhoto;
                }
            }
        }
    }



    


    void OnBtnOpenClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonNormal);
        Open();
    }

    void OnBtnCloseClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonNormal);
        Close();
    }


    //红包榜
    void OnBtnRedPackClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonOp);
        btnRedPack.isEnabled = false;
        btnCoin.isEnabled = true;
        menuRedPack.SetActive(true);
        menuCoin.SetActive(false);
        rankType = 1;
        RefreshPanelInfo();
    }

    //金币榜
    void OnBtnCoinClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonOp);
        btnRedPack.isEnabled = true;
        btnCoin.isEnabled = false;
        menuRedPack.SetActive(false);
        menuCoin.SetActive(true);
        rankType = 4;
        RefreshPanelInfo();
    }



    void OnBtnRankClick_1()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonSwitch);
        rankType = 1;
        RefreshPanelInfo();
    }

    void OnBtnRankClick_2()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonSwitch);
        rankType = 2;
        RefreshPanelInfo();
    }

    void OnBtnRankClick_3()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonSwitch);
        rankType = 3;
        RefreshPanelInfo();
    }

    void OnBtnRankClick_4()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonSwitch);
        rankType = 4;
        RefreshPanelInfo();
    }

    void OnBtnRankClick_5()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonSwitch);
        rankType = 5;
        RefreshPanelInfo();
    }

}
