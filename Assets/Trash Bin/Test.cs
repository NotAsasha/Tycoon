using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public int _moveType;
    private Rigidbody _rb;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_moveType == 1)
        {
            _rb.velocity = new Vector3(0f, 0f, 0.1f);
        }
        if (_moveType == 2)
        {
            _rb.AddForce(0f, 0f, 50f);
        }
        if (_moveType == 3)
        {
            _rb.MovePosition(new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.1f));
        }
      
    }
}
