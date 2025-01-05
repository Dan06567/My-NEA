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

    // Reference to the Animator component
    private Animator animator;

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
            // Set the door's color based on its locked/unlocked status
            doorSprite.color = isUnlocked ? unlockedColor : lockedColor;
        }

        // If an Animator component is present, update the door's animation state
        if (animator != null)
        {
            animator.SetBool("Unlocked", isUnlocked);
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
        else
        {
            // Try to load the next level by index
            int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
            if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
            {
                SceneManager.LoadScene(nextSceneIndex);
            }
            else
            {
                // Log a warning if there is no next level available
                Debug.LogWarning("No next level available!");
            }
        }
    }
}
