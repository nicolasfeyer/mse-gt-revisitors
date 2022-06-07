using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocksCtrl : MonoBehaviour
{
    public Rigidbody2D[] rocks;

    public void ReleaseRocks()
    {
        foreach (var rock in rocks)
        {
            rock.bodyType = RigidbodyType2D.Dynamic;
            rock.AddForce(Vector2.right*5f, ForceMode2D.Impulse);
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
