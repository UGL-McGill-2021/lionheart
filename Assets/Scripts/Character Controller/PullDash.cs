using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

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

        [Header("State")]
        [SerializeField] public bool IsSlingshot;
        [SerializeField] public bool IsProjectile;
        [SerializeField] public bool IsPullDashing;

        [Header("Launcher")]
        [SerializeField] public readonly float MaxLaunchPower = 35f;
        [SerializeField] public readonly float MinLaunchPower = 25f;
        [SerializeField] public float PowerStep = 0.5f;
        [SerializeField] public float CurrentPower;
        [SerializeField] private readonly float JumpPower = 3f;


        private Vector3 Dir;
        private float GravityForce = Physics.gravity.y;
        private bool canCharge;

        public Vector3 Value { get; private set; }
        public MovementModifier.MovementType Type { get; private set; }

        /// <summary>
        /// Author: Denis
        /// Initial setup
        /// </summary>
        private void Awake()
        {
            ControllerActions = new ControllerInput();
            IsSlingshot = false;
            IsProjectile = false;
            IsPullDashing = false;
            canCharge = false;

            CurrentPower = MinLaunchPower;

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
                        OtherPlayer = _Players[i];
                        OtherPlayerPullDashScript = _Players[i].GetComponent<PullDash>();
                    }
                }
            }

            if (IsSlingshot == false && IsProjectile == false && IsPullDashing == false)
            {
                if (OtherPlayerPullDashScript.IsSlingshot)
                {
                    IsProjectile = true;
                    canCharge = true;
                    Dir = (OtherPlayer.transform.position - transform.position).normalized;
                }
                else
                {
                    IsSlingshot = true;
                    //trigger UI element
                }
            }
        }

        private void Update()
        {
            if (IsProjectile == true)
            {
                if (OtherPlayerPullDashScript.IsSlingshot == false)
                {
                    IsProjectile = false;
                }
                else
                {
                    Launcher();
                }
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
            if (Gamepad.current.buttonNorth.isPressed == true)
            {
                if (CurrentPower < MaxLaunchPower && canCharge == true)
                {
                    CurrentPower += PowerStep;
                    canCharge = false;
                    StartCoroutine(ChargePullDash());
                }
            }
            else
            {
                Debug.Log("Launch force " + CurrentPower);
                IsPullDashing = true;
                OtherPlayerPullDashScript.IsSlingshot = false;
                IsProjectile = false;
                StartCoroutine(PullDashExecution());
            }
        }

        IEnumerator ChargePullDash()
        {
            yield return new WaitForSecondsRealtime(0.05f);
            canCharge = true;
        }

        IEnumerator PullDashExecution()
        {
            yield return new WaitForSecondsRealtime(0.1f);
            yield return new WaitWhile(() => !gameObject.GetComponent<Jump>().IsGrounded);
            IsPullDashing = false;
            CurrentPower = MinLaunchPower;
        }
    }
}
