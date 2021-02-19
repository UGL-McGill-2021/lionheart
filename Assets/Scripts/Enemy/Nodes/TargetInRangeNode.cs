using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Author: Daniel Holker
/// Check if target is within a given range
/// </summary>
public class TargetInRangeNode : Node
{
    private float Range;
    private Transform Target;
    private Transform Origin;

    public TargetInRangeNode(float Range, Transform Target, Transform Origin)
    {
        this.Range = Range;
        this.Target = Target;
        this.Origin = Origin;
    }

    public override NodeState Evaluate()
    {
        float distance = Vector3.Distance(Target.position, Origin.position);
        return distance <= Range ? NodeState.SUCCESS : NodeState.FAILURE;
    }
}
