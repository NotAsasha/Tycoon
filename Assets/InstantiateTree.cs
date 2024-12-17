using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateTree : MonoBehaviour
{
    public GameObject _treePrefab;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.C))    Instantiate(_treePrefab);
            
    }
}
