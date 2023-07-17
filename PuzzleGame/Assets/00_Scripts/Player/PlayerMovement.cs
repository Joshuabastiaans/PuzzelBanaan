using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float MovementSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float climbSpeed = 5f;

    private Rigidbody2D rb2d;
    [SerializeField] private Animator animator;

    private bool isGrounded;
    public bool isClimbing;
    private bool isJumping;
    private bool isRunning;

    public Transform GroundCheck;
    public LayerMask GroundLayer;

    private PlayerInputSystem playerControls;

    private float movement;
    private float climbMovement;
    private float scaleX;

    private InputAction move;
    private InputAction jump;
    private InputAction climb;

    private AudioManager audioManager;
    private float gravityScaleAtStart;

    private void Awake()
    {
        playerControls = new PlayerInputSystem();
        rb2d = GetComponent<Rigidbody2D>();
        scaleX = transform.localScale.x;
        audioManager = FindObjectOfType<AudioManager>();
        gravityScaleAtStart = rb2d.gravityScale;
    }

    private void OnEnable()
    {
        move = playerControls.Player.Move;
        move.Enable();
        jump = playerControls.Player.Jump;
        jump.performed += Jump;
        jump.Enable();
        climb = playerControls.Player.Climb;
        climb.Enable();
    }

    private void OnDisable()
    {
        move.Disable();
        jump.Disable();
        climb.Disable();
    }

    private void Update()
    {
        movement = move.ReadValue<Vector2>().x;
        climbMovement = climb.ReadValue<Vector2>().x;
        Animation();
        Sound();
    }

    private void FixedUpdate()
    {
        rb2d.velocity = new Vector2(movement * MovementSpeed, rb2d.velocity.y);
        Flip();
        Climb();

        if (isGrounded && isJumping)
        {
            // Player has landed on the ground, stop the jump animation
            animator.SetBool("IsJumping", false);
            isJumping = false;
        }
        CheckIfGrounded();
    }

    public void Flip()
    {
        if (movement > 0)
        {
            transform.localScale = new Vector3(scaleX, transform.localScale.y, transform.localScale.z);
        }
        if (movement < 0)
        {
            transform.localScale = new Vector3((-1) * scaleX, transform.localScale.y, transform.localScale.z);
        }
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if (isGrounded)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);

            animator.SetBool("IsJumping", true);
            isJumping = true;
        }
    }

    public void CheckIfGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(GroundCheck.position, GroundCheck.GetComponent<CircleCollider2D>().radius, GroundLayer);
    }

    private void Animation()
    {
        if (Mathf.Abs(rb2d.velocity.x) >= 0.05f)
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }
        animator.SetBool("IsRunning", isRunning);

    }

    private void Sound()
    {
        if (isRunning && isGrounded)
        {
            if (!audioManager.IsPlaying("Running"))
            {
                audioManager.Play("Running");
            }
        }
        else
        {
            if (audioManager.IsPlaying("Running"))
            {
                audioManager.Stop("Running");
            }
        }
    }

    private void Climb()
    {
        if (isClimbing)
        {
            rb2d.gravityScale = 0;
            rb2d.velocity = new Vector2(rb2d.velocity.x, climbMovement * climbSpeed);
        }
        else
        {
            rb2d.gravityScale = gravityScaleAtStart;
        }
    }
}
