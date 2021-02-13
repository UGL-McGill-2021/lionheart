using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

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
