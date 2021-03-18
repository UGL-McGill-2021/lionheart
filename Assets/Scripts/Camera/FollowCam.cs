using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

/// <summary>
/// Author: Daniel Holker
/// </summary>

public class FollowCam : MonoBehaviour
{
    public float MaxDistance = 30f;           //Distance at which the camera starts to pull back
    public float SmoothTime = .5f;
    public float PullbackSpeed = 1.5f;

    public List<GameObject> PlayerList;
    public Camera Cam;


    private Vector3 OffSet;
    private Vector3 Velocity;

    private void Awake()
    {
        PlayerList = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().PlayerList;
    }

    void LateUpdate()
    {
        if (PlayerList.Count == 0)
        {
            return;
        }

        //Set Camera Position
        Vector3 CenterPoint = GetCenterPoint();
        Vector3 NewPosition = CenterPoint + OffSet;
        transform.position = Vector3.SmoothDamp(transform.position, NewPosition, ref Velocity, SmoothTime);

        if (PlayerList.Count < 2) return;

        //Pull camera back if players are too far apart
        float _Distance = Vector3.Distance(PlayerList[0].transform.position, PlayerList[1].transform.position);
        if (_Distance > MaxDistance)
        {
            float _OffSetFactor = (_Distance - MaxDistance) / PullbackSpeed;
            OffSet = new Vector3(0f, _OffSetFactor/5, -(_OffSetFactor));

        } else
        {
            OffSet = Vector3.zero;
        }

    }

    private bool Visible(GameObject Object)
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Cam);
        if (GeometryUtility.TestPlanesAABB(planes, Object.GetComponent<Collider>().bounds))
            return true;
        else
            Debug.Log(Object.name + " is not in frame.");
            return false;
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

        return Bounds.center;
    }
}