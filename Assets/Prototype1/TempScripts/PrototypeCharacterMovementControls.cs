using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;
using AssemblyCSharp.Assets;
using System.Collections.Generic;

/// <summary>
/// Ahutor: Feiyang Li, Ziqi Li
/// Created for Prototype Testing Purposes
/// </summary>
public class PrototypeCharacterMovementControls : MonoBehaviour//, IPunObservable
{
    //public InputAction MoveAction = new InputAction("move", binding: "<Gamepad>/leftStick");
    //public InputAction LookAction = new InputAction("look", binding: "<Gamepad>/rightStick");

    public CharacterController controller;
    public GameObject ShootingPoint;
    public GameObject BulletPrefab;
    public float speed = 20f;
    public float gravity = -3f; 

    private Vector2 MoveDirection;
    private Vector2 LookDirection;
    private PhotonView PhotonView;

    private Vector3 RemotePosition;
    private Quaternion RemoteRotation;

    private float jumpSpeed = 6f;
    private Vector3 _AdditionalVelocity;  // additional velocity caused by other objects
    private bool isJumped = false;

    //Lag compensation
    float _CurrentTime = 0;
    double _CurrentPacketTime = 0;
    double _LastPacketTime = 0;


    private void Awake()
    {
        PhotonView = GetComponent<PhotonView>();
        _AdditionalVelocity = Vector3.zero;

        DualShock4GamepadHID controller = new AssemblyCSharp.Assets.DualShock4GamepadHID();
    }

    private void Start()
    {
        
    }

    private void FixedUpdate()
    {
        // If this character belong to the current client
        if (PhotonView.IsMine)
        {
            // calculate moving vector
            Vector3 movement = transform.forward * MoveDirection.y;
            movement += transform.right * MoveDirection.x;
            movement *= speed;

            // detect jump action
            if (controller.isGrounded)
            {
                if (isJumped)
                {
                    //set the y-movement to 0, and then add the y-movement with the jump movement value 
                    //_AdditionalVelocity.y = 0f;
                    //_AdditionalVelocity.y += Mathf.Sqrt(jumpSpeed * -gravity);
                    movement.y += Mathf.Sqrt(jumpSpeed * 200 * -gravity);
                    isJumped = false;
                }
            }
            else  //if not grounded, apply gravity
            {
                movement.y += gravity;
                isJumped = false;
            }

            movement.x += _AdditionalVelocity.x;
            movement.y += _AdditionalVelocity.y;
            movement.z += _AdditionalVelocity.z;
            controller.Move(movement * Time.deltaTime);

            // rotate character
            gameObject.transform.Rotate(new Vector3(0, LookDirection.x * 80 * Time.deltaTime, 0));
        }
        else
        {
            //Lag compensation
            double timeToReachGoal = _CurrentPacketTime - _LastPacketTime;
            _CurrentTime += Time.deltaTime;

            // update the position and rotation of this character (which doesn't belong to the current client)
            //transform.position = Vector3.Lerp(transform.position, RemotePosition, 0.1f);
            //transform.rotation = Quaternion.Lerp(transform.rotation, RemoteRotation, 0.1f);
            ////Update remote player
            transform.position = Vector3.Lerp(transform.position, RemotePosition, (float)(_CurrentTime / timeToReachGoal));
            transform.rotation = Quaternion.Lerp(transform.rotation, RemoteRotation, (float)(_CurrentTime / timeToReachGoal));
        }

        _AdditionalVelocity = Vector3.zero;
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
        // Sending messages to server if this character belong to the current client, otherwise receive messages
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            RemotePosition = (Vector3)stream.ReceiveNext();
            RemoteRotation = (Quaternion)stream.ReceiveNext();

            //Lag compensation
            _CurrentTime = 0.0f;
            _LastPacketTime = _CurrentPacketTime;
            _CurrentPacketTime = info.SentServerTime;
        }
    }

    /// <summary>
    /// Author: Ziqi Li
    /// Callbakc function of character controller's collider
    /// </summary>
    /// <param name="hit"></param>
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag == "BouncingPad" && controller.isGrounded)
        {
            isJumped = true;
        }
    }

    /// <summary>
    /// Author: Ziqi Li
    /// Callbakc function of input system
    /// </summary>
    void OnMove(InputValue movementValue)
    {
        MoveDirection = movementValue.Get<Vector2>();
    }

    /// <summary>
    /// Author: Ziqi Li
    /// Callbakc function of input system
    /// </summary>
    void OnLook(InputValue lookValue)
    {
        LookDirection = lookValue.Get<Vector2>();
    }

    /// <summary>
    /// Author: Ziqi Li
    /// Callbakc function of input system
    /// </summary>
    void OnJump()
    {
        if (controller.isGrounded) isJumped = true;
    }

    /// <summary>
    /// Author: Ziqi Li
    /// Callbakc function of input system
    /// </summary>
    void OnFire()
    {
        // call the RPC function to fire bullet if this player belong to the current client
        if (PhotonView.IsMine) PhotonView.RPC("RPC_Fire", RpcTarget.AllViaServer);
    }

    /// <summary>
    /// Author: Ziqi Li
    /// Function to fire a bullet (RPC function)
    /// </summary>
    [PunRPC]
    void RPC_Fire(PhotonMessageInfo info)
    {
        float lag = (float)(PhotonNetwork.Time - info.SentServerTime);

        GameObject bullet = Instantiate(BulletPrefab, ShootingPoint.transform.position, Quaternion.identity);
        bullet.GetComponent<TestBullet>().InitializeBullet(PhotonView.Owner, transform.forward, Mathf.Abs(lag));
    }


    /// <summary>
    /// Author: Ziqi Li
    /// Function for adding velocity to current additional velocity of this player (will be called by other objects
    /// or player ex: moving platform)
    /// </summary>
    /// <param name="velocity"></param>
    public void AddVelocity(Vector3 velocity)
    {
        _AdditionalVelocity += velocity;
    }

    /// <summary>
    /// Author: Ziqi Li
    /// Function for substract velocity from current additional velocity of this player (will be called by other objects
    /// or player ex: moving platform)
    /// </summary>
    /// <param name="velocity"></param>
    //public void SubstractVelocity(Vector3 velocity)
    //{
    //    _AdditionalVelocity -= velocity;
    //}

}
