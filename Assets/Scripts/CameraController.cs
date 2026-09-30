using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float panSpeed = 20f;
    public float zoomSpeed = 5f;
    public float minSize = 5f;
    public float maxSize = 50f;

    private Vector3 lastMousePosition;
    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        // Déplacement au clic gauche
        if (Input.GetMouseButtonDown(0))
        {
            lastMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;
            Vector3 move = new Vector3(-delta.x, 0, -delta.y) * (panSpeed * 0.001f * cam.orthographicSize);

            transform.Translate(move, Space.World);
            lastMousePosition = Input.mousePosition;
        }

        // Zoom avec la molette
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll != 0f)
        {
            cam.orthographicSize -= scroll * zoomSpeed;
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minSize, maxSize);
        }
    }
}