using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public List<PippiController> selectedUnits = new List<PippiController>();

    private Vector3 startPos;
    private bool isDragging = false;

    void Update()
    {
        // 마우스 누름
        if (Input.GetMouseButtonDown(0))
        {
            startPos = Input.mousePosition;
            isDragging = true;
        }

        // 마우스 뗌
        if (Input.GetMouseButtonUp(0))
        {
            SelectUnits();
            isDragging = false;
        }

        // 이동 (우클릭 추천)
        if (Input.GetMouseButtonDown(1))
        {
            MoveUnits();
        }
    }
    void SelectUnits()
{
    selectedUnits.Clear();

    foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Unit"))
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(obj.transform.position);

        if (IsInside(screenPos))
        {
            PippiController unit = obj.GetComponent<PippiController>();
            selectedUnits.Add(unit);

            // 선택 표시
            obj.GetComponent<Renderer>().material.color = Color.green;
        }
        else
        {
            obj.GetComponent<Renderer>().material.color = Color.white;
        }
    }
}
bool IsInside(Vector3 screenPos)
{
    Vector3 endPos = Input.mousePosition;

    float minX = Mathf.Min(startPos.x, endPos.x);
    float maxX = Mathf.Max(startPos.x, endPos.x);
    float minY = Mathf.Min(startPos.y, endPos.y);
    float maxY = Mathf.Max(startPos.y, endPos.y);

    return screenPos.x > minX && screenPos.x < maxX &&
           screenPos.y > minY && screenPos.y < maxY;
}
void MoveUnits()
{
    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    RaycastHit hit;

    if (Physics.Raycast(ray, out hit))
    {
        if (hit.collider.CompareTag("Ground"))
        {
            foreach (var unit in selectedUnits)
            {
                unit.MoveTo(hit.point);
            }
        }
    }
}
void OnGUI()
{
    if (isDragging)
    {
        Rect rect = GetRect(startPos, Input.mousePosition);
        GUI.Box(rect, "");
    }
}

Rect GetRect(Vector3 p1, Vector3 p2)
{
    return new Rect(
        Mathf.Min(p1.x, p2.x),
        Screen.height - Mathf.Max(p1.y, p2.y),
        Mathf.Abs(p1.x - p2.x),
        Mathf.Abs(p1.y - p2.y)
    );
}}