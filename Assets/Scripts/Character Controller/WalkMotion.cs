using UnityEngine;
using UnityEngine.InputSystem;

namespace Lionheart.Player.Movement {
    /// <summary>
    /// Author: Denis
    /// This class handles walking and it's acceleration and deceleration
    /// </summary>
    public class WalkMotion : MonoBehaviour, MovementModifier {

        [Header("References")]
        [SerializeField] MovementHandler PlayerMovementHandler;
        [SerializeField] ControllerInput ControllerActions;

        [Header("Parameters")]
        [SerializeField] private readonly float MaxWalkSpeed = 10f;
        [SerializeField] private readonly float Acceleration = 8f;
        [SerializeField] private readonly float Deceleration = 16f;

        private float CurrentSpeed = 1f;

        [Header("Input")]
        public InputAction MoveAction = new InputAction("move", binding: "<Gamepad>/leftStick");

        public Vector3 Value { get; private set; }
        public MovementModifier.MovementType Type { get; private set; }

        /// <summary>
        /// Author: Denis
        /// Initial setup
        /// </summary>
        private void Awake() {
            Type = MovementModifier.MovementType.Walk;
        }

        /// <summary>
        /// Author: Denis
        /// Subscribing to the controller events and adding this modifier to the movement modifiers list
        /// </summary>
        private void OnEnable() {
            MoveAction.Enable();

            PlayerMovementHandler.AddModifier(this);
        }

        /// <summary>
        /// Author: Denis
        /// Unsubscribing to the controller events and removing this modifier from the movement modifiers list
        /// </summary>
        private void OnDisable() {
            MoveAction.Disable();

            PlayerMovementHandler.RemoveModifier(this);
        }

        private void Update() => Move();

        /// <summary>
        /// Author: Denis
        /// Calculates the Speed and acceleration of the player for the current frame and produces its vector
        /// </summary>
        private void Move() {
            
            var MoveDirection = MoveAction.ReadValue<Vector2>();
            Value = new Vector3(MoveDirection.x, 0f, MoveDirection.y);

            if (Value.magnitude < 0.2f) {
                Vector3.Lerp(Value, Vector3.zero, 0.01f);
                if (CurrentSpeed > 1f) {
                    CurrentSpeed -= Deceleration * Time.deltaTime;
                } else {
                    CurrentSpeed = 1f;
                }
            } else {
                if (CurrentSpeed < MaxWalkSpeed) {
                    CurrentSpeed += Acceleration * Time.deltaTime;
                }
                Value = Value.normalized * CurrentSpeed;
            }
        }
    }
}