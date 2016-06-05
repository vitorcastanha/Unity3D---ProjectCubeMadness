using UnityEngine;
using System.Collections;

/// <summary>
/// Weapon.
/// This is the base for ranged and melee weapons.
/// Used to facilitate talking to a weapon without knowing it's type.
/// </summary>
public class Weapon : MonoBehaviour
{
    virtual public void DoAttack()
    {
        //Base
    }
}
