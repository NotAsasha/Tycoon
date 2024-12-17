using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour
{
    [Header("Settings")]
    public Transform _playerCameraTransform;
    public float _hitRange = 3;
    public GameObject _targetObject;
    public GameObject _centralPoint;

    public LayerMask _treeLayer;


    private GameObject _spawnedPoint;
    private GameObject _targetObjectParent;
    private RaycastHit _hit;
    private bool _isFirstLoop = true;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y) && Physics.Raycast(_playerCameraTransform.position, _playerCameraTransform.forward, out _hit, _hitRange + 2f, _treeLayer))
        {
            if (_isFirstLoop)
            {
                _spawnedPoint = Instantiate(_centralPoint, _hit.point, Quaternion.identity);

                _targetObject = _hit.collider.gameObject;
                _targetObjectParent = _targetObject.transform.root.gameObject;
                _spawnedPoint.GetComponent<Rigidbody>().mass = 1f;
                _spawnedPoint.GetComponent<HingeJoint>().connectedBody = _targetObjectParent.GetComponent<Rigidbody>();
                _spawnedPoint.GetComponent<HingeJoint>().breakForce = 1000f;
                _isFirstLoop = false;
            }

        }
        if (Input.GetKey(KeyCode.Y) && _targetObject)
        {
            if (!_spawnedPoint.GetComponent<HingeJoint>())
            {
                StopMoving();

            }
            if (_spawnedPoint.GetComponent<HingeJoint>())
            {
                MoveTheObject();
            }

        }
        if (Input.GetKeyUp(KeyCode.Y) || (Input.GetKeyUp(KeyCode.E) && _spawnedPoint))
        {
            StopMoving();
        }
    }
    public void MoveTheObject()
    {
        Vector3 _cameraPoint = (_playerCameraTransform.position + _playerCameraTransform.forward * _hitRange);
        Vector3 DirectionToPoint = (_cameraPoint - _targetObject.transform.position);
        float DistanceToPoint = DirectionToPoint.magnitude;
        _spawnedPoint.GetComponent<Rigidbody>().AddForce(DirectionToPoint * 40f);
        _spawnedPoint.GetComponent<Rigidbody>().velocity = _spawnedPoint.GetComponent<Rigidbody>().velocity.normalized * DistanceToPoint;
    }


    public void StopMoving()
    {
        _isFirstLoop = true;
        _targetObject = null;
        _targetObjectParent = null;
        Destroy(_spawnedPoint);
    }
}
