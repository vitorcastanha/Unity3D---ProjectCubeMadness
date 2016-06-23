using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PoolManager : MonoBehaviour 
{
    private const string ASSET_PATH = "ObjectPool";                     //ObjectPool location in the Resource folder
    private static Transform thisTransform;
    private static Dictionary<Type, Stack<PoolObject>> poolObjects;     //holds all poolObjects

    //make sure to run before Start gets called
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        thisTransform = transform;
        poolObjects = new Dictionary<Type, Stack<PoolObject>>();
        PreLoadAll();
    }

    private static void PreLoadAll()
    {
        //load all PoolObjects from resource into the poolObjects dictionary
        PoolObject[] objs = Resources.LoadAll<PoolObject>(ASSET_PATH);
        foreach (PoolObject obj in objs)
        {
            PreLoad(obj);
        }
    }

    private static void PreLoad(PoolObject obj)
    {
        //load specific PoolObject and push it into its dictionary stack
        Type type = obj.GetType();
        poolObjects.Add(type, new Stack<PoolObject>());                 //set up a new stack for the current type

        for (int i = 0; i < obj.nPreloads; i++)
        {
            PoolObject newObj = Instantiate(obj) as PoolObject;
            newObj.name = obj.name;                                     //remove (Clone)
            newObj.gameObject.SetActive(false);                         //make sure objects are not consuming resources
            newObj.transform.SetParent(thisTransform);
            poolObjects[type].Push(newObj);                             //push into the type stack
        }
    }

    public static PoolObject Spawn <T>(Transform parent = null) where T : PoolObject
    {
        //spawn object and pull it out of dictionary stack
        Type type = typeof(T);
        if (poolObjects[type].Count > 0)
        {
            PoolObject obj = poolObjects[type].Pop();                       //Pop it out of the stack
            obj.OnSpawn();                                                  //PoolObject version of Start()
            obj.transform.SetParent(parent);                                
            obj.gameObject.SetActive(true);                                 //Activate it
            return obj;
        }
        else
        {
            return null;
        }
    }

    public static void DeSpawn (GameObject go)
    {
        //despawn object and push it back into the dictionary stack
        PoolObject poolObj = go.GetComponent<PoolObject>();
        poolObj.OnDespawn();                                            //PoolObject OnDestroy version
        poolObj.transform.SetParent(thisTransform);                     //Get it back to the pool
        go.SetActive(false);                                            //turn it off to stop using resources
        Type type = poolObj.GetType();
        poolObjects[type].Push(poolObj);                                //Push it back into the stack
    }
}
