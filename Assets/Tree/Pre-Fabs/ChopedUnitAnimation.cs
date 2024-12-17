using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChopedUnitAnimation : MonoBehaviour
{
    public float _minX = -100f;
    public float _maxX = 100f;
    public float _minY = -100f;
    public float _maxY = 100f;
    public float _minZ = -100f;
    public float _maxZ = 100f;
    Rigidbody _rigidbody;
    private void Start()
    {
   
    }
    public void ChopAnimation()
    {
        Debug.Log(_rigidbody);
        if (_rigidbody != null)
        {
            Destroy(_rigidbody.gameObject);
        }
        _rigidbody = gameObject.AddComponent<Rigidbody>();
/*        MeshCollider _collider = gameObject.AddComponent<MeshCollider>();
        _collider.convex = true;*/
        float _x = Random.Range(_minX, -_maxX);
        float _y = Random.Range(_minY, -_maxY);
        float _z = Random.Range(_minZ, -_maxZ);

        _rigidbody.AddForce(new Vector3(_x, _y, _z));
    }
}
