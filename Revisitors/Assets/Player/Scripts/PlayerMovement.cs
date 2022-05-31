using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMovement : MonoBehaviourPunCallbacks, IOnPlayerDeath
{
    [Header("Movment")]
    [SerializeField] private float maxSpeed;
    [SerializeField] private float groundAcc;
    [SerializeField] private float airAcc;
    [SerializeField] private float maxDownSpeedWall;

    [Header("Jump")]
    [SerializeField] private float jumpSpeed;
    [SerializeField] private float endOfJumpSpeedReduction;
    [SerializeField] private float gravityForce;
    [SerializeField] private float jumpMinDuration;
    [SerializeField] private float jumpMaxDuration;
    [SerializeField] private AudioClip jumpAudio;

    [Header("Dash")]
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashMinDuration;
    [SerializeField] private float dashMaxDuration;
    [SerializeField] private AudioClip dashAudio;


    private Animator animator;
    private Rigidbody2D rb2d;
    private PlayerCtrl ctrl;
    private float horizontalSpeed;
    private float verticalSpeed;

    private bool isJumping = false;
    private float jumpTimer;
    private const string JUMP_BUTTON = "Jump";


    private bool canDash;
    private bool isDashing = false;
    private bool dashDown;
    private float dashTimer;
    private const string DASH_BUTTON = "Dash";

    private void OnEnable()
    {
        GetComponent<PlayerCtrl>().Subscribe(this);
    }

    private void OnDisable()
    {
        ctrl.Unsubscribe(this);
    }

    void Start()
    {
        ctrl = GetComponent<PlayerCtrl>();
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
        {
            return;
        }
        if (ctrl.CanMove)
        {
            Gravity();
            Move();
        }
        else
        {
            rb2d.velocity = Vector2.zero;
        }

    }

    private void Update()
    {
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
        {
            return;
        }
        if (ctrl.CanMove)
        {
            Dash();
            Jump();

            UpdateAnimation();
            ctrl.Velocity = rb2d.velocity;
        }
        else
        {
            animator.SetBool("IsGrounded", true);
            animator.SetFloat("VerticalSpeed", verticalSpeed);
            animator.SetFloat("HorizontalSpeed", 0f);
        }
    }

    private void Gravity()
    {
        if (ctrl.IsGrounded)
        {
            verticalSpeed = 0f;
        }

        verticalSpeed -= gravityForce * Time.fixedDeltaTime;

        if (ctrl.IsAgainstWall && verticalSpeed < -maxDownSpeedWall)
        {
            verticalSpeed = -maxDownSpeedWall;
        }
    }

    private void Move()
    {
        if (ctrl.IsAgainstWall)
        {
            horizontalSpeed = 0f;
        }

        float targetSpeed = ctrl.IsGoingRight ? maxSpeed : -maxSpeed;
        float acceleration = ctrl.IsGrounded ? groundAcc : airAcc;
        horizontalSpeed = Mathf.MoveTowards(horizontalSpeed, targetSpeed, acceleration * Time.fixedDeltaTime);

        if (isJumping)
        {
            verticalSpeed = jumpSpeed;
        }

        Vector2 velocity;

        if (isDashing)
        {
            if (dashDown)
            {
                velocity = new Vector2(horizontalSpeed, -dashSpeed);
            }
            else
            {
                horizontalSpeed = ctrl.IsGoingRight ? dashSpeed : -dashSpeed;
                velocity = new Vector2(horizontalSpeed, 0f);
            }
        }
        else
        {
            velocity = new Vector2(horizontalSpeed, verticalSpeed);
        }

        rb2d.velocity = velocity;
    }

    private void Jump()
    {
        // Can't jump while dashing
        if (isDashing) return;

        // During jump
        if (isJumping)
        {
            jumpTimer += Time.deltaTime;
            bool buttonDown = Input.GetButton(JUMP_BUTTON);

            if (jumpTimer > jumpMaxDuration || !buttonDown)
            {
                isJumping = false;
                verticalSpeed = endOfJumpSpeedReduction * verticalSpeed;
            }
        }
        // Start jump
        else
        {
            if (Input.GetButtonDown(JUMP_BUTTON) && ctrl.IsGrounded)
            {
                jumpTimer = 0f;
                isJumping = true;
                animator.SetTrigger("Jump");
                AudioSource.PlayClipAtPoint(jumpAudio, ctrl.transform.position, 2f);
            }
        }

        // Wall Jump
        if (ctrl.IsAgainstWall && !ctrl.IsGrounded && Input.GetButtonDown(JUMP_BUTTON))
        {
            jumpTimer = 0f;
            isJumping = true;
            ctrl.IsAgainstWall = false;
            //ctrl.IsGoingRight = !ctrl.IsGoingRight;
            ctrl.SetDirectionRight(!ctrl.IsGoingRight);
            horizontalSpeed = ctrl.IsGoingRight ? maxSpeed : -maxSpeed;
            animator.SetTrigger("Jump");
            AudioSource.PlayClipAtPoint(jumpAudio, ctrl.transform.position, 10f);
        }
    }

    private void Dash()
    {
        // Start dash
        if (canDash && !isDashing)
        {
            if (Input.GetButtonDown(DASH_BUTTON))
            {
                isDashing = true;
                canDash = false;
                dashTimer = 0f;
                ctrl.IsDashing = true;
                AudioSource.PlayClipAtPoint(dashAudio, ctrl.transform.position, 2f);
            }
        }
        // During dash
        if (isDashing)
        {
            // End of dash
            if ((dashTimer > dashMaxDuration) || (dashTimer > dashMinDuration && !Input.GetButton(DASH_BUTTON)))
            {
                isDashing = false;
                ctrl.IsDashing = false;
                verticalSpeed = 0f;
            }

            dashTimer += Time.deltaTime;
        }

        // Enable dash
        if (ctrl.IsGrounded || ctrl.IsAgainstWall)
        {
            canDash = true;
        }
    }

    private void UpdateAnimation()
    {
        animator.SetBool("IsGrounded", ctrl.IsGrounded);
        //animator.SetBool("IsDashing", isDashing);
        animator.SetFloat("VerticalSpeed", verticalSpeed);
        animator.SetFloat("HorizontalSpeed", ctrl.CanMove ? Mathf.Abs(horizontalSpeed) : 0f);
        //animator.SetFloat("HorizontalSpeed", Mathf.Abs(maxSpeed));
        animator.SetLayerWeight(1, isDashing ? 1f : 0f);
    }

    public void OnPlayerDeath()
    {
        verticalSpeed = 0f;
        horizontalSpeed = 0f;
        isJumping = false;
        isDashing = false;
    }
}
