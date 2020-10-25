using UnityEngine;

public class Ennemy : Entity
{
    //This field gets serialized even though it is private
    //because it has the SerializeField attribute applied.
    [SerializeField]
    private float m_verticalSpeed;
    private Camera m_camera;

    // Update is called once per frame
    void Update()
    {
        EnnemyMouvment();
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
    public override void Awake()
    {
        base.Awake();
        // retrieve the main camera
        m_camera = Camera.main;
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
