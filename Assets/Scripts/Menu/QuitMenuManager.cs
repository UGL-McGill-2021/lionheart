using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Photon.Pun;
using System.Collections.Generic;
using Lionheart.Player.Movement;
using UnityEngine.EventSystems;
using Photon.Voice.PUN;
using TMPro;
using UnityEngine.SceneManagement;

public class QuitMenuManager : MenuManager
{
    [Header("UI elements")]
    public GameObject QuitMenuUI;
    public Button ContinueButton;
    public Button QuitButton;
    public Toggle VibToggle;
    public Slider VolumeSlider;
    public TMP_Text CountDownText;

    [Header("Respawn System")]
    public float RespawnConsentTime = 15f;
    public float RespawnHeight = -35f;

    private List<GameObject> PlayerList;
    [SerializeField]
    private MultiplayerActivator CurrentPlayerActivator = null;  // the current client's playerActivator
    private PhotonView PhotonView;
    private int Respawn_RequestPlayerNum = 0;  // number of player want to reload level
    private Coroutine RespawnCoroutine = null;
    private bool HasSentRespawnRequest = false;

    // Use this for initialization
    void Start()
    {
        PhotonView = GetComponent<PhotonView>();

        // initialize the user setting using static PlayerGameSettings class
        // so the setting will be keept when loading new scene
        VibToggle.isOn = PlayerGameSettings.IsVibrationOn;
        VolumeSlider.value = PlayerGameSettings.AudioVolume;
        PlayerGameSettings.IsInGameMenuOpened = false;
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
                if (player != null && player.GetComponent<PhotonView>().IsMine)
                {
                    CurrentPlayerActivator = player.GetComponent<MultiplayerActivator>();
                    // initialize the player setting using static class field, so the setting will be keept when loading new scene
                    CurrentPlayerActivator.hasVibration = PlayerGameSettings.IsVibrationOn;  
                }
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
            PlayerGameSettings.IsInGameMenuOpened = true;
            QuitMenuUI.SetActive(true);
            CurrentPlayerActivator.DisableControls();  // disable the current player control when open menu
        }
        else
        {
            PlayerGameSettings.IsInGameMenuOpened = false;
            QuitMenuUI.SetActive(false);
            CurrentPlayerActivator.EnableControls();
        }
    }

    // <summary>
    // Author: Ziqi Li
    // Callback function of input system
    // </summary>
    void OnCloseMenu()
    {
        if (QuitMenuUI.activeSelf)
        {
            PlayerGameSettings.IsInGameMenuOpened = false;
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
        PlayerGameSettings.IsInGameMenuOpened = false;
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
    /// Button callback function for reloading level
    /// </summary>
    public void ReloadLevel()
    {
        if (!HasSentRespawnRequest)
        {
            HasSentRespawnRequest = true;
            PhotonView.RPC("RPC_Respawn", RpcTarget.AllViaServer);
        }
        OnCloseMenu();  // close the menu
    }

    /// <summary>
    /// Author: Ziqi Li
    /// Toggle callback function for vibration toggle
    /// </summary>
    public void TurnVibration()
    {
        if(CurrentPlayerActivator != null)
        {
            PlayerGameSettings.IsVibrationOn = VibToggle.isOn;
            CurrentPlayerActivator.hasVibration = PlayerGameSettings.IsVibrationOn;
        }
    }

    /// <summary>
    /// Author: Ziqi Li
    /// Slider callback function for volume slider
    /// </summary>
    public void ChangeVolume()
    {
        PlayerGameSettings.AudioVolume = VolumeSlider.value;
    }

    /// <summary>
    /// Author: Ziqi Li
    /// RPC Function to back to the main menu
    /// </summary>
    [PunRPC]
    private void RPC_Quit()
    {
        Destroy(GameObject.FindGameObjectWithTag("VoiceChatManager"));  // destroy the "DontDestroyOnLoad" VoiceChat object
        PhotonNetwork.Disconnect();  // disconnect from Photon
        SceneLoader.LoadSceneWithName("MainMenu");
    }

    /// <summary>
    /// Author: Ziqi Li
    /// RPC Function to respawn players
    /// </summary>
    [PunRPC]
    private void RPC_Respawn()
    {
        Respawn_RequestPlayerNum++;
        if(RespawnCoroutine == null) RespawnCoroutine = StartCoroutine(RequestRespawn(RespawnConsentTime));
    }

    /// <summary>
    /// Author: Ziqi Li
    /// Function to request respawn and waiting for other player's consent
    /// </summary>
    IEnumerator RequestRespawn(float countDownTime)
    {
        // activate count down
        while (countDownTime > 0 && Respawn_RequestPlayerNum < PlayerList.Count)
        {
            ShowCountDownMessage("Player requests to respawn: ", Respawn_RequestPlayerNum, PlayerList.Count, countDownTime);
            yield return new WaitForSeconds(1);  // update the text every second
            countDownTime--;
        }

        // if all players want to reload
        if (Respawn_RequestPlayerNum >= PlayerList.Count)
        {
            string message = "Player requests to respawn: " + " " + Respawn_RequestPlayerNum + "/" + PlayerList.Count;
            CountDownText.SetText(message);
            yield return new WaitForSeconds(1f);  // add a delay before reload level
            foreach (GameObject player in PlayerList) if (player.GetComponent<PhotonView>().IsMine) player.transform.Translate(Vector3.up * RespawnHeight);
        }

        // reset status
        Respawn_RequestPlayerNum = 0;  // reset num of consent players
        RespawnCoroutine = null;
        HasSentRespawnRequest = false;
        CountDownText.SetText("");  // empty the text object
    }

    /// <summary>
    /// Author: Ziqi Li
    /// Function to show count down message along with player consent status
    /// </summary>
    /// <param name="text"></param>
    /// <param name="numPlayer"></param>
    /// <param name="maxNumPlayer"></param>
    /// <param name="countDownTime"></param>
    void ShowCountDownMessage(string text, int numPlayer, int maxNumPlayer, float countDownTime)
    {
        string message = text + " " + numPlayer + "/" + maxNumPlayer + " (" + countDownTime + "s)";
        CountDownText.SetText(message);
    }
}
