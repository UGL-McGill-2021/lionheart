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
    public PhotonView PhotonView;
    public bool isOffLine = true;
    private Vector3 RemotePosition;
    private Quaternion RemoteRotation;

    void Awake()
    {
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
    }

    public override void Attacked()
    {
        print("Turret has been attacked!");
    }

    private void ConstructBehaviourTree()
    {
        TargetInRangeNode TargetInRangeNode = new TargetInRangeNode(Range, PlayerTransform, this.transform);
        ShootNode ShootNode = new ShootNode(Projectile, PlayerTransform, ProjectileSpeed, ShootCooldown, this.gameObject);

        Sequence ShootSequence = new Sequence(new List<Node> { TargetInRangeNode, ShootNode});

        RootNode = new Selector(new List<Node> { ShootSequence});
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