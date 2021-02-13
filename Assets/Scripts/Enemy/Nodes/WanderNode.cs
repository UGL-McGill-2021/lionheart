using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// move randomly around a given target

public class WanderNode : Node
{
    private Transform Target;
    private Transform CurrentTarget;
    private NavMeshAgent NavMeshAgent;
    private float WanderRadius;

    public WanderNode (Transform Target, NavMeshAgent NavMeshAgent, float WanderRadius) {
        this.Target = Target;
        this.NavMeshAgent = NavMeshAgent;
        this.WanderRadius = WanderRadius;
    }

    void Awake()
    {
        Target.position = RandomNavSphere(Target.position, 0, -1);
    }

    public override NodeState Evaluate()
    {
        if (CurrentTarget == null)
        {
            CurrentTarget = new GameObject().transform;
            CurrentTarget.position = Target.position;
        }

        float distance = Vector3.Distance(CurrentTarget.position, NavMeshAgent.transform.position);
        if(distance > 1f)
        {
            NavMeshAgent.isStopped = false;
            NavMeshAgent.SetDestination(CurrentTarget.position);
            return NodeState.RUNNING;
        }
        else
        {
            NavMeshAgent.isStopped = true;
            CurrentTarget.position = RandomNavSphere(Target.position, WanderRadius, -1);
            return NodeState.SUCCESS;
        }
    }

    private Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask) {
        Vector3 randDirection = UnityEngine.Random.insideUnitSphere * dist;
        randDirection += origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition (randDirection, out navHit, dist, layermask);
        if (navHit.position.x == Mathf.Infinity)
        {
            return Target.position;
        } else
        {
            return navHit.position;
        }
        
    }


}
