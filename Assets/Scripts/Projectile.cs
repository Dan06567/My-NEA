using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed; // Speed at which the projectile moves
    [SerializeField] private float maxLifetime = 5f; // Maximum lifetime of the projectile
    private float direction; // Direction of the projectile (1 for right, -1 for left)
    private bool hit; // Flag to determine if the projectile has hit something
    private float lifetime; // Tracks how long the projectile has been active

    private Animator anim; // Animator to handle projectile animations
    private BoxCollider2D boxCollider; // Collider for detecting collisions

    private void Awake()
    {
        // Get references to the Animator and BoxCollider2D components
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        // Update the projectile's movement and lifetime
        if (hit)
        {
            return;
        }
        // Move the projectile
        MoveProjectile();
        // Check the lifetime of the projectile
        CheckLifetime();
    }

    private void MoveProjectile()
    {
        // Calculate movement based on speed, direction, and deltaTime
        float movementSpeed = speed * Time.deltaTime * direction;
        transform.Translate(movementSpeed, 0, 0); // Move the projectile
    }

    private void CheckLifetime()
    {
        // Increment lifetime and deactivate the projectile after maxLifetime seconds
        lifetime += Time.deltaTime; // Increment the lifetime timer
        if (lifetime > maxLifetime) // Check if the lifetime has exceeded the maximum
        {
            Deactivate(); // Deactivate the projectile
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // When the projectile collides with another object
        hit = true; // Mark the projectile as having hit something
        boxCollider.enabled = false; // Disable the collider to prevent further collisions
        anim.SetTrigger("explode"); // Trigger the explosion animation
        Deactivate(); // Deactivate the projectile
    }

    public void SetDirection(float thedirection)
    {
        // Initialize the projectile with a specified direction
        lifetime = 0; // Reset the lifetime timer
        direction = thedirection; // Set the direction (1 for right, -1 for left)
        gameObject.SetActive(true); // Activate the projectile
        hit = false; // Reset the hit flag
        boxCollider.enabled = true; // Enable the collider for collision detection

        // Adjust the local scale of the projectile to match the direction
        float localScaleX = transform.localScale.x;
        if (Mathf.Sign(localScaleX) != thedirection)
        {
            localScaleX = -localScaleX; // Flip the projectile if necessary
        }
        // Apply the new local scale
        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
    }

    private void Deactivate()
    {
        // Deactivate the projectile (e.g., when it hits something or its lifetime expires)
        gameObject.SetActive(false);
    }
}
