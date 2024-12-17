using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObjV2 : MonoBehaviour
{
    [Header("Settings")]
    public Transform _playerCameraTransform;
    public float _hitRange = 3;
    public GameObject _targetObj;
    public GameObject _centralPoint;
    public LayerMask _treeLayer;


    private GameObject _spawnedPoint;
    private RaycastHit _hit;

    void Start()
    {

    }

    void Update()
    {
        if (Physics.Raycast(_playerCameraTransform.position, _playerCameraTransform.forward, out _hit, _hitRange, _treeLayer))
        {
            if (Input.GetKeyDown(KeyCode.Y))
            {
                _spawnedPoint = Instantiate(_centralPoint, _hit.point, Quaternion.identity);
                _targetObj = _hit.collider.gameObject.transform.root.gameObject;

            }
       
        }
        if (Physics.Raycast(_playerCameraTransform.position, _playerCameraTransform.forward, out _hit, _hitRange + 2f, _treeLayer))
        {
            if (Input.GetKey(KeyCode.Y))
            {

            }
        }         
       MoveTheObject();

        if (Input.GetKeyUp(KeyCode.Y))
        {
            DestroyTheObject();
      
        }
    }
    public void MoveTheObject()
    {
        _spawnedPoint.transform.position = (_playerCameraTransform.position + _playerCameraTransform.forward * _hitRange);
        _targetObj.GetComponent<Rigidbody>().AddForce((_spawnedPoint.transform.position - _hit.collider.gameObject.transform.position) * 500);

    }
    public void DestroyTheObject()
    {
        Destroy(_spawnedPoint);
    }
/*    void OnCollisionStay(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            print(contact.thisCollider.name + " hit " + contact.otherCollider.name);
            // Visualize the contact point
            Debug.DrawRay(contact.point, contact.normal, Color.white);
        }
    }*/
}
