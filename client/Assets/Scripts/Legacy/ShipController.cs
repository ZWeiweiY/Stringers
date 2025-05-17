using UnityEngine;

public class ShipController : MonoBehaviour
{
    public float thrustForce = 10f;
    public float rotationSpeed = 100f;
    private Rigidbody rb;


    void Start()
    {
        rb = GetComponent<Rigidbody>();

    }

    void Update()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        // Forward/Backward Movement
        if (Input.GetKey(KeyCode.W))
        {
            rb.AddForce(transform.forward * thrustForce);
        }
        

        // Rotation
        float rotation = 0;
        if (Input.GetKey(KeyCode.A))
        {
            rotation = -rotationSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            rotation = rotationSpeed * Time.deltaTime;
        }

        rb.AddTorque(Vector3.up * rotation);
    }
}