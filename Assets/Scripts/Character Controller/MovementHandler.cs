using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lionheart.Player.Movement
{
    public class MovementHandler : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] CharacterController PlayerController;
        [SerializeField] Camera PlayerCamera;
        [SerializeField] GameObject Player;

        private readonly List<MovementModifier> Modifiers = new List<MovementModifier>();

        private void Update() => Move();

        public void AddModifier(MovementModifier Mod) => Modifiers.Add(Mod);

        public void RemoveModifier(MovementModifier Mod) => Modifiers.Remove(Mod);

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

