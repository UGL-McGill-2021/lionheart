using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;


/// <summary>
/// Author: Ziqi Li
/// Game manager for demo scene
/// </summary>
public class GameManager : MonoBehaviour {

    // for spawning players
    public List<GameObject> PlayerSpawningPoints = new List<GameObject>();  // index 0 for master client, index1 for client

    // for spawning platforms
    public List<GameObject> Respawnable_TempPlatformPoints = new List<GameObject>();
    public List<GameObject> NonRespawnable_TempPlatformPoints = new List<GameObject>();
    public List<GameObject> OneTime_SpiritWallPoints = new List<GameObject>();
    public List<GameObject> Normal_SpiritWallPoints = new List<GameObject>();
    [System.Serializable]
    public struct PathPointsList
    {
        public List<GameObject> PathPoints;  // the path points of a single moving platform
    }
    public List<PathPointsList> MovingPlatformPathList = new List<PathPointsList>(); // list of point list for the moving platforms

    // for spawning enemies
    public List<GameObject> GruntSpawningPoints = new List<GameObject>();
    public List<GameObject> ShooterSpawningPoints = new List<GameObject>();
    public List<GameObject> TurretSpawningPoints = new List<GameObject>();  // make sure the size is the same as TurretTargetsPoints
    public List<GameObject> TurretTargetsPoints = new List<GameObject>();  // target points for the turrets

    // for targeting by enemies
    //[HideInInspector]
    public List<GameObject> PlayerList = new List<GameObject>();

    // List of platfroms (used for change speed, stop time... etc)
    private List<MovingPlatformRB> MovingPlatformScriptList = new List<MovingPlatformRB>();
    private List<TempPlatform> TempPlatformScriptList = new List<TempPlatform>();

    private PhotonView PhotonView;

    // Start is called before the first frame update
    void Start() {

        PhotonView = GetComponent<PhotonView>();

        // generate players
        Instantiate_Players();

        // the master client will instantiate all game objects that need photon network to update to other clients
        if (PhotonNetwork.IsMasterClient) 
        {
            Instantiate_Enemies();
            Instantiate_MovingPlatforms();  // will be generate with default speed, stop time...
            Instantiate_SpiritWalls();
            Instantiate_TempPlatforms();  // will be generate with default respawning time

            // Since all networking objects has to be generated using photon, if we want to change the properties of a specific platform
            // we cannot change their script fields through their prefab
            // If we want to change some specific properties of platform (ex: speed, stop time... etc)
            // we have to get the platform from the lists and change manually its property fields in their script accordingly
            // This kind of changes will be level dependent
            switch (SceneManager.GetActiveScene().name)
            {
                case LevelName.Level0:
                    break;
                case LevelName.Level1:
                    // for example, change the speed of the first moving platform in the list to 5 and stop time to 1
                    //MovingPlatformScriptList[0].speed = 5f;
                    //MovingPlatformScriptList[0].StopTime = 1f;
                    // MovingPlatformScriptList[0].isAutomatic = false;

                    // for example, change the disappear time of the second temp platform in the list to 1.5
                    //TempPlatformScriptList[1].DisappearDelay = 1.5f;

                    break;
                case LevelName.Level2:

                    break;
                case LevelName.Level3:

                    break;
                case LevelName.Level4:

                    break;
                default:
                    break;
            }
        } 
    }

    private void Update()
    {

    }

    /// <summary>
    /// Author: Ziqi
    /// function to generate players using the points lists
    /// </summary>
    void Instantiate_Players()
    {
        GameObject player;
        if (PhotonNetwork.IsMasterClient)
        {
            // generate master player
            player = PhotonNetwork.Instantiate("Playerv4", PlayerSpawningPoints[0].transform.position, PlayerSpawningPoints[0].transform.rotation);
            int ViewId = player.gameObject.GetComponent<PhotonView>().ViewID;
            PhotonView.RPC("RPC_addPlayer", RpcTarget.AllViaServer, ViewId);  // use RPC call to add player to the player list
        }
        else
        {
            // generate client player
            player = PhotonNetwork.Instantiate("Playerv4-1", PlayerSpawningPoints[1].transform.position, PlayerSpawningPoints[1].transform.rotation);
            int ViewId = player.gameObject.GetComponent<PhotonView>().ViewID;
            PhotonView.RPC("RPC_addPlayer", RpcTarget.AllViaServer, ViewId);  // use RPC call to add player to the player list
        }
    }

    /// <summary>
    /// Author: Ziqi
    /// function to generate enemies using the points lists
    /// </summary>
    void Instantiate_Enemies()
    {
        // generate grunts
        foreach (GameObject spawningPoint in GruntSpawningPoints)
        {
            if (spawningPoint != null)
            {
                GameObject obj = PhotonNetwork.Instantiate("Grunt_v2", spawningPoint.transform.position, spawningPoint.transform.rotation);
                obj.GetComponent<Grunt>().enabled = true;
                obj.GetComponent<NavMeshAgent>().enabled = true;
                obj.GetComponent<Grunt>().WanderTarget = spawningPoint.transform;
            }
        }

        // generate shooters
        foreach (GameObject spawningPoint in ShooterSpawningPoints)
        {
            if(spawningPoint != null)
            {
                GameObject obj = PhotonNetwork.Instantiate("Shooter_v2", spawningPoint.transform.position, spawningPoint.transform.rotation);
                obj.GetComponent<Shooter>().enabled = true;
                obj.GetComponent<NavMeshAgent>().enabled = true;
                obj.GetComponent<Shooter>().WanderTarget = spawningPoint.transform;
            }
        }

        // generate turrets (make sure the turretList elements are corresponding to their targets in targetList)
        if(TurretSpawningPoints.Count == TurretTargetsPoints.Count)
        {
            for (int i = 0; i < TurretSpawningPoints.Count; i++)
            {
                if (TurretSpawningPoints[i] != null && TurretTargetsPoints[i] != null)
                {
                    GameObject obj = PhotonNetwork.Instantiate("Turret_v2", TurretSpawningPoints[i].transform.position, TurretSpawningPoints[i].transform.rotation);
                    obj.GetComponent<Turret>().Target = TurretTargetsPoints[i].transform;
                    obj.transform.rotation = Quaternion.LookRotation(TurretTargetsPoints[i].transform.position);
                }
            }
        }
    }

    /// <summary>
    /// Author: Ziqi
    /// function to generate moving platforms using the path points lists
    /// Note: each pathPointsList elememt containing all the path points for a single moving platform
    /// </summary>
    void Instantiate_MovingPlatforms()
    {
        GameObject platform = null;
        // for each list in the MovingPlatformPathList, generate a moving platform accordingly
        if (MovingPlatformPathList.Count != 0)
            foreach (PathPointsList PathPointsList in MovingPlatformPathList)
            {
                for (int i = 0; i < PathPointsList.PathPoints.Count; i++)
                {
                    if(PathPointsList.PathPoints[i] != null)
                    {
                        // take the first point in the path as the spawning point
                        if (i == 0)
                        {
                            platform = PhotonNetwork.Instantiate("MPlatformRB",
                                PathPointsList.PathPoints[i].transform.position,
                                PathPointsList.PathPoints[i].transform.rotation);
                            MovingPlatformScriptList.Add(platform.GetComponent<MovingPlatformRB>());
                        }
                        // add path point to the path of this moving platform
                        platform.GetComponent<MovingPlatformRB>().PathPointObjects.Add(PathPointsList.PathPoints[i]);
                    }
                }
            }
    }

    /// <summary>
    /// Author: Ziqi
    /// function to generate spirit walls (2 types) using the points lists
    /// </summary>
    void Instantiate_SpiritWalls()
    {
        // generate one-time spirit walls
        foreach (GameObject spawningPoint in OneTime_SpiritWallPoints)
        {
            if (spawningPoint != null)
            {
                GameObject obj = PhotonNetwork.Instantiate("SpiritWall", spawningPoint.transform.position, spawningPoint.transform.rotation);
                obj.GetComponent<SpiritWall>().SetIsOneWay(true);
            }
        }
        // generate normal spirit walls
        
        foreach (GameObject spawningPoint in Normal_SpiritWallPoints)
        {
            if (spawningPoint != null)
            {
                GameObject obj = PhotonNetwork.Instantiate("SpiritWall", spawningPoint.transform.position, spawningPoint.transform.rotation);
                obj.GetComponent<SpiritWall>().SetIsOneWay(false);
            }
        }
    }

    /// <summary>
    /// Author: Ziqi
    /// function to generate temp platforms (2 types) using the points lists
    /// </summary>
    void Instantiate_TempPlatforms()
    {
        // generate respawnable temp platform
        foreach (GameObject spawningPoint in Respawnable_TempPlatformPoints)
        {
            if(spawningPoint != null)
            {
                GameObject obj = PhotonNetwork.Instantiate("TempPlatform", spawningPoint.transform.position, spawningPoint.transform.rotation);
                obj.GetComponentInChildren<TempPlatform>().isReusable = true;

                TempPlatformScriptList.Add(obj.GetComponent<TempPlatform>());
            }
        }

        // generate non-respawnable temp platform
        foreach (GameObject spawningPoint in NonRespawnable_TempPlatformPoints)
        {
            if (spawningPoint != null)
            {
                GameObject obj = PhotonNetwork.Instantiate("TempPlatform", spawningPoint.transform.position, spawningPoint.transform.rotation);
                obj.GetComponent<TempPlatform>().isReusable = false;

                TempPlatformScriptList.Add(obj.GetComponent<TempPlatform>());
            }
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
    }

}
