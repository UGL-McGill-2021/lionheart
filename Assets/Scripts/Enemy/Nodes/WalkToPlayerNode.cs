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
    private Transform Target;       //target to walk towards
    private NavMeshAgent Agent;     //navmesh agent to move
    private float Range;            //how close to get to target

    public WalkToPlayerNode(Transform Target, float Range, NavMeshAgent Agent) {
        this.Target = Target;
        this.Agent = Agent;
        this.Range = Range;
    }

    public override NodeState Evaluate()
    {
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
