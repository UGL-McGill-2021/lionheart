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
    private Rigidbody RigidBody;

    public delegate Transform GetTargetDelegate();
    private GetTargetDelegate GetTarget;
    private AnimationManager AnimManager;
    private float AnimDelay;

    public ShootNode(GameObject Projectile, GetTargetDelegate GetTarget, float Speed, float CooldownTime, GameObject Shooter, AnimationManager AnimationManager, float animDelay)
    {
        this.Projectile = Projectile;
        this.CooldownTime = CooldownTime;
        this.Shooter = Shooter;
        this.GetTarget = GetTarget;
        this.Speed = Speed;
        this.AnimManager = AnimationManager;
        this.AnimDelay = animDelay;

        Transform = Shooter.GetComponent<Transform>();
        ShooterScript = Shooter.GetComponent<Enemy>();
        RigidBody = Shooter.GetComponent<Rigidbody>();
    }

    public override NodeState Evaluate()
    {
        // Use rigidbody to rotate (Ziqi)
        //Quaternion rotation = Quaternion.LookRotation(GetTarget().position - Shooter.transform.position);
        //rotation = Quaternion.Slerp(Shooter.transform.rotation, rotation, 25 * Time.deltaTime);
        //RigidBody.MoveRotation(rotation);
        Shooter.transform.LookAt(new Vector3(GetTarget().position.x, Transform.position.y, GetTarget().position.z));

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

    private void PerformAnimation()
    {
        if (AnimManager != null)
        {
            Debug.Log("PERFORM");
            AnimManager.TriggerAttack();
        }
    }

    /// <summary>
    /// Author: Ziqi Li
    /// Function for shooting bullet
    /// </summary>
    private void Shoot()
    {
        GameObject bullet = PhotonNetwork.Instantiate("Bullet", Shooter.transform.position + Shooter.transform.forward * 2.4f, Quaternion.identity);
        bullet.GetComponent<Bullet>().owner = Shooter; // Modification made by Feiyang: Register the owner of the bullet to enable friendly fire
        bullet.GetComponent<Rigidbody>().AddForce(Shooter.transform.forward * (Speed * 100));
    }

    IEnumerator AttackAndCooldown()
    {
        PerformAnimation();
        yield return new WaitForSeconds(AnimDelay);

        // call the RPC function to fire bullet if this enemy belong to the current client
        if (PhotonNetwork.IsMasterClient)
        {
            //ShooterScript.PhotonView.RPC("RPC_Shoot", RpcTarget.AllViaServer);
            Shoot();
        }
        yield return new WaitForSeconds(CooldownTime);
        AttackRunning = false;
    }
}
