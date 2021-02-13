using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Author: Ziqi Li
/// A local version of moving platform for testing
/// </summary>
public class LocalMovingPlatform : MonoBehaviour
{
    public List<GameObject> PathPointObjects = new List<GameObject>();
    public float delay = 2f;
    public float speed = 3f;
    public float epsilon = 0.2f;
    public bool isAutomatic = true;  // whether this platform will move automatically

    [SerializeField]
    private List<Vector3> PathPoints = new List<Vector3>();
    // list to store players standing on this platform
    [SerializeField]
    private List<PrototypeCharacterMovementControls> PlayersList = new List<PrototypeCharacterMovementControls>();
    private Vector3 _CurrentTarget;
    [SerializeField]
    private Vector3 _CurrentVelocity;
    private Vector3 _RealVelocity;
    private int _CurrentTargetIndex;
    private bool isMovingToward = true;
    private bool isTriggered = false;  // motion of platform have to be triggered if !isAutomatic
    private float DelayTimer;

    public const byte AddChildEventCode = 1;  // Event code for Photon RaiseEvent
    public const byte RemoveChildEventCode = 2;  // Event code for Photon RaiseEvent

    // Start is called before the first frame update
    void Start()
    {
        // initialization
        isTriggered = isAutomatic;
        DelayTimer = delay;

        // Need at least 2 points to move
        if (PathPointObjects.Count > 1)
        {
            // convert to a Vector3 list
            foreach (GameObject obj in PathPointObjects) PathPoints.Add(obj.transform.position);

            _CurrentTarget = PathPoints[0];
            _CurrentTargetIndex = 0;
        }
    }

    private void FixedUpdate()
    {
        DelayTimer += Time.deltaTime;

        if ((PathPoints.Count) > 1 && isTriggered && (DelayTimer > delay))
        {
            if (Vector3.Distance(this.transform.position, _CurrentTarget) < epsilon)
            {
                UpdateTarget();

                // stop for a delay when reaching two ends of path
                if ((_CurrentTargetIndex == 1 && isMovingToward) || (_CurrentTargetIndex == PathPoints.Count - 2 && !isMovingToward))
                {
                    DelayTimer = 0;
                    _CurrentVelocity = Vector3.zero;
                }
            }
            else
            {
                MovePlatform();
            }
        }

        if (PlayersList.Count > 0)
        {
            foreach (PrototypeCharacterMovementControls player in PlayersList)
                player.AddVelocity(_CurrentVelocity);
        }

        //if(!PhotonView.IsMine) _CurrentVelocity = _RealVelocity;
    }

    /// <summary>
    /// Author: Ziqi Li
    /// Called by PUN several times per second, so that your script can write and
    /// read synchronization data for the PhotonView
    /// This method will be called in scripts that are assigned as Observed component of a PhotonView
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="info"></param>
    //public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    //{
    //    // Sending messages to server if this object belong to the current client, otherwise receive messages
    //    if (stream.IsWriting)
    //    {
    //        stream.SendNext(_CurrentVelocity);
    //    }
    //    else
    //    {
    //        _RealVelocity = (Vector3)stream.ReceiveNext();
    //    }
    //}

    /// <summary>
    /// Author: Ziqi Li
    /// RPC function to set the current velocity value of this platform
    /// (the current velocity will be added to players standing on this platform)
    /// </summary>
    /// <param name="velocity"></param>
    //[PunRPC]
    //void RPC_SetCurrentVelocity(Vector3 velocity)
    //{
    //    _CurrentVelocity = velocity;
    //}


    /// <summary>
    /// Author: Ziqi Li
    /// Function for moving this platform
    /// </summary>
    private void MovePlatform()
    {
        Vector3 TargetDirection = _CurrentTarget - this.transform.position;
        if (_CurrentVelocity != TargetDirection.normalized * speed) _CurrentVelocity = TargetDirection.normalized * speed;
        this.transform.Translate(TargetDirection.normalized * speed * Time.deltaTime);
    }

    /// <summary>
    /// Author: Ziqi Li
    /// Function for updating the next target position of the moving path from the PathPoints list
    /// </summary>
    private void UpdateTarget()
    {
        // If the platform goes back and forth along the path points
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
    /// Callback function of isTrigger collider
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (!isAutomatic) isTriggered = true;  // trigger the platform motion once a player standing on it

        if (other.tag == "Player")
        {
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
        if (other.tag == "Player")
        {
            PlayersList.Remove(other.gameObject.GetComponent<PrototypeCharacterMovementControls>());
        }
    }
}
