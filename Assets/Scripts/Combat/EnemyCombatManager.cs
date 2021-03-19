using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;
using Photon.Realtime;

public class EnemyCombatManager : MonoBehaviour {

    public Rigidbody Body;

    private void Awake() {
        this.Body = GetComponent<Rigidbody>();
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

    public void ReceiveSmash(float _smashTime, 
        float _ExplosionForce, 
        Vector3 _ExplosionPos, 
        float _SmashRadius) {

        PhotonView _view = PhotonView.Get(this);
        _view.RPC("OnSmashed", RpcTarget.All,
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
