﻿using System.Diagnostics;
using System;
using UnityEngine;

public class Boss1 : Boss
{
    private Camera m_camera;
    [SerializeField]
    private GameObject EVoidBullet, ELaserBullet, EWarningBullet;
    private float m_horizontalSpeed=0, m_verticalSpeed=2, ProjectileCD = 700, LaserCD = 4000, LaserChargeTime = 1300;
    private bool LaserCharging = false, moveRight = false;
    private int rand, rand2, rand3;
    private int BulletFired = 1;

    private Stopwatch BulletCDWatch;
    private Stopwatch LaserCDWatch;
    private Stopwatch LaserWatch;


    void Update()
    {
        // if not paused nor died
        if (!GameManager.Instance.GetPauseStatus() && !GameManager.Instance.IsDead())
        {
            BossMovement();
            FireLaser();    //Boss Skill
            FireBullet();   //Boss base attack
            CheckState(); //Triggers second phase if hp < 66%...
        }

    }

    private void BossMovement()
    {
        // set screen position limit
        Vector3 screenPos = m_camera.WorldToScreenPoint(transform.position);

        // move bottom until its at the desired place
        if (transform.position.y <= 2.5 && m_verticalSpeed != 0)
        {
            m_verticalSpeed = 0;
        }

        if (!LaserCharging) //Boss stops moving when it charges laser, else moves left and right
        {
            if (moveRight)
            {
                transform.position = new Vector3(transform.position.x + m_horizontalSpeed * Time.deltaTime, transform.position.y - m_verticalSpeed * Time.deltaTime, 0);
            }
            else
            {
                transform.position = new Vector3(transform.position.x - m_horizontalSpeed * Time.deltaTime, transform.position.y - m_verticalSpeed * Time.deltaTime, 0);
            }
        }

        if (transform.position.x >= 3 && moveRight)     //moves left and right depending on its screen position
        {
            moveRight = false;
        }

        if (transform.position.x <= -3 && !moveRight)
        {
            moveRight = true;
        }
    }

    private void FireBullet() //Fire bullets in 3 random directions
    {
        if (BulletCDWatch.ElapsedMilliseconds > ProjectileCD && !LaserCharging)
        {
            for (int i = 0; i < BulletFired; i++)
            {
                rand = UnityEngine.Random.Range(-50, 50);
                GameObject tmp = Instantiate(EVoidBullet, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);
                tmp.GetComponent<EBullet>().SetSpeed(Mathf.Cos((270 + rand) * Mathf.Deg2Rad), Mathf.Sin((270 + rand) * Mathf.Deg2Rad),7);
                BulletCDWatch.Restart();
            }
        }
    }

    private void FireLaser() // Fire warning non-damaging bullet and then fire Laser in these directions
    {
        if (LaserCDWatch.ElapsedMilliseconds > LaserCD)
        {
            if (!LaserCharging)
            {
                LaserCharging = true;   //Enters charging stance if it isnt already and fire warning bullets
                rand = UnityEngine.Random.Range(-50, 50);
                rand2 = UnityEngine.Random.Range(-50, 50);
                rand3 = UnityEngine.Random.Range(-50, 50);
                for (int i = 1; i < 4; i++)
                {
                    GameObject tmp = Instantiate(EWarningBullet, new Vector3(transform.position.x + Mathf.Cos((270 + rand) * Mathf.Deg2Rad) * 1.5f*i, transform.position.y + Mathf.Sin((270 + rand) * Mathf.Deg2Rad) * 1.5f*i, 0), Quaternion.Euler(0, 0, (rand-90)));
                    tmp.GetComponent<EBullet>().SetSpeed(Mathf.Cos((270 + rand) * Mathf.Deg2Rad), Mathf.Sin((270 + rand) * Mathf.Deg2Rad), 7);
                    GameObject tmp2 = Instantiate(EWarningBullet, new Vector3(transform.position.x + Mathf.Cos((270 + rand2) * Mathf.Deg2Rad) * 1.5f*i, transform.position.y + Mathf.Sin((270 + rand2) * Mathf.Deg2Rad) * 1.5f * i, 0), Quaternion.Euler(0, 0, (rand2-90)));
                    tmp2.GetComponent<EBullet>().SetSpeed(Mathf.Cos((270 + rand2) * Mathf.Deg2Rad), Mathf.Sin((270 + rand2) * Mathf.Deg2Rad), 7);
                    GameObject tmp3 = Instantiate(EWarningBullet, new Vector3(transform.position.x + Mathf.Cos((270 + rand3) * Mathf.Deg2Rad) * 1.5f * i, transform.position.y + Mathf.Sin((270 + rand3) * Mathf.Deg2Rad) * 1.5f * i, 0), Quaternion.Euler(0, 0, (rand3-90)));
                    tmp3.GetComponent<EBullet>().SetSpeed(Mathf.Cos((270 + rand3) * Mathf.Deg2Rad), Mathf.Sin((270 + rand3) * Mathf.Deg2Rad), 7);
                }
                LaserWatch.Restart();
            }
            if (LaserWatch.ElapsedMilliseconds > LaserChargeTime) // Fire lasers after a given charging time
            {
                for (int i = 0; i < 30; i++)
                {
                    GameObject tmp = Instantiate(ELaserBullet, new Vector3(transform.position.x + Mathf.Cos((270 + rand) * Mathf.Deg2Rad)*i/4, transform.position.y + Mathf.Sin((270 + rand) * Mathf.Deg2Rad)*i/4, 0), Quaternion.Euler(0, 0, rand));
                    tmp.GetComponent<EBullet>().SetSpeed(Mathf.Cos((270 + rand) * Mathf.Deg2Rad), Mathf.Sin((270 + rand) * Mathf.Deg2Rad), 20);
                    GameObject tmp2 = Instantiate(ELaserBullet, new Vector3(transform.position.x + Mathf.Cos((270 + rand2) * Mathf.Deg2Rad) * i/4, transform.position.y + Mathf.Sin((270 + rand2) * Mathf.Deg2Rad) * i/4, 0), Quaternion.Euler(0, 0, rand2));
                    tmp2.GetComponent<EBullet>().SetSpeed(Mathf.Cos((270 + rand2) * Mathf.Deg2Rad), Mathf.Sin((270 + rand2) * Mathf.Deg2Rad), 20);
                    GameObject tmp3 = Instantiate(ELaserBullet, new Vector3(transform.position.x + Mathf.Cos((270 + rand3) * Mathf.Deg2Rad) * i/4, transform.position.y + Mathf.Sin((270 + rand3) * Mathf.Deg2Rad) * i/4, 0), Quaternion.Euler(0, 0, rand3));
                    tmp3.GetComponent<EBullet>().SetSpeed(Mathf.Cos((270 + rand3) * Mathf.Deg2Rad), Mathf.Sin((270 + rand3) * Mathf.Deg2Rad), 20);
                }
                LaserCharging = false;
                LaserCDWatch.Restart();
                BulletCDWatch.Restart();
            }
        }
    }

    private void CheckState()
    {
        if (hp < maxHP * 0.70 && BulletFired == 1)  //check if the boss gets to next phase and if it isnt already
        {
            BulletFired = 2;                        //upgrade its stats to make the fight harder
            ProjectileCD = 600;
            LaserCD = 3500;
            LaserChargeTime = 1000;
            m_horizontalSpeed = 2;
        }
        if (hp < maxHP * 0.35 && BulletFired == 2) 
        {
            BulletFired = 3;
            ProjectileCD = 500;
            LaserCD = 2500;
            LaserChargeTime = 700;
            m_horizontalSpeed = 4;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        // retrieve the main camera
        m_camera = Camera.main;
        OnBossAppear(true);     //boss movement at the beginning of the fight
        LaserWatch = new Stopwatch();
        LaserWatch.Start();
        LaserCDWatch = new Stopwatch();
        LaserCDWatch.Start();
        BulletCDWatch = new Stopwatch();
        BulletCDWatch.Start();
    }

    private void OnCollisionEnter(Collision collision)
    {

        // if collides with bullet
        if (collision.gameObject.CompareTag("Bullet"))
        {
            hp -= 10;
        }

        // if collides with StarSurge Bullet
        if (collision.gameObject.CompareTag("StarSurge"))
        {
            hp -= 50;
        }

        OnBossHPChange(hp);

        // if no more hp
        if (hp <= 0)
        {
            // destroy the enemy object and award score to player
            Destroy(gameObject);
            ScoreBoss();
            OnBossAppear(false);
        }
    }

}
