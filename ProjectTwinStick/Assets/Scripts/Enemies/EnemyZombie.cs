using UnityEngine;
using System.Collections;

public class EnemyZombie : BaseEnemy
{
    private enum ZombieStates
    {
        HUNTING,
        CHASING,
        ATTACKING,
        STUNNED,
        DEAD
    }
    private ZombieStates state;

    private const float ATTACK_DAMAGE_CD = 0.5f;            //Damages player twice a second when engaged
    private const string ANIM_TRIGGER_DEAD = "dead";        //Animaion trigger parameter
    private const string ANIM_BOOL_WALK = "walk";
    private const string ANIM_BOOL_ATTACK_IN_RANGE = "attackRange";


    private bool bAttackIsReady = true;

    protected override void Start()
    {
        //if in debug mode, call OnSpawn ()
        base.Start();
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
        anim.SetBool(ANIM_BOOL_ATTACK_IN_RANGE, false); //I'm using a bool instead of trigger because the attack has a "channeling" effect
        anim.SetBool(ANIM_BOOL_WALK, false);

        if (hcTargetHero == null && state != ZombieStates.HUNTING)
        {
            state = ZombieStates.HUNTING;
        }

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
                    anim.SetBool(ANIM_BOOL_WALK, true);
                    navAgent.SetDestination(hcTargetHero.transform.position);
                    //Change to attack when it reaches the player
                    if (CheckAttackRange())
                        state = ZombieStates.ATTACKING;
                }
                break;

            case ZombieStates.ATTACKING:
                {
                    navAgent.Stop();
                    anim.SetBool(ANIM_BOOL_ATTACK_IN_RANGE, true);

                    if (bAttackIsReady)
                        StartCoroutine(DealDamage(fDamage));

                    //Go back to looking for a victim if not in attack range anymore
                    if (!CheckAttackRange())
                        state = ZombieStates.HUNTING;
                }
                break;

            default:
                break;
        }
    }

    //Deals damage to Heroes within range every cycle. ATTACK_DAMAGE_CD determines the cycle duration
    //@param damage Damage done per ATTACK_DAMAGE_CD cycle.
    private IEnumerator DealDamage(float damage)
    {
        float count = 0f;
        hcTargetHero.CalculateHealth(-damage);
        bAttackIsReady = false;

        while (count < ATTACK_DAMAGE_CD)
        {
            count += Time.deltaTime;
            yield return null;
        }

        bAttackIsReady = true;
    }


    /// <summary>
    /// Checks if the enemy is dead.
    /// </summary>
    private void CheckDeath()
    {
        if (bIsDead && state != ZombieStates.DEAD)
        {
            anim.SetTrigger(ANIM_TRIGGER_DEAD);
            navAgent.enabled = false;
            state = ZombieStates.DEAD;
            StartCoroutine(DestroySelf(fCorpseLifeSpan));
            GetComponent<Collider>().enabled = false;
        }
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
