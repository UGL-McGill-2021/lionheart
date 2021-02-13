using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;


/// <summary>
/// Author: Ziqi Li
/// Script for moving platform object
/// </summary>
public class MovingPlatform : MonoBehaviour, IPunObservable, IOnEventCallback
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
    PhotonView PhotonView;

    public const byte AddChildEventCode = 1;  // Event code for Photon RaiseEvent
    public const byte RemoveChildEventCode = 2;  // Event code for Photon RaiseEvent

    //Lag compensation
    float _CurrentTime = 0;
    double _CurrentPacketTime = 0;
    double _LastPacketTime = 0;

    private Vector3 RemotePosition;
    private Quaternion RemoteRotation;

    // Start is called before the first frame update
    void Start()
    {
        // initialization
        isTriggered = isAutomatic;
        DelayTimer = delay;
        PhotonView = this.GetComponent<PhotonView>();

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
        if(PhotonView.IsMine)
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
                        // call the RPC function to reset the current velocity
                        if (PhotonView.IsMine) PhotonView.RPC("RPC_SetCurrentVelocity", RpcTarget.AllViaServer, Vector3.zero);
                        //_CurrentVelocity = Vector3.zero;
                    }
                }
                else
                {
                    MovePlatform();
                }
            }
        }
        else
        {
            //Lag compensation
            //double timeToReachGoal = _CurrentPacketTime - _LastPacketTime;
            //_CurrentTime += Time.deltaTime;

            ////Update remote position
            transform.position = Vector3.Lerp(transform.position, RemotePosition, Time.deltaTime);
            //transform.rotation = Quaternion.Lerp(transform.rotation, RemoteRotation, (float)(_CurrentTime / timeToReachGoal));
        }


        if(PlayersList.Count > 0)
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
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // Sending messages to server if this object belong to the current client, otherwise receive messages
        if (stream.IsWriting)
        {
            //stream.SendNext(_CurrentVelocity);
            stream.SendNext(transform.position);
            //stream.SendNext(transform.rotation);
        }
        else
        {
            //_RealVelocity = (Vector3)stream.ReceiveNext();
            RemotePosition = (Vector3)stream.ReceiveNext();
            //RemoteRotation = (Quaternion)stream.ReceiveNext();

            //Lag compensation
            //_CurrentTime = 0.0f;
            //_LastPacketTime = _CurrentPacketTime;
            //_CurrentPacketTime = info.SentServerTime;
        }
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
    /// Function for moving this platform
    /// </summary>
    private void MovePlatform()
    {
        Vector3 TargetDirection = _CurrentTarget - this.transform.position;
        if (_CurrentVelocity != TargetDirection.normalized * speed) PhotonView.RPC("RPC_SetCurrentVelocity", RpcTarget.AllViaServer, TargetDirection.normalized * speed);
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

    /*
    /// <summary>
    /// Author: Ziqi Li
    /// Callback function of isTrigger collider
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (!isAutomatic) isTriggered = true;

        if (PhotonView.IsMine && other.tag == "Player")
        {
            int ViewId = other.gameObject.GetComponent<PhotonView>().ViewID;
            PhotonView.RPC("AddChild", RpcTarget.AllViaServer, ViewId);  // have to use RPC call to add child
        }
    }

    /// <summary>
    /// Author: Ziqi Li
    /// Callback function of isTrigger collider
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if (PhotonView.IsMine && other.tag == "Player" && other.transform.parent == this.gameObject.transform)
        {
            int ViewId = other.gameObject.GetComponent<PhotonView>().ViewID;
            PhotonView.RPC("RemoveChild", RpcTarget.AllViaServer, ViewId);  // have to use RPC call to remove child
            //SendRemoveChildEvent(other.gameObject);
        }
    }

    //private void SendAddChildEvent(GameObject child)
    //{
    //    // set the Receivers to All in order to receive this event on the local client as well
    //    RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All }; 
    //    PhotonNetwork.RaiseEvent(AddChildEventCode, child, raiseEventOptions, SendOptions.SendReliable);
    //}

    //private void SendRemoveChildEvent(GameObject child)
    //{
    //    // set the Receivers to All in order to receive this event on the local client as well
    //    RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All }; 
    //    PhotonNetwork.RaiseEvent(RemoveChildEventCode, child, raiseEventOptions, SendOptions.SendReliable);
    //}

    /// <summary>
    /// Author: Ziqi Li
    /// Add a gameObject to the child of this platform
    /// Make this function RPC to call this function on the same platform on all clients
    /// </summary>
    /// <param name="child"></param>
    [PunRPC]
    private void AddChild(int ViewID)
    {
        Debug.Log(ViewID);
        //child.gameObject.transform.parent = transform;
        PhotonView.Find(ViewID).gameObject.transform.parent = transform;
        Debug.Log(PhotonNetwork.IsMasterClient);
    }

    /// <summary>
    /// Author: Ziqi Li
    /// Remove a gameObject from its parent
    /// Make this function RPC to call this function on the same platform on all clients
    /// </summary>
    /// <param name="child"></param>
    [PunRPC]
    private void RemoveChild(int ViewID)
    {
        //child.transform.parent = null;
        PhotonView.Find(ViewID).gameObject.transform.parent = null;
    }
    */
    /// <summary>
    /// Author: Ziqi Li
    /// Callback function when this gameObject is enable
    /// </summary>
    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    /// <summary>
    /// Author: Ziqi Li
    /// Callback function when this gameObject is disable
    /// </summary>
    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    /// <summary>
    /// Callback function of Phonton RaiseEvent
    /// </summary>
    /// <param name="photonEvent"></param>
    public void OnEvent(EventData photonEvent)
    {
    //    byte eventCode = photonEvent.Code;
    //    object[] data = (object[])photonEvent.CustomData;

    //    switch (eventCode)
    //    {
    //        case AddChildEventCode:
    //            GameObject child = (GameObject) data[0];
    //            AddChild(child);
    //            break;

    //        case RemoveChildEventCode:
    //            GameObject child1 = (GameObject)data[0];
    //            RemoveChild(child1);
    //            break;
    //    }

    }


}
