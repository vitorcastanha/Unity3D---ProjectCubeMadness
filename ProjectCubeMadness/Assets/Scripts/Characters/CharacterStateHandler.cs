using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Will take care of anything that affects the character state:
/// poison, slow, stun, damage, state related power ups, etc
/// </summary>
public class CharacterStateHandler : MonoBehaviour
{
    public delegate void OnTakeDamageAction(float damage);
    public delegate void OnTakeDamageCallBack();
    public delegate void OnHealingAction(float healing);
    public delegate void OnHealingCallBack();

    public event OnHealingAction onHealing;
    public event OnHealingCallBack onHealingCallBack;
    public event OnTakeDamageAction onTakeDamage;
    public event OnTakeDamageCallBack onTakeDamageCallBack;

    /// <summary>
    /// Deals damage to the Character.
    /// </summary>
    /// <param name="damage">This value is always interpreted as negative.</param>
    public void DamageCharacter(float damage)
    {
        if(onTakeDamage != null)
            onTakeDamage(Mathf.Abs(damage) * -1f);
        if(onTakeDamageCallBack != null)
            onTakeDamageCallBack();
    }

    /// <summary>
    /// Heals the Character.
    /// </summary>
    /// <param name="healing">This value is always interpreted as positive.</param>
    public void HealCharacter(float healing)
    {
        if(onHealing != null)
            onHealing(Mathf.Abs(healing));
        if(onHealingCallBack != null)
            onHealingCallBack();
    }
}
