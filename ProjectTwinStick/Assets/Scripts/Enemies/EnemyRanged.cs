using UnityEngine;
using System.Collections;

public class EnemyRanged : BaseEnemy 
{
    private const string ANIM_TRIGGER_DEATH = "dead";
    private const string ANIM_BOOL_CHASING = "chasing";
    private const string ANIM_BOOL_ATTACKING = "inAttackRange";
    private const string ANIM_BOOL_AIMING = "aiming";

    private enum RangedEnemyStates
    {
        HUNTING,
        CHASING,
        ATTACKING,
        STUNNED,
        DEAD
    }
    private RangedEnemyStates state;

    protected override void Start()
    {
        //if in debug mode, call OnSpawn ()
        base.Start();

    }

    protected override void Update()
    {
        base.Update();
        StateController();
        CheckDeath();
    }

    /// <summary>
    /// Controls the state of Zombie.
    /// </summary>
    private void StateController()
    {
        //Set animation bools to false
        anim.SetBool(ANIM_BOOL_CHASING, false);
        anim.SetBool(ANIM_BOOL_ATTACKING, false);

        if (hcTargetHero == null && state != RangedEnemyStates.HUNTING)
        {
            state = RangedEnemyStates.HUNTING;
        }

        switch (state)
        {
            case RangedEnemyStates.HUNTING:
                {
                    LookForPlayer();
                    if (hcTargetHero == null)
                        return;
                    else
                        state = RangedEnemyStates.CHASING;
                }
                break;

            case RangedEnemyStates.CHASING:
                {
                    navAgent.Resume();
                    //Set moving animation bool to true
                    anim.SetBool(ANIM_BOOL_CHASING, true);

                    navAgent.SetDestination(hcTargetHero.transform.position);

                    //Change to attack when it reaches the player
                    if (CheckAttackRange())
                        state = RangedEnemyStates.ATTACKING;
                }
                break;

            case RangedEnemyStates.ATTACKING:
                {
                    navAgent.Stop();

                    transform.LookAt(new Vector3(hcTargetHero.transform.position.x, transform.position.y, hcTargetHero.transform.position.z));

                    anim.SetBool(ANIM_BOOL_ATTACKING, true); 

                    //Go back to looking for a victim if not in attack range anymore AND not currently preparing an attack
                    if (!CheckAttackRange() && !anim.GetBool(ANIM_BOOL_AIMING))
                        state = RangedEnemyStates.HUNTING;
                }
                break;

            default:
                break;
        }
    }

    /// <summary>
    /// Checks if the enemy is dead.
    /// </summary>
    private void CheckDeath()
    {
        if (bIsDead && state != RangedEnemyStates.DEAD)
        {
            //Play death animation
            anim.SetTrigger(ANIM_TRIGGER_DEATH);

            //Disable agent
            navAgent.enabled = false;
            state = RangedEnemyStates.DEAD;
            StartCoroutine(DestroySelf(fCorpseLifeSpan));
            GetComponent<Collider>().enabled = false;
        }
    }

    public override void OnSpawn()
    {
        base.OnSpawn();

    }

    private void OnEnable()
    {
        state = RangedEnemyStates.HUNTING;
        StartCoroutine(CoolDownHuntPulse());
    }

    public override void OnDespawn()
    {
        base.OnDespawn();
        StopAllCoroutines();
        ResetStats();
    }

    private void ResetStats()
    {
        fHealth = fMaxHealth;
        state = RangedEnemyStates.HUNTING;
        GetComponent<Collider>().enabled = true;
        navAgent.enabled = true;
        bIsDead = false;
    }
}
