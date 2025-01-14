using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    [Header("Attack Parameters")]
    [SerializeField] private float attackCooldown;  // Time between consecutive attacks
    [SerializeField] private float range;  // Range of the melee attack
    [SerializeField] private int damage;  // Amount of damage dealt by the melee attack

    [Header("Collider Parameters")]
    [SerializeField] private float colliderDistance;  // Distance to cast the collider for detecting the player
    [SerializeField] private BoxCollider2D boxCollider;  // BoxCollider2D for the melee attack area

    [Header("Player Layer")]
    [SerializeField] private LayerMask playerLayer;  // LayerMask to specify which objects are considered "players"
    private float cooldownTimer = Mathf.Infinity;  // Timer to manage attack cooldown

    // References to other components for animations, health, and patrol
    private Animator anim;
    private Health playerHealth;
    private EnemyPatrol enemyPatrol;

    private void Awake()
    {
        // Initialize references to animator and enemy patrol script
        anim = GetComponent<Animator>();
        enemyPatrol = GetComponentInParent<EnemyPatrol>();
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime;  // Increment cooldown timer based on time passed

        // Check if the player is in sight and cooldown is finished
        if (PlayerInSight())
        {
            if (cooldownTimer >= attackCooldown)  // If the attack is ready to execute
            {
                cooldownTimer = 0;  // Reset cooldown timer
                anim.SetTrigger("meleeAttack");  // Trigger melee attack animation
            }
        }

        // Disable enemy patrol if player is in sight, otherwise continue patrolling
        if (enemyPatrol != null)
            enemyPatrol.enabled = !PlayerInSight();
    }

    // Check if the player is within the attack range using a BoxCast
    private bool PlayerInSight()
    {
        // Perform a BoxCast to detect if the player is in front of the enemy
        RaycastHit2D hit = Physics2D.BoxCast(
            boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, 0, playerLayer);  // Cast box in front of the enemy to detect the player

        if (hit.collider != null)  // If player is hit by the BoxCast
            playerHealth = hit.transform.GetComponent<Health>();  // Get the player's health component

        return hit.collider != null;  // Return true if a player was detected, false otherwise
    }

    // Visualize the attack range in the editor using Gizmos
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;  // Set Gizmo color to red for visibility
        Gizmos.DrawWireCube(
            boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));  // Draw attack range box
    }

    // Deal damage to the player if they are in sight
    private void DamagePlayer()
    {
        if (PlayerInSight())  // If player is within attack range
            playerHealth.TakeDamage(damage);  // Apply damage to the player's health
    }
}
