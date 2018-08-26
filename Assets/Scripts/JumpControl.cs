using UnityEngine;

public class JumpControl : MonoBehaviour
{
    private Rigidbody2D body;
    private Player player;

    // Use this for initialization
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
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
        var force = Vector2.up * player.JumpPower;
        Debug.Log(force);
        body.AddForce(force, ForceMode2D.Impulse);
        player.OnJump();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log(other.gameObject.tag);
        if (other.gameObject.CompareTag("Ground"))
        {
            player.OnStopJumping();
        }
    }
}