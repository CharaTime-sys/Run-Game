using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    public static UI_Manager Instance;
    #region UI���
    [SerializeField] Text grade_text;
    public Image jump_buff_ui;
    public Image down_buff_ui;
    [SerializeField] Text hp_ui;
    [SerializeField] Text status_ui;
    [SerializeField] Text tip_ui;
    [Header("������ùܣ����Զ�װ��ȥ")]
    [SerializeField] Text buff_ui;
    [SerializeField] public Text score_ui;
    #endregion

    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// ������Ծui
    /// </summary>
    /// <param name="enable"></param>
    public void Set_Jump_UI(bool enable)
    {
        //����������Ч
        if (enable)
        {
            jump_buff_ui.GetComponent<buff_ui>().Reset_uis();
        }
        jump_buff_ui.gameObject.SetActive(enable);
        if (Input.touchCount == 0)
        {
            return;
        }
        //����λ��
        jump_buff_ui.transform.position = new Vector3(Input.touches[0].position.x, jump_buff_ui.transform.position.y, 0);
    }

    /// <summary>
    /// ���û���ui
    /// </summary>
    /// <param name="enable"></param>
    public void Set_Down_UI(bool enable)
    {
        //����������Ч
        if (enable)
        {
            down_buff_ui.GetComponent<buff_ui>().Reset_uis();
        }
        down_buff_ui.gameObject.SetActive(enable);
        if (Input.touchCount == 0)
        {
            return;
        }
        //����λ��
        down_buff_ui.transform.position = new Vector3(Input.touches[0].position.x, down_buff_ui.transform.position.y, 0);
    }

    public void Set_Hp_UI()
    {
        hp_ui.text = "Ѫ����" + Game_Controller.Instance.ninja.Hp.ToString();
    }

    public void Set_Score_UI()
    {
        score_ui.text = "������" + Game_Controller.Instance.ninja.Score.ToString();
    }

    public void Set_Status_UI(string content)
    {
        status_ui.text = content;
        status_ui.GetComponent<Animator>().Play("Status");
    }

    public void Set_Tip(bool enable,string content)
    {
        tip_ui.text = content;
        tip_ui.gameObject.SetActive(enable);
    }
}
