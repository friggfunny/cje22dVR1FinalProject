using UnityEngine;
using TMPro;

[RequireComponent(typeof(AudioSource))] // Automatically adds an AudioSource to the object
public class CoinFlipBet : MonoBehaviour
{
    // The bet amount is fixed at $10
    private const int betAmount = 10;

    // You get your $10 back + $10 prize
    private const int winnings = betAmount * 2;

    [Header("Audio Settings")]
    [Tooltip("Sound to play when the player wins.")]
    public AudioClip winSound;
    [Tooltip("Sound to play when the player loses.")]
    public AudioClip loseSound;

    [Range(0f, 1f)]
    [Tooltip("Volume of the sound effects (0 is silent, 1 is full volume).")]
    public float soundVolume = 0.5f;

    // Private reference to the speaker on this object
    private AudioSource audioSource;

    [Header("UI Settings")]
    [Tooltip("Drag a TextMeshPro object here to see WIN/LOSE appear in the world.")]
    public TMP_Text resultText;

    [Tooltip("Drag a TextMeshPro object here to display the current active streak.")]
    public TMP_Text currentStreakText;

    [Tooltip("Drag a TextMeshPro object here to display the longest streak.")]
    public TMP_Text streakText;

    // Streak Tracking Variables
    private int currentStreak = 0;
    private bool currentStreakIsWin = false; // true = winning streak, false = losing streak
    private int longestStreak = 0;
    private bool longestStreakIsWin = false;

    private void Start()
    {
        // Get the AudioSource component attached to this object
        audioSource = GetComponent<AudioSource>();

        // 1. CLEAR THE TEXT ON START
        if (resultText != null) resultText.text = "";

        // Initialize Streak Texts
        if (streakText != null) streakText.text = "Longest Streak: 0";
        if (currentStreakText != null) currentStreakText.text = "Current Streak: 0";

        // DIAGNOSTIC: Check the wallet as soon as the game starts.
        if (WalletManager.Instance != null)
        {
            Debug.Log($"[CoinFlipBet] Connected to Wallet. Starting Money: ${WalletManager.Instance.CurrentMoney}");
        }
        else
        {
            Debug.LogError("[CoinFlipBet] WalletManager NOT found in scene!");
        }
    }

    // This adds a "Right Click -> Test Bet" option in the Inspector
    [ContextMenu("Test Place Bet")]
    public void PlaceBet()
    {
        Debug.Log("Attempting to place bet...");

        // FAILSAFE: If the game isn't running (Instance is null), try to find it manually.
        WalletManager wallet = WalletManager.Instance;
        if (wallet == null)
        {
            wallet = FindObjectOfType<WalletManager>();
        }

        // 1. Check for Wallet
        if (wallet == null)
        {
            Debug.LogError("CoinFlipBet: WalletManager not found! Make sure you have a 'GameManager' object with the WalletManager script attached.");
            UpdateResultText("Error: No Wallet");
            return;
        }

        // 2. Check for Money
        bool betPlaced = wallet.TrySpendMoney(betAmount);

        if (betPlaced)
        {
            // 3. Flip the Coin
            float roll = Random.value;
            Debug.Log($"Bet placed! Rolled: {roll}");

            if (roll > 0.5f)
            {
                // --- WIN ---
                Debug.Log("CoinFlip: WINNER!");
                wallet.AddMoney(winnings);
                UpdateResultText("WINNER! (+$10)");
                UpdateStreak(true); // Update streak logic (Win)

                // Play Win Sound
                if (winSound != null && audioSource != null)
                {
                    audioSource.pitch = Random.Range(0.8f, 1.2f);
                    audioSource.PlayOneShot(winSound, soundVolume);
                }
            }
            else
            {
                // --- LOSE ---
                Debug.Log("CoinFlip: Lost.");
                UpdateResultText("LOST (-$10)");
                UpdateStreak(false); // Update streak logic (Loss)

                // Play Lose Sound
                if (loseSound != null && audioSource != null)
                {
                    audioSource.pitch = Random.Range(0.8f, 1.2f);
                    audioSource.PlayOneShot(loseSound, soundVolume);
                }
            }
        }
        else
        {
            Debug.Log("CoinFlip: Not enough money.");
            UpdateResultText("Broke!");
        }
    }

    private void UpdateStreak(bool isWin)
    {
        // 1. Calculate Current Streak
        if (currentStreak == 0)
        {
            // First game ever
            currentStreak = 1;
            currentStreakIsWin = isWin;
        }
        else if (currentStreakIsWin == isWin)
        {
            // The streak continues!
            currentStreak++;
        }
        else
        {
            // Streak broken, reset to 1
            currentStreak = 1;
            currentStreakIsWin = isWin;
        }

        // 2. Update Current Streak UI
        if (currentStreakText != null)
        {
            string currentType = currentStreakIsWin ? "WINS" : "LOSSES";
            currentStreakText.text = $"Current Streak: {currentStreak}";
        }

        // 3. Check for High Score (Longest Streak)
        if (currentStreak > longestStreak)
        {
            longestStreak = currentStreak;
            longestStreakIsWin = currentStreakIsWin;

            // Update Longest Streak UI
            if (streakText != null)
            {
                string type = longestStreakIsWin ? "WINS" : "LOSSES";
                streakText.text = $"Longest Streak: {longestStreak} ({type})";
            }
        }
    }

    private void UpdateResultText(string message)
    {
        if (resultText != null)
        {
            resultText.text = message;
            if (Application.isPlaying)
            {
                Invoke(nameof(ClearText), 2f);
            }
        }
    }

    private void ClearText()
    {
        if (resultText != null) resultText.text = "";
    }
}