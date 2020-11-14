using UnityEngine;

public class Boss1 : Entity
{
    [SerializeField]
    private float m_verticalSpeed;
    private Camera m_camera;


    void Update()
    {
        BossAppear();
    }

    private void BossAppear()
    {
        // set screen position limit
        Vector3 screenPos = m_camera.WorldToScreenPoint(transform.position);

        // move to the bottom until its at the desired place
        if (transform.position.y >= 2.5)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - m_verticalSpeed * Time.deltaTime, 0);
        }
    }
    protected override void Awake()
    {
        base.Awake();
        // retrieve the main camera
        m_camera = Camera.main;
    }

    private void OnCollisionEnter(Collision collision)
    {

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
