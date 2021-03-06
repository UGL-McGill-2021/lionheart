using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// In the order they're listed, waits for each node in Nodes to return SUCCESS before returning SUCCESS itself
/// </summary>

public class Sequence : Node
{
    protected List<Node> Nodes = new List<Node>();

    public Sequence(List<Node> Nodes)
    {
        this.Nodes = Nodes;
    }

    public override NodeState Evaluate()
    {
        foreach (Node N in Nodes)
        {
            switch (N.Evaluate())
            {
                case NodeState.RUNNING:
                    CurrentNodeState = NodeState.RUNNING;
                    return CurrentNodeState;
                case NodeState.FAILURE:
                    CurrentNodeState = NodeState.FAILURE;
                    return CurrentNodeState;
            }
        }
        CurrentNodeState = NodeState.SUCCESS;
        return CurrentNodeState;
    }
}
