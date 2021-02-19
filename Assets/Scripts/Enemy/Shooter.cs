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
    //TODO: find a better way to access player transform
    public Transform PlayerTransform;

    public float ChasingRange;      // How far the player must be for the shooter to take notice
    public float NearnessToPlayer;  // How close the shooter will get to the player when approaching

    public Transform WanderTarget;  // the area the shooter will wander around
    public float WanderRange;       // how far to wander around the wander target

    public float ProjectileSpeed;   // how fast the projectile will travel
    public float ShootCooldown;     // how long to wait in between attacks

    public GameObject Projectile;

    private Node RootNode;
    private NavMeshAgent NavMeshAgent;

    // Photon:
    public PhotonView PhotonView;
    public bool isOffLine = true;
    private Vector3 RemotePosition;
    private Quaternion RemoteRotation;

    void Awake()
    {
        NavMeshAgent = this.GetComponent<NavMeshAgent>();
        PhotonView = this.GetComponent<PhotonView>();
    }

    void Start()
    {
        ConstructBehaviourTree();
    }

    private void Update()
    {
        if(!isOffLine)
        {
            if (PhotonView.IsMine)
            {
                RootNode.Evaluate();
            }
            else
            {
                //Update remote player
                transform.position = Vector3.Lerp(transform.position, RemotePosition, Time.deltaTime);
                transform.rotation = Quaternion.Lerp(transform.rotation, RemoteRotation, Time.deltaTime);
            }
        }
        else
        {
            RootNode.Evaluate();
        }

        //DEBUGGING: show where the shooter will go next
        //Debug.DrawLine(NavMeshAgent.destination, new Vector3(NavMeshAgent.destination.x, NavMeshAgent.destination.y + 1f, NavMeshAgent.destination.z), Color.red);
    }

    public override void Attacked()
    {
        print("Shooter has been attacked!");
    }

    /// <summary>
    /// Author: Daniel Holker
    /// Constructs nodes and puts them together into a behaviour tree that determines its actions
    /// </summary>
    private void ConstructBehaviourTree()
    {
        WalkToPlayerNode WalkToPlayerNode = new WalkToPlayerNode(PlayerTransform, NearnessToPlayer, NavMeshAgent);
        WanderNode WanderNode = new WanderNode(WanderTarget, NavMeshAgent, WanderRange);
        TargetInRangeNode TargetInRangeNode = new TargetInRangeNode(ChasingRange, PlayerTransform, this.transform);
        ShootNode ShootNode = new ShootNode(Projectile, PlayerTransform, ProjectileSpeed, ShootCooldown, this.gameObject);

        Inverter TargetNotInRange = new Inverter(TargetInRangeNode);


        Sequence AttackSequence = new Sequence(new List<Node> { WalkToPlayerNode, ShootNode });
        Sequence ChaseSequence = new Sequence(new List<Node> { TargetInRangeNode, AttackSequence });
        Sequence ReturnToWander = new Sequence(new List<Node> { TargetNotInRange, WanderNode });

        RootNode = new Selector(new List<Node> { ChaseSequence, ReturnToWander });
    }

    /// <summary>
    /// Author: Ziqi Li
    /// Called by PUN several times per second, so that your script can write and
    /// read synchronization data for the PhotonView
    /// This method will be called in scripts that are assigned as Observed component of a PhotonView
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="info"></param>
    public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // Sending messages to server if this object belong to the current client, otherwise receive messages
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            RemotePosition = (Vector3)stream.ReceiveNext();
            RemoteRotation = (Quaternion)stream.ReceiveNext();
        }
    }

}