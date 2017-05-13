using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
/// <summary>
/// Base enemy.
/// This class is used as a base for every Enemy class.
/// It's main purpuse is to enable any one to access common functions such as
/// damage functions without knowing exacly which type of enemy is being hit.
/// </summary>
public class BaseEnemy : BaseCharacter
{
    #if UNITY_EDITOR
    [Header ("Editor only behavior")]
    public bool bEnableDebug;
    #endif

    #region Designer Variables
    [Header ("Searchers for player around this instance, in units")]
    [SerializeField] protected float fHuntingRadius = 20f;         //Look for player within this radius
    [SerializeField] protected float fCorpseLifeSpan = 3f;         //how long before corpse is cleaned from the scene
    [SerializeField] protected float fDamage = 10f;
    [Range (1,10)]                                                 //Range is capped to keep enemies from firing from off-screen
    [SerializeField] protected float fAttackRange = 3f;            //Change to attack state when target is this close
    #endregion

    protected bool bHuntPulse;                                     //When true, this unit will look for the player
    protected HeroCharacter hcTargetHero;                          //If it finds a hero, target it

    protected const float HUNT_PULSE_CD = 0.2f;
    protected const int PLAYER_LAYER = 256;                        //Layer 08

    protected Animator anim;
    protected UnityEngine.AI.NavMeshAgent navAgent;

    protected override void Start()
    {
        base.Start();
        navAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();

        #if UNITY_EDITOR
        if (bEnableDebug)
        {
            //if an enemy is placed on the scene for testing purpuses, call Spawn at Start
            Debug.Log("Debug function: Start Coroutine", this.gameObject);
            OnSpawn();
        }
        #endif
    }

    /// <summary>
    /// Checks the attack range.
    /// </summary>
    /// <returns><c>true</c>, if attack is in range, <c>false</c> otherwise.</returns>
    protected bool CheckAttackRange()
    {
        return (Vector3.Distance(hcTargetHero.transform.position, transform.position) < fAttackRange);
    }

    /// <summary>
    /// Looks for player.
    /// </summary>
    protected void LookForPlayer()
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
    protected IEnumerator CoolDownHuntPulse()
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
    /// Cleans gameObject from scene.
    /// </summary>
    /// <param name="delay">Time before cleaning the gameObject from scene.</param>
    protected IEnumerator DestroySelf(float delay = 0f)
    {
        float count = 0f;
        while (count < delay)
        {
            count += Time.deltaTime;
            yield return null;
        }

        #if UNITY_EDITOR
        //this behavior is only relevant against enemies that are placed on the scene without spawning
        if (bEnableDebug)
        {
            Destroy(gameObject);
            yield break;
        }
        #endif
        PoolManager.DeSpawn(gameObject);
    }

}
