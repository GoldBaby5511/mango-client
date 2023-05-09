using UnityEngine;
using System.Collections;
using DG.Tweening;

public class PnlSystemMsg : View
{

    private UILabel lblMsg;

    public override void Init()
    {
        lblMsg = transform.Find("bg/Panel/Label").GetComponent<UILabel>();

        gameObject.SetActive(false);
    }

    public override void RegisterAction()
    {
        GameEvent.S_ReceiveSystemMsg += OnReceiveSystemMsg;        
    }

    public override void RemoveAction()
    {
        GameEvent.S_ReceiveSystemMsg -= OnReceiveSystemMsg;    
    }

    void OnReceiveSystemMsg()
    {
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
            StartCoroutine(PlaySystemMessage());
        }
    }

    IEnumerator PlaySystemMessage()
    {
        string message = "";
        if (HallModel.messageList.Count > 0)
        {
            message = HallModel.messageList[0];
            HallModel.messageList.RemoveAt(0);
            yield return new WaitForEndOfFrame();
            lblMsg.text = message;
            float dis = 420f + lblMsg.localSize.x;
            float timer = dis / 100f;
            lblMsg.transform.localPosition = new Vector3(dis / 2, 0f, 0f);
            lblMsg.transform.DOLocalMoveX(-dis / 2, timer).SetEase(Ease.Linear);
            yield return new WaitForSeconds(timer);
            StartCoroutine(PlaySystemMessage());
        }
        else
        {
            message = HallModel.defaultMessage;
            gameObject.SetActive(false);
            yield return new WaitForEndOfFrame();
        }
        
    }
}
