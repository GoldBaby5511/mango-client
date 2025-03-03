using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class DlgSet : View
{
    private Transform bg;
    private Transform shade;

    private UIButton btnMusic;
    private UIButton btnSound;

    private UITexture userPhoto;
    private UILabel lblUserName;
    private UIButton btnSwitchAccount;

    private UIButton btnClose;

    public override void Init()
    {
        bg = transform.Find("bg");
        shade = transform.Find("shade");

        btnMusic = transform.Find("bg/btnMusic").GetComponent<UIButton>();
        btnSound = transform.Find("bg/btnSound").GetComponent<UIButton>();
        btnSwitchAccount = transform.Find("bg/btnSwitchAccount").GetComponent<UIButton>();
        userPhoto = transform.Find("bg/sptPhoto").GetComponent<UITexture>();
        lblUserName = transform.Find("bg/lblUserName").GetComponent<UILabel>();

        btnClose = transform.Find("bg/btnClose").GetComponent<UIButton>();

        EventDelegate.Add(btnMusic.onClick, OnBtnMusicClick);
        EventDelegate.Add(btnSound.onClick, OnBtnSoundClick);
        EventDelegate.Add(btnSwitchAccount.onClick, OnBtnSwitchAccountClick);
        EventDelegate.Add(btnClose.onClick, OnBtnCloseClick);

        gameObject.SetActive(false);
        bg.GetComponent<TweenScale>().ResetToBeginning();
        bg.GetComponent<TweenAlpha>().ResetToBeginning();
        shade.GetComponent<TweenAlpha>().ResetToBeginning();
    }

    public override void RegisterAction()
    {
        GameEvent.V_OpenDlgSet += Open;
    }

    public override void RemoveAction()
    {
        GameEvent.V_OpenDlgSet -= Open;
    }

    #region UI操作

    public void Open()
    {
        gameObject.SetActive(true);
        bg.GetComponent<TweenScale>().PlayForward();
        bg.GetComponent<TweenAlpha>().PlayForward();
        shade.GetComponent<TweenAlpha>().PlayForward();

        if (SceneManager.GetActiveScene().name.ToLower().Contains("hall"))
        {
            btnSwitchAccount.gameObject.SetActive(true);
        }
        else
        {
            btnSwitchAccount.gameObject.SetActive(false);
        }

        RefreshSetInfo();
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

    void RefreshSetInfo()
    {
        if (HallModel.userPhotos.ContainsKey(HallModel.userId))
        {
            userPhoto.mainTexture = HallModel.userPhotos[HallModel.userId];
        }
        else
        {
            userPhoto.mainTexture = HallModel.defaultPhoto;
        }
        lblUserName.text = HallModel.userName;

        btnMusic.GetComponent<UISprite>().spriteName = AudioManager.Instance.IsPlayMusic ? "switch_open" : "switch_close";
        btnSound.GetComponent<UISprite>().spriteName = AudioManager.Instance.IsPlaySound ? "switch_open" : "switch_close";

        btnMusic.GetComponent<UIButton>().normalSprite = AudioManager.Instance.IsPlayMusic ? "switch_open" : "switch_close";
        btnSound.GetComponent<UIButton>().normalSprite = AudioManager.Instance.IsPlaySound ? "switch_open" : "switch_close";
    }

    #endregion

    #region UI响应

    public void OnBtnCloseClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonClose);
        Close();
    }

    //音乐开关
    public void OnBtnMusicClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonSwitch);
        AudioManager.Instance.IsPlayMusic = !AudioManager.Instance.IsPlayMusic;
        RefreshSetInfo();
    }

    //音效开关
    public void OnBtnSoundClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonSwitch);
        AudioManager.Instance.IsPlaySound = !AudioManager.Instance.IsPlaySound;
        RefreshSetInfo();
    }

    //切换账号
    public void OnBtnSwitchAccountClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonNormal);
        HallService.Instance.LoginOut();

        Close();
        HallModel.isSwitchAccount = true;
        HallService.Instance.BreakConnect();
        DoAction(HallEvent.V_ClosePnlMain);
        DoActionDelay(HallEvent.V_OpenPnlLogin, 0.3f);
    }

    #endregion
}
