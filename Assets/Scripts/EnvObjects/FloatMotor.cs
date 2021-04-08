using UnityEngine;

/// <summary>
/// Author: Feiyang Li
/// Floats objects in the background
/// </summary>
public class FloatMotor : MonoBehaviour {
    [SerializeField]
    float Height = 0.1f;

    [SerializeField]
    float Period = 1;

    private Vector3 InitialPos;
    private float Offset;

    private void Awake() {
        InitialPos = transform.position;

        Offset = 1 - (Random.value * 2);
    }

    private void Update() {
        transform.position = InitialPos - Vector3.up * Mathf.Sin((Time.time + Offset) * Period) * Height;
    }
}
