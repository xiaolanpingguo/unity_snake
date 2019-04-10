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

    private bool isDie = false; // 是否死亡

   // public AudioClip eatClip;
   // public AudioClip dieClip;

    public GameObject dieEffect;

    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.Find("Canvas").transform;

        // 重复调用
        InvokeRepeating("Move", 0, velocity);
        x = -step;
        y = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !MainUIController.Instance().isPause && !isDie)
        {
            CancelInvoke();
            InvokeRepeating("Move", 0, velocity - 0.2f);
        }
        if (Input.GetKeyUp(KeyCode.Space) && !MainUIController.Instance().isPause && !isDie)
        {
            CancelInvoke();
            InvokeRepeating("Move", 0, velocity);
        }
        if (Input.GetKey(KeyCode.W) && y != -step && !MainUIController.Instance().isPause && !isDie)
        {
            x = 0;
            y = step;
            //gameObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        if (Input.GetKey(KeyCode.S) && y != step && !MainUIController.Instance().isPause && !isDie)
        {
            x = 0;
            y = -step;
        }
        if (Input.GetKey(KeyCode.A) && x != step && !MainUIController.Instance().isPause && !isDie)
        {
            x = -step;
            y = 0;
        }
        if (Input.GetKey(KeyCode.D) && x != -step && !MainUIController.Instance().isPause && !isDie)
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

    // 死亡
    void Die()
    {
        // 播放声音
        //AudioSource.PlayClipAtPoint(dieClip, Vector3.zero);
        CancelInvoke();
        isDie = true;
        
        // 爆炸特效
        Instantiate(dieEffect);
        StartCoroutine(GameOver(1.5f));
    }

    // 协程
    IEnumerator GameOver(float t)
    {
        yield return new WaitForSeconds(t);
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 食物
        if (collision.tag == "Food")
        {
            Destroy(collision.gameObject);
            MainUIController.Instance().UpdateUI();
            Grow();
            FoodMaker.Instance().MakeFood();
        }
        // 奖励
        else if (collision.tag == "Reward")
        {
            Destroy(collision.gameObject);
            MainUIController.Instance().UpdateUI(Random.Range(5, 15) * 10);
            Grow();
        }
        // 身体
        else if (collision.tag == "Body")
        {
            Debug.Log("body...");
            Die();
        }
        // 边界
        else
        {
            if (StartUIController.Instance().IsNoBoard())
            {
                switch (collision.gameObject.name)
                {
                    case "Up":
                        transform.localPosition = new Vector3(transform.localPosition.x, -transform.localPosition.y + 30, transform.localPosition.z);
                        break;
                    case "Down":
                        transform.localPosition = new Vector3(transform.localPosition.x, -transform.localPosition.y - 30, transform.localPosition.z);
                        break;
                    case "Left":
                        transform.localPosition = new Vector3(-transform.localPosition.x + 300 - 30, transform.localPosition.y, transform.localPosition.z);
                        break;
                    case "Right":
                        transform.localPosition = new Vector3(-transform.localPosition.x + 300 + 30, transform.localPosition.y, transform.localPosition.z);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                Die();
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
