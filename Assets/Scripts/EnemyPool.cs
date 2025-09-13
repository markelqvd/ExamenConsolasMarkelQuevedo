using UnityEngine;
using System.Collections.Generic;

public class EnemyPool : MonoBehaviour
{
    [Header("Prefabs de enemigos")]
    public GameObject[] enemyPrefabs; 
    public int poolSize = 10;

    private Dictionary<int, List<GameObject>> pools = new Dictionary<int, List<GameObject>>();
    private static EnemyPool _instance;
    public static EnemyPool Instance => _instance;

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        
        for (int i = 0; i < enemyPrefabs.Length; i++)
        {
            pools[i] = new List<GameObject>();

            for (int j = 0; j < poolSize; j++)
            {
                GameObject obj = Instantiate(enemyPrefabs[i]);
                obj.SetActive(false);
                pools[i].Add(obj);
            }
        }
    }

    public GameObject GetEnemy(int enemyType, Vector3 position)
    {
        int index = enemyType - 1;

        if (!pools.ContainsKey(index))
        {
            Debug.LogError($"No existe pool para enemigo tipo {enemyType}");
            return null;
        }

        foreach (var enemy in pools[index])
        {
            if (!enemy.activeInHierarchy)
            {
                enemy.transform.position = position;
                enemy.GetComponent<Enemy>().ResetEnemy();
                enemy.SetActive(true);
                return enemy;
            }
        }
        
        GameObject obj = Instantiate(enemyPrefabs[index], position, Quaternion.identity);
        obj.SetActive(true);
        obj.GetComponent<Enemy>().ResetEnemy();
        pools[index].Add(obj);
        return obj;
    }
    
    public List<GameObject> GetAllEnemies()
    {
        List<GameObject> allEnemies = new List<GameObject>();

        foreach (var kvp in pools)
        {
            allEnemies.AddRange(kvp.Value);
        }

        return allEnemies;
    }
}
