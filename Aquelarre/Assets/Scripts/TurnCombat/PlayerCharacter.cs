using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    public NPCStatsManager npcStats;
    public GameObject magicAttackPrefab; // Prefab del ataque m�gico
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
        // Aqu� no iniciamos autom�ticamente el ataque, sino que esperamos a que el jugador haga clic en el bot�n de la UI.
    }

    public void PerformMagicAttack(Character target)
    {
        StartCoroutine(ExecuteMagicAttack(target));
    }

    IEnumerator ExecuteMagicAttack(Character target)
    {
        // Instanciar el prefab del ataque m�gico
        GameObject attackInstance = Instantiate(magicAttackPrefab, transform.position, Quaternion.identity);

        // Aqu� puedes agregar animaciones o efectos especiales antes de aplicar el da�o

        yield return new WaitForSeconds(2.0f); // Duraci�n de la habilidad

        // Calcular el da�o basado en la inteligencia del jugador y la defensa del enemigo
        int damage = CalculateDamage(target);

        // Aplicar el da�o al enemigo
        target.TakeDamage(damage);

        // Destruir el prefab del ataque despu�s de usarlo
        Destroy(attackInstance);

        // Terminar el turno
        TurnBasedCombat.Instance.EndTurn();
    }

    int CalculateDamage(Character target)
    {
        Enemy enemyComponent = target.GetComponent<Enemy>();
        if (enemyComponent != null)
        {
            int attackPower = npcStats.intelligence;
            int enemyDefense = enemyComponent.enemyStats.defense; // Asumiendo que agregaste 'defense' en EnemyStatsManager
            return Mathf.Max(0, attackPower - enemyDefense); // Asegurarse de que el da�o no sea negativo
        }
        return 0;
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
