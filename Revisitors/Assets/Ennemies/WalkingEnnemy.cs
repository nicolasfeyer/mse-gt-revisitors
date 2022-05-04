using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ennemy : MonoBehaviour { public abstract void Die(); }

public class WalkingEnnemy : Ennemy {
    [SerializeField] private GameObject particlesPrefab;
    [SerializeField] private bool goRight;
    [SerializeField] private float speed;
    [SerializeField] private AudioClip dieSound;

    bool canWalk = false;
    private Rigidbody2D rb2d;

    private void Start() {
        rb2d = GetComponent<Rigidbody2D>();
        //Walk();
    }

    public void Walk() {
        canWalk = true;
    }

    public void Refresh() {
        canWalk = false;
        rb2d.velocity = Vector2.zero;
    }

    override public void Die() {
        AudioSource.PlayClipAtPoint(dieSound, rb2d.position);
        canWalk = false;
        rb2d.velocity = Vector2.zero;
        GameObject go = Instantiate(particlesPrefab);
        go.transform.position = transform.position;
        this.gameObject.SetActive(false);
    }

    private void FixedUpdate() {
        if(canWalk) {
            rb2d.velocity = new Vector2(goRight ? speed : -speed, rb2d.velocity.y);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.GetComponent<PlayerKiller>()) {
            Die();
        }
    }
}
