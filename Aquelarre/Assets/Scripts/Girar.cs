using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Girar : MonoBehaviour
{
    public void OnRollDiceButtonPressed()
    {
        DiceRoller.Instance.RollDice();
    }
}
