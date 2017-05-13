using UnityEngine;
using System.Collections;

public class HeroMovementController : MonoBehaviour {

    #region Designer Variables
    [Header ("Units per second")]
    [SerializeField] private float speed = 5;
    [Header ("Degrees per frame")]
    [SerializeField] private float turnSpeed = 20;
    #endregion

    private HeroAnimationController animController;         //This is a required component in HeroCharacter
    private Rigidbody rb;                                   //This is a required component in HeroCharacter
    private Vector3 v3Movement;                             //This allows me to calculate it in Update and apply it in FixedUpdate.
    private Quaternion qRotation;                           //This allows me to calculate it in Update and apply it in FixedUpdate.

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animController = GetComponent<HeroAnimationController>();
        InputHandler.PlayerController.onMove += CalculateMovement;
        InputHandler.PlayerController.onAim += CalculateRotation;
    }

    private void FixedUpdate()
    {
        ApplyMotion();
        ApplyAnimation(v3Movement.magnitude);
    }

    private void CalculateMovement(Vector3 moveVector)
    {
        v3Movement = moveVector * speed;
    }

    private void CalculateRotation(Vector3 rotationVector)
    {
        Quaternion newRotation = new Quaternion();
        newRotation.SetLookRotation(rotationVector);
        //Lerp in constant speed towards the new forward vector
        float distance = Quaternion.Angle(newRotation, transform.localRotation);
        if (distance != 0f) //stop division by zero
        {
            float t = turnSpeed / distance;
            qRotation = Quaternion.Lerp(transform.localRotation, newRotation, t);
        }
    }

    //Makes sure the motion is being applied at a fixed frame rate, while input gathering is not
    private void ApplyMotion()
    {
        rb.velocity = v3Movement;
        transform.localRotation = qRotation;
    }

    private void ApplyAnimation(float moveMagnitude)
    {
        if (moveMagnitude > 0.25f)
        {
            animController.SetWalkingAnimation(true);
        }
        else
        {
            animController.SetWalkingAnimation(false);
        }
    }
}
