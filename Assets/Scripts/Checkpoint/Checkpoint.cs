using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public List<Transform> SpawnPoints;

    private CheckpointManager CheckpointMan;
    private GameObject LastPlayerEntered;


    void Awake()
    {
        CheckpointMan = GameObject.Find("CheckpointManager").GetComponent<CheckpointManager>();
    }

    void Update()
    {
        
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
                    GetComponent<BoxCollider>().enabled = false;
                    GetComponent<MeshRenderer>().enabled = false;
                }
            }
        }

        
    }
}
