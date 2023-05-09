using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 对象池，子池
/// </summary>
public class ObjectPool 
{

    public GameObject prefab;   //对象池元素

    private int maxSize;        // 对象池最大容量，此值小于等于0时，对象池动态增加，不设上限

    /// <summary>
    /// 对象池名称
    /// </summary>
    public string Name
    {
        get { return prefab.name; }
    }
    
    /// <summary>
    /// 对象池当前容量
    /// </summary>
    private int CurrentSize
    {
        get { return activeObjectList.Count + inactiveObjectList.Count; }
    }

    private List<GameObject> activeObjectList = new List<GameObject>();      //已用对象列表
    private List<GameObject> inactiveObjectList = new List<GameObject>();    //待用对象列表



    public ObjectPool(GameObject obj)
    {
        this.prefab = obj;
        maxSize = -1;
    }

    public ObjectPool(GameObject obj, int maxCount)
    {
        this.prefab = obj;
        maxSize = maxCount;
    }

    /// <summary>
    /// 初始化对象池
    /// </summary>
    public void InitPool(int initSize)
    {
        if (maxSize > 0 && initSize > maxSize)
        {
            Debug.LogError("对象池初始容量不能超过其最大容量！！");
            for (int i = 0; i < maxSize; i++)
            {
                GameObject go = GameObject.Instantiate(prefab);
                go.name = prefab.name;
                go.SetActive(false);
                inactiveObjectList.Add(go);
            }
        }
        else
        {
            for (int i = 0; i < initSize; i++)
            {
                GameObject go = GameObject.Instantiate(prefab);
                go.name = prefab.name;
                go.SetActive(false);
                inactiveObjectList.Add(go);
            }
        }
    }

    /// <summary>
    /// 取对象
    /// </summary>
    public GameObject Spawn()
    {
        if (inactiveObjectList.Count == 0)
        {
            if (maxSize > 0 && CurrentSize >= maxSize)
            {
                Debug.LogError("警告：对象池 " + Name + " 当前容量已达到最大设定值 : " + maxSize);
                GameObject go = activeObjectList[0];
                return go;
            }
            else
            {
                GameObject go = GameObject.Instantiate(prefab);
                go.name = prefab.name;
                go.SetActive(true);
                activeObjectList.Add(go);
                return go;
            }
        }
        else
        {
            GameObject go = inactiveObjectList[0];
            go.SetActive(true);
            inactiveObjectList.RemoveAt(0);
            activeObjectList.Add(go);
            return go;
        }
    }

    /// <summary>
    /// 回收对象
    /// </summary>
    public void Unspawn(GameObject go)
    {
        if (activeObjectList.Contains(go))
        {
            go.SetActive(false);
            activeObjectList.Remove(go);
            inactiveObjectList.Add(go);
        }
        else
        {
            Debug.LogError("回收对象异常，当前对象不在对象列表中!!");
        }
    }

    /// <summary>
    /// 回收池中所有对象
    /// </summary>
    public void UnspawnAll()
    {
        foreach (GameObject go in activeObjectList)
        {
            Unspawn(go);
        }
    }


}
