using System.Collections.Generic;
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

    private PhotonVoiceNetwork punVoiceNetwork;
    private PhotonView PhotonView;

    private List<GameObject> PlayerList;
    [SerializeField]
    private List<Image> PlayerIconList = new List<Image>();

    private void Awake() {
        punVoiceNetwork = PhotonVoiceNetwork.Instance;
        PhotonView = GetComponent<PhotonView>();

        PlayerList = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().PlayerList;
    }

    private void Update()
    {
        // Get the players' MicIcon components from game manager
        // (in Update instead of Start since the Player list may be not initialized due to Start() execution order
        // so we may habe to keep trying to get the players)
        GetPlayerMicIcons();
    }

    /// <summary>
    /// Author: Ziqi
    /// Function to get all players' MicIcon component and add them to the list
    /// </summary>
    /// <param name="numPlayers">Number of players we have</param>
    void GetPlayerMicIcons()
    {
        // if we didn't get all players in the list
        if (PlayerList.Count != PlayerIconList.Count)
        {
            PlayerIconList = new List<Image>();
            foreach (GameObject player in PlayerList)
            {
                foreach (Canvas canvas in player.GetComponentsInChildren<Canvas>())
                {
                    if (canvas.tag == "VoiceChatCanvas") PlayerIconList.Add(canvas.GetComponentInChildren<Image>());
                }
            }
        }
    }

    /// <summary>
    /// Author: Ziqi Li
    /// CallBack function from input system
    /// Activate talking
    /// </summary>
    public void OnTalk() {
        Debug.Log("Talk");
        this.recorder.TransmitEnabled = true;

        // find the current player and call RPC to update the icon state
        foreach (Image icon in PlayerIconList)
        {
            if(icon.GetComponentInParent<PhotonView>().IsMine)
            {
                int ViewId = icon.GetComponentInParent<PhotonView>().ViewID;
                PhotonView.RPC("RPC_SetTalkIcon", RpcTarget.AllViaServer, ViewId);
            }
        }

    }

    /// <summary>
    /// Author: Ziqi Li
    /// CallBack function from input system
    /// Mute talking
    /// </summary>
    public void OnMute() {
        Debug.Log("Mute");
        this.recorder.TransmitEnabled = false;

        // find the current player and call RPC to update the icon state
        foreach (Image icon in PlayerIconList)
        {
            if (icon.GetComponentInParent<PhotonView>().IsMine)
            {
                int ViewId = icon.GetComponentInParent<PhotonView>().ViewID;
                PhotonView.RPC("RPC_SetMuteIcon", RpcTarget.AllViaServer, ViewId);
            }
        }
    }

    /// <summary>
    /// Author: Ziqi Li
    /// RPC function for setting talking icon
    /// </summary>
    /// <param name="isMaster"></param>
    [PunRPC]
    private void RPC_SetTalkIcon(int playerViewID)
    {
        foreach (Image icon in PlayerIconList)
        {
            if (playerViewID == icon.GetComponentInParent<PhotonView>().ViewID)
            {
                icon.sprite = TalkIcon;
                // set icon to opaque
                var tempColor = icon.color;
                tempColor.a = 1f;
                icon.color = tempColor;
            }
        }
    }

    /// <summary>
    /// Author: Ziqi Li
    /// RPC function for setting mute icon
    /// </summary>
    /// <param name="isMaster"></param>
    [PunRPC]
    private void RPC_SetMuteIcon(int playerViewID)
    {
        foreach (Image icon in PlayerIconList)
        {
            if (playerViewID == icon.GetComponentInParent<PhotonView>().ViewID)
            {
                icon.sprite = MuteIcon;
                // set icon to transparent
                var tempColor = icon.color;
                tempColor.a = 0f;
                icon.color = tempColor;
            }
        }
    }
}
