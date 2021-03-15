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
    private Enemy ShooterScript;    //used for running coroutines
    private GameObject Shooter;
    private bool AttackRunning = false;
    private bool AttackComplete = false;
    private Transform Transform;
    private Transform Target;
    private Rigidbody RigidBody;

    public ShootNode(GameObject Projectile, Transform Target, float Speed, float CooldownTime, GameObject Shooter)
    {
        this.Projectile = Projectile;
        this.CooldownTime = CooldownTime;
        this.Shooter = Shooter;
        this.Target = Target;
        this.Speed = Speed;

        Transform = Shooter.GetComponent<Transform>();
        ShooterScript = Shooter.GetComponent<Enemy>();
        RigidBody = Shooter.GetComponent<Rigidbody>();
    }

    public override NodeState Evaluate()
    {
        // Use rigidbody to rotate (Ziqi)
        Quaternion rotation = Quaternion.LookRotation(Target.position - Shooter.transform.position);
        rotation = Quaternion.Slerp(Shooter.transform.rotation, rotation, 25 * Time.deltaTime);
        RigidBody.MoveRotation(rotation);

        if (!AttackRunning)
        {
            AttackRunning = true;
            ShooterScript.StartCoroutine(AttackAndCooldown());
        }

        if (AttackComplete)
        {
            AttackComplete = false;
            AttackRunning = false;
            return NodeState.SUCCESS;
        }

        return NodeState.RUNNING;
    }


    
    //private void Shoot()
    //{
    //    GameObject bullet = ((MonoBehaviour) MonoBehaviour).Instantiate(Projectile, Transform.position + Transform.forward * 1.5f, Quaternion.identity) as GameObject;
    //    bullet.GetComponent<Rigidbody>().AddForce(Transform.forward * (Speed*100));
    //}

    IEnumerator AttackAndCooldown()
    {
        // call the RPC function to fire bullet if this enemy belong to the current client
        if (ShooterScript.PhotonView.IsMine) ShooterScript.PhotonView.RPC("RPC_Shoot", RpcTarget.AllViaServer);
        yield return new WaitForSeconds(CooldownTime);
        AttackRunning = false;
    }
}
