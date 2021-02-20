using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

/// <summary>
/// Author: Daniel Holker
/// Nodes are constructed into a behaviour tree and determine how it is traversed.
/// It returns a RUNNING, SUCCESS or FAILURE NodeState.
/// </summary>
public abstract class Node
{
    protected NodeState CurrentNodeState;

    public NodeState GetNodeState {
        get {
            return CurrentNodeState;
        }
    }

    public abstract NodeState Evaluate();

    public enum NodeState
    {
        RUNNING, SUCCESS, FAILURE,
    }

}
