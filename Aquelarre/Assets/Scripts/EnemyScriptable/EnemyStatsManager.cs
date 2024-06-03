using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyStats", menuName = "Enemy Stats")]
public class EnemyStatsManager : ScriptableObject
{
    public int maxHealth;
    public int strength;
    public int dexterity;
    public int intelligence;
    public int defense;
}