using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossAttack : MonoBehaviour
{
    [SerializeField] public int[] phases = { 0 };

    public virtual IEnumerator Attack()
    {
        yield return MoveToAttack();
        yield return TelegraphAttack();
        yield return DoAttack();
    }

    protected virtual IEnumerator MoveToAttack()
    {
        yield return null;
    }

    protected virtual IEnumerator TelegraphAttack()
    {
        yield return null;
    }

    protected abstract IEnumerator DoAttack();

    static protected void ShootCircleOfProjectiles(BulletScript bullet, Vector3 origin, float principleAngle, float angleBetweenShots, float radius = 0f)
    {
        float originalAngle = principleAngle;

        while (principleAngle <= originalAngle + 360f)
        {
            Vector3 dir = new Vector2(Mathf.Sin(principleAngle * Mathf.Deg2Rad), Mathf.Cos(principleAngle * Mathf.Deg2Rad));
            Instantiate(bullet, origin + dir * radius, Quaternion.Euler(0f, 0f, -principleAngle));

            principleAngle += angleBetweenShots;
        }
    }
}
