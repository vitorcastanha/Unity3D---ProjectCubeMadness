using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroCharacterStateHandler : CharacterStateHandler
{
    private bool isImune;

    public delegate void OnHitImunityCallback(float duration);
    public event OnHitImunityCallback onHitImunityCallback;
    
    public override void DamageCharacter(float damage)
    {
        if (isImune)
            return;
        base.DamageCharacter(damage);
        if (onHitImunityCallback != null)
        {
            onHitImunityCallback(HeroCharacter.GetInstance().ImunityDuration);
            StartCoroutine(DamageImunity());
        }
    }

    private IEnumerator DamageImunity()
    {
        isImune = true;
        yield return new WaitForSeconds(HeroCharacter.GetInstance().ImunityDuration);
        isImune = false;
    }
}
