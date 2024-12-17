using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelMove : MonoBehaviour
{
    [Header("Main Settings")]
    public LayerMask _hitLayer;
    public float _rayDistance = 1f;
    public float _carTopSpeed;
    public AnimationCurve _powerCurve;
    [Header("Spring Settings")]
    public float _springStrength = 10f;
    public float _springResistance = 10f;
    [Header("Wheel Settings")]
    public float _tireGripFactor = 5f;
    public float _tireMass = 5f;
    public bool _isRotatingWheel = false;
    [SerializeField] float rotateBackSpeed = 3f; // degrees per second
    [SerializeField] float rotateSpeed = 10f;    // degrees per second
    [SerializeField] float minAngle = -45f;     // degrees
    [SerializeField] float maxAngle = 45f;      // degrees
    [SerializeField] float neutralAngle = 0f;    // degrees


    private Rigidbody _carRigidbody;
    private Transform _carTransform;
    private float _force;
    private Transform _tireTransform;
    private RaycastHit _rayHit;
    private float angle = 0f;


    private void Start()
    {
        _carRigidbody = GetComponentInParent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        _tireTransform = transform;
        _carTransform = transform.parent.transform;
        float _accelInput = Input.GetAxis("Vertical") * 100f;
        bool _hit = Physics.Raycast(_tireTransform.position, Vector3.down, out _rayHit, _rayDistance, _hitLayer);
        if (_hit)
        {
            Vector3 _springDir = _tireTransform.up;
            Vector3 _tireWorldVel = _carRigidbody.GetPointVelocity(_tireTransform.position);
            float _offset = _rayDistance - _rayHit.distance;
            float _velocity = Vector3.Dot(_springDir, _tireWorldVel);
            _force = (_offset * _springStrength) - (_velocity * _springResistance);
            _carRigidbody.AddForceAtPosition(_springDir * _force, _tireTransform.position);
        }
        if (_hit)
        {
            Vector3 _steeringDir = _tireTransform.right;
            Vector3 _tireWorldVel = _carRigidbody.GetPointVelocity(_tireTransform.position);
            float _steeringVel = Vector3.Dot(_steeringDir, _tireWorldVel);
            float _desiredVelChange = -_steeringVel * _tireGripFactor;
            float _desiredAccel = _desiredVelChange;
            _carRigidbody.AddForceAtPosition(_steeringDir * _tireMass * _desiredAccel, _tireTransform.position);
        }
        if (_hit)
        {
            Vector3 _accelDir = _tireTransform.forward;
            if (_accelInput != 0.0f)
            {
                float _carSpeed = Vector3.Dot(_carTransform.forward, _carRigidbody.velocity);
                float _normalizedSpeed = Mathf.Clamp01(Mathf.Abs(_carSpeed) / _carTopSpeed);
                float _avaibleTorque = _powerCurve.Evaluate(_normalizedSpeed) * _accelInput;
                if (_normalizedSpeed == 1) _avaibleTorque = -_carSpeed;
                _carRigidbody.AddForceAtPosition(_accelDir * _avaibleTorque, _tireTransform.position);
            }
        }
        if (_isRotatingWheel)
        {
            angle = Mathf.Clamp(angle + Input.GetAxis("Horizontal") * rotateSpeed
            * Time.deltaTime, minAngle, maxAngle);

            if (Input.GetAxis("Horizontal") == 0)
            {
                angle = Mathf.MoveTowardsAngle(angle, neutralAngle,
                        rotateBackSpeed * Time.deltaTime);
            }
            Debug.Log(angle);
            _tireTransform.localRotation = Quaternion.Euler(0f, angle, 0f);
        }
    }
}