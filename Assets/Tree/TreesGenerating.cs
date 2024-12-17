using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreesGenerating : MonoBehaviour
{
    [Header("Spawn settings")]
    public GameObject _treePrefab;
    public float spawnChance;

    [Header("Raycast setup")]
    public float distanceBetweenCheck;
    public float heightOfCheck = 10f, rangeOfCheck = 30f;
    public LayerMask _treeSpawnLayer;
    public Vector2 positivePosition, negativePosition;


    void Start()
    {
        SpawnTrees();
    }

    public void SpawnTrees()
    {
        for (float x = negativePosition.x; x < positivePosition.x; x += distanceBetweenCheck)
        {
            for (float z = negativePosition.y; z < positivePosition.y; z += distanceBetweenCheck)
            {
                RaycastHit hit;
                if (Physics.Raycast(new Vector3(x, heightOfCheck, z), Vector3.down, out hit, rangeOfCheck, _treeSpawnLayer))
                {
                    if (spawnChance > Random.Range(0f, 101f))
                    {
                        Instantiate(_treePrefab, hit.point, Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)), transform);
                    }
                }
            }
        }
    }

    void DeleteResources()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}
