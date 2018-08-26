using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public int MaxStamina;
    public float StaminaCooldown;
    public float JumpDelay;
    public float JumpPower;
    public float StampDelay;
    
    public bool resetingStamina = false;
    public bool isStan = false;
    public bool isMoving = false;
    public bool grounded = true;
    public float lastJumpAt = float.MaxValue;
    public int stamina;

    public AudioClip OnStanSound;
    public AudioClip OnLandGround;
    public AudioClip OnWalkGround;
    
    private Animator animator;
    private AudioSource audioSource;
    
    void Start()
    {   
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        resetingStamina = false;
        stamina = MaxStamina;
    }
    
    public bool CanStomp()
    {
        return !isMoving && !isStan && (stamina > 0) && grounded;
    }
    
    public bool CanJump()
    {
        return grounded && (lastJumpAt >= JumpDelay) && !isStan;
    }
    
    public void ResetStamina()
    {
        stamina = MaxStamina;
        resetingStamina = false;
    }

    public void ResetStan()
    {
        isStan = false;
    }

    public void Stan()
    {
        isStan = true;
        audioSource.PlayOneShot(OnStanSound);
    }

    public bool NeedStamina()
    {
        return stamina <= 0;
    }

    public void OnJump()
    {
        stamina--;
        lastJumpAt = 0;
        grounded = false;
        animator.SetBool("playerJumping", true);
    }

    public void OnStopJumping()
    {
        if (!grounded)
        {
            grounded = true;
            animator.SetBool("playerJumping", false);
            audioSource.PlayOneShot(OnLandGround);
        }
    }
    
    public void OnStomp()
    {
        stamina--;
        animator.SetTrigger("playerStomp");
    }

    public void OnMove()
    {
        stamina--;
    }

    public void OnStartMoving()
    {
        if (!isMoving)
        {
            audioSource.PlayOneShot(OnWalkGround);
            isMoving = true;
        }
    }

    public void OnStopMoving()
    {
        if (isMoving)
        {
            isMoving = false;
        }
    }
}
