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
    [Header("���ֿ�ʼ���ӳ�")]
    public float music_delay;
    #region �ٶȱ���
    //���ò����ٶȣ��������ɾ��
    public Material floor_material;
    [Header("�ٶȱ���------------------------------------------")]
    [Header("�ϰ����ƶ��ٶ�")]
    public float speed;
    [Header("�����ƶ��ٶ�")]
    public float curve_speed;
    [Header("�ذ��ƶ��ٶ�")]
    public float tile_speed;
    #endregion

    #region �ϰ���
    [Header("�ϰ������------------------------------------------")]
    [Header("�ϰ���Ԥ���壨��Ծ���»���ת��,buff��")]
    public GameObject[] blocks;
    public GameObject[] downs;
    public GameObject[] turns;
    public GameObject[] buffs;
    #endregion

    #region λ�ò���
    [Header("�ϰ���λ�ñ���------------------------------------------")]
    [Header("�ϰ������ɵ�λ�ò���(��Ծ�ϰ����»��ϰ���ת���ϰ��������ϰ�)")]
    public Vector3[] block_pos;
    public Vector3[] down_pos;
    public Vector3[] turn_pos;
    public Vector3[] gesture_pos;
    public Vector3[] buff_pos;
    #endregion

    #region ����
    [Header("�����ֱ�")]
    [SerializeField] DynamicJoystick joystick;
    [Header("����")]
    public Ninja ninja;
    #endregion

    #region �ж���
    [Header("�ж���")]
    [SerializeField] RectTransform check_line;
    [Header("�ж�������")]
    [SerializeField] float line_range;
    #endregion

    #region ״̬����
    [Header("��ָ״̬�����ùܣ�------------------------------------------")]
    //�Ƿ����ڰ���
    bool is_pressing;
    //�ж��Ƿ��ǳ��������Ծ����ֹ��ָ�ſ���ʱ����Ծ
    public bool is_jump_after;
    //�Ƿ񵽴���ui��
    public bool is_reached;
    //����״̬
    public bool pressed;
    #endregion

    [Header("����ʱ��")]
    [SerializeField] float buff_time;
    [Header("��ָ��ui�����Χ")]
    [SerializeField] float buff_max_distance;
    float buff_distance;
    //buff����
    [SerializeField] Buff_Type buff_Type;

    #region ����λ��
    [Header("���Ʊ���(���ù�)------------------------------------------")]
    //��ָ���º�̧�������
    public List<Vector2> finger_start_pos;
    public Vector2 test_vector;//���Ʒ���
    #endregion

    #region UI���
    [SerializeField] Text grade_text;
    [SerializeField] Image jump_buff_ui;
    [SerializeField] Image down_buff_ui;
    [SerializeField] Text hp_ui;
    [Header("������ùܣ����Զ�װ��ȥ")]
    [SerializeField] Text buff_ui;
    [SerializeField] public Text score_ui;
    #endregion

    Vector3 target_pos;//���ڳ������ж�
    Vector3 press_pos;//Ŀǰ����ָλ��
    public Text status_ui;

    #region ����
    public Vector3 Press_pos { get => press_pos; }
    public Buff_Type Buff_Type { get => buff_Type; }
    #endregion

    //��ʱ����
    float offset_y;

    private void Awake()
    {
        Instance = this;
        Set_buff_time_And_type(3f,Buff_Type.Jump);
    }

    /// <summary>
    /// ����Ѫ��
    /// </summary>
    /// <param name="hp"></param>
    public void Set_HP(int hp)
    {
        ninja.Hp += hp;
        hp_ui.text = "Ѫ����" + ninja.Hp.ToString();
    }

    /// <summary>
    /// ���÷���
    /// </summary>
    /// <param name="score"></param>
    public void Set_Score(int score)
    {
        ninja.Score += score;
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
        switch (Buff_Type)
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
        //����ƫ���������ڲ��ʣ��������ɾ��
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
        if (pressed)
        {
            //������ڰ��²�����ָ�ľ�����buffui�ķ�Χ�������ڿ�
            if (is_pressing && buff_distance <= buff_max_distance)
            {
                //����Ҿ�����Ծ���¶�buff
                if (ninja.Is_buffing)
                {
                    //���������ɺ󷵻ص���
                    switch (Buff_Type)
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
                    //�����˱�ʶ��
                    is_reached = true;
                }
            }
            //����ָ������֮��ʱ����ʧ������ָ�ſ����߾������
            if ((is_reached && (buff_time < 0f || buff_distance >= buff_max_distance)) || !is_pressing)
            {
                //ȡ�����buff״̬
                Debug.Log("ȡ��buff״̬");
                ninja.Is_buffing = false;
                //���
                switch (Buff_Type)
                {
                    //ֻ�е����ȷʵ�����˰�ť�Ż᷵����Ծ����Ȼ������ͨ����Ծ
                    case Buff_Type.Jump:
                        if (is_reached)
                        {
                            ninja.Resume_Jump();
                        }
                        Set_Jump_UI(false);
                        break;
                    case Buff_Type.Down:
                        if (is_reached)
                        {
                            ninja.Resume_Down();
                        }
                        Set_Down_UI(false);
                        break;
                }
                //ȡ������ui״̬
                is_reached = false;
                //���ó�����Ծ״̬
                is_jump_after = true;
            }
            //�����û��buffʱ����ֹʱ��һֱ����
            if (ninja.Is_buffing)
            {
                buff_time -= Time.deltaTime;
            }
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
        if (Input.touchCount==0)
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
        down_buff_ui.gameObject.SetActive(enable);
        if (Input.touchCount == 0)
        {
            return;
        }
        //����λ��
        down_buff_ui.transform.position = new Vector3(Input.touches[0].position.x, down_buff_ui.transform.position.y, 0);
    }

    /// <summary>
    /// ������Ծ���ǻ��У��ı�ui
    /// </summary>
    public void Check_Down_And_Jump()
    {
        if (!ninja.Is_buffing)
        {
            return;
        }
        switch (Buff_Type)
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
