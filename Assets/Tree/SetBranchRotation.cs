using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetBranchRotation : MonoBehaviour
{
    [Header("Distance between brunches:")]
    public int _minBrunchDistance = 15;
    public List<float> _rotationsX;
    public List<float> _rotationsZ;
    [Header("Just useless info:")]
    public float _branchRotationX = 0f;
    public float _branchRotationZ = 0f;
    private float _newBranchRotationX;
    private float _newBranchRotationZ;
    private int _numberOfRotations;
    private int _successCount;
    public bool _isFirst = true;
    public bool _haveRotation;
    public float _errorCount;
    public bool _debugMode = false;

    private void Start()
    {
        _haveRotation = false;
        _rotationsX = new List<float>();
        _rotationsZ = new List<float>();
    }
    public void ChooseBranchPosition()
    {
        if (_isFirst == true)
        {
            _branchRotationX = Random.Range(-60, 60);
            _branchRotationZ = Random.Range(-60, 60);
            _isFirst = false;
            _haveRotation = true;
        }
        else
        {
            GenerateRotation();
            if (_successCount == _rotationsX.Count)
            {
                _branchRotationX = _newBranchRotationX;
                _branchRotationZ = _newBranchRotationZ;
                _successCount = 0;
                _haveRotation = true;
            }
            else
            {
                GenerateRotation();
            }
        }
    }
    private void GenerateRotation()
    {
        if (_errorCount < 20)
        {
        _newBranchRotationX = Random.Range(-60, 60);
        _newBranchRotationZ = Random.Range(-60, 60);
        int _numberOfrotations = _rotationsZ.Count;
        int _numberOfLoops = 0;
        for (int i = 0; i < _numberOfrotations; i++)
        {
            float _lastRotationX = _rotationsX[_numberOfLoops];
            float _lastRotationZ = _rotationsZ[_numberOfLoops];
            if (_newBranchRotationX == _lastRotationX && _newBranchRotationZ == _lastRotationZ)
            {
                GenerateRotation();
            }
            _numberOfLoops += 1;
        }
        CanItBe();
        }

    }
    private void CanItBe()
    {
        _numberOfRotations = _rotationsX.Count;
        _successCount = 0;
        for (int i = 0; i < _numberOfRotations; i++)
        {
            float _rotationX = _rotationsX[i];
            float _rotationZ = _rotationsZ[i];


            if (Mathf.Abs(_newBranchRotationX - _rotationX) >= _minBrunchDistance || Mathf.Abs(_newBranchRotationZ - _rotationZ) >= _minBrunchDistance)
            {
                _successCount += 1;
            }
            else
            {
                _errorCount += 1;
                GenerateRotation();
                break;
            }
        }
    }
}
