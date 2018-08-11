using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public LayerMask blockingLayer; //Layer on which collision will be checked.
    public float moveTime = 0.1f;

    private BoxCollider2D collider;
    private Rigidbody2D body;
    private float inverseMoveTime;

    private const float EPSILON = 0.001f;
    private float sqrRemainingDistance = 0f;
    private Vector3 end = Vector3.zero;
    private bool isMoving = false;
    

    // Use this for initialization
    void Start()
    {
        collider = GetComponent<BoxCollider2D>();
        body = GetComponent<Rigidbody2D>();
        inverseMoveTime = 1f / moveTime;
    }

    // Update is called once per frame
    void Update()
    {
        SmoothMovement();

        int h = (int)Input.GetAxisRaw("Horizontal");
        int v = (int)Input.GetAxisRaw("Vertical");

        if (h == 0) return;
        
        if (h < 0)
        {
            TryMove(Vector3.left);
        }
        else
        {
            TryMove(Vector3.right);
        }
    }

    bool TryMove(Vector3 dir)
    {
        if (isMoving) return false;
        
        var start = transform.position;
        end = start + dir;
        
        collider.enabled = false;
        var hit = Physics2D.Linecast(start, end, blockingLayer);
        collider.enabled = true;
        
        if (hit.transform != null) return false;
        
        Debug.LogFormat("MOVE {0} to {1}", start, end);
        isMoving = true;
        
        return true;
    }

    private void SmoothMovement()
    {
        if (!isMoving) return;
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
        }
    }
}