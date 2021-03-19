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
        this.Force = force;
        this.KnockBackTime = knockbackTime;
    }
}
