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
        [SerializeField] Vector3 Direction;
        [SerializeField] GameObject OwnPullDashTarget;
        [SerializeField] GameObject OtherPlayerTarget;
        [SerializeField] PullDash OtherPlayerPullDashScript;

        [Header("State")]
        [SerializeField] public bool ChargingPullDash;
        [SerializeField] public bool IsPullDashing;
        [SerializeField] public bool DisableGravity;

        [Header("Parameters")]
        [SerializeField] public readonly float LaunchVectorMultiplier = 1f;
        [SerializeField] public readonly float MinVectorMagnitude = 15f;
        [SerializeField] public readonly float MaxVectorMagnitude = 40f;
        [SerializeField] public readonly float CompletionDistance = 2f;
        [SerializeField] public readonly float ExpiryTimer = 0.8f;

        private Vector3 T;
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
            IsPullDashing = false;
            DisableGravity = false;

            Type = MovementModifier.MovementType.PullDash;
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
            if (OtherPlayerPullDashScript == null)
            {
                //get a reference to the other player
                GameObject[] _Players = GameObject.FindGameObjectsWithTag("Player");
                for (int i = 0; i < _Players.Length; i++)
                {
                    if (_Players[i].Equals(gameObject) == false)
                    {
                        OtherPlayerPullDashScript = _Players[i].GetComponent<PullDash>();
                        OtherPlayerTarget = OtherPlayerPullDashScript.OwnPullDashTarget;
                    }
                }
            }

            if (ChargingPullDash == false && IsPullDashing == false && OtherPlayerPullDashScript != null)
            {
                ChargingPullDash = true;
            }
        }

        private void FixedUpdate()
        {
            if (ChargingPullDash == true)
            {
                Launcher();
            }

            if (IsPullDashing == true)
            {
                Vector3 _V = Dir * LaunchVectorMultiplier;

                if (_V.magnitude > MaxVectorMagnitude)
                {
                    Value = _V.normalized * MaxVectorMagnitude;
                }
                else if (_V.magnitude < MinVectorMagnitude)
                {
                    Value = _V.normalized * MinVectorMagnitude;
                }
                else
                {
                    Value = _V;
                }

                CheckDistance();
            }
            else
            {
                Value = Vector3.zero;
            }
        }

        private void Launcher()
        {
            if (Gamepad.current.buttonNorth.isPressed == false)
            {
                T = OtherPlayerTarget.transform.position;
                Dir = (OtherPlayerTarget.transform.position - transform.position);

                Debug.Log("Vector " + Dir.magnitude + "Time stamp: " + Time.deltaTime);
               
                ChargingPullDash = false;
                IsPullDashing = true;
                DisableGravity = true;

                StartCoroutine(PullDashTimer());
            }
        }

        private void CheckDistance()
        {
            if (Vector3.Distance(gameObject.transform.position, T)< CompletionDistance)
            {
                DisableGravity = false;
                StartCoroutine(PullDashFall());
            }
        }

        IEnumerator PullDashFall()
        {
            yield return new WaitWhile(() => !gameObject.GetComponent<Jump>().IsGrounded);
            IsPullDashing = false;
        }

        IEnumerator PullDashTimer()
        {
            yield return new WaitForSecondsRealtime(ExpiryTimer);
            DisableGravity = false;
            StartCoroutine(PullDashFall());
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.gameObject.layer == 3)
            {
                DisableGravity = false;
                IsPullDashing = false;
            }
        }
    }
}
