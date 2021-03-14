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
        [SerializeField] GameObject OtherPlayer;
        [SerializeField] PullDash OtherPlayerPullDashScript;
        [SerializeField] GameObject OwnPullDashTarget;

        [Header("State")]
        [SerializeField] public bool IsProjectile;
        [SerializeField] public bool IsPullDashing;

        [Header("Launcher")]
        [SerializeField] public readonly float LaunchPower = 25f;
        [SerializeField] public float PowerStep = 0.5f;
        [SerializeField] public float CurrentPower;


        private Vector3 Dir;
        private float GravityForce = Physics.gravity.y;
        private bool canCharge;

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
            IsProjectile = false;
            IsPullDashing = false;
            canCharge = false;

            CurrentPower = LaunchPower;

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
                        OtherPlayer = OtherPlayerPullDashScript.OwnPullDashTarget;
                    }
                }
            }

            if (IsProjectile == false && IsPullDashing == false && OtherPlayerPullDashScript != null)
            {
                IsProjectile = true;
                canCharge = true;
            }
        }

        private void FixedUpdate()
        {
            if (IsProjectile == true)
            {
                Launcher();
            }

            if (IsPullDashing == true)
            {
                Value = Dir * CurrentPower;
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
                Dir = (OtherPlayer.transform.position - transform.position);
                Debug.Log("Vector " + Dir.magnitude + "Time stamp: " + Time.deltaTime);
                Dir = Dir.normalized;
               
                IsProjectile = false;
                canCharge = false;
                IsPullDashing = true;
                StartCoroutine(PullDashExecution());
            }
        }

        IEnumerator PullDashExecution()
        {
            yield return new WaitForSecondsRealtime(0.5f);
            yield return new WaitWhile(() => !gameObject.GetComponent<Jump>().IsGrounded);
            IsPullDashing = false;
            CurrentPower = LaunchPower;
        }
    }
}
