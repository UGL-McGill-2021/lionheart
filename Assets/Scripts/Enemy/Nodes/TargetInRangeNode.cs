using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Author: Daniel Holker
/// Given a list of targets, returns if one of them is within range
/// If so, uses a delegate to set the new target
/// </summary>
public class TargetInRangeNode : Node
{
    private float Range;
    private List<GameObject> TargetList;
    private Transform Origin;

    public delegate void SetTargetDelegate(Transform Transform);
    private SetTargetDelegate SetTarget;

    public TargetInRangeNode(float Range, List<GameObject> TargetList, Transform Origin, SetTargetDelegate SetTargetDelegate)
    {
        this.Range = Range;
        this.TargetList = TargetList;
        this.Origin = Origin;
        this.SetTarget = SetTargetDelegate;
    }

    public override NodeState Evaluate()
    {
        foreach (GameObject Target in TargetList)
        {
            if (Vector3.Distance(Target.transform.position, Origin.position) <= Range)
            {
                SetTarget?.Invoke(Target.transform);
                return NodeState.SUCCESS;
            }
        }
        return  NodeState.FAILURE;
    }
}
