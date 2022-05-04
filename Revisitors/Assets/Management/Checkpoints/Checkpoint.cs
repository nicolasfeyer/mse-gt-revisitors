using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Checkpoint : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private bool goRight;
    [SerializeField] private bool isEnd;

    public Transform SpawnPoint { get { return spawnPoint; } }
    public bool GoRight { get { return goRight; } }
    public bool isStartingPoint = false;

    public int Id { get; set; } = -1;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (!collision.GetComponent<PlayerCtrl>())
            return;

        GameManager.instance?.ChangeCheckpoint(Id);

        if(isEnd) {
            GameManager.instance?.EndGame();
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(spawnPoint.position, 0.2f);
        if(isStartingPoint)
            Gizmos.DrawWireSphere(spawnPoint.position, 0.30f);
        Gizmos.DrawRay(spawnPoint.position, goRight ? spawnPoint.right : -spawnPoint.right);
    }
}
