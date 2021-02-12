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
public class MovingPlatform : MonoBehaviour, IOnEventCallback
{
    public List<GameObject> PathPointObjects = new List<GameObject>();
    public float delay = 2f;
    public float speed = 3f;
    public float epsilon = 0.2f;
    public bool isAutomatic = true;  // whether this platform will move automatically

    [SerializeField]
    private List<Vector3> PathPoints = new List<Vector3>();
    private Vector3 _CurrentTarget;
    private int CurrentTargetIndex;
    private bool isMovingToward = true;
    private bool isTriggered = false;  // motion of platform have to be triggered if !isAutomatic
    private float DelayTimer;
    PhotonView PhotonView;

    public const byte AddChildEventCode = 1;  // Event code for Photon RaiseEvent
    public const byte RemoveChildEventCode = 2;  // Event code for Photon RaiseEvent

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
            CurrentTargetIndex = 0;
            //CurrentMovingCoroutine = StartCoroutine(MovePlatform());  // start the coroutine
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
                if ((CurrentTargetIndex == 1 && isMovingToward) || (CurrentTargetIndex == PathPoints.Count - 2 && !isMovingToward))
                {
                    DelayTimer = 0;
                }
            }
            MovePlatform();
        }

    }


    /// <summary>
    /// Author: Ziqi Li
    /// Function for moving this platform
    /// </summary>
    private void MovePlatform()
    {
        Vector3 TargetDirection = _CurrentTarget - this.transform.position;
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
            if (CurrentTargetIndex < PathPoints.Count - 1)
            {
                CurrentTargetIndex++;
            }
            else
            {
                isMovingToward = false;
                CurrentTargetIndex--;
            }
        }
        else
        {
            if (CurrentTargetIndex == 0)
            {
                isMovingToward = true;
                CurrentTargetIndex++;
            }
            else
            {
                CurrentTargetIndex--;
            }
        }
        _CurrentTarget = PathPoints[CurrentTargetIndex];  // update the current target
    }
    
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
