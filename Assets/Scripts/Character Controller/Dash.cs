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
        [SerializeField] MovementHandler PlayerMovementHandler;
        [SerializeField] PullDash PlayerPullDash;
        [SerializeField] Jump PlayerJump;
        [SerializeField] Knockback PlayerKnockback;
        [SerializeField] PlayerCombatManager PlayerCombat;
        [SerializeField] ControllerInput ControllerActions;
        [SerializeField] Animator AnimatorController;
        [SerializeField] PlayerCombatManager CombatManager;
        [SerializeField] MultiplayerActivator PlayerMultiplayer;
        [SerializeField] Vector3 Direction;

        // Feiyang: Player SFX integration
        public PlayerAudioController AudioController;

        [Header("Parameters")]
        [SerializeField] private float DashForce = 10f;
        [SerializeField] private float DashExecutionTime = 0.2f;
        [SerializeField] private float DashCooldownTime = 0.3f;
        [SerializeField] private float KnockbackForce = 1000f;
        [SerializeField] private int KnockbackTime = 1;

        [Header("State")]
        public bool IsDashing;
        public bool IsAirDashing;

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
        /// Caching components
        /// </summary>
        private void Start()
        {
            PlayerPullDash = gameObject.GetComponent<PullDash>();
            PlayerJump = gameObject.GetComponent<Jump>();
            PlayerCombat = gameObject.GetComponent<PlayerCombatManager>(); 
            PlayerKnockback = gameObject.GetComponent<Knockback>();
            PlayerMultiplayer = gameObject.GetComponent<MultiplayerActivator>();
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
        /// Author: Denis, Feiyang
        /// Processes the X(XB) button press and starts the dash execution, cooldown timers and combat action
        /// </summary>
        /// <param name="Ctx"></param>
        private void RegisterDash(InputAction.CallbackContext Ctx)
        {
            if (DashOnCooldown == false && PlayerPullDash.DisableGravity == false
                 && PlayerKnockback.IsKnockback == false)
            {
                IsDashing = true;
                DashOnCooldown = true;
                StartCoroutine(DashExecution());

                AnimatorStateInfo St = AnimatorController.GetCurrentAnimatorStateInfo(0);
                if (St.IsName("Jump") == true || St.IsName("Airborne") == true) 
                {
                    IsAirDashing = true;
                }

                PlayerCombat.Attack(new Kick(KnockbackForce, KnockbackTime));

                if (AudioController != null)
                    AudioController.TriggerPlaySFXOnAll((int)PlayerSFX.DASH);
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
            yield return new WaitWhile(() => !PlayerJump.IsGrounded);
            DashOnCooldown = false;
        }

        /// <summary>
        /// Author: Denis, Ziqi
        /// Dash execution timer (how long the dash takes)
        /// Also triggers rumble.
        /// </summary>
        /// <returns></returns>
        IEnumerator DashExecution()
        {
            AnimatorController.SetBool("IsDashing", true);
            CombatManager.SetInvincible(true);

            if (PlayerMultiplayer.hasVibration == true)
            {
                Gamepad.current.SetMotorSpeeds(0f, 0.5f);
            }

            yield return new WaitForSecondsRealtime(DashExecutionTime);

            IsDashing = false;
            CombatManager.SetInvincible(false);

            PlayerCombat.StopAttack();

            if (IsAirDashing == true)
            {
                AnimatorController.SetBool("IsAirDashing", true);
                IsAirDashing = false;
            }

            AnimatorController.SetBool("IsDashing", false);
            Gamepad.current.ResetHaptics();

            StartCoroutine(DashCooldown());
        }

        private void FixedUpdate() => DashMove();

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

