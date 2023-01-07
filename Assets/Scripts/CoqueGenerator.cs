using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoqueGenerator : MonoBehaviour
{
    private OvenController OC;
    private bool isBurnt = false;
    private bool isRaw = false;
    private string[] suffix = new string[] { "", "_BB", "_C", "_D", "_G" };

    private void Awake()
    {
        OC = GameObject.Find("OvenObject").GetComponent<OvenController>();
    }

    public void CoqueGeneration()
    {
        string fileName = "coque";
        if (PrepareManager.Instance.shakeCount > 110f) fileName += "Over";
        if (PrepareManager.Instance.shakeCount < 90f) fileName += "Under";
        if (OC.elapsedTime > 12f) isBurnt = true;
        if (OC.elapsedTime < 10f) isRaw = true;
        int index = Random.Range(0, 5);
        fileName += suffix[index];
        GameObject go = SpawnCoque(fileName);
        if (isBurnt) go.GetComponent<MeshRenderer>().material = (Material)Resources.Load("_coque_Burned", typeof(Material));
        if (isRaw) go.GetComponent<MeshRenderer>().material = (Material)Resources.Load("_coque_Raw", typeof(Material));
        isBurnt = false;
        isRaw = false;
    }


    private static GameObject SpawnCoque(string fileName)
    {
        GameObject go = (GameObject)Instantiate(Resources.Load(fileName, typeof(GameObject)));
        return go;
    }
}
