using UnityEngine;
using System.Collections;

public class EnemyRanged : BaseEnemy {

    private enum RangedEnemyStates
    {
        HUNTING,
        CHASING,
        ATTACKING,
        STUNNED,
        DEAD
    }
    private RangedEnemyStates state;

    private bool bAttackIsReady = true;

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
        //Set moving animation bool to false

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
                    navAgent.SetDestination(hcTargetHero.transform.position);

                    //Change to attack when it reaches the player
                    if (CheckAttackRange())
                        state = RangedEnemyStates.ATTACKING;
                }
                break;

            case RangedEnemyStates.ATTACKING:
                {
                    navAgent.Stop();

                    if (bAttackIsReady)
                        //Do attack

                    //Go back to looking for a victim if not in attack range anymore
                    if (!CheckAttackRange())
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
        state = RangedEnemyStates.HUNTING;
        StartCoroutine(CoolDownHuntPulse());
    }

    public override void OnDespawn()
    {
        base.OnDespawn();
        StopAllCoroutines();
    }
}
