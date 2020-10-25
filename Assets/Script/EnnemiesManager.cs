using System.Collections;
using UnityEngine;

public class EnnemiesManager : MonoBehaviour
{    //This field gets serialized even though it is private
    //because it has the SerializeField attribute applied.
    [SerializeField]
    private GameObject ennemy;
    [SerializeField]
    private float apparitionCD;

    private Camera m_camera;

    private GameObject m_player;

    private IEnumerator coroutine;


    // Start is called before the first frame update
    void Start()
    {
        coroutine = GenerateEnnemy(apparitionCD);
        StartCoroutine(coroutine);
    }

    private IEnumerator GenerateEnnemy(float waitTime)
    {
        Vector3 point;
        while (Application.isPlaying)
        {
            yield return new WaitForSeconds(waitTime);

            point = m_camera.ScreenToWorldPoint(new Vector3(Random.Range(0, Screen.width), Screen.height, 10));
            Instantiate(ennemy, point, Quaternion.identity);
        }
    }
    private void Awake()
    {
        // retrieve the main camera
        m_camera = Camera.main;
        m_player = GameObject.Find("Player");
    }
}
