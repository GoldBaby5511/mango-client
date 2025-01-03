using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

public class HallDlgShareTask : View
{
    private Transform bg;
    private Transform shade;
    private GameObject pnlInfo;
    private UILabel lblFriendCount;
    private UIButton btnList;
    private UILabel[] lblCanTakeCount = new UILabel[2];
    private UILabel[] lblTaskDescribe = new UILabel[4];
    private UIButton[] btnTakeReward = new UIButton[2];
    private int friendCount = 0;
    private int rewardTask1Count = 0;
    private int rewardTask2Count = 0;


    private GameObject pnlList;
    private Transform[] itemRecords = new Transform[5];
    private UIButton btnPre;
    private UIButton btnNext;
    private UIButton btnBack;
    private UILabel lblPage;
    private int currentPage = 1;
    private int totalPage = 1;

    private UIButton btnShare;
    private UIButton btnShareCircle;
    private UIButton btnClose;
    //private UILabel lblCurCount;
    private UILabel lblRewardCount;

    public override void Init()
    {
        bg = transform.Find("bg");
        shade = transform.Find("shade");
        btnShare = transform.Find("bg/btnShare").GetComponent<UIButton>();
        btnShareCircle = transform.Find("bg/btnShareCircle").GetComponent<UIButton>();
        btnClose = transform.Find("bg/btnClose").GetComponent<UIButton>();
        btnList = transform.Find("bg/btnList").GetComponent<UIButton>();
        lblFriendCount = transform.Find("bg/lblFriendCount").GetComponent<UILabel>();

        pnlInfo = transform.Find("bg/pnlInfo").gameObject;
        for (int i = 0; i < 2; i++)
        { 
            lblCanTakeCount[i] = transform.Find("bg/pnlInfo/itemTask" + i + "/lblCount").GetComponent<UILabel>();
            btnTakeReward[i] = transform.Find("bg/pnlInfo/itemTask" + i + "/btnTake").GetComponent<UIButton>();
        }

        lblTaskDescribe[0] = transform.Find("bg/pnlInfo/itemTask0/lblTip").GetComponent<UILabel>();
        lblTaskDescribe[1] = transform.Find("bg/pnlInfo/itemTask0/lblReward").GetComponent<UILabel>();

        lblTaskDescribe[2] = transform.Find("bg/pnlInfo/itemTask1/lblTip").GetComponent<UILabel>();
        lblTaskDescribe[3] = transform.Find("bg/pnlInfo/itemTask1/lblReward").GetComponent<UILabel>();

        pnlList = transform.Find("bg/pnlList").gameObject;
        for (int i = 0; i < 5; i++)
        {
            itemRecords[i] = transform.Find("bg/pnlList/itemUser" + i);    
        }
        btnPre = transform.Find("bg/pnlList/btnPre").GetComponent<UIButton>();
        btnNext = transform.Find("bg/pnlList/btnNext").GetComponent<UIButton>();
        btnBack = transform.Find("bg/pnlList/btnBack").GetComponent<UIButton>();
        lblPage = transform.Find("bg/pnlList/lblPage").GetComponent<UILabel>();

        //lblCurCount = transform.Find("bg/lblCurCount").GetComponent<UILabel>();
        lblRewardCount = transform.Find("bg/lblRewardCount").GetComponent<UILabel>();

        //按钮相应
        EventDelegate.Add(btnList.onClick, OnBtnListClick);
        EventDelegate.Add(btnBack.onClick, OnBtnBackClick);
        EventDelegate.Add(btnShare.onClick, OnbtnShareClick);
        EventDelegate.Add(btnShareCircle.onClick, OnBtnShareCircleClick);
        EventDelegate.Add(btnClose.onClick, OnBtnCloseClick);
        EventDelegate.Add(btnTakeReward[0].onClick, OnBtnTakeReward1Click);
        EventDelegate.Add(btnTakeReward[1].onClick, OnBtnTakeReward2Click);
        EventDelegate.Add(btnPre.onClick, OnBtnPreClick);
        EventDelegate.Add(btnNext.onClick, OnBtnNextClick);


        gameObject.SetActive(false);
        bg.GetComponent<TweenScale>().ResetToBeginning();
        bg.GetComponent<TweenAlpha>().ResetToBeginning();
        shade.GetComponent<TweenAlpha>().ResetToBeginning();
    }

    public override void RegisterAction()
    {
        HallEvent.V_OpenDlgShareTask += Open;
    }

    public override void RemoveAction()
    {
        HallEvent.V_OpenDlgShareTask -= Open;
    }

    //打开首充界面
    void Open()
    {
        gameObject.SetActive(true);
        bg.GetComponent<TweenScale>().PlayForward();
        bg.GetComponent<TweenAlpha>().PlayForward();
        shade.GetComponent<TweenAlpha>().PlayForward();

        pnlInfo.gameObject.SetActive(true);
        pnlList.gameObject.SetActive(false);

        GetInviteAwardInfo();
    }

    void Close()
    {
        bg.GetComponent<TweenScale>().PlayReverse();
        bg.GetComponent<TweenAlpha>().PlayReverse();
        shade.GetComponent<TweenAlpha>().PlayReverse();
        DoActionDelay(CloseCor, 0.4f);
    }

    void CloseCor()
    {
        gameObject.SetActive(false);
    }

    //好友列表
    void OnBtnListClick()
    {
        //判断当前是否已显示
        if(pnlList.gameObject.activeSelf) 
        {
            return;
        }

        AudioManager.Instance.PlaySound(GameModel.audioButtonNormal);
        
        //数量判断
        if(int.Parse(lblFriendCount.text) <= 0)
        {
            DoAction(GameEvent.V_OpenShortTip, "暂无好友,快去邀请吧！");
            return;
        }

        pnlInfo.gameObject.SetActive(false);
        pnlList.gameObject.SetActive(true);

        lblPage.text = currentPage + "/" + totalPage;
        for (int i = 0; i < itemRecords.Length; i++)
        {
            itemRecords[i].gameObject.SetActive(false);
        }
        GetPlayerTaskRecord(currentPage);
    }

    //返回信息
    void OnBtnBackClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonNormal);
        pnlInfo.gameObject.SetActive(true);
        pnlList.gameObject.SetActive(false);
    }


    //分享好友
    void OnbtnShareClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonOp);
        StartCoroutine(WeiXinShare(0, null));
    }

    //分享朋友圈
    void OnBtnShareCircleClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonOp);
        //StartCoroutine(WeiXinShare(1, HallService.Instance.ShareSuccess));
        StartCoroutine(WeiXinShare(1, null));
    }

    void OnBtnCloseClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonClose);
        Close();
    }

    //领取奖励1
    void OnBtnTakeReward1Click()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonNormal);

        //数量判断
        if(rewardTask1Count <= 0)
        {
            DoAction(GameEvent.V_OpenShortTip, "暂无奖励可领取！");
            return;
        }
        GetInviteAward(1);
    }

    //领取奖励2
    void OnBtnTakeReward2Click()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonNormal);

        //数量判断
        if(rewardTask2Count <= 0)
        {
            DoAction(GameEvent.V_OpenShortTip, "暂无奖励可领取！");
            return;
        }
        GetInviteAward(2);
    }

    //上一页
    void OnBtnPreClick()
    {
        if (currentPage <= 1)
        {
            AudioManager.Instance.PlaySound(GameModel.audioTipWarn);
            currentPage = 1;
            lblPage.text = currentPage + "/" + totalPage;
            DoAction(GameEvent.V_OpenShortTip, "当前已是第一页！");
        }
        else
        {
            AudioManager.Instance.PlaySound(GameModel.audioButtonNormal);
            GetPlayerTaskRecord(currentPage - 1);
            for (int i = 0; i < itemRecords.Length; i++)
            {
                itemRecords[i].gameObject.SetActive(false);
            }
        }
    }

    //下一页
    void OnBtnNextClick()
    {
        if (currentPage >= totalPage)
        {
            AudioManager.Instance.PlaySound(GameModel.audioTipWarn);
            currentPage = totalPage;
            lblPage.text = currentPage + "/" + totalPage;
            DoAction(GameEvent.V_OpenShortTip, "当前已是最后一页！");
        }
        else
        {
            AudioManager.Instance.PlaySound(GameModel.audioButtonNormal);
            GetPlayerTaskRecord(currentPage + 1);
            for (int i = 0; i < itemRecords.Length; i++)
            {
                itemRecords[i].gameObject.SetActive(false);
            }
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

    //请求邀请奖励信息
    void GetInviteAwardInfo()
    {
        //Web_C_InviteFriendInfo pro = new Web_C_InviteFriendInfo();
        //pro.userId = HallModel.userId;
        //WebService.Instance.Send<Web_S_InviteFriendInfo>(AppConfig.url_InviteFriend, pro, OnGetInviteAwardInfo);
    }

    //收到邀请奖励信息
    void OnGetInviteAwardInfo(Web_S_InviteFriendInfo pro)
    {
        if (pro == null || pro.return_code != 10000)
        {
            //DoAction(GameEvent.V_OpenShortTip, "邀请好友基础信息请求数据异常！");
            friendCount = 0;
            rewardTask1Count = 0;
            rewardTask2Count = 0;
            return;
        }
        else
        { 
            //lblCurCount.text = pro.return_message.CurCount.ToString();
            lblRewardCount.text = pro.return_message.TakeCount.ToString();
            friendCount = pro.return_message.UserCount;
            rewardTask1Count = pro.return_message.TaskCount1;
            rewardTask2Count = pro.return_message.TaskCount2;            
        }

        lblFriendCount.text = friendCount.ToString();
        lblCanTakeCount[0].text = "可领: "+ rewardTask1Count.ToString() ;
        lblCanTakeCount[1].text = "可领: "+ rewardTask2Count.ToString() ;

        lblTaskDescribe[0].text = pro.return_message.Task1Describe;
        lblTaskDescribe[1].text = pro.return_message.Task1RewardCount.ToString();
        lblTaskDescribe[2].text = pro.return_message.Task2Describe;
        lblTaskDescribe[3].text = pro.return_message.Task2RewardCount.ToString();
    }

    //领取邀请奖励
    void GetInviteAward(int taskid)
    {
        //Web_C_GetInviteAward pro = new Web_C_GetInviteAward();
        //pro.userId = HallModel.userId;
        //pro.taskid = taskid;
        //WebService.Instance.Send<Web_S_GetInviteAward>(AppConfig.url_InviteFriend, pro, OnGetInviteAward);
    }

    //收到领取奖励
    void OnGetInviteAward(Web_S_GetInviteAward pro)
    {
        if (pro == null)
        {
            DoAction(GameEvent.V_OpenShortTip, "领取奖励失败！");
        }
        else if (pro.return_code != 10000)
        {
            DoAction(GameEvent.V_OpenShortTip, pro.message);
        }
        else
        {
            if (HallEvent.V_OpenDlgGetAward != null)
            {
                HallEvent.V_OpenDlgGetAward.Invoke(pro.IngotCount, pro.RoomCardCount, pro.RedEnvelopesCount, 1);
            }
            //请求奖励信息
            GetInviteAwardInfo();
            //刷新
            HallService.Instance.QueryBankInfo();
            DoActionDelay(HallService.Instance.QueryBankInfo, 5f);
        }
    }

    //请求好友任务记录
    void GetPlayerTaskRecord(int index)
    {
        //Web_C_PlayerTaskRecord pro = new Web_C_PlayerTaskRecord();
        //pro.userId = HallModel.userId;
        //pro.pageIndex = index;
        //WebService.Instance.Send<Web_S_PlayerTaskRecord>(AppConfig.url_InviteFriend, pro, OnGetPlayerTaskRecord);
    }

    //收到好友任务记录
    void OnGetPlayerTaskRecord(Web_S_PlayerTaskRecord pro)
    {
        if (pro == null)
        {
            //DoAction(GameEvent.V_OpenShortTip, "邀请好友记录数据请求数据异常！");
            return;
        }
        else if (pro.return_code != 10000)
        {
            DoAction(GameEvent.V_OpenShortTip, pro.message);
        }
        else
        {
            currentPage = pro.pageIndex;
            totalPage = pro.count;
            lblPage.text = currentPage + "/" + totalPage;
            for (int i = 0; i < 5; i++)
            {
                if (i < pro.return_message.Count)
                {
                    itemRecords[i].gameObject.SetActive(true);
                    itemRecords[i].Find("lblUserName").GetComponent<UILabel>().text = pro.return_message[i].NickName;
                    itemRecords[i].Find("lblCount").GetComponent<UILabel>().text = "¥"+ pro.return_message[i].PayCount.ToString();
                    itemRecords[i].Find("lblState").GetComponent<UILabel>().text = GetStateName(pro.return_message[i].TaskStatus1);
                    itemRecords[i].Find("lblState").GetComponent<UILabel>().color = GetStateColor(pro.return_message[i].TaskStatus1);
                }
            }
        }
    }

    string GetStateName(int TaskStatus1)
    { 
        return TaskStatus1 == 0?"未完成":"已完成";
    }

    Color GetStateColor(int TaskStatus1)
    {
        switch (TaskStatus1)
        {
            case 1:
                return new Color(255 / 255f, 2 / 255f, 2 / 255f);
            case 3:
                return new Color(80 / 255f, 27 / 255f, 205 / 255f);
            case 4:
                return new Color(3 / 255f, 95 / 255f, 25 / 255f);
            default:
                return new Color(255 / 255f, 2 / 255f, 2 / 255f);
        }
    }



}
