using UnityEngine;
using UnityEngine.UI;

public class LevelDisplay : MonoBehaviour
{
    [SerializeField] private Text levelNameText; // Reference to the Text component
    [SerializeField] private string levelName; // Name of the level

    private void Start()
    {
        // Set the level name text
        if (levelNameText != null)
        {
            levelNameText.text = levelName;
        }
    }
}
