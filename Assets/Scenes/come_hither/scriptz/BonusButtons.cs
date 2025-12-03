using UnityEngine;

[RequireComponent(typeof(AudioSource))] // Automatically adds an AudioSource to the object
public class BonusButton : MonoBehaviour
{
    [Header("Game Settings")]
    [SerializeField]
    private int bonusAmount = 5;

    [SerializeField]
    [Range(0f, 1f)]
    private float chanceToWin = 0.1f; // 10% chance

    [Header("Audio Settings")]
    [Tooltip("Sound to play when the player wins the bonus.")]
    public AudioClip winSound;
    [Tooltip("Sound to play when the player does not win.")]
    public AudioClip loseSound;

    [Range(0f, 1f)]
    [Tooltip("Volume of the sound effects (0 is silent, 1 is full volume).")]
    public float soundVolume = 0.5f;

    // Private reference to the speaker on this object
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // Ensure PlayOnAwake is off so it doesn't beep when the scene loads
        if (audioSource != null)
        {
            audioSource.playOnAwake = false;
        }
    }

    /// <summary>
    /// This is the public function you will call from your VR interaction.
    /// (e.g., UnityEvent, Oculus Interactable's OnSelect, etc.)
    /// </summary>
    [ContextMenu("Test Bonus Button")]
    public void TryForBonus()
    {
        // FAILSAFE: If the game isn't running (Instance is null), try to find it manually.
        WalletManager wallet = WalletManager.Instance;
        if (wallet == null)
        {
            wallet = FindObjectOfType<WalletManager>();
        }

        // Check if the WalletManager exists
        if (wallet == null)
        {
            Debug.LogError("BonusButton: WalletManager is not found! Make sure you have a GameManager.");
            return;
        }

        // Roll the dice (0.0 to 1.0)
        float roll = Random.value;

        // If our roll (e.g., 0.07) is less than or equal to the chance (0.1)
        if (roll <= chanceToWin)
        {
            // --- Player WINS ---
            Debug.Log($"You win a ${bonusAmount} bonus!");
            wallet.AddMoney(bonusAmount);

            PlaySound(winSound);
        }
        else
        {
            // --- Player LOSES ---
            Debug.Log("No bonus this time.");

            PlaySound(loseSound);
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            // Randomize pitch slightly (0.8 to 1.2) so overlapping sounds are distinct
            audioSource.pitch = Random.Range(0.8f, 1.2f);
            // Play the sound with the specific volume
            audioSource.PlayOneShot(clip, soundVolume);
        }
    }
}