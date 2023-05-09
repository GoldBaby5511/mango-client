using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DlgGPS : View
{
    private UIButton btnOpen;

    private Transform bg3;
    private Transform bg4;
    private Transform shade;

    private UITexture[] players = new UITexture[4];
    private Transform line_0_1;
    private Transform line_0_2;
    private Transform line_0_3;
    private Transform line_1_2;
    private Transform line_1_3;
    private Transform line_2_3;

    private UIButton btnClose3;
    private UIButton btnClose4;

    public override void Init()
    {
        btnOpen = transform.Find("btnOpen").GetComponent<UIButton>();

        bg3 = transform.Find("bg3");
        bg4 = transform.Find("bg4");
        shade = transform.Find("shade");

        btnClose3 = transform.Find("bg3/btnClose").GetComponent<UIButton>();
        btnClose4 = transform.Find("bg4/btnClose").GetComponent<UIButton>();

        EventDelegate.Add(btnOpen.onClick, OnBtnOpenClick);
        EventDelegate.Add(btnClose3.onClick, OnBtnCloseClick);
        EventDelegate.Add(btnClose4.onClick, OnBtnCloseClick);

        gameObject.SetActive(true);
        bg3.gameObject.SetActive(false);
        bg4.gameObject.SetActive(false);
        shade.gameObject.SetActive(false);
        bg3.GetComponent<TweenAlpha>().ResetToBeginning();
        bg4.GetComponent<TweenAlpha>().ResetToBeginning();
        shade.GetComponent<TweenAlpha>().ResetToBeginning();

        btnOpen.gameObject.SetActive(GameModel.serverType == GameModel.ServerKind_Private);
    }

    public override void RegisterAction()
    {
        
    }

    public override void RemoveAction()
    {
        
    }

    void Open()
    {
        if (GameModel.deskPlayerCount == 3)
        {
            for (int i = 0; i < 3; i++)
            {
                players[i] = transform.Find("bg3/photo_" + i).GetComponent<UITexture>();
                line_0_1 = transform.Find("bg3/line_0_1");
                line_0_2 = transform.Find("bg3/line_0_2");
                line_1_2 = transform.Find("bg3/line_1_2");
            }

            bg3.gameObject.SetActive(true);
            bg4.gameObject.SetActive(false);
            shade.gameObject.SetActive(true);
            bg3.GetComponent<TweenAlpha>().PlayForward();
            shade.GetComponent<TweenAlpha>().PlayForward();
            //显示头像
            for (int i = 0; i < 3; i++)
            {
                players[i].mainTexture = GameModel.GetUserPhoto(i);
            }
            //0-1
            if (GameModel.GetDeskUser(0) != null && GameModel.GetDeskUser(1) != null)
            {
                if (GameModel.GetUserGPSPos(0) != Vector2.zero && GameModel.GetUserGPSPos(1) != Vector2.zero)
                {
                    float dis = GetDis(GameModel.GetUserGPSPos(0), GameModel.GetUserGPSPos(1));
                    if (dis < 100)
                    {
                        line_0_1.GetComponent<UISprite>().color = new Color(156 / 255f, 29 / 255f, 29 / 255f);
                        line_0_1.Find("Label").GetComponent<UILabel>().color = new Color(156 / 255f, 29 / 255f, 29 / 255f);
                    }
                    else
                    {
                        line_0_1.GetComponent<UISprite>().color = new Color(0 / 255f, 189 / 255f, 103 / 255f);
                        line_0_1.Find("Label").GetComponent<UILabel>().color = new Color(0 / 255f, 189 / 255f, 103 / 255f);
                    }
                    line_0_1.Find("Label").GetComponent<UILabel>().text = dis.ToString("f1") + "米";
                }
                else
                {
                    line_0_1.GetComponent<UISprite>().color = Color.gray;
                    line_0_1.Find("Label").GetComponent<UILabel>().color = Color.gray;
                    line_0_1.Find("Label").GetComponent<UILabel>().text = "未知";
                }
            }
            else
            {
                line_0_1.GetComponent<UISprite>().color = Color.gray;
                line_0_1.Find("Label").GetComponent<UILabel>().color = Color.gray;
                line_0_1.Find("Label").GetComponent<UILabel>().text = "未知";
            }
            //0-2
            if (GameModel.GetDeskUser(0) != null && GameModel.GetDeskUser(2) != null)
            {
                if (GameModel.GetUserGPSPos(0) != Vector2.zero && GameModel.GetUserGPSPos(2) != Vector2.zero)
                {
                    float dis = GetDis(GameModel.GetUserGPSPos(0), GameModel.GetUserGPSPos(2));
                    if (dis < 100)
                    {
                        line_0_2.GetComponent<UISprite>().color = new Color(156 / 255f, 29 / 255f, 29 / 255f);
                        line_0_2.Find("Label").GetComponent<UILabel>().color = new Color(156 / 255f, 29 / 255f, 29 / 255f);
                    }
                    else
                    {
                        line_0_2.GetComponent<UISprite>().color = new Color(0 / 255f, 189 / 255f, 103 / 255f);
                        line_0_2.Find("Label").GetComponent<UILabel>().color = new Color(0 / 255f, 189 / 255f, 103 / 255f);
                    }
                    line_0_2.Find("Label").GetComponent<UILabel>().text = dis.ToString("f1") + "米";
                }
                else
                {
                    line_0_2.GetComponent<UISprite>().color = Color.gray;
                    line_0_2.Find("Label").GetComponent<UILabel>().color = Color.gray;
                    line_0_2.Find("Label").GetComponent<UILabel>().text = "未知";
                }
            }
            else
            {
                line_0_2.GetComponent<UISprite>().color = Color.gray;
                line_0_2.Find("Label").GetComponent<UILabel>().color = Color.gray;
                line_0_2.Find("Label").GetComponent<UILabel>().text = "未知";
            }
            //1-2
            if (GameModel.GetDeskUser(1) != null && GameModel.GetDeskUser(2) != null)
            {
                if (GameModel.GetUserGPSPos(1) != Vector2.zero && GameModel.GetUserGPSPos(2) != Vector2.zero)
                {
                    float dis = GetDis(GameModel.GetUserGPSPos(1), GameModel.GetUserGPSPos(2));
                    if (dis < 100)
                    {
                        line_1_2.GetComponent<UISprite>().color = new Color(156 / 255f, 29 / 255f, 29 / 255f);
                        line_1_2.Find("Label").GetComponent<UILabel>().color = new Color(156 / 255f, 29 / 255f, 29 / 255f);
                    }
                    else
                    {
                        line_1_2.GetComponent<UISprite>().color = new Color(0 / 255f, 189 / 255f, 103 / 255f);
                        line_1_2.Find("Label").GetComponent<UILabel>().color = new Color(0 / 255f, 189 / 255f, 103 / 255f);
                    }
                    line_1_2.Find("Label").GetComponent<UILabel>().text = dis.ToString("f1") + "米";
                }
                else
                {
                    line_1_2.GetComponent<UISprite>().color = Color.gray;
                    line_1_2.Find("Label").GetComponent<UILabel>().color = Color.gray;
                    line_1_2.Find("Label").GetComponent<UILabel>().text = "未知";
                }
            }
            else
            {
                line_1_2.GetComponent<UISprite>().color = Color.gray;
                line_1_2.Find("Label").GetComponent<UILabel>().color = Color.gray;
                line_1_2.Find("Label").GetComponent<UILabel>().text = "未知";
            }
        }
        else if (GameModel.deskPlayerCount == 4)
        {
            for (int i = 0; i < 4; i++)
            {
                players[i] = transform.Find("bg4/photo_" + i).GetComponent<UITexture>();
                line_0_1 = transform.Find("bg4/line_0_1");
                line_0_2 = transform.Find("bg4/line_0_2");
                line_0_3 = transform.Find("bg4/line_0_3");
                line_1_2 = transform.Find("bg4/line_1_2");
                line_1_3 = transform.Find("bg4/line_1_3");
                line_2_3 = transform.Find("bg4/line_2_3");
            }

            bg3.gameObject.SetActive(false);
            bg4.gameObject.SetActive(true);
            shade.gameObject.SetActive(true);
            bg4.GetComponent<TweenAlpha>().PlayForward();
            shade.GetComponent<TweenAlpha>().PlayForward();
            //显示头像
            for (int i = 0; i < 4; i++)
            {
                players[i].mainTexture = GameModel.GetUserPhoto(i);
            }
            //0-1
            if (GameModel.GetDeskUser(0) != null && GameModel.GetDeskUser(1) != null)
            {
                if (GameModel.GetUserGPSPos(0) != Vector2.zero && GameModel.GetUserGPSPos(1) != Vector2.zero)
                {
                    float dis = GetDis(GameModel.GetUserGPSPos(0), GameModel.GetUserGPSPos(1));
                    if (dis < 100)
                    {
                        line_0_1.GetComponent<UISprite>().color = new Color(156 / 255f, 29 / 255f, 29 / 255f);
                        line_0_1.Find("Label").GetComponent<UILabel>().color = new Color(156 / 255f, 29 / 255f, 29 / 255f);
                    }
                    else
                    {
                        line_0_1.GetComponent<UISprite>().color = new Color(0 / 255f, 189 / 255f, 103 / 255f);
                        line_0_1.Find("Label").GetComponent<UILabel>().color = new Color(0 / 255f, 189 / 255f, 103 / 255f);
                    }
                    line_0_1.Find("Label").GetComponent<UILabel>().text = dis.ToString("f1") + "米";
                }
                else
                {
                    line_0_1.GetComponent<UISprite>().color = Color.gray;
                    line_0_1.Find("Label").GetComponent<UILabel>().color = Color.gray;
                    line_0_1.Find("Label").GetComponent<UILabel>().text = "未知";
                }
            }
            else
            {
                line_0_1.GetComponent<UISprite>().color = Color.gray;
                line_0_1.Find("Label").GetComponent<UILabel>().color = Color.gray;
                line_0_1.Find("Label").GetComponent<UILabel>().text = "未知";
            }
            //0-2
            if (GameModel.GetDeskUser(0) != null && GameModel.GetDeskUser(2) != null)
            {
                if (GameModel.GetUserGPSPos(0) != Vector2.zero && GameModel.GetUserGPSPos(2) != Vector2.zero)
                {
                    float dis = GetDis(GameModel.GetUserGPSPos(0), GameModel.GetUserGPSPos(2));
                    if (dis < 100)
                    {
                        line_0_2.GetComponent<UISprite>().color = new Color(156 / 255f, 29 / 255f, 29 / 255f);
                        line_0_2.Find("Label").GetComponent<UILabel>().color = new Color(156 / 255f, 29 / 255f, 29 / 255f);
                    }
                    else
                    {
                        line_0_2.GetComponent<UISprite>().color = new Color(0 / 255f, 189 / 255f, 103 / 255f);
                        line_0_2.Find("Label").GetComponent<UILabel>().color = new Color(0 / 255f, 189 / 255f, 103 / 255f);
                    }
                    line_0_2.Find("Label").GetComponent<UILabel>().text = dis.ToString("f1") + "米";
                }
                else
                {
                    line_0_2.GetComponent<UISprite>().color = Color.gray;
                    line_0_2.Find("Label").GetComponent<UILabel>().color = Color.gray;
                    line_0_2.Find("Label").GetComponent<UILabel>().text = "未知";
                }
            }
            else
            {
                line_0_2.GetComponent<UISprite>().color = Color.gray;
                line_0_2.Find("Label").GetComponent<UILabel>().color = Color.gray;
                line_0_2.Find("Label").GetComponent<UILabel>().text = "未知";
            }
            //0-3
            if (GameModel.GetDeskUser(0) != null && GameModel.GetDeskUser(3) != null)
            {
                if (GameModel.GetUserGPSPos(0) != Vector2.zero && GameModel.GetUserGPSPos(3) != Vector2.zero)
                {
                    float dis = GetDis(GameModel.GetUserGPSPos(0), GameModel.GetUserGPSPos(3));
                    if (dis < 100)
                    {
                        line_0_3.GetComponent<UISprite>().color = new Color(156 / 255f, 29 / 255f, 29 / 255f);
                        line_0_3.Find("Label").GetComponent<UILabel>().color = new Color(156 / 255f, 29 / 255f, 29 / 255f);
                    }
                    else
                    {
                        line_0_3.GetComponent<UISprite>().color = new Color(0 / 255f, 189 / 255f, 103 / 255f);
                        line_0_3.Find("Label").GetComponent<UILabel>().color = new Color(0 / 255f, 189 / 255f, 103 / 255f);
                    }
                    line_0_3.Find("Label").GetComponent<UILabel>().text = dis.ToString("f1") + "米";
                }
                else
                {
                    line_0_3.GetComponent<UISprite>().color = Color.gray;
                    line_0_3.Find("Label").GetComponent<UILabel>().color = Color.gray;
                    line_0_3.Find("Label").GetComponent<UILabel>().text = "未知";
                }
            }
            else
            {
                line_0_3.GetComponent<UISprite>().color = Color.gray;
                line_0_3.Find("Label").GetComponent<UILabel>().color = Color.gray;
                line_0_3.Find("Label").GetComponent<UILabel>().text = "未知";
            }
            //1-2
            if (GameModel.GetDeskUser(1) != null && GameModel.GetDeskUser(2) != null)
            {
                if (GameModel.GetUserGPSPos(1) != Vector2.zero && GameModel.GetUserGPSPos(2) != Vector2.zero)
                {
                    float dis = GetDis(GameModel.GetUserGPSPos(1), GameModel.GetUserGPSPos(2));
                    if (dis < 100)
                    {
                        line_1_2.GetComponent<UISprite>().color = new Color(156 / 255f, 29 / 255f, 29 / 255f);
                        line_1_2.Find("Label").GetComponent<UILabel>().color = new Color(156 / 255f, 29 / 255f, 29 / 255f);
                    }
                    else
                    {
                        line_1_2.GetComponent<UISprite>().color = new Color(0 / 255f, 189 / 255f, 103 / 255f);
                        line_1_2.Find("Label").GetComponent<UILabel>().color = new Color(0 / 255f, 189 / 255f, 103 / 255f);
                    }
                    line_1_2.Find("Label").GetComponent<UILabel>().text = dis.ToString("f1") + "米";
                }
                else
                {
                    line_1_2.GetComponent<UISprite>().color = Color.gray;
                    line_1_2.Find("Label").GetComponent<UILabel>().color = Color.gray;
                    line_1_2.Find("Label").GetComponent<UILabel>().text = "未知";
                }
            }
            else
            {
                line_1_2.GetComponent<UISprite>().color = Color.gray;
                line_1_2.Find("Label").GetComponent<UILabel>().color = Color.gray;
                line_1_2.Find("Label").GetComponent<UILabel>().text = "未知";
            }
            //1-3
            if (GameModel.GetDeskUser(1) != null && GameModel.GetDeskUser(3) != null)
            {
                if (GameModel.GetUserGPSPos(1) != Vector2.zero && GameModel.GetUserGPSPos(3) != Vector2.zero)
                {
                    float dis = GetDis(GameModel.GetUserGPSPos(1), GameModel.GetUserGPSPos(3));
                    if (dis < 100)
                    {
                        line_1_3.GetComponent<UISprite>().color = new Color(156 / 255f, 29 / 255f, 29 / 255f);
                        line_1_3.Find("Label").GetComponent<UILabel>().color = new Color(156 / 255f, 29 / 255f, 29 / 255f);
                    }
                    else
                    {
                        line_1_3.GetComponent<UISprite>().color = new Color(0 / 255f, 189 / 255f, 103 / 255f);
                        line_1_3.Find("Label").GetComponent<UILabel>().color = new Color(0 / 255f, 189 / 255f, 103 / 255f);
                    }
                    line_1_3.Find("Label").GetComponent<UILabel>().text = dis.ToString("f1") + "米";
                }
                else
                {
                    line_1_3.GetComponent<UISprite>().color = Color.gray;
                    line_1_3.Find("Label").GetComponent<UILabel>().color = Color.gray;
                    line_1_3.Find("Label").GetComponent<UILabel>().text = "未知";
                }
            }
            else
            {
                line_1_3.GetComponent<UISprite>().color = Color.gray;
                line_1_3.Find("Label").GetComponent<UILabel>().color = Color.gray;
                line_1_3.Find("Label").GetComponent<UILabel>().text = "未知";
            }
            //2-3
            if (GameModel.GetDeskUser(2) != null && GameModel.GetDeskUser(3) != null)
            {
                if (GameModel.GetUserGPSPos(2) != Vector2.zero && GameModel.GetUserGPSPos(3) != Vector2.zero)
                {
                    float dis = GetDis(GameModel.GetUserGPSPos(2), GameModel.GetUserGPSPos(3));
                    if (dis < 100)
                    {
                        line_2_3.GetComponent<UISprite>().color = new Color(156 / 255f, 29 / 255f, 29 / 255f);
                        line_2_3.Find("Label").GetComponent<UILabel>().color = new Color(156 / 255f, 29 / 255f, 29 / 255f);
                    }
                    else
                    {
                        line_2_3.GetComponent<UISprite>().color = new Color(0 / 255f, 189 / 255f, 103 / 255f);
                        line_2_3.Find("Label").GetComponent<UILabel>().color = new Color(0 / 255f, 189 / 255f, 103 / 255f);
                    }
                    line_2_3.Find("Label").GetComponent<UILabel>().text = dis.ToString("f1") + "米";
                }
                else
                {
                    line_2_3.GetComponent<UISprite>().color = Color.gray;
                    line_2_3.Find("Label").GetComponent<UILabel>().color = Color.gray;
                    line_2_3.Find("Label").GetComponent<UILabel>().text = "未知";
                }
            }
            else
            {
                line_2_3.GetComponent<UISprite>().color = Color.gray;
                line_2_3.Find("Label").GetComponent<UILabel>().color = Color.gray;
                line_2_3.Find("Label").GetComponent<UILabel>().text = "未知";
            }
        }
    }

    void Close()
    {
        bg3.GetComponent<TweenAlpha>().PlayReverse();
        bg4.GetComponent<TweenAlpha>().PlayReverse();
        shade.GetComponent<TweenAlpha>().PlayReverse();

        DoActionDelay(CloseCor, 0.3f);
    }

    void CloseCor()
    {
        bg3.gameObject.SetActive(false);
        bg4.gameObject.SetActive(false);
        shade.gameObject.SetActive(false);
    }


    void OnBtnOpenClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonNormal);
        Open();
    }

    void OnBtnCloseClick()
    {
        AudioManager.Instance.PlaySound(GameModel.audioButtonClose);
        Close();
    }

    float GetDis(Vector2 pos1, Vector2 pos2)
    {
        float a, b, R;
        R = 6378137; //地球半径
        a = (pos1.y- pos2.y) * Mathf.PI / 180.0f;
        b = (pos1.x - pos2.x) * Mathf.PI / 180.0f;
        float d = 0;
        float sa2, sb2;
        sa2 = Mathf.Sin(a / 2.0f);
        sb2 = Mathf.Sin(b / 2.0f);
        d = 2 * R * Mathf.Asin(Mathf.Sqrt(sa2 * sa2 + Mathf.Cos(pos1.y) * Mathf.Cos(pos2.y) * sb2 * sb2));
        return d;
    }
}
