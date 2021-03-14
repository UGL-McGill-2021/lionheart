using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Author: Daniel
/// </summary>

public class Checkpoint : MonoBehaviour
{
    public List<Transform> SpawnPoints;

    private CheckpointManager CheckpointMan;
    private GameObject LastPlayerEntered;
    private bool SpawnSwitch = false;   //used to help prevent players from being spawned in the same position 


    void Awake()
    {
        CheckpointMan = GameObject.Find("CheckpointManager").GetComponent<CheckpointManager>();
    }

    private void OnTriggerEnter(Collider Other)
    {
        if (Other.tag == "Player")
        {
            if (!LastPlayerEntered)
            {
                CheckpointMan.SetCheckpoint(Other.gameObject, this);
                LastPlayerEntered = Other.gameObject;
            }
            else
            {
                if (LastPlayerEntered != Other.gameObject)
                {
                    CheckpointMan.SetCheckpoint(Other.gameObject, this);
                    CheckpointMan.CheckpointCompleted(this);
                    GetComponent<BoxCollider>().enabled = false;
                    GetComponent<MeshRenderer>().enabled = false;
                }
            }
        }

        
    }

    public Transform GetSpawnPoint()
    {
        SpawnSwitch = !SpawnSwitch;
        return SpawnSwitch ? SpawnPoints[0] : SpawnPoints[1];
    }
}
