using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Lionheart.Player.Movement
{
    /// <summary>
    /// Author: Denis
    /// This class handles gravity and jumps.
    /// </summary>
    public class Vertical : MonoBehaviour, MovementModifier
    {
        [Header("References")]
        [SerializeField] CharacterController PlayerController;
        [SerializeField] MovementHandler PlayerMovementHandler;
        [SerializeField] ControllerInput ControllerActions;
        [SerializeField] GameObject GroundCheck;
        [SerializeField] Gamepad Controller;

        [Header("Parameters")]
        [SerializeField] private readonly float GroundPull = 2f;
        [SerializeField] private readonly float JumpHeight = 25f;
        [SerializeField] private float GroundDistance = 0.4f;
        [SerializeField] private LayerMask GroundMask;

        private readonly float GravityMagnitude = Physics.gravity.y;
        private bool IsAirborne;
        private bool IsGrounded;
        private bool HasJumped;
        private bool JumpedLastFrame;

        public Vector3 Value { get; private set; }
        public MovementModifier.MovementType Type { get; private set; }

        /// <summary>
        /// Author: Denis
        /// Initial setup
        /// </summary>
        private void Awake()
        {
            ControllerActions = new ControllerInput();
            IsAirborne = false;
            IsGrounded = true;
            HasJumped = false;
            JumpedLastFrame = false;
            Type = MovementModifier.MovementType.Vertical;
        }

        /// <summary>
        /// Author: Denis
        /// Subscribing to the controller events and adding this modifier to the movement modifiers list
        /// </summary>
        private void OnEnable()
        {
            ControllerActions.Player.Jump.performed += RegisterJump;
            ControllerActions.Player.Jump.Enable();

            PlayerMovementHandler.AddModifier(this);
        }

        /// <summary>
        /// Author: Denis
        /// Unsubscribing to the controller events and removing this modifier from the movement modifiers list
        /// </summary>
        private void OnDisable()
        {
            ControllerActions.Player.Jump.performed -= RegisterJump;
            ControllerActions.Player.Jump.Disable();

            PlayerMovementHandler.RemoveModifier(this);
        }

        /// <summary>
        /// Author: Denis
        /// Processes the A(XB) button press and executes the jump
        /// TODO: Variable height jumps depending on press time
        /// </summary>
        /// <param name="Ctx"></param>
        private void RegisterJump(InputAction.CallbackContext Ctx)
        {
            if (IsAirborne == false)
            {
                Value = new Vector3(0f, Mathf.Sqrt(JumpHeight * -2 * GravityMagnitude), 0f);
                IsAirborne = true;
                HasJumped = true;
                JumpedLastFrame = true;
            }
        }

        private void Update() => VerticalForces();

        /// <summary>
        /// Author: Denis
        /// Solves the jump and gravity vectors to produce a final y axis vector.
        /// Also checks for ground collision.
        /// </summary>
        private void VerticalForces()
        {
            Vector3 Vec;
            IsGrounded = Physics.CheckSphere(GroundCheck.transform.position, GroundDistance, GroundMask);

            if (IsGrounded == false)
            {
                Vec = new Vector3(0f, 3f * GravityMagnitude * Time.deltaTime, 0f);
                //IsAirborne = true;
            }
            else
            {
                Vec = Vector3.zero;
            }

            if (IsGrounded == true && JumpedLastFrame == false)
            {
                
                Value = Vector3.zero;
                Vec = Vector3.zero;

                if (IsAirborne == true)
                {
                    StartCoroutine(PlayHaptics());
                    IsAirborne = false;
                }
            }

            if (JumpedLastFrame == true)
            {
                JumpedLastFrame = false;
            }

            Value += Vec;
        }
        
        /// <summary>
        /// Author: Denis
        /// Simple Rumble feedback on landing
        /// TODO: rumble doesn't trigger if fell off an edge
        /// </summary>
        /// <returns></returns>
        IEnumerator PlayHaptics()
        {
            Gamepad.current.SetMotorSpeeds(0.05f, 0f);
            yield return new WaitForSecondsRealtime(0.1f);
            Gamepad.current.ResetHaptics();
        }
    }
}

