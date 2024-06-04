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

        // Mostrar UI de selecci�n de ataque
        ShowAttackSelectionUI();

        // Esperar a que el jugador seleccione un ataque
        yield return new WaitUntil(() => playerHasSelectedAttack);

        // L�gica de ataque del jugador
        ExecutePlayerAttack(player, selectedAttack);

        // Esconder UI de selecci�n de ataque
        HideAttackSelectionUI();

        yield return new WaitForSeconds(2f);
    }

    IEnumerator EnemyTurn(CharacterStats enemy)
    {
        combatLog.text = enemy.characterName + "'s turn.";
        yield return new WaitForSeconds(1f); // A�adir un peque�o retraso para la acci�n del enemigo

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
        // L�gica para realizar el ataque del jugador
        EnemyController target = SelectTarget(); // Implementa l�gica para seleccionar el objetivo

        switch (attackType)
        {
            case AttackType.Physical:
                target.TakeDamage(CalculateDamage(player, target.enemyStats, attackType));
                break;
            case AttackType.Magic:
                target.TakeDamage(CalculateDamage(player, target.enemyStats, attackType));
                break;
        }

        combatLog.text = player.characterName + " used " + attackType + " attack on " + target.enemyStats.characterName + ".";
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
        // Implementa l�gica para seleccionar un objetivo (por ahora selecciona el primer enemigo vivo)
        foreach (var enemy in enemies)
        {
            if (enemy.GetCurrentHealth() > 0)
            {
                return enemy;
            }
        }
        return null;
    }
    EnemyController SelectEnemyTarget()
    {
        // Implementa l�gica para seleccionar un objetivo (por ahora selecciona el primer enemigo vivo)
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
        // Muestra la UI de selecci�n de ataque
    }

    void HideAttackSelectionUI()
    {
        // Esconde la UI de selecci�n de ataque
    }
}
