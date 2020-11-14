using System.Diagnostics;
using UnityEngine;

public class Ennemy : Entity
{
    //This field gets serialized even though it is private
    //because it has the SerializeField attribute applied.
    [SerializeField]
    private float m_verticalSpeed;
    private Camera m_camera;
    [SerializeField]
    private float projectileCD;
    [SerializeField]
    private GameObject Ebullet;

    private Stopwatch stopWatch;

    // Update is called once per frame
    void Update()
    {
        EnnemyMouvment();
        FireBullet();
    }

    private void EnnemyMouvment()
    {
        // set screen position limit
        Vector3 screenPos = m_camera.WorldToScreenPoint(transform.position);

        // move to the bottom
        transform.position = new Vector3(transform.position.x, transform.position.y - m_verticalSpeed * Time.deltaTime, 0);

        //// if out of bottom screen
        if (screenPos.y <= 0)
        {
            Destroy(gameObject); // destroy ennemy
        }
    }
    private void FireBullet()
    {
        if (stopWatch.ElapsedMilliseconds > projectileCD)
        {
            Instantiate(Ebullet, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);

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
        // if collides with the player
        if (collision.gameObject.CompareTag("Player"))
        {
            // destroy the ennemy object
            Destroy(gameObject);
        }

        // if collides with bullet
        if (collision.gameObject.CompareTag("Bullet"))
        {
            hp -= 10;

            // if no more hp
            if (hp <= 0)
            {
                // destroy the ennemy object
                Destroy(gameObject);

            }
        }
    }


}
