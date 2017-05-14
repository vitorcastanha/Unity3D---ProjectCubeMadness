using UnityEngine;
using System.Collections;

public class puHealth : PickUp {

    private const float FEEDBACK_DURATION = 0.6f;
    [SerializeField] private GameObject feedback;

    private void Update()
    {
        transform.Rotate(Vector3.one);
    }

    protected override void Activate(HeroCharacter hero)
    {
        base.Activate(hero);
        hero.HeroStateHandler.HealCharacter(30f);
    }

    protected override void VisualFeedBack(HeroCharacter hero)
    {
        base.VisualFeedBack(hero);
        feedback = Instantiate(feedback, hero.transform.position, hero.transform.rotation) as GameObject;
        feedback.transform.SetParent(hero.transform);
        feedback.transform.localPosition = Vector3.zero;
        Destroy(feedback, FEEDBACK_DURATION);
    }

    private void DespawnSelf()
    {
        PoolManager.DeSpawn(feedback.gameObject);
    }
}
