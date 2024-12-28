using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed ; // Speed of the player movement
    [SerializeField] private float jumpPower ; // Force of the player's jump
    [SerializeField] private LayerMask groundLayer; // Layer used to identify the ground
    [SerializeField] private LayerMask wallLayer; // Layer used to identify walls

    private Rigidbody2D body; // Rigidbody2D component of the player
    private Animator anim; // Animator component to control animations
    private BoxCollider2D boxCollider; // BoxCollider2D for collision detection
    private float wallJumpCooldown; // Cooldown timer for wall jumping
    private float horizontalInput; // Horizontal movement input from the player


    // Constants for wall jumping and movement
    private const float wallJumpCooldownTime = 0.2f;
    private const float speedMultiplier = 1.7f;
    private const float gravityScale = 2f;

    private void Awake()
    {
        // Grab references for Rigidbody2D, Animator, and BoxCollider2D components
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        // Handle player input and animations
        InputMovement();
        HandleAnimation();
    }

    //fixed update for physics
    private void FixedUpdate()
    {
        // Handle player movement and wall jumping
        HandleMovement();
        HandleWallJump();
    }

    private void InputMovement()
    {
        horizontalInput = 0f; // horizontal input

        if (Input.GetKey(KeyCode.A))
        {
            horizontalInput = -0.5f; // Move left with reduced speed
        }
        else if (Input.GetKey(KeyCode.D))
        {
            horizontalInput = 0.5f; // Move right with reduced speed
        }

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            horizontalInput *= speedMultiplier; // Apply a speed multiplier
        }

        if (Input.GetKey(KeyCode.Space))
        {
            Jump();// Jump if the player presses the space key
        }
    }

    private void HandleMovement()
    {
        // Move the player horizontally based on the input
        body.linearVelocity = new Vector2(horizontalInput * speed, body.linearVelocity.y);

        // Flip the player sprite based on the movement direction
        if (horizontalInput > 0.01f)
        {
            transform.localScale = Vector3.one;
        }
        else if (horizontalInput < -0.01f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    //player animations
    private void HandleAnimation()
    {
        anim.SetBool("run", horizontalInput != 0);
        anim.SetBool("grounded", Grounded());
    }

    //wall jump
    private void HandleWallJump()
    {
        // Check if the player is touching a wall and not grounded
        if (wallJumpCooldown > wallJumpCooldownTime)
        {
            if (Wall() && !Grounded())
            {
                
                body.gravityScale = 0; // Disable gravity to allow wall jumping
                body.linearVelocity = Vector2.zero; // Reset the velocity
            }
            else
            {
                body.gravityScale = gravityScale; // Reset the gravity scale
            }
        }
        else
        {
            wallJumpCooldown += Time.deltaTime; // Increment the wall jump cooldown
        }
    }

    //jump
    private void Jump()
    {
        // Jump if the player is grounded
        if (Grounded())
        {
            body.linearVelocity = new Vector2(body.linearVelocity.x, jumpPower); // Apply jump force
            anim.SetTrigger("jump"); // Trigger the jump animation
        }
        // Wall jump if the player is touching a wall
        else if (Wall() && !Grounded())
        {
            if (horizontalInput == 0)
            {
                body.linearVelocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 10, 0); // Jump off
                transform.localScale = new Vector3(-Mathf.Sign(transform.localScale.x), transform.localScale.y, transform.localScale.z); // Flip the player
            }
            else
            {
                body.linearVelocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 3, 6); // Jump off
            }
            // Reset the wall jump cooldown
            wallJumpCooldown = 0;
        }
    }

    private bool Grounded()
    {
        // Check if the player is grounded using a boxcast
        RaycastHit2D raycastHit = Physics2D.BoxCast(
            boxCollider.bounds.center,
            boxCollider.bounds.size,
            0,
            Vector2.down,
            0.1f,
            groundLayer
        );
        // Return true if the boxcast hits the ground layer
        return raycastHit.collider != null;
    }

    private bool Wall()
    { // Check if the player is touching a wall using a boxcast
        RaycastHit2D raycastHit = Physics2D.BoxCast(
            boxCollider.bounds.center,
            boxCollider.bounds.size,
            0,
            new Vector2(transform.localScale.x, 0),
            0.1f,
            wallLayer
        );
        // Return true if the boxcast hits the wall layer
        return raycastHit.collider != null;
    }

}
