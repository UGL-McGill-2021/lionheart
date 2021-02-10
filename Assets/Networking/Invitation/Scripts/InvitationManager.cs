using AssemblyCSharp.Assets;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

enum ControllerButton {
    X = 0,
    Y = 1,
    A = 2,
    B = 3
}

/* 
 *  This class handles the creation of invitation codes and display them on canvas
 *  Special note: Invitation code = room name
 */
public class InvitationManager : MonoBehaviourPunCallbacks {
    public InvitationCodeInputAction InputAction;
    private string InvitationCodeInput = "";

    [Header("Invitation Code Properties")]
    public int CodeLength = 4;

    [Header("UI")]
    public InvitationCodePresenter GeneratedCodePresenter;
    public InvitationCodePresenter InputCodePresenter;

    /// <summary>
    /// Author: Feiyang Li
    /// Initialize controller input system for invitation code input
    /// @TODO: Move this to UI controller when UI is being formally constructed
    /// </summary>
    private void Awake() {
        InputAction = new InvitationCodeInputAction();
        InputAction.Player.AddA.performed += _ => AddCodeCharacter('A');
        InputAction.Player.AddB.performed += _ => AddCodeCharacter('B');
        InputAction.Player.AddX.performed += _ => AddCodeCharacter('X');
        InputAction.Player.AddY.performed += _ => AddCodeCharacter('Y');
        InputAction.Player.Enable();

        PhotonNetwork.AutomaticallySyncScene = true;
    }

    /// <summary>
    /// Author: Feiyang Li
    /// Called at the beginning of the networking process,
    /// enabling players to invite another player or join another player
    /// </summary>
    void Start() {
        PhotonNetwork.ConnectUsingSettings();
    }

    /// <summary>
    /// Author: Feiyang Li
    /// Invoked when successfully connected to Photon network.
    /// Generate invitation code and display it to player
    /// </summary>
    public override void OnConnectedToMaster() {
        Debug.Log("Network(INFO): Connected to Master Server");
        if (InvitationCodeInput.Length == CodeLength) {
            PhotonNetwork.JoinRoom(string.Copy(InvitationCodeInput));
            InvitationCodeInput = "";
        } else
            CreateInvitation();
    }

    /// <summary>
    /// Author: Feiyang Li
    /// Create an invitation and display the invitation code on UI
    /// </summary>
    public void CreateInvitation() {
        // Generate and display invitation code
        string InvitationCode = "";
        for (int i = 0; i < CodeLength; i++) {
            InvitationCode += (ControllerButton)Random.Range(0, 3);
        }

        RoomOptions options = new RoomOptions();
        options.IsVisible = false;
        options.MaxPlayers = 2;
        options.IsOpen = true;
        options.EmptyRoomTtl = 100000;
        PhotonNetwork.CreateRoom(InvitationCode, options);

        Debug.Log("Network(INFO): Invitation Code " + InvitationCode);

        GeneratedCodePresenter.gameObject.SetActive(true);
        GeneratedCodePresenter.Present(InvitationCode);
    }

    /// <summary>
    /// Author: Feiyang Li
    /// Invoked when successfully handled the invitation
    /// </summary>
    public override void OnJoinedRoom() {
        // Debug.Log("Network (INFO): Successfully joined room " + PhotonNetwork.CurrentRoom.Name);
        //if (PhotonNetwork.IsMasterClient)
        //    PhotonNetwork.LoadLevel("SampleScene");
    }

    /// <summary>
    /// Author: Feiyang Li
    /// Invoked when 
    /// </summary>
    /// <param name="newPlayer"></param>
    public override void OnPlayerEnteredRoom(Player newPlayer) {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2) {
            Debug.Log("Network (INFO): Invitation complete!");
            if (PhotonNetwork.IsMasterClient)
                PhotonNetwork.LoadLevel("SampleScene");
        }
    }

    /// <summary>
    /// Author: Feiyang Li
    /// Invoked when failed to create invitation
    /// </summary>
    /// <param name="returnCode"></param>
    /// <param name="message"></param>
    public override void OnCreateRoomFailed(short returnCode, string message) {
        Debug.Log("Network(WARNING): Failed to create room: " + returnCode + " " + message);
    }

    private void AddCodeCharacter(char character) {
        if (GeneratedCodePresenter.gameObject.activeSelf)
            GeneratedCodePresenter.gameObject.SetActive(false);

        if (!InputCodePresenter.gameObject.activeSelf)
            InputCodePresenter.gameObject.SetActive(true);

        InvitationCodeInput += character;

        InputCodePresenter.Present(InvitationCodeInput);

        if (InvitationCodeInput.Length == 4) {
            // invitation code input completed. Accept invitation and try to join.
            if (PhotonNetwork.CurrentRoom != null) {
                PhotonNetwork.LeaveRoom();
                GeneratedCodePresenter.enabled = false;
            } else {
                PhotonNetwork.JoinRoom(string.Copy(InvitationCodeInput));
                InvitationCodeInput = "";
            }
        }
    }

    public override void OnJoinRoomFailed(short returnCode, string message) {
        Debug.Log("Failed to join room " + message);
        InputCodePresenter.Present("");
    }
}
