using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySelector : MonoBehaviour
{
    public Button[] enemyButtons;
    private TurnBasedCombatSystem combatSystem;

    void Start()
    {
        combatSystem = FindObjectOfType<TurnBasedCombatSystem>();

        for (int i = 0; i < enemyButtons.Length; i++)
        {
            int index = i; // Necesario para evitar problemas de cierre de variable en el loop
            enemyButtons[i].onClick.AddListener(() => SelectEnemy(index));
        }
    }

    void SelectEnemy(int index)
    {
        combatSystem.SetSelectedEnemy(index);
    }
}
