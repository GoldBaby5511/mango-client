using UnityEngine;
using System.Collections;

public class ActionCenter : MonoBehaviour
{
    private View[] views;

    void Awake()
    {
        views = GetComponentsInChildren<View>(true);
        for (int i = 0; i < views.Length; i++)
        {
            views[i].Init();
            views[i].RegisterAction();
        }
    }

    void OnDestroy()
    {
        for (int i = 0; i < views.Length; i++)
        {
            views[i].RemoveAction();
        }
    }
}
