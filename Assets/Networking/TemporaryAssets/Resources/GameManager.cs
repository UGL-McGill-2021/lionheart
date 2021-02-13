using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    public List<GameObject> PathPoints = new List<GameObject>();


    // Start is called before the first frame update
    void Start() {

        if (PhotonNetwork.IsMasterClient) // 2
        {
            GameObject player = PhotonNetwork.Instantiate("PrototypeCharacter", new Vector3(0, 0, 0), Quaternion.identity);
            AudioListener listener = player.GetComponentInChildren<AudioListener>();
            if (listener != null)
            {
                listener.enabled = true;
            }

            // Generate moving platforms
            GameObject platform = PhotonNetwork.InstantiateRoomObject("MPlatform",
                PathPoints[0].transform.position,
                PathPoints[0].transform.rotation, 0);
            //platform.GetComponent<MovingPlatform>().enabled = true;
            platform.GetComponent<MovingPlatform>().PathPointObjects.Add(PathPoints[0]);
            platform.GetComponent<MovingPlatform>().PathPointObjects.Add(PathPoints[1]);

            platform = PhotonNetwork.InstantiateRoomObject("MPlatform",
                PathPoints[2].transform.position,
                PathPoints[2].transform.rotation, 0);
            //platform.GetComponent<MovingPlatform>().enabled = true;
            platform.GetComponent<MovingPlatform>().PathPointObjects.Add(PathPoints[2]);
            platform.GetComponent<MovingPlatform>().PathPointObjects.Add(PathPoints[3]);

            platform = PhotonNetwork.InstantiateRoomObject("MPlatform",
                PathPoints[4].transform.position,
                PathPoints[4].transform.rotation, 0);
            //platform.GetComponent<MovingPlatform>().enabled = true;
            platform.GetComponent<MovingPlatform>().PathPointObjects.Add(PathPoints[4]);
            platform.GetComponent<MovingPlatform>().PathPointObjects.Add(PathPoints[5]);
        }
        else
        {
            GameObject player = PhotonNetwork.Instantiate("PrototypeCharacter", new Vector3(4, 0, 0), Quaternion.identity);
            AudioListener listener = player.GetComponentInChildren<AudioListener>();
            if (listener != null)
            {
                listener.enabled = true;
            }
        }

    }

}
