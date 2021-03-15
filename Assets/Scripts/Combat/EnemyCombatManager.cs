using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;
using Photon.Realtime;

public class EnemyCombatManager : MonoBehaviour {

    public Rigidbody Body;
    public NavMeshAgent NavAgent;

    private void Awake() {
        this.Body = GetComponent<Rigidbody>();
        this.NavAgent = GetComponent<NavMeshAgent>();
    }

    public void ReceiveAttack(Vector3 _AttackVelocity, int _AttackTimeSpan) {
        PhotonView _view = PhotonView.Get(this);
        Debug.Log("Invoking OnAttacked on MasterClient");
        _view.RPC("OnAttacked", RpcTarget.All, _AttackVelocity.x, _AttackVelocity.y, _AttackVelocity.z, _AttackTimeSpan);
    }

    [PunRPC]
    public IEnumerator OnAttacked(float _x, float _y, float _z, int _time) {
        Debug.Log("OnAttacked executed with " + _x + " " + _y + " " + _z + " with knockback " + _time);
        this.NavAgent.enabled = false;
        this.Body.isKinematic = false;
        this.Body.AddForce(new Vector3(_x, _y, _z));

        yield return new WaitForSeconds(_time);

        this.Body.isKinematic = true;
        this.NavAgent.enabled = true;
    }
}
