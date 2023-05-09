using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class HallDlgWheelSignDay : View
{
    private Transform bg;
    private Transform shade;

    private UIButton btnClose;
    //签到活动
    private UIButton btnSign;
    private GameObject pnlGetReward;
    private GameObject WaitCursor1;
    private GameObject WaitCursor2;
    public float proTime = 0.0f;
    public float NextTime = 0.0f;
    public bool bShow1 = true;


    private Transform[] wheelItems = new Transform[10];
    private Transform[] seriesCheckInItems = new Transform[5];


    //幸运转盘
    private Transform pnlWheel;
    private GameObject itemReward;
    private List<GameObject> rewardItemList = new List<GameObject>();
    private int nConfigID = 0;                              //当前道具
    private int nAddConfigID = 0;                               //当前道具

    public float endAngle = 100;//旋转停止位置，相对y坐标方向的角度

    public Vector3 targetDir;//目标点的方向向量
    bool isMoving = false;//是否在旋转
    public float speed = 0;//当前的旋转速度
    public float maxSpeed = 200;//最大旋转速度
    public float minSpeed = 0.8f;//最小旋转速度
    float rotateTimer = 2;//旋转计时器
    public int moveState = 0;//旋转状态，旋转，减速
    public int keepTime = 3;//旋转减速前消耗的时间

    void Update()
    {
        if (isMoving)
        {
            WaitCursor1.SetActive(false);
            WaitCursor2.SetActive(false);

            if (moveState == 1 && (rotateTimer > 0 || getAngle() < 270))
            {
                //如果旋转时间小于旋转保持时间，或者大于旋转保持时间但是与停止方向角度小于270，继续保持旋转
                rotateTimer -= Time.deltaTime;
                if (speed < maxSpeed) speed += 1;
                pnlWheel.Rotate(new Vector3(0, 0, speed));
            }
            else
            {
                //减速旋转，知道停止在目标位置
                moveState = 2;
                if (speed > minSpeed) speed -= 7 * speed / 100;
                if (getAngle() > 10)
                    pnlWheel.Rotate(new Vector3(0, 0, speed));
                else
                {
                    //stop
                    endMove();
                }
            }
        }
        else
        {
            proTime = Time.fixedTime;
            if (proTime - NextTime > 0.5)
            {
                bShow1 = !bShow1;
                if (bShow1)
                {
                    WaitCursor1.SetActive(false);
                    WaitCursor2.SetActive(true);
                }
                else
                {
                    WaitCursor1.SetActive(true);
                    WaitCursor2.SetActive(false);
                }
                NextTime = proTime;
            }
        }
    }

    public override void Init()
    {
        bg = transform.Find("bg");
        shade = transform.Find("shade");

        btnClose = transform.Find("bg/SignGift/ButtonClose").GetComponent<UIButton>();
        btnSign = transform.Find("bg/ImageZhiZhen/ButtonGo").GetComponent<UIButton>();

        //加载特效
        pnlGetReward = (GameObject)Instantiate(Resources.Load("Prefabs/PanelGetItem"));
        itemReward = pnlGetReward.transform.Find("PanelContent/PanelRewardList/ItemReward").gameObject;
        pnlGetReward.SetActive(false);
        pnlGetReward.transform.parent = transform.Find("bg");
        pnlGetReward.transform.localScale = Vector3.one;

        WaitCursor1 = transform.Find("bg/ImageZhiZhen/WaitCursor1").gameObject;
        WaitCursor2 = transform.Find("bg/ImageZhiZhen/WaitCursor2").gameObject;

        for (int i = 0; i < 10; i++)
        {
            wheelItems[i] = transform.Find("bg/Wheelbg/WheelSign/Item" + i);
            wheelItems[i].Find("Sprite").gameObject.SetActive(false);
        }

        for(int i = 0; i < 5; ++i)
        {
            seriesCheckInItems[i] = transform.Find("bg/SignGift/ImgGiftShow/ItemGiftBG" + i);
            seriesCheckInItems[i].Find("Get").gameObject.SetActive(false);
        }

        //获取转盘
        pnlWheel = transform.Find("bg/Wheelbg/WheelSign");

        EventDelegate.Add(btnClose.onClick, OnBtnCloseClick);
        EventDelegate.Add(btnSign.onClick, OnBtnSignClick);

        gameObject.SetActive(false);
        bg.GetComponent<TweenScale>().ResetToBeginning();
        bg.GetComponent<TweenAlpha>().ResetToBeginning();
        shade.GetComponent<TweenAlpha>().ResetToBeginning();
    }

    public override void RegisterAction()
    {
        HallEvent.V_OpenDlgWheelSignDay += Open;
    }

    public override void RemoveAction()
    {
        HallEvent.V_OpenDlgWheelSignDay -= Open;
    }

    void Open(CMD_Hall_S_SignInResult pro)
    {
        if(pro == null)
        {
            //是否已经签到
            btnSign.isEnabled = (!HallModel.isSign);

            //连续签到
            transform.Find("bg/SignGift/ImgGiftShow/ImageSignDayTxt/Label").GetComponent<UILabel>().text = HallModel.signDay.ToString();

            //初始化转盘
            for (int i = 0; i < HallModel.RewardCheckIn.Length; i++)
            {
                string propName = GetNameByPropID(HallModel.RewardCheckIn[i]);
                wheelItems[i].GetComponent<UISprite>().spriteName = propName;
                if (HallModel.RewardCheckIn[i].nPropID == 600)  wheelItems[i].GetComponent<UISprite>().MakePixelPerfect();
                wheelItems[i].Find("Sprite").gameObject.SetActive((HallModel.RewardCheckIn[i].cbBigReward == 1));
                wheelItems[i].Find("Label").GetComponent<UILabel>().text = HallModel.RewardCheckIn[i].nGiveCount.ToString();
            }

            //初始连续奖励
            for (int i = 0; i < HallModel.SeriesRewardInfo.Length; ++i)
            {
                seriesCheckInItems[i].Find("Label").GetComponent<UILabel>().text = HallModel.SeriesRewardInfo[i].RewardItem.nGiveCount.ToString();
                string propName = GetNameByPropID(HallModel.SeriesRewardInfo[i].RewardItem);
                seriesCheckInItems[i].Find("ImageGift").GetComponent<UISprite>().spriteName = propName;
                if (HallModel.SeriesRewardInfo[i].RewardItem.nPropID == 600) seriesCheckInItems[i].Find("ImageGift").GetComponent<UISprite>().MakePixelPerfect();
                seriesCheckInItems[i].Find("Get").gameObject.SetActive((HallModel.signDay >= HallModel.SeriesRewardInfo[i].nSeriesDays));
                seriesCheckInItems[i].Find("Days/Label").GetComponent<UILabel>().text = HallModel.SeriesRewardInfo[i].nSeriesDays.ToString() + "天";
            }

            gameObject.SetActive(true);
            bg.GetComponent<TweenScale>().PlayForward();
            bg.GetComponent<TweenAlpha>().PlayForward();
            shade.GetComponent<TweenAlpha>().PlayForward();
        }
        else
        {
            nConfigID = pro.nConfigID;
            nAddConfigID = pro.nAddConfigID;
            //Debug.LogError("签到" + nConfigID + ",附加," + nAddConfigID);

            //开始旋转
            StartMove(pro.nResultIndex);
        }
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

    #region 计算当前对象y方向与目标方向的夹角

    float getAngle()
    {
        return calAngle(targetDir, pnlWheel.up);//计算y轴方向的旋转角度
    }
    //计算从dir1旋转到dir2的角度

    float calAngle(Vector3 dir1, Vector3 dir2)
    {
        float angle = Vector3.Angle(dir1, dir2);
        Vector3 normal = Vector3.Cross(dir1, dir2);
        angle = normal.z > 0 ? angle : (360 - angle);
        return angle;
    }

    #endregion

    /// <summary>
    /// 计算目标位置的向量
    /// Calculates the dir.
    /// </summary>
    /// <param name="endAngle">End angle.</param>
    Vector3 calculateDir(float endAngle)
    {
        float radiansX = Mathf.Cos(Mathf.PI * (endAngle + 90) / 180);
        float radiansY = Mathf.Sin(Mathf.PI * (endAngle + 90) / 180);
        return new Vector3(radiansX, radiansY, 0);

    }

    void endMove()
    {
        AudioManager.Instance.PlaySound(GameModel.audioGetAward);

        speed = 0;
        isMoving = false;
        moveState = 0;
        //加载预设体资源
        itemReward.SetActive(false);
        foreach (TagCheckInItem item in HallModel.RewardCheckIn)
        {
            if (item.nConfigID != nConfigID) continue;
            AddRewardItem(item);
            break;
        }

        foreach (TagSeriesCheckInReward item in HallModel.SeriesRewardInfo)
        {
            if (item.RewardItem.nConfigID != nAddConfigID) continue;
            AddRewardItem(item.RewardItem);
            break;
        }
        pnlGetReward.SetActive(true);

        //连续签到
        transform.Find("bg/SignGift/ImgGiftShow/ImageSignDayTxt/Label").GetComponent<UILabel>().text = HallModel.signDay.ToString();

        //已领取标识
        for (int i = 0; i < HallModel.SeriesRewardInfo.Length; ++i)
        {
            seriesCheckInItems[i].Find("Get").gameObject.SetActive((HallModel.signDay >= HallModel.SeriesRewardInfo[i].nSeriesDays));
        }
    }

    void AddRewardItem(TagCheckInItem checkInItem)
    {
        if (checkInItem == null) return;

        GameObject obj = new GameObject();
        obj = (GameObject)Instantiate(itemReward);
        obj.name = itemReward.name + "_" + checkInItem.nConfigID;
        obj.transform.parent = itemReward.transform.parent;
        obj.transform.localScale = Vector3.one;
        obj.transform.Find("PanelContent/TextName").gameObject.SetActive(false);
        obj.transform.Find("PanelContent/Text").GetComponent<Text>().text = checkInItem.nGiveCount.ToString();
        Image imageReward = obj.transform.Find("PanelContent/ImageIconBg/ImageIcon").GetComponent<Image>();
        if(checkInItem.nPropID == 600) imageReward.GetComponent<RectTransform>().sizeDelta = new Vector2(52f, 67f);
        string rewardName = GetNameByPropID(checkInItem);
        imageReward.sprite = Resources.Load("texture/icons/"+ rewardName, typeof(Sprite)) as Sprite;
        obj.SetActive(true);

        //加入队列
        rewardItemList.Add(obj);

        return;
    }

    void StartMove(int index)
    {
        //非法判断 合法值 1-10
        if (index < 1 || index > 10) return;
        if (isMoving) return;
        //是否已经签到
        btnSign.isEnabled = false;

        endAngle = (index-1) * 360 / 10;//获得目标位置相对y坐标方向的角度
        targetDir = calculateDir(endAngle);//获得目标位置方向向量
        rotateTimer = keepTime;
        isMoving = true;
        moveState = 1;
    }

    //签到活动
    void OnBtnSignClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonSwitch);

        //是否已经签到
        if (HallModel.isSign) return;
        HallService.Instance.SignIn();
        
        return;
    }

    void OnBtnCloseClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonClose);
        Close();

        pnlGetReward.SetActive(false);
        rewardItemList.Clear();
        //删除物体
        Transform transform = pnlGetReward.transform.Find("PanelContent/PanelRewardList");
        for (int i = transform.childCount - 1; i >= 1; i --)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        //弹月卡
        if (!HallModel.isBuyMonthCard)
        {
            Util.Instance.DoAction(HallEvent.V_OpenDlgMonthCard, 0);
        }
        else
        {
            //弹领取
            if (!HallModel.isGetMonthCardGift)
            {
                Util.Instance.DoAction(HallEvent.V_OpenDlgMonthCard, 0);
            }
            else
            {
                //弹分享任务
                Util.Instance.DoActionDelay(HallEvent.V_OpenDlgShareTask, 0.2f);
            }
        }
    }

    string GetNameByPropID(TagCheckInItem checkInItem)
    {
        if (checkInItem == null) return "";

        int propID = checkInItem.nPropID;
        int propCount = checkInItem.nGiveCount;
        switch (propID)
        {
            case 0:
                {
                    if(propCount > 50) return "钻石03";
                    if (propCount > 10) return "钻石02";
                    return "钻石01";
                }
            case 1:
                {
                    if (propCount > 5000) return "金币03";
                    if (propCount > 1000) return "金币02";
                    return "金币01";
                }
            case 600: return "flag奖券";
        }
        return "";
    }
}
