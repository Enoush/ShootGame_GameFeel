using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubPool
{
    private List<GameObject> m_objects = new List<GameObject>();

    GameObject objPrefab;

    public string Name => objPrefab.name;

    public SubPool(GameObject go) 
    {
        this.objPrefab = go;
    }

    public GameObject OnSpawn()
    {
        GameObject gameObject = null;
        foreach (GameObject item in m_objects) 
        {
            if (item.activeSelf == false)
            {
                gameObject = item;
                break;
            }
        }
        if (gameObject == null) 
        {
            gameObject = GameObject.Instantiate(objPrefab);
            m_objects.Add(gameObject);
        }

        gameObject.SetActive(true);
        gameObject.SendMessage("OnSpawn",SendMessageOptions.DontRequireReceiver);
        return gameObject;
    }

    public void OnUnspawn(GameObject go) 
    {
        if (Contains(go))
        {
            go.SendMessage("OnUnspawn", SendMessageOptions.DontRequireReceiver);

            go.SetActive(false);
        }
    }

    public void OnUnspawnAll()
    {
        foreach (GameObject item in m_objects) 
        {
            if (item.activeSelf == true) 
            {
                OnUnspawn(item);
            }
        }
    }

    public bool Contains(GameObject go) 
    {
        return m_objects.Contains(go);
    }

    public void ClearList() 
    {
        m_objects.Clear();
    }
}
