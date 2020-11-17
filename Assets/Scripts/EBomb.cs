using System.Diagnostics;
using UnityEngine;

public class EBomb : Entity
{
    //This field gets serialized even though it is private
    //because it has the SerializeField attribute applied.
    [SerializeField]
    private float m_verticalSpeed, m_horizontalSpeed;
    private Camera m_camera;
    [SerializeField]
    private GameObject Ebullet;

    private Stopwatch stopWatch;

    // Update is called once per frame
    void Update()
    {
        // if not paused nor died
        if (!GameManager.Instance.GetPauseStatus() && !GameManager.Instance.IsDead())
        {
            EnemyMouvment();
        }
    }

    private void EnemyMouvment()
    {
        // set screen position limit
        Vector3 screenPos = m_camera.WorldToScreenPoint(transform.position);

        // move to the bottom
        transform.position = new Vector3(transform.position.x + m_horizontalSpeed * Time.deltaTime, transform.position.y - m_verticalSpeed * Time.deltaTime, 0);

        //// if out of bottom screen
        if (screenPos.y <= 0)
        {
            for (int i = 0; i < 6; i++)
            {
                GameObject tmp = Instantiate(Ebullet, new Vector3(transform.position.x, transform.position.y+0.1f, 0), Quaternion.identity);
                tmp.GetComponent<EBullet>().SetSpeed(Mathf.Cos((60 * i) * Mathf.Deg2Rad), Mathf.Sin((60 * i) * Mathf.Deg2Rad), 10);
            }
            Destroy(gameObject); // destroy enemy
        }
    }

    protected override void Awake()
    {
        base.Awake();
        // retrieve the main camera
        m_camera = Camera.main;

        stopWatch = new Stopwatch();
        stopWatch.Start();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // if collides with the player
        if (collision.gameObject.CompareTag("Player"))
        {
            hp = 0;
        }

        // if collides with bullet
        if (collision.gameObject.CompareTag("Bullet"))
        {
            hp -= 10;
        }

        if (collision.gameObject.CompareTag("StarSurge"))
        {
            hp -= 100;
        }
        // if no more hp
        if (hp <= 0)
        {
            // destroy the enemy object
            for (int i = 0; i < 6; i++)
            {
                GameObject tmp = Instantiate(Ebullet, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);
                tmp.GetComponent<EBullet>().SetSpeed(Mathf.Cos((60 * i) * Mathf.Deg2Rad ), Mathf.Sin((60 * i) * Mathf.Deg2Rad), 10);
            }
            Destroy(gameObject);

        }
    }


}