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
        //Do animtion
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
        float count = 0;

        if (fShotsPerSecond <= 0f)
        {
            fShotsPerSecond = 0.01f;
        }

        float coolDownTime = 1 / fShotsPerSecond;

        while (count < coolDownTime)
        {
            count += Time.deltaTime;
            yield return null;
        }

        bReadyToFire = true;
    }

    protected void SetRecoilAnimation()
    {
        anim.SetTrigger(ANIM_TRIGGER_RECOIL);
    }

}
