using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragandMove : MonoBehaviour
{
    public Camera camera;
    bool isColl = false;    //오브젝트 충돌 여부
    Vector3 pos, initPos;
    GameObject collObj;

    void Start()
    {
        initPos = this.gameObject.transform.position;
    }

   
   
    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            shootRay();
        }
    }

    void shootRay()
    {
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log(hit.transform.tag);
            if (hit.transform.CompareTag("filling"))
            {
                MoveWithCursor(hit, ray);
            }
        }
    }

    private static void MoveWithCursor(RaycastHit hit, Ray ray)
    {
        Transform objectHit = hit.transform;
        GameObject Go = objectHit.gameObject;
        float dist = hit.distance;

        Vector3 position = ray.origin + dist * ray.direction;
        Go.transform.position = new Vector3(position.x, Go.transform.position.y, position.z);
    }



   

    /*
    void OnMouseDrag()
    {
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log(hit.transform.tag);
            MoveWithCursor(hit, ray);
            
        }

    }
    */



    /*
    void OnMouseUp()
    {
        Debug.Log(isColl);
        if (isColl == true)
        {
            this.transform.position = pos;
            this.tag = "macaroon";

            int index = this.gameObject.transform.GetSiblingIndex();
            CustomerManager.Instance.InputOrder(index % 5, index / 5);
            Debug.Log(CustomerManager.Instance.GetTopping(index / 5, index % 5) + " was added");
        }
        else
        {
            this.transform.position = initPos;
        }
    }
    */



    /*
     void Update()
     {
         if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(0))
         {
             //Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
             RaycastHit hit;
             Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

             if (Physics.Raycast(ray, out hit))
             {
                 if(hit.transform.CompareTag("filling"))
                     MoveWithCursor(hit,ray);
                 //hit.collider.gameObject.transform.position = new Vector3(collObj.transform.position.x, collObj.transform.position.y + collObj.transform.localScale.y, collObj.transform.position.z);
             }

         }
         else if (Input.GetMouseButtonUp(0))
         {
             if (isColl == true)
             {
                 this.transform.position = pos;
                 this.gameObject.tag = "macaroon";
                 pos = new Vector3(collObj.transform.position.x, collObj.transform.position.y + collObj.transform.localScale.y, collObj.transform.position.z);
             }
             else
             {

             }
         }
     }
    */

    /*
    void Click()
    {
        Debug.Log(this);
        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out hit))
        {
            hit.collider.gameObject.transform.position= Camera.main.ScreenToWorldPoint(mousePosition);
        }
        //마우스 좌표를 스크린 투 월드로 바꾸고 이 객체의 위치로 설정해 준다.
    }
    void Drop()
    {
        if (isColl == true)
        {
            hit.collider.gameObject.transform.position = pos;
            hit.collider.gameObject.tag = "macaroon";
        }
    }
    */

    /*
    void OnMouseDown()
    {
        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
        //마우스 좌표를 스크린 투 월드로 바꾸고 이 객체의 위치로 설정해 준다.
        this.transform.position = Camera.main.ScreenToWorldPoint(mousePosition);
    }
    */

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.tag=="macaroon")
        {
            float absnum;
            Vector3 collPos;
            collPos = collision.collider.gameObject.transform.position;
            collObj = collision.collider.gameObject;
            isColl = true;
            absnum = collObj.transform.localScale.y > 0 ? collObj.transform.localScale.y : -collObj.transform.localScale.y;
            pos = new Vector3(collPos.x, collPos.y + absnum, collPos.z);
        }
    }

    void onCollisonExit(Collision collision)
    {
        isColl = false;
    }

    
}