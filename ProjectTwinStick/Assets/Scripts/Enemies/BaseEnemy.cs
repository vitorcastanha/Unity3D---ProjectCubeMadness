using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
/// <summary>
/// Base enemy.
/// This class is used as a base for every Enemy class.
/// It's main purpuse is to enable any one to access common functions such as
/// damage functions without knowing exacly which type of enemy is being hit.
/// </summary>
public class BaseEnemy : BaseCharacter
{
    protected NavMeshAgent navAgent;

    protected const int PLAYER_LAYER = 256; //Layer 08

    protected override void Start()
    {
        base.Start();
        navAgent = GetComponent<NavMeshAgent>();
    }

    /// <summary>
    /// Modifies the health.
    /// </summary>
    /// <param name="delta">Delta.</param>
    public void ModifyHealth(float delta)
    {
        CalculateHealth(delta);
    }
}
