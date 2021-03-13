using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


/// <summary>
/// Author: Daniel Holker
/// move towards given target; nav mesh used for pathfinding
/// </summary>

public class WalkToPlayerNode : Node
{
    private Transform Target;
    private NavMeshAgent Agent;     //navmesh agent to move
    private float Range;            //how close to get to target

    public delegate Transform GetTargetDelegate();
    private GetTargetDelegate GetTarget;

    public WalkToPlayerNode(GetTargetDelegate GetTarget, float Range, NavMeshAgent Agent) {
        this.GetTarget = GetTarget;
        this.Agent = Agent;
        this.Range = Range;
    }

    public override NodeState Evaluate()
    {
        Target = GetTarget();

        float distance = Vector3.Distance(Target.position, Agent.transform.position);
        if(distance > Range)
        {
            Agent.isStopped = false;
            Agent.isStopped = false;
            Agent.SetDestination(Target.position);
            return NodeState.RUNNING;
        }
        else
        {
            Agent.isStopped = true;
            return NodeState.SUCCESS;
        }
    }

}
