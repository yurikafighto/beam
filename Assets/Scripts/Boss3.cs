using System.Diagnostics;
using System;
using UnityEngine;
using UnityEngine.UI;


public class Boss3 : Boss
{
    private Camera m_camera;
    [SerializeField]
    private GameObject EBullet, EVoidBullet, Eshield, Barrier;
    private float m_horizontalSpeed = 0, m_verticalSpeed = 2, ProjectileCD = 200;
    private bool aimRight = true;
    private int rand;
    private int BulletFired = 3, ShieldsActive = 0, aim = 0, aimSpeed = 2;


    private Stopwatch BulletCDWatch;
    private Stopwatch LastShieldDestroyed;


    void Update()
    {
        // if not paused nor died
        if (!GameManager.Instance.GetPauseStatus() && !GameManager.Instance.IsDead())
        {
            BossMovement();
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
            SummonShields(0, -2.8f);
        }
        transform.position = new Vector3(transform.position.x, transform.position.y - m_verticalSpeed * Time.deltaTime, 0);//mouvement de base

    }

    private void FireBullet() // Bullet Pattern changes if boss has shields on field or not
    {
        if (BulletCDWatch.ElapsedMilliseconds>ProjectileCD)
        {
            if (ShieldsActive <= 0 && BulletCDWatch.ElapsedMilliseconds > ProjectileCD*3) //Bullet Pattern if shields are down (slower attaack speed) but multiple projectile
            {
                rand = UnityEngine.Random.Range(-50, 50);
                for (int i = 0; i < BulletFired; i++)
                {
                    GameObject tmp = Instantiate(EBullet, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);
                    tmp.GetComponent<EBullet>().SetSpeed(Mathf.Cos((270 + rand - (15) * (i - 2)) * Mathf.Deg2Rad), Mathf.Sin((270 + rand - (15)) * Mathf.Deg2Rad), 4);
                }
                BulletCDWatch.Restart();
            }
            else if (ShieldsActive > 0) //Bullet pattern if shields are up
            {
                rand = UnityEngine.Random.Range(-100, -80);
                GameObject tmp2 = Instantiate(EVoidBullet, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);
                tmp2.GetComponent<EBullet>().SetSpeed(Mathf.Cos((270 + rand + aim) * Mathf.Deg2Rad), Mathf.Sin((270 + rand + aim) * Mathf.Deg2Rad), 7);
                rand = UnityEngine.Random.Range(-10, 10);
                GameObject tmp3 = Instantiate(EVoidBullet, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);
                tmp3.GetComponent<EBullet>().SetSpeed(Mathf.Cos((270 + rand + aim) * Mathf.Deg2Rad), Mathf.Sin((270 + rand + aim) * Mathf.Deg2Rad), 7);
                rand = UnityEngine.Random.Range(80, 100);
                GameObject tmp4 = Instantiate(EVoidBullet, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);
                tmp4.GetComponent<EBullet>().SetSpeed(Mathf.Cos((270 + rand + aim) * Mathf.Deg2Rad), Mathf.Sin((270 + rand + aim) * Mathf.Deg2Rad), 7);
                if (aimRight)//rotate the direction the boss is aiming
                {
                    aim += aimSpeed;
                    if (aim >= 30)
                    {
                        aimRight = false;
                    }
                }
                else
                {
                    aim -= aimSpeed;
                    if (aim <= -30)
                    {
                        aimRight = true;
                    }
                }
                BulletCDWatch.Restart();
            }
        }
    }

    private void SummonShields(float dx, float dy)
    {
        GameObject tmp = Instantiate(Eshield, new Vector3(transform.position.x + dx, transform.position.y + dy, 0), transform.rotation);
        if (ShieldsActive == 0)
        {
            Barrier.SetActive(true);
        }
        ShieldsActive++;
    }

    private void CheckState()
    {
        if (hp < maxHP * 0.66 && BulletFired == 3) //summons second wave of shields
        {
            BulletFired = 5;
            aimSpeed = 4;
            SummonShields(2.8f, 0);
            SummonShields(-2.8f, 0);
        }
        if (hp < maxHP * 0.33 && BulletFired == 5) //summons third wave of shields
        {
            BulletFired = 7;
            aimSpeed = 6;
            SummonShields(2.8f, 0);
            SummonShields(0, -2.8f);
            SummonShields(-2.8f, 0);
        }
    }
    private void ShieldDestroyed()
    {
        if (LastShieldDestroyed.ElapsedMilliseconds > 100) //Prevent bugs when the functions activate multiple times when the shield is destroyed with a Laser Beam
        {
            ShieldsActive--;
            if (ShieldsActive == 0)
            {
                Barrier.SetActive(false);
            }
            LastShieldDestroyed.Restart();
        }
    }

    protected override void Awake()
    {
        base.Awake();
        // retrieve the main camera
        m_camera = Camera.main;
        OnBossAppear(true);

        EShield.ShieldDestroyed = ShieldDestroyed;

        BulletCDWatch = new Stopwatch();
        BulletCDWatch.Start();
        LastShieldDestroyed = new Stopwatch();
        LastShieldDestroyed.Start();
    }

    private void OnCollisionEnter(Collision collision)
    {

        // if collides with bullet
        if (collision.gameObject.CompareTag("Bullet") && ShieldsActive<=0) //If the boss has active shields, it doesnt take damages
        {
            hp -= 10;
        }
        // if collides with bullet
        if (collision.gameObject.CompareTag("StarSurge") && ShieldsActive <= 0)
        {
            hp -= 50;
        }

        OnBossHPChange(hp);
        // if no more hp
        if (hp <= 0)
        {
            // destroy the enemy object
            Destroy(gameObject);
            ScoreBoss();
            OnBossAppear(false);
        }
    }

}
