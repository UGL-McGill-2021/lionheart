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

        [Header("State")]
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

            if (IsProjectile == false && IsPullDashing == false)
            {
                if (PhotonView.IsMine) PhotonView.RPC("RPC_SetIsProjectile", RpcTarget.All, true);
                canCharge = true;
                Dir = (OtherPlayer.transform.position - transform.position).normalized;
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
                if (PhotonView.IsMine) PhotonView.RPC("RPC_SetIsPullDashing", RpcTarget.All, true);
                if (PhotonView.IsMine) PhotonView.RPC("RPC_SetIsProjectile", RpcTarget.All, false);
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
            if (PhotonView.IsMine) PhotonView.RPC("RPC_SetIsPullDashing", RpcTarget.All, false);
            CurrentPower = MinLaunchPower;
        }

        /// <summary>
        /// Author: Ziqi
        /// RPC setter
        /// </summary>
        /// <param name="status"></param>
        [PunRPC]
        void RPC_SetIsSlingshot(bool status)
        {
            //IsSlingshot = status;
        }

        /// <summary>
        /// Author: Ziqi
        /// RPC setter
        /// </summary>
        /// <param name="status"></param>
        [PunRPC]
        void RPC_SetIsProjectile(bool status)
        {
            IsProjectile = status;
        }

        /// <summary>
        /// Author: Ziqi
        /// RPC setter
        /// </summary>
        /// <param name="status"></param>
        [PunRPC]
        void RPC_SetIsPullDashing(bool status)
        {
            IsPullDashing = status;
        }
    }
}
