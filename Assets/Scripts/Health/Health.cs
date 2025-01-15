using UnityEditor;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float initialHealth; // Initial health of the player
    [SerializeField] private float invincibilityDuration = 1.0f; // Duration of invincibility in seconds
    [SerializeField] private Color damageColor = Color.red; // Color when taking damage
    // Current health of the player , get is used to access the value of currentHealth
    // set is used to modify the value of currentHealth but only within the class because is private
    public float currentHealth { get; private set; } 
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
        //The Mathf.Clamp function ensures that the resulting health value is clamped between 0 
        //and the initial health (initialHealth), preventing the health from going below 0 or above the initial health.
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
                Respawn(); // Call the Respawn method
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

    public void Respawn() // Method to respawn the player
    {
        currentHealth = initialHealth; // Reset the player's health
        dead = false; // Reset the dead flag
        GetComponent<PlayerMovement>().enabled = true; // Enable the PlayerMovement script
        ResetColor(); // Reset the player color
        transform.position = GetComponent<PlayerMovement>().respawnPoint; // Reset the player's position to the respawn point
    }
}

  
