using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class EnemyEntry
    {
        public int Enemy;
        public float Time;
    }

    [System.Serializable]
    public class WaveData
    {
        public int Wave;
        public EnemyEntry[] Enemies;
    }

    [System.Serializable]
    public class WavesWrapper
    {
        public WaveData[] Waves;
    }

    public Transform[] spawnPoints;
    public string jsonUrl = "https://kev-games-development.net/Services/WavesTest.json";

    private WavesWrapper wavesData;
    private int currentWave = 0;

    void Start()
    {
        StartCoroutine(LoadWavesFromJson());
    }

    IEnumerator LoadWavesFromJson()
    {
        UnityWebRequest www = UnityWebRequest.Get(jsonUrl);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error cargando JSON: " + www.error);
        }
        else
        {
            string jsonText = www.downloadHandler.text;
            wavesData = JsonUtility.FromJson<WavesWrapper>(jsonText);
            Debug.Log("Oleadas cargadas: " + wavesData.Waves.Length);

            StartCoroutine(SpawnWaves());
        }
    }

    IEnumerator SpawnWaves()
    {
        for (currentWave = 0; currentWave < wavesData.Waves.Length; currentWave++)
        {
            WaveData wave = wavesData.Waves[currentWave];
            
            UIManager.Instance.UpdateWave(wave.Wave);

            Debug.Log("Oleada " + wave.Wave + " iniciada");

            foreach (var enemy in wave.Enemies)
            {
                yield return new WaitForSeconds(enemy.Time);
                SpawnEnemy(enemy.Enemy);
            }

            yield return new WaitUntil(() => GameObject.FindGameObjectsWithTag("Enemy").Length == 0);
            Debug.Log("Oleada " + wave.Wave + " completada");
        }

        Debug.Log("🎉 Juego completado, victoria!");
        UIManager.Instance.ShowVictory();
    }

    void SpawnEnemy(int enemyType)
    {
        Transform player = GameObject.FindGameObjectWithTag("Player").transform;
        if (player == null) return;

        float spawnRadius = 8f;
        float angle = Random.Range(0f, Mathf.PI * 2);
        Vector3 spawnPos = player.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * spawnRadius;

        EnemyPool.Instance.GetEnemy(enemyType, spawnPos);
    }

    public void ResetSpawner()
    {
        currentWave = 0;

        foreach (var enemy in EnemyPool.Instance.GetAllEnemies())
        {
            enemy.SetActive(false);
        }

        StartCoroutine(SpawnWaves());
    }
}
