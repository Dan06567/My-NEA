using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Movement Parameters")]
    [SerializeField] private float speed;
    [SerializeField] private float movementDistance;

    [Header("Combat Parameters")]
    [SerializeField] private int damage = 1;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private int enemyHealth = 1;

    private Vector3 startingPosition;
    private bool movingLeft;
    private Collider2D enemyCollider;
    private float lastAttackTime;
    private Transform player;
    private Animator animator;

    private void Start()
    {
        startingPosition = transform.position;
        enemyCollider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        transform.localScale = new Vector3(5, 5, 5); // Initial size
    }

    private void Update()
    {
        if (player == null) return;

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
        if (movingLeft)
        {
            if (transform.position.x > startingPosition.x - movementDistance)
            {
                transform.position = new Vector3(transform.position.x - speed * Time.deltaTime, transform.position.y, transform.position.z);
                transform.localScale = new Vector3(-5, 5, 5); // Left facing scale
            }
            else
                movingLeft = false;
        }
        else
        {
            if (transform.position.x < startingPosition.x + movementDistance)
            {
                transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, transform.position.y, transform.position.z);
                transform.localScale = new Vector3(5, 5, 5); // Right facing scale
            }
            else
                movingLeft = true;
        }
    }

    private void Attack()
    {
        // Check if player is in attack range and damage them
        if (Vector2.Distance(transform.position, player.position) <= attackRange)
        {
            Health playerHealth = player.GetComponent<Health>();
            if (playerHealth != null)
            {
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
            // Kill the enemy
            Die();
        }
    }

    public void TakeDamage(int damage)
    {
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
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}

