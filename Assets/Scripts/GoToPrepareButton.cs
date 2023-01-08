using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoToPrepareButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => {
            SoundManager.Instance.PlaySFXSound("s1_confirmorder");
            GameManager.Instance.GoToPrepare(); });
    }

    // Update is called once per frame
    void Update()
    {

    }
}
