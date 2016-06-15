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

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }




}
