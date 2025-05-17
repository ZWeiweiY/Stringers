using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CubeController : MonoBehaviour
{
    [SerializeField] private FaceDataSO faceData;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 100f;
    [SerializeField] private float interpolationFactor = 0.1f;

    private Rigidbody rb;
    private Vector3 targetVelocity;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        ProcessAction();
    }

    private void FixedUpdate()
    {
        ProcessMovement();
        ProcessRotation();
    }

    private void ProcessMovement()
    {
        // Convert FaceData values into movement direction
        float moveX = faceData.Yaw * moveSpeed; // Left/Right movement
        float moveY = faceData.Pitch * moveSpeed; // Up/Down movement

        // Calculate target velocity
        targetVelocity = new Vector3(moveX, moveY, 0);

        // Interpolate velocity for smooth movement
        rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, targetVelocity, interpolationFactor);
    }

    private void ProcessRotation()
    {
        // Rotate based on Roll (tilt head)
        float rollAngle = faceData.Roll * rotationSpeed * Time.fixedDeltaTime;
        rb.MoveRotation(rb.rotation * Quaternion.Euler(0, 0, -rollAngle)); // Negative to match head movement
    }

    private void ProcessAction()
    {
        // Perform action based on blendshape
        if (faceData.action == "Fire")
        {
            Debug.Log("Fire!");
        }

        if (faceData.action == "Dash")
        {
            Debug.Log("Dash!");
        }

        if(faceData.action == "Shield")
        {
            Debug.Log("Shield!");
        }
    }
}
