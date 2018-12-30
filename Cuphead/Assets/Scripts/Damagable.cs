using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Damagable : MonoBehaviour
{
    [SerializeField] protected float health;
    [SerializeField] protected float timeToDespawn;

    // Start is called before the first frame update
    void Start()
    {
    }

    // TODO: probably should replace parameters with some sort of 'context' GameObject (e.g. the bullet that hits you)
    // then the class could take info from the context, instead of only damage and direction being considered
    public virtual void OnHit(float damage, Vector2 direction)
    {
        health -= damage;
        if (health <= 0)
        {
            OnDie();
        }
    }

    void OnDie()
    {
        Destroy(gameObject, timeToDespawn);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
