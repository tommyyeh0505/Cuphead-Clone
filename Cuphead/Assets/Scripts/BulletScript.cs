using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [SerializeField] float damage = 10f;
    [SerializeField] private float velX = 0f;
    [SerializeField] private float velY = 0f;
    public float timeToDestroy = 2f;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, timeToDestroy);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "projectile")
        {
            return;
        }

        Damagable dmgComponent = collider.GetComponent<Damagable>();
        if (dmgComponent)
        {
            dmgComponent.OnHit(damage);
        }

        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * Time.deltaTime * velX); //X axis
        transform.Translate(Vector3.right * Time.deltaTime * velY); //Y axis
    }
}
