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
        InputMovement();
        HandleAnimation();
    }

    private void FixedUpdate()
    {
        HandleMovement();
        HandleWallJump();
    }

    private void InputMovement()
    {
        horizontalInput = 0f;

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
            Jump();
        }
    }

    private void HandleMovement()
    {
        body.linearVelocity = new Vector2(horizontalInput * speed, body.linearVelocity.y);

        if (horizontalInput > 0.01f)
        {
            transform.localScale = Vector3.one;
        }
        else if (horizontalInput < -0.01f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    private void HandleAnimation()
    {
        anim.SetBool("run", horizontalInput != 0);
        anim.SetBool("grounded", Grounded());
    }

    private void HandleWallJump()
    {
        if (wallJumpCooldown > wallJumpCooldownTime)
        {
            if (Wall() && !Grounded())
            {
                body.gravityScale = 0;
                body.linearVelocity = Vector2.zero;
            }
            else
            {
                body.gravityScale = gravityScale;
            }
        }
        else
        {
            wallJumpCooldown += Time.deltaTime;
        }
    }

    private void Jump()
    {
        if (Grounded())
        {
            body.linearVelocity = new Vector2(body.linearVelocity.x, jumpPower);
            anim.SetTrigger("jump");
        }
        else if (Wall() && !Grounded())
        {
            if (horizontalInput == 0)
            {
                body.linearVelocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 10, 0);
                transform.localScale = new Vector3(-Mathf.Sign(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else
            {
                body.linearVelocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 3, 6);
            }

            wallJumpCooldown = 0;
        }
    }

    private bool Grounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(
            boxCollider.bounds.center,
            boxCollider.bounds.size,
            0,
            Vector2.down,
            0.1f,
            groundLayer
        );
        return raycastHit.collider != null;
    }

    private bool Wall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(
            boxCollider.bounds.center,
            boxCollider.bounds.size,
            0,
            new Vector2(transform.localScale.x, 0),
            0.1f,
            wallLayer
        );
        return raycastHit.collider != null;
    }

}
