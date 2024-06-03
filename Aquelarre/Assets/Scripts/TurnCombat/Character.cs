using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private Enemy enemy; // Referencia al script Enemy
    private int currentHealth;

    void Start()
    {
        enemy = GetComponent<Enemy>();
        if (enemy != null)
        {
            currentHealth = enemy.enemyStats.maxHealth;
        }
        else
        {
            Debug.LogError("Enemy component not found on " + gameObject.name);
        }
    }

    public void TakeTurn(TurnBasedCombat combat)
    {
        StartCoroutine(PerformAttack(combat));
    }

    IEnumerator PerformAttack(TurnBasedCombat combat)
    {
        DiceRoller.Instance.RollDice();
        yield return new WaitForSeconds(DiceRoller.Instance.rotationTime);

        int diceRoll = DiceRoller.Instance.GetLastRoll();
        int damage = CalculateDamage(diceRoll);

        // Example attack on a random enemy
        //Character target = combat.allCharacters[Random.Range(0, combat.allCharacters.Count)];
        //target.TakeDamage(damage);

        // End the turn
        combat.EndTurn();
    }

    int CalculateDamage(int diceRoll)
    {
        return enemy.enemyStats.strength + diceRoll;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log(gameObject.name + " has died.");
        Destroy(gameObject);
    }
}
