using UnityEngine;
using Lionheart.Player.Movement;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Author: Feiyang Li
/// CombatManager for player
/// </summary>
public class PlayerCombatManager : MonoBehaviour {

    ControllerInput Input;
    Rigidbody Body;

    public Collider AttackBox;

    private AttackMotion CurrentAttackMotion;

    private bool IsAttacking;

    public float DefaultAttackForce = 10000;

    [Header("Dash Attack")]
    public float DashAttackAngleOfViewDegree;

    [Header("Smash")]
    public float SmashRadius = 1000;

    private void Awake() {
        this.Body = GetComponent<Rigidbody>();
    }

    private void Start() {
        Input = new ControllerInput();
        Input.Player.Kick.performed += _ => Attack(new Kick(DefaultAttackForce, 1));
        Input.Player.Smash.performed += _ => Smash(SmashRadius);
        Input.Enable();
    }

    public void Attack(AttackMotion _attackMotion) {
        AttackBox.enabled = true;
        CurrentAttackMotion = _attackMotion;
        IsAttacking = true;
    }

    public void StopAttack() {
        AttackBox.enabled = false;
        CurrentAttackMotion = null;
        IsAttacking = false;
    }

    private void OnTriggerStay(Collider other) {
        if (IsAttacking) {
            EnemyCombatManager _enemyCombatManager = other.gameObject.GetComponent<EnemyCombatManager>();
            if (_enemyCombatManager != null && CurrentAttackMotion != null) {
                // calculate regular attack
                Vector3 _AttackVector = this.transform.forward.normalized * CurrentAttackMotion.Force;
                Debug.Log("Attacked with " + _AttackVector);
                _enemyCombatManager.ReceiveAttack(_AttackVector, CurrentAttackMotion.KnockBackTime);
                StopAttack();
            }
        }
    }

    public void Smash(float _radius) {
        Debug.Log("ExplosionForceApplied");
        this.Body.AddExplosionForce(DefaultAttackForce, this.transform.position, _radius);
    }

}
