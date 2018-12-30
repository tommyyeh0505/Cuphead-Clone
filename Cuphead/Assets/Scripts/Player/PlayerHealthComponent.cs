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

    private PlayerMovementComponent movement;

    private void Start()
    {
        movement = GetComponent<PlayerMovementComponent>();
        UIHealthText.text = health.ToString("R");
    }

    public override void OnHit(float damage, Vector2 direction)
    {
        if (!gettingHit)
        {
            base.OnHit(damage, direction);
            UIHealthText.text = health.ToString("R");

            StartCoroutine("HitFlash");
            movement.Knockback(direction);
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