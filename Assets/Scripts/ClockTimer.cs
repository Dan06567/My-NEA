using UnityEngine;
using UnityEngine.UI;

public class ClockTimer : MonoBehaviour
{
    // Reference to the Text component to display the time
    private Text clockText;

    // Timer variables
    private float timer;
    private bool isRunning;

    // Called when the script instance is being loaded
    private void Awake()
    {
        // Get the Text component attached to the same GameObject
        clockText = GetComponent<Text>();
        timer = 0f;
        isRunning = true;
    }

    // Called once per frame
    private void Update()
    {
        if (isRunning)
        {
            // Update the timer
            timer += Time.deltaTime;

            // Convert timer to minutes and seconds
            int minutes = Mathf.FloorToInt(timer / 60F);
            int seconds = Mathf.FloorToInt(timer % 60F);

            // Update the text to show the timer in mm:ss format
            clockText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    // Method to start the timer
    public void StartTimer()
    {
        isRunning = true;
    }

    // Method to stop the timer
    public void StopTimer()
    {
        isRunning = false;
    }

    // Method to reset the timer
    public void ResetTimer()
    {
        timer = 0f;
        clockText.text = "00:00";
        
    }
}
