using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Movement Parameters")]
    [SerializeField] private float speed; // Speed of the enemy
    [SerializeField] private float movementDistance; // Distance the enemy will move from its starting position
    [SerializeField] private LayerMask blockLayer; // Layer mask for detecting blocks

    [Header("Combat Parameters")]
    [SerializeField] private int damage; // Damage dealt by the enemy
    [SerializeField] private float attackRange; // Range at which the enemy will attack
    [SerializeField] private float attackCooldown; // Cooldown between attacks
    [SerializeField] private int enemyHealth ; // Health of the enemy

    private Vector3 startingPosition; // Starting position of the enemy
    private bool movingLeft; // Boolean indicating if the enemy is moving left
    private Collider2D enemyCollider; // Collider of the enemy
    private float lastAttackTime; // Time of the last attack
    private Transform player; // Reference to the player
    private float directionChangeTimer; // Timer for changing direction

    private void Start()
    {
        startingPosition = transform.position; // Set the starting position
        enemyCollider = GetComponent<Collider2D>(); // Get the collider component
        player = GameObject.FindGameObjectWithTag("Player").transform; // Find the player object
        transform.localScale = new Vector3(5, 5, 5); // Initial size of the enemy
        directionChangeTimer = 2f; // Initialize the direction change timer
    }

    private void Update()
    {
        // Check if the player is in range
        if (player == null) return;

        // Calculate the distance to the player
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // If player is in attack range, stop and attack
        if (distanceToPlayer <= attackRange)
        {
            // Face the player with scale (5,5,5)
            transform.localScale = new Vector3(
                player.position.x > transform.position.x ? 5 : -5,
                5, 5);

            // Attack with cooldown
            if (Time.time >= lastAttackTime + attackCooldown)
            {
                Attack();
                lastAttackTime = Time.time;
            }
        }
        // Otherwise patrol
        else
        {
            Patrol();
        }
    }

    private void Patrol()
    {
        // Update the direction change timer
        directionChangeTimer -= Time.deltaTime; // Decrease the timer
        // Check if the enemy has moved the desired distance
        if (directionChangeTimer <= 0)
        {
            // Change direction and reset the timer
            movingLeft = !movingLeft;
            directionChangeTimer = 2f;
        }

        // Check for block collision
        // Cast a ray to check for blocks in front of the enemy
        Vector2 direction;
        if (movingLeft) 
        {
            direction = Vector2.left;
        }
        else
        {
            direction = Vector2.right;
        }
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 0.1f, blockLayer);
        if (hit.collider != null)// If a block is detected
        {
            // Change direction if a block is detected
            movingLeft = !movingLeft;
        }

        // Move the enemy left and right
        if (movingLeft)
        {
            // Move left
            transform.position = new Vector3(transform.position.x - speed * Time.deltaTime, transform.position.y, transform.position.z);
            transform.localScale = new Vector3(-5, 5, 5); // Left facing scale
        }
        else
        {
            // Move right
            transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, transform.position.y, transform.position.z);
            transform.localScale = new Vector3(5, 5, 5); // Right facing scale
        }
    }

    private void Attack()
    {
        // Check if player is in attack range and damage them
        if (Vector2.Distance(transform.position, player.position) <= attackRange)
        {
            // Damage the player
            Health playerHealth = player.GetComponent<Health>();
            if (playerHealth != null) // Check if the player has a health component
            {
                // Damage the player
                playerHealth.Damage(damage);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collision is with a fireball
        if (collision.gameObject.CompareTag("Fireball"))
        {
            // Deactivate the fireball instead of destroying it
            collision.gameObject.SetActive(false);
            // Take 1 damage instead of dying
            TakeDamage(1);
        }
    }

    public void TakeDamage(int damage)
    {
        // Reduce the enemy's health by the damage amount
        enemyHealth -= damage;
        if (enemyHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // If you have a death animation, trigger it here
        // animator.SetTrigger("Death");

        // Disable components
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;

        // Destroy the enemy
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize attack range in editor
        Gizmos.color = Color.red;
        // Draw a wire sphere at the enemy's position with the attack range radius
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}

