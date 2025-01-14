// Handles player movement, jumping, and wall interaction logic.
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;  // Movement speed of the player
    [SerializeField] private float jumpPower;  // Jumping force for the player
    [SerializeField] private LayerMask groundLayer;  // Layer for detecting the ground
    [SerializeField] private LayerMask wallLayer;  // Layer for detecting walls
    private Rigidbody2D body;  // Player's Rigidbody for physics-based movement
    private Animator anim;  // Animator to control player animations
    private BoxCollider2D boxCollider;  // Collider to detect ground and walls
    private float wallJumpCooldown;  // Timer for wall jump cooldown
    private float horizontalInput;  // Input for horizontal movement direction

    private void Awake()
    {
        // Initialize the references to components
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");  // Capture horizontal input for movement

        // Flip the player sprite based on movement direction
        if (horizontalInput > 0.01f)
            transform.localScale = Vector3.one;
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-1, 1, 1);

        // Update running animation and grounded status
        anim.SetBool("run", horizontalInput != 0);
        anim.SetBool("grounded", isGrounded());

        // Handle wall jump and regular jump mechanics
        if (wallJumpCooldown > 0.2f)
        {
            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

            if (onWall() && !isGrounded())  // Handle wall detection
            {
                body.gravityScale = 0;
                body.velocity = Vector2.zero;
            }
            else
                body.gravityScale = 7;

            if (Input.GetKey(KeyCode.Space))  // Perform jump
                Jump();
        }
        else
            wallJumpCooldown += Time.deltaTime;  // Increment cooldown
    }

    // Perform a regular or wall jump based on current state
    private void Jump()
    {
        if (isGrounded())  // Regular ground jump
        {
            body.velocity = new Vector2(body.velocity.x, jumpPower);
            anim.SetTrigger("jump");
        }
        else if (onWall() && !isGrounded())  // Wall jump logic
        {
            if (horizontalInput == 0)  // Jump off wall when no horizontal input
            {
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 10, 0);
                transform.localScale = new Vector3(-Mathf.Sign(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else  // Jump off wall while moving
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 3, 6);

            wallJumpCooldown = 0;
        }
    }

    // Check if the player is grounded by using a BoxCast
    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }

    // Detect if the player is touching a wall using a BoxCast
    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }

    // Check if the player can attack (must be grounded and not on a wall)
    public bool canAttack()
    {
        return horizontalInput == 0 && isGrounded() && !onWall();
    }
}
