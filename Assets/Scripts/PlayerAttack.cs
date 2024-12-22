using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] fireballs;

    private Animator anim;
    private PlayerMovement playerMovement;
    private float cooldownTimer = Mathf.Infinity;

    private void Awake()
        //finds the animator and player movement
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        //attack with mouse left click
        if (Input.GetMouseButton(0) && cooldownTimer > attackCooldown )
        {

            Attack();
        }
       //checks if the attack is on cooldown
        cooldownTimer += Time.deltaTime;
    }
    //attack method
    private void Attack()
    {
        //sets the cooldown timer to 0
        cooldownTimer = 0;

        //plays the attack animation 
        fireballs[FindFireball()].transform.position = firePoint.position;
        fireballs[FindFireball()].GetComponent<Projectile>().SetDirection(Mathf.Sign(transform.localScale.x));
    }

   
    private int FindFireball()
    {
        //finds an inactive fireball (because is an array of 10 fireballs)
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