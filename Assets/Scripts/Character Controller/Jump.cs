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
    public class Jump : MonoBehaviour, MovementModifier
    {
        [Header("References")]
        [SerializeField] MovementHandler PlayerMovementHandler;
        [SerializeField] ControllerInput ControllerActions;
        [SerializeField] GameObject GroundCheck;
        [SerializeField] Gamepad Controller;
        [SerializeField] Rigidbody Rb;

        [Header("Parameters")]
        [SerializeField] private float JumpPower = 12f;
        [SerializeField] private float CounterJumpForce = 0.75f;
        [SerializeField] private float GroundDistance = 0.6f;
        [SerializeField] private float CoyoteHopTimer = 1f;
        [SerializeField] private float FallTimer = 0.8f;
        [SerializeField] private LayerMask GroundMask;

        [Header("State")]
        public bool IsGrounded;
        public bool IsFalling;

        private float GravityForce = Physics.gravity.y;
        private bool HasJumped;
        private bool CanCoyoteHop;
        private bool WasGroundedLastFrame;
        private Vector3 Vec2 = Vector3.zero;

        //TODO: Replace by Coroutine is found to be unstable
        private int JumpedFrameCounter = 10;

        public Vector3 Value { get; private set; }
        public MovementModifier.MovementType Type { get; private set; }

        /// <summary>
        /// Author: Denis
        /// Initial setup
        /// </summary>
        private void Awake()
        {
            ControllerActions = new ControllerInput();
            IsGrounded = true;
            IsFalling = false;
            HasJumped = false;
            CanCoyoteHop = false;
            WasGroundedLastFrame = false;

            Type = MovementModifier.MovementType.Jump;
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
        /// Processes the A(XB)/X(PS4) button press and executes the jump
        /// </summary>
        /// <param name="Ctx"></param>
        private void RegisterJump(InputAction.CallbackContext Ctx)
        {
            if ((IsGrounded == true || CanCoyoteHop == true) && HasJumped == false)
            {
                Value = new Vector3(0f, Mathf.Sqrt(JumpPower * -2 * GravityForce), 0f);
                HasJumped = true;
                CanCoyoteHop = false;
                JumpedFrameCounter = 10;
            }
        }

        private void FixedUpdate() => VerticalForces();

        /// <summary>
        /// Author: Denis
        /// Solves the jump and gravity vectors to produce a final y axis vector.
        /// </summary>
        private void VerticalForces()
        {
            Vector3 Vec = Vector3.zero;
            CheckIfGrounded();

            if (IsGrounded == false && WasGroundedLastFrame == true)
            {
                CanCoyoteHop = true;
                StartCoroutine(CoyoteHopTimeWindow());
                StartCoroutine(MinFallTimeWindow());
            }

            //allows for the varying jump sizes
            if (IsGrounded == false && !Gamepad.current.buttonSouth.isPressed && Vector3.Dot(Value, Vector3.up) > 0)
            {
                Vec2 += new Vector3(0f, (-CounterJumpForce) * Time.deltaTime, 0f);
            }
            else
            {
                Vec2 = Vector3.zero;
            }

            //calculates gravity
            if (gameObject.GetComponent<PullDash>().DisableGravity == true)
            {
                Vec = Vector3.zero;
            }
            else if (IsGrounded == false && JumpedFrameCounter == 0)
            {
                Vec = new Vector3(0f, 3f * GravityForce * Time.deltaTime, 0f);
            }

            if (IsGrounded == true && JumpedFrameCounter == 0)
            {
                Value = Vector3.zero;
                Vec2 = Vector3.zero;
                GravityForce = Physics.gravity.y;
                if (HasJumped == true)
                {
                    StartCoroutine(PlayHaptics());
                    HasJumped = false;
                    IsFalling = false;
                }
                else if (IsFalling == true)
                {
                    StartCoroutine(PlayHaptics());
                    IsFalling = false;
                }
            }
            if (JumpedFrameCounter > 0)
            {
                JumpedFrameCounter--;
            }
            Value += Vec + Vec2;

            WasGroundedLastFrame = IsGrounded;
        }

        /// <summary>
        /// Author: Denis
        /// Time window after leaving the ground where the player can still jump if he hasn't already.
        /// </summary>
        /// <returns></returns>
        private IEnumerator CoyoteHopTimeWindow()
        {
            yield return new WaitForSecondsRealtime(CoyoteHopTimer);
            CanCoyoteHop = false;
        }

        private IEnumerator MinFallTimeWindow()
        {
            yield return new WaitForSecondsRealtime(FallTimer);
            IsFalling = true;
        }

        /// <summary>
        /// Author: Denis
        /// Detect collision with the ground
        /// </summary>
        private void CheckIfGrounded()
        {
            IsGrounded = Physics.CheckSphere(GroundCheck.transform.position, GroundDistance, GroundMask);
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



