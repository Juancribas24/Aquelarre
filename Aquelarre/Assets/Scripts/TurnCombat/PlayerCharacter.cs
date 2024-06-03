using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    public NPCStatsManager npcStats;
    private int currentHealth;

    void Start()
    {
        if (npcStats != null)
        {
            currentHealth = npcStats.maxHealth;
        }
        else
        {
            Debug.LogError("NPCStats not assigned for " + gameObject.name);
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
        return npcStats.strength + diceRoll;
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
