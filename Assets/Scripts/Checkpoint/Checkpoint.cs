using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Author: Daniel
/// </summary>

public class Checkpoint : MonoBehaviour
{
    public List<Transform> SpawnPoints;

    public CheckpointManager CheckpointMan;
    public bool IsFinalCheckpoint;

    private GameObject LastPlayerEntered;
    private HashSet<GameObject> PlayersInArea;

    void Awake()
    {
        CheckpointMan = GameObject.FindGameObjectWithTag("CheckpointManager").GetComponent<CheckpointManager>();
        PlayersInArea = new HashSet<GameObject>();
    }

    private void OnTriggerEnter(Collider Other)
    {
        if (IsFinalCheckpoint)
        {
            if (Other.tag == "Player")
            {
                PlayersInArea.Add(Other.gameObject);
                if (PlayersInArea.Count == 2)
                {
                    CheckpointMan.FinalCheckpointComplete();
                }
            }
        } else {
            if (Other.tag == "Player")
            {
                if (!LastPlayerEntered)
                {
                    LastPlayerEntered = Other.gameObject;
                }
                else
                {
                    if (LastPlayerEntered != Other.gameObject)
                    {
                        CheckpointMan.SetCheckpoint(this);
                        CheckpointCompleted();
                    }
                }
            }
        }
    }

    private void OnTriggerExit(Collider Other)
    {
        if (IsFinalCheckpoint) {
            if (Other.tag == "Player")
            {
                PlayersInArea.Remove(Other.gameObject);
            }
        }
    }


    private void CheckpointCompleted()
    {
        GetComponent<BoxCollider>().enabled = false;
        GetComponent<MeshRenderer>().enabled = false;
    }


    //used to help prevent players from being spawned in the same position 
    public Transform GetSpawnPoint(bool IsMaster)
    {
        return IsMaster ? SpawnPoints[0] : SpawnPoints[1];
    }
}
