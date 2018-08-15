using System;
using UnityEngine;

public class JumpControl : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D body;
    private Player player;

    // Use this for initialization
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        player = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        player.lastJumpAt += Time.deltaTime;
        TryJump();
    }

    void TryJump()
    {
        if (!Input.GetButton("Jump")) return;

        if (player.CanJump()) OnJump();
    }

    void OnJump()
    {
        player.OnJump();
        body.AddForce(Vector2.up * player.JumpPower);
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        Debug.Log(other.gameObject.tag);
        if (other.gameObject.CompareTag("Ground"))
        {
            OnStopJumping();
        }
    }

    private void OnStopJumping()
    {
        player.OnStopJumping();
    }
}