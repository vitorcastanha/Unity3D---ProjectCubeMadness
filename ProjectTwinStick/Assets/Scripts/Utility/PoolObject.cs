using UnityEngine;
using System.Collections;

public class PoolObject : MonoBehaviour {

    public int nPreloads;               //how many of this unit will be stored in the pool

    virtual public void OnSpawn()       //Play when the object spawns
    {
        
    }

    virtual public void OnDespawn()     //Play when the object despawns
    {
        //reset stats here
    }
}
