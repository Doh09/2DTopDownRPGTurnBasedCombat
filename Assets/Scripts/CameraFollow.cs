using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraFollow : MonoBehaviour
{

    public Transform target;
    public float m_speed = 0.1f;
    Camera mycam;

    // Use this for initialization
    void Start()
    {
        
        mycam = GetComponent<Camera>();
    }

    void OnEnable()
    {
        target = GameManager.instance.GetPlayer().transform;
    }

    // Update is called once per frame
    void Update()
    {

        mycam.orthographicSize = (Screen.height / 100f) / 3f;

        if (target)
        {
            transform.position = Vector3.Lerp(transform.position, target.position, m_speed) + new Vector3(0, 0, -1);
        }
    }
}
