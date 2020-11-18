using System.Diagnostics;
using System;
using UnityEngine;

public class Boss2 : Boss
{
    private Camera m_camera;
    [SerializeField]
    private GameObject Ebullet, EBigBullet;
    private float m_horizontalSpeed = 0, m_verticalSpeed = 2, ProjectileCD = 800, SkillsCD = 4000, BulletFired = 3, NextSkill = 1, AngleS2 = 2860, StarTime = 25, starGap = 27;
    private int rand;
    private bool Skill2Charging = false, LastPhase;


    private Stopwatch BulletCDWatch;
    private Stopwatch SkillCDWatch;


    void Update()
    {
        // if not paused nor died
        if (!GameManager.Instance.GetPauseStatus() && !GameManager.Instance.IsDead())
        {
            BossMovement();
            FireBullet();
            FireSkills();
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
        transform.position = new Vector3(transform.position.x, transform.position.y - m_verticalSpeed * Time.deltaTime, 0);//mouvement de base
        //if (Skill2Charging)
        //{
        //    transform.position = new Vector3(transform.position.x + m_horizontalSpeed * Time.deltaTime, transform.position.y - m_verticalSpeed * Time.deltaTime, 0);
        //}

    }

    private void FireBullet()
    {
        if (BulletCDWatch.ElapsedMilliseconds > ProjectileCD && !Skill2Charging)
        {
            rand = UnityEngine.Random.Range(-50, 50);
            for (int i = 0; i < BulletFired; i++)
            {
                GameObject tmp = Instantiate(Ebullet, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);
                tmp.GetComponent<EBullet>().SetSpeed(Mathf.Cos((270 + rand - (15) * (i - 2)) * Mathf.Deg2Rad), Mathf.Sin((270 + rand - (15) * (i - 2)) * Mathf.Deg2Rad), 4);
                BulletCDWatch.Restart();
            }
        }
    }

    private void FireSkills() 
    {
        if (SkillCDWatch.ElapsedMilliseconds > SkillsCD || Skill2Charging)
        {
            if (NextSkill < 3) // Skill 1 : Fire 3 large bullets
            {
                rand = UnityEngine.Random.Range(-50, 50);
                for (int i = 0; i < 3; i++)
                {
                    GameObject tmp = Instantiate(EBigBullet, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);
                    tmp.GetComponent<EBullet>().SetSpeed(Mathf.Cos((270 + rand - (33) * (i - 1)) * Mathf.Deg2Rad), Mathf.Sin((270 + rand - (33) * (i - 1)) * Mathf.Deg2Rad), 4);
                }
                NextSkill++;
                BulletCDWatch.Restart();
                SkillCDWatch.Restart();
            }
            else if (NextSkill == 3 && BulletCDWatch.ElapsedMilliseconds > StarTime) // Skill 2 : Fire bullets all over the screen
            {
                if (!Skill2Charging)
                {
                    Skill2Charging = true;
                }
                else if (AngleS2 >= 320)
                {
                    GameObject tmp = Instantiate(Ebullet, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);
                    tmp.GetComponent<EBullet>().SetSpeed(Mathf.Cos(AngleS2 * Mathf.Deg2Rad), Mathf.Sin(AngleS2 * Mathf.Deg2Rad), 4);
                    if (!LastPhase)
                    {
                        AngleS2 -= starGap;
                    }
                    else
                    {
                        AngleS2 += starGap;
                    }
                }
                else //End of the skill
                {
                    AngleS2 = 2860;
                    Skill2Charging = false; 
                    NextSkill=1;
                }
                BulletCDWatch.Restart();
                SkillCDWatch.Restart();
            }
        }
    }


    private void CheckState()//
    {
        if (hp < maxHP * 0.60 && BulletFired == 3) //check if the boss gets to second phase and if it isnt already
        {
            BulletFired = 5;
            ProjectileCD = 600;
            SkillsCD = 2500;
            StarTime = 20; 
            starGap = 23;
        }
        if (hp < maxHP * 0.15 && !LastPhase) //check if the boss gets to thrid phase and sets it in permanant skill 2 phase
        {
            LastPhase = true;
            NextSkill = 3;
            SkillsCD = 0;
            StarTime = 15;
            starGap = 16;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        // retrieve the main camera
        m_camera = Camera.main;
        OnBossAppear(true);
        BulletCDWatch = new Stopwatch();
        BulletCDWatch.Start();
        SkillCDWatch = new Stopwatch();
        SkillCDWatch.Start();
    }

    private void OnCollisionEnter(Collision collision)
    {

        // if collides with bullet
        if (collision.gameObject.CompareTag("Bullet"))
        {
            hp -= 10;
        }
        // if collides with bullet
        if (collision.gameObject.CompareTag("StarSurge"))
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
