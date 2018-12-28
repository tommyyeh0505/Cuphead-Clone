using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{

    [SerializeField] float velX = 0f;
    [SerializeField] float velY = 0f;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 2f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * Time.deltaTime * velX); //X axis
        transform.Translate(Vector3.right * Time.deltaTime * velY); //Y axis
    }
}
