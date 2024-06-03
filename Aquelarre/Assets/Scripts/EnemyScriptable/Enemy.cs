using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyStatsManager enemyStats;

    private int currentHealth;

    void Start()
    {
        if (enemyStats != null)
        {
            currentHealth = enemyStats.maxHealth;
        }
        else
        {
            Debug.LogError("EnemyStats not assigned for " + gameObject.name);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    protected virtual void Die()
    {
        Debug.Log(gameObject.name + " has died.");
        Destroy(gameObject);
    }
}
