using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CustomerManager : MonoBehaviour
{
    #region Singleton
    private static CustomerManager instance;
    public static CustomerManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<CustomerManager>();
            }

            return instance;
        }
    }
    #endregion

    private readonly int MAXTOPPING = 5;
    public int[] correctOrder { get; private set; }
    public int[] inputOrder { get; private set; }

    public string orderText;

    private List<List<string>> toppings { get; set; }
    private int[,] lookUpTable = new int[,] { { 0, 1, 0, 0, 1 }, { 0, 0, 0, 0, 0 } };

    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
        Init();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            GenerateOrder();
            Debug.Log(orderText);
            Debug.Log(string.Format("{0}, {1}, {2}, {3}, {4}", GetTopping(correctOrder[0], 0), GetTopping(correctOrder[1], 1), GetTopping(correctOrder[2], 2), GetTopping(correctOrder[3], 3), GetTopping(correctOrder[4], 4)));
        }
    }

    private void Init()
    {
        if (correctOrder == null)
        {
            correctOrder = new int[] { -1, -1, -1, -1, -1 };
        }
        if (inputOrder == null)
        {
            inputOrder = new int[] { -1, -1, -1, -1, -1 };
        }
        if (toppings == null)
        {
            toppings = new List<List<string>>();
            string[] _topping = new string[] { "딸기", "초콜릿", "바나나", "인절미", "카라멜" };
            AddToppings(0, _topping);
            _topping = new string[] { "블루베리", "돼지바", "아몬드", "녹차", "민초" };
            AddToppings(1, _topping);
        }
    }

    private void AddToppings(int x, string[] value)
    {
        toppings.Add(new List<string>());
        for (int i = 0; i < MAXTOPPING; i++)
        {
            toppings[x].Add(value[i]);
        }
    }

    public string GetTopping(int x, int y)
    {
        if (x != -1)
        {
            if (x != -2)
            {
                return toppings[x][y];
            }
            else return "Empty";
        }
        else
        {
            return "Empty";
        }
    }

    /// <summary>
    /// deposits specified order into inputOrder array
    /// deposits -2 if collision happens on the same index
    /// </summary>
    /// <param name="index"></param>
    /// <param name="value"></param>
    public void InputOrder(int index, int value)
    {
        if (inputOrder[index] != -1 && inputOrder[index] != value)
        {
            inputOrder[index] = -2;
        }
        else
        {
            inputOrder[index] = value;
        }
    }

    public void ResetInputOrder()
    {
        inputOrder = new int[] { -1, -1, -1, -1, -1 };
    }

    public bool CheckOrder()
    {
        return Enumerable.SequenceEqual(correctOrder, inputOrder);
    }

    private void GenerateOrder()
    {
        correctOrder = new int[] { -1, -1, -1, -1, -1 };
        int toppingCount = Random.Range(1, MAXTOPPING);
        List<int> drawn = new List<int>();
        for (int i = 0; i < toppingCount; i++)
        {
            int toppingIndex = Random.Range(0, MAXTOPPING);
            if (drawn.Contains(toppingIndex))
            {
                i--;
            }
            else
            {
                drawn.Add(toppingIndex);
                correctOrder[toppingIndex] = Random.Range(0, toppings.SelectMany(list => list).Distinct().Count() / MAXTOPPING);
            }
        }
        drawn.Clear();
        EditOrderText();
    }

    private void EditOrderText()
    {
        orderText = new string("");
        int lastIndex = -1;
        for (int i = 0; i < MAXTOPPING; i++)
        {
            if (correctOrder[i] != -1)
            {
                lastIndex = i;
            }
        }
        int idx = 0;
        foreach (int item in correctOrder)
        {
            if (item != -1)
            {
                orderText += toppings[item][idx];
                if (idx != lastIndex)
                {
                    if (lookUpTable[item, idx] == 1)
                    {
                        orderText += "이랑 ";
                    }
                    else
                    {
                        orderText += "랑 ";
                    }
                }
                else
                {
                    if (lookUpTable[item, idx] == 1)
                    {
                        orderText += "이 ";
                    }
                    else
                    {
                        orderText += "가 ";
                    }
                }
            }
            idx++;
        }
        orderText += "들어간 마카롱 주세요.";
    }
}
