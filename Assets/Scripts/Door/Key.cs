using UnityEngine;

public class Key : MonoBehaviour
{
    //trigger when player collides with key
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Notify the door manager that a key has been collected
            DoorManager.Instance.CollectKey();
            // Destroy the key object
            Destroy(gameObject);
        }
    }
}
