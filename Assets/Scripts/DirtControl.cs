using UnityEngine;

public class DirtControl : MonoBehaviour
{
    public float StampDelay = 1f;
    private int MaxStompLevel = 3;
    
    public bool canBeStomped;
    public bool grounded;
    public Collider2D playerCollider;
    public PlayerMovement player;
    private Animator animator;
    private Collider2D aCollider;
    private int stompLevel = 0;

    private Rigidbody2D body;

    // Use this for initialization
    void Start()
    {
        ResetStampCooldown();
        player = FindObjectOfType<PlayerMovement>();
        playerCollider = player.GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        aCollider = GetComponent<Collider2D>();
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Submit") && player.CanStomp())
        {
            player.OnStomp();
            if (IsCollidingPlayer() && canBeStomped && grounded)
            {
                Stomp();
            }
        }
    }

    private void Stomp()
    {
        canBeStomped = false;
        stompLevel++;
        animator.SetTrigger("press");
        if (stompLevel > MaxStompLevel)
        {
            canBeStomped = false;
            MakeSolid();
            return;
        }
        Invoke("ResetStampCooldown", StampDelay);
    }

    private void ResetStampCooldown()
    {
        canBeStomped = true;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            grounded = true;
            animator.SetBool("touchDown", true);
            MakeGround();
        }
    }

    private void MakeGround()
    {
        transform.position = new Vector3(normalize(transform.position.x), normalize(transform.position.y), transform.position.z);
        gameObject.tag = "Ground";
        gameObject.layer = LayerMask.NameToLayer("Ground");
        body.bodyType = RigidbodyType2D.Kinematic;
        body.freezeRotation = true;
        body.constraints = RigidbodyConstraints2D.FreezeAll;
        aCollider.isTrigger = true;
    }

    private void MakeSolid()
    {
        aCollider.isTrigger = false;
    }

    private bool IsCollidingPlayer()
    {
        return aCollider.IsTouching(playerCollider);
    }
    
    private float normalize(float x)
    {
        return Mathf.Ceil(x) - 0.5f;
    }
}