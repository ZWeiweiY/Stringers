using UnityEngine;

public class MoveController : MonoBehaviour
{
    public float speed = 5f;       // Movement speed
    public float rotationSpeed = 150f; // Rotation speed (degrees per second)
    public FaceDataSO faceData;
    [SerializeField] private float interpolationFactor = 0.1f;
    void Update()
    {
        // Move forward at a constant speed
        transform.position += transform.forward * (speed * Time.deltaTime);

        // Get rotation input (keyboard/controller)
        float rotationInput = Input.GetAxis("Horizontal"); // "A/D" or Left Stick X (Gamepad)
        float moveX = faceData.Roll * speed * interpolationFactor;
        // Apply rotation
        transform.Rotate(Vector3.up * (moveX * rotationSpeed * Time.deltaTime));
    }
}