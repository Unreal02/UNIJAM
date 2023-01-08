using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderDebug : MonoBehaviour
{
    public void OnButtonClicked()
    {
        int index = this.gameObject.transform.GetSiblingIndex();
        CustomerManager.Instance.InputOrder(index % 5, index / 5);
        Debug.Log(CustomerManager.Instance.GetTopping(index / 5, index % 5) + " was added");
    }

    public void CheckAnswer()
    {
        SoundManager.Instance.PlaySFXSound("s1_confirmorder");
        GameManager.Instance.toppingSuccess = CustomerManager.Instance.CheckOrder();
        Debug.Log(CustomerManager.Instance.CheckOrder());
        CustomerManager.Instance.ResetInputOrder();
        GameManager.Instance.GoToResult();
    }
}
