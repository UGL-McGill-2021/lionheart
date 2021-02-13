using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Lionheart.Player.Movement
{
    public class Gravity : MonoBehaviour, MovementModifier
    {
        [Header("References")]
        [SerializeField] CharacterController PlayerController;
        [SerializeField] MovementHandler PlayerMovementHandler;
        [SerializeField] InputActions PlayerActions;

        [Header("Parameters")]
        [SerializeField] private readonly float GroundPull = 50f;
        [SerializeField] private readonly float JumpHeight = 10f;

        private readonly float GravityMagnitude = Physics.gravity.y;

        private bool IsAirborn;
        private bool IsGrounded;

        public Vector3 Value { get; private set; }

        private void Awake()
        {
            PlayerActions = new InputActions();
            IsAirborn = false;
            IsGrounded = true;
        }

        private void OnEnable()
        {
            PlayerActions.Player.Jump.performed += RegisterJump;
            PlayerActions.Player.Jump.Enable();

            PlayerMovementHandler.AddModifier(this);
        }

        private void OnDisable()
        {
            PlayerActions.Player.Jump.performed -= RegisterJump;
            PlayerActions.Player.Jump.Disable();

            PlayerMovementHandler.RemoveModifier(this);
        }

        private void RegisterJump(InputAction.CallbackContext Ctx)
        {
            if (IsAirborn == false)
            {
                Value = new Vector3(0f, Mathf.Sqrt(JumpHeight * -2 * GravityMagnitude), 0f);
                IsAirborn = true;
            }
        }

        private void Update() => VerticalForces();

        private void VerticalForces()
        {
            Vector3 Vec;
            if (IsAirborn==true || PlayerController.isGrounded == false)
            {
                Vec = new Vector3(0f, 3f * GravityMagnitude * Time.deltaTime, 0f);
            }
            else
            {
                Vec = Vector3.zero;
            }
            if (PlayerController.isGrounded == true)
            {
                IsAirborn = false;
                Value = Vector3.zero;
                Vec = Vector3.zero;
            }

            Value += Vec;

            //Debug.Log("Vertical Vector " + Value.y);
        }
    }
}

