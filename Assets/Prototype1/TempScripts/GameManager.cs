using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Author: Ziqi Li
/// Game manager for demo scene
/// </summary>
public class GameManager : MonoBehaviour {

    public List<GameObject> PathPoints = new List<GameObject>();
    public GameObject platformPrefab;

    // Start is called before the first frame update
    void Start() {

        if (PhotonNetwork.IsMasterClient) // 2
        {
            GameObject player = PhotonNetwork.Instantiate("PrototypeCharacter", new Vector3(0, 1.25f, 0), Quaternion.identity);
            AudioListener listener = player.GetComponentInChildren<AudioListener>();
            if (listener != null) {
                listener.enabled = true;
            }

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


        } else {
            GameObject player = PhotonNetwork.Instantiate("PrototypeCharacter", new Vector3(4, 1.25f, 0), Quaternion.identity);
            AudioListener listener = player.GetComponentInChildren<AudioListener>();
            if (listener != null) {
                listener.enabled = true;
            }


        }

    }

}
