using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

namespace Lionheart.Player.Movement
{
    public class PlayerMovement : MonoBehaviour, MovementModifier
    {
        [Header("References")]
        [SerializeField] CharacterController PlayerController;
        [SerializeField] MovementHandler PlayerMovementHandler;
        [SerializeField] InputActions PlayerActions;

        [Header("Parameters")]
        [SerializeField] private readonly float MaxSpeed = 10f;
        [SerializeField] private readonly float Acceleration = 0.2f;

        private float PreviousSpeed;
        private float CurrentSpeed;

        private Vector3 Direction;
        private Vector3 PreviousVelocity;
        private Vector2 PreviousInputDirection;

        public Vector3 Value { get; private set; }

        private void Awake()
        {
            PlayerActions = new InputActions();
            Direction = new Vector3();
        }

        private void OnEnable()
        {
            PlayerActions.Player.Move.performed += Ctx => Value = new Vector3(Ctx.ReadValue<Vector2>().x, 0f, Ctx.ReadValue<Vector2>().y);
            PlayerActions.Player.Move.canceled += Ctx => Value = Vector2.zero;
            PlayerActions.Player.Move.Enable();

            PlayerMovementHandler.AddModifier(this);
        }

        private void OnDisable()
        {
            PlayerActions.Player.Move.performed -= Ctx => Value = Vector2.zero;
            PlayerActions.Player.Move.canceled -= Ctx => Value = Vector2.zero;
            PlayerActions.Player.Move.Disable();

            PlayerMovementHandler.RemoveModifier(this);
        }

        private void Update() => Move();
       
        private void Move()
        {
            /*Debug.Log("Mag" + Value.magnitude);
            Debug.Log("Speed" + CurrentSpeed);*/

            if (Value.magnitude > 0.2f)
            {
                Direction = Value.normalized;
            }

            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Direction), 15 * Time.deltaTime);

            if (Value.magnitude < 0.2f)
            {
                Vector3.Lerp(Value, Vector3.zero, 0.01f);
            }
            else
            {
                Value = Value.normalized * MaxSpeed;
            }

        }
    }
}