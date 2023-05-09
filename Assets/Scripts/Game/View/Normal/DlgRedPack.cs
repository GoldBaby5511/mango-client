using UnityEngine;
using System.Collections;

public class DlgRedPack : View
{
    private GameObject effect;

    private GameObject RedPack;
    private GameObject[] effectSprite = new GameObject[5];

    private UILabel lblRedPackCount;
    private UILabel lblTip;

    public override void Init()
    {
        effect = transform.Find("effect").gameObject;

        RedPack = transform.Find("RedPack").gameObject;
        for (int i = 0; i < 5; i++)
        {
            effectSprite[i] = transform.Find("RedPack/Sprite_" + i).gameObject;
        }

        lblTip = transform.Find("RedPack/lblTip").GetComponent<UILabel>();
        lblRedPackCount = transform.Find("RedPack/lblRedCount").GetComponent<UILabel>();

        

        gameObject.SetActive(true);
        effect.SetActive(false);
        Close();
    }

    public override void RegisterAction()
    {
        GameEvent.V_OpenRedPackButton += OpenRedPackButton;
        GameEvent.V_PlayRedPackAnim += OpenRedPackEffect;
       
    }

    public override void RemoveAction()
    {
        GameEvent.V_OpenRedPackButton -= OpenRedPackButton;
        GameEvent.V_PlayRedPackAnim -= OpenRedPackEffect;
    }

    

    void OpenRedPackButton(CMD_Game_S_GetRedPack pro)
    {
        bool isEnable = (pro.dwUserID[GameModel.chairId] == HallModel.userId);
        effect.SetActive(true);
        Invoke("CloseRedPackEffect", 8f);
    }

    public void CloseRedPackEffect()
    {
        effect.SetActive(false);
    }



    void OpenRedPackEffect(float value, string msg)
    {
        lblRedPackCount.text = "x " + value;
        lblTip.text = msg;
        StartCoroutine(RedPackAnim());
    }

    IEnumerator RedPackAnim()
    {
        RedPack.SetActive(true);
        effectSprite[0].SetActive(true);
        yield return new WaitForSeconds(0.3f);
        effectSprite[0].SetActive(false);
        effectSprite[1].SetActive(true);
        effectSprite[2].SetActive(true);
        yield return new WaitForSeconds(0.2f);
        effectSprite[3].SetActive(true);
        effectSprite[3].GetComponent<TweenScale>().PlayForward();
        yield return new WaitForSeconds(0.3f);
        effectSprite[4].SetActive(true);
        effectSprite[4].GetComponent<TweenScale>().PlayForward();
        yield return new WaitForSeconds(0.2f);
        AudioManager.Instance.PlaySound(GameModel.audioGetAward);
        lblRedPackCount.gameObject.SetActive(true);
        lblTip.gameObject.SetActive(true);
        lblRedPackCount.GetComponent<TweenAlpha>().PlayForward();
        yield return new WaitForSeconds(2f);
        Close();
    }

    

    public void Close()
    {
        effect.SetActive(false);
        for (int i = 0; i < 5; i++)
        {
            effectSprite[i].SetActive(false);
        }
        lblRedPackCount.gameObject.SetActive(false);
        lblTip.gameObject.SetActive(false);

        effectSprite[3].GetComponent<TweenScale>().ResetToBeginning();
        effectSprite[4].GetComponent<TweenScale>().ResetToBeginning();
        lblRedPackCount.GetComponent<TweenAlpha>().ResetToBeginning();
    }

    

}
