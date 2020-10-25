using System.Diagnostics;
using System.Transactions;
using UnityEngine;

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
    [SerializeField]
    GameObject bullet;

    Stopwatch stopWatch;

    int score = 0;

    // Update is called once per frame
    void Update()
    {
        PlayerControl();
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
        if (Input.GetKey(KeyCode.Space) && stopWatch.ElapsedMilliseconds > projectileCD)
        {
            
            Instantiate(bullet, new Vector3(transform.position.x, transform.position.y + 1, 0), Quaternion.identity);

            // subscribe to Bullet on hit
            Bullet.OnHit = OnBulletHit;
            stopWatch.Restart();
        }


    }
    public override void Awake()
    {
        base.Awake();
        stopWatch = new Stopwatch();
        stopWatch.Start();

    }

    private void OnCollisionEnter(Collision collision)
    {
        // if collides with ennemy 
        if (collision.gameObject.CompareTag("Ennemy"))
        {
            hp -= 10;

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
        UnityEngine.Debug.Log(score);
    }
}

