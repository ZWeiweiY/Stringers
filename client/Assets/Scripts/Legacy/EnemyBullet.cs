using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float lifetime = 3f; // Bullet disappears after 3 seconds
    public float bulletSpeed = 20f;

    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.linearVelocity = -Vector3.forward * bulletSpeed;
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<STGShipController>()!=null)
        {
            other.gameObject.GetComponent<STGShipController>().Hit();
            Destroy(gameObject);
        }
    }

    public void Blocked()
    {
        Destroy(gameObject);
    }
}