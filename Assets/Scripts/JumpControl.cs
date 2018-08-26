using System.Collections;
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
    }

    void FixedUpdate()
    {
        TryJump();
    }

    void TryJump()
    {
        if (!Input.GetButton("Jump")) return;

        if (player.CanJump()) StartCoroutine(OnJump());
    }

    IEnumerator OnJump()
    {
        var jumpVector = Vector2.up * player.JumpPower;
        player.OnJump();
        body.velocity = Vector2.zero;
        float timer = 0f;
        while (timer < player.jumpTime && !player.grounded)
        {
            float proportionCompleted = timer / player.jumpTime;
            Vector2 thisFrameJumpVector = Vector2.Lerp(jumpVector, Vector2.zero, proportionCompleted);
            body.AddForce(thisFrameJumpVector * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }
        player.OnStopJumping();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            player.OnStopJumping();
        }
    }
}