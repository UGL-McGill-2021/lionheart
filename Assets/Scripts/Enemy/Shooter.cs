using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;

public class Shooter : Enemy
{
    //TODO: find a better way to access player transform
    public Transform PlayerTransform;

    public float ChasingRange;
    public float NearnessToPlayer;

    public Transform WanderTarget;
    public float WanderRange;

    public float ProjectileSpeed;
    public float ShootCooldown;

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

        //Debug.DrawLine(NavMeshAgent.destination, new Vector3(NavMeshAgent.destination.x, NavMeshAgent.destination.y + 1f, NavMeshAgent.destination.z), Color.red);
    }

    public override void Attacked()
    {
        print("Shooter has been attacked!");
    }

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