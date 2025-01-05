using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Basic Movement")]
    [SerializeField] private float speed; // Speed of the player
    [SerializeField] private float jumpPower; // Jump power of the player
    [SerializeField] private float minJumpMultiplier = 0.5f; // Minimum jump height multiplier

    [Header("Jump Settings")]
    [SerializeField] private int maxJumps = 2; // Maximum number of jumps allowed
    [SerializeField] private float coyoteTime = 0.2f; // Time window to jump after leaving ground
    [SerializeField] private float jumpBufferTime = 0.2f; // Time window to buffer jump input

    [Header("Wall Jump Settings")]
    [SerializeField] private float wallJumpForceX = 10f; // Horizontal force applied during wall jump
    [SerializeField] private float wallJumpForceY = 6f; // Vertical force applied during wall jump
    [SerializeField] private float wallSlideSpeed = 2f; // Speed of sliding down the wall

    [Header("Layer Masks")]
    [SerializeField] private LayerMask groundLayer; // Layer mask for ground detection
    [SerializeField] private LayerMask wallLayer; // Layer mask for wall detection

    private Rigidbody2D body; // Reference to the Rigidbody2D component
    private Animator anim; // Reference to the Animator component
    private BoxCollider2D boxCollider; // Reference to the BoxCollider2D component
    private float wallJumpCooldown; // Cooldown timer for wall jumps
    private float horizontalInput; // Horizontal input value
    private int remainingJumps; // Number of remaining jumps
    private float coyoteTimeCounter; // Counter for coyote time
    private float jumpBufferCounter; // Counter for jump buffer time
    private bool isJumping; // Flag to check if the player is jumping
    private bool wasGrounded; // Flag to check if the player was grounded

    private const float wallJumpCooldownTime = 0.2f; // Cooldown time for wall jumps
    private const float speedMultiplier = 1.7f; // Speed multiplier when running
    private const float gravityScale = 1f; // Gravity scale for the player

    private void Start()
    {
        // Log the player's initial position
        Debug.Log($"Player initial position: {transform.position}");
    }
    private void Awake()
    {
        // Initialize references to components
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        remainingJumps = maxJumps; // Set remaining jumps to max jumps
    }

    private void Update()
    {
        HandleCoyoteTime(); // Handle coyote time logic
        HandleJumpBuffer(); // Handle jump buffer logic
        InputMovement(); // Handle player input for movement
        HandleAnimation(); // Handle player animations
    }

    private void FixedUpdate()
    {
        HandleMovement(); // Handle player movement
        HandleWallJump(); // Handle wall jump logic
    }

    private void HandleCoyoteTime()
    {
        if (Grounded())
        {
            coyoteTimeCounter = coyoteTime; // Reset coyote time counter
            remainingJumps = maxJumps; // Reset remaining jumps
            wasGrounded = true; // Set wasGrounded to true
        }
        else
        {
            if (wasGrounded) coyoteTimeCounter -= Time.deltaTime; // Decrease coyote time counter
            wasGrounded = false; // Set wasGrounded to false
        }
    }

    private void HandleJumpBuffer()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpBufferCounter = jumpBufferTime; // Reset jump buffer counter
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime; // Decrease jump buffer counter
        }

        // Variable jump height
        if (Input.GetKeyUp(KeyCode.Space) && body.linearVelocity.y > 0)
        {
            body.linearVelocity = new Vector2(body.linearVelocity.x, body.linearVelocity.y * minJumpMultiplier); // Reduce jump height
            isJumping = false; // Set isJumping to false
        }
    }

    private void InputMovement()
    {
        horizontalInput = 0f; // Reset horizontal input

        if (Input.GetKey(KeyCode.A))
            horizontalInput = -0.5f; // Move left
        else if (Input.GetKey(KeyCode.D))
            horizontalInput = 0.5f; // Move right

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            horizontalInput *= speedMultiplier; // Increase speed when running

        if (jumpBufferCounter > 0f && (coyoteTimeCounter > 0f || remainingJumps > 0))
        {
            Jump(); // Perform jump
            jumpBufferCounter = 0f; // Reset jump buffer counter
        }
    }

    private void HandleMovement()
    {
        float targetSpeed = horizontalInput * speed; // Calculate target speed
        body.linearVelocity = new Vector2(targetSpeed, body.linearVelocity.y); // Set player velocity

        if (horizontalInput > 0.01f)
            transform.localScale = Vector3.one; // Face right
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-1, 1, 1); // Face left
    }

    private void HandleWallJump()
    {
        // Check if wall jump cooldown has expired
        if (wallJumpCooldown > wallJumpCooldownTime)
        {
            // Check if player is touching a wall and not grounded
            if (Wall() && !Grounded())
            {
                body.linearVelocity = new Vector2(body.linearVelocity.x, Mathf.Clamp(body.linearVelocity.y, -wallSlideSpeed, float.MaxValue)); // Slide down the wall
                remainingJumps = maxJumps; // Reset jumps when wall sliding
            }
        }
        else
        {
            wallJumpCooldown += Time.deltaTime; // Increase wall jump cooldown
        }
    }

    private void Jump()
    {
        // Check if player is touching a wall and not grounded
        if (Wall() && !Grounded())
        {
            WallJump(); // Perform wall jump
        }
        // Check if player is grounded or has remaining jumps
        else if (coyoteTimeCounter > 0f || remainingJumps > 0)
        {
            // Check if player is not grounded
            if (!Grounded())
                remainingJumps--; // Decrease remaining jumps

            body.linearVelocity = new Vector2(body.linearVelocity.x, jumpPower); // Set jump velocity
            anim.SetTrigger("jump"); // Trigger jump animation
            isJumping = true; // Set isJumping to true
            coyoteTimeCounter = 0f; // Reset coyote time counter
        }
    }

    private void WallJump()
    {
        float wallJumpDirection = -Mathf.Sign(transform.localScale.x); // Determine wall jump direction
        body.linearVelocity = new Vector2(wallJumpDirection * wallJumpForceX, wallJumpForceY); // Set wall jump velocity
        transform.localScale = new Vector3(-wallJumpDirection, 1, 1); // Flip player direction
        wallJumpCooldown = 0; // Reset wall jump cooldown
        remainingJumps = maxJumps - 1; // Decrease remaining jumps
    }

    private void HandleAnimation()
    {
        anim.SetBool("run", horizontalInput != 0); // Set run animation
        anim.SetBool("grounded", Grounded()); // Set grounded animation
    }

    private bool Grounded()
    {
        float groundCheckDistance = 0.1f; // Distance to check for ground
        Vector2 boxCenter = new Vector2(boxCollider.bounds.center.x, boxCollider.bounds.min.y); // Center of the box collider
        Vector2 boxSize = new Vector2(boxCollider.bounds.size.x * 0.9f, groundCheckDistance); // Size of the box collider

        Collider2D hit = Physics2D.OverlapBox(boxCenter, boxSize, 0f, groundLayer); // Check for ground collision

        // If we hit ground and were jumping, reset jump state
        if (hit != null && isJumping && body.linearVelocity.y <= 0)
        {
            isJumping = false; // Set isJumping to false
        }

        return hit != null; // Return if grounded
    }

    private bool Wall()
    {
        // Check if player is touching a wall
        RaycastHit2D raycastHit = Physics2D.BoxCast(
            boxCollider.bounds.center, // Center of the box collider
            boxCollider.bounds.size,   // Size of the box collider
            0,                         // No rotation
            new Vector2(transform.localScale.x, 0), // Direction based on player's facing direction
            0.1f,                      // Distance to cast the box
            wallLayer                  // Layer mask for wall detection
        );

        // Return true if a wall is detected, false otherwise
        return raycastHit.collider != null;
    }
}
