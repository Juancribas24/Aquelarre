using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackLogic : MonoBehaviour
{
    public void Attack(CharacterStats attacker, CharacterStats defender)
    {
        int damage = CalculateDamage(attacker, defender);
        defender.health -= damage;
        if (defender.health < 0)
        {
            defender.health = 0;
        }
    }

    int CalculateDamage(CharacterStats attacker, CharacterStats defender)
    {
        int damage = attacker.attack + Random.Range(0, attacker.intelligence) - defender.defense;
        if (damage < 0)
        {
            damage = 0;
        }
        return damage;
    }
}
