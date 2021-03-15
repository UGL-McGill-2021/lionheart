using UnityEngine;
using Photon.Pun;

/// <summary>
/// Author: Feiyang Li
/// Class for enable camera and controller script attached to this player
/// </summary>
public class MultiplayerCharacterManager : MonoBehaviour, IPunInstantiateMagicCallback{
    public Camera cam;
    public AudioListener aud;
    public PrototypeCharacterMovementControls controls;
    void IPunInstantiateMagicCallback.OnPhotonInstantiate(PhotonMessageInfo info) {
        if (this.gameObject.GetPhotonView().IsMine) {
            cam.enabled = true;
            aud.enabled = true;
            controls.enabled = true;
        }   
    }
}
