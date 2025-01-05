using UnityEngine;
using TMPro;

public class KeyManager : MonoBehaviour
{
    public static KeyManager Instance { get; private set; }

    [Header("Key Settings")]
    [SerializeField] private int totalKeysRequired = 3;
    [SerializeField] private TextMeshProUGUI keyCountText;
    [SerializeField] private Door door;


    private int keysCollected = 0;


    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }


    }

    private void Start()
    {
        UpdateKeyUI();
    }

    public void CollectKey()
    {
        keysCollected++;

        // Play collect sound
        if (audioSource != null && keyCollectSound != null)
        {
            audioSource.PlayOneShot(keyCollectSound);
        }

        UpdateKeyUI();

        // Check if all keys are collected
        if (keysCollected >= totalKeysRequired)
        {
            UnlockDoor();
        }
    }

    private void UpdateKeyUI()
    {
        if (keyCountText != null)
        {
            keyCountText.text = $"Keys: {keysCollected}/{totalKeysRequired}";
        }
    }

    private void UnlockDoor()
    {
        if (door != null)
        {
            door.UnlockDoor();

            
        }
    }
}
