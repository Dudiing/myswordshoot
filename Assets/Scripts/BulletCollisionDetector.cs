using UnityEngine;

public class BulletCollisionDetector : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collided object has the desired tag
        if (collision.gameObject.CompareTag("DianaTag"))
        {
            // Destroy the collided object
            Destroy(collision.gameObject);
        }

        // Destroy the bullet
        Destroy(gameObject);
    }
}
