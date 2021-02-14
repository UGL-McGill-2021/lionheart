using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lionheart.Player.Movement
{
    public class Rotation : MonoBehaviour, MovementModifier
    {
        [Header("References")]
        [SerializeField] CharacterController PlayerController;
        [SerializeField] MovementHandler PlayerMovementHandler;
        [SerializeField] ControllerInput ControllerActions;

        private Vector3 Direction;

        public Vector3 Value { get; private set; }
        public MovementModifier.MovementType Type { get; private set; }

        private void Awake()
        {
            ControllerActions = new ControllerInput();
            Direction = new Vector3();
            Type = MovementModifier.MovementType.Rotation;
        }

        private void OnEnable()
        {
            ControllerActions.Player.Move.performed += Ctx => Value = new Vector3(Ctx.ReadValue<Vector2>().x, 0f, Ctx.ReadValue<Vector2>().y);
            ControllerActions.Player.Move.canceled += Ctx => Value = Vector2.zero;
            ControllerActions.Player.Move.Enable();

            PlayerMovementHandler.AddModifier(this);
        }

        private void OnDisable()
        {
            ControllerActions.Player.Move.performed -= Ctx => Value = Vector2.zero;
            ControllerActions.Player.Move.canceled -= Ctx => Value = Vector2.zero;
            ControllerActions.Player.Move.Disable();

            PlayerMovementHandler.RemoveModifier(this);
        }

        private void Update() => Move();

        private void Move()
        {
            if (Value.magnitude > 0.2f)
            {
                Direction = Value.normalized;
            }

            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Direction), 15 * Time.deltaTime);
        }
    }
}

