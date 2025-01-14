using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] fireballs;
    [SerializeField] private AudioClip fireballSound; 

    private Animator anim;
    private PlayerMovement playerMovement;
    private AudioSource audioSource; // Added AudioSource reference
    private float cooldownTimer = Mathf.Infinity;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        if (anim == null)
        {
            Debug.LogError("Animator component is missing on the Player GameObject.");
        }

        playerMovement = GetComponent<PlayerMovement>();
        if (playerMovement == null)
        {
            Debug.LogError("PlayerMovement component is missing on the Player GameObject.");
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component is missing on the Player GameObject.");
        }
    }

    private void Update()
    {
        // Check for left mouse button press, cooldown, and if player can attack
        if (Input.GetMouseButton(0) && cooldownTimer > attackCooldown && playerMovement != null && playerMovement.canAttack())
        {
            Attack();
        }

        cooldownTimer += Time.deltaTime;
    }

    private void Attack()
    {
        // Play attack animation
        if (anim != null)
        {
            anim.SetTrigger("attack");
        }

        cooldownTimer = 0;

        // Find an available fireball and set its position and direction
        int fireballIndex = FindFireball();
        if (fireballIndex != -1) // Ensure a fireball is available
        {
            GameObject fireball = fireballs[fireballIndex];
            fireball.transform.position = firePoint.position;
            fireball.GetComponent<Projectile>().SetDirection(Mathf.Sign(transform.localScale.x));

            // Play fireball sound
            if (audioSource != null && fireballSound != null)
            {
                audioSource.PlayOneShot(fireballSound);
            }
        }
        else
        {
            Debug.LogWarning("No available fireballs to launch.");
        }
    }

    private int FindFireball()
    {
        // Return the index of the first inactive fireball
        for (int i = 0; i < fireballs.Length; i++)
        {
            if (!fireballs[i].activeInHierarchy)
            {
                return i;
            }
        }
        return -1; // Return -1 if no fireballs are available
    }
}
