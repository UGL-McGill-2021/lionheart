using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using Photon.Pun;
using Photon.Realtime;

/// <summary>
/// Author: Feiyang Li
/// CombatManager for player
/// </summary>
public class PlayerCombatManager : MonoBehaviour {

    ControllerInput Input;
    Rigidbody Body;

    public Collider AttackBox;

    private AttackMotion CurrentAttackMotion;

    private void Awake() {
        this.Body = GetComponent<Rigidbody>();
    }

    private void Start() {
        Input = new ControllerInput();
        Input.Player.Kick.performed += _ => Attack(new Kick(1000, 3));
        Input.Enable();
    }

    public void Attack(AttackMotion _attackMotion) {
        AttackBox.enabled = true;
        CurrentAttackMotion = _attackMotion;
    }

    public void StopAttack() {
        AttackBox.enabled = false;
        CurrentAttackMotion = null;
    }

    private void OnTriggerEnter(Collider other) {
        EnemyCombatManager _enemyCombatManager = other.gameObject.GetComponent<EnemyCombatManager>();
        if (_enemyCombatManager != null && CurrentAttackMotion != null) {
            // calculate attack
            Vector3 _AttackVector = (other.transform.position - this.transform.position).normalized * CurrentAttackMotion.Force;
            _enemyCombatManager.ReceiveAttack(_AttackVector, CurrentAttackMotion.KnockBackTime);
            StopAttack();
        }
    }

}
