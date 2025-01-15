using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    // Door's sprite renderer
    [SerializeField] private SpriteRenderer doorSprite;

    // Color of the door when it is locked
    [SerializeField] private Color lockedColor = Color.red;

    // Color of the door when it is unlocked
    [SerializeField] private Color unlockedColor = Color.green;

    // Name of the next level scene to load when the door is entered
    [SerializeField] private string nextLevelName;

    // Flag to indicate if the door is unlocked
    private bool isUnlocked = false;


    private void Start()
    {

        // Update the door's visual state based on its locked/unlocked status
        UpdateDoorVisuals();
    }

    // Method to unlock the door
    public void UnlockDoor()
    {
        isUnlocked = true;
        // Update the door's visual state to reflect it is unlocked
        UpdateDoorVisuals();
    }

    // Update the door's visual appearance based on its locked/unlocked status
    private void UpdateDoorVisuals()
    {
        if (doorSprite != null)
        {
            // If the door is unlocked, set its color to the unlocked color, otherwise set it to the locked color
            if (isUnlocked)
            {
                doorSprite.color = unlockedColor;
            }
            else
            {
                doorSprite.color = lockedColor;
            }
        }
    }

    // Method called when another collider enters the door's trigger collider
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the colliding object is the player and the door is unlocked
        if (collision.CompareTag("Player") && isUnlocked)
        {
            // Load the next level
            LoadNextLevel();
        }
    }

    // Load the next level scene
    private void LoadNextLevel()
    {
        if (!string.IsNullOrEmpty(nextLevelName))
        {
            // Load the specified next level scene
            SceneManager.LoadScene(nextLevelName);
        }

    }
}
