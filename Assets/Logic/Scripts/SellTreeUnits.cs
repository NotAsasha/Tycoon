using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellTreeUnits : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Tree")) {
            PlayerPrefs.SetFloat("PlayerMoney", PlayerPrefs.GetFloat("PlayerMoney") + 1);
            Destroy(other.gameObject);
        }
    }
}
