using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : DamageVolume
{
    [SerializeField] private float velX = 0f;
    [SerializeField] private float velY = 0f;
    public float timeToDestroy = 2f;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, timeToDestroy);
    }

    protected override bool ApplyDamage(Collider2D collider)
    {
        if (base.ApplyDamage(collider))
        {
            Destroy(gameObject);
            return true;
        }
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * Time.deltaTime * velX); //X axis
        transform.Translate(Vector3.right * Time.deltaTime * velY); //Y axis
    }
}
