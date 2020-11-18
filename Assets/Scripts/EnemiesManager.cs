using System.Collections;
using UnityEngine;
using System;

public class EnemiesManager : MonoBehaviourSingleton<EnemiesManager>

{
    [System.Serializable]
    public class GroupEnemies
    {
        public string groupName;
        public GameObject enemy;
        public int count;
        public enum EnemyFormation { ARCUP, ARCDOWN, CENTER, RANDOM, HORIZONTAL, VERTICAL }
        public float spawnCenter;
        public EnemyFormation enemyFormation = EnemyFormation.RANDOM;
    }

    [System.Serializable]
    public class Wave
    {
        public string wavename;
        public bool isBoss = false;
        public GroupEnemies[] enemiesGroup;
        public float timeBetweenGroups;
    }
    [SerializeField]
    private bool isInfiniteLevel = false;
    [SerializeField]
    private Wave[] waves;
    [SerializeField]
    private float timeBetweenWaves = 5;
    private float waveCountdown;

    private int nextWave;
    private Camera m_camera;

    private enum SpawnState { SPAWNING, WAITING, COUNTING };
    private SpawnState state;

    private bool cleared;

    public static Action advance = delegate { };

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
                if ( waveCountdown <=3  && waveCountdown >= 2 && waves[nextWave].isBoss)
                {
                    UserInterface.Instance.DisplayBossWarning();
                }
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
            if (isInfiniteLevel)
            {
                nextWave = 0;
            }
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
            else if (enemies.enemyFormation == GroupEnemies.EnemyFormation.HORIZONTAL)
            {
                float x = (Screen.width / max) * (i + 0.5f);
                float y = y0;
                point = m_camera.ScreenToWorldPoint(new Vector3(x, y, z));
            }   
            // spawn in arc down shape
            else if (enemies.enemyFormation == GroupEnemies.EnemyFormation.VERTICAL)
            {
                float x = enemies.spawnCenter * Screen.width;
                float y = y0 + 100*i;
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
                point = m_camera.ScreenToWorldPoint(new Vector3( UnityEngine.Random.Range(2, Screen.width - 2), UnityEngine.Random.Range(Screen.height, y0 + r ), z) ); 

            }
            // instantiate ennemies
            Instantiate(enemies.enemy, point, enemies.enemy.transform.rotation);
        }
        advance();
    }

    public int getterNbWave()
    {
        int nb = 0;
        for (int i = 0; i < waves.Length; i++)
        {
            nb +=waves[i].enemiesGroup.Length;
        }
        return nb;
    }

    protected override void Awake()
    {
        m_camera = Camera.main;
        nextWave = 0;
        waveCountdown = 4;
        state = SpawnState.COUNTING;
        cleared = false;
        if (isInfiniteLevel)
        {
            UserInterface.Instance.infinite();
        }
    }
}
