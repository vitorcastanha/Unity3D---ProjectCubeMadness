using UnityEngine;
using System.Collections;

/// <summary>
/// Controls the Hero model.
/// </summary>
public class HeroModelController : MonoBehaviour
{
    //Will be used to control facial expressions, color changes, etc.

    private Material materialInstance;
    private MeshRenderer[] mRenderers;
    private const float FADE_DURATION = 0.1f;

    private void Start()
    {
        mRenderers = GetComponentsInChildren<MeshRenderer>();
        foreach (var mrender in mRenderers)
        {
            mrender.material = Instantiate(mrender.material) as Material;
        }
        HeroCharacter.GetInstance().HeroStateHandler.onHitImunityCallback += Blink;
    }

    private void Blink(float duration)
    {
        if (gameObject.activeInHierarchy)
            StartCoroutine(IEBlink(duration));
    }

    private IEnumerator IEBlink(float duration)
    {
        float count = 0f;
        while (count < duration)
        {
            for (int i = 0; i < 2; i++)
            {
                foreach (var mrender in mRenderers)
                    mrender.material.color = new Color(mrender.material.color.r, mrender.material.color.g, mrender.material.color.b, 0f);

                yield return new WaitForSeconds(FADE_DURATION);
                foreach (var mrender in mRenderers)
                    mrender.material.color = new Color(mrender.material.color.r, mrender.material.color.g, mrender.material.color.b, 1f);

                yield return new WaitForSeconds(FADE_DURATION);
            }
            count += FADE_DURATION * 2f;
            yield return null;
        }
    }
}
