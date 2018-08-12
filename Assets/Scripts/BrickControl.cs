using UnityEngine;

public class BrickControl : MonoBehaviour
{
    public float StanDelay;
    private Animator animator;
    private bool hurts;
    private PlayerMovement player;
    private Rigidbody2D _body;
    private Collider2D _collider2D;

    // Use this for initialization
    void Start()
    {
        hurts = true;
        animator = GetComponent<Animator>();
        _body = GetComponent<Rigidbody2D>();
        _collider2D = GetComponent<Collider2D>();
        player = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") && hurts)
        {
            Invoke("DisablePhisics", 0.2f);
            
            hurts = false;
            animator.SetBool("destroy", true);
            player.Hurt(StanDelay);

            Destroy(gameObject, 1f);
        }

        if (other.gameObject.CompareTag("Ground"))
        {
            Invoke("DisablePhisics", 0.2f);
            
            hurts = false;
            animator.SetBool("destroy", true);
            
            Destroy(gameObject, 1f);
        }
    }

    private void DisablePhisics()
    {
        Destroy(_collider2D);
        Destroy(_body);
    }
}