using UnityEngine;
using System.Collections;

/// <summary>
/// User interface damage.
/// This class is used as a Key to preload and spawn the particle
/// from the pool manager.
/// </summary>
[RequireComponent (typeof(ParticleSystem))]
public class UIDamage : PoolObject 
{
    private ParticleSystem pSystem;
    private void Start()
    {
        pSystem = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if (!pSystem.IsAlive() || pSystem.isStopped)
        {
            CleanParticle();
        }
    }

    private void CleanParticle()
    {
        PoolManager.DeSpawn(this.gameObject);
    }
}
