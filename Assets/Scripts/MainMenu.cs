using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    // Reference to the buttons
    public Button newUserButton;
    public Button loadUserButton;
    public Button quitGameButton;
    public Button settingsButton;

    // Called when the script instance is being loaded
    private void Awake()
    {
        // Assign button click listeners
        newUserButton.onClick.AddListener(OnNewUserButtonClicked);
        loadUserButton.onClick.AddListener(OnLoadUserButtonClicked);
        quitGameButton.onClick.AddListener(OnQuitGameButtonClicked);
        settingsButton.onClick.AddListener(OnSettingsButtonClicked);
    }

    // Method called when the new user button is clicked
    private void OnNewUserButtonClicked()
    {
        // Load the scene for creating a new user
        SceneManager.LoadScene("NewUserScene");
    }

    // Method called when the load user button is clicked
    private void OnLoadUserButtonClicked()
    {
        // Load the scene for loading an existing user
        SceneManager.LoadScene("LoadUserScene");
    }

    // Method called when the quit game button is clicked
    private void OnQuitGameButtonClicked()
    {
        // Quit the application
        Application.Quit();
    }

    // Method called when the settings button is clicked
    private void OnSettingsButtonClicked()
    {
        // Load the settings scene
        SceneManager.LoadScene("SettingsScene");
    }
}
