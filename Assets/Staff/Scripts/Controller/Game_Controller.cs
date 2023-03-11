using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public enum Buff_Type
{
    Jump,
    Down
}
public class Game_Controller : MonoBehaviour
{
    public static Game_Controller Instance;

    [Header("�ذ��ƶ��ٶ�")]
    public Material floor_material;
    public float speed;
    public float tile_speed;
    private float offset_y;

    public GameObject[] blocks;
    public GameObject[] downs;

    [Header("�ϰ������ɵ�λ�ò���")]
    public Vector3 block_pos;
    [Header("ÿ���ϰ���ļ�����루��ʱΪ��ֵ��")]
    public float distance;
    [Header("�ϰ��￪ʼ���ֵľ���")]
    public float start_pos = -4f;
    [Header("�����ֱ�")]
    public DynamicJoystick joystick;

    //��ָ���º�̧�������
    public List<Vector2> finger_start_pos;
    [Header("��ָ�ж��뾶")]
    [SerializeField] float finger_radious;
    public Vector2 test_vector;
    [Header("����")]
    public Ninja ninja;

    public LineRenderer line;
    [Header("�ж���")]
    [SerializeField] RectTransform check_line;
    [Header("�ж�������")]
    [SerializeField] float line_range;

    //�Ƿ����ڰ���
    bool is_pressing;
    //�ж��Ƿ��ǳ��������Ծ����ֹ��ָ�ſ���ʱ����Ծ
    public bool is_jump_after;
    //�Ƿ񵽴���ui��
    [SerializeField] bool is_reached;
    [SerializeField] Vector3 press_pos;//Ŀǰ����ָλ��
    [Header("����ʱ��")]
    [SerializeField] float buff_time;
    [Header("��ָ��ui��������")]
    [SerializeField]float buff_max_distance;
    [SerializeField] float buff_distance;
    //buff����
    [SerializeField] Buff_Type buff_Type;

    //UI���
    [SerializeField] Text grade_text;
    [SerializeField] Image jump_buff_ui;
    [SerializeField] Image down_buff_ui;
    Vector3 target_pos;

    //��ʱ�洢�����������ɾ��
    public Buff_Type[] temp_buff;
    [SerializeField] Text buff_ui;

    public Vector3 Press_pos { get => press_pos; }
    public float Finger_radious { get => finger_radious;}

    private void Awake()
    {
        Instance = this;
        Set_buff_time_And_type(3f,Buff_Type.Jump);
    }

    /// <summary>
    /// ����buffʱ�������
    /// </summary>
    public void Set_buff_time_And_type(float target_time, Buff_Type buff_Type)
    {
        this.buff_time = target_time;
        this.buff_Type = buff_Type;
    }

    // Update is called once per frame
    void Update()
    {
        if (is_pressing)
        {
            //�õ���ָλ��
            press_pos = Input.touches[0].position;
            Test_target_UI();
            //�õ���ָ��buffui�ľ���
            buff_distance = Vector3.Distance(press_pos, target_pos);
        }
        Buff_Pressed();
    }

    /// <summary>
    /// Ѱ��Ŀ��ui��λ��
    /// </summary>
    void Test_target_UI()
    {
        switch (buff_Type)
        {
            case Buff_Type.Jump:
                target_pos = jump_buff_ui.transform.position;
                break;
            case Buff_Type.Down:
                target_pos = down_buff_ui.transform.position;
                break;
            default:
                break;
        }
    }

    private void FixedUpdate()
    {
        offset_y += tile_speed * Time.deltaTime;
        floor_material.mainTextureOffset = new Vector2(0, offset_y);
    }

    public void Test_Direction()
    {
        //�õ�����ĽǶȣ�˳ʱ��
        float angle = Mathf.Atan(test_vector.y/test_vector.x) * Mathf.Rad2Deg;
        if (test_vector.x<0f && test_vector.y>0f)
        {
            angle = 90f+Mathf.Atan(-test_vector.x / test_vector.y) * Mathf.Rad2Deg;
        }
        else if (test_vector.x < 0f && test_vector.y < 0f)
        {
            angle = 180f+Mathf.Atan(test_vector.y / test_vector.x) * Mathf.Rad2Deg;
        }
        else if (test_vector.x > 0f && test_vector.y < 0f)
        {
            angle = 270f + Mathf.Atan(-test_vector.x / test_vector.y) * Mathf.Rad2Deg;
        }

        //���ݽǶȵ��ò�ͬ����
        if (angle < 45 || angle > 315)
        {
            ninja.Move_Left_And_Right(1);
        }
        else if (angle >135 && angle < 225)
        {
            ninja.Move_Left_And_Right(-1);
        }
        else if (angle > 45 && angle < 135)
        {
            ninja.Jump(true);
        }
        else if (angle > 225 && angle < 315)
        {
            ninja.Down(true);
        }
    }

    public void Test_Finger()
    {
        if (line.positionCount == 0)
        {
            return;
        }
        Vector2 line_screen_pos = Camera.main.WorldToScreenPoint(line.GetPosition(line.positionCount - 1));
        if (finger_start_pos.Count == 0)
        {
            Debug.Log("���֣���");
            return;
        }
        //Debug.Log("�߶�λ��"+line_screen_pos);
        //Debug.Log("��ָλ��"+ Input.touches[0].position);
        float distance = Mathf.Sqrt(Vector2.SqrMagnitude(line_screen_pos - Input.touches[0].position))/100;
        Debug.Log(distance);
        if (distance<=Finger_radious)
        {
            Debug.Log("�ӷ֣���");
        }
        else
        {
            Debug.Log("���֣���");
        }
    }

    /// <summary>
    /// �ж��߰��µ���
    /// </summary>
    public void Test_Check_Line()
    {
        if (Mathf.Abs(finger_start_pos[0].y - check_line.position.y) <=line_range)
        {
            grade_text.text = "�ж��ɹ�";
        }
        else
        {
            grade_text.text = "�ж�ʧ��";
        }
    }

    /// <summary>
    /// �жϰ��µ�״̬
    /// </summary>
    public void Press_Checked(bool enable)
    {
        is_pressing = enable;
    }

    /// <summary>
    /// ������Ծʱ����
    /// </summary>
    void Buff_Pressed()
    {
        //������ڰ��²�����ָ�ľ�����buffui�ķ�Χ�������ڿ�
        if (is_pressing && buff_distance <= buff_max_distance)
        {
            //����Ҿ�����Ծ���¶�buff
            if (ninja.Is_buffing)
            {
                //Debug.Log("buff_distance:" + buff_distance);
                switch (buff_Type)
                {
                    case Buff_Type.Jump:
                        ninja.Jump(false);
                        break;
                    case Buff_Type.Down:
                        ninja.Down(false);
                        break;
                    default:
                        break;
                }
                is_reached = true;
            }
        }
        //����ָ������֮��ʱ����ʧ������ָ�ſ����߾������
        if (is_reached && (buff_time < 0f || buff_distance >= buff_max_distance) || !is_pressing)
        {
            //ȡ�����buff״̬
            ninja.Is_buffing = false;
            //���
            switch (buff_Type)
            {
                case Buff_Type.Jump:
                    ninja.Resume_Jump();
                    Set_Jump_UI(false);
                    break;
                case Buff_Type.Down:
                    ninja.Resume_Down();
                    Set_Down_UI(false);
                    break;
            }
            //ȡ������ui״̬
            is_reached = false;
            //�����ã��������ɾ��
            Set_buff_time_And_type(3f,temp_buff[Random.Range(0,2)]);
            //���ó�����Ծ״̬
            is_jump_after = true;
            ninja.Set_Buff_Status();
        }
        //�����û��buffʱ����ֹʱ��һֱ����
        if (ninja.Is_buffing)
        {
            buff_time -= Time.deltaTime;
        }
    }

    /// <summary>
    /// ������Ծui
    /// </summary>
    /// <param name="enable"></param>
    public void Set_Jump_UI(bool enable)
    {
        //����������Ч
        jump_buff_ui.gameObject.SetActive(enable);
    }

    /// <summary>
    /// ���û���ui
    /// </summary>
    /// <param name="enable"></param>
    public void Set_Down_UI(bool enable)
    {
        //����������Ч
        down_buff_ui.gameObject.SetActive(enable);
    }

    /// <summary>
    /// ������Ծ���ǻ��У��ı�ui
    /// </summary>
    public void Check_Down_And_Jump()
    {
        switch (buff_Type)
        {
            case Buff_Type.Jump:
                Set_Jump_UI(true);
                break;
            case Buff_Type.Down:
                Set_Down_UI(true);
                break;
            default:
                break;
        }
    }
}
