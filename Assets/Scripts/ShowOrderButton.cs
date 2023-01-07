using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ShowOrderButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private GameObject orderDetails;

    // Start is called before the first frame update
    void Start()
    {
        orderDetails = transform.GetChild(0).gameObject;
        orderDetails.transform.GetChild(0).GetComponent<TMP_Text>().text = CustomerManager.Instance.orderText;
        orderDetails.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        orderDetails.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        orderDetails.SetActive(false);
    }
}
