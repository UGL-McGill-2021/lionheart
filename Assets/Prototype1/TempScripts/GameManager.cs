using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Author: Ziqi Li
/// Game manager for demo scene
/// </summary>
public class GameManager : MonoBehaviour {

    public List<GameObject> PathPoints = new List<GameObject>();
    public List<GameObject> EnemySpawningPoints = new List<GameObject>();

    // for targeting by enemies
    public List<GameObject> PlayerList = new List<GameObject>();
    public List<GameObject> TurretTargets = new List<GameObject>();

    public bool Loading = false;
    public SceneLoader SceneLoader;

    private PhotonView PhotonView;

    // Start is called before the first frame update
    void Start() {

        PhotonView = GetComponent<PhotonView>();
        GameObject Player, Player2;

        if (PhotonNetwork.IsMasterClient) // 2
        {
            // generate player1
            Player = PhotonNetwork.Instantiate("Playerv2", new Vector3(0, 4f, 0), Quaternion.identity);
            int ViewId = Player.gameObject.GetComponent<PhotonView>().ViewID;
            PhotonView.RPC("RPC_addPlayer", RpcTarget.All, ViewId);  // use RPC call to add player

            // Generate moving platforms
            GameObject platform = PhotonNetwork.Instantiate("MPlatformRB",
                PathPoints[0].transform.position,
                PathPoints[0].transform.rotation, 0);

            //platform.GetComponent<MovingPlatform>().enabled = true;
            platform.GetComponent<MovingPlatformRB>().PathPointObjects.Add(PathPoints[0]);
            platform.GetComponent<MovingPlatformRB>().PathPointObjects.Add(PathPoints[1]);

            platform = PhotonNetwork.Instantiate("MPlatformRB",
                PathPoints[2].transform.position,
                PathPoints[2].transform.rotation, 0);

            //platform.GetComponent<MovingPlatform>().enabled = true;
            platform.GetComponent<MovingPlatformRB>().PathPointObjects.Add(PathPoints[2]);
            platform.GetComponent<MovingPlatformRB>().PathPointObjects.Add(PathPoints[3]);

            PhotonNetwork.Instantiate("Ball", new Vector3(7, 1.25f, 0), Quaternion.identity);

            //Generate enemies
            GameObject enemy;
            enemy = PhotonNetwork.Instantiate("Grunt",
                 EnemySpawningPoints[0].transform.position,
                Quaternion.identity);
            enemy.GetComponent<Grunt>().WanderTarget = EnemySpawningPoints[0].transform;

            enemy = PhotonNetwork.Instantiate("Shooter",
                 EnemySpawningPoints[1].transform.position,
                Quaternion.identity);
            enemy.GetComponent<Shooter>().WanderTarget = EnemySpawningPoints[1].transform;

            /*enemy = PhotonNetwork.Instantiate("Turret",
                 EnemySpawningPoints[2].transform.position,
                Quaternion.identity);*/

            // Generate objects
            GameObject obj;
            obj = PhotonNetwork.Instantiate("TempPlatform",
                 PathPoints[4].transform.position,
                Quaternion.identity);
            obj.GetComponent<TempPlatform>().isReusable = false;

            obj = PhotonNetwork.Instantiate("TempPlatform",
                 PathPoints[5].transform.position,
                Quaternion.identity);

            obj = PhotonNetwork.Instantiate("SpiritWall",
                 PathPoints[6].transform.position,
                Quaternion.identity);

            obj = PhotonNetwork.Instantiate("SpiritWall",
                 PathPoints[7].transform.position,
                Quaternion.identity);
            obj.GetComponent<SpiritWall>().SetIsOneWay(false);


            obj = PhotonNetwork.Instantiate("Checkpoint",
                 new Vector3(-11.5f,0f,-13.6f),
                Quaternion.identity);

            GameObject.Find("CheckpointManager").GetComponent<CheckpointManager>().FirstCheckPoint = obj.GetComponent<Checkpoint>();

            obj = PhotonNetwork.Instantiate("Checkpoint",
                 new Vector3(13f, 0f, -15f),
                Quaternion.identity);

            GameObject.Find("CheckpointManager").GetComponent<CheckpointManager>().FinalCheckPoint = obj.GetComponent<Checkpoint>();



        } 
        else 
        {
            // generate player2
            Player2 = PhotonNetwork.Instantiate("Playerv2", new Vector3(4, 1.25f, 0), Quaternion.identity);
            int ViewId = Player2.gameObject.GetComponent<PhotonView>().ViewID;
            PhotonView.RPC("RPC_addPlayer", RpcTarget.All, ViewId);  // use RPC call to add player
        }
    }

    private void Update()
    {
        // for testing level loader
        if (Loading && PhotonNetwork.IsMasterClient)
        {
            SceneLoader.LoadPhotonSceneWithName("SampleScene");
            Loading = false;
        }
    }

    /// <summary>
    /// Author: Ziqi Li
    /// RPC function for adding player to player list using the player PhotonViewID
    /// since we cannot pass gameobject directly using Photon
    /// </summary>
    /// <param name="playerViewID">The PhotonViewID of the gameobject</param>
    [PunRPC]
    void RPC_addPlayer(int playerViewID)
    {
        GameObject player = PhotonView.Find(playerViewID).gameObject;
        PlayerList.Add(player);
        TurretTargets.Add(player);
    }

}
