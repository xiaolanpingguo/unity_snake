using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartUIController : MonoBehaviour
{
    public Toggle noBorder;

    private static StartUIController _instance;

    public static StartUIController Instance()
    {
        return _instance;
    }

    void Awake()
    {
        _instance = this;
    }

    public void StartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    public bool IsNoBoard()
    {
        return noBorder.isOn;
    }
}
