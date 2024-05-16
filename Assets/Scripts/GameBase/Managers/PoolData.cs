using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// pool module
/// </summary>
public class PoolData
{
    //father object of objects in the list
    public GameObject fatherObject;
    //list that stores spare objects
    public List<GameObject> poolList;

    public PoolData(GameObject obj, GameObject rootPoolObject)
    {
        fatherObject = new GameObject(obj.name);
        fatherObject.transform.parent = rootPoolObject.transform;
        poolList = new List<GameObject>();
        PushObject(obj);
    }


    public void PushObject(GameObject obj)
    {
        obj.SetActive(false);
        poolList.Add(obj);
        obj.transform.parent = fatherObject.transform;
    }

    public GameObject GetGameObject()
    {
        GameObject obj = poolList[0];
        poolList.RemoveAt(0);
        obj.SetActive(true);
        obj.transform.parent = null;
        return obj;
    }

}
