using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodMaker : MonoBehaviour
{
    private int xlimit = 17;
    private int ylimit = 12;
    private int xoffset = 10;

    public GameObject foodPrefab;
    public Sprite[] foodSprite;

    private Transform foodHolder;

    private static FoodMaker _instance;
    public static FoodMaker Instance()
    {
        return _instance;
    }

    void Awake()
    {
        _instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        foodHolder = GameObject.Find("FoodHolder").transform;
        MakeFood();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MakeFood()
    {
        int index = Random.Range(0, foodSprite.Length);
        GameObject food = Instantiate(foodPrefab);
        food.GetComponent<Image>().sprite = foodSprite[index];
        food.transform.SetParent(foodHolder, false);  
        int x = Random.Range(-xlimit + xoffset, xlimit);
        int y = Random.Range(-ylimit, ylimit);
        food.transform.localPosition = new Vector3(x * 30, y * 30, 0);
    }
}
