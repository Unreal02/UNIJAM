using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OvenController : MonoBehaviour
{
    private float elapsedTime = 0f;
    private bool isOvenOn = false;
    private GameObject clockHand;

    private void Awake()
    {
        clockHand = GameObject.Find("ClockHand");
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

    public void TurnOvenOn()
    {
        elapsedTime = 0f;
        isOvenOn = true;
        Debug.Log(elapsedTime);
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
