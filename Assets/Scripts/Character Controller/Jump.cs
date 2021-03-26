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
        [SerializeField] PullDash PlayerPullDash;
        [SerializeField] GroundPound PlayerGroundPound;
        [SerializeField] ControllerInput ControllerActions;
        [SerializeField] Animator AnimatorController;
        [SerializeField] GameObject GroundCheck;
        [SerializeField] Gamepad Controller;
        [SerializeField] Rigidbody Rb;

        [Header("Parameters")]
        [SerializeField] private float JumpPower = 12f;
        [SerializeField] private float CounterJumpForce = 0.75f;
        [SerializeField] private float GroundDistance = 0.6f;
        [SerializeField] private float CoyoteHopTimer = 1f;
        [SerializeField] private float FallTimer = 2f;
        [SerializeField] private float LandingAnimTriggerDistance = 0.5f;
        [SerializeField] private LayerMask GroundMask;

        [Header("State")]
        public bool IsGrounded;
        public bool IsFalling;
        public bool PlayedLandingAnim;

        private float GravityForce = Physics.gravity.y;
        private bool HasJumped;
        private bool CanCoyoteHop;
        private bool WasGroundedLastFrame;
        private Vector3 Vec;
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
            PlayedLandingAnim = false;

            Type = MovementModifier.MovementType.Jump;
        }

        /// <summary>
        /// Author: Denis
        /// Caching components
        /// </summary>
        private void Start()
        {
            PlayerPullDash = gameObject.GetComponent<PullDash>();
            PlayerGroundPound = gameObject.GetComponent<GroundPound>();
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

                AnimatorController.SetBool("IsJumping", true);
                StartCoroutine(AnimationTrigger("IsJumping"));
                PlayedLandingAnim = false;
            }
        }

        /// <summary>
        /// Author: Denis
        /// Jump vectors and animation logic
        /// </summary>
        private void FixedUpdate()
        {
            Vec = Vector3.zero;

            CheckIfGrounded();

            EarlyPlayAnimations();

            VerticalForces();

            if (PlayedLandingAnim == false)
            {
                LatePlayAnimations();
            }

            WasGroundedLastFrame = IsGrounded;
        }

        /// <summary>
        /// Author: Denis
        /// Manages the fall animation and a counter airborne softlock state landing animation 
        /// </summary>
        private void EarlyPlayAnimations()
        {
            if (IsGrounded == false && HasJumped == false && PlayerPullDash.IsPullDashing == false &&
                PlayerGroundPound.IsGroundPound == false && Value.y < -8f)
            {
                AnimatorController.SetBool("IsFalling", true);
                StartCoroutine(AnimationTrigger("IsFalling"));
                IsFalling = true;
                PlayedLandingAnim = false;
            }

            if (IsGrounded == true)
            {
                AnimatorStateInfo St = AnimatorController.GetCurrentAnimatorStateInfo(0);
                if (St.IsName("Airborne"))
                {
                    AnimatorController.SetBool("IsLanding", true);
                    StartCoroutine(AnimationTrigger("IsLanding"));
                    PlayedLandingAnim = true;
                }
                
                if (AnimatorController.GetBool("IsAirDashing") == true)
                {
                    AnimatorController.SetBool("IsAirDashing", false);
                }
            }
        }

        /// <summary>
        /// Author: Denis
        /// Solves the jump, jump counterforce and gravity vectors to produce a final y axis vector. Also triggers rumble
        /// </summary>
        private void VerticalForces()
        {
            //coyote time for the jump
            if (IsGrounded == false && WasGroundedLastFrame == true)
            {
                CanCoyoteHop = true;
                StartCoroutine(CoyoteHopTimeWindow());
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
            if (PlayerPullDash.DisableGravity == true)
            {
                Vec = Vector3.zero;
            }
            else if (IsGrounded == false && JumpedFrameCounter == 0)
            {
                Vec = new Vector3(0f, 3f * GravityForce * Time.deltaTime, 0f);
            }

            //checks landing after a jump or fall
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
            //reduces jump collision ignore time
            if (JumpedFrameCounter > 0)
            {
                JumpedFrameCounter--;
            }

            Value += Vec + Vec2;
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
        /// Play Landing animation when about to hit the ground
        /// TODO Integrate with photon
        /// </summary>
        private void LatePlayAnimations()
        {
            if (Physics.CheckSphere(GroundCheck.transform.position, LandingAnimTriggerDistance, GroundMask) &&
                HasJumped == true)
            {
                if (Value.y < 0f)
                {
                    AnimatorController.SetBool("IsLanding", true);
                    StartCoroutine(AnimationTrigger("IsLanding"));
                    PlayedLandingAnim = true;
                }
                else
                {
                    AnimatorStateInfo st = AnimatorController.GetCurrentAnimatorStateInfo(0);
                    if (st.IsName("Airborne"))
                    {
                        AnimatorController.SetBool("IsLanding", true);
                        StartCoroutine(AnimationTrigger("IsLanding"));
                        PlayedLandingAnim = true;
                    }
                }
            }
        }

        /// <summary>
        /// Author: Denis, Ziqi
        /// Simple Rumble feedback on landing
        /// TODO: PS4 vs XB rumble
        /// </summary>
        /// <returns></returns>
        IEnumerator PlayHaptics()
        {
            if(Gamepad.current.name == "DualShock4GamepadHID") Gamepad.current.SetMotorSpeeds(0.85f, 0.85f);
            else if(Gamepad.current.name == "PS4Controller") Gamepad.current.SetMotorSpeeds(0.85f, 0.85f);
            else Gamepad.current.SetMotorSpeeds(0.05f, 0f);
            yield return new WaitForSecondsRealtime(0.1f);
            Gamepad.current.ResetHaptics();
        }

        IEnumerator AnimationTrigger(string Name)
        {
            yield return new WaitForSecondsRealtime(0.5f);

            switch (Name)
            {
                case "IsJumping":
                    AnimatorController.SetBool("IsJumping", false);
                    break;
                case "IsFalling":
                    AnimatorController.SetBool("IsFalling", false);
                    break;
                case "IsLanding":
                    AnimatorController.SetBool("IsLanding", false);
                    break;
            }
        }
    }
}



