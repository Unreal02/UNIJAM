using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public PreparePhase phase = PreparePhase.Init;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GoToMeringue()
    {
        phase = PreparePhase.Meringue;
    }
}
