using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyTrigger : MonoBehaviour
{
    [SerializeField] private WalkingEnnemy walkingEnnemy;

    private Vector3 startPos;

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.GetComponent<PlayerCtrl>() != null) {
            walkingEnnemy.Walk();
        }
    }

    private void Start() {
        GameManager gameManager = FindObjectOfType<GameManager>();

        gameManager.onPlayerDead.AddListener(Refresh);
        startPos = walkingEnnemy.transform.position;


        walkingEnnemy.GetComponent<Animator>().SetBool("Fly", false);
    }

    void Refresh() {
        walkingEnnemy.transform.position = startPos;
        walkingEnnemy.gameObject.SetActive(true);
        walkingEnnemy.Refresh();
    }
}
