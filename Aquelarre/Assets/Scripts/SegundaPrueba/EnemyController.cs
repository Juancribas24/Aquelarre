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

    private int currentHealth;

    void Start()
    {
        InitializeEnemy();
    }

    void InitializeEnemy()
    {
        currentHealth = enemyStats.health;
        UpdateHealthUI();
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
        healthText.text = "HP: " + currentHealth + "/" + enemyStats.health;
        healthBar.value = (float)currentHealth / enemyStats.health;
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}
