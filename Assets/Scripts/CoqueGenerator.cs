using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoqueGenerator : MonoBehaviour
{
    private OvenController OC;
    private bool isBurnt = false;
    private bool isRaw = false;
    private bool isGenerated = false;
    private string[] suffix = new string[] { "", "_BB", "_C", "_D", "_G" };

    private void Awake()
    {
        OC = GameObject.Find("OvenObject").GetComponent<OvenController>();
    }

    public void CoqueNameGeneration()
    {
        string fileName = "coque";
        if (PrepareManager.Instance.shakeCount > 110f) fileName += "Over";
        if (PrepareManager.Instance.shakeCount < 90f) fileName += "Under";
        if (OC.elapsedTime > 12f) isBurnt = true;
        else if (OC.elapsedTime < 10f) isRaw = true;
        int index = Random.Range(0, 5);
        fileName += suffix[index];
        GameManager.Instance.SetFileInfo(fileName, isBurnt, isRaw);
    }
}
