using Photon.Voice.PUN;
using Photon.Voice.Unity;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Author: Ziqi Li
/// Script for voice chat manager
/// Note: attach the Player Input component and assign an InputAction to it
/// for using the callback functions from Input System
/// </summary>
public class VoiceChatManager : MonoBehaviour {

    public Recorder recorder;
    private PhotonVoiceNetwork punVoiceNetwork;

    private void Awake() {
        punVoiceNetwork = PhotonVoiceNetwork.Instance;
    }

    /// <summary>
    /// Author: Ziqi Li
    /// CallBack function from input system
    /// Activate talking
    /// </summary>
    public void OnTalk() {
        Debug.Log("Talk");
        this.recorder.TransmitEnabled = true;
    }

    /// <summary>
    /// Author: Ziqi Li
    /// CallBack function from input system
    /// Mute talking
    /// </summary>
    public void OnMute() {
        Debug.Log("Mute");
        this.recorder.TransmitEnabled = false;
    }
}
