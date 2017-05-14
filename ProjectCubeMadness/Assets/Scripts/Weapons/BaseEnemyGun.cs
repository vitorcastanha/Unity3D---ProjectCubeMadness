using UnityEngine;
using System.Collections;

/// <summary>
/// Base enemy gun.
/// Functions as a BaseGun but uses EnemyBaseProjectile as ammo instead of BaseProjectile.
/// </summary>
public class BaseEnemyGun : BaseGun {

    protected override void Fire()
    {
        //Spawn projectile
        EnemyBaseProjectile projectile = PoolManager.Spawn<EnemyBaseProjectile>(tProjectileSpawn) as EnemyBaseProjectile;
        //Stop shooting if there is no projectile in the pool
        if (projectile == null)
            return;

        SetProjectileInitPosition(projectile, Vector3.zero);
        projectile.SetMoveDirection(CalculateParallelToFloorVector(projectile));
        //Do animation
        SetRecoilAnimation();
    }
}
