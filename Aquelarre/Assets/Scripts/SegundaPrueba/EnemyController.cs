using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    public CharacterStats enemyStats;
    public TextMeshProUGUI healthText;
    public Slider healthBar;
    public Animator animator; // Referencia al Animator

    private int currentHealth;

    void Start()
    {
        // Aqu� no hacemos nada ya que la inicializaci�n ser� manejada por TurnBasedCombatSystem
    }

    public void InitializeEnemy()
    {
        if (enemyStats != null)
        {
            currentHealth = enemyStats.health;
            Debug.Log(gameObject.name + " initialized with health: " + currentHealth);
            UpdateHealthUI();
        }
        else
        {
            Debug.LogError("EnemyStats not assigned in " + gameObject.name);
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

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void UpdateHealthUI()
    {
        healthText.text = "HP: " + currentHealth + "/" + enemyStats.health;
        healthBar.value = (float)currentHealth / enemyStats.health;
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    void Die()
    {
        // L�gica para manejar la muerte del enemigo
        Debug.Log(enemyStats.characterName + " has died.");
        // Puedes a�adir animaciones de muerte u otras l�gicas aqu�
    }
}
