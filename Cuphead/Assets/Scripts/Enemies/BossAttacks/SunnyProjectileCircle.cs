using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunnyProjectileCircle : BossAttack
{
    public BulletScript bullet;
    public float angleBetweenShots = 30f;

    private float distanceToSpawn;

    public int fireTimes = 2;
    public float timeBetweenCircles = 0.5f;

    private float principalAngleChange;
    private float angle = 0f;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        CircleCollider2D collider = gameObject.GetComponent<CircleCollider2D>();
        distanceToSpawn = collider.radius * collider.transform.localScale.x;

        fireTimes = Mathf.Max(fireTimes, 1);
        principalAngleChange = angleBetweenShots / fireTimes;
    }

    public override IEnumerator Attack()
    {
        angle = 0f;
        for (int i = 0; i < fireTimes; ++i)
        {
            if (i != 0)
            {
                angle += principalAngleChange;
                yield return new WaitForSeconds(timeBetweenCircles);
            }
            ShootCircleOfProjectiles(bullet, transform.position, angle, angleBetweenShots, distanceToSpawn);
        }

        yield return null;
    }
}
