using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifetime = 3f; // Bullet disappears after 3 seconds
    public float bulletSpeed = 20f;

    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.linearVelocity = transform.forward * bulletSpeed;
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<UFOController>() != null)
        {
            other.gameObject.GetComponent<UFOController>().Hit();
            Destroy(gameObject);
        }
    }
}