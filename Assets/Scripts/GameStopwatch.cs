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
    private GameObject background;
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
        background = transform.GetChild(0).gameObject;
        sun = background.transform.Find("Sun").gameObject;
        moon = background.transform.Find("Moon").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        float sunAngle = ((float)gameManager.gameStopwatch.Elapsed.TotalSeconds * 150f / gameManager.gameTimeLimit + 45f) * Mathf.PI / 180f;
        sun.transform.localPosition = 100 * new Vector3(-Mathf.Cos(sunAngle), Mathf.Sin(sunAngle), 0f);
        moon.transform.localPosition = -sun.transform.localPosition;
        sun.GetComponent<Image>().fillAmount = sun.transform.localPosition.y / 100f + 0.5f;
        moon.GetComponent<Image>().fillAmount = moon.transform.localPosition.y / 100f + 0.5f;
        Image image = background.GetComponent<Image>();

        Debug.Log(sunAngle);
        Color sky = new Color(0.54f, 0.72f, 1f);
        Color dark = new Color(0.2f, 0.2f, 0.2f);
        if (sunAngle < Mathf.PI - 0.75f) image.color = sky;
        else if (sunAngle >= Mathf.PI - 0.75f && sunAngle <= Mathf.PI + 0.25f) image.color = Color.Lerp(sky, dark, sunAngle - Mathf.PI + 0.75f);
        else image.color = dark;
    }
}
