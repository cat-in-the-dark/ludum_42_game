﻿using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public LayerMask blockingLayer; //Layer on which collision will be checked.
    public float moveTime = 0.1f;

    private Collider2D aCollider;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private const float EPSILON = 0.00001f;
    private float sqrRemainingDistance = 0f;
    private float start;
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
        end = transform.position.x; // initial
        animator = GetComponent<Animator>();
        aCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public bool CanStomp()
    {
        return !IsMoving();
    }

    public void OnStomp()
    {
        animator.SetTrigger("playerStomp");
    }

    // Update is called once per frame
    void Update()
    {
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
        spriteRenderer.flipX = flipX;

        var startPos = transform.position;
        start = normalize(startPos.x);
        var toEnd = normalize(start + dir);
        Debug.Log(toEnd);

        // TODO: Actually it should be detected by phisics. Its kostil.
        if (toEnd >= 2.5 || toEnd <= -2.5) return false;

        var to = startPos + Vector3.right * dir + Vector3.down * 0.1f;

        aCollider.enabled = false;
        var hit = Physics2D.Linecast(startPos, to, blockingLayer, 0);
        aCollider.enabled = true;

        if (hit.transform != null) return false;

//        Debug.LogFormat("MOVE {0} to {1}", start, end);
        end = toEnd;
        OnStartMoving();

        return true;
    }

    private void SmoothMovement()
    {
        // TODO: CHECK THAT OBJECT DOESN't STUCK in box collider
        if (!isMoving)
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
        return Mathf.Ceil(x) - 0.5f;
    }
}