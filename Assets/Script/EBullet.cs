using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EBullet : MonoBehaviour
{

    [SerializeField]
    private Camera m_camera;
    [SerializeField]
    private float m_verticalSpeed, m_horizontalSpeed;
    void Update()
    {
        ProjectileMouvment();
    }
    private void ProjectileMouvment()
    {
        // set screen position limit
        Vector3 screenPos = m_camera.WorldToScreenPoint(transform.position);

        // move forward
        transform.position = new Vector3(transform.position.x + m_horizontalSpeed, transform.position.y - m_verticalSpeed * Time.deltaTime, 0);

        // if out of screen
        
        if (screenPos.y <= 0)
        {
            Destroy(gameObject); // destroy projectile
        }

    }
    private void Awake()
    {
        // retrieve the main camera
        m_camera = Camera.main;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // if collides with ennemy 
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
