using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    [SerializeField]
    private Text _moneyShowed;
    private float _playerMoney;


    void Start()
    {

    }


    void Update()
    {
        _playerMoney = PlayerPrefs.GetFloat("PlayerMoney");
        _moneyShowed.text = "Tree Units Sold: " + _playerMoney.ToString();
    }
}
