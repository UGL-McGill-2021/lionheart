using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

enum ControllerButton {
    X = 0,
    Y = 1,
    A = 2,
    B = 3
}

/* Special Note:
 * Invitation code is room name
 */
public class InvitationManager : MonoBehaviourPunCallbacks {
    [Header("Invitation Code Properties")]
    public int CodeLength = 4;

    [Header("UI")]
    public InvitationCodePresenter CodePresenter;

    /// <summary>
    /// Called at the beginning of the networking process,
    /// enabling players to invite another player or join another player
    /// </summary>
    void Start() {
        PhotonNetwork.ConnectUsingSettings();
    }

    /// <summary>
    /// Invoked when successfully connected to Photon network.
    /// Generate invitation code and display it to player
    /// </summary>
    public override void OnConnectedToMaster() {
        Debug.Log("Networking(INFO): Connected to Master");

        // Generate and display invitation code
        string InvitationCode = "";
        for (int i = 0; i < CodeLength; i++) {
            InvitationCode += (ControllerButton) Random.Range(0, 3);
        }

        RoomOptions options = new RoomOptions();
        options.IsVisible = false;
        options.MaxPlayers = 2;
        options.IsOpen = true;
        PhotonNetwork.CreateRoom(InvitationCode, options);

        Debug.Log("Networking(INFO): Invitation Code " + InvitationCode);

        CodePresenter.Present(InvitationCode);
    }
}
