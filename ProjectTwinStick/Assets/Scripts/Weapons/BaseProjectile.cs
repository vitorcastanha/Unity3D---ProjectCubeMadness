using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Rigidbody))]
public class BaseProjectile : PoolObject 
{
    [Header ("units per second:")]
    [SerializeField] private float fSpeed;
    [SerializeField] private float fLifeSpan = 4f;  //DeSpawn object after this many seconds
    [SerializeField] private float fDamage = 50f;

    protected Rigidbody rb;                         //Guarantee that Trigger functions will work

    public override void OnSpawn()
    {
        base.OnSpawn();
        Invoke("CleanProjectile", fLifeSpan);       //Clean from scene after delay
    }

	virtual protected void Start () 
    {
        rb = GetComponent<Rigidbody>();
	}
	
	virtual protected void Update () 
    {
        rb.velocity = transform.forward * fSpeed;
	}

    virtual protected void OnTriggerEnter(Collider col)
    {
        BaseEnemy enemy;
        //deals damage
        if (enemy = col.GetComponent<BaseEnemy>())
        {
            enemy.ModifyHealth(-fDamage); 
            CancelInvoke();
            CleanProjectile();
        }
    }
        
    //Clean the scene
    private void CleanProjectile()
    {
        PoolManager.DeSpawn(this.gameObject);
    }
}
