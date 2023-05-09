using UnityEngine;
using System.Collections;
using UnityEngine.Events;


public abstract class View : MonoBehaviour 
{
    public abstract void Init();
    public abstract void RegisterAction();
    public abstract void RemoveAction();

    protected void DoAction(UnityAction action)
    {
        if (action != null)
        {
            action();
        }
    }


    protected void DoAction<T>(UnityAction<T> action, T t)
    {
        if (action != null)
        {
            action.Invoke(t);
        }
    }




    protected void DoActionDelay(UnityAction action, float delay)
    {
        StartCoroutine(DoActionDelayCor(action, delay));
    }

    IEnumerator DoActionDelayCor(UnityAction action, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (action != null)
        {
            action();
        }
    }




    protected void DoActionDelay<T>(UnityAction<T> action, float delay, T t)
    {
        StartCoroutine(DoActionDelayCor(action, delay, t));
    }

    IEnumerator DoActionDelayCor<T>(UnityAction<T> action, float delay, T t)
    {
        yield return new WaitForSeconds(delay);
        if (action != null)
        {
            action(t);
        }
    }

}
