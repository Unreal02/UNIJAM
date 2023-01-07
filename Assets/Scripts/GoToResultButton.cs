using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoToResultButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => { GameManager.Instance.GoToResult(); });
    }

    // Update is called once per frame
    void Update()
    {

    }
}
