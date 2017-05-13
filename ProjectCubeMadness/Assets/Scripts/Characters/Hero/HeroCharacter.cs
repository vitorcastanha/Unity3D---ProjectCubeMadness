using UnityEngine;
using System.Collections;

/// <summary>
/// Hero character.
/// This class is the base for the Hero character and makes sure that all the
/// necessary components are in place. It also serves as an interface between
/// controllers.
/// All controllers are included as a RequiredComponent.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(HeroMovementController))]
[RequireComponent(typeof(HeroAnimationController))]
[RequireComponent(typeof(HeroCombatController))]

public class HeroCharacter : BaseCharacter
{
    static HeroCharacter instance;

    #region Designer Variables
    [SerializeField]private int maxHeartPiecesCount = 3;
    #endregion

    protected override void Awake()
    {
        base.Awake();
        instance = this;
        fMaxHealth = maxHeartPiecesCount * 30;
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    static public HeroCharacter GetInstance()
    {
        return instance;
    }

    protected override void CalculateHealth(float deltaHealth)
    {
        base.CalculateHealth(deltaHealth);
    }

    #region Properties

    public int MaxHeartPieceCount
    {
        get
        {
            return maxHeartPiecesCount;
        }
    }

    public int CurrentHealth
    {
        get
        {
            return (int)(GetHealth() * 0.1f); //while life points are calculated in float behind the scenes, it is
                                              //displayed to the player as heart pieces, an integer value
        }
    }

    public int CurrentHeartContainerIndex
    {
        get
        {
            return CurrentHealth / 3;
        }
    }
    #endregion
}
