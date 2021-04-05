using UnityEngine;
using Lionheart.Player.Movement;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;

/// <summary>
/// Author: Feiyang Li, Denis
/// CombatManager for player
/// </summary>
public class PlayerCombatManager : MonoBehaviour {

    ControllerInput Input;
    Rigidbody Body;

    MovementHandler Handler;
    Knockback PlayerKnockback;
    PhotonTransformViewClassic PhotonTransformView;

    public Collider AttackBox;

    private AttackMotion CurrentAttackMotion;

    private bool IsAttacking;

    private bool IsInvicible;

    public float DefaultAttackForce = 1000;

    [Header("Dash Attack")]
    public float DashAttackAngleOfViewDegree;

    [Header("Smash")]
    public float SmashRadius = 500;
    public float DefaultSmashTime = 1;
    public float DefaultSmashForce = 500;

    [Header("Stomp")]
    public GameObject GroundCheck;
    public float StompDistance = 10;
    public float StompForce = 5;

    private void Awake() {
        this.Body = GetComponent<Rigidbody>();
        this.Handler = GetComponent<MovementHandler>();
        this.PhotonTransformView = GetComponent<PhotonTransformViewClassic>();
    }

    private void Start() {
        Input = new ControllerInput();
        //Input.Player.Kick.performed += _ => Attack(new Kick(DefaultAttackForce, 1));
        Input.Enable();
        PlayerKnockback = gameObject.GetComponent<Knockback>();
    }

    /// <summary>
    /// Author: Feiyang
    /// Initiate an Attack
    /// </summary>
    /// <param name="_attackMotion"></param>
    public void Attack(AttackMotion _attackMotion) {
        AttackBox.enabled = true;
        CurrentAttackMotion = _attackMotion;
        IsAttacking = true;
    }

    /// <summary>
    /// Author: Feiyang
    /// Stop an attack
    /// </summary>
    public void StopAttack() {
        AttackBox.enabled = false;
        CurrentAttackMotion = null;
        IsAttacking = false;
    }

    /// <summary>
    /// Author: Feiyang
    /// Attack Logic
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay(Collider other) {
        // when invicible, attack every enemy that is colliding with the player
        if (IsAttacking || IsInvicible) {
            EnemyCombatManager _enemyCombatManager = other.gameObject.GetComponent<EnemyCombatManager>();
            if (_enemyCombatManager != null && CurrentAttackMotion != null) {
                // calculate regular attack
                Vector3 _AttackVector = this.transform.forward.normalized * CurrentAttackMotion.Force;
                Debug.Log("Trigger stay Attacked with " + _AttackVector);
                _enemyCombatManager.ReceiveAttack(_AttackVector, CurrentAttackMotion.KnockBackTime);
                StopAttack();
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        // check if player is directly above the enemy
        
        Vector3 playerToEnemy = this.transform.position - other.transform.position;
        float angle = Vector3.Angle(this.transform.forward, playerToEnemy);
        
        if (other.transform.position.y < this.transform.position.y && 
            (angle <= 120f && angle >= 60f) && 
            Vector3.Distance(GroundCheck.transform.position, other.transform.position) < StompDistance){
            Debug.Log("Stomp angle" + angle);
            EnemyCombatManager _enemyCombatManager = other.gameObject.GetComponent<EnemyCombatManager>();
            if (_enemyCombatManager != null) {
                
                _enemyCombatManager.ReceiveStomp(this.transform.forward.normalized * -1 * StompForce, 0.5f);
                Debug.Log("Stomped " + other.gameObject + " with " + this.transform.forward.normalized * -1 * StompForce);
                
            }
        }
        
    }


    /// <summary>
    /// Author: Feiyang
    /// Initiate Smash
    /// </summary>
    public void Smash() {

        Collider[] colliders = Physics.OverlapSphere(transform.position, SmashRadius);

        foreach (Collider _nearby in colliders) {
            GameObject _nearbyObjects = _nearby.gameObject;
            EnemyCombatManager _enemyCombatManager = _nearbyObjects.GetComponent<EnemyCombatManager>();
            if (_enemyCombatManager != null) {
                _enemyCombatManager.ReceiveSmash(DefaultSmashTime,
                DefaultSmashForce,
                this.transform.position,
                SmashRadius);
            }
        }
    }

    /// <summary>
    /// Author: Feiyang
    /// Invoke logic that handles the attack initiated by enemies
    /// </summary>
    /// <param name="_AttackVelocity"></param>
    /// <param name="_AttackTimeSpan"></param>
    public void ReceivePlayerAttack(Vector3 _AttackVelocity, float _AttackTimeSpan) {
        PhotonView _view = PhotonView.Get(this);
        Debug.Log("Invoking OnAttacked on MasterClient");
        _view.RPC("OnPlayerAttacked", RpcTarget.All, _AttackVelocity.x, _AttackVelocity.y, _AttackVelocity.z, _AttackTimeSpan);
    }

    /// <summary>
    /// Author: Feiyang, Denis
    /// Applies a bullet hit to the player
    /// </summary>
    /// <param name="_x"></param>
    /// <param name="_y"></param>
    /// <param name="_z"></param>
    /// <param name="_time"></param>
    /// <returns></returns>
    [PunRPC]
    public void OnPlayerAttacked(float _x, float _y, float _z, float _time) {
        if (this.IsInvicible) {
            Debug.Log(gameObject + " is invincible");
            return;
        }

        PlayerKnockback.AddKnockback(new Vector3(_x, _y, _z));
    }

    /// <summary>
    /// Author: Feiyang
    /// 
    /// </summary>
    /// <param name="_smashTime"></param>
    /// <param name="_ExplosionForce"></param>
    /// <param name="_ExplosionPos"></param>
    /// <param name="_SmashRadius"></param>
    public void ReceivePlayerSmash(float _smashTime,
        float _ExplosionForce,
        Vector3 _ExplosionPos,
        float _SmashRadius) {

        PhotonView _view = PhotonView.Get(this);
        _view.RPC("OnPlayerSmashed", RpcTarget.All,
            _smashTime,
            _ExplosionForce,
            _ExplosionPos.x,
            _ExplosionPos.y,
            _ExplosionPos.z,
            _SmashRadius);
    }

    /// <summary>
    /// Author: Feiyang, Denis
    /// Applies a smah to the player
    /// </summary>
    /// <param name="_time"></param>
    /// <param name="_explosionForce"></param>
    /// <param name="_ExplosionX"></param>
    /// <param name="_ExplosionY"></param>
    /// <param name="_ExplosionZ"></param>
    /// <param name="_smashRadius"></param>
    [PunRPC]
    public void OnPlayerSmashed(float _time,
        float _explosionForce,
        float _ExplosionX,
        float _ExplosionY,
        float _ExplosionZ,
        float _smashRadius) {

        if (this.IsInvicible) {
            Debug.Log(gameObject + " is invincible");
            return;
        }
        
        PlayerKnockback.AddExplosiveKnockback(_explosionForce, _smashRadius, new Vector3(_ExplosionX, _ExplosionY, _ExplosionZ));
    }

    /// <summary>
    /// Author: Feiyang
    /// Update Invicibility on all clients 
    /// ** NOTE ** THIS IS THE ONLY WAY TO CHANGE INVICIBILITY !!! 
    /// </summary>
    /// <param name="_Invincible"></param>
    public void SetInvincibleOnAll(bool _Invincible) {
        PhotonView view = PhotonView.Get(this);
        view.RPC("SetInvincible", RpcTarget.All, _Invincible);
    }

    [PunRPC]
    public void SetInvincible(bool _Invincible) {
        IsInvicible = _Invincible;
    }

}
