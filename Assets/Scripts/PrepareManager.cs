using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PreparePhase
{
    Init,
    Meringue,
    Oven
}

public class PrepareManager : MonoBehaviour
{
    static private PrepareManager instance;
    static public PrepareManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PrepareManager>();
            }

            return instance;
        }
    }

    public PreparePhase phase;
    private int shakeCount = 0;
    private Stick stick;
    private GameObject meringueProgress;
    private Image progressBar;

    // Start is called before the first frame update
    void Start()
    {
        phase = PreparePhase.Init;
        shakeCount = 0;
        stick = FindObjectOfType<Stick>();
        meringueProgress = GameObject.Find("MeringueProgress");
        progressBar = GameObject.Find("ProgressBar").GetComponent<Image>();
        meringueProgress.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // 스페이스바 누르면 섞기
        if (phase == PreparePhase.Meringue)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Shake();
            }
        }
    }

    public void GoToMeringue()
    {
        phase = PreparePhase.Meringue;
        meringueProgress.SetActive(true);
        progressBar.fillAmount = 0;
    }

    public void GoToOven()
    {
        phase = PreparePhase.Oven;
        Debug.Log(CheckShake());
    }

    public void Shake()
    {
        shakeCount++;
        Debug.Log(string.Format("Shake {0}", shakeCount));
        stick.Shake(shakeCount);
        // progress bar
        progressBar.fillAmount = shakeCount / 120f;
    }

    public bool CheckShake()
    {
        if (shakeCount < 90 || shakeCount > 110)
        {
            return false;

        }
        else
        {
            return true;
        }
    }
}
