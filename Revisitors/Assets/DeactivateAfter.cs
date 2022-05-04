using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateAfter : MonoBehaviour
{
    [SerializeField] private float maxTime;

    float timer;

    void Awake()
    {
        timer = maxTime;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if(timer <= 0f) {
            gameObject.SetActive(false);
        }
    }
}
