using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField] private bool isGoingRight;
    [SerializeField] private bool isGrounded;
    [SerializeField] private bool isAgainstWall;
    [SerializeField] private bool isDashing;
    [SerializeField] private bool canMove;
#endif
    [SerializeField] private ParticleSystem particles;
    [SerializeField] private bool invertSprite;
    [SerializeField] private AudioClip dieSound;

    public bool IsGoingRight { get; private set; } = true;
    public bool IsGrounded { get; set; } = false;
    public bool IsAgainstWall { get; set; } = false;
    public bool IsDashing { get; set; } = false;
    public bool CanMove { get; set; } = false;

    public Vector2 Velocity { get; set; }

    private bool wasGoingRight;
    private SpriteRenderer spriteRenderer;

    private List<IOnPlayerDeath> onPlayerDeads = new List<IOnPlayerDeath>();

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        particles = GetComponentInChildren<ParticleSystem>();
        //spriteRenderer.flipX = invertSprite ? IsGoingRight : !IsGoingRight;
        CanMove = true;
    }

    private void LateUpdate() {
#if UNITY_EDITOR
        isGoingRight = IsGoingRight;
        isGrounded = IsGrounded;
        isAgainstWall = IsAgainstWall;
        isDashing = IsDashing;
        canMove = CanMove;
#endif

        if (IsGoingRight != wasGoingRight) {
            spriteRenderer.flipX = invertSprite ? IsGoingRight : !IsGoingRight;
            wasGoingRight = IsGoingRight;
        }
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.R))
            OnDead();
    }

    public void SetDirectionRight(bool isRight) {
        IsGoingRight = isRight;
        spriteRenderer.flipX = invertSprite ? IsGoingRight : !IsGoingRight;
    }

    private void OnDead() {
        CanMove = false;
        particles.Play();
        AudioSource.PlayClipAtPoint(dieSound, particles.transform.position);
        spriteRenderer.enabled = false;
        onPlayerDeads.ForEach(x => x.OnPlayerDeath());
        StartCoroutine(Respawn());
    }

    public void EndGame() {
        CanMove = false;
    }

    private IEnumerator Respawn() {
        yield return new WaitForSeconds(particles.main.duration);
        GameManager.instance.RespawnPlayer();
        spriteRenderer.enabled = true;
        particles.Stop();
        CanMove = true;

        //GameObject[] li = UnityEngine.Object.FindObjectsOfType<WeakSpot>();
        //foreach (var i in li)
        //{
        //    i.transform.parent.transform.SetActive(true);
        //}
    }

    public void Subscribe(IOnPlayerDeath sub) {
        onPlayerDeads.Add(sub);
    }

    public void Unsubscribe(IOnPlayerDeath sub) {
        onPlayerDeads.Remove(sub);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        PlayerKiller pk = collision.GetComponent<PlayerKiller>();
        if (pk != null) {
            OnDead();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        Ennemy ennemy = collision.gameObject.GetComponent<Ennemy>();
        if (ennemy != null) {
            if(IsDashing) {
                ennemy.Die();
            }
            else {
                OnDead();
            }
        }
    }
}

//interface IOnPlayerDead {
//    void OnPlayerDead();
//}
