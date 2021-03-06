using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

namespace Lionheart.Player.Movement {
    /// <summary>
    /// Author: Denis
    /// This class adds movement vectors to produce a cobined vector and move the player.
    /// The Vectors come from the interface list Movement Modifier
    /// </summary>
    public class MovementHandler : MonoBehaviour {
        [Header("References")]
        [SerializeField] Rigidbody Rb;
        [SerializeField] Camera PlayerCamera;
        [SerializeField] GameObject Player;

        private readonly List<MovementModifier> Modifiers = new List<MovementModifier>();

        [Header("Photon")]
        public PhotonView PhotonView;

        /// <summary>
        /// Author: Ziqi
        /// </summary>
        private void Start() {
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
        private void Move() {
            if (PhotonView.IsMine)
            {
                Vector3 Movement = Vector3.zero;

                foreach (MovementModifier M in Modifiers)
                {
                    Movement += M.Value;
                }
<<<<<<< HEAD

                Rb.velocity = Movement;
            }
=======
                //Movement.x += _AdditionalVelocity.x;
                //Movement.z += _AdditionalVelocity.z;

                //Rb.MovePosition(transform.position + Movement * Time.deltaTime);
                //Vector3 rbVelocoty = Rb.velocity;
                //rbVelocoty.x = Movement.x;
                //rbVelocoty.z = Movement.z;
                //rbVelocoty.y += Movement.y;
                //Rb.velocity = rbVelocoty;
                Rb.velocity = Movement;
            }

            //_AdditionalVelocity = Vector3.zero;  // reset the additional velocity
        }

        /// <summary>
        /// Author: Ziqi Li
        /// Function for adding velocity to current additional velocity of this player (will be called by other objects
        /// or player ex: moving platform)
        /// </summary>
        /// <param name="velocity"></param>
        public void AddVelocity(Vector3 velocity)
        {
            _AdditionalVelocity += velocity;
        }

        /// <summary>
        /// Author: Denis
        /// Stops Drift after clipping through a wall. 
        /// TODO: Prevent wall clipping
        /// </summary>
        /// <param name="collision"></param>
        private void OnCollisionEnter(Collision collision)
        {
            Rb.velocity = new Vector3(0f, Rb.velocity.y, 0f);
            //Debug.Log("Colliding at " + Time.time);
>>>>>>> main
        }
    }
}

