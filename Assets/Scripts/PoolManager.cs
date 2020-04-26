using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolData
{
    public GameObject fatherObj;
    public List<GameObject> poolList;

    public PoolData(GameObject obj,  GameObject poolObj)
    {
        fatherObj = new GameObject(obj.name);
        fatherObj.transform.parent = poolObj.transform;
        poolList = new List<GameObject>(){};
        PushObj(obj);
    }

    public void PushObj(GameObject gameobj)
    {
        poolList.Add(gameobj);
        gameobj.transform.parent = fatherObj.transform;
        gameobj.SetActive(false);
    }

    public GameObject GetObj()
    {
        GameObject gameobj = null;
        gameobj = poolList[0];
        poolList.RemoveAt(0);
        gameobj.transform.parent = null;
        gameobj.SetActive(true);
        return gameobj;
    }
}

public class PoolManager : BaseManager<PoolManager>
{
    public Dictionary<string, PoolData> pool = new Dictionary<string, PoolData>();
    private GameObject poolObj;
    public GameObject GetObj(string name)
    {
        GameObject gameobj = null;
        
        if(pool.ContainsKey(name) && pool[name].poolList.Count > 0)
        {
            gameobj = pool[name].GetObj();
        }
        else
        {
            gameobj = GameObject.Instantiate(Resources.Load<GameObject>(name));
            gameobj.name = name;
        }
        return gameobj;
    }

    public void PushObj(string name, GameObject gameobj)
    {
        if(poolObj == null)
            poolObj = new GameObject("Pool");

        if(pool.ContainsKey(name))
        {
            pool[name].PushObj(gameobj);
        }
        else
        {
            pool.Add(name, new PoolData(gameobj, poolObj));
        }
    }

    public void Clear()
    {
        pool.Clear();
        poolObj = null;
    }
}