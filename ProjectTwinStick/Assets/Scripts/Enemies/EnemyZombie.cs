using UnityEngine;
using System.Collections;

public class EnemyZombie : BaseEnemy
{
    #region Designer Variables
    [Header ("Searchers for player around this instance, in units")]
    [SerializeField] private float fHuntingRadius = 20f;          //Look for player within this radius
    [SerializeField] private float fCorpseLifeSpan = 3f;         //how long before corpse is cleaned from the scene
    #endregion

    #if UNITY_EDITOR
    [Header ("Editor only behavior")]
    public bool bEnableDebug;
    #endif

    private enum ZombieStates
    {
        HUNTING,
        CHASING,
        ATTACKING,
        STUNNED,
        DEAD
    }
    private ZombieStates state;
    private Animator anim;

    private const float HUNT_PULSE_CD = 0.2f;               //Look for player every 0.2 seconds
    private bool bHuntPulse;                                //When true, this unit will look for the player
    private HeroCharacter hcTargetHero;                     //If it finds a hero, target it
    private const float ATTACK_RANGE = 3f;                  //Change to attack state when target is this close

    protected override void Start()
    {
        base.Start();
        anim = GetComponentInChildren<Animator>();

        #if UNITY_EDITOR
        if (bEnableDebug)
        {
            Debug.Log("Debug function: Start Coroutine", this.gameObject);
            OnSpawn();
        }
        #endif
    }

    protected override void Update()
    {
        base.Update();
        ZombieStateController();
        CheckDeath();
    }

    /// <summary>
    /// Controls the state of Zombie.
    /// </summary>
    private void ZombieStateController()
    {
        switch (state)
        {
            case ZombieStates.HUNTING:
                {
                    LookForPlayer();
                    if (hcTargetHero == null)
                        return;
                    else
                        state = ZombieStates.CHASING;
                }
                break;

            case ZombieStates.CHASING:
                {
                    navAgent.Resume();
                    anim.SetBool("walk", true);
                    navAgent.SetDestination(hcTargetHero.transform.position);
                    //Change to attack when it reaches the player
                    if (CheckAttackRange())
                        state = ZombieStates.ATTACKING;
                }
                break;

            case ZombieStates.ATTACKING:
                {
                    navAgent.Stop();
                    //anim.SetTrigger("attack");

                    //Go back to looking for a victim if not in attack range anymore
                    if (!CheckAttackRange())
                        state = ZombieStates.HUNTING;
                }
                break;

            default:
                break;
        }
    }

    /// <summary>
    /// Checks the attack range.
    /// </summary>
    /// <returns><c>true</c>, if attack is in range, <c>false</c> otherwise.</returns>
    private bool CheckAttackRange()
    {
        //If player died or the referrence was lost for some reason (invisibility?) return false
        if (hcTargetHero == null)
        {
            return false;
        }
        return (Vector3.Distance(hcTargetHero.transform.position, transform.position) < ATTACK_RANGE);
    }

    /// <summary>
    /// Looks for player.
    /// </summary>
    private void LookForPlayer()
    {
        //Only check for player every so often to save resources
        if (bHuntPulse)
        {
            Collider[] cols = Physics.OverlapSphere(transform.position, fHuntingRadius, PLAYER_LAYER);
            float closestDistance = 9999f;
            int index = 0;

            //finds nearest player and target it
            for (int i = 0; i < cols.Length; i++)
            {
                float dist = Vector3.Distance(cols[i].transform.position, transform.position);
                if (dist < closestDistance)
                {
                    closestDistance = dist;
                    index = i;
                }
            }
            if (cols.Length == 0)
                return;

            try
            {
                hcTargetHero = cols[index].GetComponent<HeroCharacter>();
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Object is using the PLAYER_LAYER without having the correct class attached.", cols[index].gameObject);
                Debug.LogError(ex.Message);
            }
        }
    }

    /// <summary>
    /// Cools down hunt pulse.
    /// </summary>
    /// <returns>The down hunt pulse.</returns>
    private IEnumerator CoolDownHuntPulse()
    {
        float count = 0;
        while (count < HUNT_PULSE_CD)
        {
            count += Time.deltaTime;
            yield return null;
        }
        bHuntPulse = true;
    }

    /// <summary>
    /// Checks if the enemy is dead.
    /// </summary>
    private void CheckDeath()
    {
        if (bIsDead && state != ZombieStates.DEAD)
        {
            anim.SetTrigger("dead");
            navAgent.enabled = false;
            state = ZombieStates.DEAD;
            StartCoroutine(DestroySelf(fCorpseLifeSpan));
            GetComponent<Collider>().enabled = false;
        }
    }

    private IEnumerator DestroySelf(float delay = 0f)
    {
        float count = 0f;
        while (count < delay)
        {
            count += Time.deltaTime;
            yield return null;
        }
        #if UNITY_EDITOR
        if (bEnableDebug)
        {
            Destroy(gameObject);
            return false;
        }
        #endif
        PoolManager.DeSpawn(gameObject);
    }

    public override void OnSpawn()
    {
        base.OnSpawn();
        state = ZombieStates.HUNTING;
        StartCoroutine(CoolDownHuntPulse());
    }

    public override void OnDespawn()
    {
        base.OnDespawn();
        StopAllCoroutines();
    }


}
