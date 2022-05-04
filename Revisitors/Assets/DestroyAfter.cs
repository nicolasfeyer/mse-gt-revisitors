using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfter : MonoBehaviour
{
    private float timeLeft;
    private bool init = false;

    private void OnEnable() {
        timeLeft = GetComponent<ParticleSystem>().main.duration;
        init = true;
    }

    private void Update() {
        timeLeft -= Time.deltaTime;

        if(init && timeLeft <= 0f) {
            Destroy(gameObject);
        }
    }
}
