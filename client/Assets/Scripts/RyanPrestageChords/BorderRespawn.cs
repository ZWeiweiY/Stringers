using UnityEngine;

public class BorderRespawn : MonoBehaviour
{
    public Transform respawnPoint;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")) 
        {
            collision.gameObject.transform.position = respawnPoint.position;
            collision.gameObject.transform.rotation = respawnPoint.rotation;
            collision.gameObject.GetComponent<RBMoveController>()?.StopMovement();
        }
    }
}
