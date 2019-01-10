using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SushiSpawn : BossAttack
{

    public struct Spawn
    {
        public Vector2 pos;
        public int numSpawns;
        public float speed;
        public float timeDelay;
        public bool facingRight;

        public Spawn(float x, float y, int n, float s, float d, bool r)
        {
            pos = new Vector2(x, y);
            numSpawns = n;
            speed = s;
            timeDelay = d;
            facingRight = r;
        }
    }

    private SpawnScript sushi;
    private List<Spawn> spawns;

    // Start is called before the first frame update
    void Start()
    {
        spawns = new List<Spawn>();
        spawns.Add(new Spawn(50, 50, 5, 10, 1, true));
        spawns.Add(new Spawn(50, 50, 5, 2, 1, false));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override IEnumerator DoAttack()
    {

        for (int i = 0; i < spawns.Count; ++i)
        {
            Spawn currentSpawn = spawns[i];
            for(int j = 0; j < currentSpawn.numSpawns; j++)
            {
                if (j != 0)
                {
                    yield return new WaitForSeconds(currentSpawn.timeDelay);
                }
                Vector3 vec = currentSpawn.pos;
                if (currentSpawn.facingRight) {
                    Instantiate(sushi, vec, Quaternion.Euler(new Vector3(0,180,0)));
                } else
                {
                    Instantiate(sushi, vec, Quaternion.Euler(new Vector3(0, 0, 0)));
                }

            }

        }



        yield return null;

    }
}
