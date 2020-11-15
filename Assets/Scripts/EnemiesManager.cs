using System.Collections;
using UnityEngine;

public class EnemiesManager : MonoBehaviour

{
    [System.Serializable]
    public class GroupEnemies
    {
        public string groupName;
        public GameObject enemy;
        public int count;
        public enum EnemyFormation { ARCUP, ARCDOWN, CENTER, RANDOM }
        public EnemyFormation enemyFormation = EnemyFormation.RANDOM;
    }

    [System.Serializable]
    public class Wave
    {
        public string wavename;
        public GroupEnemies[] enemiesGroup;
        public float timeBetweenGroups;
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
        // if all waves not cleared
        if (!cleared)
        {
            // if wave completed
            if (state == SpawnState.WAITING)
            {
                WaveCompleted();
                return;
            }

            // if times between waves reached
            if (waveCountdown <= 0)
            {
                // if not spawning already
                if (state != SpawnState.SPAWNING)
                {
                    // start spawning
                    StartCoroutine(SpawnWave(waves[nextWave]));
                }

            }
            else
            {
                // countdown
                waveCountdown -= Time.deltaTime;
            }
        }

    }
    private void WaveCompleted()
    {
        // update state to counting
        state = SpawnState.COUNTING;
        // reset wave countdown
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
        // update state to spwaning
        state = SpawnState.SPAWNING;
        for (int i = 0; i < wave.enemiesGroup.Length; i++)
        {
            // spawn group of enemies
            SpawnEnemies(wave.enemiesGroup[i]);

            // wait between 2 groups of enemies
            yield return new WaitForSeconds(wave.timeBetweenGroups);

        }

        // finished spawn, update state to waiting
        state = SpawnState.WAITING;
        yield break;
        
    }


    void SpawnEnemies(GroupEnemies enemies)
    {
        int max = enemies.count;
        float z = m_camera.transform.position.z;
        float x0 = Screen.width / 2;
        float r = Screen.width / 2 - 50;
        float y0 = Screen.height;
        Vector3 point;
        float angle = Mathf.PI / (max - 1); 
        for (int i = 0; i < max; i++)
        {
            // spwan in arc up shape
            if (enemies.enemyFormation == GroupEnemies.EnemyFormation.ARCDOWN)
            {
                float x = x0 + ( r * Mathf.Cos( Mathf.PI + (angle * i) ) );
                float y = y0 + r + ( r * Mathf.Sin( Mathf.PI + (angle * i) ) );
                point = m_camera.ScreenToWorldPoint(new Vector3(x, y, z));
            }
            // spawn in arc down shape
            else if (enemies.enemyFormation == GroupEnemies.EnemyFormation.ARCUP)
            {
                float x = x0 + (r * Mathf.Cos(angle * i));
                float y = y0 + (r * Mathf.Sin(angle * i));
                point = m_camera.ScreenToWorldPoint(new Vector3(x, y, z));
            }
            // spawn in arc down shape
            else if (enemies.enemyFormation == GroupEnemies.EnemyFormation.CENTER)
            {
                point = m_camera.ScreenToWorldPoint(new Vector3(x0, y0, z));
            }
            // random spawning
            else
            {
                point = m_camera.ScreenToWorldPoint(new Vector3( Random.Range(2, Screen.width - 2), Random.Range(Screen.height, y0 + r ), z) ); 

            }
            // instantiate ennemies
            Instantiate(enemies.enemy, point, enemies.enemy.transform.rotation);
        }    
        
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
