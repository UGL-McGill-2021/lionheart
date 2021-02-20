using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

/// <summary>
/// Author : Daniel Holker
/// Instantiates a GameObject and adds force towards a given Transform
/// </summary>

public class ShootNode : Node
{
    private GameObject Projectile;
    private float Speed;                    //how fast the projectile will travel
    private float CooldownTime;             //how often to shoot
    private MonoBehaviour MonoBehaviour;    //used for running coroutines
    private GameObject Shooter;
    private bool AttackRunning = false;
    private bool AttackComplete = false;
    private Transform Transform;
    private Transform Target;

    private PhotonView PhotonView;

    public ShootNode(GameObject Projectile, Transform Target, float Speed, float CooldownTime, GameObject Shooter)
    {
        this.Projectile = Projectile;
        this.CooldownTime = CooldownTime;
        this.Shooter = Shooter;
        this.Target = Target;
        this.Speed = Speed;

        Transform = Shooter.GetComponent<Transform>();
        MonoBehaviour = Shooter.GetComponent<MonoBehaviour>();
        //PhotonView = Shooter.GetComponent<Shooter>().PhotonView;
    }

    public override NodeState Evaluate()
    {
        Transform.LookAt(Target.position);

        if (!AttackRunning)
        {
            AttackRunning = true;
            MonoBehaviour.StartCoroutine(AttackAndCooldown());
        }

        if (AttackComplete)
        {
            AttackComplete = false;
            AttackRunning = false;
            return NodeState.SUCCESS;
        }

        return NodeState.RUNNING;
    }


    //[PunRPC]
    private void Shoot()
    {
        GameObject bullet = MonoBehaviour.Instantiate(Projectile, Transform.position + Transform.forward * 1.5f, Quaternion.identity) as GameObject;
        bullet.GetComponent<Rigidbody>().AddForce(Transform.forward * (Speed*100));
    }

    IEnumerator AttackAndCooldown()
    {
        // call the RPC function to fire bullet if this player belong to the current client
        //if (PhotonView.IsMine) PhotonView.RPC("Shoot", RpcTarget.AllViaServer);
        Shoot();
        yield return new WaitForSeconds(CooldownTime);
        AttackRunning = false;
    }
}
