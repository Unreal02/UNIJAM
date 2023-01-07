using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastInteraction : MonoBehaviour
{
    public Camera camera;
    private GameObject pan;
    private Vector3 initPanPos;
    private OvenController OC;

    private bool isDraggingPan = false;

    private void Awake()
    {
        camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        pan = GameObject.FindGameObjectWithTag("Pan")?.gameObject;
        if (pan != null)
        {
            initPanPos = pan.transform.position;
        }
        OC = GameObject.Find("OvenObject").GetComponent<OvenController>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            FireRayDown();
        }
        if (Input.GetMouseButton(0))
        {
            FireRay();
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (isDraggingPan)
            {
                if (OC.isDoorOpen && pan.transform.localPosition.x > -5f && pan.transform.localPosition.x < 5f)
                {
                    pan.transform.localPosition = new Vector3(0f, 1f, 15f);
                }
                else
                {
                    pan.transform.position = initPanPos;
                }
            }
            isDraggingPan = false;
        }
    }

    private void FireRay()
    {
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.CompareTag("Pan"))
            {
                MoveWithCursor(hit, ray);
            }
        }
    }
    private void FireRayDown()
    {
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.CompareTag("Oven"))
            {
                OvenDoorControl(hit);
            }
        }
    }

    private void OvenDoorControl(RaycastHit hit)
    {
        if (OC.isDoorOpen)
        {
            OC.CloseDoor();
        }
        else
        {
            OC.OpenDoor();
        }
    }

    private void MoveWithCursor(RaycastHit hit, Ray ray)
    {
        Transform objectHit = hit.transform;
        GameObject Go = objectHit.gameObject;
        float dist = hit.distance;

        Vector3 position = ray.origin + dist * ray.direction;
        Go.transform.position = new Vector3(position.x, initPanPos.y + 3.2f, position.z);
        isDraggingPan = true;
    }
}
