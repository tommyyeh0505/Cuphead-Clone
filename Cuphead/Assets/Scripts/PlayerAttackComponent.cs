using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackComponent : MonoBehaviour
{

    float projectileSpeed;
    float projectileDelay;
    float initialFireMode;
    float projectileFireRate;

    [HideInInspector] private float timeElapsedAfterFire;
    [HideInInspector] private float aimDirection;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
