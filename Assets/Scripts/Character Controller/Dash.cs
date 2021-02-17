using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Lionheart.Player.Movement
{
    /// <summary>
    /// Author: Denis
    /// Dash movement modifier. Dash only acts on the z and x axes
    /// </summary>
    public class Dash : MonoBehaviour, MovementModifier
    {
        [Header("References")]
        [SerializeField] CharacterController PlayerController;
        [SerializeField] MovementHandler PlayerMovementHandler;
        [SerializeField] ControllerInput ControllerActions;
        [SerializeField] Vector3 Direction;

        [Header("Parameters")]
        [SerializeField] private float DashForce = 10f;
        [SerializeField] private float DashExecutionTime = 0.2f;
        [SerializeField] private float DashCooldownTime = 0.3f;

        private bool IsDashing;
        private bool DashOnCooldown;

        public Vector3 Value { get; private set; }
        public MovementModifier.MovementType Type { get; private set; }

        /// <summary>
        /// Author: Denis
        /// Initial setup
        /// </summary>
        private void Awake()
        {
            ControllerActions = new ControllerInput();
            IsDashing = false;
            DashOnCooldown = false;

            Type = MovementModifier.MovementType.Dash;
        }

        /// <summary>
        /// Author: Denis
        /// Subscribing to the controller events and adding this modifier to the movement modifiers list
        /// </summary>
        private void OnEnable()
        {
            ControllerActions.Player.Dash.performed += RegisterDash;
            ControllerActions.Player.Dash.Enable();

            PlayerMovementHandler.AddModifier(this);
        }

        /// <summary>
        /// Author: Denis
        /// Unsubscribing to the controller events and removing this modifier from the movement modifiers list
        /// </summary>
        private void OnDisable()
        {
            ControllerActions.Player.Dash.performed -= RegisterDash;
            ControllerActions.Player.Dash.Disable();

            PlayerMovementHandler.RemoveModifier(this);
        }

        /// <summary>
        /// Author: Denis
        /// Processes the X(XB) button press and starts the dash execution and cooldown timers
        /// </summary>
        /// <param name="Ctx"></param>
        private void RegisterDash(InputAction.CallbackContext Ctx)
        {
            if (DashOnCooldown == false)
            {
                IsDashing = true;
                DashOnCooldown = true;
                StartCoroutine(DashCooldown());
                StartCoroutine(DashExecution());
            }
        }

        /// <summary>
        /// Author: Denis
        /// Dash cooldown timer (how long it takes to dash again)
        /// </summary>
        /// <returns></returns>
        IEnumerator DashCooldown()
        {
            yield return new WaitForSecondsRealtime(DashCooldownTime);
            DashOnCooldown = false;
        }

        /// <summary>
        /// Author: Denis
        /// Dash execution timer (how long the dash takes)
        /// Also triggers rumble.
        /// </summary>
        /// <returns></returns>
        IEnumerator DashExecution()
        {
            Gamepad.current.SetMotorSpeeds(0f, 0.3f);
            yield return new WaitForSecondsRealtime(DashExecutionTime);
            IsDashing = false;
            Gamepad.current.ResetHaptics();
        }

        private void Update() => DashMove();

        /// <summary>
        /// Author: Denis
        /// Calculates a dash vector if dashing
        /// </summary>
        private void DashMove()
        {
            if (IsDashing == true)
            {
                Value = gameObject.transform.forward * DashForce;
            }
            else
            {
                Value = Vector3.zero;
            }
        }
    }
}
