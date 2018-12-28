using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealthComponent : Damagable
{
    float damageFlashTime = 0.05f;
    float damageFlashWaitTime = 0.3f;
    bool gettingHit = false;

    public override void OnHit(float damage)
    {
        base.OnHit(damage);

        if (!gettingHit)
        {
            StartCoroutine("HitFlash");
        }
    }

    private IEnumerator HitFlash()
    {
        gettingHit = true;
        gameObject.GetComponent<SpriteRenderer>().color = new Color(255f, 255f, 255f, 0f);
        yield return new WaitForSeconds(damageFlashTime);
        gameObject.GetComponent<SpriteRenderer>().color = new Color(255f, 255f, 255f, 255f);
        yield return new WaitForSeconds(damageFlashWaitTime);
        gettingHit = false;
        yield return null;
    }
}
