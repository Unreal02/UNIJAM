using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragTest : MonoBehaviour
{
    float distance = 10f;
    float scroll = 1f;
    Vector3 initpos;

    private void Start()
    {
        initpos = transform.position;
    }

    void OnMouseDrag()
    {
        scroll += Input.mouseScrollDelta.y * 0.1f;
        Mathf.Clamp(scroll, 1f, 5f);
        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance);
        Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector3 dir = objPosition - Camera.main.transform.position;
        transform.position = Camera.main.transform.position + dir.normalized * (10f * scroll);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.CompareTag("macaroon"))
        {
            this.gameObject.tag = "macaroon";

            if (this.gameObject.transform.parent)
            {
                int index = this.gameObject.transform.parent.GetSiblingIndex();
                CustomerManager.Instance.InputOrder(index % 5, index / 5);
                Debug.Log(CustomerManager.Instance.GetTopping(index / 5, index % 5) + " was added");
            }

        }
        else if (collision.collider.gameObject.CompareTag("shelf"))
        {

        }
        else
        {
            if (!collision.collider.gameObject.CompareTag("filling"))
            {
                Debug.Log(collision.collider.gameObject);
                transform.position = initpos;
                this.gameObject.tag = "filling";
            }
        }
    }
}
