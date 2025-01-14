// Controls the behavior of projectiles fired by the player or enemies.
public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed;  // Speed at which the projectile moves
    private float direction;  // Direction of projectile movement (1 for right, -1 for left)
    private bool hit;  // Flag to track whether the projectile has hit something
    private float lifetime;  // Time before the projectile is deactivated

    private Animator anim;  // Animator for projectile's visual effects (e.g., explosion)
    private BoxCollider2D boxCollider;  // Collider to detect impacts

    private void Awake()
    {
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (hit) return;  // Ignore movement if projectile already hit something
        float movementSpeed = speed * Time.deltaTime * direction;  // Calculate movement speed based on direction
        transform.Translate(movementSpeed, 0, 0);  // Move projectile

        lifetime += Time.deltaTime;  // Increase lifetime
        if (lifetime > 5) gameObject.SetActive(false);  // Deactivate projectile after 5 seconds
    }

    // Handle collision with objects (e.g., enemies)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        hit = true;  // Mark as hit to stop movement
        boxCollider.enabled = false;  // Disable the collider to prevent further collisions
        anim.SetTrigger("explode");  // Trigger explosion animation
        if (collision.tag == "Enemy")  // If the projectile hits an enemy, deal damage
            collision.GetComponent<Health>().TakeDamage(1);
    }

    // Activate the projectile and set its movement direction
    public void SetDirection(float _direction)
    {
        lifetime = 0;  // Reset lifetime
        direction = _direction;  // Set direction (1 for right, -1 for left)
        gameObject.SetActive(true);  // Activate projectile
        hit = false;  // Reset hit flag
        boxCollider.enabled = true;  // Enable collider for future collisions

        // Flip the sprite to match the direction of travel
        float localScaleX = transform.localScale.x;
        if (Mathf.Sign(localScaleX) != _direction)
            localScaleX = -localScaleX;

        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
    }

    // Deactivate the projectile after it hits or its lifetime ends
    private void Deactivate()
    {
        gameObject.SetActive(false);  // Deactivate the projectile object
    }
}
