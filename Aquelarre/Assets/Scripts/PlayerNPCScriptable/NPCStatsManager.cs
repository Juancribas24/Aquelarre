using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewNPCStats", menuName = "NPC Stats")]
public class NPCStatsManager : ScriptableObject
{
    public int maxHealth;
    public int strength;
    public int dexterity;
    public int intelligence;
    public int defense;
}
