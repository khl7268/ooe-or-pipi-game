using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 20f;
    public float rotateSpeed = 100f;

    void Update()
    {
        // 이동
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 dir = transform.forward * v + transform.right * h;
        transform.position += dir * moveSpeed * Time.deltaTime;

        // 회전 (Q E 키)
        if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(0, -rotateSpeed * Time.deltaTime, 0);
        }
        if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);
        }
    }
}