﻿using System;
using UnityEngine;

public class DirtControl : MonoBehaviour
{
    private int MaxStompLevel = 3;

    public bool canBeStomped;
    public bool grounded;
    public Collider2D playerCollider;
    public Player player;
    public Collider2D soilCollider;

    public Animator animator;
    public Collider2D aCollider;
    public Rigidbody2D body;

    private int stompLevel = 0;

    // Use this for initialization
    void Start()
    {
        ResetStampCooldown();
        player = FindObjectOfType<Player>();

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
            if (IsCollidingPlayer() && canBeStomped && grounded)
            {
                player.OnStomp();
                Stomp();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.gameObject.CompareTag("Ground")) return;
        
        grounded = true;
        animator.SetBool("touchDown", true);
        MakeGround();
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

        Invoke("ResetStampCooldown", player.StampDelay);
    }

    private void ResetStampCooldown()
    {
        canBeStomped = true;
    }

    public void MakeGround()
    {
        if (gameObject.CompareTag("Ground")) return;
        transform.position = new Vector3(normalizeX(transform.position.x), normalizeY(transform.position.y),
            transform.position.z);
        gameObject.tag = "Ground";
        gameObject.layer = LayerMask.NameToLayer("Ground");
        body.bodyType = RigidbodyType2D.Kinematic;
        body.freezeRotation = true;
        body.constraints = RigidbodyConstraints2D.FreezeAll;
        aCollider.isTrigger = true;
        soilCollider.GetComponent<Collider2D>().enabled = true;
    }

    private void MakeSolid()
    {
        animator.SetBool("beGround", true);
        transform.position = new Vector3(normalizeX(transform.position.x), normalizeY(transform.position.y), 0);
        aCollider.isTrigger = false;
    }

    private bool IsCollidingPlayer()
    {
        var delta = Mathf.Abs(playerCollider.transform.position.y - aCollider.transform.position.y);
        return aCollider.IsTouching(playerCollider) && delta <= 0.1;
    }

    private float normalizeX(float x)
    {
        return Mathf.Ceil(x) - 0.5f;
    }

    private float normalizeY(float y)
    {
        var mod = 5;
        var rem = (y * 10f) % mod;
        if (rem < 0) rem += 5;
        var res = y - rem / 10f;
        if (res < -1.5) return -1.5f; // TODO: its costil, need smart round
        return res;
    }
}