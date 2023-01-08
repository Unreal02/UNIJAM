using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameStartButton : MonoBehaviour
{
    public GameObject desc;

    // Start is called before the first frame update
    void Start()
    {
        TMP_Text text = desc.GetComponentInChildren<TMP_Text>();
        text.text = "�̼� : 180�� �̳��� 3���� ��ī���� �մ��� �ֹ���� �ϼ��ϼ���!\nStep1. �մ��� �ֹ��� �����޽��ϴ�.\nStep2.��ǰ�⸦ ���� �����̽� �ٸ� ���� �ӷ��� �����ϰ� Ĩ�ϴ�.\nStep3.�ӷ� ������ ���쿡 �־� �¿��� ���� ��ŭ �����ϴ�. (��Ʈ: �ð� ����!)\nStep4.�ֹ��� �´� ������ �÷� ��ī���� �ϼ��մϴ�.";
        desc.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GoToOrder()
    {
        GameManager.Instance.GoToGetOrder();
    }

    public void ActivateDescription()
    {
        desc.SetActive(true);
    }

    public void CloseDescription()
    {
        desc.SetActive(false);
    }
}
