using Photon.Pun;
using Photon.Voice.PUN;
using Photon.Voice.Unity;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// Author: Ziqi Li
/// Script for voice chat manager
/// Note: attach the Player Input component and assign an InputAction to it
/// for using the callback functions from Input System
/// </summary>
public class VoiceChatManager : MonoBehaviour {

    public Recorder recorder;

    public Sprite TalkIcon;
    public Sprite MuteIcon;
    public Image MasterImage;
    public Image ClientImage;

    private PhotonVoiceNetwork punVoiceNetwork;
    private PhotonView PhotonView;

    private void Awake() {
        punVoiceNetwork = PhotonVoiceNetwork.Instance;
        PhotonView = GetComponent<PhotonView>();
    }

    /// <summary>
    /// Author: Ziqi Li
    /// CallBack function from input system
    /// Activate talking
    /// </summary>
    public void OnTalk() {
        Debug.Log("Talk");
        this.recorder.TransmitEnabled = true;

        // set the mic icon using RPC (don't need PhotonView.isMine since both clients have only one VC manager)
        if (PhotonNetwork.IsMasterClient) PhotonView.RPC("RPC_SetTalkIcon", RpcTarget.All, true);
        else PhotonView.RPC("RPC_SetTalkIcon", RpcTarget.All, false);
    }

    /// <summary>
    /// Author: Ziqi Li
    /// CallBack function from input system
    /// Mute talking
    /// </summary>
    public void OnMute() {
        Debug.Log("Mute");
        this.recorder.TransmitEnabled = false;

        if (PhotonNetwork.IsMasterClient) PhotonView.RPC("RPC_SetMuteIcon", RpcTarget.All, true);
        else PhotonView.RPC("RPC_SetMuteIcon", RpcTarget.All, false);
    }

    /// <summary>
    /// Author: Ziqi Li
    /// RPC function for setting talking icon
    /// </summary>
    /// <param name="isMaster"></param>
    [PunRPC]
    private void RPC_SetTalkIcon(bool isMaster)
    {
        if (isMaster) MasterImage.sprite = TalkIcon;
        else ClientImage.sprite = TalkIcon;
    }

    /// <summary>
    /// Author: Ziqi Li
    /// RPC function for setting mute icon
    /// </summary>
    /// <param name="isMaster"></param>
    [PunRPC]
    private void RPC_SetMuteIcon(bool isMaster)
    {
        if (isMaster) MasterImage.sprite = MuteIcon;
        else ClientImage.sprite = MuteIcon;
    }
}
