using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockTrigger : MonoBehaviour
{
    private bool asReleaseRocks = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (asReleaseRocks) return;
        if(collision.CompareTag("Player"))
        {
            RocksCtrl rocksCtrl = gameObject.GetComponentInParent(typeof(RocksCtrl)) as RocksCtrl;
            rocksCtrl.ReleaseRocks();
            asReleaseRocks = true;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
