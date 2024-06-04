using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character Stats", menuName = "Stats/Character Stats")]
public class CharacterStats : ScriptableObject
{
    public string characterName;
    public int health;
    public int dexterity;
    public int attack;
    public int defense;
    public int intelligence;
}
