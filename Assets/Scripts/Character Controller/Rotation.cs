using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace Lionheart.Player.Movement
{
    /// <summary>
    /// Author: Denis
    /// This script rotates the player depending on the left joystick value
    /// </summary>
    public class Rotation : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] CharacterController PlayerController;
        [SerializeField] MovementHandler PlayerMovementHandler;
        [SerializeField] ControllerInput ControllerActions;

        private Vector3 Direction { get; set; }

        public Vector3 Value { get; private set; }

        // Photon:
        public PhotonView PhotonView;
        private bool isOffLineMode;
        private Quaternion RemoteRotation;

        /// <summary>
        /// Author: Denis
        /// Initial setup
        /// </summary>
        private void Awake()
        {
            ControllerActions = new ControllerInput();
            Direction = new Vector3();
        }

        private void Start()
        {
            PhotonView = GetComponent<PhotonView>();
            isOffLineMode = GetComponent<MultiplayerActivator>().isOffLine;
        }

        /// <summary>
        /// Author: Denis
        /// Subscribing to the controller events and adding this modifier to the movement modifiers list
        /// </summary>
        private void OnEnable()
        {
            ControllerActions.Player.Move.performed += Ctx => Value = new Vector3(Ctx.ReadValue<Vector2>().x, 0f, Ctx.ReadValue<Vector2>().y);
            ControllerActions.Player.Move.canceled += Ctx => Value = Vector2.zero;
            ControllerActions.Player.Move.Enable();
        }

        /// <summary>
        /// Author: Denis
        /// Unsubscribing to the controller events and removing this modifier from the movement modifiers list
        /// </summary>
        private void OnDisable()
        {
            ControllerActions.Player.Move.performed -= Ctx => Value = Vector2.zero;
            ControllerActions.Player.Move.canceled -= Ctx => Value = Vector2.zero;
            ControllerActions.Player.Move.Disable();
        }

        private void Update()
        {
            // If online mode
            if (!isOffLineMode)
            {
                if (PhotonView.IsMine)
                {
                    Rotate();
                }
                else
                {
                    Debug.Log("rec");
                    transform.rotation = Quaternion.Lerp(transform.rotation, RemoteRotation, Time.deltaTime);
                }
            }
            else
            {
                Rotate();

            }
        }

        /// <summary>
        /// Author: Denis
        /// Applies the joystick rotation to the player
        /// </summary>
        private void Rotate()
        {
            if (Value.magnitude > 0.2f)
            {
                Direction = Value.normalized;
            }

            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Direction), 15 * Time.deltaTime);
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
                stream.SendNext(transform.rotation);
            }
            else
            {
                RemoteRotation = (Quaternion)stream.ReceiveNext();
            }
        }
    }
}

