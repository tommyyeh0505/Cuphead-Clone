using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    public GameObject bullet;
    Vector2 bulletPos;
    public float fireRate = 0.5f;

    private float nextFire = 0.0f;
    private bool facingRight;
    private PlayerMovementComponent movementComponent;

    // Start is called before the first frame update
    void Start()
    {
        movementComponent = gameObject.GetComponent<PlayerMovementComponent>();
    }

    // Update is called once per frame
    void Update()
    {
        facingRight = movementComponent.isFacingRight;

        //Fires when Fire1 button is pressed or held down
        if ((Input.GetButtonDown("Fire1") || Input.GetButton("Fire1") && Time.time > nextFire))
        {
            nextFire = Time.time + fireRate;
            fire();
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        //Desroys Bullet on collision with player
        //Debug.Log("collision name = " + col.gameObject.name);
        //if (col.gameObject.name == "BulletLeft(Clone)" || col.gameObject.name == "BulletRight(Clone)")
        //{
        //    Destroy(col.gameObject);
        //}
    }

    void fire()
    {
        bulletPos = transform.position;
        bulletPos += new Vector2((facingRight ? 1f : -1f) * 3f, 0);
        Instantiate(bullet, bulletPos, Quaternion.Euler(0f, 0f, 90f * (facingRight ? -1f : 1f)));
    }
}
