using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HallDlgEmail : View 
{
    private Transform emailList;
    private GameObject itemEmail;
    private UIButton btnClose_list;

    private Transform emailInfo;
    private UILabel lblTitle;
    private UILabel lblContent;
    private UILabel lblDateTime;
    private GameObject itemGift;
    private UIButton btnClose_info;

    private Transform shade;

    private int opCode = 1;     //1-查询   2-阅读
    private List<EmailInfo> emailInfoList = new List<EmailInfo>();
    private List<GameObject> emailItemList = new List<GameObject>();

    public override void Init()
    {
        emailList = transform.Find("emailList");
        itemEmail = transform.Find("emailList/pnlList/itemEmail").gameObject;
        btnClose_list = transform.Find("emailList/Panel/btnClose").GetComponent<UIButton>();

        emailInfo = transform.Find("emailInfo");
        lblTitle = transform.Find("emailInfo/lblTitle").GetComponent<UILabel>();
        lblContent = transform.Find("emailInfo/lblContent").GetComponent<UILabel>();
        lblDateTime = transform.Find("emailInfo/lblDateTime").GetComponent<UILabel>();
        itemGift = transform.Find("emailInfo/itemGift").gameObject;
        btnClose_info = transform.Find("emailInfo/btnClose").GetComponent<UIButton>();

        shade = transform.Find("shade");

        EventDelegate.Add(btnClose_info.onClick, OnBtnCloseClick_info);
        EventDelegate.Add(btnClose_list.onClick, OnBtnCloseClick_list);

        gameObject.SetActive(false);
        itemEmail.SetActive(false);
        itemGift.SetActive(false);
        emailList.GetComponent<TweenScale>().ResetToBeginning();
        emailList.GetComponent<TweenAlpha>().ResetToBeginning();
        emailInfo.GetComponent<TweenScale>().ResetToBeginning();
        emailInfo.GetComponent<TweenAlpha>().ResetToBeginning();
        shade.GetComponent<TweenAlpha>().ResetToBeginning();
    }

    public override void RegisterAction()
    {
        HallEvent.V_OpenDlgEmail += Open;
    }

    public override void RemoveAction()
    {
        HallEvent.V_OpenDlgEmail -= Open;
    }

    #region UI操作

    public void Open()
    {
        gameObject.SetActive(true);
        shade.GetComponent<TweenAlpha>().PlayForward();

        OpenEmailList();

        Web_C_EmailInfo pro = new Web_C_EmailInfo();
        pro.type = 1;
        pro.userId = HallModel.userId;
        pro.emailId = 0;
        WebService.Instance.Send<Web_S_EmailInfo>(AppConfig.url_Email, pro, OnGetEmailResult);

        opCode = pro.type;
    }

    void OpenEmailList()
    {
        emailList.gameObject.SetActive(true);
        emailList.GetComponent<TweenScale>().PlayForward();
        emailList.GetComponent<TweenAlpha>().PlayForward();
        itemEmail.transform.parent.GetComponent<UIScrollView>().ResetPosition();
    }

    void OpenEmailInfo(int index)
    {
        emailInfo.gameObject.SetActive(true);
        emailInfo.GetComponent<TweenScale>().PlayForward();
        emailInfo.GetComponent<TweenAlpha>().PlayForward();

        lblTitle.text = emailInfoList[index].EmailTitle;
        lblContent.text = emailInfoList[index].EmailContent;
        lblDateTime.text = emailInfoList[index].AddTime;
    }

    void Close()
    {
        CloseEmailList();
        UnSpawnEmailList();
        shade.GetComponent<TweenAlpha>().PlayReverse();
        DoActionDelay(CloseCor, 0.4f);
    }

    void CloseEmailList()
    {
        emailList.GetComponent<TweenScale>().PlayReverse();
        emailList.GetComponent<TweenAlpha>().PlayReverse();
        DoActionDelay(delegate { emailList.gameObject.SetActive(false); }, 0.2f);
    }

    void CloseEmailInfo()
    {
        emailInfo.GetComponent<TweenScale>().PlayReverse();
        emailInfo.GetComponent<TweenAlpha>().PlayReverse();
        DoActionDelay(delegate { emailInfo.gameObject.SetActive(false); }, 0.2f);
    }

    public void CloseCor()
    {
        gameObject.SetActive(false);
    }

    void OnGetEmailResult(Web_S_EmailInfo pro)
    {
        if (pro == null)
        {
            DoAction(GameEvent.V_OpenShortTip, "请求邮件信息失败！");
        }
        else
        {
            if (pro.return_code != 10000)
            {
                if (opCode == 1)
                {
                    DoAction(GameEvent.V_OpenShortTip, "请求邮件信息失败！");
                }
            }
            else
            {
                if (opCode == 1)
                {
                    emailInfoList.Clear();
                    emailItemList.Clear();
                    emailInfoList = pro.return_message;

                    for (int i = 0; i < emailInfoList.Count; i++)
                    {
                        GameObject obj = PoolManager.Instance.Spawn(itemEmail);
                        obj.name = "itemEmail_" + i;
                        obj.transform.parent = itemEmail.transform.parent;
                        obj.transform.localScale = Vector3.one;
                        obj.transform.localPosition = new Vector3(0f, 200f - 110f * emailItemList.Count, 0f);

                        obj.transform.Find("lblTitle").GetComponent<UILabel>().text = emailInfoList[i].EmailTitle;
                        if (emailInfoList[i].EmailContent.Length > 20)
                        {
                            obj.transform.Find("lblContent").GetComponent<UILabel>().text = emailInfoList[i].EmailContent.Remove(20) + "...";
                        }
                        else
                        {
                            obj.transform.Find("lblContent").GetComponent<UILabel>().text = emailInfoList[i].EmailContent;
                        }
                        obj.transform.Find("lblDataTime").GetComponent<UILabel>().text = emailInfoList[i].AddTime;
                        obj.transform.Find("flagRead").gameObject.SetActive(emailInfoList[i].IsRead == 1);
                        obj.GetComponent<UI2DSprite>().alpha = emailInfoList[i].IsRead == 1 ? 0.8f : 1f;

                        emailItemList.Add(obj);
                        UIEventListener.Get(obj).onClick = OnBtnEmailClick;
                    }

                    itemEmail.transform.parent.GetComponent<UIScrollView>().ResetPosition();
                }
                else if (opCode == 2)
                {
                    //阅读邮件成功！
                }
            }
        }
    }

    void UnSpawnEmailList()
    {
        emailInfoList.Clear();
        for (int i = 0; i < emailItemList.Count; i++)
        {
            emailItemList[i].name = "itemEmail";
            PoolManager.Instance.Unspawn(emailItemList[i]);
        }
        emailItemList.Clear();
    }


    #endregion

    #region UI响应

    public void OnBtnCloseClick_list()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonClose);
        Close();
    }

    public void OnBtnCloseClick_info()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonClose);
        CloseEmailInfo();
        DoActionDelay(OpenEmailList, 0.1f);
    }

    public void OnBtnEmailClick(GameObject obj)
    {
        int index = int.Parse(obj.name.Split('_')[1]);
        CloseEmailList();
        DoActionDelay(OpenEmailInfo, 0.1f, index);
        //阅读邮件
        Web_C_EmailInfo pro = new Web_C_EmailInfo();
        pro.type = 2;
        pro.userId = HallModel.userId;
        pro.emailId = emailInfoList[index].EmailId;
        WebService.Instance.Send<Web_S_EmailInfo>(AppConfig.url_Email, pro, OnGetEmailResult);

        opCode = pro.type;
    }


    #endregion
}
