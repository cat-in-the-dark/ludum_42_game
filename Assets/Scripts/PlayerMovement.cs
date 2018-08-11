using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public LayerMask blockingLayer; //Layer on which collision will be checked.
    public float moveTime = 0.1f;

    private BoxCollider2D collider;
    private Rigidbody2D body;
    private Animator animator;
    private SpriteRenderer renderer;

    private const float EPSILON = 0.001f;
    private float sqrRemainingDistance = 0f;
    private Vector3 end = Vector3.zero;
    private bool isMoving = false;
    private bool flipX = false;

    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        collider = GetComponent<BoxCollider2D>();
        body = GetComponent<Rigidbody2D>();
        renderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        SmoothMovement();

        int h = (int) Input.GetAxisRaw("Horizontal");
        int v = (int) Input.GetAxisRaw("Vertical");

        if (h == 0) return;

        if (h < 0)
        {
            flipX = true;
            TryMove(Vector3.left);
        }
        else
        {
            flipX = false;
            TryMove(Vector3.right);
        }
    }

    bool TryMove(Vector3 dir)
    {
        if (isMoving) return false;
        renderer.flipX = flipX;

        var start = transform.position;
        end = start + dir;

        collider.enabled = false;
        var hit = Physics2D.Linecast(start, end, blockingLayer);
        collider.enabled = true;

        if (hit.transform != null) return false;

        Debug.LogFormat("MOVE {0} to {1}", start, end);
        isMoving = true;
        OnStartMoving();

        return true;
    }

    private void SmoothMovement()
    {
        // TODO: CHECK THAT OBJECT DOESN't STUCK in box collider
        if (!isMoving) return;
        var inverseMoveTime = 1f / moveTime;
        sqrRemainingDistance = (transform.position - end).sqrMagnitude;
        if (sqrRemainingDistance > EPSILON)
        {
            Vector3 newPosition = Vector3.MoveTowards(body.position, end, inverseMoveTime * Time.deltaTime);
            body.MovePosition(newPosition);
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;
        }
        else
        {
            isMoving = false;
            OnStopMoving();
        }
    }

    private void OnStartMoving()
    {
        // TODO: it should be kind of event?
        animator.SetBool("playerRunning", true);
    }

    private void OnStopMoving()
    {
        // TODO: it should be kind of event?
        animator.SetBool("playerRunning", false);
    }
}