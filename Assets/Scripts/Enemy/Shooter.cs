using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// An enemy that wanders when target is not in range.
/// When target is in range, it freezes and shoots projectiles towards it.
/// </summary>
public class Shooter : Enemy
{


    public float ChasingRange;      // How far the player must be for the shooter to take notice
    public float NearnessToPlayer;  // How close the shooter will get to the player when approaching

    public Transform WanderTarget;  // the area the shooter will wander around
    public float WanderRange;       // how far to wander around the wander target

    public float ProjectileSpeed;   // how fast the projectile will travel
    public float ShootCooldown;     // how long to wait in between attacks

    public GameObject Projectile;


    public Transform CurrentTarget;
    private Node RootNode;
    private NavMeshAgent NavMeshAgent;

    // Photon:
    private List<GameObject> PlayerList;

    void Awake()
    {
        NavMeshAgent = this.GetComponent<NavMeshAgent>();
        PhotonView = this.GetComponent<PhotonView>();
    }

    void Start()
    {
        // get the player list from game manager
        PlayerList = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().PlayerList;
        CurrentTarget = PlayerList[0].transform;

        ConstructBehaviourTree();
    }

    private void Update()
    {
        if (PhotonView.IsMine) RootNode.Evaluate();


        //DEBUGGING: show where the shooter will go next
        //Debug.DrawLine(NavMeshAgent.destination, new Vector3(NavMeshAgent.destination.x, NavMeshAgent.destination.y + 1f, NavMeshAgent.destination.z), Color.red);
    }

    public override void Attacked()
    {
        print("Shooter has been attacked!");
    }

    public void SetTarget(Transform Transform)
    {
        CurrentTarget = Transform;
    }

    public Transform GetTarget()
    {
        return CurrentTarget;
    }

    /// <summary>
    /// Author: Ziqi Li
    /// RPC function for shooting bullet
    /// </summary>
    [PunRPC]
    public void RPC_Shoot()
    {
        GameObject bullet = Instantiate(Projectile, transform.position + transform.forward * 1.5f, Quaternion.identity);
        Debug.Log(bullet.transform);
        bullet.GetComponent<Rigidbody>().AddForce(transform.forward * (ProjectileSpeed * 100));
    }

    /// <summary>
    /// Author: Daniel Holker
    /// Constructs nodes and puts them together into a behaviour tree that determines its actions
    /// </summary>
    private void ConstructBehaviourTree()
    {
        WalkToPlayerNode WalkToPlayerNode = new WalkToPlayerNode(GetTarget, NearnessToPlayer, NavMeshAgent);
        WanderNode WanderNode = new WanderNode(WanderTarget, NavMeshAgent, WanderRange);
        TargetInRangeNode TargetInRangeNode = new TargetInRangeNode(ChasingRange, PlayerList, this.transform, SetTarget);
        ShootNode ShootNode = new ShootNode(Projectile, CurrentTarget, ProjectileSpeed, ShootCooldown, this.gameObject);

        Inverter TargetNotInRange = new Inverter(TargetInRangeNode);


        Sequence AttackSequence = new Sequence(new List<Node> { WalkToPlayerNode, ShootNode });
        Sequence ChaseSequence = new Sequence(new List<Node> { TargetInRangeNode, AttackSequence });
        Sequence ReturnToWander = new Sequence(new List<Node> { TargetNotInRange, WanderNode });

        RootNode = new Selector(new List<Node> { ChaseSequence, ReturnToWander });
    }


}