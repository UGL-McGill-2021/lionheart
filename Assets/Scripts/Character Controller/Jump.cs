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
        [SerializeField] private readonly float JumpPower = 15f;
        [SerializeField] private readonly float GroundPull = 2f;
        [SerializeField] private float GroundDistance = 0.4f;
        [SerializeField] private LayerMask GroundMask;

        private float DistanceToGround;
        private readonly float GravityMagnitude = Physics.gravity.y;
        private bool IsGrounded;
        private bool HasJumped;
        private int JumpedFrameCounter = 10;

        public Vector3 Value { get; private set; }
        public MovementModifier.MovementType Type { get; private set; }

        void Start()
        {
            DistanceToGround = GetComponent<Collider>().bounds.extents.y;
        }

        /// <summary>
        /// Author: Denis
        /// Initial setup
        /// </summary>
        private void Awake()
        {
            ControllerActions = new ControllerInput();
            IsGrounded = true;
            HasJumped = false;
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
        /// Processes the A(XB) button press and executes the jump
        /// TODO: Variable height jumps depending on press time
        /// </summary>
        /// <param name="Ctx"></param>
        private void RegisterJump(InputAction.CallbackContext Ctx)
        {
            if (IsGrounded == true && HasJumped == false)
            {
                Value = new Vector3(0f, Mathf.Sqrt(JumpPower * -2 * GravityMagnitude), 0f);
                HasJumped = true;
                JumpedFrameCounter = 10;
                //Debug.Log("Pressed Jump at "+Time.time);
            }
            else if (HasJumped == true)
            {
                //Debug.Log("Pressing at " + Time.time);
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
            Vector3 Vec=Vector3.zero;
            CheckIfGrounded();

            if (IsGrounded == false && JumpedFrameCounter==0)
            {
                Vec = new Vector3(0f, 3f * GravityMagnitude * Time.deltaTime, 0f);
            }

            if (IsGrounded == true && JumpedFrameCounter==0)
            {
                Value = Vector3.zero;
                if (HasJumped == true)
                {
                    StartCoroutine(PlayHaptics());
                    HasJumped = false;
                }
            }
            if (JumpedFrameCounter > 0)
            {
                JumpedFrameCounter--;
            }
            Value += Vec;
        }

        private void CheckIfGrounded()
        {
            //IsGrounded = Physics.CheckSphere(GroundCheck.transform.position, GroundDistance, GroundMask);
            if (!Physics.Raycast(transform.position, -Vector3.up, DistanceToGround + 0.1f))
            {
                IsGrounded = false;
            }
            else
            {
                IsGrounded = true;
            }
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

