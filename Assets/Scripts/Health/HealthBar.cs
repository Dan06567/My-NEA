using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Health playerHealth; // Reference to the player's health script
    [SerializeField] private Image totalHealthBar; // Reference to the total health bar image
    [SerializeField] private Image currentHealthBar; // Reference to the current health bar image


    private void Start() // Method to initialize the health bars
    {

        totalHealthBar.fillAmount = playerHealth.currentHealth / 10; // Set the total health bar fill amount

    }

    private void Update() // Method to update the current health bar
    {
        
        currentHealthBar.fillAmount = playerHealth.currentHealth / 10; // Set the current health bar fill amount
    }
        
        
}
