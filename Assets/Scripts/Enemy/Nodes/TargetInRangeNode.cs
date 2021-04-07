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
        if (TargetList.Count < 2)
        {
            if (Vector3.Distance(TargetList[0].transform.position, Origin.position) <= Range)
            {
                SetTarget(TargetList[0].transform);
                return NodeState.SUCCESS;
            }
            return NodeState.FAILURE;
        }

        GameObject _Closest = Vector3.Distance(TargetList[0].transform.position, Origin.position) < Vector3.Distance(TargetList[1].transform.position, Origin.position) ? TargetList[0] : TargetList[1];

        if (Vector3.Distance(_Closest.transform.position, Origin.position) <= Range)
        {
            SetTarget(_Closest.transform);
            return NodeState.SUCCESS;
        }
        return NodeState.FAILURE;
    }
}