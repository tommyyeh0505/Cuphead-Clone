using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealthComponent : Damagable
{
    float damageFlashTime = 0.05f;
    float damageFlashWaitTime = 0.3f;
    bool gettingHit = false;

    [HideInInspector] public int phase;
    public List<float> phaseThresholds = new List<float>();
    private float maxHealth;

    private void Start()
    {
        if (phaseThresholds.Count == 0)
        {
            phaseThresholds.Add(0f);
        }

        phaseThresholds.Sort();
        maxHealth = health;
    }

    public bool ShouldAttack()
    {
        return health > float.Epsilon;
    }

    public override void OnHit(float damage, Vector2 direction)
    {
        base.OnHit(damage, direction);

        for (int i = 0; i < phaseThresholds.Count; ++i)
        {
            if ((health / maxHealth) < phaseThresholds[i])
            {
                phase = phaseThresholds.Count - i;
                break;
            }
        }

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
