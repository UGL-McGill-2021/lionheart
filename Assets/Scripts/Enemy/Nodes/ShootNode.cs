using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootNode : Node
{
    private GameObject Projectile;
    private float Speed;
    private float CooldownTime;
    private MonoBehaviour MonoBehaviour;
    private GameObject Shooter;
    private bool AttackRunning = false;
    private bool AttackComplete = false;
    private Transform Transform;
    private Transform Target;

    public ShootNode(GameObject Projectile, Transform Target, float Speed, float CooldownTime, GameObject Shooter)
    {
        this.Projectile = Projectile;
        this.CooldownTime = CooldownTime;
        this.Shooter = Shooter;
        this.Target = Target;
        this.Speed = Speed;

        Transform = Shooter.GetComponent<Transform>();
        MonoBehaviour = Shooter.GetComponent<MonoBehaviour>();
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

    private void Shoot()
    {
        GameObject bullet = MonoBehaviour.Instantiate(Projectile, Transform.position + Transform.forward * 1.5f, Quaternion.identity) as GameObject;
        bullet.GetComponent<Rigidbody>().AddForce(Transform.forward * (Speed*100));
    }

    IEnumerator AttackAndCooldown()
    {
        Shoot();
        yield return new WaitForSeconds(CooldownTime);
        AttackRunning = false;
    }
}
