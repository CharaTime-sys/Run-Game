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

    public string[] names;
    private void Start()
    {
        first_button.onClick.AddListener(delegate { Level_Controller.Instance.Load_Level(names[0]); });
        second_button.onClick.AddListener(delegate { Level_Controller.Instance.Load_Level(names[1]); });
        third_button.onClick.AddListener(delegate { Level_Controller.Instance.Load_Level(names[2]); });
    }
}
