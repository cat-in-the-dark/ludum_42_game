using UnityEngine;

public class JumpControl : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D body;
    public bool grounded = true;

    public float lastJumpAt = float.MaxValue;
    public float jumpDelay = 1f;
    public float jumpPower = 1f;
    private PlayerMovement playerMovement;

    // Use this for initialization
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        lastJumpAt += Time.deltaTime;
        TryJump();
        if (grounded)
        {
            OnStopJumping();
        }
    }

    void TryJump()
    {
        if (!Input.GetButton("Jump")) return;

        if (CanJump())
        {
            OnJump();
        }
    }

    void OnJump()
    {
        lastJumpAt = 0;
        grounded = false;
        animator.SetBool("playerJumping", true);
        body.AddForce(Vector2.up * jumpPower);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            grounded = true;
        }
    }

    private void OnStopJumping()
    {
        animator.SetBool("playerJumping", false);
    }

    private bool CanJump()
    {
        return grounded && (lastJumpAt >= jumpDelay);
    }
}