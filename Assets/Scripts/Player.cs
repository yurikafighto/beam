﻿using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class Player : Entity
{
    //This field gets serialized even though it is private
    //because it has the SerializeField attribute applied.
    [SerializeField]
    private Camera m_camera;
    [SerializeField]
    private float m_verticalSpeed, m_horizontalSpeed;
    [SerializeField]
    // time between projectiles
    private float projectileCD;
    private float spellCD1 = 5000;
    private float spellCD2 = 10000;
    private float spellCD3 = 20000;
    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private Image CDcover, CDcover2, CDcover3;

    private Stopwatch stopWatchBullet;
    private Stopwatch stopSpell1;
    private Stopwatch stopSpell2;
    private Stopwatch stopSpell3;
    private int score = 0;
    
    // delegate action
    public static Action<int> OnHPChange = delegate { };
    public static Action<int> OnScoreChange = delegate { };


    // Update is called once per frame
    void Update()
    {
        // if not paused nor died
        if (!GameManager.Instance.GetPauseStatus() && !GameManager.Instance.IsDead())
        {
            PlayerControl();
            CDmanager();
        }            
    }
    
    private void PlayerControl()
    {
        // set screen position limit
        Vector3 screenPos = m_camera.WorldToScreenPoint(transform.position);
        
        // move forward
        if (Input.GetKey(KeyCode.UpArrow) && screenPos.y <= Screen.height)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + m_verticalSpeed * Time.deltaTime, 0);
        }

        // move backward
        if (Input.GetKey(KeyCode.DownArrow) && screenPos.y >= 0)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - m_verticalSpeed * Time.deltaTime, 0);
        }

        // move right
        if (Input.GetKey(KeyCode.RightArrow ) && screenPos.x <= Screen.width)
        {
            transform.position = new Vector3(transform.position.x + m_horizontalSpeed * Time.deltaTime, transform.position.y, 0);
        }

        // move left
        if (Input.GetKey(KeyCode.LeftArrow) && screenPos.x >= 0)
        {
            transform.position = new Vector3(transform.position.x - m_horizontalSpeed * Time.deltaTime, transform.position.y, 0);
        }

        // fire bullet
        if (Input.GetKey(KeyCode.Space) && stopWatchBullet.ElapsedMilliseconds > projectileCD)
        {
            
            Instantiate(bullet, new Vector3(transform.position.x, transform.position.y + 1, 0), Quaternion.identity);
            // subscribe to Bullet on hit
            Bullet.OnHit = OnBulletHit;
            stopWatchBullet.Restart();
        }
        if (Input.GetKey(KeyCode.X) && stopSpell1.ElapsedMilliseconds > spellCD1)
        {
            for (int i = 0; i < 10; i++)
            {
                Instantiate(bullet, new Vector3(transform.position.x, transform.position.y + (i / 2), 0), Quaternion.identity);
            }
            stopSpell1.Restart();
        }
        if (Input.GetKey(KeyCode.C) && stopSpell2.ElapsedMilliseconds > spellCD2)
        {
            for (int i = 0; i < 10; i++)
            {
                Instantiate(bullet, new Vector3(transform.position.x, transform.position.y + (i / 2), 0), Quaternion.identity);
            }
            stopSpell2.Restart();
        }
        if (Input.GetKey(KeyCode.V) && stopSpell3.ElapsedMilliseconds > spellCD3)
        {
            for (int i = 0; i < 10; i++)
            {
                Instantiate(bullet, new Vector3(transform.position.x, transform.position.y + (i / 2), 0), Quaternion.identity);
            }
            stopSpell3.Restart();
        }

    }
    protected override void Awake()
    {
        base.Awake();
        stopWatchBullet = new Stopwatch();
        stopWatchBullet.Start();
        stopSpell1 = new Stopwatch();
        stopSpell1.Start();
        stopSpell2 = new Stopwatch();
        stopSpell2.Start();
        stopSpell3 = new Stopwatch();
        stopSpell3.Start();

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Boss"))
        {
            hp -= 1000;
            // update HP bar
            OnHPChange(hp);

            // if no more hp
            if (hp <= 0)
            {
                // destroy the player object
                Destroy(gameObject);
            }
        }

        // if collides with enemy 
        if (collision.gameObject.CompareTag("EBullet20"))
        {
            hp -= 20;
            // update HP bar
            OnHPChange(hp);

            // if no more hp
            if (hp <= 0)
            {
                // destroy the player object
                Destroy(gameObject);
            }
        }

        // if collides with enemy 
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("EBullet10"))
        {
            hp -= 10;
            // update HP bar
            OnHPChange(hp);

            // if no more hp
            if (hp <= 0)
            {
                // destroy the player object
                Destroy(gameObject);
            }
        }

        // if collides with enemy 
        if (collision.gameObject.CompareTag("EBullet"))
        {
            hp -= 5;
            // update HP bar
            OnHPChange(hp);

            // if no more hp
            if (hp <= 0)
            {
                // destroy the player object
                Destroy(gameObject);
            }
        }

    }

    private void OnBulletHit()
    {
        score++;
        OnScoreChange(score);
    }

    public int GetMaxHP()
    {
        return maxHP;
    }

    public void CDmanager()
    {
        CDcover.GetComponent<Image>().fillAmount = 1 - (stopSpell1.ElapsedMilliseconds / spellCD1);
        //CDcover2.GetComponent<Image>().fillAmount = 1 - (stopSpell2.ElapsedMilliseconds / spellCD2);
        //CDcover3.GetComponent<Image>().fillAmount = 1 - (stopSpell3.ElapsedMilliseconds / spellCD3);
    }


}

