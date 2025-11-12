using UnityEngine;

/// <summary>
/// This script handles the logic for a button that gives a
/// 10% chance of winning $5.
/// Attach this to your VR "Physical Button" interactable.
/// </summary>
public class BonusButton : MonoBehaviour
{
    [SerializeField]
    private int bonusAmount = 5;

    [SerializeField]
    [Range(0f, 1f)]
    private float chanceToWin = 0.1f; // 10% chance

    /// <summary>
    /// This is the public function you will call from your VR interaction.
    /// (e.g., UnityEvent, Oculus Interactable's OnSelect, etc.)
    /// </summary>
    public void TryForBonus()
    {
        // Check if the WalletManager exists
        if (WalletManager.Instance == null)
        {
            Debug.LogError("WalletManager.Instance is not found!");
            return;
        }

        // Roll the dice (0.0 to 1.0)
        float roll = Random.value;

        // If our roll (e.g., 0.07) is less than or equal to the chance (0.1)
        if (roll <= chanceToWin)
        {
            // --- Player WINS ---
            Debug.Log($"You win a ${bonusAmount} bonus!");
            WalletManager.Instance.AddMoney(bonusAmount);
            // Trigger "Win" sound/haptic
        }
        else
        {
            // --- Player LOSES ---
            Debug.Log("No bonus this time.");
            // Trigger "Click" or "Fail" sound/haptic
        }
    }
}