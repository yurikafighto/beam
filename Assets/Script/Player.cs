using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //This field gets serialized even though it is private
    //because it has the SerializeField attribute applied.
    [SerializeField]
    private Camera m_camera;
    [SerializeField]
    private float m_verticalSpeed;
    [SerializeField]
    private float m_horizontalSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PlayerControl();
    }

    private void PlayerControl()
    {
        // set screen position limit
        Vector3 screenPos = m_camera.WorldToScreenPoint(transform.position);
        
        if (Input.GetKey(KeyCode.UpArrow) && screenPos.y <= Screen.height)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + m_horizontalSpeed * Time.deltaTime, 0);
        }
        
        if (Input.GetKey(KeyCode.DownArrow) && screenPos.y >= 0)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - m_horizontalSpeed * Time.deltaTime, 0);
        }

        if (Input.GetKey(KeyCode.RightArrow ) && screenPos.x <= Screen.width)
        {
            transform.position = new Vector3(transform.position.x + m_verticalSpeed * Time.deltaTime, transform.position.y, 0);
        }

        if (Input.GetKey(KeyCode.LeftArrow) && screenPos.x >= 0)
        {
            transform.position = new Vector3(transform.position.x - m_verticalSpeed * Time.deltaTime, transform.position.y, 0);
        }


    }
}

