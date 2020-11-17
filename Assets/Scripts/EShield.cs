using System.Diagnostics;
using UnityEngine;
using System;

public class EShield : Entity
{
    //This field gets serialized even though it is private
    //because it has the SerializeField attribute applied.
    [SerializeField]
    private float m_verticalSpeed, m_horizontalSpeed;
    private Camera m_camera;
    [SerializeField]
    private float projectileCD;
    [SerializeField]
    private GameObject Ebullet;

    private Stopwatch stopWatch;

    public static Action ShieldDestroyed = delegate { };

    // Update is called once per frame
    void Update()
    {
        // if not paused nor died
        if (!GameManager.Instance.GetPauseStatus() && !GameManager.Instance.IsDead())
        {
            EnemyMouvment();
            FireBullet();
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
            Destroy(gameObject); // destroy enemy
        }
    }
    private void FireBullet()
    {
        if (stopWatch.ElapsedMilliseconds > projectileCD)
        {
            GameObject tmp = Instantiate(Ebullet, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);
            tmp.GetComponent<EBullet>().SetSpeed(Mathf.Cos(270 * Mathf.Deg2Rad), Mathf.Sin(270 * Mathf.Deg2Rad), 10);
            // subscribe to Bullet on hit
            stopWatch.Restart();
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

        // if collides with bullet
        if (collision.gameObject.CompareTag("Bullet"))
        {
            hp -= 10;

        }

        if (collision.gameObject.CompareTag("StarSurge"))
        {
            hp -= 50;
        }

        // if no more hp
        if (hp <= 0)
        {
            // destroy the enemy object
            Destroy(gameObject);
            ShieldDestroyed();
        }
    }


}
