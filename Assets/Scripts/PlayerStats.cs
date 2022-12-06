using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int health = 100;
    public int maxHealth = 100;
    public int stamina = 100;

    public HealthBar healthBar;
    // Update is called once per frame
    private void Start()
    {
        healthBar.SetMaxHealth(maxHealth);
        health = maxHealth;
    }

    void Update()
    {
        healthBar.SetHealth(health);
    }
}
