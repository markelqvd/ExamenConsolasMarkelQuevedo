[System.Serializable]
public class WaveData
{
    public int wave;
    public EnemyData[] enemies;
}

[System.Serializable]
public class EnemyData
{
    public string type;
    public float delay;
}

[System.Serializable]
public class WavesWrapper
{
    public WaveData[] waves;
}