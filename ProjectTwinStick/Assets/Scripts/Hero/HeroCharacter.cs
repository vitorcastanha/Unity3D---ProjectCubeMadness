using UnityEngine;
using System.Collections;

/// <summary>
/// Hero character.
/// This class is the base for the Hero character and makes sure that all the
/// necessary components are in place. It also serves as an interface between
/// controllers.
/// All controllers are included as a RequiredComponent.
/// </summary>
[RequireComponent (typeof(Rigidbody))]
[RequireComponent (typeof(HeroMovementController))]
[RequireComponent (typeof(HeroAnimationController))]
[RequireComponent (typeof(HeroCombatController))]

public class HeroCharacter : BaseCharacter
{
    static HeroCharacter instance;

    #region Designer Variables
    [SerializeField]private int heartPiecesCount = 3;
    #endregion

    private void Awake()
    {
        instance = this;
        fMaxHealth = heartPiecesCount * 30; //this needs to be set-up before gameLogic runs Start
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

    public int GetHeartPieceCount()
    {
        return heartPiecesCount;
    }

    public override void CalculateHealth(float deltaHealth)
    {
        base.CalculateHealth(deltaHealth);
        UserInterfaceController.GetInstance().UpdateHealth();
    }
}
