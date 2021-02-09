using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;

/// <summary>
/// Ahutor: Feiyang Li
/// Created for Prototype Testing Purposes
/// </summary>
public class PrototypeCharacterMovementControls : MonoBehaviour{
    public InputAction MoveAction = new InputAction("move", binding: "<Gamepad>/leftStick");
    public InputAction LookAction = new InputAction("look", binding: "<Gamepad>/rightStick");
    private Vector2 MoveDirection;
    private Vector2 LookDirectoin;
    public CharacterController controller;
    public Camera camera;

    private void Awake() {
        MoveAction.Enable();
        LookAction.Enable();
    }

    private void Update() {
        var MoveDirection = MoveAction.ReadValue<Vector2>();
        //Debug.Log("Joystick Input: " + MoveDirection);
        controller.Move(new Vector3(MoveDirection.x, 0, MoveDirection.y));

        var LookDirection = LookAction.ReadValue<Vector2>();
        gameObject.transform.Rotate(new Vector3(LookDirectoin.x, 0, LookDirectoin.y));
    }
}
