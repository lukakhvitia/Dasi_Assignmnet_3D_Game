using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    public int currentHealth;

    [SerializeField] private TextMeshProUGUI healthText;

    public bool isAlive = true;
 
    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        healthText.text = ("Health: " + currentHealth.ToString());
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            isAlive = false;
        }
    }
}
