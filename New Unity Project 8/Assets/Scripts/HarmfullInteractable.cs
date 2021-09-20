using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarmfullInteractable : MonoBehaviour
{
   public Healthbar healthBar;
    public int maxHealth = 100;
    public int currentHealth = 0;
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            TakeDamage(20);

            //collider.gameObject.GetComponentAgregarItem("Algo");
        }
    }
    void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
    }
}
