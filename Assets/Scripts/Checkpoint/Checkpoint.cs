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
    private GameObject LastPlayerEntered;


    void Awake()
    {
        CheckpointMan = GameObject.FindGameObjectWithTag("CheckpointManager").GetComponent<CheckpointManager>();
    }

    private void OnTriggerEnter(Collider Other)
    {
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
                    CheckpointMan.CheckpointCompleted(this);
                    GetComponent<BoxCollider>().enabled = false;
                    GetComponent<MeshRenderer>().enabled = false;
                }
            }
        }
        
    }

    //used to help prevent players from being spawned in the same position 
    public Transform GetSpawnPoint(bool IsMaster)
    {
        return IsMaster ? SpawnPoints[0] : SpawnPoints[1];
    }
}
