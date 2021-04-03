using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Author: Daniel
/// Keeps track of the last checkpoint reached by each player
/// </summary>

public class CheckpointManagerOLD : MonoBehaviour
{
    public CheckpointOLD FirstCheckPoint;      //Put initial starting point here
    public CheckpointOLD FinalCheckPoint;      //put final checkpoint here
    public float RespawnHeight;             //height which will cause players to respawn

    public List<GameObject> PlayerList;
    public SceneLoader SceneLoader;  // used to load next scene

    public Dictionary<GameObject, CheckpointOLD> CheckpointDict = new Dictionary<GameObject, CheckpointOLD>();

    void Awake()
    {
        PlayerList = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().PlayerList;
    }

    void Update()
    {
        //move player to its corresponding checkpoint spawn if it falls bellow RespawnHeight
        if (PhotonNetwork.IsMasterClient) {
            foreach (GameObject O in PlayerList)
            {
                if (O.transform.position.y <= RespawnHeight)
                {
                    if (!CheckpointDict.ContainsKey(O)) { CheckpointDict.Add(O, FirstCheckPoint); }

                    if (O.GetComponent<PhotonView>().IsMine) {
                        O.transform.position = CheckpointDict[O].GetSpawnPoint().position;
                    } else
                    {
                        O.GetComponent<PhotonView>().RPC("Teleport", RpcTarget.All, CheckpointDict[O].GetSpawnPoint().position);
                    }
                }
            }
        }
    }

    public void SetCheckpoint(GameObject Player, CheckpointOLD NewCheckpoint)
    {
        CheckpointDict[Player] = NewCheckpoint;
    }

    public void CheckpointCompleted(CheckpointOLD Checkpoint)
    {
        if (Checkpoint == FinalCheckPoint)
        {
            FinalCheckpointComplete();
        }
    }

    private void FinalCheckpointComplete()
    {
        print("Final Chekpoint collected by both players!");

        // load the next level if we are not in the last level
        if(PhotonNetwork.IsMasterClient)
        {
            switch (SceneManager.GetActiveScene().name)
            {
                case LevelName.Level0:
                    SceneLoader.LoadPhotonSceneWithName(LevelName.Level1);
                    break;
                case LevelName.Level1:
                    SceneLoader.LoadPhotonSceneWithName(LevelName.Level2);
                    break;
                case LevelName.Level2:
                    SceneLoader.LoadPhotonSceneWithName(LevelName.Level3);
                    break;
                case LevelName.Level3:
                    SceneLoader.LoadPhotonSceneWithName(LevelName.Level4);
                    break;
                default:
                    print("All levels are completed!");
                    break;
            }
        }
    }
}
