using UnityEngine;
using UnityEngine.UI;
public class DoorManager : MonoBehaviour
{
    // Singleton instance of DoorManager
    public static DoorManager Instance { get; private set; }

    // Total number of keys required to unlock the door
    [SerializeField] private int totalKeysRequired;

    // Reference to the UI Text element that displays the key count
    [SerializeField] private Text keyCounterText;

    // Number of keys collected by the player
    private int keysCollected = 0;

    // Reference to the Door component
    private Door door;

    private void Awake()
    {
        // make sure is only one door manager in the scene
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Find the first Door object in the scene
        door = Object.FindFirstObjectByType<Door>();
        // Update the key counter UI text
        UpdateKeyCounterText();
    }

    // Method to be called when a key is collected
    public void CollectKey()
    {
        // Increment the number of keys collected
        keysCollected++;
        // Update the key counter UI text
        UpdateKeyCounterText();
        // Unlock the door if the required number of keys is collected
        if (keysCollected >= totalKeysRequired)
        {
            door.UnlockDoor();
        }
    }

    // Update the key counter UI text with the current key count
    private void UpdateKeyCounterText()
    {
        if (keyCounterText != null)
        {
            keyCounterText.text = $"Keys: {keysCollected}/{totalKeysRequired}";
        }
    }
}
