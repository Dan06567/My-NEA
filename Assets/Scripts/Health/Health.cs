using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float initialHealth;
    private float currentHealth;

    private void Awake()
    {
        currentHealth = initialHealth;

    }
    
    private void Damage(float thedamage)
    {
        currentHealth -= thedamage;
        currentHealth = Mathf.Max(0, currentHealth);

        if (currentHealth > 0 )
        {

        }
        else if (currentHealth <= 0)
        {

        }

    }
}
