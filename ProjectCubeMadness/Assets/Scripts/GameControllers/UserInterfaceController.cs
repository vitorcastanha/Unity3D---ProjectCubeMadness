using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public class UserInterfaceController : MonoBehaviour
{
    private const float HEALTH_PER_HEART_CONTAINER = 30;

    [System.Serializable]
    class UIControl
    {
        [Header("Life points")]
        public Canvas hpCanvas;
        public GameObject[] heartContainers = null;
        public GameObject damageParticle = null;               //Spawns on the hearts when they get destroyed
        public Image bloodOverlay = null;

        [Header("Menu")]
        public GameObject pauseGame;
    }
    [SerializeField] UIControl uiControl;
    List<GameObject> heartPieces;

    void Start()
    {
        BindToHandlers();
        InitHeartContainers();
    }

    void BindToHandlers()
    {
        HeroCharacter.GetInstance().StateHandler.onTakeDamageCallBack += UpdateHealth;
        HeroCharacter.GetInstance().StateHandler.onTakeDamageCallBack += DamageCameraShake;
        HeroCharacter.GetInstance().StateHandler.onHealingCallBack += UpdateHealth;
        InputHandler.MenuController.onPause += ShowPause;
    }

    void InitHeartContainers()
    {
        int heartContainerCount = HeroCharacter.GetInstance().MaxHeartPieceCount;
        heartPieces = new List<GameObject>();

        //activate heart containers based on how many are defined to be available to the player
        //this number can change at design time, and run time by picking up upgrades
        for (int i = 0; i < heartContainerCount; i++)
        {
            uiControl.heartContainers[i].SetActive(true);

            //Track how many heart pieces the player has
            foreach (Transform item in uiControl.heartContainers[i].transform)
            {
                heartPieces.Add(item.gameObject);
            }
            //Make the last healthy heart container bigger
            if (i == heartContainerCount - 1)
            {
                uiControl.heartContainers[i].transform.localScale = Vector3.one;
            }
        }
    }

    void ShowPause()
    {
        if (GameState.IsPaused)
        {
            uiControl.pauseGame.SetActive(true);
            uiControl.hpCanvas.gameObject.SetActive(false);
        }
        else
        {
            uiControl.pauseGame.SetActive(false);
            uiControl.hpCanvas.gameObject.SetActive(true);
        }
    }
    
    private void UpdateHealth()
    {
        int currentHealth = HeroCharacter.GetInstance().CurrentHealth;

        //This approach makes sure that it will work for any amount of damage or heal
        for (int i = 0; i < heartPieces.Count; i++)
        {
            //If the heart piece index is smaller than the current amount of heart pieces the player has, AND it is disabled, then activate it (heal)
            if (i < currentHealth && !heartPieces[i].activeSelf)
                heartPieces[i].SetActive(true);

            //If the heart piece index is bigger than the current amount of heart pieces the player has, AND it is activated, then deactivate it (damage)
            else if (i >= currentHealth && heartPieces[i].activeSelf)
            {
                heartPieces[i].SetActive(false);
                //spawn particle system and position it at the heart piece location
                GameObject clone = PoolManager.Spawn<UIDamage>().gameObject;
                if (clone == null)
                {
                    return;
                }
                clone.transform.position = heartPieces[i].transform.position;
                clone.transform.LookAt(Camera.main.transform.position);
            }
        }

        int currentHeart = HeroCharacter.GetInstance().CurrentHeartContainerIndex;
        //Makes the current heart container bigger then the others
        for (int i = 0; i < uiControl.heartContainers.Length; i++)
        {
            if (i == currentHeart)
            {
                uiControl.heartContainers[i].transform.localScale = Vector3.one;
            }
            else
            {
                uiControl.heartContainers[i].transform.localScale = Vector3.one * 0.8f;
            }
        }
    }

    private void DamageCameraShake()
    {
        SpringArm.GetInstance().DoShakeCamera(0.5f, 0.1f);
        float intensity = (HeroCharacter.GetInstance().CurrentHealth % 3 == 0) ? 1f : 0.3f;         
        //If the heart piece is the last in the container(3 pieces),
        //then the screen flashes with more intensity

        DoFlashOverlay(1f, intensity);
    }

    /// <summary>
    /// Dos the flash overlay.
    /// </summary>
    /// <param name="duration">Duration.</param>
    /// <param name="intensity">Intensity.</param>
    private void DoFlashOverlay(float duration, float intensity)
    {
        StartCoroutine(FlashOverlay(duration, intensity));
    }

    private IEnumerator FlashOverlay(float duration, float intensity)
    {
        float count = 0f;
        float progress = 0f;
        float newDuration = duration * 0.25f; //it takes 1/4 of the duration to reveal the overlay
                                              //and the other 3/4 to hide it.

        if (intensity > 1)
        {
            intensity = 1;
        }

        //First cycle will show overlay
        while (count < newDuration)
        {
            progress += (Time.deltaTime * intensity) / newDuration;
            count += Time.deltaTime;
            ChangeOverlayAlpha(progress);
            yield return null;
        }

        count = 0f;
        newDuration = duration * 0.75f;

        //Second cycle will hide overlay
        while (count < newDuration)
        {
            progress -= (Time.deltaTime * intensity) / newDuration;
            count += Time.deltaTime;
            ChangeOverlayAlpha(progress);
            yield return null;
        }

        //Hide the overlay
        ChangeOverlayAlpha(0f);

    }

    //Change the overlays opacity
    private void ChangeOverlayAlpha(float newAlpha)
    {
        Color newColor = uiControl.bloodOverlay.color;
        newColor = new Color(newColor.r, newColor.b, newColor.g, newAlpha);
        uiControl.bloodOverlay.color = newColor;
    }
}
