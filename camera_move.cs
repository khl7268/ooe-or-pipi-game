using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float zoomSpeed = 10f;
    public float minZoom = 5f;
    public float maxZoom = 20f;
    public float rotateSpeed = 100f;
    
public float tiltSpeed = 1000f;

float currentTilt = 45f;
public float minTilt = 20f;
public float maxTilt = 80f;

   void Update()
{
    Move();
    Zoom();
    Rotate();
    Tilt(); // 추가
}
    void Rotate()
{
    if (Input.GetKey(KeyCode.Q))
    {
        transform.Rotate(Vector3.up, -rotateSpeed * Time.deltaTime, Space.World);
    }

    if (Input.GetKey(KeyCode.E))
    {
        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime, Space.World);
    }
}

    void Move()
{
    float h = Input.GetAxis("Horizontal");
    float v = Input.GetAxis("Vertical");

    Vector3 dir = transform.forward * v + transform.right * h;
    dir.y = 0;

    transform.position += dir * moveSpeed * Time.deltaTime;
}
void Tilt()
{
    float mouseY = Input.GetAxis("Mouse Y");

    if (Input.GetMouseButton(1)) // 우클릭 누르고 있을 때만
    {
        currentTilt -= mouseY * tiltSpeed * Time.deltaTime;
        currentTilt = Mathf.Clamp(currentTilt, minTilt, maxTilt);

        Vector3 angles = transform.eulerAngles;
        angles.x = currentTilt;
        transform.eulerAngles = angles;
    }
}

    void Zoom()
{
    float scroll = Input.GetAxis("Mouse ScrollWheel");

    Vector3 dir = transform.forward;

    transform.position += dir * scroll * zoomSpeed;
}
}