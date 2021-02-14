using System.Collections;
using System.Collections.Generic;
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

        /// <summary>
        /// Author: Denis
        /// Initial setup
        /// </summary>
        private void Awake()
        {
            ControllerActions = new ControllerInput();
            Direction = new Vector3();
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

        private void Update() => Rotate();

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
    }
}

