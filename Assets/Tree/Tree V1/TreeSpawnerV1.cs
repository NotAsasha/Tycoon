using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSpawnerV1 : MonoBehaviour
{
    [Header("Tree Samples")]
    [SerializeField] private List<GameObject> _treeUnitSamples;
    [SerializeField] private List<GameObject> _branchUnitSamples;
    [Header("Tree Settings")]
    public float _treeUnitLenght = 0.2f;
    public int _minTreeHeight = 20;
    public int _maxTreeHeight = 40;
    public int _minNumberOfBrenches;
    public GameObject _treeObject;
    public GameObject _unitStorageSpawner;
    public GameObject _storage;


    [Header("Tree Info")]
    public int _treeLenght;

    [Header("Reset Tree")]
    public bool _treeReset = false;

    private GameObject _unitTrunkStorage;
    private GameObject _unitBranchStorage;
    private GameObject _branchStorage;
    private GameObject _treeUnit;
    public List<GameObject> _createdTrunkUnits;
    public List<GameObject> _createdBranchUnits;
    private int _numberOfBrenches;

    private void Awake()
    {
        _createdTrunkUnits = new List<GameObject>();
        _createdBranchUnits = new List<GameObject>();
    }
    void Start()
    {
        SpawnUnitStorage();
        SpawnTreeTrunk();
        SpawnTreeBrunches();
    }

    void Update()
    {
        if (_treeReset == true)
        {
            _treeReset = false;
            DestroyTreeTrunk();
            SpawnUnitStorage();
            SpawnTreeTrunk();
            SpawnTreeBrunches();
        }
    } 
    public void SpawnUnitStorage()
    {
        _unitTrunkStorage = Instantiate(_unitStorageSpawner, _treeObject.transform);
    }
    public void SpawnTreeTrunk()
    {
        _treeLenght = Random.Range(_minTreeHeight, _maxTreeHeight);

        float _spawnSpacing = 0.1f;
        for (int i = 0; i < _treeLenght; i++)
        {
            Vector3 _position = new Vector3(0, _spawnSpacing, 0);
            _treeUnit = Instantiate(_treeUnitSamples[Random.Range(0, _treeUnitSamples.Count)], _treeObject.transform.position + _position, Quaternion.identity);
            _treeUnit.transform.SetParent(_unitTrunkStorage.transform);
            _createdTrunkUnits.Add(_treeUnit);
            _spawnSpacing += _treeUnitLenght;
        }
    }
    public void SpawnTreeBrunches()
    {
        _branchStorage = Instantiate(_storage, _treeObject.transform);
        float _minSpawnHeight = _treeLenght - (_treeLenght / 3);
        float _maxBrunchLenght = _treeLenght / 1.7f;
        float _minBrunchLenght = _treeLenght / 3;
        float _branchUnitLenght = _treeUnitLenght * 2;
        _numberOfBrenches = Random.Range(_minNumberOfBrenches, _minNumberOfBrenches + 3);
        for (int ia = 0; ia < _numberOfBrenches; ia++)
        {
            float _brunchLenght = Random.Range(_minBrunchLenght, _maxBrunchLenght);
            float _branchRotationX = Random.Range(-60, 60);
            float _branchRotationZ = Random.Range(-60, 60);
            Vector3 _spawnSpacing = new Vector3(0f, 0.6f, 0f);
            int _brunchSpawnHeight = Mathf.RoundToInt(Random.Range(_minSpawnHeight, _treeLenght - 2));
            Vector3 _brunchSpawnPosition = _createdTrunkUnits[_brunchSpawnHeight].transform.position;

            _unitBranchStorage = Instantiate(_unitStorageSpawner, _brunchSpawnPosition, Quaternion.identity);
            _unitBranchStorage.transform.SetParent(_branchStorage.transform);
            for (int i = 0; i < _brunchLenght - 1; i++)
            {
                _treeUnit = Instantiate(_branchUnitSamples[Random.Range(0, _branchUnitSamples.Count)], _unitBranchStorage.transform.position + _spawnSpacing, Quaternion.identity);
                _treeUnit.transform.SetParent(_unitBranchStorage.transform);
                _createdBranchUnits.Add(_treeUnit);
                _spawnSpacing += new Vector3(0f, _branchUnitLenght, 0f);
            }
            _unitBranchStorage.transform.rotation = Quaternion.Euler(_branchRotationX, 0f, _branchRotationZ);
        }
    }



    public void DestroyTreeTrunk()
    {
        Destroy(_unitTrunkStorage);
        Destroy(_branchStorage);
        _createdTrunkUnits.Clear();
        _createdBranchUnits.Clear();
    }
}
