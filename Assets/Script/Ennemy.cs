using UnityEngine;

public class Ennemy : MonoBehaviour
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

    private void Awake()
    {
        // retrieve the main camera
        m_camera = Camera.main;
    }

}
