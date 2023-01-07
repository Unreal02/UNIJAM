using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStopwatch : MonoBehaviour
{
    static private GameStopwatch instance;
    static public GameStopwatch Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameStopwatch>();
            }

            return instance;
        }
    }

    private GameManager gameManager;
    private GameObject sun;
    private GameObject moon;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
        }
        if (Instance)
        {
            DontDestroyOnLoad(gameObject);
        }

        gameManager = GameManager.Instance;
        sun = transform.GetChild(0).Find("Sun").gameObject;
        moon = transform.GetChild(0).Find("Moon").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        float sunAngle = ((float)gameManager.gameStopwatch.Elapsed.TotalSeconds * 150f / gameManager.gameTimeLimit + 45f) * Mathf.PI / 180f;
        sun.transform.localPosition = 100 * new Vector3(-Mathf.Cos(sunAngle), Mathf.Sin(sunAngle), 0f);
        moon.transform.localPosition = -sun.transform.localPosition;
        sun.GetComponent<Image>().fillAmount = sun.transform.localPosition.y / 100f + 0.5f;
        moon.GetComponent<Image>().fillAmount = moon.transform.localPosition.y / 100f + 0.5f;
    }
}
