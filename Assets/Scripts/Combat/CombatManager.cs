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
    public Rigidbody body;

    // potential target is the CombatManager whom this is colliding with.
    private CombatManager mPotentialTarget;

    public Dictionary<string, AttackMotion> mPossibleAttacks = new Dictionary<string, AttackMotion>();

    [HideInInspector]
    public bool isAttackMove = false;
    [HideInInspector]
    public Vector3 finalLocation = Vector3.zero;

    public void OnTriggerEnter(Collider other) {
        Debug.Log(this.gameObject + ":CombatManager: Trigger entered with " + other.gameObject);
        CombatManager targetCM = other.gameObject.GetComponent<CombatManager>();
        if (targetCM != null) {
            Debug.Log(this.gameObject + ":CombatManager: Found valid CombatManager");
            mPotentialTarget = targetCM;
        }
    }

    public void OnTriggerExit(Collider other) {
        if (mPotentialTarget != null) {
            Debug.Log(this.gameObject + ":CombatManager target reset to null");
            mPotentialTarget = null;
        }
    }

    /// <summary>
    /// Author: Feiyang Li
    /// Attack target that the instigator ("this") is colliding with
    /// </summary>
    /// <param name="AttackName"></param>
    public void Attack(string AttackName) {
        Attack(AttackName, mPotentialTarget);
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

        if (mPotentialTarget.CompareTag(this.tag)) {
            Debug.Log(this.gameObject + "CombatManager: Target is friendly");
            return;
        }

        // ensure AttackName is valid
        if (!mPossibleAttacks.ContainsKey(AttackName)) {
            Debug.LogWarning("CombatManager: AttackName invalid");
            return;
        }

        AttackMotion motion = mPossibleAttacks[AttackName];
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
    public void StartAttackMovement(Vector3 movement) {
        PhotonView view = PhotonView.Get(this);
        view.RPC("ApplyAttackMovement", RpcTarget.MasterClient, movement.x, movement.y, movement.z);
    }

    [PunRPC]
    public void ApplyAttackMovement(float x, float y, float z) {
        this.finalLocation = this.transform.position + new Vector3(x, y, z);
        this.isAttackMove = true;
    }

    private void Update() {
        if (isAttackMove && finalLocation != Vector3.zero) {
            this.gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, finalLocation, Time.deltaTime);
            if (this.gameObject.transform.position == finalLocation) {
                isAttackMove = false;
                finalLocation = Vector3.zero;
            }
        }
    }

    public abstract IEnumerator Knockback(int KnockBackTimeSpan);
}
