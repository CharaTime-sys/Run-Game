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

    [Header("地板移动速度")]
    public Material floor_material;
    public float speed;
    public float curve_speed;
    public float tile_speed;
    private float offset_y;

    public GameObject[] blocks;
    public GameObject[] downs;
    public GameObject[] turns;
    #region 未知参数
    [Header("障碍物生成的位置参数")]
    public Vector3[] block_pos;
    public Vector3[] down_pos;
    public Vector3[] turn_pos;
    public Vector3[] gesture_pos;
    #endregion
    [Header("每个障碍物的间隔距离（暂时为定值）")]
    public float distance;
    [Header("障碍物开始出现的距离")]
    public float start_pos = -4f;
    [Header("控制手柄")]
    public DynamicJoystick joystick;

    //手指按下和抬起的坐标
    public List<Vector2> finger_start_pos;
    [Header("手指判定半径")]
    [SerializeField] float finger_radious;
    public Vector2 test_vector;
    [Header("人物")]
    public Ninja ninja;

    public LineRenderer line;
    [Header("判定线")]
    [SerializeField] RectTransform check_line;
    [Header("判定线区间")]
    [SerializeField] float line_range;

    //是否正在按下
    bool is_pressing;
    //判断是否是长按后的跳跃，防止手指放开的时候跳跃
    public bool is_jump_after;
    //是否到达了ui点
    public bool is_reached;
    //按下状态
    public bool pressed;
    [SerializeField] Vector3 press_pos;//目前的手指位置
    [Header("悬空时间")]
    [SerializeField] float buff_time;
    [Header("手指到ui的最大距离")]
    [SerializeField]float buff_max_distance;
    [SerializeField] float buff_distance;
    //buff类型
    [SerializeField] Buff_Type buff_Type;

    #region UI相关
    [SerializeField] Text grade_text;
    [SerializeField] Image jump_buff_ui;
    [SerializeField] Image down_buff_ui;
    [SerializeField] Text hp_ui;
    [SerializeField] Text score_ui;
    #endregion
    Vector3 target_pos;//用于长按的判断

    //临时存储变量，后面会删掉
    public Buff_Type[] temp_buff;
    [SerializeField] Text buff_ui;
    public Text status_ui;

    #region 属性
    public Vector3 Press_pos { get => press_pos; }
    public float Finger_radious { get => finger_radious;}
    public Buff_Type Buff_Type { get => buff_Type; }
    #endregion

    private void Awake()
    {
        Instance = this;
        Set_buff_time_And_type(3f,Buff_Type.Jump);
    }

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
        ninja.score += score;
        score_ui.text = "分数：" + ninja.score.ToString();
    }

    /// <summary>
    /// 设置buff时间和类型
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
            //得到手指位置
            press_pos = Input.touches[0].position;
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

    private void FixedUpdate()
    {
        offset_y += tile_speed * Time.deltaTime;
        floor_material.mainTextureOffset = new Vector2(0, offset_y);
    }

    public void Test_Direction()
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
            Debug.Log("减分！！");
            return;
        }
        //Debug.Log("线段位置"+line_screen_pos);
        //Debug.Log("手指位置"+ Input.touches[0].position);
        float distance = Mathf.Sqrt(Vector2.SqrMagnitude(line_screen_pos - Input.touches[0].position))/100;
        Debug.Log(distance);
        if (distance<=Finger_radious)
        {
            Debug.Log("加分！！");
        }
        else
        {
            Debug.Log("减分！！");
        }
    }

    /// <summary>
    /// 判定线按下调用
    /// </summary>
    public void Test_Check_Line()
    {
        if (Mathf.Abs(finger_start_pos[0].y - check_line.position.y) <=line_range)
        {
            grade_text.text = "判定成功";
        }
        else
        {
            grade_text.text = "判定失败";
        }
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
                //测试用，后面可以删掉
                Set_buff_time_And_type(3f, temp_buff[Random.Range(0, 2)]);
                //设置长按跳跃状态
                is_jump_after = true;
                ninja.Set_Buff_Status();
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
        Debug.Log("ui：" + enable);
        //后面增加特效
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
