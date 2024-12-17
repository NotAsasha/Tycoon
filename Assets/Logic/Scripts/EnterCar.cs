using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterCar : MonoBehaviour
{
    public bool ISCAR;
    public GameObject Playerr;
    public GameObject Carr;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (ISCAR == true)
            {
                Playerr.SetActive(false);
                Carr.SetActive(true);
                ISCAR = false;
            }
            else
            {

                Playerr.SetActive(true);
                Carr.SetActive(false);
                ISCAR = true;
               
            }
        }
    }
}
