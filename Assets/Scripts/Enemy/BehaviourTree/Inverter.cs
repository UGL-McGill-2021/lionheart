using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Returns the opposite state of its assigned node
/// </summary>

public class Inverter : Node
{
    protected Node Node;

    public Inverter(Node Node)
    {
        this.Node = Node;
    }

    public override NodeState Evaluate()
    {
        switch (Node.Evaluate()) 
        {
            case Node.NodeState.RUNNING:
                CurrentNodeState = NodeState.RUNNING;
                break;
            case Node.NodeState.SUCCESS:
                CurrentNodeState = NodeState.FAILURE;
                break;
            case Node.NodeState.FAILURE:
                CurrentNodeState = NodeState.SUCCESS;
                break;
            default:
                break;
        }
        
        return CurrentNodeState;
    }
}
