using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;

public class Turret : Enemy
{
    //TODO: find a better way to access player transform
    public Transform PlayerTransform;

    public float Range;

    public float ProjectileSpeed;
    public float ShootCooldown;

    public GameObject Projectile;

    private Node RootNode;

    // Photon:
    private List<GameObject> PlayerList;

    void Awake()
    {
        PhotonView = this.GetComponent<PhotonView>();
    }

    void Start()
    {
        // get the player list from game manager
        PlayerList = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().PlayerList;
        PlayerTransform = PlayerList[0].transform;

        ConstructBehaviourTree();
    }

    private void Update()
    {
        if (PhotonView.IsMine) RootNode.Evaluate();
    }

    public override void Attacked()
    {
        print("Turret has been attacked!");
    }

    /// <summary>
    /// Author: Ziqi Li
    /// RPC function for shooting bullet
    /// </summary>
    [PunRPC]
    public void RPC_Shoot()
    {
        GameObject bullet = Instantiate(Projectile, transform.position + transform.forward * 2.5f, Quaternion.identity);
        Debug.Log(bullet.transform);
        bullet.GetComponent<Rigidbody>().AddForce(transform.forward * (ProjectileSpeed * 100));
    }

    private void ConstructBehaviourTree()
    {
        TargetInRangeNode TargetInRangeNode = new TargetInRangeNode(Range, PlayerTransform, this.transform);
        ShootNode ShootNode = new ShootNode(Projectile, PlayerTransform, ProjectileSpeed, ShootCooldown, this.gameObject);

        Sequence ShootSequence = new Sequence(new List<Node> { TargetInRangeNode, ShootNode});

        RootNode = new Selector(new List<Node> { ShootSequence});
    }

}