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
    private bool LoadLock = false;     //used to prevent calling level loader more than once

    void Awake()
    {
        CheckpointMan = GameObject.FindGameObjectWithTag("CheckpointManager").GetComponent<CheckpointManager>();
        PlayersInArea = new HashSet<GameObject>();
    }

    private void OnTriggerEnter(Collider Other)
    {
        PlayersInArea.Add(Other.gameObject);
        if (PlayersInArea.Count == 2)
        {
            if (IsFinalCheckpoint)
            {
                if (!LoadLock)
                {
                    CheckpointMan.FinalCheckpointComplete();
                    LoadLock = true;
                }
            } else
            {
                CheckpointMan.SetCheckpoint(this);
                CheckpointCompleted();
            }
        }

    }

    private void OnTriggerExit(Collider Other)
    {
        if (Other.tag == "Player")
        {
            PlayersInArea.Remove(Other.gameObject);
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
