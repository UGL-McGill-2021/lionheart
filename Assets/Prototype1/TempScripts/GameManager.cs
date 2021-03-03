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

    public List<GameObject> PlayerList = new List<GameObject>();

    // Start is called before the first frame update
    void Start() {

        GameObject player;

        if (PhotonNetwork.IsMasterClient) // 2
        {
            player = PhotonNetwork.Instantiate("Playerv2", new Vector3(0, 4f, 0), Quaternion.identity);

            // Generate moving platforms
            GameObject platform = PhotonNetwork.Instantiate("MPlatformRB",
                PathPoints[0].transform.position,
                PathPoints[0].transform.rotation, 0);
            platform.layer = LayerMask.NameToLayer("Ground");
            //platform.GetComponent<MovingPlatform>().enabled = true;
            platform.GetComponent<MovingPlatformRB>().PathPointObjects.Add(PathPoints[0]);
            platform.GetComponent<MovingPlatformRB>().PathPointObjects.Add(PathPoints[1]);

            platform = PhotonNetwork.Instantiate("MPlatformRB",
                PathPoints[2].transform.position,
                PathPoints[2].transform.rotation, 0);
            platform.layer = LayerMask.NameToLayer("Ground");
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

            enemy = PhotonNetwork.Instantiate("Turret",
                 EnemySpawningPoints[2].transform.position,
                Quaternion.identity);


        } else {
            player = PhotonNetwork.Instantiate("Playerv2", new Vector3(4, 1.25f, 0), Quaternion.identity);
        }

        // Add players to the player list for both clients
        PlayerList.Add(player);

    }

}
