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

        private Vector3 PCamOffset;

        private readonly List<MovementModifier> Modifiers = new List<MovementModifier>();

        private void Awake()
        {
            //PCamOffset = Vector3.Distance(PlayerCamera.transform.position, PlayerController.transform.position);
        }

        private void Update() => Move();

        public void AddModifier(MovementModifier mod) => Modifiers.Add(mod);

        public void RemoveModifier(MovementModifier mod) => Modifiers.Remove(mod);

        private void Move()
        {
            Vector3 Movement = Vector3.zero;

            foreach (MovementModifier m in Modifiers)
            {
                Movement += m.Value;
            }
            
            PlayerController.Move(Movement * Time.deltaTime);
            PlayerCamera.transform.position += Movement * Time.deltaTime;
        }
    }
}

