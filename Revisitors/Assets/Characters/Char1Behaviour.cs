using UnityEngine;

public class Char1Behaviour : MonoBehaviour
{
    [SerializeField] private float playerSpeed = 15.0f;
    [SerializeField] private float jumpPower = 5.0f;
    [SerializeField] private float dashPower = 50.0f;


    private Rigidbody2D _rb;
    private bool isGrounded;
    private static readonly int LEFT = 0;
    private static readonly int RIGHT = 1;
    private static readonly int JUMP = 2;
    private static readonly int SHIFT = 3;
    private bool forward;
    private bool[] pressed = new bool[4]; // left, right, jump, shift
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        if (_rb == null)
        {
            Debug.LogError("Player is missing a Rigidbody2D component");
        }
        forward = true;
    }

    void Update()
    {
        pressed[LEFT] = Input.GetKeyDown(KeyCode.LeftArrow);//Input.GetKey("left");
        pressed[RIGHT] = Input.GetKeyDown(KeyCode.RightArrow);//Input.GetKey("right");
        pressed[JUMP] = Input.GetKeyDown(KeyCode.Space);//Input.GetKey("space");
        pressed[SHIFT] = Input.GetKeyDown(KeyCode.LeftShift);//Input.GetKey("left shift");

        if (pressed[LEFT])
            forward = false;
        if (pressed[RIGHT])
            forward = true;
    }


    private void FixedUpdate()
    {
        MovePlayer();

        if (pressed[JUMP] && isGrounded)
            Jump();

        if (pressed[SHIFT])
            Dash();

    }
    private void MovePlayer()
    {
        var horizontalInput = Input.GetAxisRaw("Horizontal");
        _rb.velocity = new Vector2(horizontalInput * playerSpeed, _rb.velocity.y);
    }
    private void Jump() => _rb.velocity = new Vector2(0, jumpPower);

    private void Dash() => _rb.velocity = new Vector2((forward ? 1 : -1) * dashPower, _rb.velocity.y);


    void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.gameObject.name == "Ground"))
            isGrounded = true;
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