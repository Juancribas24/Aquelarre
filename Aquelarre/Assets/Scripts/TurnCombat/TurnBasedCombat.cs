using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class TurnBasedCombat : MonoBehaviour
{
    public List<PlayerCharacter> playerCharacters;
    public List<Character> enemyCharacters;
    private Queue<MonoBehaviour> turnOrder;

    void Start()
    {
        InitializeCombat();
    }

    void InitializeCombat()
    {
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
                playerCharacter.TakeTurn(this);
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
}
