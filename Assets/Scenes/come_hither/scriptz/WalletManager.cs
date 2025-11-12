using UnityEngine;
using System; // Required for the 'Action' event

/// <summary>
/// This is a Singleton script to manage the player's wallet.
/// It can be accessed from any other script to add or spend money.
/// Place this on a persistent GameObject (like a "GameManager").
/// </summary>
public class WalletManager : MonoBehaviour
{
    // --- Singleton Pattern ---
    // This makes it so we can easily access the wallet from any script
    // using "WalletManager.Instance"
    public static WalletManager Instance { get; private set; }

    /// <summary>
    /// This event fires whenever the money value changes.
    /// UI scripts can "listen" to this event to update their text.
    /// </summary>
    public static event Action<int> OnMoneyChanged;

    [SerializeField]
    private int currentMoney = 100; // Starting money

    // Public getter to read the current money
    public int CurrentMoney => currentMoney;

    private void Awake()
    {
        // Implement the Singleton pattern
        if (Instance != null && Instance != this)
        {
            // If another instance already exists, destroy this one.
            Destroy(gameObject);
        }
        else
        {
            // This is the one and only instance.
            Instance = this;
            // Optional: Keep this object alive when loading new scenes.
            // DontDestroyOnLoad(gameObject); 
        }
    }

    private void Start()
    {
        // Fire the event on start so any UI can grab the initial value.
        OnMoneyChanged?.Invoke(currentMoney);
    }

    /// <summary>
    /// Tries to spend a specified amount of money.
    /// </summary>
    /// <param name="amountToSpend">The amount to try and spend.</param>
    /// <returns>True if the money was spent, false if not enough money.</returns>
    public bool TrySpendMoney(int amountToSpend)
    {
        if (amountToSpend > currentMoney)
        {
            // Not enough money
            Debug.Log("Not enough money!");
            return false;
        }

        // We have enough money, so subtract it.
        currentMoney -= amountToSpend;
        Debug.Log($"Spent {amountToSpend}. Remaining: {currentMoney}");

        // Fire the event to notify listeners (like UI) that money has changed.
        OnMoneyChanged?.Invoke(currentMoney);
        return true;
    }

    /// <summary>
    /// Adds a specified amount of money to the wallet.
    /// </summary>
    /// <param name="amountToAdd">The amount to add.</param>
    public void AddMoney(int amountToAdd)
    {
        currentMoney += amountToAdd;
        Debug.Log($"Added {amountToAdd}. New total: {currentMoney}");

        // Fire the event to notify listeners.
        OnMoneyChanged?.Invoke(currentMoney);
    }
}