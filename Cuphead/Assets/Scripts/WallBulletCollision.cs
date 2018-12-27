using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBulletCollision : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("collision name = " + col.gameObject.name);
        if (col.gameObject.name == "BulletLeft(Clone)" || col.gameObject.name == "BulletRight(Clone)")
        {
            Destroy(col.gameObject);
        }
    }
    // Update is called once per frame
    void Update()
    {
       
    }
}
