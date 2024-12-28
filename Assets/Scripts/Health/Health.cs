using UnityEditor;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float initialHealth; // Initial health of the player
    [SerializeField] private float invincibilityDuration = 1.0f; // Duration of invincibility in seconds
    [SerializeField] private Color damageColor = Color.red; // Color when taking damage
    public float currentHealth { get; private set; } // Current health of the player
    private bool dead; // Flag to determine if the player is dead
    private bool isInvincible; // Flag to determine if the player is invincible
    private float invincibilityTimer; // Timer for invincibility duration
    private Renderer playerRenderer; // Renderer component of the player
    private Color originalColor; // Original color of the player

    private void Awake() // Method to initialize the player health
    {
        currentHealth = initialHealth; // Set current health to initial health
        playerRenderer = GetComponent<Renderer>(); // Get the Renderer component
        if (playerRenderer != null) // Check if the Renderer component exists
        {
            originalColor = playerRenderer.material.color;  // Store the original color
        }
    }

    private void Update()
    {
        if (isInvincible) // Check if the player is invincible
        {
            invincibilityTimer -= Time.deltaTime; // Decrement the invincibility timer
            if (invincibilityTimer <= 0) // Check if invincibility has ended
            {
                isInvincible = false;   // Set invincibility flag to false
                ResetColor(); // Reset the player color
            }
        }
    }

    public void Damage(float thedamage) // Method to apply damage to the player
    {
        if (isInvincible || dead) // Check if the player is invincible or dead
        {
            return; // Exit the method early
        }

        currentHealth = Mathf.Clamp(currentHealth - thedamage, 0, initialHealth); // Apply damage to the player

        if (currentHealth > 0) // Check if the player is still alive
        {
            isInvincible = true; // Set invincibility flag to true
            invincibilityTimer = invincibilityDuration; // Set the invincibility timer
            ChangeColor(damageColor); // Change the player color
        }
        else if (currentHealth <= 0) // Check if the player has died
        {
            if (!dead) // Check if the player is not already dead
            {
                dead = true; // Set the dead flag to true
                GetComponent<PlayerMovement>().enabled = false; // Disable the PlayerMovement script
                ChangeColor(damageColor); // Change the player color
            }
        }
    }

    private void ChangeColor(Color color) // Method to change the player color
    {
        if (playerRenderer != null) // Check if the Renderer component exists
        {
            playerRenderer.material.color = color; // Change the player color
        }
    }

    private void ResetColor() // Method to reset the player color
    {
        if (playerRenderer != null) // Check if the Renderer component exists
        {
            playerRenderer.material.color = originalColor; // Reset the player color
        }
    }
}

  
