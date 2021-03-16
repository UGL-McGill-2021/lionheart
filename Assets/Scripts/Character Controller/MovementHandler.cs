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

        private readonly List<MovementModifier> Modifiers = new List<MovementModifier>();

        [Header("Photon")]
        public PhotonView PhotonView;

        private bool IsPullDashing;
        private bool DisableGravity;

        /// <summary>
        /// Author: Ziqi
        /// </summary>
        private void Start()
        {
            PhotonView = GetComponent<PhotonView>();
        }

        private void FixedUpdate() => Move();

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
            IsPullDashing = gameObject.GetComponent<PullDash>().IsPullDashing;
            DisableGravity = gameObject.GetComponent<PullDash>().DisableGravity;
            bool Restrict = AirControlCompensation();

            if (PhotonView.IsMine)
            {
                Vector3 Movement = Vector3.zero;

                foreach (MovementModifier M in Modifiers)
                {
                    if (M.Type == MovementModifier.MovementType.Jump)
                    {
                        if (DisableGravity == true)
                        {
                            continue;
                        }
                    }

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
        /// 
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

                if (teta > 60)
                {
                    return true;
                }
            }

            return false;
        }
    }
}

