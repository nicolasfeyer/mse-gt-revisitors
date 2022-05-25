using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemiesController : MonoBehaviour
{
    EnemyPatrol[] enemyPatrol;

    public void ReactivateEnnemies()
    {
        foreach (var enemy in enemyPatrol)
        {
            enemy.gameObject.SetActive(true);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        enemyPatrol = GetComponentsInChildren<EnemyPatrol>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
