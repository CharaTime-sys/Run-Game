using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Controller : MonoBehaviour
{
    public Button first_button;
    public Button second_button;
    public Button third_button;

    public string name;
    public string[] names;
    [Header("是否是选择难度")]
    public bool if_difficult;
    public bool if_start;
    private void Start()
    {
        if (!if_difficult)
        {
            first_button.onClick.AddListener(delegate { Level_Controller.Instance.Load_Level(name, 1); });
            second_button.onClick.AddListener(delegate { Level_Controller.Instance.Load_Level(name, 2); });
        }
        else if (if_start)
        {
            first_button.onClick.AddListener(delegate { Level_Controller.Instance.Load_Level("Choose"); });
            second_button.onClick.AddListener(delegate { Level_Controller.Instance.Load_Level("Setting"); });
            third_button.onClick.AddListener(delegate { Level_Controller.Instance.Exit_Game(); });
        }
        else
        {
            first_button.onClick.AddListener(delegate { Level_Controller.Instance.Load_Level(1); });
            second_button.onClick.AddListener(delegate { Level_Controller.Instance.Load_Level(2); });
        }
    }
}
