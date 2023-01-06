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
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GoToGetOrder()
    {
        SceneManager.LoadScene("GetOrderPhase");
        phase = Phase.GetOrder;
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
