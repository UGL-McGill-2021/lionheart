using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;
using AssemblyCSharp.Assets;

/// <summary>
/// Ahutor: Feiyang Li, Ziqi Li
/// Created for Prototype Testing Purposes
/// </summary>
public class PrototypeCharacterMovementControls : MonoBehaviour{
    public InputAction MoveAction = new InputAction("move", binding: "<Gamepad>/leftStick");
    public InputAction LookAction = new InputAction("look", binding: "<Gamepad>/rightStick");

    public CharacterController controller;
    public float speed = 20f;
    public float gravity = -3f; 

    private Vector2 MoveDirection;
    private Vector2 LookDirection;
    private PhotonView PhotonView;

    private Vector3 RealPosition;
    private Quaternion RealRotation;

    private float jumpSpeed = 6f;
    private float speedY = 0f;  // speed on Y axis
    private bool isJumped = false;


    private void Awake()
    {
        MoveAction.Enable();
        LookAction.Enable();
        PhotonView = GetComponent<PhotonView>();

        DualShock4GamepadHID controller = new AssemblyCSharp.Assets.DualShock4GamepadHID();
    }

    private void Update()
    {

        // If this character belong to the current client
        if (PhotonView.IsMine)
        {
            var MoveDirection = MoveAction.ReadValue<Vector2>();
            var LookDirection = LookAction.ReadValue<Vector2>();

            // detect jump action
            if (controller.isGrounded)
            {
                if (isJumped)
                {
                    //set the y-movement to 0, and then add the y-movement with the jump movement value 
                    speedY = 0f;
                    speedY += Mathf.Sqrt(jumpSpeed * -gravity);
                    isJumped = false;
                }
            }
            else  //if not grounded, apply gravity
            {
                speedY += gravity * Time.deltaTime;
                isJumped = false;
            }

            // calculate moving vector
            Vector3 movement = transform.forward * MoveDirection.y;
            movement += transform.right * MoveDirection.x;
            movement.y = speedY;
            controller.Move(movement * Time.deltaTime * speed);

            // rotate character
            gameObject.transform.Rotate(new Vector3(0, LookDirection.x * 80 * Time.deltaTime, 0));
        }
        else
        {
            // update the position and rotation of this character (which doesn't belong to the current client)
            transform.position = Vector3.Lerp(transform.position, RealPosition, 0.1f);
            transform.rotation = Quaternion.Lerp(transform.rotation, RealRotation, 0.1f);
        }
    }

    /// <summary>
    /// Author: Ziqi Li
    /// Called by PUN several times per second, so that your script can write and
    /// read synchronization data for the PhotonView
    /// This method will be called in scripts that are assigned as Observed component of a PhotonView
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="info"></param>
    private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // Sending messages to server if this character belong to the current client, otherwise receive messages
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            RealPosition = (Vector3)stream.ReceiveNext();
            RealRotation = (Quaternion)stream.ReceiveNext();
        }
    }


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
    void OnJump()
    {
        if (controller.isGrounded) isJumped = true;
    }





}
