using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Char1Behaviour : MonoBehaviour
{
    private static readonly int LEFT = 0;
    private static readonly int RIGHT = 1;

    private enum Mode
    {
        Sprint,
        Jump,
        Dash
    }

    private Mode current_mode = Mode.Sprint;

    public Rigidbody2D rb;
    private bool[] pressed = new bool[2]; // left, right

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        pressed[LEFT] = Input.GetKey("left");
        pressed[RIGHT] = Input.GetKey("right");
    }


    void FixedUpdate()
    {
        if (current_mode == Mode.Sprint)
        {
            // Sprint mode : check left and right move
            if (pressed[LEFT])
            {
                // Impulse to left direction
                rb.AddForce(Vector2.left * 1, ForceMode2D.Impulse);
            }
            else if (pressed[RIGHT])
            {
                // Impulse to right direction
                rb.AddForce(Vector2.right * 1, ForceMode2D.Impulse);
            }
            else
            {
                // The player don't move left or right, stop it immediatly
                rb.velocity = Vector3.zero;
            }
        }
        else if (current_mode == Mode.Jump)
        {
            // TODO
        }
        else if (current_mode == Mode.Dash)
        {
            // TODO
        }
    }
}
