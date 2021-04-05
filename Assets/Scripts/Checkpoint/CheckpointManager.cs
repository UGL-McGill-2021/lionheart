using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Author: Daniel
/// Keeps track of the last checkpoint reached by each player
/// </summary>

public class CheckpointManager : MonoBehaviour
{
    public Checkpoint FirstCheckPoint;      //Put initial starting point here
    public Checkpoint FinalCheckPoint;      //put final checkpoint here
    public Checkpoint CurrentCheckPoint;    //Currently activated checkpoint
    public float RespawnHeight;             //height which will cause players to respawn

    public List<GameObject> PlayerList;     //keeps track of players for respawning
    public SceneLoader SceneLoader;  // used to load next scene

    void Awake()
    {
        PlayerList = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().PlayerList;
        if (CurrentCheckPoint == null) { CurrentCheckPoint = FirstCheckPoint; }
    }

    void Update()
    {
        //move player to its corresponding checkpoint spawn if it falls bellow RespawnHeight
        if (PhotonNetwork.IsMasterClient) {
            foreach (GameObject O in PlayerList)
            {
                if (O.transform.position.y <= RespawnHeight)
                {
                    if (O.GetComponent<PhotonView>().IsMine) {
                        O.transform.position = CurrentCheckPoint.GetSpawnPoint(true).position;
                    } else
                    {
                        O.GetComponent<PhotonView>().RPC("Teleport", RpcTarget.All, CurrentCheckPoint.GetSpawnPoint(false).position);
                    }
                    O.GetComponent<Animator>().Play("Idle");
                }
            }
        }
    }

    public void SetCheckpoint(Checkpoint NewCheckpoint)
    {
        CurrentCheckPoint = NewCheckpoint;
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
        print("Final Chekpoint collected by both players!");

        // load the next level if we are not in the last level
        switch (SceneManager.GetActiveScene().name)
        {
            case LevelName.Level0:
                if (PhotonNetwork.IsMasterClient) SceneLoader.LoadPhotonSceneWithName(LevelName.Level1);
                break;
            case LevelName.Level1:
                if (PhotonNetwork.IsMasterClient) SceneLoader.LoadPhotonSceneWithName(LevelName.Level2);
                break;
            case LevelName.Level2:
                if (PhotonNetwork.IsMasterClient) SceneLoader.LoadPhotonSceneWithName(LevelName.Level3);
                break;
            case LevelName.Level3:
                print("All levels are completed!");
                SceneLoader.LoadSceneWithName("MainMenu");
                break;
            default:
                break;
        }
    }
}
