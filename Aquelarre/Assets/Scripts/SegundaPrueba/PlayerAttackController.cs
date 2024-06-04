using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{
    public TurnBasedCombatSystem combatSystem;

    public void OnPhysicalAttack()
    {
        combatSystem.PlayerSelectedAttack(AttackType.Physical);
    }

    public void OnMagicAttack()
    {
        combatSystem.PlayerSelectedAttack(AttackType.Magic);
    }
}

public enum AttackType
{
    Physical,
    Magic
}
