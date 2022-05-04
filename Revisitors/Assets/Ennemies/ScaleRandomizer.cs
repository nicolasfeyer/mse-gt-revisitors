using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleRandomizer : MonoBehaviour
{
    [SerializeField] private float minScale;
    [SerializeField] private float maxScale;

    [SerializeField] private float changeDelay;

    private float timer;

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if(timer <= 0f) {
            float scale = Random.Range(minScale, maxScale);
            transform.localScale = new Vector3(scale, scale, 1f);
            timer = changeDelay;
        }
    }
}
