using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public float minRotationTime;
    public float maxRotationTime;

    private bool scriptStarted = false;
    private BossMovement movement;
    private BossAttack attack;
    private BossHealthComponent health;

    // Start is called before the first frame update
    void Start()
    {
        movement = gameObject.GetComponent<BossMovement>();
        attack = GetComponent<BossAttack>();
        health = GetComponent<BossHealthComponent>();
    }

    private void Update()
    {
        if (!scriptStarted)
        {
            StartCoroutine("BossScript");
            scriptStarted = true;
        }
    }

    IEnumerator BossScript()
    {
        movement.StartBobbing();

        while (health.ShouldAttack())
        {
            float waitTime = attack.Attack();
            yield return new WaitForSeconds(waitTime + Random.Range(minRotationTime, maxRotationTime));
        }
    }
}
