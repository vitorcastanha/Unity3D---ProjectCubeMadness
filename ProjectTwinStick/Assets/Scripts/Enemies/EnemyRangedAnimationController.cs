using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Animator))]
public class EnemyRangedAnimationController : MonoBehaviour 
{
    //makes sure the enemy is stuck in the state while finishing the fire animation.
    private const string ANIM_BOOL_AIMING = "aiming";

    #region Designer Variables
    [SerializeField] private Weapon weapon;
    #endregion
    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    /// <summary>
    /// Sets the aiming. This is made to be used as an animation event.
    /// </summary>
    /// <param name="aiming">If set to <c>true</c> aiming.</param>
    public void SetAiming(bool aiming)
    {
        anim.SetBool(ANIM_BOOL_AIMING, aiming);
    }

    /// <summary>
    /// Fire weapon.
    /// </summary>
    public void DoFire()
    {
        weapon.DoAttack();
        SetAiming(false);
    }
}
