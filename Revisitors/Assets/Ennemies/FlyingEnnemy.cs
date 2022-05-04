using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnnemy : Ennemy
{
    [SerializeField] private float goUpDst;
    [SerializeField] private float speed;
    [SerializeField] private GameObject particlesPrefab;

    private bool goUp;
    private float currentHeight;
    private Vector3 startPos;
    private Rigidbody2D rb2d;

    private void Awake() {
        startPos = transform.position;
        rb2d = GetComponent<Rigidbody2D>();
    }

    public void Refresh() {
        GetComponent<Animator>().SetBool("Fly", true);
        transform.position = startPos;
        currentHeight = 0f;
        goUp = true;
    }

    public override void Die() {
        GameObject go = Instantiate(particlesPrefab);
        go.transform.position = transform.position;
        this.gameObject.SetActive(false);
    }

    private void Update() {
        float moveDst = (goUp ? speed : -speed) * Time.deltaTime;
        currentHeight += moveDst;

        rb2d.MovePosition(transform.position + Vector3.up * moveDst);

        if (currentHeight > goUpDst) goUp = false;
        if (currentHeight < 0f) goUp = true;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;

        Gizmos.DrawRay(transform.position, Vector2.up * goUpDst);
    }
}
