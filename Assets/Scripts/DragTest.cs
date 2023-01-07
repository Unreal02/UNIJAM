using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragTest : MonoBehaviour
{
    float distance = 10f;
    float scroll = 1f;

    void OnMouseDrag()
    {
        scroll += Input.mouseScrollDelta.y * 0.1f;
        Mathf.Clamp(scroll, 1f, 5f);
        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance);
        Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector3 dir = objPosition - Camera.main.transform.position;
        transform.position = Camera.main.transform.position + dir.normalized * (10f * scroll);
    }
}
