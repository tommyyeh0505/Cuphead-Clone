using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public float parallaxRatio = 0.1f;
    private GameObject playerObject;
    private float parallaxAddition = 0f;

    // Start is called before the first frame update
    void Start()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (playerObject)
        {
            float originalX = transform.position.x - parallaxAddition; // minus change from last frame
            parallaxAddition = -parallaxRatio * playerObject.transform.position.x; // set new parallax
            transform.position = new Vector2(originalX + parallaxAddition, transform.position.y);
        }
    }
}
