using UnityEngine;

/// <summary>
/// Author: Feiyang Li
/// Floats objects in the background
/// </summary>
public class FloatMotor : MonoBehaviour {
    private float Height = 0.1f;
    private float Period = 1;

    private Vector3 InitialPos;
    private float Offset;

    private void Awake() {
        InitialPos = transform.position;

        Offset = 1 - (Random.value * 2);

        Height = Random.Range(2f, 7f);
        Period = Random.Range(0.5f, 1.5f);
    }

    private void Update() {
        transform.position = InitialPos - Vector3.up * Mathf.Sin((Time.time + Offset) * Period) * Height;
    }
}
