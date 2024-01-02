using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//单例基类
public class BaseManager<T> where T:new()
{
    private static T instance;

    public static T GetInstance()
    {
        if (instance == null)
            instance = new T();
        return instance;
    }
}

