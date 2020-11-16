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

    public static Action OnHit = delegate { };

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
        transform.position = new Vector3(transform.position.x + m_horizontalSpeed * Time.deltaTime, transform.position.y + m_verticalSpeed * Time.deltaTime, 0);

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


    private void OnCollisionEnter(Collision collision)
    {
        // if collides with enemy 
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Boss") )
        {
            OnHit();
            Destroy(gameObject);
        }
    }

}
