using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    public static UI_Manager Instance;
    #region UI相关
    public Image jump_buff_ui;
    public Image down_buff_ui;
    [SerializeField] Slider hp_slider;
    [SerializeField] GameObject status_ui;
    [SerializeField] GameObject game_ui;
    [SerializeField] GameObject pause_panel;
    [SerializeField] Slider slider_ui;
    public Image countdown_text;
    int countdown = 3;
    [SerializeField] Text tip_ui;
    [Header("这个不用管，会自动装上去")]
    [SerializeField] Text buff_ui;
    [SerializeField] public Text score_ui;
    #endregion

    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// 设置跳跃ui
    /// </summary>
    /// <param name="enable"></param>
    public void Set_Jump_UI(bool enable)
    {
        //后面增加特效
        if (enable)
        {
            jump_buff_ui.GetComponent<buff_ui>().Reset_uis();
        }
        jump_buff_ui.gameObject.SetActive(enable);
        if (Input.touchCount == 0)
        {
            return;
        }
        //设置位置
        jump_buff_ui.transform.position = new Vector3(Input.touches[0].position.x, jump_buff_ui.transform.position.y, 0);
    }

    /// <summary>
    /// 设置滑行ui
    /// </summary>
    /// <param name="enable"></param>
    public void Set_Down_UI(bool enable)
    {
        //后面增加特效
        if (enable)
        {
            down_buff_ui.GetComponent<buff_ui>().Reset_uis();
        }
        down_buff_ui.gameObject.SetActive(enable);
        if (Input.touchCount == 0)
        {
            return;
        }
        //设置位置
        down_buff_ui.transform.position = new Vector3(Input.touches[0].position.x, down_buff_ui.transform.position.y, 0);
    }

    public void Set_Hp_UI()
    {
        hp_slider.value = Game_Controller.Instance.ninja.Hp;
    }

    public void Set_Score_UI()
    {
        score_ui.text =((int)Game_Controller.Instance.ninja.Score).ToString();
    }

    public void Set_Slider(float cur,float full)
    {
        if (slider_ui==null)
        {
            return;
        }
        slider_ui.value = cur / full;
    }

    public void Set_Status_UI(string content)
    {
        status_ui.gameObject.SetActive(true);
        status_ui.GetComponent<Animator>().Play("Status");
    }

    public void Set_Tip(bool enable,string content)
    {
        tip_ui.text = content;
        tip_ui.gameObject.SetActive(enable);
    }

    public void Set_Count_Down()
    {
        countdown_text.gameObject.SetActive(true);
        countdown_text.transform.GetChild(0).GetComponent<Text>().text = countdown.ToString();
        countdown--;
        if (countdown == 0)
        {
            Invoke(nameof(Set_CountDown_UI), 1f);
            return;
        }
        Invoke(nameof(Set_Count_Down), 1f);
    }

    public void Set_CountDown_UI()
    {
        countdown = 3;
        countdown_text.gameObject.SetActive(false);
        Game_Controller.Instance.Pause_Start();
        Game_Controller.Instance.Set_Pause_Bool();
    }

    public void Set_Main_UI()
    {
        game_ui.SetActive(false);
    }

    public void Set_Pause_Panel(bool enable)
    {
        pause_panel.SetActive(enable);
    }
}
