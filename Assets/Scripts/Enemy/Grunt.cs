using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// An enemy that wanders when target is not in range.
/// When target is in range, it moves towards it to perform a melee attack
/// </summary>

public class Grunt : Enemy
{
    //TODO: find a better way to access player transform
    public Transform PlayerTransform;

    public float ChasingRange;      // How far the player must be for the grunt to take notice
    public float NearnessToPlayer;  // How close the grunt will get to the player when approaching

    public Transform WanderTarget;  // the area the grunt will wander around
    public float WanderRange;       // how far to wander around the wander target
    public float AttackCooldown;    // how long to wait in between attacks
    
    private Node RootNode;
    private NavMeshAgent NavMeshAgent;

    // Photon:
    public bool isOffLine = false;
    private List<GameObject> PlayerList;

    void Awake() {
        NavMeshAgent = this.GetComponent<NavMeshAgent>();
        PhotonView = this.GetComponent<PhotonView>();
    }

    void Start() {
        // get the player list from game manager
        PlayerList = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().PlayerList;
        PlayerTransform = PlayerList[0].transform;

        ConstructBehaviourTree();
    }

    private void Update()
    {
        if (!isOffLine)
        {
            if(PhotonView.IsMine) RootNode.Evaluate();
        }
        else
        {
            RootNode.Evaluate();
        }

        //DEBUGGING: show where the grunt will go next
        //Debug.DrawLine(NavMeshAgent.destination, new Vector3(NavMeshAgent.destination.x, NavMeshAgent.destination.y + 1f, NavMeshAgent.destination.z), Color.red);
    }

    public override void Attacked()
    {
        //TODO: determin how to implement attacks.
        print("Grunt has been attacked!");
    }

    /// <summary>
    /// Author: Daniel Holker
    /// Constructs nodes and puts them together into a behaviour tree that determines its actions
    /// </summary>
    private void ConstructBehaviourTree() {
        WalkToPlayerNode WalkToPlayerNode = new WalkToPlayerNode(PlayerTransform, NearnessToPlayer ,NavMeshAgent);
        WanderNode WanderNode = new WanderNode(WanderTarget, NavMeshAgent, WanderRange);
        TargetInRangeNode TargetInRangeNode = new TargetInRangeNode(ChasingRange, PlayerTransform, this.transform);
        MeleeAttackNode MeleeAttackNode = new MeleeAttackNode(AttackCooldown, GetComponent<MonoBehaviour>());

        Inverter TargetNotInRange = new Inverter(TargetInRangeNode);


        Sequence AttackSequence = new Sequence(new List<Node> { WalkToPlayerNode, MeleeAttackNode});
        Sequence ChaseSequence = new Sequence(new List<Node>{TargetInRangeNode, AttackSequence});
        Sequence ReturnToWander = new Sequence(new List<Node>{TargetNotInRange, WanderNode});
        
        RootNode = new Selector(new List<Node>{ChaseSequence, ReturnToWander});
    }


}
