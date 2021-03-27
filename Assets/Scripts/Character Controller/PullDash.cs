using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using Photon.Pun;

namespace Lionheart.Player.Movement
{
    /// <summary>
    /// Author: Denis
    /// This class handles the co-op pull dash mechanic
    /// </summary>
    public class PullDash : MonoBehaviour, MovementModifier
    {
        [Header("References")]
        [SerializeField] MovementHandler PlayerMovementHandler;
        [SerializeField] ControllerInput ControllerActions;
        [SerializeField] Animator AnimatorController;
        [SerializeField] Vector3 Direction;
        [SerializeField] GameObject OwnPullDashTarget;
        [SerializeField] GameObject OtherPlayer;
        [SerializeField] GameObject OtherPlayerTarget;
        [SerializeField] PullDash OtherPlayerPullDashScript;
        [SerializeField] PlayerCombatManager CombatManager;
        [SerializeField] MultiplayerActivator PlayerMultiplayer;

        [Header("State")]
        [SerializeField] public bool ChargingPullDash;
        [SerializeField] public bool PullDashCharged;
        [SerializeField] public bool IsPullDashing;
        [SerializeField] public bool DisableGravity;

        [Header("Parameters")]
        [SerializeField] private float MaxTriggerDistance = 60f;
        [SerializeField] private float LaunchVectorMultiplier = 1f;
        [SerializeField] private float MinVectorMagnitude = 15f;
        [SerializeField] private float MaxVectorMagnitude = 40f;
        [SerializeField] private float CompletionDistance = 2f;
        [SerializeField] private float ExpiryTimer = 0.8f;
        [SerializeField] private float AirControlAngleRange = 60;
        [SerializeField] private float TriggerTime = 0.5f;

        private Vector3 T;
        private Vector3 OgDir;
        private Vector3 Dir;

        public Vector3 Value { get; private set; }
        public MovementModifier.MovementType Type { get; private set; }

        private PhotonView PhotonView;

        /// <summary>
        /// Author: Denis
        /// Initial setup
        /// </summary>
        private void Awake()
        {
            PhotonView = GetComponent<PhotonView>();
            ControllerActions = new ControllerInput();
            ChargingPullDash = false;
            PullDashCharged = false;
            IsPullDashing = false;
            DisableGravity = false;

            Type = MovementModifier.MovementType.PullDash;
        }

        /// <summary>
        /// Author: Denis
        /// Caching components
        /// </summary>
        private void Start()
        {
            PlayerMultiplayer = gameObject.GetComponent<MultiplayerActivator>();
        }

        /// <summary>
        /// Author: Denis
        /// Subscribing to the controller events and adding this modifier to the movement modifiers list
        /// </summary>
        private void OnEnable()
        {
            ControllerActions.Player.PullDash.performed += RegisterPullDash;
            ControllerActions.Player.PullDash.Enable();

            PlayerMovementHandler.AddModifier(this);
        }

        /// <summary>
        /// Author: Denis
        /// Unsubscribing to the controller events and removing this modifier from the movement modifiers list
        /// </summary>
        private void OnDisable()
        {
            ControllerActions.Player.PullDash.performed -= RegisterPullDash;
            ControllerActions.Player.PullDash.Disable();

            PlayerMovementHandler.RemoveModifier(this);
        }

        /// <summary>
        /// Author: Denis
        /// Processes the Y(XB)/Triangle(PS4) button press and executes the jump
        /// </summary>
        /// <param name="Ctx"></param>
        private void RegisterPullDash(InputAction.CallbackContext Ctx)
        {
            //get a reference to the other player if null
            if (OtherPlayerPullDashScript == null)
            {
                GameObject[] _Players = GameObject.FindGameObjectsWithTag("Player");
                for (int i = 0; i < _Players.Length; i++)
                {
                    if (_Players[i].Equals(gameObject) == false)
                    {
                        OtherPlayer = _Players[i];
                        OtherPlayerPullDashScript = _Players[i].GetComponent<PullDash>();
                        OtherPlayerTarget = OtherPlayerPullDashScript.OwnPullDashTarget;
                    }
                }
            }

            //the pull dash activation can only be validated if within distance
            Vector3 V = (OtherPlayer.transform.position - transform.position);
            
            if (ChargingPullDash == false && IsPullDashing == false 
                && OtherPlayerPullDashScript != null && V.magnitude <= MaxTriggerDistance)
            {
                ChargingPullDash = true;
                PullDashCharged = false;
                StartCoroutine(PullDashCharge());
            }
        }

        /// <summary>
        /// Author: Denis
        /// Enables the pull dash after a hold timer period.
        /// Triggers rumble if action activated
        /// </summary>
        /// <returns></returns>
        private IEnumerator PullDashCharge()
        {
            yield return new WaitForSecondsRealtime(TriggerTime);

            PullDashCharged = true;

            if (PlayerMultiplayer.hasVibration == true)
            {
                if (Gamepad.current.name == "DualShock4GamepadHID") Gamepad.current.SetMotorSpeeds(4f, 4f);
                else if (Gamepad.current.name == "PS4Controller") Gamepad.current.SetMotorSpeeds(4f, 4f);
                else Gamepad.current.SetMotorSpeeds(0.05f, 0.3f);
            }

            yield return new WaitForSecondsRealtime(0.2f);
            Gamepad.current.ResetHaptics();
        }

        /// <summary>
        /// Author: Denis
        /// Manages pull dash action logic
        /// </summary>
        private void FixedUpdate()
        {
            //pull dash is in pre-motion state
            if (ChargingPullDash == true)
            {
                Charger();
            }

            //if pull dash was successfully activated execute the motion
            if (IsPullDashing == true)
            {
                /*if (DisableGravity == false && Vector3.Angle(OgDir, transform.forward) <= AirControlAngleRange)
                {
                    float M = Value.magnitude;
                    Dir = transform.forward * M;
                }*/

                Vector3 V = Dir * LaunchVectorMultiplier;

                if (V.magnitude > MaxVectorMagnitude)
                {
                    Value = V.normalized * MaxVectorMagnitude;
                }
                else if (V.magnitude < MinVectorMagnitude)
                {
                    Value = V.normalized * MinVectorMagnitude;
                }
                else
                {
                    Value = V;
                }

                CheckDistance();
            }
            else
            {
                Value = Vector3.zero;
            }
        }

        /// <summary>
        /// Author: Denis
        /// Checks for button press, if the button is released pre-emptively the action is cancelled.
        /// </summary>
        private void Charger()
        {
            if (Gamepad.current.buttonNorth.isPressed == false)
            {
                Vector3 V = (OtherPlayer.transform.position - transform.position);
                if (PullDashCharged == false || V.magnitude > MaxTriggerDistance)
                {
                    StopAllCoroutines();
                    Gamepad.current.ResetHaptics();
                    ChargingPullDash = false;
                }
                else
                {
                    T = OtherPlayerTarget.transform.position;
                    Dir = (OtherPlayerTarget.transform.position - transform.position);
                    OgDir = Dir;

                    ChargingPullDash = false;
                    IsPullDashing = true;
                    DisableGravity = true;

                    AnimatorController.SetBool("IsPullDashing", true);
                    StartCoroutine(AnimationTrigger("IsPullDashing"));

                    CombatManager.SetInvincible(true);

                    //Time limit on the pull dash execution
                    StartCoroutine(PullDashTimer());
                }
            }
        }

        /// <summary>
        /// Author: Denis
        /// Pull dash first part of the motion ends when the player gets within range of the target.
        /// Gravity is enabled.
        /// </summary>
        private void CheckDistance()
        {
            if (Vector3.Distance(gameObject.transform.position, T)< CompletionDistance)
            {
                gameObject.GetComponent<Rotation>().EnablePullDashRotationSpeed();
                DisableGravity = false;

                StartCoroutine(PullDashFall());
            }
        }

        /// <summary>
        /// Author: Denis
        /// The second part of the pull dash motion is affected by gravity. 
        /// The move terminates on ground collision.
        /// </summary>
        /// <returns></returns>
        IEnumerator PullDashFall()
        {
            yield return new WaitWhile(() => !gameObject.GetComponent<Jump>().IsGrounded);
            gameObject.GetComponent<Rotation>().ResetRotationSpeed();
            IsPullDashing = false;
        }

        /// <summary>
        /// Author: Denis
        /// The first part of pull dash move has a timer upper bound after which it is interrupted
        /// if it is still executing no matter what. Gravity is enabled to ensure fall to graound and termination.
        /// </summary>
        /// <returns></returns>
        IEnumerator PullDashTimer()
        {
            yield return new WaitForSecondsRealtime(ExpiryTimer);
            gameObject.GetComponent<Rotation>().EnablePullDashRotationSpeed();
            DisableGravity = false;

            CombatManager.SetInvincible(false);

            AnimatorController.SetBool("IsFalling", true);
            StartCoroutine(AnimationTrigger("IsFalling"));

            StartCoroutine(PullDashFall());
        }

        /// <summary>
        /// Author: Denis
        /// The whole pull dash move is interruptible upon ground collision at any stage
        /// TODO: Add more interruptible source layers or tags. 
        /// </summary>
        /// <param name="collision"></param>
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.gameObject.layer == 3)
            {
                DisableGravity = false;
                IsPullDashing = false;
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
            yield return new WaitForSecondsRealtime(0.1f);

            switch (Name)
            {
                case "IsFalling":
                    AnimatorController.SetBool("IsFalling", false);
                    break;
                case "IsPullDashing":
                    AnimatorController.SetBool("IsPullDashing", false);
                    break;
            }
        }
    }
}
