using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OvenController : MonoBehaviour
{
    public float elapsedTime { get; private set; } = 0f;
    private bool isOvenOn = false;
    private GameObject clockHand;
    private GameObject door;
    public bool isDoorOpen = false;
    private bool isInTransition = false;
    public bool isPanIn = false;
    private bool isCompleted = false;

    private Quaternion destQ;
    private Quaternion initRotation;

    private void Awake()
    {
        clockHand = GameObject.Find("timerBase")?.gameObject;
        door = this.transform.GetChild(1).gameObject;
        destQ = Quaternion.AngleAxis(-80f, Vector3.right);
        initRotation = door.transform.rotation;
    }

    private void Update()
    {
        if (isOvenOn) elapsedTime += Time.deltaTime;
        float angle = 30f * elapsedTime;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        if (clockHand != null)
        {
            clockHand.transform.GetChild(0).transform.localRotation = rotation;
        }
    }

    private Vector3 RotateAround(Vector3 position, Vector3 pivot, Quaternion rotation)
    {
        return rotation * (position - pivot) + pivot;
    }

    public void TurnOvenOn()
    {
        elapsedTime = 0f;
        isOvenOn = true;
        //여기 playsfx하면 될것같은데 12초짜리라 중간에 끊는 방법이 없나
        Debug.Log(elapsedTime);
    }

    public void OpenDoor()
    {
        if (!isInTransition && !isCompleted)
        {
            StartCoroutine(DoorOpenClose(true));
        }
    }

    public void CloseDoor()
    {
        if (!isInTransition && !isCompleted)
        {
            StartCoroutine(DoorOpenClose(false));
        }
    }

    public IEnumerator DoorOpenClose(bool OpeningSequence)
    {
        float timeCount = 0f;
        isInTransition = true;
        if (OpeningSequence)
        {
            if (isOvenOn)
            {
                StartCoroutine("TurnOvenOff");
            }
            while (timeCount < 1f)
            {
                door.transform.rotation = Quaternion.Slerp(initRotation, destQ * initRotation, timeCount);
                timeCount += Time.deltaTime;
                yield return null;
            }
            isDoorOpen = true;
            isInTransition = false;
            yield break;
        }
        else
        {
            while (timeCount < 1f)
            {
                door.transform.rotation = Quaternion.Slerp(initRotation, destQ * initRotation, 1 - timeCount);
                timeCount += Time.deltaTime;
                yield return null;
            }
            isDoorOpen = false;
            isInTransition = false;
            TurnOvenOn();
            yield break;
        }
    }

    public IEnumerator TurnOvenOff()
    {
        isOvenOn = false;
        isCompleted = true;
        Debug.Log(elapsedTime);
        GameManager.Instance.ovenSuccess = CheckTimer();
        this.gameObject.GetComponent<CoqueGenerator>().CoqueNameGeneration();
        yield return new WaitForSeconds(3f);
        GameManager.Instance.GoToTopping();
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
