using System.Collections;
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
        [SerializeField] CharacterController PlayerController;
        [SerializeField] Camera PlayerCamera;
        [SerializeField] GameObject Player;

        private readonly List<MovementModifier> Modifiers = new List<MovementModifier>();

        private void Update() => Move();

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
            Vector3 Movement = Vector3.zero;

            foreach (MovementModifier M in Modifiers)
            {
                Movement += M.Value;
            }
            
            PlayerController.Move(Movement * Time.deltaTime);

            Player.transform.position = new Vector3(PlayerController.transform.position.x,
                PlayerController.transform.position.y, PlayerController.transform.position.z);
        }
    }
}

