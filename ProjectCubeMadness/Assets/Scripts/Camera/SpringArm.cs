using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[ExecuteInEditMode]
public class SpringArm : MonoBehaviour
{
    static private SpringArm instance;

    [SerializeField] private Camera cCamera;
    [SerializeField] float fArmLegnth = 10f;
    [Header ("Follows the target around")]
    [SerializeField] private Transform tTarget;

    private Vector3 cameraSocketPosition;
    private Vector3 shakeCoefficient = Vector3.zero;

    [SerializeField] private enum CameraUpdate
    {
        Update,
        FixedUpdate
    }
    [SerializeField] private CameraUpdate cameraUpdate;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        cameraSocketPosition = (transform.TransformVector(Vector3.up) * fArmLegnth) + shakeCoefficient;

        if(cCamera != null)
            cCamera.transform.position = transform.position + cameraSocketPosition;

        if(cameraUpdate == CameraUpdate.Update)
            transform.position = tTarget.position;
    }

    private void FixedUpdate()
    {
        if(cameraUpdate == CameraUpdate.FixedUpdate)
            transform.position = tTarget.position;
    }

    //Draw SpringArm on editor to allow for designer tuning
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;   
        Gizmos.DrawLine(transform.position, (transform.position + cameraSocketPosition));
    }

    /// <summary>
    /// Does the camera shake.
    /// </summary>
    /// <param name="magnitude">Magnitude.</param>
    /// <param name="duration">Duration.</param>
    public void DoShakeCamera(float magnitude, float duration)
    {
        StartCoroutine(ShakeCamera(magnitude, duration));
    }

    IEnumerator ShakeCamera(float magnitude, float duration)
    {
        float count = 0f;
        //Randomizes the shake direction
        float x = Random.Range(-magnitude, magnitude);
        float y = Random.Range(-magnitude, magnitude);
        float z = Random.Range(-magnitude, magnitude);

        //First cycle will move the camera away
        while (count < duration * 0.5f)
        {
            shakeCoefficient += new Vector3(x, y, z);
            count += Time.deltaTime;
            yield return null;
        }
        count = 0f;

        //Second cycle will move camera back
        while (count < duration * 0.5f)
        {
            shakeCoefficient -= new Vector3(x, y, z);
            count += Time.deltaTime;
            yield return null;
        }

        //Fix camera position to be exacly where it started
        shakeCoefficient = Vector3.zero;

    }

    static public SpringArm GetInstance()
    {
        return instance;
    }
}
