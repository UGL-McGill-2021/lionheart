using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Photon.Pun;
using System.Collections.Generic;
using Lionheart.Player.Movement;
using UnityEngine.EventSystems;
using Photon.Voice.PUN;

public class QuitMenuManager : MenuManager
{
    public GameObject QuitMenuUI;
    public Button ContinueButton;
    public Button QuitButton;

    private List<GameObject> PlayerList;
    [SerializeField]
    private MultiplayerActivator CurrentPlayerActivator = null;  // the current client's playerActivator
    private PhotonView PhotonView;

    // Use this for initialization
    void Start()
    {
        PhotonView = GetComponent<PhotonView>();
        QuitMenuUI.SetActive(false);  // hide the menu UI
        PlayerList = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().PlayerList;
        if (ContinueButton != null)
        {
            base.DefaultButton = ContinueButton;  // set the defaultButton in the parent class
        }
    }

    /// <summary>
    /// Author: Ziqi
    /// An Update function that override (extend) the parent class's Update function
    /// </summary>
    protected override void Update()
    {
        // handle the default selection of button
        if (QuitMenuUI.activeSelf) base.Update();
        else if(EventSystem.current.currentSelectedGameObject) EventSystem.current.SetSelectedGameObject(null);

        // Get the current player activator from game manager
        // (in Update instead of Start since the Player list may be not initialized due to Start() execution order
        // so we may have to keep trying to find current player)
        GetPlayerActivator();
    }

    /// <summary>
    /// Author: Ziqi
    /// Function to get current player activator
    /// </summary>
    /// <param name="numPlayers">Number of players we have</param>
    void GetPlayerActivator()
    {
        // we have to keep trying to find current player from the player list
        if (CurrentPlayerActivator == null)
        {
            foreach (GameObject player in PlayerList)
            {
                if (player != null && player.GetComponent<PhotonView>().IsMine) CurrentPlayerActivator = player.GetComponent<MultiplayerActivator>();
            }
        }
    }

    /// <summary>
    /// Author: Ziqi Li
    /// Callback function of input system
    /// </summary>
    void OnOpenMenu()
    {
        if(!QuitMenuUI.activeSelf)
        {
            QuitMenuUI.SetActive(true);
            CurrentPlayerActivator.DisableControls();  // disable the current player control when open menu
        }
        else
        {
            QuitMenuUI.SetActive(false);
            CurrentPlayerActivator.EnableControls();
        }
    }

    /// <summary>
    /// Author: Ziqi Li
    /// Button callback function to close the menu
    /// </summary>
    public void Continue()
    {
        QuitMenuUI.SetActive(false);
        CurrentPlayerActivator.EnableControls();
    }

    /// <summary>
    /// Author: Ziqi Li
    /// Button callback function to back to the main menu
    /// </summary>
    public void Quit()
    {
        // use AllViaServer option to make sure that the RPC can be executed on both client before the client disconnects
        PhotonView.RPC("RPC_Quit", RpcTarget.AllViaServer);
    }

    /// <summary>
    /// Author: Ziqi Li
    /// Function to back to the main menu
    /// </summary>
    [PunRPC]
    private void RPC_Quit()
    {
        Destroy(GameObject.FindGameObjectWithTag("VoiceChatManager"));  // destroy the "DontDestroyOnLoad" VoiceChat object
        PhotonNetwork.Disconnect();  // disconnect from Photon
        SceneLoader.LoadSceneWithName("MainMenu");
    }
}
