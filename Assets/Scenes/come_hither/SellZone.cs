using UnityEngine;

[RequireComponent(typeof(AudioSource))] // Ensures the zone has an AudioSource
public class SellZone : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("The sound to play when an item is sold.")]
    public AudioClip sellSound; // Changed from AudioSource to AudioClip for easier setup

    [Range(0f, 1f)]
    public float volume = 1.0f;

    private void OnTriggerEnter(Collider other)
    {
        // 1. Check if the object entering the zone has the "SellableItem" script
        SellableItem item = other.GetComponent<SellableItem>();

        if (item != null)
        {
            // 2. Add the money
            if (WalletManager.Instance != null)
            {
                WalletManager.Instance.AddMoney(item.value);
                Debug.Log($"Sold {other.name} for ${item.value}");
            }

            // 3. Play the Sound (The robust way)
            if (sellSound != null)
            {
                // PlayClipAtPoint creates a temporary object at this location
                // to play the sound. It won't get cut off if objects are destroyed.
                AudioSource.PlayClipAtPoint(sellSound, transform.position, volume);
            }

            // 4. Destroy the sold item
            Destroy(other.gameObject);
        }
    }
}