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
        // Inicializar todos los controladores de jugadores y enemigos
        InitializeControllers();

        // Iniciar la rutina de combate
        StartCoroutine(StartCombat());
    }

    void InitializeControllers()
    {
        foreach (var player in playerControllers)
        {
            player.InitializePlayer();
        }

        foreach (var enemy in enemies)
        {
            enemy.InitializeEnemy();
        }
    }

    IEnumerator StartCombat()
    {
        // Verificar si hay jugadores y enemigos vivos antes de empezar el combate
        if (IsCombatOver())
        {
            EndCombat();
            yield break;
        }

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

        // Lanzar el dado
        DiceRoller.Instance.RollDice();

        // Esperar a que el dado termine de girar
        yield return new WaitForSeconds(DiceRoller.Instance.rotationTime);

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
            Debug.Log("Player " + player.name + " health: " + player.GetCurrentHealth());
            if (player.GetCurrentHealth() > 0)
            {
                playersAlive = true;
                break;
            }
        }

        foreach (var enemy in enemies)
        {
            Debug.Log("Enemy " + enemy.name + " health: " + enemy.GetCurrentHealth());
            if (enemy.GetCurrentHealth() > 0)
            {
                enemiesAlive = true;
                break;
            }
        }

        Debug.Log("Players Alive: " + playersAlive);
        Debug.Log("Enemies Alive: " + enemiesAlive);

        return !(playersAlive && enemiesAlive);
    }

    void EndCombat()
    {
        combatLog.text = "Combat is over!";
        // Implement end of combat logic here
        Debug.Log("Combat Ended");
    }

    public void PlayerSelectedAttack(AttackType attackType)
    {
        selectedAttack = attackType;
        playerHasSelectedAttack = true;
    }

    void ExecutePlayerAttack(CharacterStats player, AttackType attackType)
    {
        // Obtener el resultado del dado
        int diceRoll = DiceRoller.Instance.GetLastRoll();

        // Calcular el tiro de ataque
        int attackRoll = diceRoll + player.attack;

        // Verificar si es un golpe crítico (20 natural en el dado)
        bool isCriticalHit = (diceRoll == 20);

        // Seleccionar el objetivo
        EnemyController target = SelectTarget();

        if (target != null)
        {
            int damage = 0;
            if (isCriticalHit)
            {
                // Golpe crítico
                damage = Mathf.CeilToInt(CalculateDamage(player, target.enemyStats, attackType) * 1.5f);
                combatLog.text = player.characterName + " scored a critical hit on " + target.enemyStats.characterName + " for " + damage + " damage!";
            }
            else if (attackRoll > target.enemyStats.defense)
            {
                // Ataque normal
                damage = CalculateDamage(player, target.enemyStats, attackType);
                combatLog.text = player.characterName + " used " + attackType + " attack on " + target.enemyStats.characterName + " for " + damage + " damage.";
            }
            else
            {
                // El ataque falla
                combatLog.text = player.characterName + "'s attack missed due to low attack roll.";
            }

            // Aplicar el daño
            target.TakeDamage(damage);
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
