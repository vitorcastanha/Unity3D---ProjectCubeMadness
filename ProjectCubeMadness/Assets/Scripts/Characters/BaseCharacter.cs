using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterStateHandler))]
public class BaseCharacter : PoolObject
{

    protected float fHealth;
    protected float fMaxHealth = 100f;
    protected bool bIsDead = false;
    private CharacterStateHandler stateHandler;
    public CharacterStateHandler StateHandler { get { return stateHandler; } }

    protected virtual void Awake()
    {
        stateHandler = GetComponent<CharacterStateHandler>();
        stateHandler.onTakeDamage += CalculateHealth;
        stateHandler.onHealing += CalculateHealth;
    }

    virtual protected void Start()
    {
	    //Base
        fHealth = fMaxHealth;
    }

    virtual protected void Update()
    {
	    //Base
    }

    /// <summary>
    /// Calculates the health.
    /// </summary>
    /// <param name="deltaHealth">Delta health. Can be positive or negative.</param>
    virtual protected void CalculateHealth(float deltaHealth)
    {
        fHealth += deltaHealth;
        if (fHealth > fMaxHealth)
        {
            fHealth = fMaxHealth;
        }
        CalculateDead();
    }

    private void CalculateDead()
    {
        if (fHealth <= 0)
        {
            bIsDead = true;
        }
        else
        {
            bIsDead = false;
        }
    }

    /// <summary>
    /// Determines whether this instance is dead.
    /// </summary>
    /// <returns><c>true</c> if this instance is dead; otherwise, <c>false</c>.</returns>
    public bool IsDead()
    {
        return bIsDead;
    }

    /// <summary>
    /// Gets the health.
    /// </summary>
    /// <returns>The health.</returns>
    public float GetHealth()
    {
        return fHealth;
    }

    public float GetMaxHealth()
    {
        return fMaxHealth;
    }
}
