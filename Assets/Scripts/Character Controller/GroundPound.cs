using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Lionheart.Player.Movement
{
    /// <summary>
    /// Author: Denis
    /// This class handles the ground pound movement. A move that propulses the 
    /// player down to the ground if in the air.
    /// TODO: Consider making Ground Pound force dependend on height OR damage dependent on height
    /// </summary>
    public class GroundPound : MonoBehaviour, MovementModifier
    {
        [Header("References")]
        [SerializeField] MovementHandler PlayerMovementHandler;
        [SerializeField] ControllerInput ControllerActions;
        [SerializeField] Animator AnimatorController;
        [SerializeField] PlayerCombatManager CombatManager;
        [SerializeField] Jump PlayerJump;

        [Header("State")]
        [SerializeField] public bool IsGroundPound;

        [Header("Parameters")]
        [SerializeField] private float FreezeTimer = 0.2f;
        [SerializeField] private float GroundPoundForce = 40f;

        public Vector3 Value { get; private set; }
        public MovementModifier.MovementType Type { get; private set; }

        /// <summary>
        /// Author: Denis
        /// Initial setup
        /// </summary>
        private void Awake()
        {
            ControllerActions = new ControllerInput();
            IsGroundPound = false;

            Type = MovementModifier.MovementType.GroundPound;
        }

        /// <summary>
        /// Author: Denis
        /// Caching components
        /// </summary>
        private void Start()
        {
            PlayerJump = gameObject.GetComponent<Jump>();
        }

        /// <summary>
        /// Author: Denis
        /// Subscribing to the controller events and adding this modifier to the movement modifiers list
        /// </summary>
        private void OnEnable()
        {
            ControllerActions.Player.GroundPound.performed += RegisterGroundPound;
            ControllerActions.Player.GroundPound.Enable();

            PlayerMovementHandler.AddModifier(this);
        }

        /// <summary>
        /// Author: Denis
        /// Unsubscribing to the controller events and removing this modifier from the movement modifiers list
        /// </summary>
        private void OnDisable()
        {
            ControllerActions.Player.GroundPound.performed -= RegisterGroundPound;
            ControllerActions.Player.GroundPound.Disable();

            PlayerMovementHandler.RemoveModifier(this);
        }

        /// <summary>
        /// Author: Denis
        /// Processes the RB(XB)/R1(PS4) button press and executes the ground pound
        /// </summary>
        /// <param name="Ctx"></param>
        private void RegisterGroundPound(InputAction.CallbackContext Ctx)
        {
            if (IsGroundPound == false && PlayerJump.IsGrounded == false) 
            {
                IsGroundPound = true;

                AnimatorController.SetBool("IsGroundPound", true);
                StartCoroutine(AnimationTrigger("IsGroundPound"));

                StartCoroutine(GroundPoundRumble());
            }
        }

        /// <summary>
        /// Author: Denis
        /// Ground Pound rumble
        /// </summary>
        /// <returns></returns>
        IEnumerator GroundPoundRumble()
        {
            if (Gamepad.current.name == "DualShock4GamepadHID") Gamepad.current.SetMotorSpeeds(6f, 1f);
            else if (Gamepad.current.name == "PS4Controller") Gamepad.current.SetMotorSpeeds(6f, 1f);
            else Gamepad.current.SetMotorSpeeds(0.6f, 0.1f);

            yield return new WaitWhile(() => !PlayerJump.WithinSmashDistance);
            AnimatorController.SetBool("IsSmashing", true);
            StartCoroutine(AnimationTrigger("IsSmashing"));

            yield return new WaitWhile(() => !PlayerJump.IsGrounded);
            IsGroundPound = false;

            if (CombatManager != null) {
                CombatManager.Smash();
            }

            if (Gamepad.current.name == "DualShock4GamepadHID") Gamepad.current.SetMotorSpeeds(1f, 5f);
            else if (Gamepad.current.name == "PS4Controller") Gamepad.current.SetMotorSpeeds(1f, 5f);
            else Gamepad.current.SetMotorSpeeds(0.1f, 0.5f);
            yield return new WaitForSecondsRealtime(0.1f);
            Gamepad.current.ResetHaptics();
        }

        private void FixedUpdate() => GroundPoundMove();

        /// <summary>
        /// Author: Denis
        /// Calculates a downwards vector to execute a ground pound
        /// </summary>
        private void GroundPoundMove()
        {
            if (IsGroundPound == true)
            {
                Value = new Vector3(0f, -1f, 0f) * GroundPoundForce;
            }
            else
            {
                Value = Vector3.zero;
            }
        }

        IEnumerator AnimationTrigger(string Name)
        {
            yield return new WaitForSecondsRealtime(0.5f);

            switch (Name)
            {
                case "IsGroundPound":
                    AnimatorController.SetBool("IsGroundPound", false);
                    break;
                case "IsSmashing":
                    AnimatorController.SetBool("IsSmashing", false);
                    break;
            }
        }
    }
}

