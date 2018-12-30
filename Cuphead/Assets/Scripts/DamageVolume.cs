using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageVolume : MonoBehaviour
{
    [SerializeField] public float damageOnContact = 10f;
    [SerializeField] public bool canHurtPlayer = true;
    [SerializeField] public bool canHurtEnemy = true;

    protected virtual bool ApplyDamage(Collider2D collider)
    {
        if (collider.tag == "projectile")
        {
            return false;
        }

        if (!canHurtEnemy && collider.tag == "enemy")
        {
            return false;
        }

        if (!canHurtPlayer && collider.tag == "Player")
        {
            return false;
        }

        Damagable dmgComponent = collider.GetComponent<Damagable>();
        if (dmgComponent)
        {
            dmgComponent.OnHit(damageOnContact, (collider.transform.position - transform.position).normalized);
        }

        return true;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        ApplyDamage(collider);
    }
}
