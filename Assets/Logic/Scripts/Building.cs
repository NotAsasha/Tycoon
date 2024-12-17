using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{

    public Transform _playerCameraTransform;

    public float _buildRange = 10;
    public LayerMask _groundLayer;
    public GameObject _spawnedPoint;
    private RaycastHit _hit;
    void Update()
    {
        Physics.Raycast(_playerCameraTransform.position, _playerCameraTransform.forward, out _hit, _buildRange, _groundLayer);

        if (Input.GetKeyDown(KeyCode.J))
        {
            Instantiate(_spawnedPoint, _hit.point, Quaternion.identity);
        }
    }
}
