using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Bag : MonoBehaviour
{
    [SerializeField] private int currentLog;
    [SerializeField] private TextMeshProUGUI logQuantityText;

    private void Start()
    {
        currentLog = 0;
    }

    private void Update()
    {
        logQuantityText.text = "Bag: " + currentLog;
    }

    private void GetLog()
    {
        currentLog += 1;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Log"))
        {
            GetLog();
            Destroy(other.gameObject);
        }
    }
}
