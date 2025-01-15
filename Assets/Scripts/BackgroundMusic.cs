using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    [SerializeField] private AudioClip backgroundMusic; // The background music clip
    [SerializeField] private float volume = 0.5f; // Volume of the background music

    private AudioSource audioSource; // Reference to the AudioSource component

    private void Awake()
    {
        // Initialize the AudioSource component
        audioSource = gameObject.AddComponent<AudioSource>();
        // Set the background music clip, volume, and loop
        audioSource.clip = backgroundMusic;
        // Set the volume of the background music
        audioSource.volume = volume;
        audioSource.loop = true; // Loop the background music
    }

    private void Start()
    {
        // Play the background music
        audioSource.Play();
    }
}
