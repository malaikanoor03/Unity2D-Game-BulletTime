// Controls the behavior of an arrow trap that fires projectiles periodically.
public class ArrowTrap : MonoBehaviour
{
    [SerializeField] private float attackCooldown;  // Cooldown between consecutive attacks
    [SerializeField] private Transform firePoint;  // Location where arrows will be fired
    [SerializeField] private GameObject[] arrows;  // Array of arrow objects to be used
    private float cooldownTimer;  // Tracks the cooldown time for firing
    private GameObject firetrap;  // Reference to the firetrap object itself

    private void Start()
    {
        // Find the firetrap object by its tag
        firetrap = GameObject.FindWithTag("Enemy");

        if (firetrap == null)
        {
            Debug.LogWarning("Firetrap object not found!");  // Warn if firetrap is not found
        }
    }

    // Fires an arrow when the cooldown timer expires
    private void Attack()
    {
        cooldownTimer = 0;  // Reset the cooldown
        arrows[FindArrows()].transform.position = firePoint.position;  // Set the arrow's spawn position
        arrows[FindArrows()].GetComponent<EnemyProjectile>().activateProjectile();  // Activate the arrow

        // Log the firetrap's status
        if (firetrap != null)
        {
            if (!firetrap.activeInHierarchy)
            {
                Debug.Log("Firetrap is deactivated.");
            }
            else
            {
                Debug.Log("Firetrap is active.");
            }
        }
    }

    // Find an inactive arrow from the array
    private int FindArrows()
    {
        for (int i = 0; i < arrows.Length; i++)
        {
            if (!arrows[i].activeInHierarchy)
                return i;  // Return the index of the first inactive arrow
        }
        return 0;  // Default to the first arrow if none are inactive
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime;  // Increment cooldown timer
        if (cooldownTimer >= attackCooldown)
            Attack();  // Fire the arrow if the cooldown is complete

        // Reactivate firetrap if it was deactivated
        if (firetrap != null && !firetrap.activeInHierarchy)
        {
            Debug.Log("Firetrap was deactivated. Reactivating...");
            firetrap.SetActive(true);  // Reactivate the firetrap
        }
    }
}
