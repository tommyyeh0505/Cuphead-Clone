using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    public float attackCooldownTime = 4f;
    public BulletScript bullet;
    public float bossRadius = 2.5f;

    private bool fireAgain = true;
    private float angle = 0f;
    private float angleBetweenShots = 30f;

    private List<IEnumerator> attacks;

    private void Start()
    {
        attacks = new List<IEnumerator>();
        attacks.Add(Attack1());
        attacks.Add(Attack2());
    }

    public float Attack()
    {
        StopAllCoroutines();
        StartCoroutine("Attack1");
//        StartCoroutine(attacks[Random.Range(0, attacks.Count)]);
        return attackCooldownTime;
    }

    IEnumerator Attack1()
    {
        float shotAngle = angle;

        while (shotAngle <= angle + 360f)
        {
            Vector3 dir = new Vector2(Mathf.Sin(shotAngle * Mathf.Deg2Rad), Mathf.Cos(shotAngle * Mathf.Deg2Rad));
            Instantiate(bullet, transform.position + dir * bossRadius, Quaternion.Euler(0f, 0f, -shotAngle));

            shotAngle += angleBetweenShots;
        }

        angle += 15f;
        yield return null;  
    }

    IEnumerator Attack2()
    {
        float shotAngle = angle;
        while (shotAngle <= angle + 360f)
        {
            Vector3 dir = new Vector2(Mathf.Sin(shotAngle * Mathf.Deg2Rad), Mathf.Cos(shotAngle * Mathf.Deg2Rad));
            Instantiate(bullet, transform.position + dir * bossRadius, Quaternion.Euler(0f, 0f, -shotAngle));

            shotAngle += angleBetweenShots;
        }

        angle += 15f;
        if (fireAgain)
        {
            fireAgain = false;
            yield return new WaitForSeconds(0.5f);
        }
        else
        {
            fireAgain = true;
            yield return null;
        }       
    }
}
