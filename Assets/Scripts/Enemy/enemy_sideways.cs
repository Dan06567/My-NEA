using UnityEngine;

public class Spike : MonoBehaviour
{
    [SerializeField] private float damage; // Damage dealt by the enemy

    private void OnTriggerEnter2D(Collider2D collision) // Method to detect collisions with the player
    { 
        if (collision.gameObject.tag == "Player") // Check if the collision is with the player
        {
            collision.gameObject.GetComponent<Health>().Damage(damage); // Damage the player
        }
    }
}
