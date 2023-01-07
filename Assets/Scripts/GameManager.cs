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
    private bool MeringueSuccess = false;
    private bool OvenSuccess = false;
    private bool ToppingSuccess = false;
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
        if (gameStopwatch.Elapsed.Seconds >= gameTimeLimit)
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
        SceneManager.LoadScene("PreparePhase");
        phase = Phase.Prepare;
    }

    public void GoToTopping()
    {
        SceneManager.LoadScene("ToppingPhase");
        phase = Phase.Topping;
    }

    public void GoToResult()
    {
        SceneManager.LoadScene("ResultPhase");
        phase = Phase.Result;

        if (MeringueSuccess && OvenSuccess && ToppingSuccess)
        {
            score++;
        }
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

    public void Done()
    {
        // 성공/실패 판정
        if (MeringueSuccess && OvenSuccess)
        {
            // 성공
        }
        else
        {
            // 실패
        }
    }
}
