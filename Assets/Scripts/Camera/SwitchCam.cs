using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Photon.Pun;
using UnityEngine.InputSystem;
using UnityEngine;

public class SwitchCam : MonoBehaviour
{
    public ControllerInput ControllerActions;
    public CinemachineVirtualCamera Cam;
    public List<GameObject> PlayerList;
    public bool BlockInput = false;

    private bool CurrState = true;

    private void Awake()
    {
        ControllerActions = new ControllerInput();
        Cam = GetComponent<CinemachineVirtualCamera>();
    }

    void Update()
    {
        if (Cam.Follow == null) {
            PlayerList = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().PlayerList;
            foreach (GameObject O in PlayerList)
            {
                if (O.GetComponent<PhotonView>().IsMine)
                {
                    Cam.Follow = O.transform;
                    return;
                }
            }
        }
    }

    private void OnEnable()
    {
        ControllerActions.Player.SwitchCamera.performed += RegisterSwitchCamera;
        ControllerActions.Player.SwitchCamera.Enable();
    }

    private void OnDisable()
    {
        ControllerActions.Player.SwitchCamera.performed -= RegisterSwitchCamera;
        ControllerActions.Player.SwitchCamera.Disable();
    }

    private void RegisterSwitchCamera(InputAction.CallbackContext Ctx)
    {
        if (BlockInput == false)
        {
            Cam.enabled = CurrState;
            CurrState = !CurrState;
        }
    }

    public IEnumerator WaitForButtonRelease()
    {
        yield return new WaitWhile(() => Gamepad.current.buttonEast.isPressed);
        BlockInput = false;
    }
}
