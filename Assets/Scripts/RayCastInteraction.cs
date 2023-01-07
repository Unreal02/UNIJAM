using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastInteraction : MonoBehaviour
{
    public Camera camera;
    private GameObject pan;
    private Vector3 initPanPos;
    private void Awake()
    {
        camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        pan = GameObject.FindGameObjectWithTag("Pan")?.gameObject;
        if (pan != null)
        {
            initPanPos = pan.transform.position;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            FireRay();
        }
    }

    private void FireRay()
    {
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.CompareTag("Oven"))
            {
                OvenDoorControl(hit);
            }
            if (hit.transform.CompareTag("Pan"))
            {
                MoveWithCursor(hit, ray);
            }
        }
    }

    private static void OvenDoorControl(RaycastHit hit)
    {
        OvenController OC = hit.transform.parent.GetComponent<OvenController>();
        if (OC.isDoorOpen)
        {
            OC.CloseDoor();
        }
        else
        {
            OC.OpenDoor();
        }
    }

    private static void MoveWithCursor(RaycastHit hit, Ray ray)
    {
        Transform objectHit = hit.transform;
        GameObject Go = objectHit.gameObject;
        float dist = hit.distance;

        Vector3 position = ray.origin + dist * ray.direction;
        Go.transform.position = new Vector3(position.x, position.y, Go.transform.position.z);
    }
}
