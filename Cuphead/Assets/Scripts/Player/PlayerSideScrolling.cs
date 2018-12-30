using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSideScrolling : MonoBehaviour
{
    public float leftBoundary;
    public float rightBoundary;

    // Update is called once per frame
    void Update()
    {
        Camera camera = Camera.main;
        float cameraNewXPos = Mathf.Clamp(transform.position.x, leftBoundary, rightBoundary);
        Vector3 oldPos = camera.transform.position;
        camera.transform.position = new Vector3(cameraNewXPos, oldPos.y, oldPos.z);
    }
}
