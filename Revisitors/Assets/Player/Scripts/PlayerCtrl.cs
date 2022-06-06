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
    [SerializeField] private EnnemiesController ennemiesController;
    [SerializeField] private ParticleSystem particles;
    [SerializeField] private bool invertSprite;
    [SerializeField] private AudioClip dieSound;

    public bool CanDestroyObstacles { get; set; } = false;
    public bool IsGoingRight { get; private set; } = true;
    public bool IsGrounded { get; set; } = false;
    public bool IsAgainstWall { get; set; } = false;
    public bool IsDashing { get; set; } = false;
    public bool CanMove { get; set; } = false;

    public Vector2 Velocity { get; set; }

    private ScoreCtrl scoreCtrl;
    private bool wasGoingRight;
    private SpriteRenderer spriteRenderer;
    private PiocheUI piocheUI;
    private List<IOnPlayerDeath> onPlayerDeads = new List<IOnPlayerDeath>();

    private void Awake() {
        scoreCtrl = FindObjectOfType<ScoreCtrl>();
        ennemiesController = FindObjectOfType<EnnemiesController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        particles = GetComponentInChildren<ParticleSystem>();
        piocheUI = FindObjectOfType<PiocheUI>();
        piocheUI.gameObject.SetActive(false);
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
        scoreCtrl.Add(1000);
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
        ennemiesController.ReactivateEnnemies();
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
            return;
        }

        Pioche pioche = collision.GetComponent<Pioche>();
        if (pioche != null) {
            piocheUI.gameObject.SetActive(true);
            pioche.gameObject.SetActive(false);
            CanDestroyObstacles = true;
        }
        
    }
    private void OnCollisionEnter2D(Collision2D collision) {
        PlayerKiller pk = collision.gameObject.GetComponent<PlayerKiller>();
        if (pk != null) {
            OnDead();
            return;
        }
    }

}

//interface IOnPlayerDead {
//    void OnPlayerDead();
//}
