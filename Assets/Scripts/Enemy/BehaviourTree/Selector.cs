using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//a selector node waits one of its children to run/succeed

public class Selector : Node
{
    protected List<Node> Nodes = new List<Node>();

    public Selector(List<Node> Nodes)
    {
        this.Nodes = Nodes;
    }

    public override NodeState Evaluate()
    {
        foreach (Node N in Nodes) {
            switch (N.Evaluate()) 
            {
                case Node.NodeState.RUNNING:
                    CurrentNodeState = NodeState.RUNNING;
                    return CurrentNodeState;
                case Node.NodeState.SUCCESS:
                    CurrentNodeState = NodeState.SUCCESS;
                    return CurrentNodeState;
                case Node.NodeState.FAILURE:
                    break;
                default:
                    break;
            }
        }

        CurrentNodeState = NodeState.FAILURE;
        return CurrentNodeState;
    }
}
