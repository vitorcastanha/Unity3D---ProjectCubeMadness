using UnityEngine;
using System.Collections;

public class SpawnEnemies : MonoBehaviour {

    public enum EnemyType
    {
        ZOMBIE,
        MOOD_GUY
    }
    public EnemyType type;
    private const float SPAWN_TIMER = 10f;
    private float counter = 0f;

	void Start () 
    {
        SpawnEnemy(type);
	}
	
	void Update () 
    {
        counter += Time.deltaTime;

        if (counter > SPAWN_TIMER)
        {
            SpawnEnemy(type);
            counter = 0f;
        }
	}

    private void SpawnEnemy(EnemyType type)
    {
        switch (type)
        {
            case EnemyType.ZOMBIE:
                EnemyZombie zombie = PoolManager.Spawn<EnemyZombie>(transform) as EnemyZombie;
                if (zombie == null)
                    return;
                zombie.transform.position = transform.position;
                break;
            case EnemyType.MOOD_GUY:
                EnemyRanged ranged = PoolManager.Spawn<EnemyRanged>(transform) as EnemyRanged;
                if (ranged == null)
                    return;
                ranged.transform.position = transform.position;
                break;
            default:
                break;
        }   
    }
}
