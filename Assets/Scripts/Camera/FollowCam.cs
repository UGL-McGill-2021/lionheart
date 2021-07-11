using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Photon.Pun;
using UnityEngine;

/// <summary>
/// Author: Daniel Holker, Denis Racicot
/// </summary>

public class FollowCam : MonoBehaviour
{
    public float MaxDistance = 30f;           //Distance at which the camera starts to pull back
    public float SmoothTime = .5f;
    public float PullbackSpeed = 1.5f;

    public float StopFollowHeight;               //Height at which the camera stops following the player

    public List<GameObject> PlayerList;
    public Camera Cam;

    private Vector3 OffSet;
    private Vector3 Velocity;
    private Vector3 LastCenter;

    private CheckpointManager CheckpointMan;

    public CinemachineVirtualCamera VCam;
    public GameObject LocalPlayer;
    public GameObject CameraTarget;

    private void Awake()
    {
        PlayerList = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().PlayerList;
        CheckpointMan = GameObject.Find("CheckpointManager").GetComponent<CheckpointManager>();

        VCam.LookAt=CameraTarget.transform;
        VCam.Follow= CameraTarget.transform;
    }

    private void Update()
    {
        if (LocalPlayer == null)
        {
            if (PhotonNetwork.IsMasterClient == true)
            {
                LocalPlayer = PlayerList[0];
            }
            else
            {
                LocalPlayer = PlayerList[1];
            }
        }
    }

    void LateUpdate()
    {
        if (PlayerList.Count == 0)
        {
            return;
        }

        //Set Camera Position
        Vector3 CenterPoint = GetCenterPoint();
        //print(CenterPoint);
        Vector3 NewPosition = CenterPoint + OffSet;
        transform.position = Vector3.SmoothDamp(transform.position, NewPosition, ref Velocity, SmoothTime);

        if (PlayerList.Count < 2)
        {
            CameraTarget.transform.position = NewPosition;
            return;
        }

        //Stop tracking if a player falls too low

        //Pull camera back if players are too far apart
        float _Distance = Vector3.Distance(PlayerList[0].transform.position, PlayerList[1].transform.position);
        if (_Distance > MaxDistance)
        {
            float _OffSetFactor = (_Distance - MaxDistance) / PullbackSpeed;
            OffSet = new Vector3(0f, _OffSetFactor/5, -(_OffSetFactor));
            CameraTarget.transform.position = LocalPlayer.transform.position;

        } else
        {
            OffSet = Vector3.zero;
            CameraTarget.transform.position = NewPosition;
        }

        

    }

    Vector3 GetCenterPoint()
    {
        if (PlayerList.Count == 1)
        {
            return PlayerList[0].transform.position;
        }

        Bounds Bounds = new Bounds(PlayerList[0].transform.position, Vector3.zero);
        foreach(GameObject G in PlayerList)
        {
            Bounds.Encapsulate(G.transform.position);
        }

        LastCenter = Bounds.center;
        return Bounds.center;
    }
}
