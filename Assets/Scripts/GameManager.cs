using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Phase
{
    Title,
    GetOrder,
    Prepare,
    Topping,
    Result,
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
    private bool prepareSuccess = false;
    private bool assembleSuccess = false;

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

        // 디버깅용: GetOrderPhase scene에서 시작할 경우
        if (phase == Phase.GetOrder)
        {
            GoToGetOrder();
        }
    }

    // Update is called once per frame
    void Update()
    {

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
    }

    public void Done()
    {
        // 성공/실패 판정
        if (prepareSuccess && assembleSuccess)
        {
            // 성공
        }
        else
        {
            // 실패
        }
    }
}
