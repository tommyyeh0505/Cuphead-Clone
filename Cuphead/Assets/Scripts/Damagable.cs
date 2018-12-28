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

    public virtual void OnHit(float damage)
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
