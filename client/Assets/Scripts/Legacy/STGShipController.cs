using System;
using UnityEngine;

public class STGShipController : MonoBehaviour
{
    public float moveSpeed = 10f; // Movement speed
    public float rotationSpeed = 300f; // Rotation speed
                                       //public Vector2 moveLimits = new Vector2(7f, 4f); // Screen boundaries
    public KeyCode clockwiseRotate = KeyCode.RightArrow;
    public KeyCode counterclockwiseRotate = KeyCode.LeftArrow;

    public FaceDataSO faceData;
    private Vector3 targetVelocity;
    [SerializeField] private float interpolationFactor = 0.1f;
    
    [Header("Fire")] public GameObject bulletPrefab; // Assign bullet prefab in the Inspector
    public float fireRate = 0.2f; // Time between shots
    public Transform firePoint; // Empty GameObject positioned at the front of the ship
    private float nextFireTime = 0f;
    public event Action FireActionTriggered;

    [Header("Dash")] public float dashDistance = 3f;
    public float dashCooldown = 1f;
    private float nextDashTime = 0f;
    public bool dashing;
    public event Action DashActionTriggered;

    [Header("Shield")] public GameObject shieldObject;
    public float shieldCooldown = 1f;
    private float nextShieldTime = 0f;
    public event Action SheildOnActionTriggered;
    public event Action SheildOffActionTriggered;
    public event Action SheildHitActionTriggered;


    private void Awake()
    {
        shieldObject.SetActive(false);
    }

    void Update()
    {
        NewMovement();
        //HandleMovement();
        HandleAction();
    }

    void NewMovement() 
    {
        Vector3 movement = transform.forward.normalized * Time.deltaTime;
        transform.Translate(movement, Space.World);
        if (Input.GetKey(clockwiseRotate))
        {
            transform.Rotate(transform.up, rotationSpeed * Time.deltaTime);
        }

        if (Input.GetKey(counterclockwiseRotate))
        {
            transform.Rotate(transform.up, -rotationSpeed * Time.deltaTime);
        }
    }
    void HandleMovement()
    {
        float moveX = faceData.Roll * moveSpeed * interpolationFactor;
        float moveY = faceData.Pitch * moveSpeed * interpolationFactor; // Up/Down movement

        // Move the ship instantly (without physics inertia)
        Vector3 newPosition = transform.position + new Vector3(moveX, 0, moveY) * (moveSpeed * Time.deltaTime);

        // Clamp movement within the screen boundaries
        //newPosition.x = Mathf.Clamp(newPosition.x, -moveLimits.x, moveLimits.x);
        //newPosition.z = Mathf.Clamp(newPosition.z, -moveLimits.y, moveLimits.y);

        transform.position = newPosition;

        // Rotate ship towards movement direction (optional)
        /*if (moveX != 0 || moveY != 0)
        {
            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(moveX, 0, moveY));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }*/
    }
    
    private void ProcessMovement()
    {
        // Convert FaceData values into movement direction
        float moveX = faceData.Yaw * moveSpeed; // Left/Right movement
        float moveY = faceData.Pitch * moveSpeed; // Up/Down movement

        // Calculate target velocity
        targetVelocity = new Vector3(moveX, 0, moveY);

        // Interpolate velocity for smooth movement
        GetComponent<Rigidbody>().linearVelocity = Vector3.Lerp(GetComponent<Rigidbody>().linearVelocity, targetVelocity, interpolationFactor);
    }

    void HandleAction()
    {
        // Shoot
        if (faceData.action == "Fire" && Time.time >= nextFireTime)
        {
            Shoot();
        }

        if (faceData.action == "Shield" && Time.time >= nextShieldTime)
        {
            Guard();
        }

        if (faceData.action == "Dash" && Time.time >= nextDashTime)
        {
            Dash();
        }
    }

    void Shoot()
    {
        Debug.Log("fire");
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        nextFireTime = Time.time + fireRate;
        FireActionTriggered?.Invoke();
    }

    void Guard()
    {
        Debug.Log("guard");
        shieldObject.SetActive(true);
        SheildOnActionTriggered?.Invoke();

    }

    void Dash()
    {
        Debug.Log("dash");
        transform.position += transform.forward * dashDistance;
        nextDashTime = Time.time + dashCooldown;
        DashActionTriggered?.Invoke();
    }

    public void GuardDown()
    {
        if (Time.time >= nextShieldTime)
        {
            shieldObject.SetActive(false);
            nextShieldTime = Time.time + shieldCooldown;
        }
        SheildOffActionTriggered?.Invoke();
    }

    public void Hit()
    {
        Debug.Log("hit");
    }

}