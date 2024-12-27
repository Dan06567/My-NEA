using UnityEditor;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float initialHealth;
    public float currentHealth { get; private set; }

    private void Awake()
    {
        currentHealth = initialHealth;
    }

    private void Damage(float thedamage)
    {
        currentHealth -= thedamage;
        currentHealth = Mathf.Max(0, currentHealth);

        if (currentHealth > 0)
        {

        }
        else if (currentHealth <= 0)
        {

        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            Damage(1);
        }
    }
}


