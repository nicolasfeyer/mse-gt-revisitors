using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnnemyStarter : MonoBehaviour
{
    [SerializeField] private FlyingEnnemy flyingEnnemy;

    // Start is called before the first frame update
    void Start()
    {
        GameManager gameManager = FindObjectOfType<GameManager>();
        gameManager.onPlayerDead.AddListener(Refresh);

        flyingEnnemy.GetComponent<Animator>().SetBool("Fly", true);
    }

    private void Refresh() {
        flyingEnnemy.gameObject.SetActive(true);
        flyingEnnemy.Refresh();
    }
}
