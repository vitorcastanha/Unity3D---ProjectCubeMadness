using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Collider))]
public class PickUp : MonoBehaviour
{

    protected virtual void Activate(HeroCharacter hero)
    {
        //Run pickup function
    }

    protected virtual void VisualFeedBack(HeroCharacter hero)
    {
        //Spawn particle effects
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            HeroCharacter hero = col.GetComponent<HeroCharacter>();
            Activate(hero);
            VisualFeedBack(hero);
            Destroy(gameObject);
        }
    }
}
