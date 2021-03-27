using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

namespace Lionheart.Player.Movement
{
    /// <summary>
    /// Author: Denis
    /// This class adds movement vectors to produce a cobined vector and move the player.
    /// The Vectors come from the interface list Movement Modifier
    /// </summary>
    public class MovementHandler : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] Rigidbody Rb;
        [SerializeField] Camera PlayerCamera;
        [SerializeField] GameObject Player;
        [SerializeField] PullDash PlayerPullDash;
        [SerializeField] GroundPound PlayerGroundPound;
        [SerializeField] MultiplayerActivator PlayerMultiplayer;
        [SerializeField] Animator AnimatorController;

        [Header("Parameters")]
        [SerializeField] public float AirRestrictAngle = 60f;

        [Header("Photon")]
        public PhotonView PhotonView;

        private readonly List<MovementModifier> Modifiers = new List<MovementModifier>();

        private bool IsPullDashing;
        private bool DisableGravity;
        private bool IsGroundPound;

        /// <summary>
        /// Author: Ziqi
        /// </summary>
        private void Start()
        {
            PhotonView = GetComponent<PhotonView>();
            PlayerPullDash = gameObject.GetComponent<PullDash>();
            PlayerGroundPound = gameObject.GetComponent<GroundPound>();
            PlayerMultiplayer = gameObject.GetComponent<MultiplayerActivator>();
        }

        /// <summary>
        /// Author: Denis
        /// </summary>
        private void FixedUpdate()
        {
            if (PlayerMultiplayer.IgnoreControlInput == false)
            {
                Move();
            }
            else
            {
                AnimatorController.SetFloat("MoveMagnitude", 0.0f);
                Rb.velocity = Vector3.zero;
            }
        }

        /// <summary>
        /// Author: Denis
        /// Adds a movement modifier to the list
        /// </summary>
        /// <param name="Mod"></param>
        public void AddModifier(MovementModifier Mod) => Modifiers.Add(Mod);

        /// <summary>
        /// Author: Denis
        /// Removes a movement modifier to the list
        /// </summary>
        /// <param name="Mod"></param>
        public void RemoveModifier(MovementModifier Mod) => Modifiers.Remove(Mod);

        /// <summary>
        /// Author: Denis
        /// Adds every movement modifier vector and moves the player to the final position.
        /// Also move sthe camera to follow the player.
        /// </summary>
        private void Move()
        {
            IsPullDashing = PlayerPullDash.IsPullDashing;
            DisableGravity = PlayerPullDash.DisableGravity;
            IsGroundPound = PlayerGroundPound.IsGroundPound;
            //TODO xDeprecated
            bool Restrict = AirControlCompensation();

            if (PhotonView.IsMine)
            {
                Vector3 Movement = Vector3.zero;

                foreach (MovementModifier M in Modifiers)
                {
                    //ground pound state overrides other movement vectors
                    if (IsGroundPound == true)
                    {
                        if (M.Type != MovementModifier.MovementType.GroundPound)
                        {
                            continue;
                        }
                    }

                    //gravity is disabled by the pull dash don't apply the gravity vector
                    if (M.Type == MovementModifier.MovementType.Jump)
                    {
                        if (DisableGravity == true)
                        {
                            continue;
                        }
                    }

                    //while pull dashing ignore the walk vector if it is outside of the angle tolerance range
                    if (M.Type == MovementModifier.MovementType.Walk)
                    {
                        if (Restrict == true)
                        {
                            continue;
                        }
                    }

                    Movement += M.Value;
                }

                Rb.velocity = Movement;
            }
        }

        /// <summary>
        /// Author: Denis
        /// While pull dashing air control is restricted to an angle range
        /// TODO xDeprecated
        /// </summary>
        /// <returns></returns>
        private bool AirControlCompensation()
        {
            Vector3 V1 = Vector3.one, V2 = Vector3.one;

            if (IsPullDashing == true)
            {
                foreach (MovementModifier M in Modifiers)
                {
                    if (M.Type == MovementModifier.MovementType.Walk)
                    {
                        V1 = M.Value;
                    }
                    else if (M.Type == MovementModifier.MovementType.PullDash)
                    {
                        V2 = M.Value;
                    }
                }

                float teta = Vector3.Angle(V2, V1);

                if (teta > AirRestrictAngle)
                {
                    return true;
                }
            }

            return false;
        }
    }
}

