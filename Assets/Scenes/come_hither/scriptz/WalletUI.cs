using UnityEngine;
using TMPro; // You need this to control TextMeshPro text!

/// <summary>
/// This script listens to the WalletManager and updates a UI text element
/// to display the player's current money.
/// Attach this script to your TextMeshPro UI object.
/// </summary>
public class WalletUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text moneyText; // The UI text element to update

    // OnEnable is called when the object becomes active.
    // It's a great place to "subscribe" to events.
    private void OnEnable()
    {
        // Tell the WalletManager: "Hey, when OnMoneyChanged fires,
        // please also call my UpdateMoneyText function."
        WalletManager.OnMoneyChanged += UpdateMoneyText;

        // Also, when this UI first turns on, it needs to get the *current*
        // money value, just in case the game is already in progress.
        if (WalletManager.Instance != null)
        {
            // We manually call the update function with the current money.
            UpdateMoneyText(WalletManager.Instance.CurrentMoney);
        }
    }

    // OnDisable is called when the object is turned off or destroyed.
    // It's CRITICAL to "unsubscribe" from events to prevent errors.
    private void OnDisable()
    {
        // Tell the WalletManager: "I don't need to listen to
        // your OnMoneyChanged event anymore."
        WalletManager.OnMoneyChanged -= UpdateMoneyText;
    }

    /// <summary>
    /// This function is called by the WalletManager event.
    /// It receives the new money total and updates the text.
    /// </summary>
    /// <param name="newAmount">The new amount of money.</param>
    private void UpdateMoneyText(int newAmount)
    {
        // We check if the text object exists first to avoid errors.
        if (moneyText != null)
        {
            // Format the text to include a dollar sign!
            moneyText.text = $"${newAmount}";
        }
    }
}