using UnityEngine;

/// <summary>
/// This script handles the logic for a simple $10 coin flip bet.
/// Attach this component to your VR "Coin Flip" button or interactable object.
/// </summary>
public class CoinFlipBet : MonoBehaviour
{
    // The bet amount is fixed at $10 as requested.
    private const int betAmount = 10;

    // The winnings are the bet amount * 2 (you get your $10 back + $10 prize)
    private const int winnings = betAmount * 2;

    /// <summary>
    /// This is the public function you will call from your VR interaction.
    /// (e.g., UnityEvent, Oculus Interactable's OnSelect, etc.)
    /// </summary>
    public void PlaceBet()
    {
        // First, check if the WalletManager exists
        if (WalletManager.Instance == null)
        {
            Debug.LogError("WalletManager.Instance is not found!");
            return;
        }

        // Try to spend the bet amount.
        bool betPlaced = WalletManager.Instance.TrySpendMoney(betAmount);

        // If the bet was successfully placed (player had enough money)
        if (betPlaced)
        {
            // Perform the coin flip (50/50 chance)
            // Random.value returns a float between 0.0 and 1.0
            if (Random.value > 0.5f)
            {
                // --- Player WINS ---
                Debug.Log("You WON the coin flip!");
                WalletManager.Instance.AddMoney(winnings);
                // You would trigger a "Win" animation or sound effect here
            }
            else
            {
                // --- Player LOSES ---
                Debug.Log("You LOST the coin flip!");
                // No money is returned, the bet is lost.
                // You would trigger a "Lose" animation or sound effect here
            }
        }
        else
        {
            // Player didn't have enough money.
            Debug.Log("Not enough money to place the $10 bet.");
            // You could trigger a "not enough money" sound here.
        }
    }
}