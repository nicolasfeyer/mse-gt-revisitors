using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrounded : MonoBehaviour
{
    [SerializeField] ContactFilter2D groundFilter;

    private Rigidbody2D rb2d;
    private PlayerCtrl ctrl;

    // Start is called before the first frame update
    void Start()
    {
        ctrl = GetComponent<PlayerCtrl>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        ctrl.IsGrounded = rb2d.IsTouching(groundFilter);
    }
}
