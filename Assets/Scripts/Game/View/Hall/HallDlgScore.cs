using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HallDlgScore : View 
{
    private Transform bg;
    private Transform shade;
    private GameObject loadTip;

    private UIButton btnClose;

    private UIButton btnGameLandlords;

    private Transform pnlScore;

    private GameObject itemRecord4;
    private GameObject itemRecord6;
    private GameObject itemRecord8;

    private List<GameObject> itemList = new List<GameObject>();

    public override void Init()
    {
        bg = transform.Find("bg");
        shade = transform.Find("shade");
        loadTip = transform.Find("bg/loadTip").gameObject;

        btnClose = transform.Find("bg/Panel/btnClose").GetComponent<UIButton>();
        btnGameLandlords = transform.Find("bg/menu/Panel/btnGameLandlords").GetComponent<UIButton>();

        pnlScore = transform.Find("bg/pnlScore");
        itemRecord4 = transform.Find("bg/pnlScore/itemRecord4").gameObject;
        itemRecord6 = transform.Find("bg/pnlScore/itemRecord6").gameObject;
        itemRecord8 = transform.Find("bg/pnlScore/itemRecord8").gameObject;

        EventDelegate.Add(btnClose.onClick, OnBtnCloseClick);
        EventDelegate.Add(btnGameLandlords.onClick, OnBtnGameLandloardsClick);

        gameObject.SetActive(false);
        itemRecord4.SetActive(false);
        itemRecord6.SetActive(false);
        itemRecord8.SetActive(false);
        bg.GetComponent<TweenScale>().ResetToBeginning();
        bg.GetComponent<TweenAlpha>().ResetToBeginning();
        shade.GetComponent<TweenAlpha>().ResetToBeginning();
    }

    public override void RegisterAction()
    {
        HallEvent.V_OpenDlgRecord += Open;
        HallEvent.V_RefreshDlgRecord += RefreshPanel;
    }

    public override void RemoveAction()
    {
        HallEvent.V_OpenDlgRecord -= Open;
        HallEvent.V_RefreshDlgRecord -= RefreshPanel;
    }

    void Open()
    {
        gameObject.SetActive(true);
        bg.GetComponent<TweenScale>().PlayForward();
        bg.GetComponent<TweenAlpha>().PlayForward();
        shade.GetComponent<TweenAlpha>().PlayForward();

        OpenLandlordsRecord();
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

    void CloseAll()
    {
        OpenLoadTip();
        btnGameLandlords.isEnabled = true;

        for (int i = 0; i < itemList.Count; i++)
        {
            itemList[i].name = itemList[i].name.Split('_')[0];
            PoolManager.Instance.Unspawn(itemList[i]);
        }
        itemList.Clear();
    }

    void OpenLoadTip()
    {
        loadTip.SetActive(true);
        CancelInvoke("CloseLoadTip");
        Invoke("CloseLoadTip", 10f);
    }

    void CloseLoadTip()
    {
        CancelInvoke("CloseLoadTip");
        loadTip.SetActive(false);
    }


    void RefreshPanel()
    {
        if (!btnGameLandlords.isEnabled)
        {
            OpenLandlordsRecord();
        }
    }

    //斗地主记录
    void OpenLandlordsRecord()
    {
        CloseAll();
        CloseLoadTip();
        btnGameLandlords.isEnabled = false;
        //记录排序
        List<CMD_Hall_S_GameRecord> list = new List<CMD_Hall_S_GameRecord>();
        foreach (CMD_Hall_S_GameRecord info in HallModel.gameRecordList.Values)
        {
            if (info.wKindID == AppConfig.gameDic[GameFlag.Landlords3].kindId)
            {
                list.Add(info);
            }
        }
        list.Sort
            (
                    delegate(CMD_Hall_S_GameRecord record1, CMD_Hall_S_GameRecord record2)
                    {
                        return -record1.InsertTime.CompareTo(record2.InsertTime);//升序
                    }
            );
        //显示游戏记录
        for (int i = 0; i < list.Count; i++)
        {
            GameObject obj = PoolManager.Instance.Spawn(itemRecord4);
            obj.name = itemRecord4.name + "_" + list[i].dwPrivateDrawID;
            obj.transform.parent = pnlScore;
            obj.transform.localScale = Vector3.one;
            obj.transform.localPosition = new Vector3(0f, 180f - 120f * itemList.Count, 0f);

            obj.transform.Find("lblRoomID").GetComponent<UILabel>().text = "房间ID：" + list[i].dwRoomNumber + "（" + list[i].dwGameCount + "局）";
            obj.transform.Find("lblTime").GetComponent<UILabel>().text = list[i].InsertTime.ToString();
            obj.transform.Find("lblKey").GetComponent<UILabel>().text = "编码：" + list[i].dwPrivateDrawID.ToString();
            for (int j = 0; j < 3; j++)
            {
                obj.transform.Find("user_" + j).gameObject.SetActive(true);
                obj.transform.Find("user_" + j + "/lblUserName").GetComponent<UILabel>().text = list[i].szUserNickName[j];
                if (list[i].lUserScore[j] > 0)
                {
                    obj.transform.Find("user_" + j + "/lblUserScore").GetComponent<UILabel>().color = new Color(255f / 255, 66f / 255, 66f / 255);
                    obj.transform.Find("user_" + j + "/lblUserScore").GetComponent<UILabel>().text = "+" + list[i].lUserScore[j].ToString();
                }
                else
                {
                    obj.transform.Find("user_" + j + "/lblUserScore").GetComponent<UILabel>().color = new Color(77f / 255, 153f / 255, 203f / 255);
                    obj.transform.Find("user_" + j + "/lblUserScore").GetComponent<UILabel>().text = list[i].lUserScore[j].ToString();
                }
            }
            obj.transform.Find("user_3").gameObject.SetActive(false);
            itemList.Add(obj);
        }
        pnlScore.GetComponent<UIScrollView>().ResetPosition();
    }


    void OnBtnGameLandloardsClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonNormal);
        CloseAll();
        btnGameLandlords.isEnabled = false;


        bool isHasRecord = false;
        foreach (CMD_Hall_S_GameRecord info in HallModel.gameRecordList.Values)
        {
            if (info.wKindID == AppConfig.gameDic[GameFlag.Landlords3].kindId)
            {
                isHasRecord = true;
                break;
            }
        }
        if (isHasRecord)
        {
            OpenLandlordsRecord();
        }
        else
        {
            HallService.Instance.GetGameRecord(GameFlag.Landlords3);
        }
    }

    void OnBtnCloseClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonClose);
        Close();
    }
}
