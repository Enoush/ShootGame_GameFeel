using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool:SingletonMono<ObjectPool>
{
    public string resourcePath = "";

    public Dictionary<string, SubPool> poolsDir = new Dictionary<string, SubPool>();

    public GameObject OnSpawn(string name)
    {
        SubPool pool = null;

        if (!poolsDir.ContainsKey(name))
            RegisterNewPool(name);

        pool = poolsDir[name];

        return pool.OnSpawn();
    }

    public void OnUnspawn(GameObject go)
    {
        foreach (var item in poolsDir.Values) 
        {
            if (item.Contains(go)) 
            {
                item.OnUnspawn(go);
            }
        }
    }


    public void OnUnspawnAll()
    {
        foreach (var item in poolsDir) 
        {
            item.Value.OnUnspawnAll();
        }
    }

    /// <summary>
    /// 注册新的子池子
    /// </summary>
    /// <param name="name">新的子池子的名字</param>
    public void RegisterNewPool(string name) 
    {
        string prefabPath;

        //首先判断路径
        if (string.IsNullOrEmpty(resourcePath))
        {
            prefabPath = name;
        }
        else
        {
            prefabPath = resourcePath+"/"+ name;
        }

        GameObject gameObject = Resources.Load<GameObject>(prefabPath);
        SubPool subPool = new SubPool(gameObject);

        this.poolsDir.Add(name,subPool);
    }

    public void ClearAllPoolList() 
    {
        foreach (var item in poolsDir) 
        {
            item.Value.ClearList();
        }
    }
}
