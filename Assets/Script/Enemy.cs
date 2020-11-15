﻿using System.Diagnostics;
using UnityEngine;

public class Enemy : Entity
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
        transform.position = new Vector3(transform.position.x, transform.position.y - m_verticalSpeed * Time.deltaTime, 0);

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
            // destroy the enemy object
            Destroy(gameObject);
        }

        // if collides with bullet
        if (collision.gameObject.CompareTag("Bullet"))
        {
            hp -= 10;

            // if no more hp
            if (hp <= 0)
            {
                // destroy the enemy object
                Destroy(gameObject);

            }
        }
    }


}