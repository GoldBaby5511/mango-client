using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class PlnWaitMatch : View
{
    public override void Init()
    {
        //lblMsg = transform.Find("bg/Panel/Label").GetComponent<UILabel>();

        gameObject.SetActive(false);
    }

    public override void RegisterAction()
    {
        GameEvent.V_ShowWaitMatch += OnShowWaitMatch;
    }

    public override void RemoveAction()
    {
        GameEvent.V_ShowWaitMatch -= OnShowWaitMatch;
    }

    void OnShowWaitMatch(bool show)
    {
        gameObject.SetActive(show);
    }
}
