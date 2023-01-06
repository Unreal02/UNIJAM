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
            transform.localPosition = new Vector3(0f, 4f, 0f);
            transform.localRotation = Quaternion.identity;
        }
    }
}
