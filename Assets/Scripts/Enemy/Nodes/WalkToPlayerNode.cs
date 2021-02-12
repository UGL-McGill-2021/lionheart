using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// moves towards given target
// nav mesh used for pathfinding

public class WalkToPlayerNode : Node
{
    private Transform Target;
    private NavMeshAgent Agent;
    private float Range;

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
