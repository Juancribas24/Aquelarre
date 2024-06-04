using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TurnBasedCombatSystem : MonoBehaviour
{
    public PlayerController[] playerControllers;
    public EnemyController[] enemies;
    public TextMeshProUGUI combatLog;
    public GameObject enemySelectionUI; // UI para la selección de enemigos

    private int currentPlayerIndex = 0;
    private int currentEnemyIndex = 0;
    private bool isPlayerTurn = true;
    private bool playerHasSelectedAttack = false;
    private AttackType selectedAttack;
    private EnemyController currentTarget;
    private int selectedEnemyIndex = -1;

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

        // Mostrar UI de selección de enemigos
        ShowEnemySelectionUI();

        // Esperar a que el jugador seleccione un enemigo
        yield return new WaitUntil(() => selectedEnemyIndex >= 0);

        // Seleccionar el objetivo del ataque
        currentTarget = SelectTarget();

        // Lógica de ataque del jugador
        yield return ExecutePlayerAttack(player, selectedAttack, currentTarget);

        // Esconder UI de selección de ataque y enemigos
        HideAttackSelectionUI();
        HideEnemySelectionUI();

        // Resetear el índice de enemigo seleccionado
        selectedEnemyIndex = -1;

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

    public void SetSelectedEnemy(int index)
    {
        selectedEnemyIndex = index;
    }

    IEnumerator ExecutePlayerAttack(CharacterStats player, AttackType attackType, EnemyController target)
    {
        // Obtener el resultado del dado
        int diceRoll = DiceRoller.Instance.GetLastRoll();

        // Calcular el tiro de ataque
        int attackRoll = diceRoll + player.attack;

        // Verificar si es un golpe crítico (20 natural en el dado)
        bool isCriticalHit = (diceRoll == 20);

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
                yield break; // Terminar la coroutine si el ataque falla
            }

            // Reproducir la animación de ataque
            if (attackType == AttackType.Physical)
            {
                playerControllers[currentPlayerIndex].PlayPhysicalAttackAnimation();
                yield return new WaitForSeconds(1.0f); // Duración de la animación de ataque físico

                // Deslizarse hacia el enemigo
                Vector3 startPosition = playerControllers[currentPlayerIndex].transform.position;
                Vector3 endPosition = target.transform.position;
                float moveDuration = 0.5f;
                yield return MoveToPosition(playerControllers[currentPlayerIndex].transform, endPosition, moveDuration);

                // Realizar la animación de ataque
                playerControllers[currentPlayerIndex].PlayPhysicalAttackAnimation();
                yield return new WaitForSeconds(1.0f); // Duración de la animación de ataque físico

                // Volver a la posición original
                yield return MoveToPosition(playerControllers[currentPlayerIndex].transform, startPosition, moveDuration);
                playerControllers[currentPlayerIndex].StopPhysicalAttackAnimation();
            }
            else if (attackType == AttackType.Magic)
            {
                playerControllers[currentPlayerIndex].PlayMagicAttackAnimation();
                playerControllers[currentPlayerIndex].ExecuteMagicAttack(target.transform.position);
                yield return new WaitForSeconds(1.0f); // Duración de la animación de ataque mágico
                playerControllers[currentPlayerIndex].StopMagicAttackAnimation();
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
        // Implementa lógica para seleccionar un objetivo (por ahora selecciona el enemigo vivo seleccionado)
        if (selectedEnemyIndex >= 0 && selectedEnemyIndex < enemies.Length && enemies[selectedEnemyIndex].GetCurrentHealth() > 0)
        {
            return enemies[selectedEnemyIndex];
        }

        // Si no hay enemigo seleccionado, selecciona el primer enemigo vivo
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
        // Implementa tu lógica aquí
    }

    void HideAttackSelectionUI()
    {
        // Esconde la UI de selección de ataque
        // Implementa tu lógica aquí
    }

    void ShowEnemySelectionUI()
    {
        // Muestra la UI de selección de enemigos
        enemySelectionUI.SetActive(true);
    }

    void HideEnemySelectionUI()
    {
        // Esconde la UI de selección de enemigos
        enemySelectionUI.SetActive(false);
    }

    IEnumerator MoveToPosition(Transform transform, Vector3 position, float duration)
    {
        Vector3 initialPosition = transform.position;
        float timer = 0;

        while (timer < duration)
        {
            transform.position = Vector3.Lerp(initialPosition, position, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }

        transform.position = position;
    }
}
