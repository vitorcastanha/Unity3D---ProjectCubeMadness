using UnityEngine;
using System.Collections;

public class HeroAnimationController : MonoBehaviour
{
    private const string ANIM_BOOL_WALK = "walk";
    private Animator anim;

    private void Start()
    {
        //ensures we are dealing with the model animation
        anim = GetComponentInChildren<HeroModelController>().gameObject.GetComponent<Animator>();
    }

    private void Update()
    {
	    
    }

    /// <summary>
    /// Sets the walking animation.
    /// </summary>
    /// <param name="walk">If set to <c>true</c> walk.</param>
    public void SetWalkingAnimation(bool walk)
    {
        anim.SetBool(ANIM_BOOL_WALK, walk);
    }
}
