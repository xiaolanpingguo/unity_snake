using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq; // List.Last()

public class SnakeHead : MonoBehaviour
{
    // 多久调用一次
    public float velocity = 0.35f;
    public int step;
    private int x;
    private int y;
    private Vector3 headPos;

    // 蛇身
    public List<Transform> bodyList = new List<Transform>();

    // 蛇身预制体
    public GameObject bodyPrefab;
    public Sprite[] bodySprites = new Sprite[2];

    // canvas
    private Transform canvas;

    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.Find("Canvas").transform;

        // 重复调用
        InvokeRepeating("Move", 0, velocity);
        x = step;
        y = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CancelInvoke();
            InvokeRepeating("Move", 0, velocity - 0.2f);
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            CancelInvoke();
            InvokeRepeating("Move", 0, velocity);
        }
        if (Input.GetKey(KeyCode.W) && y != -step)
        {
            x = 0;
            y = step;
            //gameObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        if (Input.GetKey(KeyCode.S) && y != step)
        {
            x = 0;
            y = -step;
        }
        if (Input.GetKey(KeyCode.A) && x != step)
        {
            x = -step;
            y = 0;
        }
        if (Input.GetKey(KeyCode.D) && x != -step)
        {
            x = step;
            y = 0;
        }
    }

    void Move()
    {
        headPos = gameObject.transform.localPosition;
        gameObject.transform.localPosition = new Vector3(headPos.x + x, headPos.y + y, headPos.z);
        if (bodyList.Count > 0)
        {
            bodyList.Last().localPosition = headPos;
            bodyList.Insert(0, bodyList.Last());
            bodyList.RemoveAt(bodyList.Count - 1);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 食物
        if (collision.tag == "Food")
        {
            Destroy(collision.gameObject);
            Grow();
            FoodMaker.Instance().MakeFood();
        }
        // 身体
        else if (collision.tag == "Body")
        {
            Debug.Log("Body Die....");
        }
        // 边界
        else
        {
            switch (collision.gameObject.name)
            {
                case "Up":
                    transform.localPosition = new Vector3(transform.localPosition.x, -transform.localPosition.y + 30, transform.localPosition.z);
                    Debug.Log("Up Die....");
                    break;
                case "Down":
                    transform.localPosition = new Vector3(transform.localPosition.x, -transform.localPosition.y - 30, transform.localPosition.z);
                    Debug.Log("Down Die....");
                    break;
                case "Left":
                    transform.localPosition = new Vector3(-transform.localPosition.x + 300 - 30, transform.localPosition.y, transform.localPosition.z);
                    Debug.Log("Left Die....");
                    break;
                case "Right":
                    transform.localPosition = new Vector3(-transform.localPosition.x + 300 + 30, transform.localPosition.y, transform.localPosition.z);
                    Debug.Log("Right Die....");
                    break;
                default:
                    break;
            }
        }
    }

    private void Grow()
    {
        int index = (bodyList.Count % 2 == 0) ? 0 : 1; 
        GameObject body = Instantiate(bodyPrefab, new Vector3(2000, 2000, 0), Quaternion.identity);
        body.GetComponent<Image>().sprite = bodySprites[index];

        // false:不自动保存它的世界坐标
        body.transform.SetParent(canvas, false);

        bodyList.Add(body.transform);
    }
}
