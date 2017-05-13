using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Animator))]
public class BaseGun : Weapon
{
    private const string ANIM_TRIGGER_RECOIL = "recoil";

    #region Designer Variables
    [SerializeField] protected Transform tProjectileSpawn;
    [SerializeField] protected float fShotsPerSecond = 2f;
    #endregion
    protected bool bReadyToFire = true;

    private Animator anim;

    virtual protected void Start()
    {
        //Base
        anim = GetComponent<Animator>();
    }

    virtual protected void Update()
    {
	    //Base
    }

    /// <summary>
    /// Do the attack.
    /// </summary>
    public override void DoAttack()
    {
        base.DoAttack();
        if (bReadyToFire)
        {
            Fire();
            bReadyToFire = false;
            StartCoroutine(CoolDown());
        }
    }

    /// <summary>
    /// Fire projectile.
    /// </summary>
    protected virtual void Fire()
    {
        //Spawn projectile
        BaseProjectile projectile = PoolManager.Spawn<BaseProjectile>(tProjectileSpawn) as BaseProjectile;
        //Stop shooting if there is no projectile in the pool
        if (projectile == null)
            return;

        SetProjectilePosition(projectile, Vector3.zero);
        //Do animation
        SetRecoilAnimation();
    }

    protected void SetProjectilePosition(BaseProjectile projectile, Vector3 pos)
    {
        projectile.transform.localPosition = Vector3.zero;
        projectile.transform.localRotation = Quaternion.identity;
        projectile.transform.SetParent(null);
        projectile.transform.localScale = Vector3.one;
    }

    protected IEnumerator CoolDown()
    {
        if (fShotsPerSecond <= 0f)
        {
            fShotsPerSecond = 0.01f;
            Debug.LogError("The shots per second in " + gameObject.name + " are set to less than 0 per second. Defaulted to 0.01 shots/s.");
        }

        float coolDownTime = 1f / fShotsPerSecond;

        yield return new WaitForSeconds(coolDownTime);

        bReadyToFire = true;
    }

    protected void SetRecoilAnimation()
    {
        anim.SetTrigger(ANIM_TRIGGER_RECOIL);
    }

}
