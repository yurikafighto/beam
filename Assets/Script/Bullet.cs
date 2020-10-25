using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //This field gets serialized even though it is private
    //because it has the SerializeField attribute applied.
    [SerializeField]
    private Camera m_camera;
    [SerializeField]
    private float m_verticalSpeed, m_horizontalSpeed;

    // Update is called once per frame
    void Update()
    {
        ProjectileMouvment();
    }

    private void ProjectileMouvment()
    {
        // set screen position limit
        Vector3 screenPos = m_camera.WorldToScreenPoint(transform.position);

        // move forward
        transform.position = new Vector3(transform.position.x, transform.position.y + m_verticalSpeed * Time.deltaTime, 0);

        //// if out of screen
        if (screenPos.y >= Screen.height)
        {
            Destroy(gameObject); // destroy projectile
        }
    }

    private void Awake()
    {
        // retrieve the main camera
        m_camera = Camera.main;
    }

    //public event Action<Collision> OnHit;

    private void OnCollisionEnter(Collision collision)
    {
        //OnHit = collision => OnCollisionEnter(collision);

        // if collides with ennemy 
        if (collision.gameObject.CompareTag("Ennemy"))
        {
            Destroy(gameObject);
        }
    }

}
