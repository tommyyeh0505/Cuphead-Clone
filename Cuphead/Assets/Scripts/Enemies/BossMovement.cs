using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMovement : MonoBehaviour
{
    public float bobXMagnitude = 2f;
    public float bobYMagnitude = 3f;
    public float bobYPeriod = 1.2f;
    public float bobXPeriod = 4f;
    public float movementSpeed = 5f;
    public float moveRightBoundary = 19f;
    public float moveLeftBoundary = -19f;

    private Rigidbody2D rg2d;
    private Vector3 origin;
    private bool bobStarted = false;
    private float bobStartTime;
    private float direction = 1f;

    // Start is called before the first frame update
    void Start()
    {
        rg2d = GetComponent<Rigidbody2D>();
        StartBobbing();
    }

    // Update is called once per frame
    void Update()
    {
        Bobbing();
    }

    void Bobbing()
    {
        if (!bobStarted)
        {
            return;
        }

        float bobberY = bobYMagnitude * Mathf.Sin(bobYPeriod * (Time.time - bobStartTime));
        float bobberX = bobXMagnitude * Mathf.Sin(bobXPeriod * (Time.time - bobStartTime));

        if (origin.x > moveRightBoundary || origin.x < moveLeftBoundary)
        {
            direction = -direction;
        }
        origin.x = origin.x + (direction * movementSpeed * Time.deltaTime);

        rg2d.MovePosition(origin + new Vector3(bobberX, bobberY, 0f));
    }

    public void StartBobbing()
    {
        bobStarted = true;
        origin = rg2d.transform.position;
        bobStartTime = Time.time;
    }

    public void StopBobbing()
    {
        bobStarted = false;
    }

    public void MoveToLocation(Vector2 newPosition)
    {
        rg2d.MovePosition(newPosition);
    }
}
