using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float startingHealth = 100f;  // The initial health value
    public float currentHealth { get; private set; }  // The current health value (read-only)
    private Animator anim;  // Reference to the Animator for triggering animations
    private bool dead;  // Flag to check if the player is dead

    [Header("iFrames")]
    [SerializeField] private float iFramesDuration;  // Duration of the invincibility (iFrames) after taking damage
    [SerializeField] private int numberOfFlashes;  // Number of flashes during invincibility
    private SpriteRenderer spriteRend;  // Reference to the SpriteRenderer to modify the player's appearance during invincibility
    private int heartsCollected = 0;  // Count of collected hearts for respawn
    [SerializeField] private int totalHeartsRequired = 3;  // Number of hearts required to re-enable movement

    private void Awake()
    {
        currentHealth = startingHealth;  // Set the player's initial health
        anim = GetComponent<Animator>();  // Get the Animator component attached to the player
        spriteRend = GetComponent<SpriteRenderer>();  // Get the SpriteRenderer component for changing player visuals
    }

    // Method to take damage and apply effects such as invincibility and death logic
    public void TakeDamage(float _damage)
    {
        if (!dead)
        {
            // Debug log for tracking damage and health
            Debug.Log($"Taking damage: {_damage}, current health: {currentHealth}");

            currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);  // Reduce health and clamp it within valid range
            
            anim.SetTrigger("hurt");  // Trigger the hurt animation
            StartCoroutine(Invulnerability());  // Start the invulnerability period after taking damage

            // Check if health has reached zero, triggering death
            if (currentHealth <= 0 && !dead)
            {
                anim.SetTrigger("die");  // Trigger the death animation

                // Disable certain components when the player dies (e.g., movement)
                if (GetComponent<PlayerMovement>() != null)
                    GetComponent<PlayerMovement>().enabled = false;

                if (GetComponentInParent<EnemyPatrol>() != null)
                    GetComponentInParent<EnemyPatrol>().enabled = false;

                if (GetComponentInParent<MeleeEnemy>() != null)
                    GetComponentInParent<MeleeEnemy>().enabled = false;

                dead = true;  // Mark the player as dead
            }
        }
    }

    // Method to add health to the player (e.g., after collecting hearts)
    public void AddHealth(float _value)
    {
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, startingHealth);  // Increase health, ensuring it doesn't exceed max

        heartsCollected++;  // Increment hearts collected
        if (heartsCollected >= totalHeartsRequired)  // If enough hearts are collected, re-enable movement
        {
            ReenableMovement();
        }
    }

    // Method to respawn the player (reset health, re-enable movement)
    public void Respawn()
    {
        dead = false;  // Set dead to false to allow movement
        AddHealth(startingHealth);  // Restore full health
        anim.ResetTrigger("die");  // Reset the die animation trigger
        anim.Play("Idle");  // Play idle animation
        StartCoroutine(Invulnerability());  // Start invulnerability after respawn

        // Re-enable PlayerMovement if it was disabled
        var playerMovement = GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            playerMovement.enabled = true;
        }

        // You can add more components to re-enable here
    }

    // Coroutine to disable movement briefly after death (optional)
    private IEnumerator DisableMovementAfterDeath()
    {
        yield return new WaitForSeconds(1f);  // Wait for 1 second after death
        // Disable movement when the player dies
        GetComponent<PlayerMovement>().enabled = false;
    }

    // Method to re-enable movement after collecting all hearts
    public void ReenableMovement()
    {
        if (dead)
        {
            dead = false;  // Set dead to false
            GetComponent<PlayerMovement>().enabled = true;  // Re-enable movement
            Debug.Log("Movement re-enabled after collecting all hearts.");
        }
    }

    // Coroutine to manage the invulnerability (iFrames) after taking damage
    private IEnumerator Invulnerability()
    {
        // Ignore collisions between specific layers (e.g., player and enemies)
        Physics2D.IgnoreLayerCollision(10, 11, true);
        for (int i = 0; i < numberOfFlashes; i++)
        {
            spriteRend.color = new Color(1, 0, 0, 0.5f);  // Make the player semi-transparent red during invincibility
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));  // Flash duration
            spriteRend.color = Color.white;  // Reset the player's color back to normal
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));  // Pause between flashes
        }
        // Resume normal collision detection after invincibility ends
        Physics2D.IgnoreLayerCollision(10, 11, false);
    }
}
