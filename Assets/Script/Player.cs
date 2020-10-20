using System.Diagnostics;
using UnityEngine;

public class Player : MonoBehaviour
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
    GameObject projectile;

    Stopwatch stopWatch;


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
            Instantiate(projectile, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);
            stopWatch.Restart();
        }


    }

    private void Awake()
    {
        stopWatch = new Stopwatch();
        stopWatch.Start();

    }
}

