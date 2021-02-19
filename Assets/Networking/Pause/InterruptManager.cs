using UnityEngine;
using Photon.Pun;
using UnityEngine.InputSystem;

public class InterruptManager : IInRoomCallbacks {
    public GameObject PauseWidgetPrefab;
    public Canvas UICanvas;
    private InvitationCodePresenter Presenter;
    public PlayerInput PlayerInputComponent;

    private GameObject widget;

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer) {
        Debug.Log("Player left room");
        widget = Instantiate(PauseWidgetPrefab);
        widget.transform.SetParent(UICanvas.transform, false);
        RectTransform rect = widget.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(1, 0);
        rect.anchorMax = new Vector2(0, 1);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.offsetMax = new Vector2(10, 20);
        rect.offsetMin = new Vector2(800, 500);
        Presenter = widget.GetComponentInChildren<InvitationCodePresenter>();
        if (Presenter != null) {
            Presenter.Present(PhotonNetwork.CurrentRoom.Name);
        }

        if (PlayerInputComponent != null) {
            PlayerInputComponent.enabled = false;
        }

        Time.timeScale = 0;
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer) {
        Debug.Log("Player joined room");
        Destroy(widget);
        PlayerInputComponent.enabled = true;
        Time.timeScale = 1;
    }
}
