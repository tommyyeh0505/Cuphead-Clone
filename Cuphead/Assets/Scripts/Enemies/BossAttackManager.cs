using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackManager: MonoBehaviour
{
    private Dictionary<int, List<BossAttack>> attacks = new Dictionary<int, List<BossAttack>>();

    private void Start()
    { 
        // TODO: add probability weightings to attacks (making some attacks more likely than others)
        foreach (BossAttack component in GetComponents<BossAttack>())
        {
            foreach (int phase in component.phases)
            {
                if (!attacks.ContainsKey(phase))
                {
                    attacks.Add(phase, new List<BossAttack>());
                }
                attacks[phase].Add(component);
            }
        }
    }

    public IEnumerator Attack(int phase)
    {
        if (attacks.ContainsKey(phase))
        {
            BossAttack attack = attacks[phase][Random.Range(0, attacks[phase].Count)];
            yield return StartCoroutine(attack.Attack());            
        }
        else
        {
            Debug.unityLogger.Log("Boss cannot attack because are no attacks in this phase.");
        }
    }
}
