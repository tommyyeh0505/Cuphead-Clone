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

    public float controllerDeadzone;

    [HideInInspector] public bool isFacingRight;

    [HideInInspector] private bool hasJumped;
    [HideInInspector] private float verticalSpeed;
    [HideInInspector] private float currentJumpDuration;
    [HideInInspector] private bool grounded;
    [HideInInspector] private float jumpSinWavePeriod;
    [HideInInspector] private float initialVerticalSpeed;

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
            currentJumpDuration = 0f;
            verticalSpeed = initialVerticalSpeed;
            hasJumped = true;
            animator.SetBool("animJumpBool", hasJumped);
            animator.ResetTrigger("animFall");
        }
        else if (!JumpHeld || !hasJumped || currentJumpDuration > jumpMaxDuration + jumpLingerTime) // situation 2
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

    Vector2 ApplyVerticalMovement(bool JumpPressed, bool JumpHeld, Vector2 CurrentVector)
    {
        float speed = CalculateVerticalSpeed(JumpPressed, JumpHeld);
        float distance = speed * Time.deltaTime;

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

    Vector2 ApplyHorizontalMovement(float horizontalAxis, Vector2 CurrentVector)
    {
        isFacingRight = horizontalAxis > 0f;
        Vector3 currentScale = gameObject.transform.localScale;
        gameObject.transform.localScale = new Vector3(horizontalAxis * Mathf.Abs(currentScale.x), currentScale.y, currentScale.z);

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
        Vector2 NewPosition = boxCollider.transform.position;
        if (Mathf.Abs(horizontalAxis) > controllerDeadzone)
        {
            NewPosition = ApplyHorizontalMovement(Mathf.Sign(horizontalAxis), NewPosition);
        }

        bool jumpButtionDown = Input.GetButtonDown("Jump");
        bool jumpButtionHeld = Input.GetButton("Jump");

        NewPosition = ApplyVerticalMovement(jumpButtionDown, jumpButtionHeld, NewPosition);

        transform.position = NewPosition;
    }
}
