using UnityEngine;

public class Player : MonoBehaviour
{
    public float JumpDelay;
    public float JumpPower;
    public float StampDelay;

    public bool isStan = false;
    public bool isMoving = false;
    public bool grounded = true;
    public float lastJumpAt = float.MaxValue;
    public float jumpTime = 2f;

    public AudioClip OnStanSound;
    public AudioClip OnLandGround;
    public AudioClip OnWalkGround;

    private Animator animator;
    private AudioSource audioSource;

    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    public bool CanStomp()
    {
        return !isMoving && !isStan && grounded;
    }

    public bool CanJump()
    {
        return grounded && (lastJumpAt >= JumpDelay) && !isStan;
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

    public void OnJump()
    {
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
        animator.SetTrigger("playerStomp");
    }

    public void OnMove()
    {
        //
    }

    public void OnStartMoving()
    {
        if (!isMoving)
        {
            isMoving = true;
            if (grounded)
            {
                audioSource.PlayOneShot(OnWalkGround);
            }
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