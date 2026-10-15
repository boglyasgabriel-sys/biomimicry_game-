using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float panSpeed = 20f;
    public float zoomSpeed = 10f;
    public float minSize = 5f;
    public float maxSize = 150f;
    public float startSize = 150f; // Calqué sur le Size de la capture

    private Vector3 lastMousePosition;
    private Camera cam;
    private bool isDragging = false;

    void Start()
    {
        cam = GetComponent<Camera>();

        // Application exacte du cadrage et de la taille de ta capture
        transform.position = new Vector3(-197.3508f, 200f, -190.7162f);
        cam.orthographicSize = startSize;

        isDragging = false;
        lastMousePosition = Vector3.zero;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lastMousePosition = Input.mousePosition;
            isDragging = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        if (isDragging && Input.GetMouseButton(0))
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;

            Vector3 move = new Vector3(
                -delta.x,
                0f,
                -delta.y
            ) * (panSpeed * 0.001f * cam.orthographicSize);

            transform.Translate(move, Space.World);

            lastMousePosition = Input.mousePosition;
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (Mathf.Abs(scroll) > 0.001f)
        {
            cam.orthographicSize -= scroll * zoomSpeed * 5f;

            cam.orthographicSize = Mathf.Clamp(
                cam.orthographicSize,
                minSize,
                maxSize
            );
        }
    }
}