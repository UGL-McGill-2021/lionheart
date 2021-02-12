using System.Collections;
using System.Collections.Generic;
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

    void Awake()
    {
        NavMeshAgent = this.GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        ConstructBehaviourTree();
    }

    private void Update()
    {
        RootNode.Evaluate();

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

}