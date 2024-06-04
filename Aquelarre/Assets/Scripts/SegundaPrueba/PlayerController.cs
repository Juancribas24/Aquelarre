using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public CharacterStats playerStats;
    public TextMeshProUGUI healthText;
    public Slider healthBar;

    private int currentHealth;

    void Start()
    {
        // Aquí no hacemos nada ya que la inicialización será manejada por TurnBasedCombatSystem
    }

    public void InitializePlayer()
    {
        if (playerStats != null)
        {
            currentHealth = playerStats.health;
            Debug.Log(gameObject.name + " initialized with health: " + currentHealth);
            UpdateHealthUI();
        }
        else
        {
            Debug.LogError("PlayerStats not assigned in " + gameObject.name);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }
        UpdateHealthUI();
    }

    void UpdateHealthUI()
    {
        healthText.text = "HP: " + currentHealth + "/" + playerStats.health;
        healthBar.value = (float)currentHealth / playerStats.health;
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}
