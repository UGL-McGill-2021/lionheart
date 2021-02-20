﻿using UnityEngine;
using System.Collections;
using Photon.Pun;
using System.Collections.Generic;

public class MovingPlatformRB : MonoBehaviour
{
    public bool isOffLine;
    public List<GameObject> PathPointObjects = new List<GameObject>();
    public float StopTime = 2f;  // the time of stopping when reaching ends
    public float speed = 3f;
    public float epsilon = 0.2f;
    public bool isAutomatic = true;  // whether this platform will move automatically

    [SerializeField]
    private List<Vector3> PathPoints = new List<Vector3>();
    // list to store players standing on this platform
    [SerializeField]
    private List<PrototypeCharacterMovementControls> PlayersList = new List<PrototypeCharacterMovementControls>();
    [SerializeField]
    private Vector3 _CurrentTarget;
    private Vector3 _RealVelocity;

    // Motion:
    [SerializeField]
    private Vector3 _CurrentVelocity;
    private int _CurrentTargetIndex;
    private bool isMovingToward = true; // for determining when to stop
    private bool isTriggered = false;  // motion of platform have to be triggered by player if !isAutomatic
    private Coroutine CurrentCoroutine;

    private PhotonView PhotonView;
    private Rigidbody RigidBody;

    //Lag compensation
    //float _CurrentTime = 0;
    //double _CurrentPacketTime = 0;
    //double _LastPacketTime = 0;

    private Vector3 RemotePosition;
    private Quaternion RemoteRotation;

    // Start is called before the first frame update
    void Start()
    {
        // initialization
        if (isOffLine) PhotonNetwork.OfflineMode = true;
        PhotonView = this.GetComponent<PhotonView>();
        RigidBody = this.GetComponent<Rigidbody>();

        isTriggered = isAutomatic;

        // Need at least 2 points to move
        if(PathPointObjects.Count > 1)
        {
            // convert to a Vector3 list
            foreach (GameObject obj in PathPointObjects)
                PathPoints.Add(obj.transform.position);
            _CurrentTarget = PathPoints[0];
            _CurrentTargetIndex = 0;
        }

        if(isOffLine || PhotonView.IsMine)
        {
            CurrentCoroutine = StartCoroutine(StartMotion(isTriggered));
        }
            
    }

    private void FixedUpdate()
    {
        if(!isOffLine && !PhotonView.IsMine)
        {
            // Only add additional velocity for non master clients (Platforms are belong to the master client)
            if (PlayersList.Count > 0)
            {
                foreach (PrototypeCharacterMovementControls player in PlayersList)
                {
                    if (player.gameObject.GetComponent<PhotonView>().IsMine) player.AddVelocity(_CurrentVelocity);
                }
            }
        }
    }

    /// <summary>
    /// Author: Ziqi Li
    /// Coroutine for starting the motion of this platform
    /// </summary>
    /// <returns></returns>
    private IEnumerator StartMotion(bool isTriggered)
    {
        while (isTriggered)
        {
            if (Vector3.Distance(this.transform.position, _CurrentTarget) < epsilon)
            {
                UpdateTarget();

                // stop for a delay when reaching two ends of path
                if ((_CurrentTargetIndex == 1 && isMovingToward) || (_CurrentTargetIndex == PathPoints.Count - 2 && !isMovingToward))
                {
                    // call the RPC function to reset the current velocity
                    if (PhotonView.IsMine) PhotonView.RPC("RPC_SetCurrentVelocity", RpcTarget.All, Vector3.zero);
                    yield return new WaitForSeconds(StopTime);
                }
            }
            else
            {
                MovePlatform();
                yield return new WaitForFixedUpdate();
            }
        }
        
    }

    /// <summary>
    /// Author: Ziqi Li
    /// Function for moving this platform
    /// </summary>
    private void MovePlatform()
    {
        Vector3 TargetDirection = _CurrentTarget - this.transform.position;

        // call RPC to update the current velocity field
        if (PhotonView.IsMine && _CurrentVelocity != TargetDirection.normalized * speed) PhotonView.RPC("RPC_SetCurrentVelocity", RpcTarget.All, TargetDirection.normalized * speed);
        this.RigidBody.MovePosition(this.transform.position + TargetDirection.normalized * speed * Time.deltaTime);
    }

    /// <summary>
    /// Author: Ziqi Li
    /// Function for updating the next target position of the moving path from the PathPoints list
    /// </summary>
    private void UpdateTarget()
    {
        if (isMovingToward)
        {
            if (_CurrentTargetIndex < PathPoints.Count - 1)
            {
                _CurrentTargetIndex++;
            }
            else
            {
                isMovingToward = false;
                _CurrentTargetIndex--;
            }
        }
        else
        {
            if (_CurrentTargetIndex == 0)
            {
                isMovingToward = true;
                _CurrentTargetIndex++;
            }
            else
            {
                _CurrentTargetIndex--;
            }
        }
        _CurrentTarget = PathPoints[_CurrentTargetIndex];  // update the current target
    }

    /// <summary>
    /// Author: Ziqi Li
    /// RPC function to set the current velocity value of this platform
    /// (the current velocity will be added to players standing on this platform)
    /// </summary>
    /// <param name="velocity"></param>
    [PunRPC]
    void RPC_SetCurrentVelocity(Vector3 velocity)
    {
        _CurrentVelocity = velocity;
    }

    /// <summary>
    /// Author: Ziqi Li
    /// Callback function of isTrigger collider
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // trigger the platform motion once a player standing on it
            if (!isAutomatic && !isTriggered)
            {
                isTriggered = true;
                CurrentCoroutine = StartCoroutine(StartMotion(isTriggered));
            }
            // update player list
            PlayersList.Add(other.gameObject.GetComponent<PrototypeCharacterMovementControls>());
        }
    }

    /// <summary>
    /// Author: Ziqi Li
    /// Callback function of isTrigger collider
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayersList.Remove(other.gameObject.GetComponent<PrototypeCharacterMovementControls>());
        }
    }
}