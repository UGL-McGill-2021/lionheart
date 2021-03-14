using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public Checkpoint FirstCheckPoint;      //Put initial starting point here
    public List<GameObject> PlayerList;

    Dictionary<GameObject, Checkpoint> CheckpointDict = new Dictionary<GameObject, Checkpoint>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerList.Count < 2)
        {
            PlayerList = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().PlayerList;

            foreach (GameObject O in PlayerList)
            {
                CheckpointDict.Add(O, FirstCheckPoint);
            }
        }
    }

    public void SetCheckpoint(GameObject Player, Checkpoint NewCheckpoint)
    {
        CheckpointDict[Player] = NewCheckpoint;
    }
}
