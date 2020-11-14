using System.Diagnostics;
using System.Collections;
using UnityEngine;

public class EnnemiesManager : MonoBehaviour
{    //This field gets serialized even though it is private
    //because it has the SerializeField attribute applied.
    [SerializeField]
    private GameObject ennemy, boss;
    [SerializeField]
    private float apparitionCD;

    private Camera m_camera;

    private GameObject m_player;

    private Stopwatch BossStopWatch;

    private IEnumerator coroutine;

    private bool BossSpawned;


    // Start is called before the first frame update
    void Start()
    {
        coroutine = GenerateEnnemy(apparitionCD);
        StartCoroutine(coroutine);
    }

    private IEnumerator GenerateEnnemy(float waitTime)
    {
        Vector3 point;
        while (Application.isPlaying && !BossSpawned)
        {
            if (BossStopWatch.ElapsedMilliseconds < 10000)//check si le boss doit spawn
            {
                yield return new WaitForSeconds(waitTime);

                point = m_camera.ScreenToWorldPoint(new Vector3(Random.Range(0, Screen.width), Screen.height, 10));
                Instantiate(ennemy, point, Quaternion.identity);
            }
            else
            {
                BossSpawned = true;
                yield return new WaitForSeconds(5);

                point = m_camera.ScreenToWorldPoint(new Vector3(Screen.width/2, Screen.height+3, 10));
                Instantiate(boss, point, Quaternion.identity);
            }
        }
    }

    private void Awake()
    {
        // retrieve the main camera
        m_camera = Camera.main;
        m_player = GameObject.Find("Player");
        BossSpawned = false;
        BossStopWatch = new Stopwatch();
        BossStopWatch.Start();
        
    }
}
