using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public LayerMask blockingLayer; //Layer on which collision will be checked.
    public float moveTime = 0.1f;
    public LineRenderer lr;
    
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    public Animator HeartAnimator;

    private const float EPSILON = 0.00001f;
    private float start;
    private float end;
    private bool flipX;

    private Player player;

    // Use this for initialization
    void Start()
    {
        player = GetComponent<Player>();
        end = transform.position.x; // initial
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.NeedStamina())
        {
            OnResetStamina();
        }
        if (transform.position.y < -1.5f)
        {
            // TODO: actually it's phisics problem. But try to fix
            transform.position = new Vector3(transform.position.x, -1.5f, transform.position.z);
        }
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
//        if (isMoving) return false;
        if (player.isStan || player.NeedStamina()) return false;
        spriteRenderer.flipX = flipX;

        var startPos = transform.position;
        start = normalize(startPos.x);
        var toEnd = normalize(start + dir);

        // TODO: Actually it should be detected by phisics. Its kostil.
        if (transform.position.y < 6)
        {
            if (toEnd >= 2.5 || toEnd <= -2.5) return false;
        }

        var to = startPos + Vector3.right * dir + Vector3.down * 0.1f;

        if (willBeCollided(startPos, to)) return false;
        
        if (Math.Abs(end - toEnd) > 0.1)
        {
            player.OnMove();
        }
            
        end = toEnd;
        OnStartMoving();

        return true;
    }

    private void SmoothMovement()
    {
        if (willBeCollided(transform.position, new Vector3(end, transform.position.y, transform.position.z)))
        {
            end = start;
        }
        
        if (!player.isMoving)
        {
            transform.position = new Vector3(end, transform.position.y, transform.position.z);
            return;
        }
        var inverseMoveTime = 1f / moveTime;

        var delta = end - transform.position.x;
        var maxDistanceDelta = inverseMoveTime * Time.deltaTime;
        if (Mathf.Abs(delta) > EPSILON && Mathf.Abs(delta) > maxDistanceDelta)
        {
            transform.Translate(maxDistanceDelta * Math.Sign(delta), 0, 0);
        }
        else
        {
            transform.position = new Vector3(end, transform.position.y, transform.position.z);
            start = end;
            OnStopMoving();
        }
    }

    private bool willBeCollided(Vector3 startPos, Vector3 to)
    {
        lr.SetPosition(0, to);
        lr.SetPosition(1, startPos);
        
//        aCollider.enabled = false;
        var hit = Physics2D.Linecast(startPos, to, blockingLayer, 0);
//        aCollider.enabled = true;

        return hit.transform != null;
    }

    private void OnStartMoving()
    {
        player.OnStartMoving();
        animator.SetBool("playerRunning", true);
    }

    private void OnStopMoving()
    {
        player.OnStopMoving();
        animator.SetBool("playerRunning", false);
    }

    private float normalize(float x)
    {
        return Mathf.Ceil(x) - 0.5f;
    }

    public void Hurt(float stanDelay)
    {
        CancelInvoke("ResetStan");
        animator.SetBool("stan", true);
        player.Stan();
        Invoke("ResetStan", stanDelay);
    }

    private void ResetStan()
    {
        animator.SetBool("stan", false);
        player.ResetStan();
    }

    private void OnResetStamina()
    {
        if (player.resetingStamina || player.isStan) return;
        player.resetingStamina = true;
        animator.SetBool("resting", true);
        HeartAnimator.SetTrigger("exploid");
        Invoke("ResetStamina", player.StaminaCooldown);
    }

    private void ResetStamina()
    {
        animator.SetBool("resting", false);
        player.ResetStamina();
    }
}