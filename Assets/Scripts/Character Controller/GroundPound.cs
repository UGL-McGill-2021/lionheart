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
        [SerializeField] MultiplayerActivator PlayerMultiplayer;

        [Header("State")]
        [SerializeField] public bool BlockInput;
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
            BlockInput = false;

            Type = MovementModifier.MovementType.GroundPound;
        }

        /// <summary>
        /// Author: Denis
        /// Caching components
        /// </summary>
        private void Start()
        {
            PlayerJump = gameObject.GetComponent<Jump>();
            PlayerMultiplayer = gameObject.GetComponent<MultiplayerActivator>();
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
            if (IsGroundPound == false && PlayerJump.IsGrounded == false && BlockInput == false) 
            {
                IsGroundPound = true;

                CombatManager.SetInvincible(true);

                AnimatorController.SetBool("IsGroundPound", true);
                StartCoroutine(AnimationTrigger("IsGroundPound"));

                StartCoroutine(GroundPoundExecution());
            }
        }

        /// <summary>
        /// Author: Denis
        /// Ground Pound execution
        /// </summary>
        /// <returns></returns>
        IEnumerator GroundPoundExecution()
        {
            if (PlayerMultiplayer.hasVibration == true)
            {
                Gamepad.current.SetMotorSpeeds(0.5f, 0.7f);
            }

            /*yield return new WaitWhile(() => !PlayerJump.WithinSmashDistance);
            AnimatorController.SetBool("IsSmashing", true);
            StartCoroutine(AnimationTrigger("IsSmashing"));*/

            yield return new WaitWhile(() => !PlayerJump.IsGrounded);

            AnimatorController.SetBool("IsSmashing", true);
            StartCoroutine(AnimationTrigger("IsSmashing"));

            CombatManager.SetInvincible(false);
            IsGroundPound = false;

            if (CombatManager != null) {
                CombatManager.Smash();
            }

            if (PlayerMultiplayer.hasVibration == true)
            {
                Gamepad.current.SetMotorSpeeds(1f, 0.2f);
            }

            yield return new WaitForSecondsRealtime(0.2f);
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

        /// <summary>
        /// Author: Denis
        /// Simulates animation trigger for bools
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
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

