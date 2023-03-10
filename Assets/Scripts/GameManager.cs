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
    public float gameTimeLimit = 180f;

    public string fileName { get; private set; }
    public bool isBurnt { get; private set; }
    public bool isRaw { get; private set; }

    public void SetFileInfo(string f, bool b, bool r)
    {
        fileName = f;
        isBurnt = b;
        isRaw = r;
    }
    public GameObject SpawnCoque()
    {
        GameObject go = (GameObject)Instantiate(Resources.Load(fileName, typeof(GameObject)));
        if (isBurnt) go.GetComponent<MeshRenderer>().material = (Material)Resources.Load("_coque_Burned", typeof(Material));
        else if (isRaw) go.GetComponent<MeshRenderer>().material = (Material)Resources.Load("_coque_Raw", typeof(Material));
        isBurnt = false;
        isRaw = false;
        return go;
    }

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
        score = 0;
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
        StartCoroutine("_GoToTopping");
    }

    private IEnumerator _GoToTopping()
    {
        // ToppingScene scene으로 이동
        AsyncOperation asyncLoadLevel = SceneManager.LoadSceneAsync("ToppingScene");
        while (!asyncLoadLevel.isDone)
        {
            yield return null;
        }
        phase = Phase.Topping;

        GameObject coqueRef = SpawnCoque();

        GameObject coque = GameObject.Find("coque");
        GameObject coqueTop = GameObject.Find("coque_top");

        coque.GetComponent<MeshFilter>().mesh = coqueRef.GetComponent<MeshFilter>().mesh;
        coqueTop.GetComponent<MeshFilter>().mesh = coqueRef.GetComponent<MeshFilter>().mesh;
        coque.GetComponent<MeshRenderer>().material = coqueRef.GetComponent<MeshRenderer>().material;
        coqueTop.GetComponent<MeshRenderer>().material = coqueRef.GetComponent<MeshRenderer>().material;
    }

    public void GoToResult()
    {
        StartCoroutine("_GoToResult");
    }

    public IEnumerator _GoToResult()
    {
        // 완성된 마카롱 전달
        List<GameObject> macaron = new List<GameObject>();
        GameObject coque = GameObject.Find("coque");
        GameObject coqueTop = GameObject.Find("coque_top");
        macaron.Add(coque);
        macaron.Add(coqueTop);
        for (int i = 0; i < 10; i++)
        {
            GameObject filling = GameObject.Find(string.Format("filling0{0}", i));
            if (Mathf.Abs(filling.transform.position.x - coque.transform.position.x) <= 5f &&
                Mathf.Abs(filling.transform.position.z - coque.transform.position.z) <= 5f)
                filling.transform.parent = null;
            macaron.Add(filling);
        }

        foreach (GameObject i in macaron)
        {
            DontDestroyOnLoad(i);
        }

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
            SoundManager.Instance.StopBGMSound();
            SoundManager.Instance.PlaySFXSound("s1_success_ending");
        }
        else
        {
            // 실패
            UnityEngine.Debug.Log("fail");
            resultUI.transform.GetChild(1).gameObject.SetActive(true);
            resultUI.transform.GetChild(0).gameObject.SetActive(false);
            TMP_Text text = resultUI.transform.GetChild(1).GetComponentInChildren<TMP_Text>();
            SoundManager.Instance.StopBGMSound();
            SoundManager.Instance.PlaySFXSound("s1_fail_ending");

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
        foreach (GameObject i in macaron)
        {
            Destroy(i);
        }
        SoundManager.Instance.StopSFXSound();
        SoundManager.Instance.PlayBGMSound(0.5f);
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
        GameObject resultObject = GameObject.Find("ResultObjects");
        TMP_Text text = uiObject.transform.GetChild(0).GetComponent<TMP_Text>();
        text.text = string.Format("180초 동안 {0}개의 마카롱을 만들었어요.\n", score);
        if (score >= 3)
        {
            text.text += "이 가게는 성공적이네요!";
            resultObject.transform.GetChild(1).gameObject.SetActive(false);
        }
        else
        {
            text.text += "이 가게는 폐업 각이네요!";
            resultObject.transform.GetChild(0).gameObject.SetActive(false);
        }

        yield return new WaitForSeconds(10f);

        GoToTitle();
    }
}
