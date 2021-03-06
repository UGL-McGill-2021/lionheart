using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Author: Daniel Holker
/// move randomly on a navmesh around a given target and radius
/// </summary>

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
        //set initial point to nearest position on navmesh to given target
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

    //find a random point on the navmesh within the given radius 
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
