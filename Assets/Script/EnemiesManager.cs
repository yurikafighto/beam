using System.Collections;
using UnityEngine;

public class EnemiesManager : MonoBehaviour

{    //This field gets serialized even though it is private
     //because it has the SerializeField attribute applied.
    [System.Serializable]
    public class Wave
    {
        public enum EnemyFormation { V, FORWARDARC, RANDOM}
        public EnemyFormation enemyFormation = EnemyFormation.RANDOM;
        public GameObject enemy;
        public int count; // number of enemies
        public float rate;
        public bool isBoss;
    }

    [SerializeField]
    private Wave[] waves;
    [SerializeField]
    private float timeBetweenWaves = 5;
    public float waveCountdown;

    private int nextWave;
    private Camera m_camera;

    private enum SpawnState { SPAWNING, WAITING, COUNTING };
    private SpawnState state;

    private bool cleared;

    private void Update()
    {
        if (!cleared)
        {
            if (state == SpawnState.WAITING)
            {
                WaveCompleted();
                return;
            }

            if (waveCountdown <= 0)
            {
                if (state != SpawnState.SPAWNING)
                {
                    // start spawning
                    StartCoroutine(SpawnWave(waves[nextWave]));
                }

            }
            else
            {
                waveCountdown -= Time.deltaTime;
            }
        }

    }
    private void WaveCompleted()
    {

        state = SpawnState.COUNTING;
        waveCountdown = timeBetweenWaves;

        // if no more incoming wave
        if (nextWave + 1 > waves.Length - 1)
        {
            cleared = true;
        }
        else
        {
            nextWave++;
        }
    }

    IEnumerator SpawnWave(Wave wave)
    {
        state = SpawnState.SPAWNING;
        for (int i = 0; i < wave.count; i++)
        {
            SpawnEnemy(wave.enemy.transform);
            yield return new WaitForSeconds(1f/ wave.rate);

        }

        state = SpawnState.WAITING;
        yield break;
        
    }


    void SpawnEnemy(Transform enemy)
    {
        
        Vector3 point;
        float z = m_camera.transform.position.z;
        point = m_camera.ScreenToWorldPoint(new Vector3(Random.Range(0, Screen.width), Screen.height, z));
        Instantiate(enemy, point, enemy.rotation);
    }

    private void Awake()
    {
        m_camera = Camera.main;
        nextWave = 0;
        waveCountdown = timeBetweenWaves;
        state = SpawnState.COUNTING;
        cleared = false;
    }
}
