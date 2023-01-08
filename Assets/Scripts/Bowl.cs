using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bowl : MonoBehaviour
{
    private Vector3 initialPosition;
    private GameObject meringue;
    private GameObject pan;
    private GameObject panMeringue;
    public bool isBowlDragging=false;

    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.localPosition;
        meringue = transform.GetChild(1).gameObject;
        meringue.transform.localScale = new Vector3(1f, 0.001f, 1f);
        pan = GameObject.Find("Pan");
        panMeringue = pan.transform.GetChild(0).gameObject;
        panMeringue.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Shake(int shakeCount)
    {
        meringue.transform.localScale = new Vector3(1f, shakeCount / 100f, 1f);
    }

    private void OnMouseDrag()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float y = ray.direction.y;
        float dist = (ray.origin.y - 0.5f) / Mathf.Abs(y);
        transform.localPosition = ray.GetPoint(dist);
        isBowlDragging = true;
    }

    private void OnMouseUpAsButton()
    {
        if (PrepareManager.Instance.phase != PreparePhase.Meringue)
        {
            transform.localPosition = initialPosition;
            isBowlDragging = false;
            return;
        }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // 쟁반으로 옮겼는지 확인
        if (Mathf.Abs(transform.position.x - pan.transform.position.x) < 5 &&
            Mathf.Abs(transform.position.z - pan.transform.position.z) < 5)
        {
            PrepareManager.Instance.GoToOven();
            panMeringue.SetActive(true);
            panMeringue.transform.localScale = meringue.transform.localScale;
            meringue.gameObject.SetActive(false);
            SoundManager.Instance.PlaySFXSound("s2_select");
        }

        transform.localPosition = initialPosition;
        isBowlDragging = false;
    }
}
