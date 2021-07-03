using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

namespace Lionheart.Player.Movement {
    /// <summary>
    /// Author: Denis
    /// This class handles walking and it's acceleration and deceleration
    /// </summary>
    public class WalkMotion : MonoBehaviour, MovementModifier {

        [Header("References")]
        [SerializeField] MovementHandler PlayerMovementHandler;
        [SerializeField] ControllerInput ControllerActions;
        [SerializeField] Animator AnimatorController;
        [SerializeField] CinemachineBrain CamBrain;

        [Header("Parameters")]
        [SerializeField] private readonly float MaxWalkSpeed = 10f;
        [SerializeField] private readonly float Acceleration = 8f;
        [SerializeField] private readonly float Deceleration = 16f;

        [Header("State")]
        public bool BlockInput = false;

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

            CamBrain = GameObject.Find("FollowCamV2").GetComponentInChildren<CinemachineBrain>();
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

        private void FixedUpdate()
        {
            if (BlockInput == false)
            {
                Move();
            }
        }

        /// <summary>
        /// Author: Denis
        /// Calculates the Speed and acceleration of the player for the current frame and produces its vector
        /// </summary>
        private void Move() {

            var MoveDirection = MoveAction.ReadValue<Vector2>();
            
            Vector2 CorrectedV = Quaternion.Euler(0, 0, 
                -CamBrain.ActiveVirtualCamera.VirtualCameraGameObject.transform.eulerAngles.y) * MoveDirection;

            Value = new Vector3(CorrectedV.x, 0f, CorrectedV.y);

            if (Value.magnitude < 0.2f) {
                Vector3.Lerp(Value, Vector3.zero, 0.01f);
                if (CurrentSpeed > 1f) {
                    CurrentSpeed -= Deceleration * Time.deltaTime;
                } 
                else 
                {
                    CurrentSpeed = 1f;
                }
            } 
            else 
            {
                if (CurrentSpeed < MaxWalkSpeed) {
                    CurrentSpeed += Acceleration * Time.deltaTime;
                }
                Value = Value.normalized * CurrentSpeed;
            }

            AnimatorController.SetFloat("MoveMagnitude", Value.magnitude);
        }

        public void ResetMovementVector()
        {
            Value = Vector3.zero;
        }
    }
}