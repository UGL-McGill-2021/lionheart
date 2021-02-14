using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Lionheart.Player.Movement
{
    public class Vertical : MonoBehaviour, MovementModifier
    {
        [Header("References")]
        [SerializeField] CharacterController PlayerController;
        [SerializeField] MovementHandler PlayerMovementHandler;
        [SerializeField] ControllerInput ControllerActions;
        [SerializeField] GameObject GroundCheck;
        [SerializeField] Gamepad Controller;

        [Header("Parameters")]
        [SerializeField] private readonly float GroundPull = 50f;
        [SerializeField] private readonly float JumpHeight = 10f;
        [SerializeField] private float GroundDistance = 0.4f;
        [SerializeField] private LayerMask GroundMask;

        private readonly float GravityMagnitude = Physics.gravity.y;

        private bool IsAirborne;
        private bool IsGrounded;
        private bool HasJumped;
        private bool JumpedLastFrame;

        public Vector3 Value { get; private set; }
        public MovementModifier.MovementType Type { get; private set; }

        private void Awake()
        {
            ControllerActions = new ControllerInput();
            IsAirborne = false;
            IsGrounded = true;
            HasJumped = false;
            JumpedLastFrame = false;
            Type = MovementModifier.MovementType.Vertical;
        }

        private void OnEnable()
        {
            ControllerActions.Player.Jump.performed += RegisterJump;
            ControllerActions.Player.Jump.Enable();

            PlayerMovementHandler.AddModifier(this);
        }

        private void OnDisable()
        {
            ControllerActions.Player.Jump.performed -= RegisterJump;
            ControllerActions.Player.Jump.Disable();

            PlayerMovementHandler.RemoveModifier(this);
        }

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

        private void VerticalForces()
        {
            Vector3 Vec;
            IsGrounded = Physics.CheckSphere(GroundCheck.transform.position, GroundDistance, GroundMask);
            //Debug.Log(IsGrounded);

            if (IsGrounded == false)
            {
                Vec = new Vector3(0f, 3f * GravityMagnitude * Time.deltaTime, 0f);
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
    
        IEnumerator PlayHaptics()
        {
            Gamepad.current.SetMotorSpeeds(0.05f, 0f);
            yield return new WaitForSecondsRealtime(0.1f);
            Gamepad.current.SetMotorSpeeds(0f, 0f);
        }
    }
}

