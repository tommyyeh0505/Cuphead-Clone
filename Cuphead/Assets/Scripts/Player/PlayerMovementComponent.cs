using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementComponent : MonoBehaviour
{
    public float moveSpeed; // constant move speed
    public float jumpMaxHeight; // apex of the jump
    public float jumpMaxDuration; // time it takes to reach the apex
    public float jumpLingerTime; // time after jump apex to retain jumping logic (before going into falling logic)
    public float gravity; // parabolic falling coefficient
    public float jumpMinDuration = 0.4f;

    public float jumpPrelandingTimer = 0.2f;

    public float controllerDeadzone;

    public float knockbackSpeed = 15f;
    public float knockbackHeight = 0.5f;
    public float knockbackTime = 0.2f;

    [HideInInspector] public bool isFacingRight;
    [HideInInspector] private bool hasJumped;
    [HideInInspector] private float verticalSpeed;
    [HideInInspector] private float currentJumpDuration;
    [HideInInspector] private bool grounded;
    [HideInInspector] private float jumpSinWavePeriod;
    [HideInInspector] private float initialVerticalSpeed;
    private float lastPressJumpTime;
    private Vector2 knockbackDirection;
    private float knockBackTimeRemaining = 0f;

    private BoxCollider2D boxCollider;
    private Animator animator;

    public LayerMask environmentLayer;

    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        hasJumped = false;
        animator.SetBool("animJumpBool", hasJumped);
        currentJumpDuration = 0;
        verticalSpeed = 0;

        // Precalculate some values so we don't have to do it every frame
        jumpSinWavePeriod = Mathf.PI / (2 * jumpMaxDuration);
        initialVerticalSpeed = jumpSinWavePeriod * jumpMaxHeight;
    }

    public void Knockback(Vector2 direction)
    {
        knockBackTimeRemaining = knockbackTime;

        Vector2 endLocation = direction * knockbackSpeed * knockbackTime;
        endLocation.y = Mathf.Max(endLocation.y + knockbackHeight, knockbackHeight);
        knockbackDirection = endLocation.normalized;
    }

    Vector2 UpdateKnockback(Vector2 CurrentVector)
    {
        knockBackTimeRemaining = Mathf.Max(0, knockBackTimeRemaining - Time.deltaTime);
        Vector2 knockbackVel = knockbackDirection * knockbackSpeed;
        Vector2 outPosition = ApplyHorizontalVelocity(knockbackVel.x, CurrentVector);
        outPosition = ApplyVerticalVelocity(knockbackVel.y, outPosition);

        return outPosition;
    }

    float CalculateVerticalSpeed(bool JumpDown, bool JumpHeld)
    {
        if (JumpDown)
        {
            lastPressJumpTime = Time.time;
        }

        // 3 situations:
        // 1. We are starting a jump. Only allow this if we are on the ground and if we issue a button down.
        // 2. We are free falling. Do this if we are not holding the jump button, or if jump time has expired, or if we never jumped to begin with, or if we have let go of the jump button
        // 3. We are gaining altitude / lingering in the air after the apex of our jump, because jump button is held
        if (grounded && (JumpDown || (Time.time - lastPressJumpTime < jumpPrelandingTimer))) // situation 1
        {
            lastPressJumpTime = 0f;
            currentJumpDuration = 0f;
            verticalSpeed = initialVerticalSpeed;
            hasJumped = true;
            animator.SetBool("animJumpBool", hasJumped);
            animator.ResetTrigger("animFall");
        }
        else if ((currentJumpDuration >= jumpMinDuration) && (!JumpHeld || !hasJumped || currentJumpDuration > jumpMaxDuration + jumpLingerTime)) // situation 2
        {
            // if we get in situation 2, we can never get out until we hit ground           
            animator.SetTrigger("animFall");
            hasJumped = false;
            animator.SetBool("animJumpBool", hasJumped);
            // We use a parabolic curve to model the fall. Note that we linger with the sin curve for a little bit before falling
            verticalSpeed = Mathf.Min(verticalSpeed - (gravity * Time.deltaTime), -initialVerticalSpeed);
        }
        else // situation 3
        {
            // We use a sin curve to model the jump upwards
            currentJumpDuration += Time.deltaTime;
            verticalSpeed = initialVerticalSpeed * Mathf.Cos(jumpSinWavePeriod * currentJumpDuration);
        }

        return verticalSpeed;
    }

    Vector2 ApplyVerticalVelocity(float velY, Vector2 CurrentVector)
    {
        float distance = velY * Time.deltaTime;

        Bounds bounds = boxCollider.bounds;
        RaycastHit2D hit = Physics2D.BoxCast(CurrentVector, bounds.size, 0f, new Vector2(0f, 1f), distance, environmentLayer);

        if (hit.transform == null)
        {
            grounded = false;
            animator.ResetTrigger("animGround");
            return new Vector2(CurrentVector.x, CurrentVector.y + distance);
        }
        else
        {
            bool goingUp = distance > float.Epsilon;
            float toMoveToEdge = (goingUp ? 1 : -1) * (hit.distance - 0.01f);
            hasJumped = false;

            if (!goingUp) // we have hit a floor
            {
                animator.SetTrigger("animGround");
                animator.ResetTrigger("animFall");

                verticalSpeed = 0f;
                grounded = true;
            }

            return new Vector2(CurrentVector.x, CurrentVector.y + toMoveToEdge);
        }
    }

    Vector2 ApplyHorizontalVelocity(float velX, Vector2 CurrentVector)
    {
        Bounds bounds = boxCollider.bounds;
        float distance = velX * Time.deltaTime;
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

    Vector2 InputVerticalMovement(bool JumpPressed, bool JumpHeld, Vector2 CurrentVector)
    {
        float speed = CalculateVerticalSpeed(JumpPressed, JumpHeld);
        return ApplyVerticalVelocity(speed, CurrentVector);
    }

    Vector2 InputHorizontalMovement(float horizontalAxis, Vector2 CurrentVector)
    {
        isFacingRight = horizontalAxis > 0f;
        Vector3 currentScale = gameObject.transform.localScale;
        gameObject.transform.localScale = new Vector3(horizontalAxis * Mathf.Abs(currentScale.x), currentScale.y, currentScale.z);

        return ApplyHorizontalVelocity(horizontalAxis * moveSpeed, CurrentVector);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 OutNewPosition = boxCollider.transform.position;

        if (knockBackTimeRemaining > float.Epsilon)
        {
            OutNewPosition = UpdateKnockback(OutNewPosition);
            hasJumped = false;
        }
        else
        {
            float horizontalAxis = Input.GetAxisRaw("Horizontal");
            if (Mathf.Abs(horizontalAxis) > controllerDeadzone)
            {
                animator.SetBool("animWalking", true);
                OutNewPosition = InputHorizontalMovement(Mathf.Sign(horizontalAxis), OutNewPosition);
            }
            else
            {
                animator.SetBool("animWalking", false);
            }

            bool jumpButtionDown = Input.GetButtonDown("Jump");
            bool jumpButtionHeld = Input.GetButton("Jump");
            OutNewPosition = InputVerticalMovement(jumpButtionDown, jumpButtionHeld, OutNewPosition);
        }

        transform.position = OutNewPosition;
    }
}
