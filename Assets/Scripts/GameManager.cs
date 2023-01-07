using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Diagnostics;
using TMPro;

public enum Phase
{
    Title,
    GetOrder,
    Prepare,
    Topping,
    Result,
    FinalResult,
}

public class GameManager : MonoBehaviour
{
    static private GameManager instance;
    static public GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
            }

            return instance;
        }
    }

    public Phase phase = Phase.Title;
    public bool meringueSuccess;
    public bool ovenSuccess;
    public bool toppingSuccess;
    private int score = 0;
    public Stopwatch gameStopwatch;
    public float gameTimeLimit = 100f;

    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.Instance != this)
        {
            Destroy(gameObject);
        }
        if (GameManager.Instance)
        {
            DontDestroyOnLoad(gameObject);
        }

        gameStopwatch = new Stopwatch();

        // 디버깅용: GetOrderPhase scene에서 시작할 경우
        if (phase == Phase.GetOrder)
        {
            GoToGetOrder();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gameStopwatch.Elapsed.TotalSeconds >= gameTimeLimit)
        {
            gameStopwatch.Reset();
            GoToFinalResult();
        }
    }

    public void GoToTitle()
    {
        SceneManager.LoadScene("Title");
        phase = Phase.Title;
    }

    public void GoToGetOrder()
    {
        StartCoroutine("_GoToGetOrder");
    }

    public IEnumerator _GoToGetOrder()
    {
        if (SceneManager.GetActiveScene().name != "GetOrderPhase")
        {
            // GetOrderPhase scene으로 이동
            AsyncOperation asyncLoadLevel = SceneManager.LoadSceneAsync("GetOrderPhase");
            while (!asyncLoadLevel.isDone)
            {
                yield return null;
            }
        }

        phase = Phase.GetOrder;
        yield return new WaitForEndOfFrame();
        CustomerManager.Instance.GenerateOrder();
        gameStopwatch.Start();
        score = 0;
    }

    public void GoToPrepare()
    {
        SoundManager.Instance.PlaySFXSound("s1_confirmorder");
        SceneManager.LoadScene("PreparePhase");
        phase = Phase.Prepare;
        meringueSuccess = false;
        ovenSuccess = false;
        toppingSuccess = false;
    }

    public void GoToTopping()
    {
        SceneManager.LoadScene("ToppingScene");
        phase = Phase.Topping;
    }

    public void GoToResult()
    {
        StartCoroutine("_GoToResult");
    }

    public IEnumerator _GoToResult()
    {
        // GetOrderPhase scene으로 이동
        AsyncOperation asyncLoadLevel = SceneManager.LoadSceneAsync("GetOrderPhase");
        while (!asyncLoadLevel.isDone)
        {
            yield return null;
        }
        phase = Phase.Result;

        // GetOrderPhaseUI 숨기기
        GameObject getOrderPhaseUI = GameObject.Find("GetOrderPhaseUI");
        getOrderPhaseUI.SetActive(false);

        UnityEngine.Debug.Log(meringueSuccess);
        UnityEngine.Debug.Log(ovenSuccess);
        UnityEngine.Debug.Log(toppingSuccess);

        // ResultUI
        GameObject resultUI = GameObject.Find("ResultPhaseUI");
        if (meringueSuccess && ovenSuccess && toppingSuccess)
        {
            // 성공
            score++;
            UnityEngine.Debug.Log("success");
            resultUI.transform.GetChild(0).gameObject.SetActive(true);
            resultUI.transform.GetChild(1).gameObject.SetActive(false);
        }
        else
        {
            // 실패
            UnityEngine.Debug.Log("fail");
            resultUI.transform.GetChild(1).gameObject.SetActive(true);
            resultUI.transform.GetChild(0).gameObject.SetActive(false);
            TMP_Text text = resultUI.transform.GetChild(1).GetComponentInChildren<TMP_Text>();

            if (!meringueSuccess)
            {
                text.text = "뭐야!\n꼬끄가 맛이\n이상하잖아요!";
            }
            else if (!ovenSuccess)
            {
                text.text = "뭐야!\n꼬끄가 제대로\n안 구워졌잖아요!";
            }
            else if (!toppingSuccess)
            {
                text.text = "뭐야!\n제가 원했던 토핑이\n아니잖아요!";
            }
            else
            {
                UnityEngine.Debug.LogError("result check logic error");
            }
        }

        yield return new WaitForSeconds(4f);

        // GetOrderPhase로 이동
        getOrderPhaseUI.SetActive(true);
        resultUI.transform.GetChild(0).gameObject.SetActive(false);
        resultUI.transform.GetChild(1).gameObject.SetActive(false);
        GoToGetOrder();
    }

    public void GoToFinalResult()
    {
        StartCoroutine("_GoToFinalResult");
    }

    private IEnumerator _GoToFinalResult()
    {
        // FinalResultPhase scene으로 이동
        AsyncOperation asyncLoadLevel = SceneManager.LoadSceneAsync("FinalResultPhase");
        while (!asyncLoadLevel.isDone)
        {
            yield return null;
        }
        phase = Phase.FinalResult;
        Destroy(GameStopwatch.Instance.gameObject);
        GameObject uiObject = GameObject.Find("FinalResultUI");
        TMP_Text text = uiObject.transform.Find("Text").GetComponent<TMP_Text>();
        text.text = string.Format("100초 동안 {0}개의 마카롱을 만들었어요.\n", score);
        if (score >= 6)
        {
            text.text += "이 가게는 성공적이네요!";
        }
        else
        {
            text.text += "이 가게는 폐업 각이네요!";
        }

        yield return new WaitForSeconds(10f);

        GoToTitle();
    }
}
