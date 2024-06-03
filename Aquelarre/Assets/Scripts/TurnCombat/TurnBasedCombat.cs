using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class TurnBasedCombat : MonoBehaviour
{
    public List<PlayerCharacter> playerCharacters;
    public List<Character> enemyCharacters;
    private Queue<MonoBehaviour> turnOrder;
    public GameObject combatUI; // Referencia a la UI del combate

    public static TurnBasedCombat Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        InitializeCombat();
    }

    void InitializeCombat()
    {
        // Deshabilitar el movimiento del jugador
        foreach (PlayerCharacter player in playerCharacters)
        {
            player.GetComponent<PlayerMovInputSystem>().canMove = false;
        }

        // Mostrar la UI del combate
        combatUI.SetActive(true);

        StartCoroutine(DetermineTurnOrder());
    }

    IEnumerator DetermineTurnOrder()
    {
        DiceRoller.Instance.RollDice();
        yield return new WaitForSeconds(DiceRoller.Instance.rotationTime);

        List<MonoBehaviour> characters = new List<MonoBehaviour>();
        characters.AddRange(playerCharacters);
        characters.AddRange(enemyCharacters);
        characters.Sort((x, y) => DiceRoller.Instance.GetLastRoll().CompareTo(DiceRoller.Instance.GetLastRoll()));
        turnOrder = new Queue<MonoBehaviour>(characters);

        StartNextTurn();
    }

    void StartNextTurn()
    {
        if (turnOrder.Count > 0)
        {
            MonoBehaviour currentCharacter = turnOrder.Dequeue();
            if (currentCharacter is PlayerCharacter playerCharacter)
            {
                // Esperar a que el jugador haga clic en el botón de ataque
                // No llamamos automáticamente a TakeTurn
            }
            else if (currentCharacter is Character enemyCharacter)
            {
                enemyCharacter.TakeTurn(this);
            }
        }
        else
        {
            InitializeCombat();
        }
    }

    public void EndTurn()
    {
        StartNextTurn();
    }

    public void OnMagicAttackButtonPressed()
    {
        // Seleccionar el jugador y el enemigo objetivo (puedes mejorar esto para seleccionar enemigos específicos)
        PlayerCharacter currentPlayer = turnOrder.Peek() as PlayerCharacter;
        if (currentPlayer != null)
        {
            Character targetEnemy = enemyCharacters[Random.Range(0, enemyCharacters.Count)];
            currentPlayer.PerformMagicAttack(targetEnemy);
            turnOrder.Dequeue(); // Eliminar el jugador de la cola ya que está realizando su turno
            Debug.Log("Se usó");
        }
        Debug.Log("No se usó");
    }
}
