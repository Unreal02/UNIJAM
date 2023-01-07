using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OvenController : MonoBehaviour
{
    private float elapsedTime = 0f;
    private bool isOvenOn = false;
    private GameObject clockHand;
    private GameObject door;
    public bool isDoorOpen = false;

    private Quaternion destQ;
    private Quaternion initRotation;
    private Vector3 initPos;
    private Vector3 destPos;

    private void Awake()
    {
        clockHand = GameObject.Find("ClockHand");
        door = this.transform.GetChild(1).gameObject;
        destQ = Quaternion.AngleAxis(-90f, Vector3.right);
        initRotation = door.transform.rotation;
        initPos = door.transform.position;
        destPos = RotateAround(initPos, new Vector3(initPos.x, initPos.y - 1f, initPos.z + 0.25f), destQ);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)) TurnOvenOn();
        if (Input.GetKeyDown(KeyCode.KeypadEnter)) TurnOvenOff();
        if (isOvenOn) elapsedTime += Time.deltaTime;
        float angle = -30f * elapsedTime;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        clockHand.transform.GetChild(0).transform.rotation = rotation;
    }

    private Vector3 RotateAround(Vector3 position, Vector3 pivot, Quaternion rotation)
    {
        return rotation * (position - pivot) + pivot;
    }
    public void TurnOvenOn()
    {
        elapsedTime = 0f;
        isOvenOn = true;
        Debug.Log(elapsedTime);
    }

    public void OpenDoor()
    {
        StartCoroutine(DoorOpenClose(true));
    }

    public void CloseDoor()
    {
        StartCoroutine(DoorOpenClose(false));
    }

    public IEnumerator DoorOpenClose(bool isOpen)
    {
        float timeCount = 0f;
        if (isOpen)
        {
            while (timeCount < 1f)
            {
                door.transform.rotation = Quaternion.Slerp(initRotation, destQ * initRotation, timeCount);
                door.transform.position = Vector3.Slerp(initPos, destPos, timeCount);
                timeCount += Time.deltaTime;
                yield return null;
            }
            isDoorOpen = true;
            yield break;
        }
        else
        {
            while (timeCount < 1f)
            {
                door.transform.rotation = Quaternion.Slerp(initRotation, destQ * initRotation, 1 - timeCount);
                door.transform.position = Vector3.Slerp(initPos, destPos, 1 - timeCount);
                timeCount += Time.deltaTime;
                yield return null;
            }
            isDoorOpen = false;
            yield break;
        }
    }

    public void TurnOvenOff()
    {
        isOvenOn = false;
        Debug.Log(elapsedTime);
    }

    public bool CheckTimer()
    {
        if (elapsedTime < 10f || elapsedTime > 12f)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

}
