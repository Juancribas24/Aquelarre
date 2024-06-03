using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    public NPCStatsManager npcStats;
    public GameObject magicAttackPrefab; // Prefab del ataque mágico
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
        // Aquí no iniciamos automáticamente el ataque, sino que esperamos a que el jugador haga clic en el botón de la UI.
    }

    public void PerformMagicAttack(Character target)
    {
        StartCoroutine(ExecuteMagicAttack(target));
    }

    IEnumerator ExecuteMagicAttack(Character target)
    {
        // Instanciar el prefab del ataque mágico
        GameObject attackInstance = Instantiate(magicAttackPrefab, transform.position, Quaternion.identity);

        // Aquí puedes agregar animaciones o efectos especiales antes de aplicar el daño

        yield return new WaitForSeconds(2.0f); // Duración de la habilidad

        // Calcular el daño basado en la inteligencia del jugador y la defensa del enemigo
        int damage = CalculateDamage(target);

        // Aplicar el daño al enemigo
        target.TakeDamage(damage);

        // Destruir el prefab del ataque después de usarlo
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
            return Mathf.Max(0, attackPower - enemyDefense); // Asegurarse de que el daño no sea negativo
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
