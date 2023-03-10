using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stick : MonoBehaviour
{
    private PrepareManager prepareManager;
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    // Start is called before the first frame update
    void Start()
    {
        prepareManager = PrepareManager.Instance;
        initialPosition = transform.localPosition;
        initialRotation = transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnMouseUpAsButton()
    {
        if (prepareManager.phase == PreparePhase.Init)
        {
            SoundManager.Instance.PlaySFXSound("s2_select");
            prepareManager.GoToMeringue();
            // bowl 위로
            transform.parent = FindObjectOfType<Bowl>().transform;
            transform.localPosition = new Vector3(0f, 8f, 0f);
            //transform.localRotation = Quaternion.identity;
            transform.localRotation = Quaternion.Euler(0f, 0f, -20f);
        }
    }

    public void Shake(int shakeCount)
    {
        // 좌우 왔다갔다
        if (shakeCount % 2 == 0)
        {
            StartCoroutine(ShakeCoroutine());
        }
        else
        {
            StartCoroutine(ShakeCoroutine());
        }
        SoundManager.Instance.PlaySFXSound("s2_meringue", 2f);
    }

    public IEnumerator ShakeCoroutine()
    {
        float elapsedTime = 0f;
        while (elapsedTime < 1f)
        {
            transform.localRotation = Quaternion.AngleAxis(30f, Vector3.up) * transform.localRotation;
            elapsedTime += Time.deltaTime * 5f;
            yield return null;
        }
        yield break;
    }

    public void OnGoToOven()
    {
        transform.parent = null;
        transform.localPosition = initialPosition;
        transform.localRotation = initialRotation;
    }
}
