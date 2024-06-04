using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TurnBasedCombatSystem : MonoBehaviour
{
    public PlayerController[] playerControllers;
    public EnemyController[] enemies;
    public TextMeshProUGUI combatLog;

    private int currentPlayerIndex = 0;
    private int currentEnemyIndex = 0;
    private bool isPlayerTurn = true;
    private bool playerHasSelectedAttack = false;
    private AttackType selectedAttack;

    void Start()
    {
        StartCoroutine(StartCombat());
    }

    IEnumerator StartCombat()
    {
        while (!IsCombatOver())
        {
            if (isPlayerTurn)
            {
                yield return PlayerTurn(playerControllers[currentPlayerIndex].playerStats);
                currentPlayerIndex = (currentPlayerIndex + 1) % playerControllers.Length;
            }
            else
            {
                yield return EnemyTurn(enemies[currentEnemyIndex].enemyStats);
                currentEnemyIndex = (currentEnemyIndex + 1) % enemies.Length;
            }

            isPlayerTurn = !isPlayerTurn;
        }
        EndCombat();
    }

    IEnumerator PlayerTurn(CharacterStats player)
    {
        combatLog.text = player.characterName + "'s turn.";
        playerHasSelectedAttack = false;
        selectedAttack = AttackType.Physical; // Default or placeholder

        // Mostrar UI de selección de ataque
        ShowAttackSelectionUI();

        // Esperar a que el jugador seleccione un ataque
        yield return new WaitUntil(() => playerHasSelectedAttack);

        // Lógica de ataque del jugador
        ExecutePlayerAttack(player, selectedAttack);

        // Esconder UI de selección de ataque
        HideAttackSelectionUI();

        yield return new WaitForSeconds(2f);
    }

    IEnumerator EnemyTurn(CharacterStats enemy)
    {
        combatLog.text = enemy.characterName + "'s turn.";
        yield return new WaitForSeconds(1f); // Añadir un pequeño retraso para la acción del enemigo

        // Seleccionar un objetivo (un jugador) al azar
        PlayerController target = SelectRandomPlayerTarget();

        if (target != null)
        {
            // Realizar el ataque
            ExecuteEnemyAttack(enemy, target);
            combatLog.text = enemy.characterName + " attacked " + target.playerStats.characterName + ".";
        }

        yield return new WaitForSeconds(2f); // Esperar un poco antes de pasar el turno
    }

    bool IsCombatOver()
    {
        bool playersAlive = false;
        bool enemiesAlive = false;

        foreach (var player in playerControllers)
        {
            if (player.GetCurrentHealth() > 0)
            {
                playersAlive = true;
                break;
            }
        }

        foreach (var enemy in enemies)
        {
            if (enemy.GetCurrentHealth() > 0)
            {
                enemiesAlive = true;
                break;
            }
        }

        return !(playersAlive && enemiesAlive);
    }

    void EndCombat()
    {
        combatLog.text = "Combat is over!";
        // Implement end of combat logic here
    }

    public void PlayerSelectedAttack(AttackType attackType)
    {
        selectedAttack = attackType;
        playerHasSelectedAttack = true;
    }

    void ExecutePlayerAttack(CharacterStats player, AttackType attackType)
    {
        // Lógica para realizar el ataque del jugador
        EnemyController target = SelectTarget(); // Implementa lógica para seleccionar el objetivo

        if (target != null)
        {
            int damage = CalculateDamage(player, target.enemyStats, attackType);
            target.TakeDamage(damage);
            combatLog.text = player.characterName + " used " + attackType + " attack on " + target.enemyStats.characterName + " for " + damage + " damage.";
        }
    }

    void ExecuteEnemyAttack(CharacterStats enemy, PlayerController target)
    {
        int damage = CalculateDamage(enemy, target.playerStats, AttackType.Physical); // Puedes variar el tipo de ataque
        target.TakeDamage(damage);
        combatLog.text = enemy.characterName + " attacked " + target.playerStats.characterName + " for " + damage + " damage.";
    }

    int CalculateDamage(CharacterStats attacker, CharacterStats defender, AttackType attackType)
    {
        int damage = attacker.attack + Random.Range(0, attacker.intelligence) - defender.defense;
        if (damage < 0)
        {
            damage = 0;
        }
        return damage;
    }

    EnemyController SelectTarget()
    {
        // Implementa lógica para seleccionar un objetivo (por ahora selecciona el primer enemigo vivo)
        foreach (var enemy in enemies)
        {
            if (enemy.GetCurrentHealth() > 0)
            {
                return enemy;
            }
        }
        return null;
    }

    PlayerController SelectRandomPlayerTarget()
    {
        // Seleccionar un jugador vivo al azar
        PlayerController[] alivePlayers = System.Array.FindAll(playerControllers, player => player.GetCurrentHealth() > 0);
        if (alivePlayers.Length > 0)
        {
            return alivePlayers[Random.Range(0, alivePlayers.Length)];
        }
        return null;
    }

    void ShowAttackSelectionUI()
    {
        // Muestra la UI de selección de ataque
    }

    void HideAttackSelectionUI()
    {
        // Esconde la UI de selección de ataque
    }
}
