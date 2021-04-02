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
        Shooter.transform.LookAt(new Vector3(GetTarget().position.x, 0f, GetTarget().position.z));

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
        if (AnimManager != null) AnimManager.TriggerAttack();
    }

    //private void Shoot()
    //{
    //    GameObject bullet = ((MonoBehaviour) MonoBehaviour).Instantiate(Projectile, Transform.position + Transform.forward * 1.5f, Quaternion.identity) as GameObject;
    //    bullet.GetComponent<Rigidbody>().AddForce(Transform.forward * (Speed*100));
    //}

    IEnumerator AttackAndCooldown()
    {
        PerformAnimation();
        yield return new WaitForSeconds(AnimDelay);

        // call the RPC function to fire bullet if this enemy belong to the current client
        if (ShooterScript.PhotonView.IsMine)
        {
            ShooterScript.PhotonView.RPC("RPC_Shoot", RpcTarget.AllViaServer);
        }
        yield return new WaitForSeconds(CooldownTime);
        AttackRunning = false;
    }
}
