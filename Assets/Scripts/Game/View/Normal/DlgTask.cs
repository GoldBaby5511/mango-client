using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class DlgTask : View
{
    private Transform bg;
    private Transform shade;

    private GameObject tipActivity;
    private UIButton[] btnActiveReward = new UIButton[5];

    private UIButton btnClose;
    private GameObject itemTask;

    //奖励效果
    private GameObject pnlGetReward;
    private GameObject itemReward;
    private List<GameObject> taskItemList = new List<GameObject>();
    public float timerReward = 2.0f;

    void Update()
    {
        //是否显示
        if(pnlGetReward.activeSelf == true)
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

        tipActivity = transform.Find("bg/top/PanelActivity/BoxTip").gameObject;
        tipActivity.SetActive(false);
        for (int i = 0; i < btnActiveReward.Length; ++i)
        {
            string btnName = "bg/top/PanelActivity/btnActivity_" + i;
            btnActiveReward[i] = transform.Find(btnName).GetComponent<UIButton>();
            UIEventListener.Get(btnActiveReward[i].gameObject).onPress = OnBtnPressActivity;
            UIEventListener.Get(btnActiveReward[i].gameObject).onClick = OnBtnTakeTaskReward;
        }
        btnClose = transform.Find("bg/pnlClose/btnClose").GetComponent<UIButton>();

        itemTask = transform.Find("bg/Panel/item").gameObject;

        //加载特效
        pnlGetReward = (GameObject)Instantiate(Resources.Load("Prefabs/PanelGetItem"));
        itemReward = pnlGetReward.transform.Find("PanelContent/PanelRewardList/ItemReward").gameObject;
        pnlGetReward.SetActive(false);
        pnlGetReward.transform.parent = transform.Find("bg");
        pnlGetReward.transform.localScale = Vector3.one;

        EventDelegate.Add(btnClose.onClick, OnBtnCloseClick);

        gameObject.SetActive(false);
        bg.GetComponent<TweenScale>().ResetToBeginning();
        bg.GetComponent<TweenAlpha>().ResetToBeginning();
        shade.GetComponent<TweenAlpha>().ResetToBeginning();
    }

    public override void RegisterAction()
    {
        GameEvent.V_OpenDlgTask += Open;
        GameEvent.V_RefreshDlgTask += OnTaskResult;
    }

    public override void RemoveAction()
    {
        GameEvent.V_OpenDlgTask -= Open;
        GameEvent.V_RefreshDlgTask -= OnTaskResult;
    }

    public void Open()
    {
        gameObject.SetActive(true);
        bg.GetComponent<TweenScale>().PlayForward();
        bg.GetComponent<TweenAlpha>().PlayForward();
        shade.GetComponent<TweenAlpha>().PlayForward();

        //构建列表
        BuildTaskList();
    }

    public void OnTaskResult(CMD_CM_S_TaskResult pro)
    {
        if (pro == null) return;
        if (pro.bSuccessed == false) return;

        //Debug.LogError("领取结果,ID," + pro.nPropID + "数量," + pro.nPropCount);

        //设置状态
        TagTaskStatus statusItem = null;
        foreach (TagTaskStatus item in HallModel.taskStatus)
        {
            if (item.wTaskID != pro.wTaskID) continue;
            item.cbTaskStatus = 3;
            statusItem = item;
            break;
        }
        if (statusItem == null) return;

        //构建列表
        BuildTaskList();

        //rewardItemList.Clear();
        //删除物体
        Transform transform = pnlGetReward.transform.Find("PanelContent/PanelRewardList");
        for (int i = transform.childCount - 1; i >= 1; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        itemReward.SetActive(false);

        GameObject obj = PoolManager.Instance.Spawn(itemReward);
        obj.name = itemReward.name + "_" + statusItem.wTaskID;
        obj.transform.parent = itemReward.transform.parent;
        obj.transform.localScale = Vector3.one;
        obj.transform.Find("PanelContent/TextName").gameObject.SetActive(false);
        obj.transform.Find("PanelContent/Text").GetComponent<Text>().text = pro.nPropCount.ToString();
        Image imageReward = obj.transform.Find("PanelContent/ImageIconBg/ImageIcon").GetComponent<Image>();
        if (pro.nPropID == 600) imageReward.GetComponent<RectTransform>().sizeDelta = new Vector2(52f, 67f);
        string rewardName = GetNameByPropID(pro.nPropID, pro.nPropCount);
        imageReward.sprite = Resources.Load("texture/icons/" + rewardName, typeof(Sprite)) as Sprite;
        obj.SetActive(true);

        pnlGetReward.SetActive(true);
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

    public void OnBtnCloseClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonClose);
        Close();
    }

    /// <summary>
    /// 领取奖励
    /// </summary>
    /// <param name="button"></param>
    public void OnBtnTakeTaskReward(GameObject button)
    {
        int taskID = int.Parse(button.name.Split('_')[1]);
        TagTaskStatus statusItem = GetStatusItemByID(taskID);
        if (statusItem == null) return;
        if (statusItem.cbTaskStatus != 1) return;

        AudioManager.Instance.PlaySound(GameModel.audioButtonNormal);
        //Debug.LogError("领取奖励,ID," + taskID);
        HallService.Instance.TakeTaskReward(taskID);
    }

    /// <summary>
    /// 查看奖励
    /// </summary>
    /// <param name="button"></param>
    public void OnBtnPressActivity(GameObject button, bool state)
    {
        int taskID = int.Parse(button.name.Split('_')[1]);
        TagTaskParameter taskItem = GetTaskItemByID(taskID);
        if (taskItem == null) return;
        int btnID = taskID - 200;

        //Debug.LogError("查看奖励,ID," + btnID + ",状态," + state);
        tipActivity.SetActive(state);
        tipActivity.transform.Find("Label").GetComponent<UILabel>().text = taskItem.szTaskDescribe;
        tipActivity.transform.localPosition = new Vector3(btnActiveReward[btnID].transform.localPosition.x, tipActivity.transform.localPosition.y, tipActivity.transform.localPosition.z);
    }

    /// <summary>
    /// 领取奖励
    /// </summary>
    /// <param name="button"></param>
    public void OnBtnClickActivity(GameObject button)
    {
        int taskID = int.Parse(button.name.Split('_')[1]);
        TagTaskStatus statusItem = GetStatusItemByID(taskID);
        if (statusItem == null) return;
        if (statusItem.cbTaskStatus != 1) return;

        //Debug.LogError("领取奖励,ID," + taskID);
        AudioManager.Instance.PlaySound(GameModel.audioButtonNormal);
    }

    void BuildTaskList()
    {
        //UIScrollView
        //删除物体
        Transform transformList = transform.Find("bg/Panel");
        for (int i = transformList.childCount - 1; i >= 1; i--)
        {
            Destroy(transformList.GetChild(i).gameObject);
        }
        taskItemList.Clear();
        float curActivity = 0;
        float totalActivity = 0;
        for (int i = 0; i < HallModel.taskParameter.Count; ++i)
        {
            int taskID = HallModel.taskParameter[i].wTaskID;

            //获取状态
            TagTaskStatus statusItem = null;
            foreach (TagTaskStatus item in HallModel.taskStatus)
            {
                if (item.wTaskID != HallModel.taskParameter[i].wTaskID) continue;
                statusItem = item;
                break;
            }
            if (statusItem == null) continue;

            //ID 3-199为每日任务、 200-299 为活跃度任务
            if (HallModel.taskParameter[i].wTaskID < 200)
            {
                GameObject obj = PoolManager.Instance.Spawn(itemTask);

                obj.name = "item_" + HallModel.taskParameter[i].wTaskID;
                obj.transform.parent = itemTask.transform.parent;
                obj.transform.localScale = Vector3.one;
                //obj.transform.localPosition = new Vector3(0f, 302f - 112f * i, 0f);
                obj.transform.localPosition = new Vector3(0f, 90f - 112f * i, 0f);

                //状态判断
                if (statusItem.cbTaskStatus == 3)
                {
                    obj.transform.Find("imgComplete").gameObject.SetActive(true);
                    obj.transform.Find("btnTake").gameObject.SetActive(false);
                    curActivity += HallModel.taskParameter[i].nActivityCount;
    }
                else
                {
                    obj.transform.Find("imgComplete").gameObject.SetActive(false);
                    obj.transform.Find("btnTake").gameObject.SetActive(true);
                    obj.transform.Find("btnTake").GetComponent<UIButton>().isEnabled = (statusItem.cbTaskStatus == 1);
                }
                string btnName = obj.transform.Find("btnTake").name + "_" + HallModel.taskParameter[i].wTaskID;
                obj.transform.Find("btnTake").name = btnName;
                obj.transform.Find("lblName").GetComponent<UILabel>().text = HallModel.taskParameter[i].szTaskDescribe;
                obj.transform.Find("lblActivityCount").GetComponent<UILabel>().text = "活跃度 +" + HallModel.taskParameter[i].nActivityCount;
                string propName = GetNameByPropID(HallModel.taskParameter[i].nStandardAwardPropID, HallModel.taskParameter[i].nStandardAwardPropCount);
                obj.transform.Find("Rewardbg/Sprite").GetComponent<UISprite>().spriteName = propName;
                if (HallModel.taskParameter[i].nStandardAwardPropID == 600) obj.transform.Find("Rewardbg/Sprite").GetComponent<UISprite>().MakePixelPerfect();
                obj.transform.Find("Rewardbg/Label").GetComponent<UILabel>().text = HallModel.taskParameter[i].nStandardAwardPropCount.ToString();
                obj.transform.Find("slider").GetComponent<UISlider>().value = ((float)statusItem.wTaskProgress / (float)HallModel.taskParameter[i].wTaskObject);
                obj.transform.Find("slider/Label").GetComponent<UILabel>().text = statusItem.wTaskProgress.ToString() + "/" + HallModel.taskParameter[i].wTaskObject.ToString();

                UIEventListener.Get(obj.transform.Find(btnName).gameObject).onClick = OnBtnTakeTaskReward;

                obj.SetActive(true);

                taskItemList.Add(obj);
            }
            else if (taskID >= 200 && taskID < 300)
            {
                btnActiveReward[taskID - 200].transform.Find("Label").GetComponent<UILabel>().text = HallModel.taskParameter[i].wTaskObject.ToString();
                btnActiveReward[taskID - 200].name = "btnActivity_" + taskID;
                totalActivity = HallModel.taskParameter[i].wTaskObject;
            }
        }

        if (curActivity > totalActivity) curActivity = totalActivity;

        //偷懒，客户端自己修改活跃度完成状态,总之领取的时候服务端会校验
        for (int i = 0; i < HallModel.taskParameter.Count; ++i)
        {
            //ID 3-199为每日任务、 200-299 为活跃度任务
            int taskID = HallModel.taskParameter[i].wTaskID;
            if (taskID < 200 || taskID >= 300) continue;

            //获取状态
            TagTaskStatus statusItem = null;
            foreach (TagTaskStatus item in HallModel.taskStatus)
            {
                if (item.wTaskID != HallModel.taskParameter[i].wTaskID) continue;
                statusItem = item;
                break;
            }
            if (statusItem == null) continue;
            if (statusItem.cbTaskStatus != 0) continue;
            if (HallModel.taskParameter[i].wTaskObject < curActivity) continue;

            //设为成功状态
            statusItem.cbTaskStatus = 1;
        }


        //活跃度进度
        transform.Find("bg/top/curActicity/Label").GetComponent<UILabel>().text = curActivity.ToString();
        transform.Find("bg/top/slider").GetComponent<UISlider>().value = (curActivity / totalActivity);

        //设置起始位置
        //Vector3[] vt =transform.Find("bg/Panel").GetComponent<UIScrollView>().panel.localCorners;
        //Vector3 v3 = transform.Find("bg/Panel/item_3").transform.localPosition;

        //transform.Find("bg/Panel").GetComponent<UIScrollView>().MoveRelative(new Vector3(0, -210, 0));
    }

    string GetNameByPropID(int propID, int propCount)
    {
        switch (propID)
        {
            case 0:
                {
                    if (propCount > 50) return "钻石03";
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

    TagTaskStatus GetStatusItemByID(int taskID)
    {
        TagTaskStatus statusItem = null;
        foreach (TagTaskStatus item in HallModel.taskStatus)
        {
            if (item.wTaskID != taskID) continue;
            statusItem = item;
            break;
        }
        return statusItem;
    }

    TagTaskParameter GetTaskItemByID(int taskID)
    {
        TagTaskParameter statusItem = null;
        foreach (TagTaskParameter item in HallModel.taskParameter)
        {
            if (item.wTaskID != taskID) continue;
            statusItem = item;
            break;
        }
        return statusItem;
    }

}
