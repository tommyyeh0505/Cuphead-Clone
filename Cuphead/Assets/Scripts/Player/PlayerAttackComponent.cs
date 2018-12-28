using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackComponent : MonoBehaviour
{
    public GameObject bullet;
    public float fireRate = 0.5f;
    public float upDeadZone = 0.4f;
    public float distanceAwayToSpawnProjectile = 3f;

    private float nextFire = 0.0f;
    private PlayerMovementComponent movementComponent;

    // Start is called before the first frame update
    void Start()
    {
        movementComponent = gameObject.GetComponent<PlayerMovementComponent>();
    }

    Vector2 GetFireDirection(float vertical, float horizontal)
    {
        bool facingUp = false;
        float xDir = 0f;

        if (vertical > upDeadZone)
        {
            facingUp = true;
        }

        if (!facingUp)
        {
            xDir = movementComponent.isFacingRight ? 1f : -1f;
        }

        return new Vector2(xDir, facingUp ? 1f : 0f).normalized;
    }

    // Update is called once per frame
    void Update()
    {
        //Fires when Fire1 button is pressed or held down
        if ((Input.GetButtonDown("Fire1") || Input.GetButton("Fire1") && Time.time > nextFire))
        {
            float verticalAxis = Input.GetAxisRaw("Vertical");
            float horizontalAxis = Input.GetAxisRaw("Horizontal");

            Vector2 fireDirection = GetFireDirection(verticalAxis, horizontalAxis);
            nextFire = Time.time + fireRate;
            Fire(fireDirection);
        }
    }

    void Fire(Vector2 fireDirection)
    {
        float angle = Vector2.SignedAngle(new Vector2(0f, 1f), fireDirection);
        Vector3 bulletPos = transform.position + (Vector3)(fireDirection * distanceAwayToSpawnProjectile);
        Instantiate(bullet, bulletPos, Quaternion.Euler(0f, 0f, angle));
    }
}
