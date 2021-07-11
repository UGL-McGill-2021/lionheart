using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Photon.Pun;
using UnityEngine.InputSystem;
using UnityEngine;

/// <summary>
/// Author: Denis
/// Newer camera implementation that includes refined versions of the 
/// follow and switch cams.
/// 
/// The group cam replaces the follow cam. Exhibits the same behavior where the
/// cam keeps both players in frame until the other player gets too far. Then it 
/// focuses on the local player exclusively.
/// 
/// The third person cam replaces the player cam. It is a traditional 3rd person camera.
/// </summary>
public class CameraMultimode : MonoBehaviour
{
    [Header("References")]
    [SerializeField] ControllerInput ControllerActions;
    [SerializeField] GameManager Gm;
    [SerializeField] CinemachineVirtualCamera GroupCam;
    [SerializeField] CinemachineVirtualCamera ThirdCam;
    [SerializeField] GameObject GroupCamTarget;
    [SerializeField] GameObject ThirdCamTarget;
    [SerializeField] GameObject LocalPlayer;
    [SerializeField] Animator CamAnimator;

    [Header("State")]
    [SerializeField] bool IsGroupCam = true;
    [SerializeField] bool BlockInput = false;
    [SerializeField] Vector3 OffSet;
    [SerializeField] Vector3 Velocity;
    [SerializeField] Vector3 LastCenter;


    [Header("Parameters")]
    [SerializeField] float MaxDistance = 30f;           //Distance at which the camera starts to pull back
    [SerializeField] float SmoothTime = .5f;
    [SerializeField] float PullbackSpeed = 1.5f;
    [SerializeField] float StopFollowHeight;               //Height at which the camera stops following the player


    /// <summary>
    /// Author: Denis
    /// Initial setup
    /// </summary>
    void Awake()
    {
        ControllerActions = new ControllerInput();
        Gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        GroupCam.LookAt = GroupCamTarget.transform;
        GroupCam.Follow = GroupCamTarget.transform;

        ThirdCam.LookAt = ThirdCamTarget.transform;
        ThirdCam.Follow = ThirdCamTarget.transform;
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

    /// <summary>
    /// Author: Denis
    /// Switches the active camera
    /// </summary>
    /// <param name="Ctx"></param>
    private void RegisterSwitchCamera(InputAction.CallbackContext Ctx)
    {
        if (BlockInput == false)
        {
            IsGroupCam = !IsGroupCam;

            if (IsGroupCam == true)
            {
                GroupCam.Priority = 1;
                ThirdCam.Priority = 0;
                GroupCam.ForceCameraPosition(ThirdCam.transform.position, ThirdCam.transform.rotation);
                CamAnimator.Play("GroupCam");
            }
            else
            {
                GroupCam.Priority = 0;
                ThirdCam.Priority = 1;
                ThirdCam.ForceCameraPosition(GroupCam.transform.position, GroupCam.transform.rotation);
                CamAnimator.Play("ThirdCam");
            }
        }
    }

    /// <summary>
    /// Author: Denis
    /// Prevents the switch from happening when exiting the pause menu with B
    /// </summary>
    /// <returns></returns>
    public IEnumerator WaitForButtonRelease()
    {
        yield return new WaitWhile(() => Gamepad.current.buttonEast.isPressed);
        BlockInput = false;
    }

    private void Update()
    {
        //cache local player ref
        if (LocalPlayer == null) LocalPlayer = GetLocalPlayer();
    }

    /// <summary>
    /// Author: Daniel, Denis
    /// </summary>
    private void LateUpdate()
    {
        if (Gm.PlayerList.Count == 0)
        {
            return;
        }

        if (IsGroupCam == true)
        {
            RunGroupCam();
            //ThirdCam.ForceCameraPosition(GroupCam.transform.position, GroupCam.transform.rotation);
        }
        else
        {
            RunThirdCam();
            //GroupCam.ForceCameraPosition(ThirdCam.transform.position, ThirdCam.transform.rotation);
        }
    }

    /// <summary>
    /// Author: Denis
    /// Makes the third person camera follow the player
    /// </summary>
    private void RunThirdCam()
    {
        ThirdCamTarget.transform.position = LocalPlayer.transform.position;
    }

    /// <summary>
    /// Author: Daniel, Denis
    /// Adjust the camera based on the player positions
    /// </summary>
    private void RunGroupCam()
    {
        //Set Camera Position
        Vector3 CenterPoint = GetCenterPoint();
        //print(CenterPoint);
        Vector3 NewPosition = CenterPoint + OffSet;
        transform.position = Vector3.SmoothDamp(transform.position, NewPosition, ref Velocity, SmoothTime);

        if (Gm.PlayerList.Count < 2)
        {
            GroupCamTarget.transform.position = NewPosition;
            return;
        }

        //Pull camera back if players are too far apart
        float _Distance = Vector3.Distance(Gm.PlayerList[0].transform.position, Gm.PlayerList[1].transform.position);
        if (_Distance > MaxDistance)
        {
            float _OffSetFactor = (_Distance - MaxDistance) / PullbackSpeed;
            OffSet = new Vector3(0f, _OffSetFactor / 5, -(_OffSetFactor));
            GroupCamTarget.transform.position = LocalPlayer.transform.position;

        }
        else
        {
            OffSet = Vector3.zero;
            GroupCamTarget.transform.position = NewPosition;
        }
    }

    /// <summary>
    /// Author: Daniel
    /// </summary>
    /// <returns>
    /// 2 Players: point halfway between them
    /// 1 Player: player position
    /// </returns>
    private Vector3 GetCenterPoint()
    {
        if (Gm.PlayerList.Count == 1)
        {
            return Gm.PlayerList[0].transform.position;
        }

        Bounds Bounds = new Bounds(Gm.PlayerList[0].transform.position, Vector3.zero);
        foreach (GameObject G in Gm.PlayerList)
        {
            Bounds.Encapsulate(G.transform.position);
        }

        LastCenter = Bounds.center;
        return Bounds.center;
    }

    /// <summary>
    /// Author: Denis
    /// </summary>
    /// <returns>
    /// On Successful search: Player GameObject
    /// Else null
    /// </returns>
    private GameObject GetLocalPlayer()
    {
        foreach (GameObject O in Gm.PlayerList)
        {
            if (O.GetComponent<PhotonView>().IsMine)
            {
                return O;
            }
        }
        return null;
    }
}
