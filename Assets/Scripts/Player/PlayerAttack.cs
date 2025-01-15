using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject fireballPrefab; // Prefab for the fireball
    [SerializeField] private int maxFireballs = 10; // Maximum number of fireballs

    private Animator anim;
    private PlayerMovement playerMovement;
    private float cooldownTimer = Mathf.Infinity;
    private GameObject[] fireballs;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();

        // Initialize the fireballs array
        fireballs = new GameObject[maxFireballs];
        for (int i = 0; i < maxFireballs; i++)
        {
            fireballs[i] = Instantiate(fireballPrefab);
            fireballs[i].SetActive(false);
        }
    }

    private void Update()
    {
        // Attack with mouse left click
        if (Input.GetMouseButton(0) && cooldownTimer > attackCooldown)
        {
            Attack();
        }

        // Check if the attack is on cooldown
        cooldownTimer += Time.deltaTime;
    }

    private void Attack()
    {
        // Set the cooldown timer to 0
        cooldownTimer = 0;

        // Play the attack animation
        GameObject fireball = fireballs[FindFireball()];
        fireball.transform.position = firePoint.position;
        fireball.GetComponent<Projectile>().SetDirection(Mathf.Sign(transform.localScale.x));
        fireball.SetActive(true);
    }

    private int FindFireball()
    {
        // Find an inactive fireball
        for (int i = 0; i < fireballs.Length; i++)
        {
            if (!fireballs[i].activeInHierarchy)
            {
                return i;
            }
        }
        
        return 0;
    }
}
