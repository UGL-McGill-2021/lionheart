using Photon.Voice.PUN;
using Photon.Voice.Unity;
using UnityEngine;
using UnityEngine.InputSystem;

public class VoiceChatManager : MonoBehaviour {
    public Recorder recorder;
    private PhotonVoiceNetwork punVoiceNetwork;

    // Temporary key binding
    private InvitationCodeInputAction VoiceChatAction;

    private void Awake() {
        punVoiceNetwork = PhotonVoiceNetwork.Instance;
        VoiceChatAction = new InvitationCodeInputAction();
        VoiceChatAction.Player.Talk.performed += _ => Talk();
        VoiceChatAction.Player.Mute.performed += _ => Mute();
        VoiceChatAction.Enable();
    }

    public void Talk() {
        Debug.Log("Talk");
        this.recorder.TransmitEnabled = true;
    }

    public void Mute() {
        Debug.Log("Mute");
        this.recorder.TransmitEnabled = false;
    }
}
