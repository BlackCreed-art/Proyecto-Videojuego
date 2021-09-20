using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLife : MonoBehaviour
{
    // Start is called before the first frame update

    public int maxHealth = 100;
    public int currentHealth = 0;
    public Healthbar healthBar;

    void Start()
    {
         // healthBar = GameObject.FindGameObjectWithTag("Player").GetComponent<Healthbar>();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
    }
}

