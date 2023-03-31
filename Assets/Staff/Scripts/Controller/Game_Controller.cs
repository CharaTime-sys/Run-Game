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

public enum Dir_Type
{
    None,
    Left,
    Right,
    Up,
    Down
}
public class Game_Controller : MonoBehaviour
{
    public static Game_Controller Instance;
    [Header("物体开始的延迟")]
    public float staff_delay;
    [Header("音乐开始的延迟")]
    public float music_delay;
    #region 速度变量
    //设置材质速度，后面可以删除
    public Material floor_material;
    [Header("速度变量------------------------------------------")]
    [Header("障碍物移动速度")]
    public float speed;
    [Header("曲线移动速度")]
    public float curve_speed;
    #endregion

    #region 物体
    [Header("控制手柄")]
    [SerializeField] DynamicJoystick joystick;
    [Header("人物")]
    public Ninja ninja;
    #endregion

    #region 判定线
    [Header("判定线")]
    [SerializeField] RectTransform check_line;
    [Header("判定线区间")]
    [SerializeField] float line_range;
    #endregion

    #region 状态变量
    [Header("手指状态（不用管）------------------------------------------")]
    //是否正在按下
    bool is_pressing;
    //判断是否是长按后的跳跃，防止手指放开的时候跳跃
    public bool is_jump_after;
    //是否到达了ui点
    public bool is_reached;
    //按下状态
    public bool pressed;
    #endregion

    #region 时间变量
    [Header("开始判定的时间")]
    public float start_time;
    [Header("完美时间")]
    public float prefect_time;
    [Header("失败时间")]
    public float loss_time;
    #endregion

    [Header("悬空时间")]
    [SerializeField] float buff_time;
    [Header("手指到ui的最大范围")]
    [SerializeField] float buff_max_distance;
    float buff_distance;
    //buff类型
    [SerializeField] Buff_Type buff_Type;

    #region 手势位置
    [Header("手势变量(不用管)------------------------------------------")]
    //手指按下和抬起的坐标
    public Vector2 finger_start_pos;
    public Vector2 test_vector;//手势方向
    #endregion

    #region UI相关
    [SerializeField] Text grade_text;
    [SerializeField] Image jump_buff_ui;
    [SerializeField] Image down_buff_ui;
    [SerializeField] Text perfermence_ui;
    [SerializeField] Text hp_ui;
    [Header("这个不用管，会自动装上去")]
    [SerializeField] Text buff_ui;
    [SerializeField] public Text score_ui;
    #endregion

    Vector3 target_pos;//用于长按的判断
    Vector3 press_pos;//目前的手指位置
    public Text status_ui;
    public float target_z;

    #region 属性
    public Vector3 Press_pos { get => press_pos; }
    public Buff_Type Buff_Type { get => buff_Type; }
    #endregion

    //临时变量
    float offset_y;

    [SerializeField] GameObject[] startups;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        AudioManager.instance.PlaySFX(4);
        Invoke(nameof(Set_Staff), staff_delay);
    }

    #region 音乐相关

    void Set_Staff()
    {
        foreach (var item in startups)
        {
            item.SetActive(true);
        }
        Invoke(nameof(Set_Audio), music_delay);
    }

    /// <summary>
    /// 播放音乐
    /// </summary>
    void Set_Audio()
    {
        AudioManager.instance.PlayBGM(AudioManager.instance.background_bgm);
    }

    #endregion

    #region 设置属性
    /// <summary>
    /// 设置血量
    /// </summary>
    /// <param name="hp"></param>
    public void Set_HP(int hp)
    {
        ninja.Hp += hp;
        hp_ui.text = "血量：" + ninja.Hp.ToString();
    }

    /// <summary>
    /// 设置分数
    /// </summary>
    /// <param name="score"></param>
    public void Set_Score(int score)
    {
        ninja.Score += score;
    }

    /// <summary>
    /// 设置buff时间和类型
    /// </summary>
    public void Set_buff_time_And_type(float target_time, Buff_Type buff_Type)
    {
        this.buff_time = target_time;
        this.buff_Type = buff_Type;
    }
    #endregion

    // Update is called once per frame
    void Update()
    {
        if (is_pressing)
        {
            //得到手指位置
            if (Input.touches.Length!=0)
            {
                press_pos = Input.touches[Input.touches.Length - 1].position;
            }
            Test_target_UI();
            //得到手指和buffui的距离
            buff_distance = Vector3.Distance(press_pos, target_pos);
        }
        Buff_Pressed();
    }

    /// <summary>
    /// 寻找目标ui的位置
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

    public Dir_Type Test_Direction()
    {
        //得到具体的角度，顺时针
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

        //根据角度调用不同方法
        if (angle < 45 || angle > 315)
        {
            ninja.Move_Left_And_Right(1);
            return Dir_Type.Right;
        }
        else if (angle >135 && angle < 225)
        {
            ninja.Move_Left_And_Right(-1);
            return Dir_Type.Left;
        }
        else if (angle > 45 && angle < 135)
        {
            ninja.Jump(true);
            return Dir_Type.Up;
        }
        else if (angle > 225 && angle < 315)
        {
            ninja.Down(true);
            return Dir_Type.Down;
        }
        return Dir_Type.Up;
    }

    /// <summary>
    /// 判断按下的状态
    /// </summary>
    public void Press_Checked(bool enable)
    {
        is_pressing = enable;
    }

    /// <summary>
    /// 按下跳跃时悬空
    /// </summary>
    void Buff_Pressed()
    {
        if (pressed)
        {
            //如果正在按下并且手指的距离在buffui的范围内则能腾空
            if (is_pressing && buff_distance <= buff_max_distance)
            {
                //当玩家具有跳跃或下蹲buff
                if (ninja.Is_buffing)
                {
                    //不让玩家完成后返回地面
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
                    //到达了标识点
                    is_reached = true;
                }
            }
            //当手指到达了之后，时间消失或者手指放开或者距离过大
            if ((is_reached && (buff_time < 0f || buff_distance >= buff_max_distance)) || !is_pressing)
            {
                //取消玩家buff状态
                ninja.Is_buffing = false;
                //落地
                switch (Buff_Type)
                {
                    //只有当玩家确实按到了按钮才会返回跳跃，不然就是普通的跳跃
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
                //取消到达ui状态
                is_reached = false;
                //设置长按跳跃状态
                is_jump_after = true;
            }
            //当玩家没有buff时，防止时间一直减少
            if (ninja.Is_buffing)
            {
                buff_time -= Time.deltaTime;
            }
        }
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
        if (Input.touchCount==0)
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

    /// <summary>
    /// 测试跳跃还是滑行，改变ui
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

    public void Set_preference_Text(string content)
    {
        perfermence_ui.text = content;
    }
}
