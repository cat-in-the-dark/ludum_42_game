using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public LayerMask blockingLayer; //Layer on which collision will be checked.
    public float moveTime = 0.1f;

    private BoxCollider2D collider;
    private Rigidbody2D body;
    private Animator animator;
    private SpriteRenderer renderer;

    private const float EPSILON = 0.00001f;
    private float sqrRemainingDistance = 0f;
    private float end;
    private bool isMoving;
    private bool flipX;

    public bool IsMoving()
    {
        return isMoving;
    }

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

        if (h == 0) return;

        if (h < 0)
        {
            flipX = true;
            TryMove(-1);
        }
        else
        {
            flipX = false;
            TryMove(1);
        }
    }

    bool TryMove(int dir)
    {
        if (isMoving) return false;
        renderer.flipX = flipX;

        var start = transform.position;
        end = normalize(start.x + dir);
        
        var to = start + Vector3.right * dir;

        collider.enabled = false;
        var hit = Physics2D.Linecast(start, to, blockingLayer);
        collider.enabled = true;

        if (hit.transform != null) return false;

//        Debug.LogFormat("MOVE {0} to {1}", start, end);        
        OnStartMoving();

        return true;
    }

    private void SmoothMovement()
    {
        // TODO: CHECK THAT OBJECT DOESN't STUCK in box collider
        if (!isMoving) return;
        var inverseMoveTime = 1f / moveTime;

        var delta = end - transform.position.x;
        sqrRemainingDistance = delta * delta;
        if (sqrRemainingDistance > EPSILON)
        {
            var maxDistanceDelta = inverseMoveTime * Time.deltaTime;
            if (sqrRemainingDistance <= maxDistanceDelta)
            {
                transform.position = new Vector3(end, transform.position.y, transform.position.z);
            }
            else
            {
                transform.Translate(maxDistanceDelta / delta, 0, 0);
            }
        }
        else
        {
            transform.position = new Vector3(end, transform.position.y, transform.position.z);
            OnStopMoving();
        }
    }

    private void OnStartMoving()
    {
        isMoving = true;
        animator.SetBool("playerRunning", true);
    }

    private void OnStopMoving()
    {
        isMoving = false;
        animator.SetBool("playerRunning", false);
    }

    private float normalize(float x)
    {
        return Mathf.Floor(x / 0.5f) * 0.5f;
    }
}