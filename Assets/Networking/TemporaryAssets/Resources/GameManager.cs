using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {
        PhotonNetwork.Instantiate("PrototypeCharacter", new Vector3(0, 0, 0), Quaternion.identity);
    }

    // Update is called once per frame
    void Update() {

    }
}
