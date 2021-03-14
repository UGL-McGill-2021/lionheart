using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

/// <summary>
/// Author: Daniel
/// Keeps track of the last checkpoint reached by each player
/// </summary>

public class CheckpointManager : MonoBehaviour
{
    public Checkpoint FirstCheckPoint;      //Put initial starting point here
    public Checkpoint FinalCheckPoint;      //put final checkpoint here
    public float RespawnHeight;             //height which will cause players to respawn

    public List<GameObject> PlayerList;
    

    Dictionary<GameObject, Checkpoint> CheckpointDict = new Dictionary<GameObject, Checkpoint>();

    void Awake()
    {
        PlayerList = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().PlayerList;

        foreach (GameObject O in PlayerList)
        {
            CheckpointDict.Add(O, FirstCheckPoint);
        }
    }

    void Update()
    {

        //move player to its corresponding checkpoint spawn if it falls bellow RespawnHeight
        foreach (GameObject O in PlayerList)
        {
            if (O.transform.position.y <= RespawnHeight)
            {
                if (O.GetComponent<PhotonView>().IsMine) {
                    O.transform.position = CheckpointDict[O].GetSpawnPoint().position;
                } else
                {
                    O.GetComponent<PhotonView>().RPC("Teleport", RpcTarget.All, CheckpointDict[O].GetSpawnPoint());
                }
            }
        }
    }

    public void SetCheckpoint(GameObject Player, Checkpoint NewCheckpoint)
    {
        CheckpointDict[Player] = NewCheckpoint;
    }

    public void CheckpointCompleted(Checkpoint Checkpoint)
    {
        if (Checkpoint == FinalCheckPoint)
        {
            FinalCheckpointComplete();
        }
    }

    private void FinalCheckpointComplete()
    {
        //TODO: have this call whatever's needed to load the next level
        print("Final Chekpoint collected by both players!");
    }
}
