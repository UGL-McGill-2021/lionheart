using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Author: Daniel Holker
/// Constantly shoots at a given target
/// </summary>


public class Turret : Enemy
{
    public Transform Target;        //target to shoot at

    public float ProjectileSpeed;   //how fast projectiles travel
    public float ShootCooldown;     //how long to wait before next shot
    public float AttackDelay;  // delay for performing animation

    public GameObject Projectile;   //gameobject to instantiate and shoot

    private Node RootNode;
    public TurretAnimationManager AnimManager { get; set; }

    // Photon:
    private List<GameObject> PlayerList;

    void Awake()
    {
        PhotonView = this.GetComponent<PhotonView>();
        AnimManager = this.GetComponent<TurretAnimationManager>();
    }

    void Start()
    {
        ConstructBehaviourTree();
    }

    private void Update()
    {
        if (PhotonView.IsMine) RootNode.Evaluate();
    }

    public Transform GetTarget()
    {
        return Target;
    }

    public override void Attacked()
    {
        print("Turret has been attacked!");
    }

    /// <summary>
    /// Author: Ziqi Li
    /// RPC function for shooting bullet
    /// </summary>
    //[PunRPC]
    //public void RPC_Shoot()
    //{
    //    GameObject bullet = Instantiate(Projectile, transform.position + transform.forward * 2.5f, Quaternion.identity);
    //    Debug.Log(bullet.transform);
    //    bullet.GetComponent<Bullet>().owner = this.gameObject; // Modification made by Feiyang: Register the owner of the bullet to enable friendly fire
    //    bullet.GetComponent<Rigidbody>().AddForce(transform.forward * (ProjectileSpeed * 100));
    //}


    private void ConstructBehaviourTree()
    {
        ShootNode ShootNode = new ShootNode(Projectile, GetTarget, ProjectileSpeed, ShootCooldown, this.gameObject, AnimManager, this.AttackDelay);
        RootNode = ShootNode;
    }

}