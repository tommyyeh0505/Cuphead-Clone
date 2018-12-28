using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    public float timeBetweenAttacks = 2f;
    public BulletScript bullet;
    public float bossRadius = 2.5f;
    bool fireAgain = true;

    bool shouldAttack = true;

    private float angle = 0f;
    private float angleBetweenShots = 30f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("Attack1");
    }

    IEnumerator Attack1()
    {
        while (shouldAttack)
        {
            float shotAngle = angle;

            while (shotAngle <= angle + 360f)
            {
                Vector3 dir = new Vector2(Mathf.Sin(shotAngle * Mathf.Deg2Rad), Mathf.Cos(shotAngle * Mathf.Deg2Rad));
                Instantiate(bullet, transform.position + dir * bossRadius, Quaternion.Euler(0f, 0f, -shotAngle));

                shotAngle += angleBetweenShots;
            }

            angle += 15f;
            yield return new WaitForSeconds(timeBetweenAttacks);
        }
    }

    IEnumerator Attack2()
    {
        while (shouldAttack)
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
                yield return new WaitForSeconds(3);
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
