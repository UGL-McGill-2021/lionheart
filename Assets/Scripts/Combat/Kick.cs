using UnityEngine;

/// <summary>
/// Author: Feiyang Li
/// Simple kick combat motion
/// </summary>
public class Kick : AttackMotion {

    /// <summary>
    /// Author: Feiyang Li
    /// Kick constructor
    /// </summary>
    /// <param name="force"></param>
    public Kick(float force, int knockbackTime) {
        this.mForce = force;
        this.mKnockBackTime = knockbackTime;
    }

    /// <summary>
    /// Author: Feiyang Li
    /// Kick logic
    /// </summary>
    /// <param name="instigator"></param>
    /// <param name="target"></param>
    public override void OnApplyAttack(CombatManager instigator, CombatManager target) {
        // find vector
        Vector3 direction = (target.transform.position - instigator.transform.position).normalized;
        Debug.Log("KickMotion: " + instigator.gameObject + " attacks " + target.gameObject + " with vector " + direction * this.mForce);

        target.StartAttackMovement(direction * mForce);
    }
}
