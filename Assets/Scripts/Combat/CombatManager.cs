using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;
using Photon.Realtime;

/// <summary>
/// Author: Feiyang Li
/// Base class for CombatManager. Different types of game characters have different combat managers
/// All types of CombatManager must inherit this base class
/// </summary>
public abstract class CombatManager : MonoBehaviour {
    public Rigidbody Body;

    // potential target is the CombatManager whom this is colliding with.
    private CombatManager PotentialTarget;

    public Dictionary<string, AttackMotion> PossibleAttacks = new Dictionary<string, AttackMotion>();

    [HideInInspector]
    public bool IsAttackMove = false;
    [HideInInspector]
    public Vector3 FinalLocation = Vector3.zero;

    public void OnTriggerEnter(Collider other) {
        Debug.Log(this.gameObject + ":CombatManager: Trigger entered with " + other.gameObject);
        CombatManager _targetCM = other.gameObject.GetComponent<CombatManager>();
        if (_targetCM != null) {
            Debug.Log(this.gameObject + ":CombatManager: Found valid CombatManager");
            PotentialTarget = _targetCM;
        }
    }

    public void OnTriggerExit(Collider other) {
        if (PotentialTarget != null) {
            Debug.Log(this.gameObject + ":CombatManager target reset to null");
            PotentialTarget = null;
        }
    }

    /// <summary>
    /// Author: Feiyang Li
    /// Attack target that the instigator ("this") is colliding with
    /// </summary>
    /// <param name="AttackName"></param>
    public void Attack(string AttackName) {
        Attack(AttackName, PotentialTarget);
    }

    /// <summary>
    /// Author: Feiyang Li
    /// Attack a specific target with specified attack motion
    /// </summary>
    /// <param name="AttackName">Name of the attack motion to use</param>
    /// <param name="other">Rigidbody of the attack target</param>
    public void Attack(string AttackName, CombatManager other) {
        if (other == null) {
            Debug.LogWarning("CombatManager: Combat Target Invalid");
            return;
        }

        if (PotentialTarget.CompareTag(this.tag)) {
            Debug.Log(this.gameObject + "CombatManager: Target is friendly");
            return;
        }

        // ensure AttackName is valid
        if (!PossibleAttacks.ContainsKey(AttackName)) {
            Debug.LogWarning("CombatManager: AttackName invalid");
            return;
        }

        AttackMotion motion = PossibleAttacks[AttackName];
        if (motion == null) {
            Debug.LogWarning("CombatManager: Chosen attack motion is NULL");
            return;
        }

        if (motion.mKnockBackTime > 0)
            other.Knockback(motion.mKnockBackTime);

        motion.OnApplyAttack(this, other);

    }

    /// <summary>
    /// Author: Feiyang Li
    /// Apply movement self.
    /// 
    /// </summary>
    /// <param name="movement"></param>
    public void InvokeAttackedMovement(Vector3 movement) {
        PhotonView view = PhotonView.Get(this);
        view.RPC("ApplyAttackedMovement", RpcTarget.MasterClient, movement.x, movement.y, movement.z);
    }

    [PunRPC]
    public void ApplyAttackedMovement(float x, float y, float z) {
        this.Body.MovePosition(new Vector3(x, y, z));
    }

    public abstract IEnumerator Knockback(int KnockBackTimeSpan);
}
