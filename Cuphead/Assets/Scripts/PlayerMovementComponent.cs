using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementComponent : MonoBehaviour
{
    public float moveSpeed;
    public float initialJumpSpeed;
    public float jumpMaxDuration;
    public float gravity;
    public float initialFallSpeed;

    [HideInInspector] private bool hasJumped;
    [HideInInspector] private float verticalSpeed;
    [HideInInspector] private float currentJumpDuration;
    [HideInInspector] private bool grounded;

    private Rigidbody2D rigidBody2D;
    private BoxCollider2D boxCollider;

    public LayerMask environmentLayer;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        hasJumped = false;
        currentJumpDuration = 0;
        verticalSpeed = 0;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
    }

    float CalculateVerticalSpeed(bool JumpDown, bool JumpHeld)
    {
        // 3 situations:
        // 1. We are starting a jump. Only allow this if we are on the ground and if we issue a button down.
        // 2. We are free falling. Do this if we are not holding the jump button, or if jump time has expired, or if we never jumped to begin with, or if we have let go of the jump button
        // 3. We are gaining altitude / lingering in the air after the apex of our jump, because jump button is held
        if (grounded && JumpDown) // situation 1
        {
            verticalSpeed = initialJumpSpeed;
            currentJumpDuration = 0f;
            hasJumped = true;
        }
        else if (!JumpHeld || !hasJumped || currentJumpDuration > jumpMaxDuration) // situation 2
        {
            // if we get in situation 2, we can never get out until we hit ground
            hasJumped = false;
            verticalSpeed = Mathf.Min(verticalSpeed - (gravity * Time.deltaTime), -initialFallSpeed);
        }
        else // situation 3
        {
            currentJumpDuration += Time.deltaTime;
            verticalSpeed = initialJumpSpeed;
        }

        return verticalSpeed;
    }

    Vector2 ApplyVerticalMovement(bool JumpPressed, bool JumpHeld, Vector2 CurrentVector)
    {
        CalculateVerticalSpeed(JumpPressed, JumpHeld);
        float distance = verticalSpeed * Time.deltaTime;

        Bounds bounds = boxCollider.bounds;
        RaycastHit2D hit = Physics2D.BoxCast(CurrentVector, bounds.size, 0f, new Vector2(0f, 1f), distance, environmentLayer);

        if (hit.transform == null)
        {
            grounded = false;
            return new Vector2(CurrentVector.x, CurrentVector.y + distance);
        }
        else
        {
            bool goingUp = distance > float.Epsilon;
            float toMoveToEdge = (goingUp ? 1 : -1) * (hit.distance - 0.01f);

            if (!goingUp) // we have hit a floor
            {
                hasJumped = false;
                verticalSpeed = 0f;
                grounded = true;
            }

            return new Vector2(CurrentVector.x, CurrentVector.y + toMoveToEdge);
        }
    }

    Vector2 ApplyHorizontalMovement(float horizontalAxis, Vector2 CurrentVector)
    {
        Bounds bounds = boxCollider.bounds;
        float distance = horizontalAxis * moveSpeed * Time.deltaTime;
        RaycastHit2D hit = Physics2D.BoxCast(CurrentVector, bounds.size, 0f, new Vector2(1f, 0f), distance, environmentLayer);

        if (hit.transform == null)
        {
            return new Vector2(CurrentVector.x + distance, CurrentVector.y);
        }
        else
        {
            bool toRight = distance > float.Epsilon;
            float toMoveToEdge = (toRight ? 1 : -1) * (hit.distance - 0.01f);
            return new Vector2(CurrentVector.x + toMoveToEdge, CurrentVector.y);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Input.GetAxisRaw("Vertical");
        float horizontalAxis = Input.GetAxisRaw("Horizontal");

        bool jumpButtionDown = Input.GetButtonDown("Jump");
        bool jumpButtionHeld = Input.GetButton("Jump");

        Vector2 WithHorizontal = ApplyHorizontalMovement(horizontalAxis, rigidBody2D.position);
        Vector2 WithHorizontalAndVertical = ApplyVerticalMovement(jumpButtionDown, jumpButtionHeld, WithHorizontal);

        rigidBody2D.MovePosition(WithHorizontalAndVertical);
    }
}
