using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunnyProjectileCircle : BossAttack
{
    public BulletScript bullet;
    public float angleBetweenShots = 30f;

    public float heightToAttack = 15f;
    public float speedMoveToAttack = 20f;

    public float telegraphTime = 1f;

    private float distanceToSpawn;

    public int fireTimes = 2;
    public float timeBetweenCircles = 0.5f;

    private float principalAngleChange;
    private float angle = 0f;
    private BossMovement movement;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        CircleCollider2D collider = GetComponent<CircleCollider2D>();
        distanceToSpawn = collider.radius * collider.transform.localScale.x;

        movement = GetComponent<BossMovement>();

        fireTimes = Mathf.Max(fireTimes, 1);
        principalAngleChange = angleBetweenShots / fireTimes;
    }

    protected override IEnumerator MoveToAttack()
    {
        movement.StopBobbing();

        while (Mathf.Abs(transform.position.y - heightToAttack) > float.Epsilon)
        {
            float newY = heightToAttack - transform.position.y;
            if (Mathf.Abs(heightToAttack - transform.position.y) > Time.deltaTime * speedMoveToAttack)
            {
                newY = Mathf.Sign(heightToAttack - transform.position.y) * Time.deltaTime * speedMoveToAttack;
            }

            movement.MoveToLocation(new Vector2(transform.position.x, transform.position.y + newY));
            yield return null;
        }
    }

    protected override IEnumerator TelegraphAttack()
    {
        float startTime = Time.time;
        float originalAngle = transform.rotation.eulerAngles.z;

        while (Time.time < startTime + telegraphTime)
        {
            float angleToRotate = originalAngle + Mathf.Lerp(0f, 360f, (Time.time - startTime) / telegraphTime);
            transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angleToRotate));
            yield return null;
        }

        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, originalAngle));
        yield return null;
    }

    protected override IEnumerator DoAttack()
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

        movement.StartBobbing();
        yield return null;
    }
}
