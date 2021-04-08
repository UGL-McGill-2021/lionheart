using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Lionheart.Player.Movement {
    /// <summary>
    /// Author: Denis
    /// This script rotates the player depending on the left joystick value
    /// </summary>
    public class Rotation : MonoBehaviour {
        [Header("References")]
        [SerializeField] MovementHandler PlayerMovementHandler;
        [SerializeField] ControllerInput ControllerActions;

        [Header("State")]
        public bool BlockInput = false;

        //The following is a polling approach
        public InputAction MoveAction = new InputAction("move", binding: "<Gamepad>/leftStick");
        public Vector2 MoveDirection = new Vector2();

        // Photon:
        public PhotonView PhotonView;

        /// <summary>
        /// Author: Ziqi
        /// Caching components
        /// </summary>
        private void Start()
        {
            PhotonView = GetComponent<PhotonView>();
        }

        /// <summary>
        /// Author: Denis
        /// Subscribing to the controller events and adding this modifier to the movement modifiers list
        /// </summary>
        private void OnEnable() {
            MoveAction.Enable();
        }

        /// <summary>
        /// Author: Denis
        /// Unsubscribing to the controller events and removing this modifier from the movement modifiers list
        /// </summary>
        private void OnDisable() {
            MoveAction.Disable();
        }

        private void FixedUpdate()
        {   
            if (BlockInput == false)
            {
                Rotate();
            }
        }

        /// <summary>
        /// Author: Denis
        /// Applies the joystick rotation to the player
        /// </summary>
        private void Rotate() {
            if (PhotonView.IsMine)
            {
                var MoveDirection = MoveAction.ReadValue<Vector2>();

                if (MoveDirection != Vector2.zero)
                {
                    transform.rotation = Quaternion.Lerp(transform.rotation, 
                        Quaternion.LookRotation(new Vector3(MoveDirection.x, 0f, MoveDirection.y)), 
                        15 * Time.deltaTime);
                }
            }
        }
    }
}

