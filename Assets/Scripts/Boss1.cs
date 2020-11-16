using System.Diagnostics;
using System;
using UnityEngine;

public class Boss1 : Entity
{
    private Camera m_camera;
    [SerializeField]
    private GameObject Ebullet, EWarningBullet;
    private float m_horizontalSpeed=0, m_verticalSpeed=2, ProjectileCD = 1000, LaserCD = 5000, LaserChargeTime = 2000;
    private bool LaserCharging = false, moveRight = false;
    private int rand, rand2, rand3;
    private int BulletFired = 1;

    private Stopwatch BulletCDWatch;
    private Stopwatch LaserCDWatch;
    private Stopwatch LaserWatch;

    public static Action<int> OnBossHPChange = delegate { };
    public static Action<bool> OnBossAppear = delegate { };

    void Update()
    {
        // if not paused nor died
        if (!GameManager.Instance.GetPauseStatus() && !GameManager.Instance.IsDead())
        {
            BossMovement();
            FireLaser();
            FireBullet();
            CheckState(); //Triggers second phase if hp < 50%
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

        //moves left and right
        if (moveRight)
        {
            transform.position = new Vector3(transform.position.x + m_horizontalSpeed * Time.deltaTime, transform.position.y - m_verticalSpeed * Time.deltaTime, 0);
        }
        else
        {
            transform.position = new Vector3(transform.position.x - m_horizontalSpeed * Time.deltaTime, transform.position.y - m_verticalSpeed * Time.deltaTime, 0);
        }

        if (transform.position.x >= 3 && moveRight)
        {
            moveRight = false;
        }

        if (transform.position.x <= -3 && !moveRight)
        {
            moveRight = true;
        }
    }

    private void FireBullet()
    {
        if (BulletCDWatch.ElapsedMilliseconds > ProjectileCD && !LaserCharging)
        {
            for (int i = 0; i < BulletFired; i++)
            {
                rand = UnityEngine.Random.Range(-20, 20);
                GameObject tmp = Instantiate(Ebullet, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);
                tmp.GetComponent<EBullet>().SetSpeed(rand, 7);
                BulletCDWatch.Restart();
            }
        }
    }

    private void FireLaser()
    {
        if (LaserCDWatch.ElapsedMilliseconds > LaserCD)
        {
            if (!LaserCharging)
            {
                LaserCharging = true;
                rand = UnityEngine.Random.Range(-20, 20);
                rand2 = UnityEngine.Random.Range(-20, 20);
                rand3 = UnityEngine.Random.Range(-20, 20);
                GameObject tmp = Instantiate(EWarningBullet, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);
                tmp.GetComponent<EBullet>().SetSpeed(rand, 20);
                GameObject tmp2 = Instantiate(EWarningBullet, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);
                tmp2.GetComponent<EBullet>().SetSpeed(rand2, 20);
                GameObject tmp3 = Instantiate(EWarningBullet, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);
                tmp3.GetComponent<EBullet>().SetSpeed(rand3, 20);
                LaserWatch.Restart();
            }
            if (LaserWatch.ElapsedMilliseconds > LaserChargeTime)
            {
                for (int i = 0; i < 30; i++)
                {
                    GameObject tmp = Instantiate(Ebullet, new Vector3(transform.position.x + (0.01f * rand * i), transform.position.y - (i * 0.2f), 0), Quaternion.identity);
                    tmp.GetComponent<EBullet>().SetSpeed(rand, 20);
                    GameObject tmp2 = Instantiate(Ebullet, new Vector3(transform.position.x + (0.01f * rand2 * i), transform.position.y - (i * 0.2f), 0), Quaternion.identity);
                    tmp2.GetComponent<EBullet>().SetSpeed(rand2, 20);
                    GameObject tmp3 = Instantiate(Ebullet, new Vector3(transform.position.x + (0.01f * rand3 * i), transform.position.y - (i * 0.2f), 0), Quaternion.identity);
                    tmp3.GetComponent<EBullet>().SetSpeed(rand3, 20);
                }
                LaserCharging = false;
                LaserCDWatch.Restart();
                BulletCDWatch.Restart();
            }
            // subscribe to Bullet on hit
        }
    }

    private void CheckState()
    {
        if (hp < maxHP * 0.66 && BulletFired == 1) //check if the boss gets to second phase and if it isnt already
        {
            BulletFired = 2;
            ProjectileCD = 750;
            LaserCD = 4000;
            LaserChargeTime = 2000;
            m_horizontalSpeed = 1;
        }
        if (hp < maxHP * 0.33 && BulletFired == 2) //check if the boss gets to second phase and if it isnt already
        {
            BulletFired = 3;
            ProjectileCD = 500;
            LaserCD = 3000;
            LaserChargeTime = 1000;
            m_horizontalSpeed = 3;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        // retrieve the main camera
        m_camera = Camera.main;
        OnBossAppear(true);
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
            OnBossHPChange(hp);

            // if no more hp
            if (hp <= 0)
            {
                // destroy the enemy object
                Destroy(gameObject);
                OnBossAppear(false);
            }
        }
    }

    public int GetMaxHP()
    {
        return maxHP;
    }
}
