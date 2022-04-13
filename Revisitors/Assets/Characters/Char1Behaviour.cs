using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Char1Behaviour : MonoBehaviour
{


    private static readonly int DOWN = 0;
    private static readonly int UP = 1;
    private static readonly int LEFT = 2;
    private static readonly int RIGHT = 3;


    public Rigidbody2D rb;
    private bool[] pressed = new bool[4]; // down, up , left, right

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        pressed[DOWN] = Input.GetKey("down");
        pressed[UP] = Input.GetKey("up");
        pressed[LEFT] = Input.GetKey("left");
        pressed[RIGHT] = Input.GetKey("right");
    }


    void FixedUpdate()
    {

        if (pressed[DOWN])
        {
            rb.AddForce(Vector2.down * 10, ForceMode2D.Force);
            pressed[DOWN] = false;
        }
        else if (pressed[UP])
        {
            rb.AddForce(Vector2.up * 20, ForceMode2D.Force);
            pressed[UP] = false;
        }
        else if (pressed[LEFT])
        {
            rb.AddForce(Vector2.left * 10, ForceMode2D.Force);
            pressed[LEFT] = false;
        }
        else if (pressed[RIGHT])
        {
            rb.AddForce(Vector2.right * 10, ForceMode2D.Force);
            pressed[RIGHT] = false;
        }

    }
}
