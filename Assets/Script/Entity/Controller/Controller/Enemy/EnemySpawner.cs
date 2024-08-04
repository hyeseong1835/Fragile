using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemySpawner : MonoBehaviour
{
    public string enemyName;
    public string PrefabResourcePath => $"Entity/Controller/Enemy/{enemyName}";
    
    protected void Start()
    {
        Spawn();
    }

    public virtual EnemyController Spawn()
    {
        GameObject prefab = Resources.Load<GameObject>(PrefabResourcePath);

        GameObject instance = Instantiate(
            prefab, 
            transform.position, 
            Quaternion.identity
        );

        EnemyController enemyController = instance.GetComponent<EnemyController>();
        
        return enemyController;
    }
}
