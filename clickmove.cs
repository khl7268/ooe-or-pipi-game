using UnityEngine;

public class ClickMove : MonoBehaviour
{
    public Camera cam;
    public PippiController unit;

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                unit.MoveTo(hit.point);
            }
        }
    }
}