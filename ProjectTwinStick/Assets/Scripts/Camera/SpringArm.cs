using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class SpringArm : MonoBehaviour
{
    [SerializeField] private Camera cCamera;
    [SerializeField] float fArmLegnth = 10f;
    [Header ("Follows the target around")]
    [SerializeField] private Transform tTarget;

    private Vector3 cameraSocketPosition;

    [SerializeField] private enum CameraUpdate
    {
        Update,
        FixedUpdate
    }
    [SerializeField] private CameraUpdate cameraUpdate;

    private void Update()
    {
        cameraSocketPosition = transform.TransformVector(Vector3.up) * fArmLegnth;

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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;   
        Gizmos.DrawLine(transform.position, (transform.position + cameraSocketPosition));
    }

}
