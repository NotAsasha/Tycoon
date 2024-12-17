using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSpawner : MonoBehaviour
{
    [Header("--- Tree Samples:")]
    [SerializeField] private List<GameObject> _treeUnitSamples;
    [SerializeField] private List<GameObject> _branchUnitSamples;
    [SerializeField] private List<GameObject> _leavesSamples;
    [Header("--- Tree Settings:")]
    public float _treeUnitLenght = 0.2f;
    public int _minTreeHeight = 20;
    public int _maxTreeHeight = 40;
    public int _minNumberOfBrenches;
    public GameObject _treeObject;
    public GameObject _unitStorageSpawner;
    public GameObject _storage;
    public bool _debugMode = false;


    [Header("--- Tree Info:")]
    public int _treeLenght;

    [Header("--- Reset Tree:")]
    public bool _treeReset = false;

    [Header("--- Lists:")]
    public List<GameObject> _treeUnits;
    public List<GameObject> _createdTrunkUnits;
    public List<GameObject> _createdBranchUnits;
    public List<List<GameObject>> _createdBranchesInBrunches;
    public List<GameObject> _createdBranchInBranchUnits;

    public List<List<GameObject>> _branches;

    private GameObject _unitTrunkStorage;
    private GameObject _unitBranchStorage;
    private GameObject _unitBranchInBrunchStorage;
    private GameObject _branchStorage;
    private GameObject _treeUnit;
    private int _numberOfBrenches;
    private SetBranchRotation _branchRotation;
    private bool _areBrunchesSpawnes = false;
    private float _branchRotationX;
    private float _branchRotationZ;
    private float _minSpawnHeight;
    private float _maxBrunchLenght;
    private float _minBrunchLenght;
    private float _branchUnitLenght;
    private UnitInfo _setID;


    private void Awake()
    {
        _treeUnits = new List<GameObject>();
        _branchRotation = GetComponent<SetBranchRotation>();
        _createdTrunkUnits = new List<GameObject>();
        _createdBranchesInBrunches = new List<List<GameObject>>();
        _branches = new List<List<GameObject>>();
        _branchRotationX = _branchRotation._branchRotationX;
        _branchRotationZ = _branchRotation._branchRotationZ;

    }
    void Start()
    {
        SpawnUnitStorage();
        SpawnTreeTrunk();
        StartCoroutine(SpawnTreeBrunches());
    }

    void Update()
    {
        if (_treeReset == true)
        {
            _treeReset = false;
            DestroyTreeTrunk();
            SpawnUnitStorage();
            SpawnTreeTrunk();
            StartCoroutine(SpawnTreeBrunches());
        }
    } 
    public void SpawnUnitStorage()
    {
        _unitTrunkStorage = Instantiate(_unitStorageSpawner, _treeObject.transform);
        _unitTrunkStorage.transform.SetParent(transform);
    }
    public void SpawnTreeTrunk()
    {
        /*Debug*/ if (_debugMode == true) Debug.LogWarning("Start Of SpawnTreeTrunk");
        _treeLenght = Random.Range(_minTreeHeight, _maxTreeHeight);

        float _spawnSpacing = 0.1f;
        for (int i = 0; i < _treeLenght; i++)
        {
            Vector3 _position = new Vector3(0, _spawnSpacing, 0);
            _treeUnit = Instantiate(_treeUnitSamples[Random.Range(0, _treeUnitSamples.Count)], _treeObject.transform.position + _position, Quaternion.identity);
            _treeUnit.transform.SetParent(_unitTrunkStorage.transform);
            _createdTrunkUnits.Add(_treeUnit);
            _treeUnits.Add(_treeUnit);
            _setID = _treeUnit.GetComponent<UnitInfo>();
            _setID._myTrunkID = i;
            _setID._unitMass = 5;
            _spawnSpacing += _treeUnitLenght;
        }
        /*Debug*/ if (_debugMode == true) Debug.LogWarning("End Of SpawnTreeTrunk");

    }
    public IEnumerator SpawnTreeBrunches()
    {
        /*Debug*/ if (_debugMode == true) Debug.LogWarning("Start Of SpawnTreeBrunches");
        _minSpawnHeight = Mathf.RoundToInt(_treeLenght - (_treeLenght / 3));
        _maxBrunchLenght = Mathf.RoundToInt(_treeLenght / 1.7f);
        _minBrunchLenght = Mathf.RoundToInt(_treeLenght / 3);
        _branchUnitLenght = _treeUnitLenght * 2;
        _numberOfBrenches = Random.Range(_minNumberOfBrenches, _minNumberOfBrenches + 3);
        /*Debug*/ if (_debugMode == true) Debug.Log("Branches Will Be Spawned - " + _numberOfBrenches);


        for (int ia = 0; ia < _numberOfBrenches; ia++)
        {
            /*Debug*/ if (_debugMode == true) Debug.Log("StartOfSpawnBranchesLoop");
            float _brunchLenght = Random.Range(_minBrunchLenght, _maxBrunchLenght);
            Vector3 _spawnSpacing = new Vector3(0f, 0.6f, 0f);
            int _brunchSpawnHeight = Mathf.RoundToInt(Random.Range(_minSpawnHeight, _treeLenght - 2));
            Vector3 _brunchSpawnPosition = _createdTrunkUnits[_brunchSpawnHeight].transform.position;
            _createdBranchUnits = new List<GameObject>();
            _branchRotation._haveRotation = false;


            /*Debug*/ if (_debugMode == true) Debug.Log("Instantiate _unitBranchStorage");
            _unitBranchStorage = Instantiate(_unitStorageSpawner, _brunchSpawnPosition, Quaternion.identity);
            _unitBranchStorage.transform.SetParent(_createdTrunkUnits[_brunchSpawnHeight].transform);
            /*Debug*/ if (_debugMode == true) Debug.Log("Start Of _treeUnit Spawn Loop");
            for (int i = 0; i < _brunchLenght; i++)
            {
                _treeUnit = Instantiate(_branchUnitSamples[Random.Range(0, _branchUnitSamples.Count)], _unitBranchStorage.transform.position + _spawnSpacing, Quaternion.identity);
                _treeUnit.transform.SetParent(_unitBranchStorage.transform);
                _createdBranchUnits.Add(_treeUnit);
                _setID = _treeUnit.GetComponent<UnitInfo>();
                _setID._idOfMyBranch = ia;
                _setID._myTrunkID = i + 1;
                _setID._unitMass = 3;

                _spawnSpacing += new Vector3(0f, _branchUnitLenght, 0f);
            }
            _branches.Add(_createdBranchUnits);
            /*Debug*/ if (_debugMode == true) Debug.LogWarning("Number Of Branches - " + _branches.Count);

            StartCoroutine(SpawnBrunchesOnBrunches());
            yield return new WaitUntil(() => _areBrunchesSpawnes == true);
            int _whatBranchesAreSpawning = 1;
            StartCoroutine(ChooseRotation(_whatBranchesAreSpawning));
            yield return new WaitUntil(() => _branchRotation._haveRotation == true || _branchRotation._errorCount >= 20);
            if (_branchRotation._errorCount >= 20)
            {
                /*Debug*/ if (_debugMode == true) Debug.LogError("StoppingSpawning, too many errors");
                Destroy(_unitBranchStorage);
                yield return false;
                yield break;
            }
            else _unitBranchStorage.transform.rotation = Quaternion.Euler(_branchRotationX, 0f, _branchRotationZ);

        }


        _branchRotation._isFirst = true;
        _branchRotation._rotationsX.Clear();
        _branchRotation._rotationsZ.Clear();
        _branchRotation._errorCount = 0;
        /*Debug*/ if (_debugMode == true) Debug.LogWarning("End Of SpawnTreeBrunches");
    }



    public IEnumerator SpawnBrunchesOnBrunches()
    {
        /*Debug*/ if (_debugMode == true) Debug.LogWarning("Start Of SpawnBrunchesOnBrunches");
        int _numberOfCycles = 0;
        for (int ia = 0; ia < 2; ia++)
        {
            /*Debug*/ if (_debugMode == true) Debug.LogWarning("Start Of SpawnBrunchesOnBrunches Loop");
            Vector3 _spawnSpacing = new Vector3(0f, 0.6f, 0f);
            List<GameObject> _specificBrench = _branches[_branches.Count - 1];
            float _brunchLenght = Random.Range(_minBrunchLenght / 2, _maxBrunchLenght / 2);  Mathf.RoundToInt(_brunchLenght);
            Vector3 _brunchSpawnPosition = _specificBrench[_specificBrench.Count - 1].transform.position;
            _createdBranchInBranchUnits = new List<GameObject>();


            _unitBranchInBrunchStorage = Instantiate(_unitStorageSpawner, _brunchSpawnPosition, Quaternion.identity);
            _unitBranchInBrunchStorage.transform.SetParent(_unitBranchStorage.transform);


            for (int i = 0; i < _brunchLenght; i++)
            {
                _treeUnit = Instantiate(_branchUnitSamples[Random.Range(0, _branchUnitSamples.Count)], _specificBrench[_specificBrench.Count - 1].transform.position + _spawnSpacing, Quaternion.identity);
                _treeUnit.transform.SetParent(_unitBranchInBrunchStorage.transform);
                _spawnSpacing += new Vector3(0f, _branchUnitLenght, 0f);
                _createdBranchInBranchUnits.Add(_treeUnit);
 
                _setID = _treeUnit.GetComponent<UnitInfo>();
                _setID._idOfMyBranchInBranch = ia;
                _setID._myTrunkID = i;
                _setID._unitMass = 1;

            }
            _createdBranchUnits.Add(_unitBranchInBrunchStorage);

            int _whatBranchesAreSpawning = 2;
            StartCoroutine(ChooseRotation(_whatBranchesAreSpawning));
            yield return new WaitUntil(() => _branchRotation._haveRotation == true || _branchRotation._errorCount >= 20);
            if (_branchRotation._errorCount >= 20)
            {
                yield return false;
                yield break;
            }
            float _branchRotationZ = _branchRotation._branchRotationZ;
            if (_debugMode == true) Debug.LogError("Взяло оберт для палки: " + _branchRotationX + " i " + _branchRotationZ);
            _unitBranchInBrunchStorage.transform.rotation = Quaternion.Euler(_branchRotationX, 0f, _branchRotationZ);

            _setID = _unitBranchInBrunchStorage.GetComponent<UnitInfo>();
            _setID._idOfMyBranchInBranch = ia;
            _setID._myTrunkID = _createdBranchUnits.Count;
            _createdBranchesInBrunches.Add(_createdBranchInBranchUnits);
            _numberOfCycles += 1;
        }
        _areBrunchesSpawnes = true;  
    }

    public IEnumerator ChooseRotation(int _whatBranchesAreSpawning)
    {
        if (_debugMode == true) Debug.LogWarning("Початок ChooseRotation");
        if (_debugMode == true) Debug.LogWarning("Почало брати оберт!");
        _branchRotation._haveRotation = false;
        _branchRotation.ChooseBranchPosition();
        yield return new WaitUntil(() => _branchRotation._haveRotation == true || _branchRotation._errorCount >= 20);
        if (_branchRotation._errorCount >= 20)
        {
            yield return false;
            yield break;
        }
        _branchRotationX = _branchRotation._branchRotationX;
        _branchRotationZ = _branchRotation._branchRotationZ;
        if (_debugMode == true) Debug.LogWarning("Взяло оберт: " + _branchRotationX + " i " + _branchRotationZ);
        switch (_whatBranchesAreSpawning)
        {
            case 1:
                _branchRotation._rotationsX.Add(_branchRotationX);
                _branchRotation._rotationsZ.Add(_branchRotationZ);
                break;
            case 2:
                break;


        }

    }

    public void DestroyTreeTrunk()
    {
        Destroy(_unitTrunkStorage);
        _createdTrunkUnits.Clear();
        _createdBranchUnits.Clear();
        _branches.Clear();
        _treeUnits.Clear();
        _branchRotation._rotationsX.Clear();
        _branchRotation._rotationsZ.Clear();
        _branchRotation._isFirst = true;
        _branchRotation._errorCount = 0;
        _branchRotation._haveRotation = false;

    }
}
