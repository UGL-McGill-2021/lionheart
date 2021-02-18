using UnityEngine;

public class CameraManager : MonoBehaviour {
    public GameObject Player;

    /// <summary>
    /// Author: Feiyang Li
    /// Check player validity
    /// </summary>
    private void Start() {
        if (Player == null) {
            this.enabled = false;
            Debug.LogError("CameraManager: Player is null");
        }
    }

    /// <summary>
    /// Author: Feiyang Li
    /// Update position of the camera
    /// </summary>
    private void Update() {
        transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y, Player.transform.position.z);
        
    }
}
