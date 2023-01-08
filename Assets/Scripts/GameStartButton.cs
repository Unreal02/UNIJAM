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
        text.text = "미션 : 180초 이내에 3개의 마카롱을 손님의 주문대로 완성하세요!\nStep1. 손님의 주문을 접수받습니다.\nStep2.거품기를 집고 스페이스 바를 눌러 머랭을 적절하게 칩니다.\nStep3.머랭 반죽을 오븐에 넣어 태우지 않을 만큼 굽습니다. (힌트: 시간 엄수!)\nStep4.주문에 맞는 토핑을 올려 마카롱을 완성합니다.";
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
