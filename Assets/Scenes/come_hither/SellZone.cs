using UnityEngine;

public class SellZone : MonoBehaviour
{
    // Optional: Add a sound effect slot
    [SerializeField] private AudioSource sellSound;

    private void OnTriggerEnter(Collider other)
    {
        // 1. Check if the object entering the zone has the "SellableItem" script
        SellableItem item = other.GetComponent<SellableItem>();

        // If 'item' is not null, it means we found the script!
        if (item != null)
        {
            // 2. Add the money to the wallet
            if (WalletManager.Instance != null)
            {
                WalletManager.Instance.AddMoney(item.value);
                Debug.Log($"Sold {other.name} for ${item.value}");
            }

            // 3. Play sound if we have one
            if (sellSound != null)
            {
                sellSound.Play();
            }

            // 4. Make the object invisible (Destroy deletes it from the game entirely)
            Destroy(other.gameObject);
        }
    }
}