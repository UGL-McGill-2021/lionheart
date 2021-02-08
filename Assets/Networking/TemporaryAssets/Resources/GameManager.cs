using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {
        GameObject player = PhotonNetwork.Instantiate("PrototypeCharacter", new Vector3(0, 0, 0), Quaternion.identity);
        AudioListener listener = player.GetComponentInChildren<AudioListener>();
        if (listener != null) {
            listener.enabled = true;
        }
    }

    // Update is called once per frame
    void Update() {

    }
}
