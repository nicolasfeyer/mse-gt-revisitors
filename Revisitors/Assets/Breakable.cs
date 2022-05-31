using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    [SerializeField] private float TimeBeforeBreak;

    private float timer;
    private bool willDestroy;
    private Collider2D coll;
    private SpriteRenderer spriteRenderer;
    private ParticleSystem particles;
    private GameManager gameManager;
    private NoPioche noPioche;

    private void Start() {
        gameManager = FindObjectOfType<GameManager>();
        coll = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        particles = GetComponent<ParticleSystem>();
        noPioche = FindObjectOfType<NoPioche>();
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        PlayerCtrl playerCtrl = collision.gameObject.GetComponent<PlayerCtrl>();
        if(playerCtrl != null) {
            if(playerCtrl.CanDestroyObstacles){
                willDestroy = true;
                gameManager.onPlayerDead.AddListener(Reconstruct);
            } else {
                noPioche.StartBlinking();
            }
        }
    }

    private void Update() {
        if(willDestroy) {
            timer += Time.deltaTime;
            if(timer >= TimeBeforeBreak) {
                willDestroy = false;
                Destroy();
            }
        }
    }

    private void Destroy() {
        spriteRenderer.enabled = false;
        coll.enabled = false;
        particles.Play();
    }

    private void Reconstruct() {
        willDestroy = false;
        spriteRenderer.enabled = true;
        coll.enabled = true;
        timer = 0f;
        gameManager.onPlayerDead.RemoveListener(Reconstruct);
    }
}
