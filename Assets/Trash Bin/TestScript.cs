using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    [SerializeField] private LayerMask _pickupLayer;
    [SerializeField] private Camera _playerCam;
    [SerializeField] private Transform PickupTarget;
    [Space]
    [SerializeField] private float PickupRange;
    private Rigidbody CurrentObject;


    void Start()
    {

    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (CurrentObject)
            {
                CurrentObject = null;
                return;
            }

            Ray CameraRay = _playerCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            if (Physics.Raycast(CameraRay, out RaycastHit HitInfo, PickupRange, _pickupLayer))
            {
                CurrentObject = HitInfo.transform.root.gameObject.GetComponent<Rigidbody>();

            }
        }
    }

    void FixedUpdate()
    {
        if (CurrentObject)
        {
            Vector3 DirectionToPoint = (PickupTarget.position - CurrentObject.position) * 12f;
            float DistanceToPoint = DirectionToPoint.magnitude;
            Vector3.Lerp(PickupTarget.position, CurrentObject.position, Time.deltaTime * 12f);
            CurrentObject.velocity = DirectionToPoint * 12f * DistanceToPoint;
        }
    }
}