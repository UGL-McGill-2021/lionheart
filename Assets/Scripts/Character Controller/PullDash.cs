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
        [SerializeField] PullDash OtherPlayer;

        [Header("State")]
        [SerializeField] public bool IsSlingshot;
        [SerializeField] public bool IsProjectile;
        [SerializeField] public bool IsPullDashing;

        [Header("Launcher")]
        [SerializeField] public readonly float MaxLaunchPower = 18f;
        [SerializeField] public readonly float MinLaunchPower = 12f;
        [SerializeField] public float PowerStep = 0.5f;
        [SerializeField] public float CurrentPower = 12f;
        [SerializeField] private Vector3 Vec;

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
            Vec = new Vector3();

            Type = MovementModifier.MovementType.PullDash;

            //get a reference to the other player
            GameObject[] _Players = GameObject.FindGameObjectsWithTag("Player");
            for (int i = 0; i < _Players.Length; i++)
            {
                if (_Players[i].Equals(gameObject) == false)
                {
                    OtherPlayer = _Players[i].GetComponent<PullDash>();
                }
            }
        }

        /// <summary>
        /// Author: Denis
        /// Subscribing to the controller events and adding this modifier to the movement modifiers list
        /// </summary>
        private void OnEnable()
        {
            ControllerActions.Player.PullDash.performed += RegisterPullDash;
            ControllerActions.Player.Dash.Enable();

            PlayerMovementHandler.AddModifier(this);
        }

        /// <summary>
        /// Author: Denis
        /// Unsubscribing to the controller events and removing this modifier from the movement modifiers list
        /// </summary>
        private void OnDisable()
        {
            ControllerActions.Player.PullDash.performed -= RegisterPullDash;
            ControllerActions.Player.Dash.Disable();

            PlayerMovementHandler.RemoveModifier(this);
        }

        /// <summary>
        /// Author: Denis
        /// Processes the Y(XB)/Triangle(PS4) button press and executes the jump
        /// </summary>
        /// <param name="Ctx"></param>
        private void RegisterPullDash(InputAction.CallbackContext Ctx)
        {
            if (IsSlingshot==false && IsProjectile == false)
            {
                if (OtherPlayer.IsSlingshot)
                {
                    IsProjectile = true;
                    //TODO charge launch
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
                if (OtherPlayer.IsSlingshot == false)
                {
                    IsProjectile = false;
                }
                else
                {
                    Launcher();
                }
            }
        }

        private void Launcher()
        {
            if (Gamepad.current.buttonNorth.isPressed == true)
            {
                CurrentPower += PowerStep * Time.deltaTime;
            }
            else
            {
                IsPullDashing = true;
                OtherPlayer.IsSlingshot = false;
                IsProjectile = false;
                StartCoroutine(PullDashExecution());
            }

            if (IsPullDashing == true)
            {
                Value = gameObject.transform.forward * CurrentPower;
            }
            else
            {
                Value = Vector3.zero;
            }
        }

        IEnumerator PullDashExecution()
        {
            yield return new WaitWhile(() => !gameObject.GetComponent<Jump>().IsGrounded);
            IsPullDashing = false;
        }
    }
}
