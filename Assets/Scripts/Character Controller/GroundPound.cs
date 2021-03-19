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

        [Header("State")]
        [SerializeField] public bool IsGroundPound;

        [Header("Parameters")]
        [SerializeField] private float FreezeTimer = 0.2f;
        [SerializeField] private float GroundPoundForce = 40f;

        /// <summary>
        /// Author: Feiyang
        /// Combat integration
        /// </summary>
        [Header("Combat")]
        public PlayerCombatManager CombatManager;

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
            if (IsGroundPound == false && gameObject.GetComponent<Jump>().IsGrounded == false) 
            {
                IsGroundPound = true;
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
            Gamepad.current.SetMotorSpeeds(0.6f, 0.1f);
            yield return new WaitWhile(() => !gameObject.GetComponent<Jump>().IsGrounded);
            IsGroundPound = false;

            if (CombatManager != null) {
                CombatManager.Smash();
            }

            Gamepad.current.SetMotorSpeeds(0.1f, 0.5f);
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

    }
}

