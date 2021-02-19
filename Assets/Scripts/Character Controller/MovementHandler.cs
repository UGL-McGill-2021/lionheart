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
        //[SerializeField] CharacterController PlayerController;
        [SerializeField] Rigidbody Rb;
        [SerializeField] Camera PlayerCamera;
        [SerializeField] GameObject Player;

        private readonly List<MovementModifier> Modifiers = new List<MovementModifier>();

        // Photon:
        public PhotonView PhotonView;
        private Vector3 RemotePosition;
        private bool isOffLineMode;

        private void Start() {
            PhotonView = GetComponent<PhotonView>();
            //isOffLineMode = this.GetComponent<MultiplayerActivator>().isOffLine;
        }

        private void FixedUpdate() {
            Move();
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
        private void Move() {
            Vector3 Movement = Vector3.zero;

            foreach (MovementModifier M in Modifiers) {
                Movement += M.Value;
            }

            Rb.MovePosition(transform.position + Movement * Time.deltaTime);
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
            Debug.Log("Colliding at " + Time.time);
        }
    }
}

