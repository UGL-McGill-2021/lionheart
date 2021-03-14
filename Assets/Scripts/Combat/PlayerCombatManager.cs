using System.Collections;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Author: Feiyang Li
/// CombatManager for player
/// </summary>
public class PlayerCombatManager : CombatManager {

    ControllerInput input;

    public void Awake() {
        this.body = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// Author: Feiyang Li
    /// Add a basic kick motion
    /// </summary>
    public void Start() {
        Kick kickMotion = new Kick(10, 5);
        this.mPossibleAttacks.Add("Kick", kickMotion);
        input = new ControllerInput();
        input.Player.Kick.performed += _ => Attack("Kick");
        input.Enable();
    }

    public override IEnumerator Knockback(int KnockBackTimeSpan) {
        Debug.Log("CombatManager: Knockback invoked on player " + gameObject);
        return null;
    }


}
