using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Char1Behaviour : MonoBehaviour
{
    private static readonly int LEFT = 0;
    private static readonly int RIGHT = 1;

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
            // TODO Later : Adapt here when we will implement jump and dash
            rb.velocity = Vector3.zero;
        }
    }
}
