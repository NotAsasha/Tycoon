using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeChop : MonoBehaviour
{
    [Header("Settings")]
    public LayerMask _treeLayer;
    public Transform _playerCameraTransform;
    [Min(1)]
    public float _hitRange = 3;
    public bool _debugMode = false;
    public GameObject _garbageStorage;
    private RaycastHit _hit;

    public GameObject _uiHint;
    public GameObject _storage;
    private GameObject _parentObject;
    private GameObject _targetUnit;
    public GameObject _highPolyUnit;
    public float _objectWeight = 0;


    private UnitInfo _unitID;
    private TreeSpawner _treeSpawner;
    private int _myTrunkID;
    private int _idOfMyBranch;
    private int _idOfMyBranchInBranch;
    private int _myBranchID;
    private int _myBranchInBranchID;
    private bool _wasChoped;
    private float _unitHealth;
    void Start()
    {

    }

    void Update()
    {
        _uiHint.SetActive(false);
        if (Physics.Raycast(_playerCameraTransform.position, _playerCameraTransform.forward, out _hit, _hitRange, _treeLayer))
        {
            _uiHint.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                GetID();
            }
        }
    }
    public void GetID()
    {
        _targetUnit = _hit.collider.gameObject;
        _unitID = _targetUnit.GetComponentInParent<UnitInfo>();
        for (int i = 0; i < _unitID.transform.childCount; i++)
        {
            GameObject _currentObject = _unitID.gameObject.transform.GetChild(i).gameObject;
            if (_currentObject.GetComponent<Optimisation>())
            {
                Destroy(_currentObject);
                Destroy(_unitID.gameObject.GetComponent<BoxCollider>());
                Instantiate(_highPolyUnit, _targetUnit.transform);
            }
        }
        if (_targetUnit.GetComponent<ChopedUnitAnimation>())
        {
            _targetUnit.GetComponent<ChopedUnitAnimation>().ChopAnimation();
            _targetUnit.transform.SetParent(_garbageStorage.transform);
        }
        if (_targetUnit.transform.parent.childCount == 0) Destroy(_targetUnit.transform.parent.gameObject);


      
        if (_unitID)
        {
            _treeSpawner = _targetUnit.GetComponentInParent<TreeSpawner>();
            _myTrunkID = _unitID._myTrunkID;
            _idOfMyBranch = _unitID._idOfMyBranch;
            _idOfMyBranchInBranch = _unitID._idOfMyBranchInBranch;
            _myBranchID = _unitID._myBranchID;
            _myBranchInBranchID = _unitID._myBranchInBranchID;
            _wasChoped = _unitID._wasChoped;
            SeparateTree();
        }
    }
    public void SeparateTree()
    {

        _unitID._unitHealth -= 1;
        if (_unitID._unitHealth < 1)
        {
            _objectWeight = 0;
            GameObject _storage1 = Instantiate(_storage);
            _parentObject = _unitID.gameObject.transform.parent.gameObject;
            GameObject _rootObject = _targetUnit.transform.root.gameObject;
            float _numberOfSiblings = _parentObject.transform.childCount;
            for (int i = 0; i < _numberOfSiblings + 1; i++)
            {

                if (_myTrunkID < i)
                {
                    GameObject _currentObject = _parentObject.transform.GetChild(_myTrunkID).gameObject;

                    _currentObject.transform.SetParent(_storage1.transform);
                    _currentObject.GetComponent<UnitInfo>()._myTrunkID = _storage1.transform.childCount;
                    CalculateTreeMass(_currentObject);
                }
            }
            float _previousWeight = _rootObject.GetComponent<Rigidbody>().mass;
            _rootObject.GetComponent<Rigidbody>().mass = _previousWeight - _objectWeight;
            _storage1.GetComponent<Rigidbody>().mass = _objectWeight;
        }
    }
    public void CalculateTreeMass(GameObject _currentObj)
    {
        if (_currentObj.transform.childCount > 1)
        {
            for (int i = 0; i < _currentObj.transform.childCount - 1; i++)
            {
                _objectWeight += _currentObj.transform.GetChild(i + 1).GetComponent<UnitInfo>()._unitMass;
                if (_currentObj.transform.childCount > 1)
                {
                    CalculateTreeMass(_currentObj.transform.GetChild(i + 1).gameObject);
                }
            }
        }
        _objectWeight += _currentObj.GetComponent<UnitInfo>()._unitMass;
    }
}
