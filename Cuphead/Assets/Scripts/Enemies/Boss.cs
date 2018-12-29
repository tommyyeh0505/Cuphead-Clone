using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public float minRotationTime;
    public float maxRotationTime;

    private bool scriptStarted = false;
    private BossMovement movement;
    private BossAttackManager attack;
    private BossHealthComponent health;

    // Start is called before the first frame update
    void Start()
    {
        movement = gameObject.GetComponent<BossMovement>();
        attack = GetComponent<BossAttackManager>();
        health = GetComponent<BossHealthComponent>();
    }

    private void Update()
    {
        if (!scriptStarted)
        {
            StartCoroutine(BossScript());
            scriptStarted = true;
        }
    }

    IEnumerator BossScript()
    {
        while (health.ShouldAttack())
        {
            yield return StartCoroutine(attack.Attack(health.phase));
            yield return new WaitForSeconds(Random.Range(minRotationTime, maxRotationTime));
        }
    }
}
