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
    public int[] Order { get; private set; }

    private List<List<string>> Toppings { get; set; }

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
            Debug.Log(string.Format("{0}, {1}, {2}, {3}, {4}", GetTopping(0, Order[0]), GetTopping(1, Order[1]), GetTopping(2, Order[2]), GetTopping(3, Order[3]), GetTopping(4, Order[4])));
        }
    }

    private void Init()
    {
        if (Order == null)
        {
            Order = new int[] { -1, -1, -1, -1, -1 };
        }

        if (Toppings == null)
        {
            Toppings = new List<List<string>>();
            string[] _topping = new string[] { "µþ±â", "ÃÊÄÝ¸´", "¹Ù³ª³ª", "ÀÎÀý¹Ì", "Ä«¶ó¸á" };
            AddToppings(0, _topping);
            _topping = new string[] { "ºí·çº£¸®", "µÅÁö¹Ù", "¾Æ¸óµå", "³ìÂ÷", "¹ÎÃÊ" };
            AddToppings(1, _topping);
        }
    }

    private void AddToppings(int x, string[] value)
    {
        Toppings.Add(new List<string>());
        for (int i = 0; i < MAXTOPPING; i++)
        {
            Toppings[x].Add(value[i]);
        }
    }

    private string GetTopping(int x, int y)
    {
        if (y != -1)
        {
            return Toppings[y][x];
        }
        else
        {
            return "Empty";
        }
    }

    private void GenerateOrder()
    {
        Order = new int[] { -1, -1, -1, -1, -1 };
        int toppingCount = Random.Range(1, MAXTOPPING);
        List<int> Drawn = new List<int>();
        for (int i = 0; i < toppingCount; i++)
        {
            int toppingIndex = Random.Range(0, MAXTOPPING);
            if (Drawn.Contains(toppingIndex))
            {
                i--;
            }
            else
            {
                Drawn.Add(toppingIndex);
                Order[toppingIndex] = Random.Range(0, Toppings.SelectMany(list => list).Distinct().Count() / MAXTOPPING);
            }
        }
        Drawn.Clear();
    }

}
