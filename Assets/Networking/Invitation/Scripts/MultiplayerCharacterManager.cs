using UnityEngine;
using Photon.Pun;

public class MultiplayerCharacterManager : MonoBehaviour, IPunInstantiateMagicCallback{
    public Camera cam;
    public PrototypeCharacterMovementControls controls;
    void IPunInstantiateMagicCallback.OnPhotonInstantiate(PhotonMessageInfo info) {
        if (this.gameObject.GetPhotonView().IsMine) {
            cam.enabled = true;
            controls.enabled = true;
        }
    }
}
