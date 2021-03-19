using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;
using Photon.Realtime;

public class EnemyCombatManager : MonoBehaviour {

    public Rigidbody Body;

    private bool IsAttacking;
    private AttackMotion CurrentAttackMotion;
    public Collider AttackBox;

    [Header("Smash")]
    public float SmashRadius = 500;
    public float DefaultSmashTime = 1;
    public float DefaultSmashForce = 500;

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
            PlayerCombatManager _playerCombatManager = other.gameObject.GetComponent<PlayerCombatManager>();
            if (_playerCombatManager != null && CurrentAttackMotion != null) {
                // calculate regular attack
                Vector3 _AttackVector = this.transform.forward.normalized * CurrentAttackMotion.Force;
                Debug.Log("Attacked with " + _AttackVector);
                _playerCombatManager.ReceiveAttack(_AttackVector, CurrentAttackMotion.KnockBackTime);
                StopAttack();
            }
        }
    }

    public void ReceiveAttack(Vector3 _AttackVelocity, int _AttackTimeSpan) {
        PhotonView _view = PhotonView.Get(this);
        Debug.Log("Invoking OnAttacked on MasterClient");
        _view.RPC("OnAttacked", RpcTarget.MasterClient, _AttackVelocity.x, _AttackVelocity.y, _AttackVelocity.z, _AttackTimeSpan);
    }

    [PunRPC]
    public IEnumerator OnAttacked(float _x, float _y, float _z, int _time) {
        Debug.Log("OnAttacked executed with " + _x + " " + _y + " " + _z + " with knockback " + _time);
        this.Body.isKinematic = false;
        this.Body.AddForce(new Vector3(_x, _y, _z));

        yield return new WaitForSeconds(_time);

        this.Body.isKinematic = true;
    }

    public void Smash() {
        Debug.Log("ExplosionForceApplied");

        Collider[] colliders = Physics.OverlapSphere(transform.position, SmashRadius);

        foreach (Collider _nearby in colliders) {
            GameObject _nearbyObjects = _nearby.gameObject;
            PlayerCombatManager _playerCombatManager = _nearbyObjects.GetComponent<PlayerCombatManager>();
            if (_playerCombatManager != null) {
                _playerCombatManager.ReceiveSmash(DefaultSmashTime,
                DefaultSmashForce,
                this.transform.position,
                SmashRadius);
            }
        }
    }

    public void ReceiveSmash(float _smashTime, 
        float _ExplosionForce, 
        Vector3 _ExplosionPos, 
        float _SmashRadius) {

        PhotonView _view = PhotonView.Get(this);
        _view.RPC("OnSmashed", RpcTarget.MasterClient,
            _smashTime,
            _ExplosionForce,
            _ExplosionPos.x,
            _ExplosionPos.y,
            _ExplosionPos.z,
            _SmashRadius);
    }

    [PunRPC]
    public IEnumerator OnSmashed(float _time, 
        float _explosionForce, 
        float _ExplosionX, 
        float _ExplosionY, 
        float _ExplosionZ, 
        float _smashRadius) {

        this.Body.isKinematic = false;
        this.Body.AddExplosionForce(_explosionForce, new Vector3(_ExplosionX, _ExplosionY, _ExplosionZ), _smashRadius);

        yield return new WaitForSeconds(_time);

        this.Body.isKinematic = true;
    }
}
