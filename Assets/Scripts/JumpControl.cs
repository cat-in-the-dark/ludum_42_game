using System.Security.Cryptography;
using UnityEngine;

public class JumpControl : MonoBehaviour
{
    private Animator animator;
    private bool canJump = true;
    private Rigidbody2D body;
    
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
        TryJump();
    }

    void TryJump()
    {
        if (!Input.GetButton("Jump")) return;

        if (canJump && !playerMovement.IsMoving())
        {
            OnJump();
        }
    }

    void OnJump()
    {
        animator.SetBool("playerJumping", true);
        body.AddForce(Vector2.up * jumpPower);
        canJump = false;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            Debug.LogFormat("Touch {0}", other);
            OnStopJumping();
        }
    }

    private void OnStopJumping()
    {
        animator.SetBool("playerJumping", false);
        canJump = true;
    }
}