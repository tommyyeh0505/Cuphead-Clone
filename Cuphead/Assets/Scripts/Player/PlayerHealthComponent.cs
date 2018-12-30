using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthComponent : Damagable
{
    float hitFlashTime = 1f;
    int hitFlashAmount = 4;
    bool gettingHit = false;
    [SerializeField] private TMP_Text UIHealthText;

    private void Start()
    {
        UIHealthText.text = health.ToString("R");
    }

    public override void OnHit(float damage)
    {
        if (!gettingHit)
        {
            base.OnHit(damage);
            UIHealthText.text = health.ToString("R");

            StartCoroutine("HitFlash");
        }
    }

    private IEnumerator HitFlash()
    {
        gettingHit = true;
        float timeToDisappear = (hitFlashTime / hitFlashAmount) / 3;
        float timeToAppear = timeToDisappear * 2;

        for (int i = 0; i < hitFlashAmount; ++i)
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(255f, 255f, 255f, 0f);
            yield return new WaitForSeconds(timeToDisappear);
            gameObject.GetComponent<SpriteRenderer>().color = new Color(255f, 255f, 255f, 255f);
            yield return new WaitForSeconds(timeToAppear);
        }
        gettingHit = false;
    }
}