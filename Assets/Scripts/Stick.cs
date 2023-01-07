using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stick : MonoBehaviour
{
    private PrepareManager prepareManager;

    // Start is called before the first frame update
    void Start()
    {
        prepareManager = PrepareManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnMouseUpAsButton()
    {
        if (prepareManager.phase == PreparePhase.Init)
        {
            prepareManager.GoToMeringue();
            // bowl 위로
            transform.parent = FindObjectOfType<Bowl>().transform;
            transform.localPosition = new Vector3(0f, 8f, 0f);
            transform.localRotation = Quaternion.identity;
        }
    }

    public void Shake(int shakeCount)
    {
        // 좌우 왔다갔다
        if (shakeCount % 2 == 0)
        {
            transform.localRotation = Quaternion.Euler(0f, 0f, -20f);
        }
        else
        {
            transform.localRotation = Quaternion.Euler(0f, 0f, 20f);
        }
    }
}
