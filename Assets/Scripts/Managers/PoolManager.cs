using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PoolManager : MonoBehaviour 
{

    private static PoolManager _instance;
    public static PoolManager Instance
    {
        get
        {
            if (_instance == null)
            {
                string name = "PoolManager";
                GameObject manager = GameObject.Find(name);
                if (manager == null)
                {
                    manager = new GameObject(name);
                    //DontDestroyOnLoad(manager);
                    _instance = manager.AddComponent<PoolManager>();
                }
                else
                {
                    _instance = manager.GetComponent<PoolManager>();
                    if (_instance == null)
                    {
                        _instance = manager.AddComponent<PoolManager>();
                    }
                }
            }
            return _instance;
        }
    }

    private Dictionary<string,ObjectPool> pools = new Dictionary<string,ObjectPool>();


    /// <summary>
    /// 取对象，通过对象实例
    /// </summary>
    public GameObject Spawn(GameObject go)
    {
        if (!pools.ContainsKey(go.name))
        {
            CreateNewPool(go);
        }
        return pools[go.name].Spawn();
    }

    /// <summary>
    /// 通过名称取对象
    /// </summary>
    public GameObject Spawn(string name)
    {
        if (pools.ContainsKey(name))
        {
            return pools[name].Spawn();
        }
        else
        {
            Debug.LogError("无法通过名称取得对象实例，该实例对象池不存在，请先创建该实例对象池，或通过实例取得对象");
            return null;
        }
    }

    /// <summary>
    /// 回收对象
    /// </summary>
    public void Unspawn(GameObject go)
    {
        if (pools.ContainsKey(go.name))
        {
            pools[go.name].Unspawn(go);
        }
        else
        {
            Debug.LogError("回收对象失败，没有相应的对象池！");
        }
    }

    /// <summary>
    /// 回收某个对象池中所有对象
    /// </summary>
    public void UnspawnPool(string name)
    {
        if (pools.ContainsKey(name))
        {
            pools[name].UnspawnAll();
        }
        else
        {
            Debug.LogError("回收某个对象池中所有对象失败，没有该对象池！");
        }
    }

    /// <summary>
    /// 回收某个对象池中所有对象
    /// </summary>
    public void UnspawnPool(GameObject go)
    {
        if (pools.ContainsKey(go.name))
        {
            pools[go.name].UnspawnAll();
        }
        else
        {
            Debug.LogError("回收某个对象池中所有对象失败，没有该对象池！");
        }
    }

    /// <summary>
    /// 回收所有对象池
    /// </summary>
    public void UnspawnAll()
    {
        foreach (ObjectPool pool in pools.Values)
        {
            pool.UnspawnAll();
        }
    }

    /// <summary>
    /// 创建一个新的对象池
    /// </summary>
    public void CreateNewPool(GameObject prefab)
    {
        if (pools.ContainsKey(prefab.name))
        {
            Debug.LogError("无法创建新的对象池，当前对象池已存在！！");
            return;
        }
        ObjectPool pool = new ObjectPool(prefab);
        pools.Add(pool.Name, pool);
    }

    /// <summary>
    /// 创建一个新的对象池，并初始化
    /// </summary>
    public void CreateNewPool(GameObject prefab, int initSize)
    {
        if (pools.ContainsKey(prefab.name))
        {
            Debug.LogError("无法创建新的对象池，当前对象池已存在！！");
            return;
        }
        ObjectPool pool = new ObjectPool(prefab);
        pool.InitPool(initSize);
        pools.Add(pool.Name,pool);
    }

    /// <summary>
    /// 创建一个新的对象池，并初始化，同时设定对象池最大容量
    /// </summary>
    public void CreateNewPool(GameObject prefab, int initSize, int maxSize)
    {
        if (pools.ContainsKey(prefab.name))
        {
            Debug.LogError("无法创建新的对象池，当前对象池已存在！！");
            return;
        }
        ObjectPool pool = new ObjectPool(prefab, maxSize);
        pool.InitPool(initSize);
        pools.Add(pool.Name, pool);
    }

}
