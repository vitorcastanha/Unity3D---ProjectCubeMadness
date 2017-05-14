using UnityEngine;
using System;
using System.Collections;

[RequireComponent (typeof(Rigidbody))]
public class BaseProjectile : PoolObject 
{
    [Header ("units per second:")]
    [SerializeField] private float fSpeed;
    [SerializeField] private float fLifeSpan = 4f;  //DeSpawn object after this many seconds
    [SerializeField] private float fDamage = 50f;
    public enum Owner
    {
        PLAYER_UNIT,
        ENEMY_UNIT
    }
    public Owner owner;

    protected Rigidbody rb;                         //Guarantee that Trigger functions will work

    public override void OnSpawn()
    {
        base.OnSpawn();
        Invoke("CleanProjectile", fLifeSpan);       //Clean from scene after delay
        if (owner == Owner.PLAYER_UNIT)
        {
            SpringArm.GetInstance().DoShakeCamera(0.1f, 0.1f);
        }
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
        if (col.tag == "Wall")
        {
            PoolManager.DeSpawn(gameObject);
        }

        BaseCharacter bcCharacter;

        //deals damage
        if (owner == Owner.PLAYER_UNIT && (bcCharacter = col.GetComponent<BaseEnemy>()))
        {
            ProjectileHit<BaseEnemy>(bcCharacter as BaseEnemy);
        }
        else if(owner == Owner.ENEMY_UNIT && (bcCharacter = col.GetComponent<HeroCharacter>()))
        {
            ProjectileHit<HeroCharacter>(bcCharacter as HeroCharacter);
        }
    }

    private void ProjectileHit<T>(T targetCharacter) where T : BaseCharacter
    {
        Type type = typeof(T);
        if (type == typeof(HeroCharacter))
            DamageHero(targetCharacter as HeroCharacter);
        else
            targetCharacter.StateHandler.DamageCharacter(fDamage);
        CancelInvoke();
        CleanProjectile();
    }

    private void DamageHero(HeroCharacter hero)
    {
        hero.HeroStateHandler.DamageCharacter(fDamage);
    }
        
    //Clean the scene
    private void CleanProjectile()
    {
        PoolManager.DeSpawn(this.gameObject);
    }
}
