using UnityEngine;

/// <summary>
/// Author: Feiyang Li
/// Base class for all attack activity
/// </summary>
public abstract class AttackMotion {
    public float mForce { get; set; }

    public int mKnockBackTime { get; set; }

    public abstract void OnApplyAttack(CombatManager instigator, CombatManager target);

}
