/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Char1Behaviour : MonoBehaviour, IOnPlayerDeath
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
        print(ctrl);
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        print("START");
    }

    private void FixedUpdate()
    {
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
*/


using UnityEngine;

public class Char1Behaviour : MonoBehaviour
{
    [SerializeField] private float playerSpeed = 15.0f;
    [SerializeField] private float jumpPower = 15.0f;
    [SerializeField] private float dashPower = 50.0f;
    [SerializeField] private float maxsPeed = 50.0f;
    private bool initMove;
    private bool stoppedMoving;
    private Rigidbody2D _rb;
    private bool isGrounded;
    //private static readonly int LEFT = 0;
    //private static readonly int RIGHT = 1;
    private static readonly int SHIFT = 3;
    private bool forward;
    private bool isJumping;
    private bool isDoubleJumping;
    private bool canDash;
    private bool hasJumped;
    private bool hasDoubleJumped;
    private float oldMvmt;
    private bool[] pressed = new bool[4]; // left, right, jump, shift
    private bool leftPressed;
    private bool rightPressed;
    private bool oldLeftPressed;//true = right false = false
    private bool oldRightPressed;//true = right false = false
    //private string btnPressed;
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        if (_rb == null)
        {   
            //btnPressed ="init";
            Debug.LogError("Player is missing a Rigidbody2D component");
        }
        forward = true;
    }

    void Update()
    {
        leftPressed =  Input.GetKey(KeyCode.LeftArrow);
        rightPressed =  Input.GetKey(KeyCode.RightArrow);

        if(isGrounded && !hasJumped){
            isJumping =  Input.GetKeyDown(KeyCode.Space);
            isGrounded=!isJumping;
        }
        if(hasJumped&&!hasDoubleJumped){
            isDoubleJumping = Input.GetKey(KeyCode.Space);
        }

        if (leftPressed && !rightPressed){
            forward = false;
            stoppedMoving =false;
        }
            
        if (rightPressed && !leftPressed){
            forward = true;
            stoppedMoving =false;
        }
            

        if(!leftPressed && !rightPressed){
            stoppedMoving = true;
        }
        if(rightPressed &&leftPressed)stoppedMoving =true;
    }


    private void FixedUpdate()
    {
        
        MovePlayer();
        
        if (isJumping){
            Jump();
            isJumping = false;
            hasJumped = true;
        }
        if (isDoubleJumping && !isGrounded){
            Jump();
            isDoubleJumping = false;
            hasDoubleJumped = true;
        }           

        if (pressed[SHIFT])
            Dash();

    }
    private void MovePlayer()
    {
        var horizontalInput = Input.GetAxisRaw("Horizontal");
        //_rb.velocity = new Vector2(horizontalInput * playerSpeed, _rb.velocity.y);
        if(_rb.velocity.magnitude  < maxsPeed ){
            _rb.AddForce(new Vector2((horizontalInput * playerSpeed),0));
        }else if(stoppedMoving){
            float oldy =  _rb.velocity.y;
            _rb.velocity = new Vector2(0, oldy);//tres brut mais ok
        }
         
    }
    private void Jump(){
        _rb.AddForce(new Vector2(0,jumpPower)); 
    }

    private void Dash() => _rb.velocity = new Vector2((forward ? 1 : -1) * dashPower, _rb.velocity.y);


    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.name);
        if ((collision.gameObject.name == "collGround")) {//&& collision.GetContact(0).normal == Vector2.up
            isJumping = false;
            isDoubleJumping = false;
            isGrounded = true;
            hasJumped = false;
            hasDoubleJumped = false;
        }else if ((collision.gameObject.name == "bottle")){
            Debug.Log("END GAME");
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if ((collision.gameObject.name == "Ground"))
            isGrounded = false;
    }
}


/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Char1Behaviour : MonoBehaviour
{
    private static readonly int LEFT = 0;
    private static readonly int RIGHT = 1;
    private static readonly int JUMP = 2;
    private static readonly int SHIFT = 3;

    private enum Mode
    {
        Sprint,
        Jump,
        Dash,
        Idle
    }

    private Mode current_mode;

    public Rigidbody2D rb;
    private bool[] pressed = new bool[4]; // left, right, jump, shift

    private static readonly float JUMP_FORCE = 1f;
    private static readonly float SPRINT_FORCE = 0.5f;

    [SerializeField] private float jumpPower = 5.0f;
    [SerializeField] private float playerSpeed = 10.0f;

    private void Jump() => rb.velocity = new Vector2(0, jumpPower);

    private bool IsGrounded()
    {
        var groundCheck = Physics2D.Raycast(transform.position, Vector2.down, 0.7f);
        return groundCheck.collider != null && groundCheck.collider.CompareTag("Ground");
    }


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        pressed[LEFT] = Input.GetKeyDown(KeyCode.LeftArrow);//Input.GetKey("left");
        pressed[RIGHT] = Input.GetKeyDown(KeyCode.RightArrow);//Input.GetKey("right");
        pressed[JUMP] = Input.GetKeyDown(KeyCode.Space);//Input.GetKey("space");
        pressed[SHIFT] = Input.GetKeyDown(KeyCode.LeftShift);//Input.GetKey("left shift");

        if (pressed[LEFT] || pressed[RIGHT])
        {
            current_mode = Mode.Sprint;
        }
        else if (pressed[JUMP])
        {
            current_mode = Mode.Jump;
        }
        else if (pressed[SHIFT])
        {
            current_mode = Mode.Dash;
        }
    }


    void FixedUpdate()
    {
        if (pressed[LEFT] || pressed[RIGHT])
        {
            rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * playerSpeed, rb.velocity.y);
        }
        else if (pressed[JUMP])
        {
            Jump();
        }
        else if (pressed[SHIFT])
        {
            // TODO
        }
    }
}*/